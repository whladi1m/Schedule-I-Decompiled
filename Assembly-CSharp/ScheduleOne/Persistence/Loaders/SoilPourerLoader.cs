using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003C4 RID: 964
	public class SoilPourerLoader : GridItemLoader
	{
		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001505 RID: 5381 RVA: 0x0005E7EC File Offset: 0x0005C9EC
		public override string ItemType
		{
			get
			{
				return typeof(SoilPourerData).Name;
			}
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x0005E800 File Offset: 0x0005CA00
		public override void Load(string mainPath)
		{
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			SoilPourerData data = base.GetData<SoilPourerData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load toggleableitem data", null);
				return;
			}
			SoilPourer soilPourer = gridItem as SoilPourer;
			if (soilPourer != null)
			{
				soilPourer.SendSoil(data.SoilID);
			}
		}
	}
}
