#ifndef _UTILS_H_
#define _UTILS_H_
typedef struct
{
	char *argument;
	u16 arg_len;
} __attribute__((__packed__)) 
OPTION_CTX;

typedef struct
{
	u8 *buffer;
	u64 size;
} __attribute__((__packed__)) 
COMPONENT_STRUCT;
#endif

//MISC
void char_to_int_array(unsigned char destination[], char source[], int size, int endianness, int base);
void endian_memcpy(u8 *destination, u8 *source, u32 size, int endianness);
void u8_hex_print_be(u8 *array, int len);
void u8_hex_print_le(u8 *array, int len);
u32 align_value(u32 value, u32 alignment);
void resolve_flag(unsigned char flag, unsigned char *flag_bool);
void resolve_flag_u16(u16 flag, unsigned char *flag_bool);
int append_filextention(char *output, u16 max_outlen, char *input, char extention[]); 
//IO Related
int ExportFileToFile(FILE *in, FILE *out, u64 size, u64 in_offset, u64 out_offset);
void WriteBuffer(void *buffer, u64 size, u64 offset, FILE *output);
void ReadFile_64(void *outbuff, u64 size, u64 offset, FILE *file);
u64 GetFileSize_u64(char *filename);
u32 GetFileSize_u32(FILE *file);
int TruncateFile_u64(char *filename, u64 filelen);
int fseek_64(FILE *fp, u64 file_pos, int whence);
int makedir(const char* dir);
char *getcwdir(char *buffer,int maxlen);
void _free(void *ptr); // frees and nullifies pointer
//Data Size conversion
u16 u8_to_u16(u8 *value, u8 endianness);
u32 u8_to_u32(u8 *value, u8 endianness);
u64 u8_to_u64(u8 *value, u8 endianness);
int u16_to_u8(u8 *out_value, u16 in_value, u8 endianness);
int u32_to_u8(u8 *out_value, u32 in_value, u8 endianness);
int u64_to_u8(u8 *out_value, u64 in_value, u8 endianness);
//from ctrtool
void memdump(FILE* fout, const char* prefix, const u8* data, u32 size);

