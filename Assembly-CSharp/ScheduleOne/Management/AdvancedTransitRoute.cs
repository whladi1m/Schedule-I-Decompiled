using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x02000554 RID: 1364
	public class AdvancedTransitRoute : TransitRoute
	{
		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x00089AD2 File Offset: 0x00087CD2
		// (set) Token: 0x06002175 RID: 8565 RVA: 0x00089ADA File Offset: 0x00087CDA
		public ManagementItemFilter Filter { get; private set; } = new ManagementItemFilter(ManagementItemFilter.EMode.Blacklist);

		// Token: 0x06002176 RID: 8566 RVA: 0x00089AE3 File Offset: 0x00087CE3
		public AdvancedTransitRoute(ITransitEntity source, ITransitEntity destination) : base(source, destination)
		{
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x00089AFC File Offset: 0x00087CFC
		public AdvancedTransitRoute(AdvancedTransitRouteData data) : base((!string.IsNullOrEmpty(data.SourceGUID)) ? GUIDManager.GetObject<ITransitEntity>(new Guid(data.SourceGUID)) : null, (!string.IsNullOrEmpty(data.DestinationGUID)) ? GUIDManager.GetObject<ITransitEntity>(new Guid(data.DestinationGUID)) : null)
		{
			this.Filter.SetMode(data.FilterMode);
			for (int i = 0; i < data.FilterItemIDs.Count; i++)
			{
				ItemDefinition item = Registry.GetItem(data.FilterItemIDs[i]);
				if (item != null)
				{
					this.Filter.AddItem(item);
				}
			}
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x00089BAC File Offset: 0x00087DAC
		public ItemInstance GetItemReadyToMove()
		{
			if (base.Source == null || base.Destination == null)
			{
				return null;
			}
			foreach (ItemSlot itemSlot in base.Source.OutputSlots)
			{
				if (itemSlot.ItemInstance != null && this.Filter.DoesItemMeetFilter(itemSlot.ItemInstance))
				{
					int inputCapacityForItem = base.Destination.GetInputCapacityForItem(itemSlot.ItemInstance, null);
					if (inputCapacityForItem > 0)
					{
						return itemSlot.ItemInstance.GetCopy(Mathf.Min(inputCapacityForItem, itemSlot.ItemInstance.Quantity));
					}
				}
			}
			return null;
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x00089C64 File Offset: 0x00087E64
		public AdvancedTransitRouteData GetData()
		{
			List<string> list = new List<string>();
			foreach (ItemDefinition itemDefinition in this.Filter.Items)
			{
				list.Add(itemDefinition.ID);
			}
			string sourceGUID = string.Empty;
			string destinationGUID = string.Empty;
			if (base.Source != null)
			{
				sourceGUID = base.Source.GUID.ToString();
			}
			if (base.Destination != null)
			{
				destinationGUID = base.Destination.GUID.ToString();
			}
			return new AdvancedTransitRouteData(sourceGUID, destinationGUID, this.Filter.Mode, list);
		}
	}
}
