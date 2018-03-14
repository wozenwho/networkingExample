using System;
using System.Threading;

namespace Networking
{
	unsafe class client_TCP
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
        private static string destIP                = "192.168.0.13";
        private static ushort PORT_NO               = 9999;
        private static int MAX_BUFFER_SIZE          = 1200;

        private static TCPClient client;
		private static bool running;

        private static Thread recvThread;
        
        // This class has Main by default. If you want to run the server example, un-main this
        //  and main the ServerWrapper NotMain function.
		public static void Main (string[] args)
		{
			Int32 result;
			byte[] buffer = new byte[MAX_BUFFER_SIZE];
			EndPoint ep = new EndPoint (destIP, PORT_NO);
			client = new TCPClient ();
			result = client.Init (ep);
			if (result == 0)
			{
				Console.WriteLine ("Fuck I couldn't init fuck.");
			}

			for (int i = 0; i < MAX_BUFFER_SIZE; i++)
			{
				buffer [i] = (byte)'a';
			}

			client.Send (buffer, MAX_BUFFER_SIZE);
			result = client.Recv (buffer, MAX_BUFFER_SIZE);
			if (result > 0)
			{
				string contents = System.Text.Encoding.UTF8.GetString (buffer);
				Console.WriteLine ("Received: " + contents);
			}


		} //End of Main



		public static void recvThrdFunc()
		{

		} //End of recvThrdFunc
	} // End of MainClass class
}//End of connectionExample namespace
