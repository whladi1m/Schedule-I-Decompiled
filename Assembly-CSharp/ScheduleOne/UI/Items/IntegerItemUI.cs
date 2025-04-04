using System;
using ScheduleOne.ItemFramework;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000B44 RID: 2884
	public class IntegerItemUI : ItemUI
	{
		// Token: 0x06004CAE RID: 19630 RVA: 0x00142D7A File Offset: 0x00140F7A
		public override void Setup(ItemInstance item)
		{
			this.integerItemInstance = (item as IntegerItemInstance);
			base.Setup(item);
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x00142D8F File Offset: 0x00140F8F
		public override void UpdateUI()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.ValueLabel.text = this.integerItemInstance.Value.ToString();
			base.UpdateUI();
		}

		// Token: 0x040039FE RID: 14846
		public Text ValueLabel;

		// Token: 0x040039FF RID: 14847
		protected IntegerItemInstance integerItemInstance;
	}
}
