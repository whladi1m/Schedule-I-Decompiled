using System;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.HandheldBin
{
	// Token: 0x02000BCE RID: 3022
	public class StoredItem_Bin : StoredItem
	{
		// Token: 0x04003EE3 RID: 16099
		[Header("References")]
		public HandheldBin_Functional bin;
	}
}
