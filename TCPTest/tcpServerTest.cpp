#include "tcpserver.h"
#define MAP_HEADER_SIZE		4
#define MAP_BUFF_SIZE		8000

// Test single client connection, sending 'map data'
int main ()
{
	int32_t	clientSocket;
	int32_t clientArray[MAX_NUM_CLIENTS];
	int32_t port;
    int32_t result;
	EndPoint client;
	int32_t totalBufferSize = MAP_HEADER_SIZE + MAP_BUFF_SIZE;
	char buffer[totalBufferSize];

	TCPServer server;

	result = server.initializeSocket(SERVER_TCP_PORT);
    if (result == -1)
    {
        std::cout << strerror(errno) << std::endl;
    }

	clientSocket = server.acceptConnection(&client);
    if (clientSocket == -1)
    {
        std::cout << strerror(errno) << std::endl;
    }

	buffer[0] = 4096;

	//first char is 'A'
	char j = 65;
	for (size_t i = MAP_HEADER_SIZE; i < totalBufferSize; i++)
	{
		// Reset char back to 'A'
		if (j > 123)
		{
			j = 65;
		}
		buffer[i] = j;
		j++;
	}

	// Print to check bcus i dum
	fprintf(stderr, "%s", buffer);

	while (TRUE)
	{
		server.receiveBytes(clientSocket, buffer, 200);
        std::cout << buffer << std::endl;
		server.sendBytes(clientSocket, buffer, 200);
	}
	//server.sendBytes(clientSocket, buffer, totalBufferSize);

	close(clientSocket);
	return(0);
}
