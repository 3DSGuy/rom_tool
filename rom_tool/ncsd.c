#include "lib.h"
#include "ncsd.h"

int NCSDProcess(CCI_CONTEXT *ctx)
{
	ctx->ncsd_struct = malloc(sizeof(NCSD_STRUCT));
	if(GetNCSDData(ctx) != 0)
		return Fail;
		
	if(!ctx->ncsd_struct->CCI_FILE_STATUS){
		printf("[!] CCI is malformed\n");
		return Fail;
	}
		
	if(ctx->flags[extract] == True){
		if(ExtractCCIPartitions(ctx) != 0){
			return Fail;
		}
	}	
	if(ctx->flags[trim] == True){
		if(TrimCCI(ctx) != 0)
			return Fail;
	}
	if(ctx->flags[restore] == True){
		if(RestoreCCI(ctx) != 0)
			return Fail;
	}
	return 0;
}

int TrimCCI(CCI_CONTEXT *ctx)
{
	printf("[+] Trimming CCI\n");
	u64 trim_size = ctx->ncsd_struct->CCI_IMAGE_SIZE;
	if(ctx->flags[remove_update_partition] == True && ctx->ncsd_struct->partition_data[7].active == True)trim_size = ctx->ncsd_struct->CCI_S_TRIM_SIZE;
	if(TruncateFile_u64(ctx->cci_file.argument,trim_size) != 0){
		printf("[!] Failed to trim CCI\n");
		return Fail;
	}
	return 0;
}

int RestoreCCI(CCI_CONTEXT *ctx)
{
	printf("[+] Restoring CCI\n");
	if(ctx->ncsd_struct->CCI_FILE_STATUS == IS_S_TRIM){
		printf("[!] Update Data has been removed, CCI cannot be restored\n");
		return Fail;
	}
	if(TruncateFile_u64(ctx->cci_file.argument,ctx->ncsd_struct->MEDIA_SIZE) != 0){
		printf("[!] Failed to Restore CCI\n");
		return Fail;
	}
	FILE *cci = fopen(ctx->cci_file.argument,"rb+");
	fseek_64(cci,ctx->ncsd_struct->CCI_IMAGE_SIZE,SEEK_SET);
	WriteDummyBytes(cci,0xff,(ctx->ncsd_struct->MEDIA_SIZE - ctx->ncsd_struct->CCI_IMAGE_SIZE));
	fclose(cci);
	return 0;
}

void WriteDummyBytes(FILE *file, u8 dummy_byte, u64 len)
{
	u8 dummy_bytes[0x200];
	memset(&dummy_bytes,dummy_byte,0x200);
	for(u64 i = 0; i < len; i += 0x200){
		fwrite(&dummy_bytes,0x200,1,file);
	}
}

int GetNCSDData(CCI_CONTEXT *ctx)
{
	// Opening CCI
	FILE *cci = fopen(ctx->cci_file.argument,"rb");
	if(ctx->ncsd_struct == NULL)
		return Fail;
	memset(ctx->ncsd_struct,0x0,sizeof(NCSD_STRUCT));
	
	// Getting File Size of CCI File
	ctx->ncsd_struct->CCI_FILE_SIZE = GetFileSize_u64(ctx->cci_file.argument);

	NCSD_HEADER header;
	CARD_INFO_HEADER card_info;
	DEV_CARD_INFO_HEADER dev_card_info;
	
	// Reading CCI Header Sections
	fseek(cci,0x0,SEEK_SET);
	fread(&ctx->ncsd_struct->signature,0x100,1,cci);
	fseek(cci,0x100,SEEK_SET);
	fread(&header,sizeof(NCSD_HEADER),1,cci);
	fseek(cci,0x200,SEEK_SET);
	fread(&card_info,sizeof(CARD_INFO_HEADER),1,cci);
	fseek(cci,0x1200,SEEK_SET);
	fread(&dev_card_info,sizeof(DEV_CARD_INFO_HEADER),1,cci);
	
	// Checking CCI Magic
	if(u8_to_u32(header.magic,BE) != NCSD_MAGIC){
		printf("[!] NCSD File is corrupt\n");
		goto fail;
	}
	
	// Checking Media Type to see if suitable for CCI
	if(header.partition_flags[MEDIA_TYPE_INDEX] != CARD1 && header.partition_flags[MEDIA_TYPE_INDEX] != CARD2){
		printf("[!] NCSD File is not CCI\n");
		goto fail;
	}	
	
	// Determining Media Unit Size
	ctx->ncsd_struct->MEDIA_UNIT_SIZE = 0x200*pow(2,header.partition_flags[MEDIA_UNIT_SIZE]);
	
	// Media Size
	ctx->ncsd_struct->MEDIA_SIZE = u8_to_u32(header.media_size,LE)*(ctx->ncsd_struct->MEDIA_UNIT_SIZE);
	
	// Get Writable Address
	ctx->ncsd_struct->WRITABLE_ADDRESS = u8_to_u32(card_info.writable_address,LE)*(ctx->ncsd_struct->MEDIA_UNIT_SIZE);
	ctx->ncsd_struct->CARD2_MAX_SAVEDATA_SIZE = ctx->ncsd_struct->MEDIA_SIZE - ctx->ncsd_struct->WRITABLE_ADDRESS;
	// Getting CCI Image Size by summing total size of NCSD Partitions
	u32 tmp = u8_to_u32(header.offsetsize_table[0].offset,LE);
	for(int i = 0; i < 8; i++){
		tmp += u8_to_u32(header.offsetsize_table[i].size,LE);
		if (i == 6) ctx->ncsd_struct->CCI_S_TRIM_SIZE = tmp*ctx->ncsd_struct->MEDIA_UNIT_SIZE; // The Update Partition is always in Partition 7, so this size doesn't include Partition 7
	}
	ctx->ncsd_struct->CCI_IMAGE_SIZE = tmp*ctx->ncsd_struct->MEDIA_UNIT_SIZE;
	
	// Comparing CCI File Size, with calculated size
	ctx->ncsd_struct->CCI_FILE_STATUS = 0;
	if(ctx->ncsd_struct->CCI_FILE_SIZE == ctx->ncsd_struct->MEDIA_SIZE) ctx->ncsd_struct->CCI_FILE_STATUS = IS_FULL;
	else if(ctx->ncsd_struct->CCI_FILE_SIZE == ctx->ncsd_struct->CCI_IMAGE_SIZE) ctx->ncsd_struct->CCI_FILE_STATUS = IS_TRIM;
	else if(ctx->ncsd_struct->CCI_FILE_SIZE == ctx->ncsd_struct->CCI_S_TRIM_SIZE) ctx->ncsd_struct->CCI_FILE_STATUS = IS_S_TRIM;
	else {
		/*
		printf("CCI_FILE_SIZE = 0x%llx\n",ctx->ncsd_struct->CCI_FILE_SIZE);
		printf("MEDIA_SIZE = 0x%llx\n",ctx->ncsd_struct->MEDIA_SIZE);
		printf("CCI_IMAGE_SIZE = 0x%llx\n",ctx->ncsd_struct->CCI_IMAGE_SIZE);
		printf("CCI_S_TRIM_SIZE = 0x%llx\n",ctx->ncsd_struct->CCI_S_TRIM_SIZE);
		*/
		ctx->ncsd_struct->CCI_FILE_STATUS = IS_MALFORMED;
		goto fail;
	}
	
	// Storing Partition Offsets
	ctx->ncsd_struct->partition_count = 0;
	for(int i = 0; i < 8; i++){
		ctx->ncsd_struct->partition_data[i].offset = u8_to_u32(header.offsetsize_table[i].offset,LE)*ctx->ncsd_struct->MEDIA_UNIT_SIZE;
		ctx->ncsd_struct->partition_data[i].size = u8_to_u32(header.offsetsize_table[i].size,LE)*ctx->ncsd_struct->MEDIA_UNIT_SIZE;
		if(ctx->ncsd_struct->partition_data[i].offset != 0 && ctx->ncsd_struct->partition_data[i].size != 0)
			ctx->ncsd_struct->partition_data[i].active = True;
		ctx->ncsd_struct->partition_data[i].title_id = u8_to_u64(header.partition_id_table[i],LE);
		ctx->ncsd_struct->partition_data[i].fs_type = header.partitions_fs_type[i];
		ctx->ncsd_struct->partition_data[i].crypto_type = header.partitions_crypto_type[i];
		
		// Checking to see if partition actually exists
		if(ctx->ncsd_struct->partition_data[i].offset >= ctx->ncsd_struct->CCI_FILE_SIZE){
			ctx->ncsd_struct->partition_data[i].active = False;
			break;
		}
		if(ctx->ncsd_struct->partition_data[i].active) ctx->ncsd_struct->partition_count++;
	}
	
	// Exit if no CXI found
	if(!ctx->ncsd_struct->partition_data[0].active){
		printf("[!] CXI Not Found\n");
		goto fail;
	}
	
	// Getting Data from CXI
	NCCH_HEADER cxi_header;
	fseek_64(cci,(ctx->ncsd_struct->partition_data[0].offset + 0x100),SEEK_SET);
	fread(&cxi_header,sizeof(NCCH_HEADER),1,cci);
	if(u8_to_u32(cxi_header.magic,BE) != NCCH_MAGIC){
		printf("[!] CXI is Corrupt\n");
		goto fail;
	}
	// Must have ExeFS region to be CXI
	if((cxi_header.flags[MEDIA_TYPE_INDEX] & ExeFS) != ExeFS){
		printf("[!] CXI Not Found\n");
		goto fail;
	}
	ctx->ncsd_struct->partition_data[0].content_type = CXI;
	
	// Getting Product Code
	fseek_64(cci,(ctx->ncsd_struct->partition_data[0].offset + 0x150),SEEK_SET);
	fread(ctx->ncsd_struct->partition_data[0].product_code,16,1,cci);
	
	// Checking 'other flag' for crypto settings
	if((cxi_header.flags[OtherFlag] & 1) == 1){
		if((cxi_header.flags[OtherFlag] & 4) == 4) ctx->ncsd_struct->partition_data[0].ncch_crypto_key = no_crypto;	
		else if ((cxi_header.program_id[4] && 0x10) == 0x10) ctx->ncsd_struct->partition_data[0].ncch_crypto_key = fixed_system;
		else ctx->ncsd_struct->partition_data[0].ncch_crypto_key = fixed_zeros;
	}
	else if(!cxi_header.flags[OtherFlag]){
		ctx->ncsd_struct->partition_data[0].ncch_crypto_key = secure_key;
		if(cxi_header.flags[SecureCryptoType2Flag]) ctx->ncsd_struct->partition_data[0].ncch_crypto_key = secure_key2;
	}
	
	// Getting SDK Version
	u32 CXI_Media_Unit_Size = 0x200*pow(2,cxi_header.flags[MEDIA_UNIT_SIZE]);
	u64 plain_region_offset = ctx->ncsd_struct->partition_data[0].offset + u8_to_u32(cxi_header.plain_region_offset,LE)*CXI_Media_Unit_Size;
	u64 plain_region_size = u8_to_u32(cxi_header.plain_region_size,LE)*CXI_Media_Unit_Size;
	int result = GetSDKVersion(cci,plain_region_offset,plain_region_size,ctx->ncsd_struct);
	// If that failed, attempt to guess SDK version
	if(result){
		strcpy(ctx->ncsd_struct->SDK_PATCH,"Release");
		memset(&ctx->ncsd_struct->SDK_VER,0,3);
		ctx->ncsd_struct->SDK_VER[0] = 1;
		if(header.partition_flags[MEDIA_CARD_DEVICE_OLD]) ctx->ncsd_struct->SDK_VER[0] = 2;
		if(header.partition_flags[MEDIA_CARD_DEVICE]) ctx->ncsd_struct->SDK_VER[0] = 3;
		if(u8_to_u32(cxi_header.logo_region_offset,LE) || u8_to_u32(cxi_header.logo_region_size,LE)) ctx->ncsd_struct->SDK_VER[0] = 5;
		if(cxi_header.flags[SecureCryptoType2Flag])  ctx->ncsd_struct->SDK_VER[0] = 6;
	}
	
	
	// Getting Data from remaining CFA partitions	
	for(int i = 1; i < 8; i++){
		u8 magic[4];
		fseek_64(cci,(ctx->ncsd_struct->partition_data[i].offset + 0x100),SEEK_SET);
		fread(&magic,4,1,cci);
		if(u8_to_u32(magic,BE) == NCCH_MAGIC){
			u8 flags[8];
			u8 ProgramID[8];
			fseek_64(cci,(ctx->ncsd_struct->partition_data[i].offset + 0x188),SEEK_SET);
			fread(&flags,8,1,cci);
			fseek_64(cci,(ctx->ncsd_struct->partition_data[i].offset + 0x118),SEEK_SET);
			fread(&ProgramID,8,1,cci);
			if( (flags[NCCH_Type] & ExeFS) != ExeFS && (flags[NCCH_Type] & RomFS) == RomFS ){
				if((flags[NCCH_Type] & Manual) == Manual && (flags[NCCH_Type] & SystemUpdate) != SystemUpdate)
					ctx->ncsd_struct->partition_data[i].content_type = CFA_Manual;
				else if((flags[NCCH_Type] & Child) == Child)
					ctx->ncsd_struct->partition_data[i].content_type = CFA_DLPChild;
				else if((flags[NCCH_Type] & SystemUpdate) == SystemUpdate && (flags[NCCH_Type] & Manual) != Manual)
					ctx->ncsd_struct->partition_data[i].content_type = CFA_Update;
				else
					ctx->ncsd_struct->partition_data[i].content_type = CFA_Simple;
			}
			else if((flags[NCCH_Type] & ExeFS) == ExeFS)
				ctx->ncsd_struct->partition_data[i].content_type = CXI;
			else
				ctx->ncsd_struct->partition_data[i].content_type = _unknown;


			// Checking 'other flag' for crypto settings
			if((flags[OtherFlag] & 1) == 1){
				if((flags[OtherFlag] & 4) == 4) ctx->ncsd_struct->partition_data[i].ncch_crypto_key = no_crypto;	
				else if ((ProgramID[4] && 0x10) == 0x10) ctx->ncsd_struct->partition_data[i].ncch_crypto_key = fixed_system;
				else ctx->ncsd_struct->partition_data[i].ncch_crypto_key = fixed_zeros;
			}
			else if(!flags[OtherFlag]){
				ctx->ncsd_struct->partition_data[i].ncch_crypto_key = secure_key;
				if(flags[SecureCryptoType2Flag]) ctx->ncsd_struct->partition_data[i].ncch_crypto_key = secure_key2;
			}

			fseek_64(cci,(ctx->ncsd_struct->partition_data[i].offset + 0x150),SEEK_SET);
			fread(ctx->ncsd_struct->partition_data[i].product_code,16,1,cci);
		}
		else
			ctx->ncsd_struct->partition_data[i].content_type = _unknown;
	} 
		
	if(ctx->flags[info])
		PrintNCSDHeaderData(ctx->ncsd_struct,&header,&card_info,&dev_card_info);
	if(ctx->flags[part_info])
		PrintCCIPartitionData(ctx->ncsd_struct,&header,&card_info,&dev_card_info);
	fclose(cci);
	return 0;
fail:
	fclose(cci);
	return Fail;

}

int ExtractCCIPartitions(CCI_CONTEXT *ctx)
{
	FILE *cci = fopen(ctx->cci_file.argument,"rb");
	if(cci == NULL)
		return Fail;
		
	chdir(ctx->outfile.argument);
	
	u64 chunk_size = ctx->ncsd_struct->MEDIA_UNIT_SIZE;
	u8 *chunk = malloc(chunk_size);
	for(int i = 0; i < 8; i++){	
		if(ctx->ncsd_struct->partition_data[i].active == True){
			char output[1024];
			memset(&output,0,1024);
			switch(ctx->ncsd_struct->partition_data[i].content_type){
				case CXI : snprintf(output,1024,"%s_%d_APPDATA.cxi",ctx->ncsd_struct->partition_data[i].product_code,i); break;
				case CFA_Manual : snprintf(output,1024,"%s_%d_MANUAL.cfa",ctx->ncsd_struct->partition_data[i].product_code,i); break;
				case CFA_DLPChild : snprintf(output,1024,"%s_%d_DLP.cfa",ctx->ncsd_struct->partition_data[i].product_code,i); break;
				case CFA_Update : snprintf(output,1024,"%s_%d_UPDATEDATA.cfa",ctx->ncsd_struct->partition_data[i].product_code,i); break;
				case CFA_Simple : snprintf(output,1024,"%s_%d.cfa",ctx->ncsd_struct->partition_data[i].product_code,i); break;
				default: snprintf(output,1024,"%s_%d.bin",ctx->ncsd_struct->partition_data[i].product_code,i); break;
			}
			FILE *out = fopen(output,"wb");
			if(out == NULL){
				fclose(cci);
				return Fail;
			}
			printf("[+] Writing '%s'\n",output);
			u64 size = ctx->ncsd_struct->partition_data[i].size;
			u64 chunk_num = (size/chunk_size);
			u64 in_offset = ctx->ncsd_struct->partition_data[i].offset;
			u64 out_offset = 0;
			for(u64 i = 0; i < chunk_num; i++){				
				fseek_64(cci,in_offset,SEEK_SET);
				fseek_64(out,out_offset,SEEK_SET);
				
				fread(chunk,chunk_size,1,cci);
				fwrite(chunk,chunk_size,1,out);
				
				in_offset += chunk_size;
				out_offset += chunk_size;
			}
			fclose(out);
		}
	}
	free(chunk);
	fclose(cci);
	return 0;
}

void PrintNCSDHeaderData(NCSD_STRUCT *ctx, NCSD_HEADER *header, CARD_INFO_HEADER *card_info, DEV_CARD_INFO_HEADER *dev_card_info)
{
	printf("[+] CCI Image Details\n");
	u8 CardDevice = header->partition_flags[MEDIA_CARD_DEVICE_OLD] + header->partition_flags[MEDIA_CARD_DEVICE];
	switch (header->partition_flags[MEDIA_TYPE_INDEX]){
		case INNER_DEVICE: printf(" Media Type:            INTERNAL_DEVICE\n"); break;
		case CARD1: printf(" Media Type:            CARD1\n"); break;
		case CARD2: printf(" Media Type:            CARD2\n  > Writable Region:\n   - Offset:            0x%llx\n   - Size:              0x%llx (%lld MB)\n",ctx->WRITABLE_ADDRESS,ctx->CARD2_MAX_SAVEDATA_SIZE,(ctx->CARD2_MAX_SAVEDATA_SIZE/MB)); break;
		case EXTENDED_DEVICE: printf(" Media Type:            EXTENDED_DEVICE\n"); break;
	}
	GetCHIPFullSize(ctx->MEDIA_SIZE,ctx->type);
	GetCCIDataSize(ctx->CCI_IMAGE_SIZE,ctx->type);
	GetCCIFileStatus(ctx->CCI_FILE_SIZE,ctx->CCI_FILE_STATUS,ctx->type);
	switch (CardDevice){
		case CARD_DEVICE_NOR_FLASH: printf(" Additional Device:     EEPROM\n"); break;
		case CARD_DEVICE_NONE: printf(" Additional Device:     None\n"); break;
		case CARD_DEVICE_BT: printf(" Additional Device:     BT\n"); break;
	}
	printf(" Partition Count:       %d\n",ctx->partition_count);
	if(u8_to_u64(card_info->cver_title_id,LE) && u8_to_u16(card_info->cver_title_version,LE)){
		//printf("CVer Title ID:          %016llx\n",u8_to_u64(card_info->cver_title_id,LE));
		//printf("CVer Title Ver:         v%d\n",u8_to_u16(card_info->cver_title_version,LE));
		GetCUPVersion(card_info);
	}
	if(!header->partition_flags[MEDIA_6X_SAVE_CRYPTO] && !header->partition_flags[MEDIA_CARD_DEVICE] && !header->partition_flags[MEDIA_CARD_DEVICE_OLD]){
		printf(" Save Crypto:           Repeating CTR Fail\n");
	}
	else if(!header->partition_flags[MEDIA_6X_SAVE_CRYPTO] && (header->partition_flags[MEDIA_CARD_DEVICE] || header->partition_flags[MEDIA_CARD_DEVICE_OLD])){
		printf(" Save Crypto:           2.2.0 KeyY Method\n");
	}
	else if(header->partition_flags[MEDIA_6X_SAVE_CRYPTO] == 1 && !header->partition_flags[MEDIA_CARD_DEVICE_OLD]){
		if(!header->partition_flags[MEDIA_CARD_DEVICE]){
			printf(" Save Crypto:           2.2.0 KeyY Method\n");
		}
		else if(header->partition_flags[MEDIA_CARD_DEVICE]){
			printf(" Save Crypto:           6.0.0 KeyY Method\n");
		}
	}
	
	printf("[+] CXI Partition\n");
	printf(" Product Code:          %.16s\n",card_info->partition_0_header.product_code);
	printf(" Company Code:          %.2s\n",card_info->partition_0_header.maker_code);
	u32 UniqueID = u8_to_u32(&card_info->partition_0_header.program_id[1],LE)&0xffffff;
	printf(" Unique ID:             %05x\n",UniqueID);
	printf(" Build Type:            %s\n",ctx->BUILD_TYPE? "Debug or Development" : "Release");
	printf(" SDK Version:           %d.%d.%d %s\n",ctx->SDK_VER[0],ctx->SDK_VER[1],ctx->SDK_VER[2],ctx->SDK_PATCH);
	if(ctx->FW_VER[0] || ctx->FW_VER[1]) printf(" Req. Kernel Version:   %d.%d-%d\n",ctx->FW_VER[0],ctx->FW_VER[1],ctx->FW_VER[2]);
	printf("[+] CFA Partitions\n");
	for(int i = 1; i < 8; i++){
		if(i == 1) printf(" E-Manual:              %s\n",ctx->partition_data[i].active? "Yes" : "No"); 
		else if(i == 2) printf(" DLP Child:             %s\n",ctx->partition_data[i].active? "Yes" : "No"); 
		else if(i == 7) printf(" Update Data:           %s\n",ctx->partition_data[i].active? "Yes" : "No"); 
		else{
			if(ctx->partition_data[i].active) printf(" CFA %d:                %s\n",i,ctx->partition_data[i].active? "Yes" : "No"); 
		}
	}
	
	
	
}

void PrintCCIPartitionData(NCSD_STRUCT *ctx, NCSD_HEADER *header, CARD_INFO_HEADER *card_info, DEV_CARD_INFO_HEADER *dev_card_info)
{
	//printf("\n[+] CCI Partitions\n");
	for(int i = 0; i < 8; i++){
		if(ctx->partition_data[i].active){
			if(ctx->partition_data[i].content_type == CXI){
				printf("[+] CXI %d\n",i);
			}
			else {
				printf("[+] CFA %d\n",i);
			}
			printf(" > Title ID:              %016llx\n",ctx->partition_data[i].title_id);
			printf(" > Product Code:          %s\n",ctx->partition_data[i].product_code);
			printf(" > Content Type:          ");
			switch(ctx->partition_data[i].content_type){
				case _unknown : printf("Unknown\n"); break;
				case CXI : printf("Application\n"); break;
				case CFA_Manual : printf("Electronic Manual\n"); break;
				case CFA_DLPChild : printf("Download Play Child\n"); break;
				case CFA_Update : printf("System Update Data\n"); break;
			}
			
			//printf(" FS Type:               %x\n",ctx->partition_data[i].fs_type);
			//printf(" Crypto Type:           %x\n",ctx->partition_data[i].crypto_type);
			printf(" > Crypto Key:            ");
			switch(ctx->partition_data[i].ncch_crypto_key){
				case(fixed_zeros) : printf("Fixed Key (Zeros)\n"); break;
				case(fixed_system) : printf("Fixed Key (System)\n"); break;
				case(secure_key) : printf("Secure Key (1.0.0 KeyY Method)\n"); break;
				case(secure_key2) : printf("Secure Key (7.0.0 KeyY Method)\n"); break;
				case(no_crypto) : printf("Not Encrypted\n"); break;
			}
			printf(" > Offset:                0x%x\n",ctx->partition_data[i].offset);
			printf(" > Size:                  0x%x",ctx->partition_data[i].size);
			//if(ctx->partition_data[i].size > GB) printf(" (%d GB)\n",ctx->partition_data[i].size/GB);
			if(ctx->partition_data[i].size > MB) printf(" (%d MB)\n",ctx->partition_data[i].size/MB);
			else printf(" (%d KB)\n",ctx->partition_data[i].size/KB);
			//printf("\n");
		}
	}
}

void GetCHIPFullSize(u64 MEDIA_SIZE, int type)
{
	char string[100];
	u64 UnitSizesBytes[3] = {GB,MB,KB};
	u64 UnitSizesBits[3] = {GB/8,MB/8,KB/8};
	char Str_UnitSizesBytes[3][3] = {"GB","MB","KB"};
	char Str_UnitSizesBits[3][5] = {"Gbit","Mbit","Kbit"};
	char ChipName[30] = {" Media Size:           "};
	u8 ByteIndex = 0;
	while(MEDIA_SIZE < UnitSizesBytes[ByteIndex]){
		if(ByteIndex == 2) break;
		ByteIndex++;
	}
	
	sprintf(string,"%s %lld %s",ChipName,(MEDIA_SIZE/UnitSizesBytes[ByteIndex]),Str_UnitSizesBytes[ByteIndex]);
	if((MEDIA_SIZE/UnitSizesBits[ByteIndex]) >= UnitSizesBytes[ByteIndex+1])
		ByteIndex--;
	sprintf(string,"%s (%lld %s)",string,(MEDIA_SIZE/UnitSizesBits[ByteIndex]),Str_UnitSizesBits[ByteIndex]);

	printf("%s\n",string);
	return;
}
void GetCCIDataSize(u64 CCI_IMAGE_SIZE, int type)
{
	char string[100];
	u64 UnitSizesBytes[2] = {MB,KB};
	char Str_UnitSizesBytes[2][3] = {"MB","KB"};
	char ChipName[30] = {" CCI Data Size:         "};
	u8 ByteIndex = 0;
	if(CCI_IMAGE_SIZE < UnitSizesBytes[ByteIndex]) ByteIndex = 1;
	
	sprintf(string,"%s%lld %s",ChipName,(CCI_IMAGE_SIZE/UnitSizesBytes[ByteIndex]),Str_UnitSizesBytes[ByteIndex]);
	sprintf(string,"%s (0x%llx bytes)",string,CCI_IMAGE_SIZE);

	printf("%s\n",string);
	return;
}

void GetCCIFileStatus(u64 CCI_FILE_SIZE, u8 CCI_FILE_STATUS, int type)
{	
	char string[100];
	u64 UnitSizesBytes[2] = {MB,KB};
	char Str_UnitSizesBytes[2][3] = {"MB","KB"};
	char Str_CCI_Status[4][50] = {"Malformed","Full Size","Trimmed","Update Partition Removed (Not Reversible)"};
	u8 ByteIndex = 0;
	if(CCI_FILE_SIZE < UnitSizesBytes[ByteIndex]) ByteIndex = 1;
	
	sprintf(string," CCI File:\n  > Size                %lld %s",(CCI_FILE_SIZE/UnitSizesBytes[ByteIndex]),Str_UnitSizesBytes[ByteIndex]);
	sprintf(string,"%s\n  > Status              %s",string,Str_CCI_Status[CCI_FILE_STATUS]);
	printf("%s\n",string);
	return;
}

void GetCUPVersion(/*char *FW_STRING, */CARD_INFO_HEADER *card_info)
{
	u8 MAJOR = 0;
	u8 MINOR = 0;
	u8 BUILD = 0;
	char REGION_CHAR = ' ';

	u16 CVer_ver = u8_to_u16(card_info->cver_title_version,LE);
	u32 CVer_UID = u8_to_u32(card_info->cver_title_id,LE);
		
	switch(CVer_UID){
		case EUR_UPDATE : REGION_CHAR = 'E'; break;
		case JPN_UPDATE : REGION_CHAR = 'J'; break;
		case USA_UPDATE : REGION_CHAR = 'U'; break;
		case CHN_UPDATE : REGION_CHAR = 'C'; break;
		case KOR_UPDATE : REGION_CHAR = 'K'; break;
		case TWN_UPDATE : REGION_CHAR = 'T'; break;
	}
	
	
	switch(CVer_ver){
		case 3088 : MAJOR = 3; MINOR = 0; BUILD = 0; break;
		default ://This tends to work most of the time, use above for manual overrides
			MAJOR = (CVer_ver & 0x7f00) >> 10;
			MINOR = (CVer_ver & 0x3f0) >> 4;
			//BUILD = CVer_ver & 0xf; 
			break;	
	}
	printf(" CUP Version:           %d.%d.%d%c\n",MAJOR,MINOR,BUILD,REGION_CHAR);
}

int GetSDKVersion(FILE *cxi, u64 plain_region_offset, u64 plain_region_size, NCSD_STRUCT *ctx)
{
	// Counting Strings in Plain Region
	int string_count = 0;
	fseek_64(cxi,plain_region_offset,SEEK_SET);
	while (ftell(cxi) <= (u32)(plain_region_offset+plain_region_size)){
		if(fgetc(cxi) == '[') string_count++;
	}
	
	// Storing Plain Region Strings
	fseek_64(cxi,plain_region_offset,SEEK_SET);
	char tmp = '\0';
	int j = 0;
	char MiddleWareString[string_count][100];
	memset(&MiddleWareString,0,100*string_count*sizeof(char));
	for(int i = 0; i < string_count; i++){
		while(tmp != '[') tmp = fgetc(cxi);
		tmp = fgetc(cxi);
		while(tmp != ']') {
			MiddleWareString[i][j] = tmp;
			tmp = fgetc(cxi);
			j++;
		}
		MiddleWareString[i][j] = '\0';
		j = 0;
	}
	//printf("Total MDW Strings: %d\n",string_count);
	
	// Checking Plain Region Strings
	int found_sdk_ver = 0;
	char SDK_VER[3][4];
	char FW_VER[3][4];
	for(int i = 0; i < string_count; i++){
		//printf(" %s\n",MiddleWareString[i]);
		if(strncmp("SDK+NINTENDO:DEBUG",MiddleWareString[i],18) == 0) ctx->BUILD_TYPE = 1;
		else if(strncmp("SDK+NINTENDO:CTR_SDK-",MiddleWareString[i],21) == 0 && !found_sdk_ver){
			found_sdk_ver = true;
			int set = 0;
			memset(&SDK_VER,0,4*3*sizeof(char));
			int len = strlen(MiddleWareString[i]);
			for(int k = 21, ver_str = 0; k < len; k++){
				//printf(" %c\n",MiddleWareString[i][k]); 
				if(MiddleWareString[i][k] == '_'){
					if(set < 3){
						SDK_VER[set][ver_str] = '\0';
					}
					else{
						ctx->SDK_PATCH[ver_str] = '\0';
					}
					ver_str = 0;
					set++;
					k++;
				}
				if(set < 3)
					SDK_VER[set][ver_str] = MiddleWareString[i][k];
				else
					ctx->SDK_PATCH[ver_str] = MiddleWareString[i][k];
				
				ver_str++;
			}
		}
		else if(strncmp("SDK+NINTENDO:Firmware-",MiddleWareString[i],22) == 0){
			int set = 0;
			memset(&FW_VER,0,4*3*sizeof(char));
			int len = strlen(MiddleWareString[i]);
			for(int k = 22, ver_str = 0; k < len; k++){
				//printf(" %c\n",MiddleWareString[i][k]); 
				if(MiddleWareString[i][k] == '_'){
					FW_VER[set][ver_str] = '\0';
					ver_str = 0;
					set++;
					k++;
				}
				FW_VER[set][ver_str] = MiddleWareString[i][k];
				ver_str++;
			}
		}
	}
	if(!found_sdk_ver) return 1;
	
	if(strcmp(ctx->SDK_PATCH,"none") == 0) strcpy(ctx->SDK_PATCH,"Release");
	
	for(int i = 0; i < 3; i++){
		ctx->SDK_VER[i] = strtol(SDK_VER[i],NULL,10);
	}
	for(int i = 0; i < 3; i++){
		ctx->FW_VER[i] = strtol(FW_VER[i],NULL,10);
	}
	
	return 0;
}

