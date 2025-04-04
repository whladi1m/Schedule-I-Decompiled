using System;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003BA RID: 954
	public class GridItemLoader : BuildableItemLoader
	{
		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060014E9 RID: 5353 RVA: 0x0005DDDE File Offset: 0x0005BFDE
		public override string ItemType
		{
			get
			{
				return typeof(GridItemData).Name;
			}
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0005DDF7 File Offset: 0x0005BFF7
		public override void Load(string mainPath)
		{
			this.LoadAndCreate(mainPath);
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0005DE04 File Offset: 0x0005C004
		protected GridItem LoadAndCreate(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, "Data", out json))
			{
				GridItemData gridItemData = null;
				try
				{
					gridItemData = JsonUtility.FromJson<GridItemData>(json);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (gridItemData != null)
				{
					ItemInstance itemInstance = ItemDeserializer.LoadItem(gridItemData.ItemString);
					if (itemInstance == null)
					{
						return null;
					}
					Grid @object = GUIDManager.GetObject<Grid>(new Guid(gridItemData.GridGUID));
					if (@object == null)
					{
						Console.LogWarning("Failed to find grid for " + gridItemData.GridGUID, null);
						return null;
					}
					return Singleton<BuildManager>.Instance.CreateGridItem(itemInstance, @object, gridItemData.OriginCoordinate, gridItemData.Rotation, gridItemData.GUID);
				}
			}
			return null;
		}
	}
}
