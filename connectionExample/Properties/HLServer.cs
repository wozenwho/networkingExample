using System;
using System.Threading;

namespace Networking
{
	unsafe class HLServer
	{
		/* Notes:
		 * Call order: Client/Server.cs -> ServerLibrary.cs -> library.cpp -> server.cpp
         * A simple echo server implemented using our networking library wrapper functions.
         * 
         * 
         * POTENTIAL ISSUE WITH RECEIVE, ENDPOINT IS READ ONLY, CANNOT ASSIGN NEW VALUES.
         */
		private static Int32 SOCKET_NODATA          = 0;
		private static Int32 SOCKET_DATA_WAITING    = 1;
        private static ushort PORT_NO               = 9999;
        private static int MAX_BUFFER_SIZE          = 1200;

        private static Server server;
		private static IntPtr serverInstance;
		private static bool running;

        private static Thread recvThread;
        

		public static void Main (string[] args)
		{
            server = new Server();
            byte[] sendBuffer = new byte[MAX_BUFFER_SIZE];

            // Initializes an EndPoint in the client object, returns 0 on success, -1 on fail. 
            Int32 result = server.Init(PORT_NO);
            if (result != 0)
            {
                Console.WriteLine("Failed to initialize socket.");
            }

            recvThread = new Thread(recvThrdFunc);
            running = true;
            recvThread.Start();

		} //End of Main

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
                if (client.Poll() == SOCKET_DATA_WAITING)
                {
                    numRead = client.Recv(&ep, recvBuffer, MAX_BUFFER_SIZE);
                    if (numRead <= 0)
                    {
                        Console.WriteLine("Failed to read from socket.");
                    }
                    else
                    {
                        Console.WriteLine("Read: " + recvBuffer);
                        // Console.WriteLine(ep.CAddr);
                        server.Send(ep, recvBuffer, MAX_BUFFER_SIZE);
                    }
                }

            }
		} //End of recvThrdFunc
	} // End of MainClass class
}//End of connectionExample namespace
