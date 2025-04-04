using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x0200089E RID: 2206
	public class WorldStorageEntity : StorageEntity, IGUIDRegisterable, ISaveable
	{
		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06003C15 RID: 15381 RVA: 0x000FD238 File Offset: 0x000FB438
		// (set) Token: 0x06003C16 RID: 15382 RVA: 0x000FD240 File Offset: 0x000FB440
		public Guid GUID { get; protected set; }

		// Token: 0x06003C17 RID: 15383 RVA: 0x000FD24C File Offset: 0x000FB44C
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06003C18 RID: 15384 RVA: 0x000FD274 File Offset: 0x000FB474
		public string SaveFolderName
		{
			get
			{
				return "Entity_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06003C19 RID: 15385 RVA: 0x000FD2A8 File Offset: 0x000FB4A8
		public string SaveFileName
		{
			get
			{
				return "Entity_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06003C1A RID: 15386 RVA: 0x0004691A File Offset: 0x00044B1A
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06003C1B RID: 15387 RVA: 0x00014002 File Offset: 0x00012202
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06003C1C RID: 15388 RVA: 0x000FD2DA File Offset: 0x000FB4DA
		// (set) Token: 0x06003C1D RID: 15389 RVA: 0x000FD2E2 File Offset: 0x000FB4E2
		public List<string> LocalExtraFiles { get; set; } = new List<string>
		{
			"Contents"
		};

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06003C1E RID: 15390 RVA: 0x000FD2EB File Offset: 0x000FB4EB
		// (set) Token: 0x06003C1F RID: 15391 RVA: 0x000FD2F3 File Offset: 0x000FB4F3
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06003C20 RID: 15392 RVA: 0x000FD2FC File Offset: 0x000FB4FC
		// (set) Token: 0x06003C21 RID: 15393 RVA: 0x000FD304 File Offset: 0x000FB504
		public bool HasChanged { get; set; }

		// Token: 0x06003C22 RID: 15394 RVA: 0x000FD310 File Offset: 0x000FB510
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Storage.WorldStorageEntity_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x000FD32F File Offset: 0x000FB52F
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x000FD33E File Offset: 0x000FB53E
		public virtual bool ShouldSave()
		{
			return base.ItemCount > 0;
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x000FD349 File Offset: 0x000FB549
		public virtual string GetSaveString()
		{
			return new WorldStorageEntityData(this.GUID, new ItemSet(base.ItemSlots)).GetJson(true);
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x000FD368 File Offset: 0x000FB568
		public virtual void Load(WorldStorageEntityData data)
		{
			for (int i = 0; i < data.Contents.Items.Length; i++)
			{
				ItemInstance instance = ItemDeserializer.LoadItem(data.Contents.Items[i]);
				if (base.ItemSlots.Count > i)
				{
					base.ItemSlots[i].SetStoredItem(instance, false);
				}
			}
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x000FD3C1 File Offset: 0x000FB5C1
		protected override void ContentsChanged()
		{
			base.ContentsChanged();
			this.HasChanged = true;
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x000FD410 File Offset: 0x000FB610
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.WorldStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.WorldStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x000FD429 File Offset: 0x000FB629
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.WorldStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.WorldStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x000FD442 File Offset: 0x000FB642
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x000FD450 File Offset: 0x000FB650
		protected virtual void dll()
		{
			base.Awake();
			WorldStorageEntity.All.Add(this);
			if (!GUIDManager.IsGUIDValid(this.BakedGUID))
			{
				Console.LogError(base.gameObject.name + "'s baked GUID is not valid! Bad.", null);
			}
			if (GUIDManager.IsGUIDAlreadyRegistered(new Guid(this.BakedGUID)))
			{
				Console.LogError(base.gameObject.name + "'s baked GUID is already registered! Bad.", this);
			}
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
			this.InitializeSaveable();
		}

		// Token: 0x04002B4D RID: 11085
		public static List<WorldStorageEntity> All = new List<WorldStorageEntity>();

		// Token: 0x04002B4F RID: 11087
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x04002B53 RID: 11091
		private bool dll_Excuted;

		// Token: 0x04002B54 RID: 11092
		private bool dll_Excuted;
	}
}
