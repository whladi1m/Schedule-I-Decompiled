using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008B5 RID: 2229
	public class StationItem : MonoBehaviour
	{
		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06003CA2 RID: 15522 RVA: 0x000FEC4D File Offset: 0x000FCE4D
		// (set) Token: 0x06003CA3 RID: 15523 RVA: 0x000FEC55 File Offset: 0x000FCE55
		public List<ItemModule> ActiveModules { get; protected set; } = new List<ItemModule>();

		// Token: 0x06003CA4 RID: 15524 RVA: 0x00050741 File Offset: 0x0004E941
		protected virtual void Awake()
		{
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Task"));
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Initialize(StorableItemDefinition itemDefinition)
		{
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x000FEC60 File Offset: 0x000FCE60
		public void ActivateModule<T>() where T : ItemModule
		{
			ItemModule itemModule = this.GetModule<T>();
			if (itemModule == null)
			{
				Console.LogWarning(itemModule.GetType().Name + " is not a valid module for " + base.name, null);
				return;
			}
			this.ActiveModules.Add(itemModule);
			itemModule.ActivateModule(this);
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x000F7B06 File Offset: 0x000F5D06
		public void Destroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x000FECB7 File Offset: 0x000FCEB7
		public bool HasModule<T>() where T : ItemModule
		{
			return this.Modules.Exists((ItemModule x) => x.GetType() == typeof(T));
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x000FECE3 File Offset: 0x000FCEE3
		public T GetModule<T>() where T : ItemModule
		{
			return (T)((object)this.Modules.Find((ItemModule x) => x.GetType() == typeof(T)));
		}

		// Token: 0x04002BC9 RID: 11209
		public List<ItemModule> Modules;

		// Token: 0x04002BCA RID: 11210
		public TrashItem TrashPrefab;
	}
}
