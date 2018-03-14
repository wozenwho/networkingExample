#include "tcpclient.h"
#define MAP_BUFFER_SIZE         4096
#define MAP_HEADER_SIZE         4

// Receive test for map data
int main() {
    TCPClient client;
	int result;
    int numRecv = 0;
    char temp[] = "Hello\n";
	char buffer[MAP_BUFFER_SIZE];
    char headerBuf[MAP_HEADER_SIZE];
    int count  = 0;

	result = client.initializeSocket(9999, (char *)"142.232.18.92");
    if (result == -1)
        std::cout << strerror(errno) << std::endl;

    client.receiveBytes(headerBuf, sizeof(headerBuf));


	//while (true) {
	numRecv = client.receiveBytes(buffer, sizeof(buffer));

	if(numRecv == 0)
		printf("%s\n", buffer);
	//}
	return 1;
}
