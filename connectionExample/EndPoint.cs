using System;
using System.Runtime.InteropServices;

namespace connectionExample
{
	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	public struct CAddr{

		//public static CAddr Loopback = new CAddr("127.0.0.1");

		[FieldOffset(0)]
		public uint Packet;
		[FieldOffset(0)]
		public byte Byte0;
		[FieldOffset(1)]
		public byte Byte1;
		[FieldOffset(2)]
		public byte Byte2;
		[FieldOffset(3)]
		public byte Byte3;


		public CAddr (string ip) {
			string[] parts = ip.Split('.');
			Packet = 0;
			Byte0 = byte.Parse(parts[3]);
			Byte1 = byte.Parse(parts[2]);
			Byte2 = byte.Parse(parts[1]);
			Byte3 = byte.Parse(parts[0]);
		}

		public CAddr (byte a, byte b, byte c, byte d) {
			Packet = 0;
			Byte0 = d;
			Byte1 = c;
			Byte2 = b;
			Byte3 = a;
		}

	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct EndPoint {
		public CAddr addr;
		public ushort port;
	}
}

