using System;
using System.Threading;

namespace Networking
{
	unsafe class HLClient
	{
		/* Notes:
		 * Call order: Client/Server.cs -> ServerLibrary.cs -> library.cpp -> server.cpp
         */


		private static Int32 SOCKET_NODATA          = 0;
		private static Int32 SOCKET_DATA_WAITING    = 1;
        private static string destIP                = "192.168.0.42";
        private static ushort PORT_NO               = 9999;
        private static int MAX_BUFFER_SIZE          = 1200;

        private static Client client;
		private static IntPtr serverInstance;
		private static bool running;

        private static Thread recvThread;
        

		public static void Main (string[] args)
		{
            client = new Client();
            byte[] sendBuffer = new byte[MAX_BUFFER_SIZE];

            // Initializes an EndPoint in the client object, returns 0 on success, -1 on fail. 
            Int32 result = client.Init(destIP, PORT_NO);
            if (result != 0)
            {
                Console.WriteLine("Failed to initialize socket.");
            }

            recvThread = new Thread(recvThrdFunc);
            running = true;
            recvThread.Start();

            Int32 numRead;
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    sendBuffer[j] = (byte) i;
                }
                // Send returns the number of bytes written
                numRead = client.Send(sendBuffer, i + 1);
                if (numRead <= 0)
                {
                    Console.WriteLine("Failed to write to socket.");
                }
            }

		} //End of Main

		public static void recvThrdFunc()
		{
            byte[] recvBuffer = new byte[MAX_BUFFER_SIZE];
            Int32 numRead;

            while (running)
            {
                // If poll returns 1 (SOCKET_DATA_WAITING), there is data waiting to be read
                // If poll returns 0 (SOCKET_NODATA), there is no data waiting to be read
                if (client.Poll() == SOCKET_DATA_WAITING)
                {
                    numRead = client.Recv(recvBuffer, MAX_BUFFER_SIZE);
                    if (numRead <= 0)
                    {
                        Console.WriteLine("Failed to read from socket.");
                    }
                    else
                    {
                        Console.WriteLine("Read: " + recvBuffer);
                    }
                }

            }
		} //End of recvThrdFunc
	} // End of MainClass class
}//End of connectionExample namespace
