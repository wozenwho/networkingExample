using System;
using System.Threading;

namespace Networking
{
	unsafe class HLServer
	{
		/* Notes:
		 * Call order: Client/Server.cs -> ServerLibrary.cs -> library.cpp -> server.cpp
         * A simple echo server implemented using our networking library wrapper functions.
         */
		private static Int32 SOCKET_NODATA          = 0;
		private static Int32 SOCKET_DATA_WAITING    = 1;
        private static ushort PORT_NO               = 9999;
        private static int MAX_BUFFER_SIZE          = 1200;

        private static Server server;
		private static bool running;

        private static Thread recvThread;
        
        // Make this the main function if you want to run the program as a server. 
        // Also un-main the ClientWrapper class' Main function.
		public static void NotMain(string[] args)
		{
            byte[] sendBuffer = new byte[MAX_BUFFER_SIZE];

            // Create a new Server object
            // You must call the Server's Init method with a port number in order for the server to listen for packets.
            server = new Server();

            // Initializes an EndPoint in the server object, returns 0 on success, -1 on fail. 
            Int32 result = server.Init(PORT_NO);

            if (result != 0)
            {
                Console.WriteLine("Failed to initialize socket.");
            }

            recvThread = new Thread(recvThrdFunc);
            running = true;
            recvThread.Start();

            System.Threading.Thread.Sleep(2000);
		} //End of Main



        // A simple thread function to read data from the EndPoint
        /*
         * NOTE: There might be an issue with Recv and passing a readonly EndPoint, but it has not been observed
         * in testing. 
         */
		public static void recvThrdFunc()
		{
            server = new Server();
            Int32 result = server.Init(PORT_NO);
            byte[] recvBuffer = new byte[MAX_BUFFER_SIZE];
            Int32 numRead;
            EndPoint ep = new EndPoint();

            while (running)
            {
                // If poll returns 1 (SOCKET_DATA_WAITING), there is data waiting to be read
                // If poll returns 0 (SOCKET_NODATA), there is no data waiting to be read
                if (server.Poll() == SOCKET_DATA_WAITING)
                {
                    numRead = server.Recv(&ep, recvBuffer, MAX_BUFFER_SIZE);
                    if (numRead <= 0)
                    {
                        Console.WriteLine("Failed to read from socket.");
                    }
                    else
                    {
                        string contents = System.Text.Encoding.UTF8.GetString(recvBuffer);
                        Console.WriteLine("Read: " + contents);
                        Console.WriteLine("From Endpoint: " + ep.addr.Byte3 + '.' + ep.addr.Byte2 + '.' + ep.addr.Byte1 + '.' + ep.addr.Byte0 + '\n');
                        server.Send(ep, recvBuffer, MAX_BUFFER_SIZE);
                    }
                }
            }
		} //End of recvThrdFunc
	} // End of MainClass class
}//End of connectionExample namespace
