using System;
using System.Collections.Generic;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;

namespace ScheduleOne.Persistence
{
	// Token: 0x0200035F RID: 863
	public class GenericSaveablesManager : Singleton<GenericSaveablesManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x0005724B File Offset: 0x0005544B
		public string SaveFolderName
		{
			get
			{
				return "GenericSaveables";
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x0005724B File Offset: 0x0005544B
		public string SaveFileName
		{
			get
			{
				return "GenericSaveables";
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001381 RID: 4993 RVA: 0x00057252 File Offset: 0x00055452
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001382 RID: 4994 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001383 RID: 4995 RVA: 0x0005725A File Offset: 0x0005545A
		// (set) Token: 0x06001384 RID: 4996 RVA: 0x00057262 File Offset: 0x00055462
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x0005726B File Offset: 0x0005546B
		// (set) Token: 0x06001386 RID: 4998 RVA: 0x00057273 File Offset: 0x00055473
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x0005727C File Offset: 0x0005547C
		// (set) Token: 0x06001388 RID: 5000 RVA: 0x00057284 File Offset: 0x00055484
		public bool HasChanged { get; set; }

		// Token: 0x06001389 RID: 5001 RVA: 0x0005728D File Offset: 0x0005548D
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0005729B File Offset: 0x0005549B
		public void RegisterSaveable(IGenericSaveable saveable)
		{
			if (this.Saveables.Contains(saveable))
			{
				return;
			}
			this.Saveables.Add(saveable);
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0003CD29 File Offset: 0x0003AF29
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x000572B8 File Offset: 0x000554B8
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			for (int i = 0; i < this.Saveables.Count; i++)
			{
				if (this.Saveables[i] != null)
				{
					string json = this.Saveables[i].GetSaveData().GetJson(true);
					string text = this.Saveables[i].GUID.ToString().Substring(0, 6) + ".json";
					list.Add(text);
					string text2 = Path.Combine(containerFolder, text);
					try
					{
						File.WriteAllText(text2, json);
					}
					catch (Exception ex)
					{
						Console.LogWarning("Failed to write generic saveable file: " + text2 + " - " + ex.Message, null);
					}
				}
			}
			return list;
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0005739C File Offset: 0x0005559C
		public void LoadSaveable(GenericSaveData data)
		{
			if (!GUIDManager.IsGUIDValid(data.GUID))
			{
				Console.LogWarning("Invalid GUID found in generic save data: " + data.GUID, null);
				return;
			}
			Guid guid = new Guid(data.GUID);
			IGenericSaveable genericSaveable = this.Saveables.Find((IGenericSaveable x) => x.GUID == guid);
			if (genericSaveable == null)
			{
				string str = "No saveable found with GUID: ";
				Guid guid2 = guid;
				Console.LogWarning(str + guid2.ToString(), null);
				return;
			}
			genericSaveable.Load(data);
		}

		// Token: 0x040012A6 RID: 4774
		protected List<IGenericSaveable> Saveables = new List<IGenericSaveable>();

		// Token: 0x040012A7 RID: 4775
		private GenericSaveablesLoader loader = new GenericSaveablesLoader();
	}
}
