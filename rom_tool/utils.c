#include "lib.h"

//MISC
void char_to_int_array(unsigned char destination[], char source[], int size, int endianness, int base)
{	
	char tmp[size][2];
    unsigned char *byte_array = malloc(size*sizeof(unsigned char));
	memset(byte_array, 0, size);
	memset(destination, 0, size);
	memset(tmp, 0, size*2);
    
    for (int i = 0; i < size; i ++){
		tmp[i][0] = source[(i*2)];
        tmp[i][1] = source[((i*2)+1)];
		tmp[i][2] = '\0';
        byte_array[i] = (unsigned char)strtol(tmp[i], NULL, base);
    }
	endian_memcpy(destination,byte_array,size,endianness);
	/**
	for (int i = 0; i < size; i++){
        switch (endianness){
        	case(BE):
        	destination[i] = byte_array[i];
        	break;
        	case(LE):
        	destination[i] = byte_array[((size-1)-i)];
        	break;
        }
    }
	**/
	_free(byte_array);
}

void endian_memcpy(u8 *destination, u8 *source, u32 size, int endianness)
{ 
    for (u32 i = 0; i < size; i++){
        switch (endianness){
            case(BE):
                destination[i] = source[i];
                break;
            case(LE):
                destination[i] = source[((size-1)-i)];
                break;
        }
    }
}

void u8_hex_print_be(u8 *array, int len)
{
	for(int i = 0; i < len; i++)
		printf("%02x",array[i]);
}

void u8_hex_print_le(u8 *array, int len)
{
	for(int i = 0; i < len; i++)
		printf("%02x",array[len - i - 1]);
}

u32 align_value(u32 value, u32 alignment)
{
	u32 tmp = value;
	while(tmp > alignment)
		tmp -= alignment;
	return (value + (alignment - tmp));
}

void resolve_flag(unsigned char flag, unsigned char *flag_bool)
{
	unsigned char bit_mask[8] = {0x80,0x40,0x20,0x10,0x8,0x4,0x2,0x1};
	for(int i = 0; i < 8; i++){
		if (flag >= bit_mask[i]){
			flag_bool[7-i] = True;
			flag -= bit_mask[i];
		}
		else
			flag_bool[7-i] = False;
	}
}

void resolve_flag_u16(u16 flag, unsigned char *flag_bool)
{
	u16 bit_mask[16] = {0x8000,0x4000,0x2000,0x1000,0x800,0x400,0x200,0x100,0x80,0x40,0x20,0x10,0x8,0x4,0x2,0x1};
	for(int i = 0; i < 16; i++){
		if (flag >= bit_mask[i]){
			flag_bool[15-i] = True;
			flag -= bit_mask[i];
		}
	else
		flag_bool[15-i] = False;
	}
}

int append_filextention(char *output, u16 max_outlen, char *input, char extention[])
{
	if(output == NULL || input == NULL){
		printf("[!] Memory Error\n");
		return Fail;
	}
	memset(output,0,max_outlen);
	u16 extention_point = strlen(input)+1;
	for(int i = strlen(input)-1; i > 0; i--){
		if(input[i] == '.'){
			extention_point = i;
			break;
		}
	}
	if(extention_point+strlen(extention) >= max_outlen){
		printf("[!] Input File Name Too Large for Output buffer\n");
		return Fail;
	}
	memcpy(output,input,extention_point);
	sprintf(output,"%s%s",output,extention);
	return 0;
}

//IO Related
int ExportFileToFile(FILE *in, FILE *out, u64 size, u64 in_offset, u64 out_offset)
{
	u8 *buffer = malloc(size);
	if(buffer == NULL)
		return IO_ERROR;
	ReadFile_64(buffer,size,in_offset,in);
	WriteBuffer(buffer,size,out_offset,out);
	return 0;
}

void WriteBuffer(void *buffer, u64 size, u64 offset, FILE *output)
{
	fseek_64(output,offset,SEEK_SET);
	fwrite(buffer,size,1,output);
} 

void ReadFile_64(void *outbuff, u64 size, u64 offset, FILE *file)
{
	fseek_64(file,offset,SEEK_SET);
	fread(outbuff,size,1,file);
}

u64 GetFileSize_u64(char *filename)
{
	u64 size;
#ifdef _WIN32
        int fh;
        u64 n;
        fh = _open( filename, 0 );
        n = _lseeki64(fh, 0, SEEK_END);
        _close(fh);
        size = (n / sizeof(short))*2;
#else
#ifdef __linux__
        struct stat st;
        stat(filename, &st);
        size = st.st_size;
#else
        FILE *file = fopen(filename,"rb");
        fseeko(file, 0L, SEEK_END);
        size = (u64)ftello(file);
        fclose(file);
#endif //__linux__
#endif //_WIN32
        return size;
}

u32 GetFileSize_u32(FILE *file)
{
	u32 size = 0;
	fseek(file, 0L, SEEK_END);
	size = ftell(file);
	fseek(file, 0L, SEEK_SET);
	return size;
}

int TruncateFile_u64(char *filename, u64 filelen)
{
#ifdef _WIN32
	HANDLE fh;
 
	LARGE_INTEGER fp;
	fp.QuadPart = filelen;
 
	fh = CreateFile(filename, GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
	if (fh == INVALID_HANDLE_VALUE) {
		printf("[!] Invalid File handle\n");
		return 1;
	}
 
	if (SetFilePointerEx(fh, fp, NULL, FILE_BEGIN) == 0 ||
	    SetEndOfFile(fh) == 0) {
		printf("[!] truncate failed\n");
		CloseHandle(fh);
		return 1;
	}
 
	CloseHandle(fh);
	return 0;
#else
	return truncate64(filename,filelen); //truncate(filename,filelen);
#endif	
}

int fseek_64(FILE *fp, u64 file_pos, int whence)
{
#ifdef _WIN32
	if(whence != SEEK_SET)
		printf("[!] fseek_64, whence has been overided to SEEK_SET\n");
	fpos_t pos = file_pos;
	return fsetpos(fp,&pos); //I can't believe the 2gb problem with Windows & MINGW, maybe I have a bad installation :/
#else
	return fseeko(fp,file_pos,whence);
#endif
}

int makedir(const char* dir)
{
#ifdef _WIN32
	return _mkdir(dir);
#else
	return mkdir(dir, 0777);
#endif
}

char *getcwdir(char *buffer,int maxlen)
{
#ifdef _WIN32
	return _getcwd(buffer,maxlen);
#else
	return getcwd(buffer,maxlen);
#endif
}

void _free(void *ptr)
{
	free(ptr);
	ptr = NULL;
}

//Data Size conversion
u16 u8_to_u16(u8 *value, u8 endianness)
{
	u16 new_value;
	switch(endianness){
		case(BE): new_value =  (value[1]<<0) | (value[0]<<8); break;
		case(LE): new_value = (value[0]<<0) | (value[1]<<8); break;
	}
	return new_value;
}

u32 u8_to_u32(u8 *value, u8 endianness)
{
	u32 new_value;
	switch(endianness){
		case(BE): new_value = (value[3]<<0) | (value[2]<<8) | (value[1]<<16) | (value[0]<<24); break;
		case(LE): new_value = (value[0]<<0) | (value[1]<<8) | (value[2]<<16) | (value[3]<<24); break;
	}
	return new_value;
}


u64 u8_to_u64(u8 *value, u8 endianness)
{
	u64 u64_return = 0;
	switch(endianness){
		case(BE): 
			u64_return |= (u64)value[7]<<0;
			u64_return |= (u64)value[6]<<8;
			u64_return |= (u64)value[5]<<16;
			u64_return |= (u64)value[4]<<24;
			u64_return |= (u64)value[3]<<32;
			u64_return |= (u64)value[2]<<40;
			u64_return |= (u64)value[1]<<48;
			u64_return |= (u64)value[0]<<56;
			break;
			//return (value[7]<<0) | (value[6]<<8) | (value[5]<<16) | (value[4]<<24) | (value[3]<<32) | (value[2]<<40) | (value[1]<<48) | (value[0]<<56);
		case(LE): 
			u64_return |= (u64)value[0]<<0;
			u64_return |= (u64)value[1]<<8;
			u64_return |= (u64)value[2]<<16;
			u64_return |= (u64)value[3]<<24;
			u64_return |= (u64)value[4]<<32;
			u64_return |= (u64)value[5]<<40;
			u64_return |= (u64)value[6]<<48;
			u64_return |= (u64)value[7]<<56;
			break;
			//return (value[0]<<0) | (value[1]<<8) | (value[2]<<16) | (value[3]<<24) | (value[4]<<32) | (value[5]<<40) | (value[6]<<48) | (value[7]<<56);
	}
	return u64_return;
}

int u16_to_u8(u8 *out_value, u16 in_value, u8 endianness)
{
	switch(endianness){
		case(BE):
			out_value[0]=(in_value >> 8);
			out_value[1]=(in_value >> 0);
			break;
		case(LE):
			out_value[0]=(in_value >> 0);
			out_value[1]=(in_value >> 8);
			break;
	}
	return 0;
}

int u32_to_u8(u8 *out_value, u32 in_value, u8 endianness)
{
	switch(endianness){
		case(BE):
			out_value[0]=(in_value >> 24);
			out_value[1]=(in_value >> 16);
			out_value[2]=(in_value >> 8);
			out_value[3]=(in_value >> 0);
			break;
		case(LE):
			out_value[0]=(in_value >> 0);
			out_value[1]=(in_value >> 8);
			out_value[2]=(in_value >> 16);
			out_value[3]=(in_value >> 24);
			break;
	}
	return 0;
}

int u64_to_u8(u8 *out_value, u64 in_value, u8 endianness)
{
	switch(endianness){
		case(BE):
			out_value[0]=(in_value >> 56);
			out_value[1]=(in_value >> 48);
			out_value[2]=(in_value >> 40);
			out_value[3]=(in_value >> 32);
			out_value[4]=(in_value >> 24);
			out_value[5]=(in_value >> 16);
			out_value[6]=(in_value >> 8);
			out_value[7]=(in_value >> 0);
			break;
		case(LE):
			out_value[0]=(in_value >> 0);
			out_value[1]=(in_value >> 8);
			out_value[2]=(in_value >> 16);
			out_value[3]=(in_value >> 24);
			out_value[4]=(in_value >> 32);
			out_value[5]=(in_value >> 40);
			out_value[6]=(in_value >> 48);
			out_value[7]=(in_value >> 56);
			break;
	}
	return 0;
}

//Copied from ctrtool
void memdump(FILE* fout, const char* prefix, const u8* data, u32 size)
{
	u32 i;
	u32 prefixlen = strlen(prefix);
	u32 offs = 0;
	u32 line = 0;
	while(size)
	{
		u32 max = 32;

		if (max > size)
			max = size;

		if (line==0)
			fprintf(fout, "%s", prefix);
		else
			fprintf(fout, "%*s", prefixlen, "");


		for(i=0; i<max; i++)
			fprintf(fout, "%02X", data[offs+i]);
		fprintf(fout, "\n");
		line++;
		size -= max;
		offs += max;
	}
}


