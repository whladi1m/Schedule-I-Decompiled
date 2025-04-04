using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000A63 RID: 2659
	public class ShopInterfaceDetailPanel : MonoBehaviour
	{
		// Token: 0x060047D5 RID: 18389 RVA: 0x0012D9B9 File Offset: 0x0012BBB9
		private void Awake()
		{
			this.Panel.gameObject.SetActive(false);
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x0012D9CC File Offset: 0x0012BBCC
		public void Open(ListingUI _listing)
		{
			this.listing = _listing;
			this.DescriptionLabel.text = this.listing.Listing.Item.Description;
			if (this.listing.Listing.Item.RequiresLevelToPurchase && !this.listing.Listing.Item.IsPurchasable)
			{
				this.UnlockLabel.text = "Unlocks at <color=#2DB92D>" + this.listing.Listing.Item.RequiredRank.ToString() + "</color>";
				this.UnlockLabel.gameObject.SetActive(true);
			}
			else
			{
				this.UnlockLabel.gameObject.SetActive(false);
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<Open>g__Wait|6_0());
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x0012DA9D File Offset: 0x0012BC9D
		private void LateUpdate()
		{
			this.Position();
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x0012DAA8 File Offset: 0x0012BCA8
		private void Position()
		{
			if (this.listing == null)
			{
				return;
			}
			this.Panel.position = this.listing.DetailPanelAnchor.position;
			this.Panel.anchoredPosition = new Vector2(this.Panel.anchoredPosition.x + this.Panel.sizeDelta.x / 2f, this.Panel.anchoredPosition.y);
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x0012DB26 File Offset: 0x0012BD26
		public void Close()
		{
			this.listing = null;
			this.Panel.gameObject.SetActive(false);
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x0012DB40 File Offset: 0x0012BD40
		[CompilerGenerated]
		private IEnumerator <Open>g__Wait|6_0()
		{
			this.LayoutGroup.enabled = false;
			yield return new WaitForEndOfFrame();
			this.Panel.gameObject.SetActive(true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.Panel);
			this.LayoutGroup.CalculateLayoutInputVertical();
			this.LayoutGroup.enabled = true;
			this.Position();
			yield break;
		}

		// Token: 0x0400357A RID: 13690
		[Header("References")]
		public RectTransform Panel;

		// Token: 0x0400357B RID: 13691
		public VerticalLayoutGroup LayoutGroup;

		// Token: 0x0400357C RID: 13692
		public TextMeshProUGUI DescriptionLabel;

		// Token: 0x0400357D RID: 13693
		public TextMeshProUGUI UnlockLabel;

		// Token: 0x0400357E RID: 13694
		private ListingUI listing;
	}
}
