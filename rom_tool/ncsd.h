typedef enum
{
	secure_key,
	secure_key2,
	fixed_zeros,
	fixed_system,
	no_crypto
} fixed_ncch_crypto;

// Flag Enums
typedef enum
{
	SecureCryptoType2Flag = 3,
	NCCH_Platform = 4,
	NCCH_Type = 5,
	NCCH_MediaUnitSize = 6,
	OtherFlag = 7
} NcchFlagIndex;

typedef enum
{
	MEDIA_6X_SAVE_CRYPTO = 1,
	MEDIA_CARD_DEVICE = 3,
	MEDIA_PLATFORM_INDEX = 4,
	MEDIA_TYPE_INDEX = 5,
	MEDIA_UNIT_SIZE = 6,
	MEDIA_CARD_DEVICE_OLD = 7
} NcsdFlagIndex;

typedef enum
{
	CARD_DEVICE_NOR_FLASH = 1,
	CARD_DEVICE_NONE = 2,
	CARD_DEVICE_BT = 3
} _CardDevice;

typedef enum
{
	CTR,
} _PlatformIndex;

typedef enum
{
	INNER_DEVICE,
	CARD1,
	CARD2,
	EXTENDED_DEVICE
} _TypeIndex;

typedef enum
{
	RomFS = 0x1,
	ExeFS = 0x2,
	SystemUpdate = 0x4,
	Manual = 0x8,
	Child = (0x4|0x8),
	Trial = 0x10
} ncch_content_bitmask;

//
typedef enum
{
	EUR_UPDATE = 0x00017102,
	JPN_UPDATE = 0x00017202,
	USA_UPDATE = 0x00017302,
	CHN_UPDATE = 0x00017402,
	KOR_UPDATE = 0x00017502,
	TWN_UPDATE = 0x00017602
} CVER_UID_REGION;

typedef struct
{
	u8 offset[4];
	u8 size[4];
} partition_offsetsize;

typedef struct
{
	u8 magic[4];
	u8 media_size[4];
	u8 title_id[8];
	u8 partitions_fs_type[8];
	u8 partitions_crypto_type[8];
	partition_offsetsize offsetsize_table[8];
	u8 exheader_hash[0x20];
	u8 additional_header_size[0x4];
	u8 sector_zero_offset[0x4];
	u8 partition_flags[8];
	u8 partition_id_table[8][8];
	u8 reserved[0x30];
} NCSD_HEADER;

typedef struct
{
	u8 magic[4];
	u8 content_size[4];
	u8 title_id[8];
	u8 maker_code[2];
	u8 version[2];
	u8 reserved_0[4];
	u8 program_id[8];
	u8 temp_flag;
	u8 reserved_1[0xF];
	u8 logo_sha_256_hash[0x20];
	u8 product_code[0x10];
	u8 extended_header_sha_256_hash[0x20];
	u8 extended_header_size[4];
	u8 reserved_2[4];
	u8 flags[8];
	u8 plain_region_offset[4];
	u8 plain_region_size[4];
	u8 logo_region_offset[4];
	u8 logo_region_size[4];
	u8 exefs_offset[4];
	u8 exefs_size[4];
	u8 exefs_hash_size[4];
	u8 reserved_4[4];
	u8 romfs_offset[4];
	u8 romfs_size[4];
	u8 romfs_hash_size[4];
	u8 reserved_5[4];
	u8 exefs_sha_256_hash[0x20];
	u8 romfs_sha_256_hash[0x20];
} __attribute__((__packed__)) 
NCCH_HEADER;

typedef struct
{
	u8 writable_address[4];
	u8 card_info_bitmask[4];
	u8 reserved_0[0xf8];
	u8 media_size_used[8];
	u8 reserved_1[0x18];
	u8 cver_title_id[8];
	u8 cver_title_version[2];
	u8 reserved_2[0xcd6];
	u8 partition_0_title_id[8];
	u8 reserved_3[8];
	u8 initial_data[0x30];
	u8 reserved_4[0xc0];
	NCCH_HEADER partition_0_header;
} CARD_INFO_HEADER;

typedef struct
{
	u8 CardDeviceReserved1[0x200];
	u8 TitleKey[0x10];
	u8 CardDeviceReserved2[0xf0];
} DEV_CARD_INFO_HEADER;
/*
typedef struct
{
	int Count;
	char 
} PLAIN_REGION_STRUCT;
*/
/**
typedef struct
{
	int valid;
	int sig_valid;
	int type;
	u8 signature[0x100];
	u8 ncsd_header_hash[0x20];
	NCSD_HEADER header;
	CARD_INFO_HEADER card_info;
	DEV_CARD_INFO_HEADER dev_card_info;
	
	u64 media_size;
	u64 used_media_size;
	PARTITION_DATA partition_data[8];
} NCSD_STRUCT;
**/
/**
typedef struct
{
	int active;
	int sig_valid;
	u8 fs_type;
	u8 crypto_type;
	u32 offset;
	u32 size;
	u64 title_id;
} PARTITION_DATA;

**/



int NCSDProcess(CCI_CONTEXT *ctx);
int GetNCSDData(CCI_CONTEXT *ctx);
int TrimCCI(CCI_CONTEXT *ctx);
int RestoreCCI(CCI_CONTEXT *ctx);
int ExtractCCIPartitions(CCI_CONTEXT *ctx);
void WriteDummyBytes(FILE *file, u8 dummy_byte, u64 len);
void PrintNCSDHeaderData(NCSD_STRUCT *ctx, NCSD_HEADER *header, CARD_INFO_HEADER *card_info, DEV_CARD_INFO_HEADER *dev_card_info);
void PrintNCSDPartitionData(NCSD_STRUCT *ctx, NCSD_HEADER *header, CARD_INFO_HEADER *card_info, DEV_CARD_INFO_HEADER *dev_card_info);
void GetCHIPFullSize(u64 MEDIA_SIZE, int type);
void GetCCIDataSize(u64 CCI_IMAGE_SIZE, int type);
void GetCCIFileStatus(u64 CCI_FILE_SIZE, u8 CCI_FILE_STATUS, int type);
void GetCUPVersion(/*char *FW_STRING, */CARD_INFO_HEADER *card_info);
//int GetPlainRegionStrings(FILE *cxi, u64 plain_region_offset, u64 plain_region_size, char *MiddleWareString[100]);
int GetSDKVersion(FILE *cxi, u64 plain_region_offset, u64 plain_region_size, NCSD_STRUCT *ctx);
