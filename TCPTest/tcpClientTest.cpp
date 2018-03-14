#include "tcpclient.h"
#define MAP_BUFFER_SIZE         8192
#define MAP_HEADER_SIZE         4

// Receive test for map data
int main() {
    TCPClient client;
	int result;
	char buffer[MAP_BUFFER_SIZE];
    char writeBuffer[MAP_BUFFER_SIZE];
    char headerBuf[MAP_HEADER_SIZE];
    int numSent  = 0;
    int numRecv = 0;
    int running = 1;

	result = client.initializeSocket(9999, (char *)"192.168.0.13");
    if (result == -1)
        std::cout << strerror(errno) << std::endl;
    else
    {
        std::cout << "Connection success." << std::endl;
    }

    /*
    Uncomment this segment of code to test a single send/recv, the while loop is useless.
    */
    // while (running)
    // {
    //     std::cin >> writeBuffer;
    //     numSent = client.sendBytes(writeBuffer, MAP_BUFFER_SIZE);
    //     if (numSent == 0)
    //     {
    //         std::cout << "Sent: " << numSent << " bytes." << std::endl;
    //     }
    //     numRecv = client.receiveBytes(buffer, sizeof(buffer));
    //     std::cout << "Received: " << buffer << std::endl;
    // }

    numRecv = client.receiveBytes(buffer, sizeof(buffer));
    if (numRecv == -1)
    {
        std::cout << strerror(errno) << std::endl;
    }
    else
    {
        std::cout << "Received: " << buffer << std::endl;
    }

	// if(numRecv == 0)
	// 	printf("%s\n", buffer);
	//}
	return 1;
}
