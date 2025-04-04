using System;
using FishNet.Utility.Performance;

namespace FishySteamworks
{
	// Token: 0x02000C11 RID: 3089
	internal struct LocalPacket
	{
		// Token: 0x06005656 RID: 22102 RVA: 0x0016A408 File Offset: 0x00168608
		public LocalPacket(ArraySegment<byte> data, byte channel)
		{
			this.Data = ByteArrayPool.Retrieve(data.Count);
			this.Length = data.Count;
			Buffer.BlockCopy(data.Array, data.Offset, this.Data, 0, this.Length);
			this.Channel = channel;
		}

		// Token: 0x04004037 RID: 16439
		public byte[] Data;

		// Token: 0x04004038 RID: 16440
		public int Length;

		// Token: 0x04004039 RID: 16441
		public byte Channel;
	}
}
