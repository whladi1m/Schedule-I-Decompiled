using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000A54 RID: 2644
	public class CategoryButton : MonoBehaviour
	{
		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x0600476F RID: 18287 RVA: 0x0012C0E7 File Offset: 0x0012A2E7
		// (set) Token: 0x06004770 RID: 18288 RVA: 0x0012C0EF File Offset: 0x0012A2EF
		public bool isSelected { get; protected set; }

		// Token: 0x06004771 RID: 18289 RVA: 0x0012C0F8 File Offset: 0x0012A2F8
		private void Awake()
		{
			this.button = base.GetComponent<Button>();
			this.shop = base.GetComponentInParent<ShopInterface>();
			this.button.onClick.AddListener(new UnityAction(this.Clicked));
			this.Deselect();
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x0012C134 File Offset: 0x0012A334
		private void OnValidate()
		{
			base.gameObject.name = this.Category.ToString();
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x0012C152 File Offset: 0x0012A352
		private void Clicked()
		{
			if (this.isSelected)
			{
				this.Deselect();
				return;
			}
			this.Select();
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x0012C169 File Offset: 0x0012A369
		public void Deselect()
		{
			this.isSelected = false;
			this.RefreshUI();
		}

		// Token: 0x06004775 RID: 18293 RVA: 0x0012C178 File Offset: 0x0012A378
		public void Select()
		{
			this.isSelected = true;
			this.RefreshUI();
			this.shop.CategorySelected(this.Category);
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x0012C198 File Offset: 0x0012A398
		private void RefreshUI()
		{
			this.button.interactable = !this.isSelected;
		}

		// Token: 0x04003510 RID: 13584
		public EShopCategory Category;

		// Token: 0x04003511 RID: 13585
		private Button button;

		// Token: 0x04003512 RID: 13586
		private ShopInterface shop;
	}
}
