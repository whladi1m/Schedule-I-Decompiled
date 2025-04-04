using System;
using System.Collections.Generic;
using EasyButtons;
using FishNet.Object;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.DevUtilities;
using ScheduleOne.Growing;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.Persistence;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne
{
	// Token: 0x02000263 RID: 611
	public class Registry : PersistentSingleton<Registry>
	{
		// Token: 0x06000CC6 RID: 3270 RVA: 0x00038F50 File Offset: 0x00037150
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<Registry>.Instance == null || Singleton<Registry>.Instance != this)
			{
				return;
			}
			foreach (Registry.ItemRegister itemRegister in this.ItemRegistry)
			{
				if (this.ItemDictionary.ContainsKey(Registry.GetHash(itemRegister.ID)))
				{
					Console.LogError("Duplicate item ID: " + itemRegister.ID, null);
				}
				else
				{
					this.AddToItemDictionary(itemRegister);
				}
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00038FF4 File Offset: 0x000371F4
		public static GameObject GetPrefab(string id)
		{
			Registry.ObjectRegister objectRegister = Singleton<Registry>.Instance.ObjectRegistry.Find((Registry.ObjectRegister x) => x.ID.ToLower() == id.ToString());
			if (objectRegister == null)
			{
				return null;
			}
			return objectRegister.Prefab.gameObject;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0003903A File Offset: 0x0003723A
		public static ItemDefinition GetItem(string ID)
		{
			return Singleton<Registry>.Instance._GetItem(ID);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00039047 File Offset: 0x00037247
		public static T GetItem<T>(string ID) where T : ItemDefinition
		{
			return Singleton<Registry>.Instance._GetItem(ID) as T;
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00039060 File Offset: 0x00037260
		public ItemDefinition _GetItem(string ID)
		{
			if (string.IsNullOrEmpty(ID))
			{
				return null;
			}
			int hash = Registry.GetHash(ID);
			if (!this.ItemDictionary.ContainsKey(hash))
			{
				if (Singleton<LoadManager>.InstanceExists && !Singleton<LoadManager>.Instance.IsLoading)
				{
					Console.LogError("Item '" + ID + "' not found in registry!", null);
				}
				return null;
			}
			Registry.ItemRegister itemRegister = this.ItemDictionary[hash];
			if (itemRegister == null)
			{
				return null;
			}
			return itemRegister.Definition;
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x000390D0 File Offset: 0x000372D0
		public static Constructable GetConstructable(string id)
		{
			GameObject prefab = Registry.GetPrefab(id);
			if (!(prefab != null))
			{
				return null;
			}
			return prefab.GetComponent<Constructable>();
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x000390F5 File Offset: 0x000372F5
		private static int GetHash(string ID)
		{
			return ID.ToLower().GetHashCode();
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00039104 File Offset: 0x00037304
		private static string RemoveAssetsAndPrefab(string originalString)
		{
			int num = originalString.IndexOf("Assets/");
			if (num != -1)
			{
				originalString = originalString.Substring(num + "Assets/".Length);
			}
			int num2 = originalString.LastIndexOf(".prefab");
			if (num2 != -1)
			{
				originalString = originalString.Substring(0, num2);
			}
			return originalString;
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x00039150 File Offset: 0x00037350
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(new UnityAction(this.RemoveRuntimeItems));
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00039174 File Offset: 0x00037374
		public void AddToRegistry(ItemDefinition item)
		{
			Console.Log("Adding " + item.ID + " to registry: " + ((item != null) ? item.ToString() : null), null);
			Registry.ItemRegister itemRegister = new Registry.ItemRegister
			{
				Definition = item,
				ID = item.ID,
				AssetPath = string.Empty
			};
			this.ItemRegistry.Add(itemRegister);
			this.AddToItemDictionary(itemRegister);
			if (Application.isPlaying)
			{
				this.ItemsAddedAtRuntime.Add(new Registry.ItemRegister
				{
					Definition = item,
					ID = item.ID,
					AssetPath = string.Empty
				});
			}
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00039218 File Offset: 0x00037418
		private void AddToItemDictionary(Registry.ItemRegister reg)
		{
			int hash = Registry.GetHash(reg.ID);
			if (this.ItemDictionary.ContainsKey(hash))
			{
				Console.LogError("Duplicate item ID: " + reg.ID, null);
				return;
			}
			this.ItemDictionary.Add(hash, reg);
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00039264 File Offset: 0x00037464
		private void RemoveItemFromDictionary(Registry.ItemRegister reg)
		{
			int hash = Registry.GetHash(reg.ID);
			this.ItemDictionary.Remove(hash);
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0003928C File Offset: 0x0003748C
		public void RemoveRuntimeItems()
		{
			foreach (Registry.ItemRegister itemRegister in new List<Registry.ItemRegister>(this.ItemsAddedAtRuntime))
			{
				this.RemoveFromRegistry(itemRegister.Definition);
			}
			this.ItemsAddedAtRuntime.Clear();
			Console.Log("Removed runtime items from registry", null);
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00039300 File Offset: 0x00037500
		public void RemoveFromRegistry(ItemDefinition item)
		{
			Registry.ItemRegister itemRegister = this.ItemRegistry.Find((Registry.ItemRegister x) => x.Definition == item);
			if (itemRegister != null)
			{
				this.ItemRegistry.Remove(itemRegister);
				this.RemoveItemFromDictionary(itemRegister);
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0003934C File Offset: 0x0003754C
		[Button]
		public void LogOrderedUnlocks()
		{
			List<ItemDefinition> list = new List<ItemDefinition>();
			for (int i = 0; i < this.ItemRegistry.Count; i++)
			{
				if ((this.ItemRegistry[i].Definition as StorableItemDefinition).RequiresLevelToPurchase)
				{
					list.Add(this.ItemRegistry[i].Definition);
				}
			}
			list.Sort((ItemDefinition x, ItemDefinition y) => (x as StorableItemDefinition).RequiredRank.CompareTo((y as StorableItemDefinition).RequiredRank));
			Console.Log("Ordered Unlocks:", null);
			foreach (ItemDefinition itemDefinition in list)
			{
				string id = itemDefinition.ID;
				string str = " - ";
				FullRank requiredRank = (itemDefinition as StorableItemDefinition).RequiredRank;
				Console.Log(id + str + requiredRank.ToString(), null);
			}
		}

		// Token: 0x04000D51 RID: 3409
		[SerializeField]
		private List<Registry.ObjectRegister> ObjectRegistry = new List<Registry.ObjectRegister>();

		// Token: 0x04000D52 RID: 3410
		[SerializeField]
		private List<Registry.ItemRegister> ItemRegistry = new List<Registry.ItemRegister>();

		// Token: 0x04000D53 RID: 3411
		[SerializeField]
		private List<Registry.ItemRegister> ItemsAddedAtRuntime = new List<Registry.ItemRegister>();

		// Token: 0x04000D54 RID: 3412
		private Dictionary<int, Registry.ItemRegister> ItemDictionary = new Dictionary<int, Registry.ItemRegister>();

		// Token: 0x04000D55 RID: 3413
		public List<SeedDefinition> Seeds = new List<SeedDefinition>();

		// Token: 0x02000264 RID: 612
		[Serializable]
		public class ObjectRegister
		{
			// Token: 0x04000D56 RID: 3414
			public string ID;

			// Token: 0x04000D57 RID: 3415
			public string AssetPath;

			// Token: 0x04000D58 RID: 3416
			public NetworkObject Prefab;
		}

		// Token: 0x02000265 RID: 613
		[Serializable]
		public class ItemRegister
		{
			// Token: 0x04000D59 RID: 3417
			public string ID;

			// Token: 0x04000D5A RID: 3418
			public string AssetPath;

			// Token: 0x04000D5B RID: 3419
			public ItemDefinition Definition;
		}
	}
}
