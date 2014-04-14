#include "lib.h"
#include "main.h"
#include "ncsd.h"

typedef enum
{
	MAJOR = 3,
	MINOR = 2
} AppVer;

void app_title(void);
void help(char *app_name);

int main(int argc, char *argv[])
{	
	//Filter Out Bad number of arguments
	if (argc < 2){
		printf("[!] Must Specify Arguments\n");
		help(argv[0]);
		return ARGC_FAIL;
	}
	
	CCI_CONTEXT *ctx = malloc(sizeof(CCI_CONTEXT));
	memset(ctx,0x0,sizeof(CCI_CONTEXT));
		
	for(int i = 1; i < argc; i++){
		if(strcmp(argv[i], "-h") == 0 || strcmp(argv[i], "--help") == 0){
			help(argv[0]);
			free_buffers(ctx);
			return ARGC_FAIL;
		}
		else if(strcmp(argv[i], "-i") == 0 || strcmp(argv[i], "--info") == 0)
			ctx->flags[info] = True;
		else if(strcmp(argv[i], "-p") == 0 || strcmp(argv[i], "--partition_info") == 0)
			ctx->flags[part_info] = True;
		else if(strcmp(argv[i], "-r") == 0 || strcmp(argv[i], "--restore") == 0)
			ctx->flags[restore] = True;
		else if(strcmp(argv[i], "-t") == 0 || strcmp(argv[i], "--trim") == 0)
			ctx->flags[trim] = True;
		else if(strcmp(argv[i], "-u") == 0 || strcmp(argv[i], "--remove_update") == 0){
			ctx->flags[trim] = True;
			ctx->flags[remove_update_partition] = True;
		}
		else if(strcmp(argv[i], "-x") == 0 && ctx->flags[extract] == False && i+1 < (argc - 1)){
			ctx->flags[extract] = True;
			ctx->outfile.arg_len = strlen(argv[i+1]);
			ctx->outfile.argument = malloc(ctx->outfile.arg_len);
			if(ctx->outfile.argument == NULL){
				printf("[!] MEM ERROR\n");
				return Fail;
			}
			memcpy(ctx->outfile.argument,argv[i+1],ctx->outfile.arg_len+1);
		}
		else if(strncmp(argv[i], "--extract=",10) == 0 && ctx->flags[extract] == False){
			ctx->flags[extract] = True;
			ctx->outfile.arg_len = strlen(argv[i]+10);
			ctx->outfile.argument = malloc(ctx->outfile.arg_len);
			if(ctx->outfile.argument == NULL){
				printf("[!] MEM ERROR\n");
				return Fail;
			}
			memcpy(ctx->outfile.argument,argv[i]+10,ctx->outfile.arg_len+1);
		}
		else if(i == argc-1){
			ctx->cci_file.arg_len = strlen(argv[i]);
			ctx->cci_file.argument = malloc(ctx->cci_file.arg_len+1);
			if(ctx->cci_file.argument == NULL){
				printf("[!] MEM ERROR\n");
				return Fail;
			}
			memcpy(ctx->cci_file.argument,argv[i],ctx->cci_file.arg_len+1);
			FILE *cci = fopen(ctx->cci_file.argument,"rb");
			if(cci == NULL){
				printf("[!] Failed to open '%s'\n",ctx->cci_file.argument);
				return 1;
			}
			fclose(cci);
		}
	}
	
	if(ctx->flags[restore] == True && ctx->flags[trim] == True){
		printf("[!] You cannot trim and restore a CCI at the same time\n");
		help(argv[0]);
		free_buffers(ctx);
		return 1;
	}
	
	int Action = 0;
	for(int i = 0; i < 6; i++){
		if(ctx->flags[i] == True)
			Action++;
	}
	if(!Action){
		printf("[!] Nothing To Do\n");
		help(argv[0]);
		free_buffers(ctx);
		return 1;
	}

	if(NCSDProcess(ctx) != 0)
		goto fail_cleanup;
	
	printf("[*] Completed Successfully\n");
	free_buffers(ctx);
	return 0;
fail_cleanup:
	printf("[!] Failed\n");
	free_buffers(ctx);
	return 1;
}

void free_buffers(CCI_CONTEXT *ctx)
{	
	//Freeing Arguments
	if(ctx->cci_file.arg_len)
		free(ctx->cci_file.argument);
		
	if(ctx->outfile.arg_len)
		free(ctx->outfile.argument);
		
	if(ctx->rw_dumpfile.arg_len > 0)
		free(ctx->rw_dumpfile.argument);
	
	//Freeing CCI Data buffers
	if(ctx->ncsd_struct_malloc_flag)
		free(ctx->ncsd_struct);
	
	//Freeing Main context
	free(ctx);
}

void app_title(void)
{
	printf("CTR Card Image Tool\n");
	printf("Version %d.%d (C) 3DSGuy 2013\n",MAJOR,MINOR);
}

void help(char *app_name)
{
	app_title();
	printf("Usage: %s [options] <cci filepath>\n", app_name);
	printf("OPTIONS            Possible Values         Explanation\n");
	printf(" -h, --help                                Print this help.\n");
	printf(" -i, --info                                Display CCI info\n");
	printf(" -p, --partition_info                      Display CCI partitions\n");
	printf(" -r, --restore                             Restore unused bytes to CCI file.\n");
	printf(" -t, --trim                                Remove unused bytes from CCI file.\n");
	printf(" -u, --remove_update                       Remove update data\n");
	printf(" -x, --extract=    Dir-out                 Extract CCI partitions to directory\n");
}
