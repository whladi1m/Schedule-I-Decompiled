using System;
using System.Collections.Generic;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x0200066D RID: 1645
	public class SupplierLocation : MonoBehaviour
	{
		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002D80 RID: 11648 RVA: 0x000BECEF File Offset: 0x000BCEEF
		public bool IsOccupied
		{
			get
			{
				return this.ActiveSupplier != null;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002D81 RID: 11649 RVA: 0x000BECFD File Offset: 0x000BCEFD
		// (set) Token: 0x06002D82 RID: 11650 RVA: 0x000BED05 File Offset: 0x000BCF05
		public Supplier ActiveSupplier { get; private set; }

		// Token: 0x06002D83 RID: 11651 RVA: 0x000BED10 File Offset: 0x000BCF10
		public void Awake()
		{
			SupplierLocation.AllLocations.Add(this);
			this.GenericContainer.gameObject.SetActive(false);
			WorldStorageEntity[] deliveryBays = this.DeliveryBays;
			for (int i = 0; i < deliveryBays.Length; i++)
			{
				deliveryBays[i].transform.Find("Container").gameObject.SetActive(false);
			}
			this.configs = base.GetComponentsInChildren<SupplierLocationConfiguration>();
			SupplierLocationConfiguration[] array = this.configs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Deactivate();
			}
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000BED94 File Offset: 0x000BCF94
		private void OnDestroy()
		{
			SupplierLocation.AllLocations.Remove(this);
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000BEDA4 File Offset: 0x000BCFA4
		public void SetActiveSupplier(Supplier supplier)
		{
			this.ActiveSupplier = supplier;
			this.GenericContainer.gameObject.SetActive(this.ActiveSupplier != null);
			WorldStorageEntity[] deliveryBays = this.DeliveryBays;
			for (int i = 0; i < deliveryBays.Length; i++)
			{
				deliveryBays[i].transform.Find("Container").gameObject.SetActive(this.ActiveSupplier != null);
			}
			foreach (SupplierLocationConfiguration supplierLocationConfiguration in this.configs)
			{
				if (this.ActiveSupplier != null && supplierLocationConfiguration.SupplierID == this.ActiveSupplier.ID)
				{
					supplierLocationConfiguration.Activate();
				}
				else
				{
					supplierLocationConfiguration.Deactivate();
				}
			}
		}

		// Token: 0x04002067 RID: 8295
		public static List<SupplierLocation> AllLocations = new List<SupplierLocation>();

		// Token: 0x04002069 RID: 8297
		[Header("Settings")]
		public string LocationName;

		// Token: 0x0400206A RID: 8298
		public string LocationDescription;

		// Token: 0x0400206B RID: 8299
		[Header("References")]
		public Transform GenericContainer;

		// Token: 0x0400206C RID: 8300
		public Transform SupplierStandPoint;

		// Token: 0x0400206D RID: 8301
		public WorldStorageEntity[] DeliveryBays;

		// Token: 0x0400206E RID: 8302
		private SupplierLocationConfiguration[] configs;
	}
}
