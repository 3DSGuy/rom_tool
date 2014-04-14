OBJS = utils.o main.o ncsd.o
LIBS = -static-libgcc -static-libstdc++ -lm
CXXFLAGS = -I.
CFLAGS = --std=c99 -Wall -I.
OUTPUT = rom_tool
CC = gcc

main: $(OBJS)
	g++ -o $(OUTPUT) $(LIBS) $(OBJS)
	
clean:
	rm -rf $(OUTPUT) $(OBJS)
	
cleanwin:
	del $(OUTPUT)
	del $(OBJS)
