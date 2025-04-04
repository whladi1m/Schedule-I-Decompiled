using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Audio;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.UI.Shop;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Delivery
{
	// Token: 0x02000AA3 RID: 2723
	public class DeliveryApp : App<DeliveryApp>
	{
		// Token: 0x0600494B RID: 18763 RVA: 0x00132A4E File Offset: 0x00130C4E
		protected override void Awake()
		{
			base.Awake();
			this.deliveryShops = base.GetComponentsInChildren<DeliveryShop>(true).ToList<DeliveryShop>();
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x00132A68 File Offset: 0x00130C68
		protected override void Start()
		{
			base.Start();
			if (!this.started)
			{
				this.started = true;
				NetworkSingleton<DeliveryManager>.Instance.onDeliveryCreated.AddListener(new UnityAction<DeliveryInstance>(this.CreateDeliveryStatusDisplay));
				NetworkSingleton<DeliveryManager>.Instance.onDeliveryCompleted.AddListener(new UnityAction<DeliveryInstance>(this.DeliveryCompleted));
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.OnMinPass));
				for (int i = 0; i < NetworkSingleton<DeliveryManager>.Instance.Deliveries.Count; i++)
				{
					this.CreateDeliveryStatusDisplay(NetworkSingleton<DeliveryManager>.Instance.Deliveries[i]);
				}
			}
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x00132B19 File Offset: 0x00130D19
		protected override void Update()
		{
			base.Update();
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x00132B24 File Offset: 0x00130D24
		public override void SetOpen(bool open)
		{
			base.SetOpen(open);
			if (open)
			{
				foreach (DeliveryShop deliveryShop in this.deliveryShops)
				{
					deliveryShop.RefreshShop();
				}
				foreach (DeliveryStatusDisplay deliveryStatusDisplay in this.statusDisplays)
				{
					deliveryStatusDisplay.RefreshStatus();
				}
				if (this.MainScrollRect.verticalNormalizedPosition > 1f)
				{
					this.MainScrollRect.verticalNormalizedPosition = 1f;
				}
				this.OrderSubmittedAnim.GetComponent<CanvasGroup>().alpha = 0f;
			}
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x00132BF8 File Offset: 0x00130DF8
		private void OnMinPass()
		{
			if (!base.isOpen)
			{
				return;
			}
			foreach (DeliveryStatusDisplay deliveryStatusDisplay in this.statusDisplays)
			{
				deliveryStatusDisplay.RefreshStatus();
			}
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x00132C54 File Offset: 0x00130E54
		public void RefreshContent(bool keepScrollPosition = true)
		{
			DeliveryApp.<>c__DisplayClass15_0 CS$<>8__locals1 = new DeliveryApp.<>c__DisplayClass15_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.keepScrollPosition = keepScrollPosition;
			CS$<>8__locals1.scrollPos = this.MainScrollRect.verticalNormalizedPosition;
			base.StartCoroutine(CS$<>8__locals1.<RefreshContent>g__Delay|0());
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x00132C93 File Offset: 0x00130E93
		public void PlayOrderSubmittedAnim()
		{
			this.OrderSubmittedAnim.Play();
			this.OrderSubmittedSound.Play();
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x00132CAC File Offset: 0x00130EAC
		private void CreateDeliveryStatusDisplay(DeliveryInstance instance)
		{
			DeliveryStatusDisplay deliveryStatusDisplay = UnityEngine.Object.Instantiate<DeliveryStatusDisplay>(this.StatusDisplayPrefab, this.StatusDisplayContainer);
			deliveryStatusDisplay.AssignDelivery(instance);
			this.statusDisplays.Add(deliveryStatusDisplay);
			this.SortStatusDisplays();
			this.RefreshContent(true);
			this.RefreshNoDeliveriesIndicator();
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x00132CF4 File Offset: 0x00130EF4
		private void DeliveryCompleted(DeliveryInstance instance)
		{
			DeliveryStatusDisplay deliveryStatusDisplay = this.statusDisplays.FirstOrDefault((DeliveryStatusDisplay d) => d.DeliveryInstance.DeliveryID == instance.DeliveryID);
			if (deliveryStatusDisplay != null)
			{
				this.statusDisplays.Remove(deliveryStatusDisplay);
				UnityEngine.Object.Destroy(deliveryStatusDisplay.gameObject);
			}
			this.RefreshNoDeliveriesIndicator();
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x00132D50 File Offset: 0x00130F50
		private void SortStatusDisplays()
		{
			this.statusDisplays = (from d in this.statusDisplays
			orderby d.DeliveryInstance.GetTimeStatus()
			select d).ToList<DeliveryStatusDisplay>();
			for (int i = 0; i < this.statusDisplays.Count; i++)
			{
				this.statusDisplays[i].transform.SetSiblingIndex(i);
			}
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x00132DBF File Offset: 0x00130FBF
		private void RefreshNoDeliveriesIndicator()
		{
			this.NoDeliveriesIndicator.gameObject.SetActive(this.statusDisplays.Count == 0);
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x00132DE0 File Offset: 0x00130FE0
		public static void RefreshLayoutGroupsImmediateAndRecursive(GameObject root)
		{
			LayoutGroup[] componentsInChildren = root.GetComponentsInChildren<LayoutGroup>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(componentsInChildren[i].GetComponent<RectTransform>());
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(root.GetComponent<LayoutGroup>().GetComponent<RectTransform>());
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x00132E20 File Offset: 0x00131020
		public DeliveryShop GetShop(ShopInterface matchingShop)
		{
			return this.deliveryShops.Find((DeliveryShop x) => x.MatchingShop == matchingShop);
		}

		// Token: 0x06004958 RID: 18776 RVA: 0x00132E54 File Offset: 0x00131054
		public DeliveryShop GetShop(string shopName)
		{
			return this.deliveryShops.Find((DeliveryShop x) => x.MatchingShop.ShopName == shopName);
		}

		// Token: 0x040036B0 RID: 14000
		private List<DeliveryShop> deliveryShops = new List<DeliveryShop>();

		// Token: 0x040036B1 RID: 14001
		public DeliveryStatusDisplay StatusDisplayPrefab;

		// Token: 0x040036B2 RID: 14002
		[Header("References")]
		public Animation OrderSubmittedAnim;

		// Token: 0x040036B3 RID: 14003
		public AudioSourceController OrderSubmittedSound;

		// Token: 0x040036B4 RID: 14004
		public RectTransform StatusDisplayContainer;

		// Token: 0x040036B5 RID: 14005
		public RectTransform NoDeliveriesIndicator;

		// Token: 0x040036B6 RID: 14006
		public ScrollRect MainScrollRect;

		// Token: 0x040036B7 RID: 14007
		public LayoutGroup MainLayoutGroup;

		// Token: 0x040036B8 RID: 14008
		private List<DeliveryStatusDisplay> statusDisplays = new List<DeliveryStatusDisplay>();

		// Token: 0x040036B9 RID: 14009
		private bool started;
	}
}
