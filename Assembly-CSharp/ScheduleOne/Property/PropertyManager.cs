using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;

namespace ScheduleOne.Property
{
	// Token: 0x02000802 RID: 2050
	public class PropertyManager : Singleton<PropertyManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060037E3 RID: 14307 RVA: 0x000ECABC File Offset: 0x000EACBC
		public string SaveFolderName
		{
			get
			{
				return "Properties";
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x060037E4 RID: 14308 RVA: 0x000ECABC File Offset: 0x000EACBC
		public string SaveFileName
		{
			get
			{
				return "Properties";
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x060037E5 RID: 14309 RVA: 0x000ECAC3 File Offset: 0x000EACC3
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060037E6 RID: 14310 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060037E7 RID: 14311 RVA: 0x000ECACB File Offset: 0x000EACCB
		// (set) Token: 0x060037E8 RID: 14312 RVA: 0x000ECAD3 File Offset: 0x000EACD3
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060037E9 RID: 14313 RVA: 0x000ECADC File Offset: 0x000EACDC
		// (set) Token: 0x060037EA RID: 14314 RVA: 0x000ECAE4 File Offset: 0x000EACE4
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060037EB RID: 14315 RVA: 0x000ECAED File Offset: 0x000EACED
		// (set) Token: 0x060037EC RID: 14316 RVA: 0x000ECAF5 File Offset: 0x000EACF5
		public bool HasChanged { get; set; }

		// Token: 0x060037ED RID: 14317 RVA: 0x000ECAFE File Offset: 0x000EACFE
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x0003CD29 File Offset: 0x0003AF29
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x000ECB0C File Offset: 0x000EAD0C
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			for (int i = 0; i < Property.OwnedProperties.Count; i++)
			{
				try
				{
					if (Property.OwnedProperties[i].ShouldSave())
					{
						new SaveRequest(Property.OwnedProperties[i], containerFolder);
						list.Add(Property.OwnedProperties[i].SaveFolderName);
					}
				}
				catch (Exception ex)
				{
					Console.LogError("Error saving property: " + Property.OwnedProperties[i].PropertyCode + " - " + ex.Message, null);
					SaveManager.ReportSaveError();
				}
			}
			for (int j = 0; j < Property.UnownedProperties.Count; j++)
			{
				try
				{
					if (Property.UnownedProperties[j].ShouldSave())
					{
						new SaveRequest(Property.UnownedProperties[j], containerFolder);
						list.Add(Property.UnownedProperties[j].SaveFolderName);
					}
				}
				catch (Exception ex2)
				{
					Console.LogError("Error saving property: " + Property.OwnedProperties[j].PropertyCode + " - " + ex2.Message, null);
					SaveManager.ReportSaveError();
				}
			}
			return list;
		}

		// Token: 0x060037F1 RID: 14321 RVA: 0x000ECC64 File Offset: 0x000EAE64
		public void LoadProperty(PropertyData propertyData, string containerPath)
		{
			Property property = Property.UnownedProperties.FirstOrDefault((Property p) => p.PropertyCode == propertyData.PropertyCode);
			if (property == null)
			{
				property = Property.OwnedProperties.FirstOrDefault((Property p) => p.PropertyCode == propertyData.PropertyCode);
			}
			if (property == null)
			{
				property = Business.UnownedBusinesses.FirstOrDefault((Business p) => p.PropertyCode == propertyData.PropertyCode);
			}
			if (property == null)
			{
				property = Business.OwnedBusinesses.FirstOrDefault((Business p) => p.PropertyCode == propertyData.PropertyCode);
			}
			if (property == null)
			{
				Console.LogWarning("Property not found for data: " + propertyData.PropertyCode, null);
				return;
			}
			property.Load(propertyData, containerPath);
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x000ECD28 File Offset: 0x000EAF28
		public Property GetProperty(string code)
		{
			Property property = Property.UnownedProperties.FirstOrDefault((Property p) => p.PropertyCode == code);
			if (property == null)
			{
				property = Property.OwnedProperties.FirstOrDefault((Property p) => p.PropertyCode == code);
			}
			return property;
		}

		// Token: 0x040028C6 RID: 10438
		private PropertiesLoader loader = new PropertiesLoader();
	}
}
