using System;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.UI.Tooltips;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Delivery
{
	// Token: 0x02000AAB RID: 2731
	public class DeliveryStatusDisplay : MonoBehaviour
	{
		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x0600498A RID: 18826 RVA: 0x00133A75 File Offset: 0x00131C75
		// (set) Token: 0x0600498B RID: 18827 RVA: 0x00133A7D File Offset: 0x00131C7D
		public DeliveryInstance DeliveryInstance { get; private set; }

		// Token: 0x0600498C RID: 18828 RVA: 0x00133A88 File Offset: 0x00131C88
		public void AssignDelivery(DeliveryInstance instance)
		{
			this.DeliveryInstance = instance;
			this.DestinationLabel.text = this.DeliveryInstance.Destination.PropertyName + " [" + (this.DeliveryInstance.LoadingDockIndex + 1).ToString() + "]";
			this.ShopLabel.text = this.DeliveryInstance.StoreName;
			foreach (StringIntPair stringIntPair in this.DeliveryInstance.Items)
			{
				Transform component = UnityEngine.Object.Instantiate<GameObject>(this.ItemEntryPrefab, this.ItemEntryContainer).GetComponent<RectTransform>();
				ItemDefinition item = Registry.GetItem(stringIntPair.String);
				component.Find("Label").GetComponent<Text>().text = stringIntPair.Int.ToString() + "x " + item.Name;
			}
			int num = Mathf.CeilToInt((float)this.DeliveryInstance.Items.Length / 2f);
			this.Rect.sizeDelta = new Vector2(this.Rect.sizeDelta.x, (float)(70 + 20 * num));
			this.RefreshStatus();
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00133BAC File Offset: 0x00131DAC
		public void RefreshStatus()
		{
			if (this.DeliveryInstance.Status == EDeliveryStatus.InTransit)
			{
				this.StatusImage.color = this.StatusColor_Transit;
				int timeUntilArrival = this.DeliveryInstance.TimeUntilArrival;
				int num = timeUntilArrival / 60;
				int num2 = timeUntilArrival % 60;
				this.StatusLabel.text = num.ToString() + "h " + num2.ToString() + "m";
				this.StatusTooltip.text = "This delivery is currently in transit.";
				return;
			}
			if (this.DeliveryInstance.Status == EDeliveryStatus.Waiting)
			{
				this.StatusImage.color = this.StatusColor_Waiting;
				this.StatusLabel.text = "Waiting";
				this.StatusTooltip.text = "This delivery is waiting for the loading dock " + (this.DeliveryInstance.LoadingDockIndex + 1).ToString() + " to be empty.";
				return;
			}
			if (this.DeliveryInstance.Status == EDeliveryStatus.Arrived)
			{
				this.StatusImage.color = this.StatusColor_Arrived;
				this.StatusLabel.text = "Arrived";
				this.StatusTooltip.text = "This delivery has arrived and is ready to be unloaded.";
			}
		}

		// Token: 0x040036E2 RID: 14050
		public GameObject ItemEntryPrefab;

		// Token: 0x040036E3 RID: 14051
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x040036E4 RID: 14052
		public Text DestinationLabel;

		// Token: 0x040036E5 RID: 14053
		public Text ShopLabel;

		// Token: 0x040036E6 RID: 14054
		public Image StatusImage;

		// Token: 0x040036E7 RID: 14055
		public Text StatusLabel;

		// Token: 0x040036E8 RID: 14056
		public Tooltip StatusTooltip;

		// Token: 0x040036E9 RID: 14057
		public RectTransform ItemEntryContainer;

		// Token: 0x040036EA RID: 14058
		[Header("Settings")]
		public Color StatusColor_Transit;

		// Token: 0x040036EB RID: 14059
		public Color StatusColor_Waiting;

		// Token: 0x040036EC RID: 14060
		public Color StatusColor_Arrived;
	}
}
