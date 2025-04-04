using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Vehicles.Modification
{
	// Token: 0x020007CD RID: 1997
	public class VehicleColors : Singleton<VehicleColors>
	{
		// Token: 0x0600369E RID: 13982 RVA: 0x000E5F34 File Offset: 0x000E4134
		public string GetColorName(EVehicleColor c)
		{
			return this.colorLibrary.Find((VehicleColors.VehicleColorData x) => x.color == c).colorName;
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x000E5F6C File Offset: 0x000E416C
		public Color32 GetColorUIColor(EVehicleColor c)
		{
			return this.colorLibrary.Find((VehicleColors.VehicleColorData x) => x.color == c).UIColor;
		}

		// Token: 0x0400278F RID: 10127
		public List<VehicleColors.VehicleColorData> colorLibrary = new List<VehicleColors.VehicleColorData>();

		// Token: 0x020007CE RID: 1998
		[Serializable]
		public class VehicleColorData
		{
			// Token: 0x04002790 RID: 10128
			public EVehicleColor color;

			// Token: 0x04002791 RID: 10129
			public string colorName;

			// Token: 0x04002792 RID: 10130
			public Material material;

			// Token: 0x04002793 RID: 10131
			public Color32 UIColor = Color.white;
		}
	}
}
