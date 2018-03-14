#include <sys/types.h>
#include <sys/socket.h>
#include <unistd.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <map>
#include <poll.h>
#include <errno.h>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <netdb.h>
#include <errno.h>
#include <iostream>

#ifndef SOCK_NONBLOCK
#include <fcntl.h>
#define SOCK_NONBLOCK O_NONBLOCK
#endif


#define SERVER "142.232.135.38"
#define SOCKET_DATA_WAITING 555
#define SOCKET_NODATA 666


class TCPClient {

public:
	TCPClient();
	int initializeSocket(short port, char * server);
	void sendBytes(char * data, uint32_t len);
	int32_t receiveBytes(char * buffer, uint32_t size);

	void closeConnection();


private:
	int clientSocket;
	sockaddr_in serverAddr;


};
