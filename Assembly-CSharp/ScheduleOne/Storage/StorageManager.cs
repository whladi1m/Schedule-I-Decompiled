using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Storage
{
	// Token: 0x02000894 RID: 2196
	public class StorageManager : NetworkSingleton<StorageManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06003BC1 RID: 15297 RVA: 0x000FBEE0 File Offset: 0x000FA0E0
		public string SaveFolderName
		{
			get
			{
				return "WorldStorageEntities";
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06003BC2 RID: 15298 RVA: 0x000FBEE0 File Offset: 0x000FA0E0
		public string SaveFileName
		{
			get
			{
				return "WorldStorageEntities";
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06003BC3 RID: 15299 RVA: 0x000FBEE7 File Offset: 0x000FA0E7
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06003BC4 RID: 15300 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06003BC5 RID: 15301 RVA: 0x000FBEEF File Offset: 0x000FA0EF
		// (set) Token: 0x06003BC6 RID: 15302 RVA: 0x000FBEF7 File Offset: 0x000FA0F7
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06003BC7 RID: 15303 RVA: 0x000FBF00 File Offset: 0x000FA100
		// (set) Token: 0x06003BC8 RID: 15304 RVA: 0x000FBF08 File Offset: 0x000FA108
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06003BC9 RID: 15305 RVA: 0x000FBF11 File Offset: 0x000FA111
		// (set) Token: 0x06003BCA RID: 15306 RVA: 0x000FBF19 File Offset: 0x000FA119
		public bool HasChanged { get; set; }

		// Token: 0x06003BCB RID: 15307 RVA: 0x000FBF22 File Offset: 0x000FA122
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Storage.StorageManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x000FBF38 File Offset: 0x000FA138
		public Pallet CreatePallet(Vector3 position, Quaternion rotation, string initialSlotGuid = "")
		{
			Pallet component = UnityEngine.Object.Instantiate<GameObject>(this.PalletPrefab).GetComponent<Pallet>();
			component.transform.position = position;
			component.transform.rotation = rotation;
			base.NetworkObject.Spawn(component.gameObject, null, default(Scene));
			if (GUIDManager.IsGUIDValid(initialSlotGuid))
			{
				PalletSlot @object = GUIDManager.GetObject<PalletSlot>(new Guid(initialSlotGuid));
				if (@object != null)
				{
					component.BindToSlot_Server(@object.GUID);
				}
			}
			return component;
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x0003CD29 File Offset: 0x0003AF29
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x000FBFB4 File Offset: 0x000FA1B4
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			for (int i = 0; i < WorldStorageEntity.All.Count; i++)
			{
				if (WorldStorageEntity.All[i].ShouldSave())
				{
					new SaveRequest(WorldStorageEntity.All[i], containerFolder);
					list.Add(WorldStorageEntity.All[i].SaveFileName);
				}
			}
			return list;
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x000FC048 File Offset: 0x000FA248
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.StorageManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.StorageManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x000FC061 File Offset: 0x000FA261
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.StorageManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.StorageManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x000FC07A File Offset: 0x000FA27A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x000FC088 File Offset: 0x000FA288
		protected virtual void dll()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x04002B1F RID: 11039
		[Header("Prefabs")]
		public GameObject PalletPrefab;

		// Token: 0x04002B20 RID: 11040
		private StorageLoader loader = new StorageLoader();

		// Token: 0x04002B24 RID: 11044
		private bool dll_Excuted;

		// Token: 0x04002B25 RID: 11045
		private bool dll_Excuted;
	}
}
