using System;
using ScheduleOne.Clothing;
using TMPro;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000A53 RID: 2643
	public class CartEntry_Clothing : CartEntry
	{
		// Token: 0x0600476D RID: 18285 RVA: 0x0012C080 File Offset: 0x0012A280
		protected override void UpdateTitle()
		{
			base.UpdateTitle();
			if ((base.Listing.Item as ClothingDefinition).Colorable)
			{
				TextMeshProUGUI nameLabel = this.NameLabel;
				nameLabel.text = nameLabel.text + " (" + (base.Listing as ClothingShopListing).Color.GetLabel() + ")";
			}
		}
	}
}
