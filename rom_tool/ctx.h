typedef enum
{
	NCCH_MAGIC = 0x4E434348,
	NCSD_MAGIC = 0x4E435344
} file_magic;

typedef enum
{
	KB = 1024,
	MB = 1048576,
	GB = 1073741824
} file_size;

typedef enum
{
	_unknown = 0,
	CXI,
	CFA_Simple,
	CFA_Manual,
	CFA_DLPChild,
	CFA_Update
} ncch_types;

typedef enum
{
	IS_MALFORMED = 0,
	IS_FULL,
	IS_TRIM,
	IS_S_TRIM,
} rom_status;

typedef enum
{
	info = 0,
	part_info,
	restore,
	trim,
	remove_update_partition,
	extract,
} flag_index;

//Variable Structs

typedef struct
{
	int active;
	char product_code[0x10];
	u8 content_type;
	u8 fs_type;
	u8 crypto_type;
	u8 ncch_crypto_key;
	u32 offset;
	u32 size;
	u64 title_id;
} PARTITION_DATA;

typedef struct
{
	int type;
	u8 signature[0x100];
	u64 MEDIA_UNIT_SIZE;	
	
	// Sizes
	u64 MEDIA_SIZE;
	u64 CCI_IMAGE_SIZE;
	u64 CCI_S_TRIM_SIZE;
	
	// CCI File Status
	u64 CCI_FILE_SIZE;
	u8 CCI_FILE_STATUS;
	//u8 md5_hash[16];
	
	// SDK Details
	int BUILD_TYPE;
	u8 FW_VER[3];
	u8 SDK_VER[3];
	char SDK_PATCH[100];
	
	u64 WRITABLE_ADDRESS;
	u64 CARD2_MAX_SAVEDATA_SIZE;
	
	// CCI Partition Records
	u8 partition_count;
	PARTITION_DATA partition_data[8];
} NCSD_STRUCT;

typedef struct
{		
	//Input Info
	OPTION_CTX outfile;
	OPTION_CTX rw_dumpfile;
	OPTION_CTX cci_file;
	
	//NCSD Data
	u8 ncsd_struct_malloc_flag;
	NCSD_STRUCT *ncsd_struct;
	
	//Settings
	u8 flags[6];
} __attribute__((__packed__)) 
CCI_CONTEXT;
