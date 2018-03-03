using System;
using System.Threading;

namespace connectionExample
{
	unsafe class MainClass
	{
		/* Notes:
		 * - EndPoint.cs (EndPoint Struct) was modified to remove readonly fields
		 * - EndPoint fields are hardcoded (destination IP = 192.168.0.42)
		 * - library file is in /usr/lib, for Unity project it should be in /Assets/Plugins
		 * - EndPoint.cs and Server.cs are both in the same directory as this code
		 * - The EndPoint struct is analogous to a sockaddr_in, which holds the destination for the server,
		 * 		and the source for the client
		 */

		private static Int32 SOCKET_NODATA = 0;
		private static Int32 SOCKET_DATA_WAITING = 1;
		private static IntPtr serverInstance;
		private static bool running = true;


		public static void Main (string[] args)
		{

			//Initializes the server object which holds the networking functions
			serverInstance = Server.Server_CreateServer (); 
			string destIP = "127.0.0.1";
			ushort portNo = 9999;
			EndPoint sendEp = new EndPoint ();
			sendEp.addr = new CAddr (destIP);

			sendEp.port = portNo;

			// max buffer size of 1200 bytes
			byte[] buffer = new byte[1200];

			Int32 error = Server.Server_initServer (serverInstance, portNo);
			Int32 result;

			Thread recvThread = new Thread (recvThrdFunc);
			recvThread.Start ();


			Console.WriteLine ("---Starting Send---");
			for (int i = 0; i < 50; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					buffer [j] = (byte) i;
				}
				fixed (byte* tempBuffer = buffer)
				{
					// sendBytes args: Server, EndPoint, IntPtr, numBytes to write
					Server.Server_sendBytes (serverInstance, sendEp, new IntPtr (tempBuffer), 10);
				}
			}
			Console.WriteLine ("---Completed Sending---");

			System.Threading.Thread.Sleep (5000);
			running = false;


		} //End of Main

		public static void recvThrdFunc()
		{
			// variable storing number of bytes received by a recvBytes call
			Int32 length;
			EndPoint recvEP = new EndPoint ();
			recvEP.port = 9999;
			// max buffer size of 1200 bytes
			byte[] recvBuffer = new byte[1200];
			// result of socket call
			Int32 result;

			while (running)
			{
				// Polls socket to check if there is data waiting
				result = Server.Server_PollSocket (serverInstance);
				if (result == SOCKET_DATA_WAITING)
				{
					fixed(byte* tempBuffer = recvBuffer)
					{
						// reads 10 bytes to the buffer and fills in the EndPoint struct (packet source)
						length = Server.Server_recvBytes (serverInstance, &recvEP, new IntPtr (tempBuffer), 10);
						Console.Write ("Received: ");
						Console.Write(recvBuffer);
						Console.WriteLine ();
					}
				}
			}
		} //End of recvThrdFunc
	} // End of MainClass class
}//End of connectionExample namespace
