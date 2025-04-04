using System;
using System.Collections.Generic;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006D4 RID: 1748
	public class MetadataManager : Singleton<MetadataManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002F8F RID: 12175 RVA: 0x000C63F4 File Offset: 0x000C45F4
		// (set) Token: 0x06002F90 RID: 12176 RVA: 0x000C63FC File Offset: 0x000C45FC
		public DateTime CreationDate { get; protected set; }

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002F91 RID: 12177 RVA: 0x000C6405 File Offset: 0x000C4605
		// (set) Token: 0x06002F92 RID: 12178 RVA: 0x000C640D File Offset: 0x000C460D
		public string CreationVersion { get; protected set; } = string.Empty;

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002F93 RID: 12179 RVA: 0x000C6416 File Offset: 0x000C4616
		public string SaveFolderName
		{
			get
			{
				return "Metadata";
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002F94 RID: 12180 RVA: 0x000C6416 File Offset: 0x000C4616
		public string SaveFileName
		{
			get
			{
				return "Metadata";
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002F95 RID: 12181 RVA: 0x000C641D File Offset: 0x000C461D
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002F96 RID: 12182 RVA: 0x00014002 File Offset: 0x00012202
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002F97 RID: 12183 RVA: 0x000C6425 File Offset: 0x000C4625
		// (set) Token: 0x06002F98 RID: 12184 RVA: 0x000C642D File Offset: 0x000C462D
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002F99 RID: 12185 RVA: 0x000C6436 File Offset: 0x000C4636
		// (set) Token: 0x06002F9A RID: 12186 RVA: 0x000C643E File Offset: 0x000C463E
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002F9B RID: 12187 RVA: 0x000C6447 File Offset: 0x000C4647
		// (set) Token: 0x06002F9C RID: 12188 RVA: 0x000C644F File Offset: 0x000C464F
		public bool HasChanged { get; set; }

		// Token: 0x06002F9D RID: 12189 RVA: 0x000C6458 File Offset: 0x000C4658
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
			if (this.CreationVersion == string.Empty)
			{
				this.CreationVersion = Application.version;
			}
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x000C6484 File Offset: 0x000C4684
		public virtual string GetSaveString()
		{
			DateTime now = DateTime.Now;
			return new MetaData(new DateTimeData(this.CreationDate), new DateTimeData(now), this.CreationVersion, Application.version, false).GetJson(true);
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x000C64BF File Offset: 0x000C46BF
		public void Load(MetaData data)
		{
			this.CreationDate = data.CreationDate.GetDateTime();
			this.CreationVersion = data.CreationVersion;
			this.HasChanged = true;
		}

		// Token: 0x040021F5 RID: 8693
		private MetadataLoader loader = new MetadataLoader();
	}
}
