using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x02000897 RID: 2199
	public class StorageVisualizer : MonoBehaviour
	{
		// Token: 0x06003BDC RID: 15324 RVA: 0x000FC238 File Offset: 0x000FA438
		protected virtual void Awake()
		{
			for (int i = 0; i < this.StorageGrids.Length; i++)
			{
				this.totalFootprintCapacity += this.StorageGrids[i].GetTotalFootprintSize();
			}
			this.RefreshVisuals();
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x000FC278 File Offset: 0x000FA478
		protected virtual void FixedUpdate()
		{
			if (Singleton<LoadManager>.InstanceExists && Singleton<LoadManager>.Instance.IsLoading)
			{
				return;
			}
			if (this.updateVisuals)
			{
				this.updateVisuals = false;
				if (this.BlockRefreshes)
				{
					return;
				}
				this.RefreshVisuals();
			}
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x000FC2AC File Offset: 0x000FA4AC
		public void AddSlot(ItemSlot slot, bool update = false)
		{
			if (!this.itemSlots.Contains(slot))
			{
				this.itemSlots.Add(slot);
				slot.onItemDataChanged = (Action)Delegate.Combine(slot.onItemDataChanged, new Action(delegate()
				{
					this.updateVisuals = true;
				}));
			}
			if (update)
			{
				this.updateVisuals = true;
			}
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x000FC2FF File Offset: 0x000FA4FF
		public Dictionary<StorableItemInstance, int> GetVisualRepresentation()
		{
			return StorageVisualizationUtility.GetVisualRepresentation(this.GetContentsDictionary(), this.totalFootprintCapacity);
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x000FC314 File Offset: 0x000FA514
		public virtual void RefreshVisuals()
		{
			Dictionary<StorableItemInstance, int> visualRepresentation = this.GetVisualRepresentation();
			List<StorableItemInstance> list = visualRepresentation.Keys.ToList<StorableItemInstance>();
			List<StorableItemInstance> list2 = this.activeStoredItems.Keys.ToList<StorableItemInstance>();
			for (int i = 0; i < list2.Count; i++)
			{
				int quantityRequirement = 0;
				if (visualRepresentation.ContainsKey(list2[i]))
				{
					quantityRequirement = visualRepresentation[list2[i]];
				}
				this.DestroyExcessStoredItems(list2[i], quantityRequirement);
			}
			int num = 0;
			for (int j = 0; j < list.Count; j++)
			{
				num += this.EnsureSufficientStoredItems(list[j], visualRepresentation[list[j]]).Count;
			}
			List<StoredItem> list3 = new List<StoredItem>();
			if (num > 0 || num == 0 || this.FullRefreshOnItemRemoved)
			{
				foreach (StorableItemInstance key in list)
				{
					for (int k = 0; k < this.activeStoredItems[key].Count; k++)
					{
						this.activeStoredItems[key][k].ClearFootprintOccupancy();
					}
				}
				foreach (StorableItemInstance storableItemInstance in list)
				{
					List<StoredItem> list4 = this.activeStoredItems[storableItemInstance];
					int num2 = list4[0].FootprintX * list4[0].FootprintY;
					List<StoredItem> list5 = new List<StoredItem>();
					list5.AddRange(list4);
					foreach (StoredItem storedItem in list4)
					{
						bool flag = false;
						for (int l = 0; l < this.StorageGrids.Length; l++)
						{
							Coordinate c;
							float rotation;
							if (this.StorageGrids[l].freeTiles.Count >= num2 && this.StorageGrids[l].TryFitItem(storedItem.FootprintX, storedItem.FootprintY, new List<Coordinate>(), out c, out rotation))
							{
								storedItem.InitializeStoredItem(storableItemInstance, this.StorageGrids[l], c, rotation);
								list5.Remove(storedItem);
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							break;
						}
					}
					list3.AddRange(list5);
				}
			}
			if (list3.Count > 0)
			{
				Console.LogWarning("Failed to fit " + list3.Count.ToString() + " stored items into the storage entity. Deleting them.", null);
				for (int m = 0; m < list3.Count; m++)
				{
					if (!(list3[m] == null))
					{
						UnityEngine.Object.Destroy(list3[m].gameObject);
					}
				}
			}
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x000FC630 File Offset: 0x000FA830
		private List<StoredItem> EnsureSufficientStoredItems(StorableItemInstance item, int quantityRequirement)
		{
			int num = 0;
			if (this.activeStoredItems.ContainsKey(item))
			{
				num = this.activeStoredItems[item].Count;
			}
			List<StoredItem> list = new List<StoredItem>();
			if (num < quantityRequirement)
			{
				if (!this.activeStoredItems.ContainsKey(item))
				{
					this.activeStoredItems.Add(item, new List<StoredItem>());
				}
				int num2 = quantityRequirement - num;
				for (int i = 0; i < num2; i++)
				{
					StoredItem component = UnityEngine.Object.Instantiate<StoredItem>(item.StoredItem, (this.ItemContainer != null) ? this.ItemContainer : base.transform).GetComponent<StoredItem>();
					component.transform.localScale = Vector3.one;
					this.activeStoredItems[item].Add(component);
					list.Add(component);
					Collider[] componentsInChildren = component.GetComponentsInChildren<Collider>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].enabled = false;
					}
				}
			}
			return list;
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x000FC724 File Offset: 0x000FA924
		private void DestroyExcessStoredItems(StorableItemInstance item, int quantityRequirement)
		{
			int num = 0;
			if (this.activeStoredItems.ContainsKey(item))
			{
				num = this.activeStoredItems[item].Count;
			}
			if (num > quantityRequirement)
			{
				int num2 = num - quantityRequirement;
				for (int i = 0; i < num2; i++)
				{
					this.activeStoredItems[item][this.activeStoredItems[item].Count - 1].DestroyStoredItem();
					this.activeStoredItems[item].RemoveAt(this.activeStoredItems[item].Count - 1);
				}
			}
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x000FC7B4 File Offset: 0x000FA9B4
		public Dictionary<StorableItemInstance, int> GetContentsDictionary()
		{
			Dictionary<StorableItemInstance, int> dictionary = new Dictionary<StorableItemInstance, int>();
			for (int i = 0; i < this.itemSlots.Count; i++)
			{
				if (this.itemSlots[i].ItemInstance != null && this.itemSlots[i].ItemInstance is StorableItemInstance && this.itemSlots[i].Quantity > 0 && !dictionary.ContainsKey(this.itemSlots[i].ItemInstance as StorableItemInstance))
				{
					dictionary.Add(this.itemSlots[i].ItemInstance as StorableItemInstance, this.itemSlots[i].Quantity);
				}
			}
			return dictionary;
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x000FC86E File Offset: 0x000FAA6E
		protected void QueueRefresh()
		{
			this.updateVisuals = true;
		}

		// Token: 0x04002B2B RID: 11051
		[Header("References")]
		public StorageGrid[] StorageGrids;

		// Token: 0x04002B2C RID: 11052
		public Transform ItemContainer;

		// Token: 0x04002B2D RID: 11053
		[Header("Settings")]
		[Tooltip("Should storage visuals be fully recalculated when item(s) are removed?")]
		public bool FullRefreshOnItemRemoved;

		// Token: 0x04002B2E RID: 11054
		protected List<ItemSlot> itemSlots = new List<ItemSlot>();

		// Token: 0x04002B2F RID: 11055
		protected int totalFootprintCapacity;

		// Token: 0x04002B30 RID: 11056
		protected Dictionary<StorableItemInstance, List<StoredItem>> activeStoredItems = new Dictionary<StorableItemInstance, List<StoredItem>>();

		// Token: 0x04002B31 RID: 11057
		public bool BlockRefreshes;

		// Token: 0x04002B32 RID: 11058
		protected bool updateVisuals;
	}
}
