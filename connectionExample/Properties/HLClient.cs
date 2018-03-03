using System;
using System.Threading;

namespace Networking
{
	unsafe class HLClient
	{
		/* Notes:
		 * Call order: Client/Server.cs -> ServerLibrary.cs -> library.cpp -> server.cpp
		 * 
		 * 1) The library (libNetwork.so) is placed in the project folder:
		 *      connectionExample/bin/Debug
		 *     - for Unity, the library should be placed in /Assets/Plugins
         */


		private static Int32 SOCKET_NODATA          = 0;
		private static Int32 SOCKET_DATA_WAITING    = 1;
        private static string destIP                = "142.232.18.12";
        private static ushort PORT_NO               = 9999;
        private static int MAX_BUFFER_SIZE          = 1200;

        private static Client client;
		private static bool running;

        private static Thread recvThread;
        
        // This class has Main by default. If you want to run the server example, un-main this
        //  and main the ServerWrapper NotMain function.
		public static void Main (string[] args)
		{
            byte[] sendBuffer = new byte[MAX_BUFFER_SIZE];

            // Creates a new client object. 
            // To use it, you must call the Client's Init function with a destination IP (server) and a port number.
            client = new Client();

            // Initializes an EndPoint in the client object, returns 0 on success, -1 on fail. 
            Int32 result = client.Init(destIP, PORT_NO);

            if (result != 0)
            {
                Console.WriteLine("Failed to initialize socket.");
            }

            recvThread = new Thread(recvThrdFunc);
            running = true;
            recvThread.Start();

            Console.WriteLine("Sending to EndPoint: " + destIP);
            Int32 numSent;

            // Sends 0 - z to the EndPoint.
            for (int i = 48; i < 123; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    sendBuffer[j] = (byte) i;
                }
                // Send returns the number of bytes written
                numSent = client.Send(sendBuffer, i + 1);
                if (numSent <= 0)
                {
                    Console.WriteLine("Failed to write to socket.");
                }
            }

            System.Threading.Thread.Sleep(2000);
            running = false;

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
                    // Client.Recv returns number of read bytes.
                    numRead = client.Recv(recvBuffer, MAX_BUFFER_SIZE);
                    if (numRead <= 0)
                    {
                        // Error if there is data waiting and we fail to read.
                        Console.WriteLine("Failed to read from socket.");
                    }
                    else
                    {
                        // Converts contents of recvBuffer to a string and print to console
                        string contents = System.Text.Encoding.UTF8.GetString(recvBuffer);
                        Console.WriteLine("Read: " + contents);
                    }
                }

            }
		} //End of recvThrdFunc
	} // End of MainClass class
}//End of connectionExample namespace
