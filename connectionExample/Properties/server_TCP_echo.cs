using System;
using System.Threading;

namespace Networking
{
	unsafe class server_TCP_echo
	{
		/* Notes:
		 * Call order: Client/Server.cs -> ServerLibrary.cs -> library.cpp -> server.cpp
         * A simple echo server implemented using our networking library wrapper functions.
         */
		//private static Int32 SOCKET_NODATA          = 0;
		private static Int32 SOCKET_DATA_WAITING    = 1;
        private static ushort PORT_NO               = 9999;
        private static int MAX_BUFFER_SIZE          = 8192;

        private static TCPServer server;
		private static bool running;

        private static Thread listenThread;
        
        // Make this the main function if you want to run the program as a server. 
        // Also un-main the ClientWrapper class' Main function.
		public static void NotMain(string[] args)
		{
			Int32 result;


			TCPServer server = new TCPServer ();
			result = server.Init (PORT_NO);
			// validate server init call
            if (result != 0)
            {
                Console.WriteLine("Failed to initialize socket.");
            }



            listenThread = new Thread(listenThrdFunc);
            running = true;
            listenThread.Start();

            System.Threading.Thread.Sleep(2000);
		} //End of Main

		/*
		 * 
		 * 
		 */
		public static void listenThrdFunc()
		{
			int[] clientArr = new int[30];
			Int32 client;
			Thread[] threadArr = new Thread[30];
			int numClients = 0;
			Int32 result;
            EndPoint ep = new EndPoint();
			byte[] buffer = new byte[MAX_BUFFER_SIZE];

			result = server.AcceptConnection (ref ep);
			if (result != 0)
			{
				client = result;

				result = server.Recv (client, buffer, MAX_BUFFER_SIZE * sizeof(byte));
				if (result <= 0)
				{
					Console.WriteLine ("Fuck. Error Receiving.");
				}
				result = server.Send (client, buffer, MAX_BUFFER_SIZE * sizeof(byte));
				if (result <= 0)
				{
					Console.WriteLine ("Fuck. Error Sending.");
				}
				else
				{
					string contents = System.Text.Encoding.UTF8.GetString (buffer);
					Console.WriteLine ("Received: " + contents);	
				}
			}



//            while (running)
//            {
//				result = server.AcceptConnection (ref ep);
//				if (result != 0)
//				{
//					clientArr [numClients] = result;
//					threadArr [numClients] = new Thread (recvThrdFunc);
//					threadArr [numClients].Start ();
//					numClients++;
//				}
//
//            }
		} //End of listenThrdFunc

//		public static void recvThrdFunc()
//		{
//			byte[] recvBuffer = new byte[MAX_BUFFER_SIZE];
//
//			while (running)
//			{
//				server.Recv ();
//			}
//
//		} //End of recvThrdFunc
	} // End of MainClass class
}//End of connectionExample namespace
