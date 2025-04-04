using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Management
{
	// Token: 0x02000577 RID: 1399
	public class ManagementUtilities : Singleton<ManagementUtilities>
	{
		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06002309 RID: 8969 RVA: 0x0008F844 File Offset: 0x0008DA44
		public static List<string> WeedSeedAssetPaths
		{
			get
			{
				return Singleton<ManagementUtilities>.Instance.weedSeedAssetPaths;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x0600230A RID: 8970 RVA: 0x0008F850 File Offset: 0x0008DA50
		public static List<string> AdditiveAssetPaths
		{
			get
			{
				return Singleton<ManagementUtilities>.Instance.additiveAssetPaths;
			}
		}

		// Token: 0x04001A3D RID: 6717
		public List<string> weedSeedAssetPaths = new List<string>();

		// Token: 0x04001A3E RID: 6718
		public List<string> additiveAssetPaths = new List<string>();

		// Token: 0x04001A3F RID: 6719
		public List<AdditiveDefinition> AdditiveDefinitions = new List<AdditiveDefinition>();
	}
}
