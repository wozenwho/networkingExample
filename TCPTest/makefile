CC = g++
FLAGS = -Wall -ggdb


clean:
	rm -f *.o *.bak tcpServerTest

tcpserver.o: tcpserver.cpp
	$(CC) $(FLAGS) -c -O tcpserver.cpp

tcpservertest.o: tcpServerTest.cpp
	$(CC) $(FLAGS) -c -O tcpServerTest.cpp -o tcpservertest.o

tcpclient.o: tcpclient.cpp
	$(CC) $(FLAGS) -c -O tcpclient.cpp -o tcpclient.o

tcpclienttest.o: tcpClientTest.cpp
	$(CC) $(FLAGS) -c -O tcpClientTest.cpp -o tcpclienttest.o


serverTest: tcpserver.o tcpservertest.o
	$(CC) $(FLAGS) tcpserver.o tcpservertest.o -o serverTest

clientTest: tcpclient.o tcpclienttest.o
	$(CC) $(FLAGS) tcpclient.o tcpclienttest.o -o clientTest
