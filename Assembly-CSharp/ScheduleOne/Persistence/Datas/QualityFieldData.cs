using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003FB RID: 1019
	[Serializable]
	public class QualityFieldData
	{
		// Token: 0x0600155A RID: 5466 RVA: 0x0005F280 File Offset: 0x0005D480
		public QualityFieldData(EQuality value)
		{
			this.Value = value;
		}

		// Token: 0x040013BF RID: 5055
		public EQuality Value;
	}
}
