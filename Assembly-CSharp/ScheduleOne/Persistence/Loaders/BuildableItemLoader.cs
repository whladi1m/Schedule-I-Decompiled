using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003B3 RID: 947
	public class BuildableItemLoader : Loader
	{
		// Token: 0x170003DA RID: 986
		// (get) Token: 0x060014D5 RID: 5333 RVA: 0x0005D709 File Offset: 0x0005B909
		public virtual string ItemType
		{
			get
			{
				return typeof(BuildableItemData).Name;
			}
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0005D71A File Offset: 0x0005B91A
		public BuildableItemLoader()
		{
			Singleton<LoadManager>.Instance.ObjectLoaders.Add(this);
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0005D734 File Offset: 0x0005B934
		public override void Load(string mainPath)
		{
			BuildableItemData buildableItemData = this.GetBuildableItemData(mainPath);
			if (buildableItemData != null)
			{
				BuildableItemLoader objectLoader = Singleton<LoadManager>.Instance.GetObjectLoader(buildableItemData.DataType);
				if (objectLoader != null)
				{
					new LoadRequest(mainPath, objectLoader);
				}
			}
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0005D768 File Offset: 0x0005B968
		public BuildableItemData GetBuildableItemData(string mainPath)
		{
			return this.GetData<BuildableItemData>(mainPath);
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0005D774 File Offset: 0x0005B974
		protected T GetData<T>(string mainPath) where T : BuildableItemData
		{
			string json;
			if (base.TryLoadFile(mainPath, "Data", out json))
			{
				T result = default(T);
				try
				{
					result = JsonUtility.FromJson<T>(json);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				return result;
			}
			return default(T);
		}
	}
}
