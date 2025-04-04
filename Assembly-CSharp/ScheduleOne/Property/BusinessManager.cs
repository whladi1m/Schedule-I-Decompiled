using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;

namespace ScheduleOne.Property
{
	// Token: 0x020007F6 RID: 2038
	public class BusinessManager : Singleton<BusinessManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x0600376A RID: 14186 RVA: 0x000EB26B File Offset: 0x000E946B
		public string SaveFolderName
		{
			get
			{
				return "Businesses";
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x0600376B RID: 14187 RVA: 0x000EB26B File Offset: 0x000E946B
		public string SaveFileName
		{
			get
			{
				return "Businesses";
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x0600376C RID: 14188 RVA: 0x000EB272 File Offset: 0x000E9472
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x0600376D RID: 14189 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x0600376E RID: 14190 RVA: 0x000EB27A File Offset: 0x000E947A
		// (set) Token: 0x0600376F RID: 14191 RVA: 0x000EB282 File Offset: 0x000E9482
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06003770 RID: 14192 RVA: 0x000EB28B File Offset: 0x000E948B
		// (set) Token: 0x06003771 RID: 14193 RVA: 0x000EB293 File Offset: 0x000E9493
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06003772 RID: 14194 RVA: 0x000EB29C File Offset: 0x000E949C
		// (set) Token: 0x06003773 RID: 14195 RVA: 0x000EB2A4 File Offset: 0x000E94A4
		public bool HasChanged { get; set; }

		// Token: 0x06003774 RID: 14196 RVA: 0x000EB2AD File Offset: 0x000E94AD
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x0003CD29 File Offset: 0x0003AF29
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x000EB2BC File Offset: 0x000E94BC
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			for (int i = 0; i < Business.UnownedBusinesses.Count; i++)
			{
				new SaveRequest(Business.UnownedBusinesses[i], containerFolder);
				list.Add(Business.UnownedBusinesses[i].SaveFolderName);
			}
			for (int j = 0; j < Business.OwnedBusinesses.Count; j++)
			{
				new SaveRequest(Business.OwnedBusinesses[j], containerFolder);
				list.Add(Business.OwnedBusinesses[j].SaveFolderName);
			}
			return list;
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x000EB354 File Offset: 0x000E9554
		public void LoadBusiness(BusinessData businessData, string containerPath)
		{
			Business business = Business.Businesses.FirstOrDefault((Business p) => p.PropertyCode == businessData.PropertyCode);
			if (business == null)
			{
				business = Business.Businesses.FirstOrDefault((Business p) => p.PropertyCode == businessData.PropertyCode);
			}
			if (business == null)
			{
				Debug.LogWarning("Business not found: " + businessData.PropertyCode);
				return;
			}
			business.Load(businessData, containerPath);
		}

		// Token: 0x04002881 RID: 10369
		private BusinessesLoader loader = new BusinessesLoader();
	}
}
