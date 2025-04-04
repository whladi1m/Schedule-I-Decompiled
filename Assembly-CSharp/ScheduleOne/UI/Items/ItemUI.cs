using System;
using ScheduleOne.ItemFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000B48 RID: 2888
	public class ItemUI : MonoBehaviour
	{
		// Token: 0x06004CBE RID: 19646 RVA: 0x00143180 File Offset: 0x00141380
		public virtual void Setup(ItemInstance item)
		{
			if (item == null)
			{
				Console.LogError("ItemUI.Setup called and passed null item", null);
			}
			this.itemInstance = item;
			ItemInstance itemInstance = this.itemInstance;
			itemInstance.onDataChanged = (Action)Delegate.Remove(itemInstance.onDataChanged, new Action(this.UpdateUI));
			ItemInstance itemInstance2 = this.itemInstance;
			itemInstance2.onDataChanged = (Action)Delegate.Combine(itemInstance2.onDataChanged, new Action(this.UpdateUI));
			this.UpdateUI();
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x001431F8 File Offset: 0x001413F8
		public virtual void Destroy()
		{
			this.Destroyed = true;
			ItemInstance itemInstance = this.itemInstance;
			itemInstance.onDataChanged = (Action)Delegate.Remove(itemInstance.onDataChanged, new Action(this.UpdateUI));
			this.itemInstance = null;
			UnityEngine.Object.Destroy(this.Rect.gameObject);
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x0014324C File Offset: 0x0014144C
		public virtual RectTransform DuplicateIcon(Transform parent, int overriddenQuantity = -1)
		{
			int displayedQuantity = this.DisplayedQuantity;
			if (overriddenQuantity != -1)
			{
				this.SetDisplayedQuantity(overriddenQuantity);
			}
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.IconImg.gameObject, parent).GetComponent<RectTransform>();
			component.localScale = Vector3.one;
			this.SetDisplayedQuantity(displayedQuantity);
			return component;
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x00143293 File Offset: 0x00141493
		public virtual void SetVisible(bool vis)
		{
			this.Rect.gameObject.SetActive(vis);
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x001432A6 File Offset: 0x001414A6
		public virtual void UpdateUI()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.IconImg.sprite = this.itemInstance.Icon;
			this.SetDisplayedQuantity(this.itemInstance.Quantity);
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x001432D8 File Offset: 0x001414D8
		public virtual void SetDisplayedQuantity(int quantity)
		{
			this.DisplayedQuantity = quantity;
			if (quantity > 1)
			{
				this.QuantityLabel.text = quantity.ToString() + "x";
				return;
			}
			this.QuantityLabel.text = string.Empty;
		}

		// Token: 0x04003A0F RID: 14863
		protected ItemInstance itemInstance;

		// Token: 0x04003A10 RID: 14864
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x04003A11 RID: 14865
		public Image IconImg;

		// Token: 0x04003A12 RID: 14866
		public TextMeshProUGUI QuantityLabel;

		// Token: 0x04003A13 RID: 14867
		protected int DisplayedQuantity;

		// Token: 0x04003A14 RID: 14868
		protected bool Destroyed;
	}
}
