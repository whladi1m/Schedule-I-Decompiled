using System;
using ScheduleOne.Construction;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.Property;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Market
{
	// Token: 0x0200054F RID: 1359
	public class BuilderMerchant : MonoBehaviour
	{
		// Token: 0x06002164 RID: 8548 RVA: 0x00089880 File Offset: 0x00087A80
		public void Hovered()
		{
			if (Singleton<ConstructionManager>.Instance.constructionModeEnabled || this.selector.isOpen)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.openTime, this.closeTime))
			{
				this.intObj.SetMessage("View construction menu");
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
			this.intObj.SetMessage("Closed");
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x00089904 File Offset: 0x00087B04
		public void Interacted()
		{
			this.selector.OpenSelector(new PropertySelector.PropertySelected(this.PropertySelected));
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0008991D File Offset: 0x00087B1D
		private void PropertySelected(Property p)
		{
			Singleton<ConstructionManager>.Instance.EnterConstructionMode(p);
		}

		// Token: 0x040019AA RID: 6570
		[Header("Settings")]
		[SerializeField]
		protected int openTime = 600;

		// Token: 0x040019AB RID: 6571
		[SerializeField]
		protected int closeTime = 1800;

		// Token: 0x040019AC RID: 6572
		[Header("References")]
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x040019AD RID: 6573
		[SerializeField]
		private PropertySelector selector;
	}
}
