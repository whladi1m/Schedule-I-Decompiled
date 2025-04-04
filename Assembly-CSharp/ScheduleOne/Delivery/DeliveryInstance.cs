using System;
using FishNet.Serializing.Helping;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Property;
using ScheduleOne.UI.Phone.Delivery;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Delivery
{
	// Token: 0x02000702 RID: 1794
	[Serializable]
	public class DeliveryInstance
	{
		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06003076 RID: 12406 RVA: 0x000C9D54 File Offset: 0x000C7F54
		// (set) Token: 0x06003077 RID: 12407 RVA: 0x000C9D5C File Offset: 0x000C7F5C
		[CodegenExclude]
		public DeliveryVehicle ActiveVehicle { get; private set; }

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06003078 RID: 12408 RVA: 0x000C9D65 File Offset: 0x000C7F65
		[CodegenExclude]
		public Property Destination
		{
			get
			{
				return Singleton<PropertyManager>.Instance.GetProperty(this.DestinationCode);
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06003079 RID: 12409 RVA: 0x000C9D77 File Offset: 0x000C7F77
		[CodegenExclude]
		public LoadingDock LoadingDock
		{
			get
			{
				return this.Destination.LoadingDocks[this.LoadingDockIndex];
			}
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x000C9D8B File Offset: 0x000C7F8B
		public DeliveryInstance(string deliveryID, string storeName, string destinationCode, int loadingDockIndex, StringIntPair[] items, EDeliveryStatus status, int timeUntilArrival)
		{
			this.DeliveryID = deliveryID;
			this.StoreName = storeName;
			this.DestinationCode = destinationCode;
			this.LoadingDockIndex = loadingDockIndex;
			this.Items = items;
			this.Status = status;
			this.TimeUntilArrival = timeUntilArrival;
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x0000494F File Offset: 0x00002B4F
		public DeliveryInstance()
		{
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x000C9DC8 File Offset: 0x000C7FC8
		public int GetTimeStatus()
		{
			if (this.Status == EDeliveryStatus.Arrived)
			{
				return -1;
			}
			if (this.Status == EDeliveryStatus.Waiting)
			{
				return 0;
			}
			return this.TimeUntilArrival;
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x000C9DE8 File Offset: 0x000C7FE8
		public void SetStatus(EDeliveryStatus status)
		{
			Console.Log("Setting delivery status to " + status.ToString() + " for delivery " + this.DeliveryID, null);
			this.Status = status;
			if (this.Status == EDeliveryStatus.Arrived)
			{
				this.ActiveVehicle = NetworkSingleton<DeliveryManager>.Instance.GetShopInterface(this.StoreName).DeliveryVehicle;
				this.ActiveVehicle.Activate(this);
			}
			if (this.Status == EDeliveryStatus.Completed)
			{
				if (this.ActiveVehicle != null)
				{
					this.ActiveVehicle.Deactivate();
				}
				if (this.onDeliveryCompleted != null)
				{
					this.onDeliveryCompleted.Invoke();
				}
			}
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x000C9E8C File Offset: 0x000C808C
		public void AddItemsToDeliveryVehicle()
		{
			DeliveryVehicle deliveryVehicle = PlayerSingleton<DeliveryApp>.Instance.GetShop(this.StoreName).MatchingShop.DeliveryVehicle;
			foreach (StringIntPair stringIntPair in this.Items)
			{
				ItemDefinition item = Registry.GetItem(stringIntPair.String);
				int j = stringIntPair.Int;
				while (j > 0)
				{
					int num = Mathf.Min(j, item.StackLimit);
					j -= num;
					ItemInstance defaultInstance = Registry.GetItem(stringIntPair.String).GetDefaultInstance(num);
					deliveryVehicle.Vehicle.Storage.InsertItem(defaultInstance, true);
				}
			}
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x000C9F27 File Offset: 0x000C8127
		public void OnMinPass()
		{
			this.TimeUntilArrival = Mathf.Max(0, this.TimeUntilArrival - 1);
		}

		// Token: 0x040022C1 RID: 8897
		public string DeliveryID;

		// Token: 0x040022C2 RID: 8898
		public string StoreName;

		// Token: 0x040022C3 RID: 8899
		public string DestinationCode;

		// Token: 0x040022C4 RID: 8900
		public int LoadingDockIndex;

		// Token: 0x040022C5 RID: 8901
		public StringIntPair[] Items;

		// Token: 0x040022C6 RID: 8902
		public EDeliveryStatus Status;

		// Token: 0x040022C7 RID: 8903
		public int TimeUntilArrival;

		// Token: 0x040022C9 RID: 8905
		[CodegenExclude]
		[NonSerialized]
		public UnityEvent onDeliveryCompleted;
	}
}
