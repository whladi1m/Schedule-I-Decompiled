using System;
using UnityEngine.UI;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x02000999 RID: 2457
	public class ACSliderReplicator : ACReplicator
	{
		// Token: 0x0600427D RID: 17021 RVA: 0x00116D15 File Offset: 0x00114F15
		protected override void AvatarSettingsChanged(AvatarSettings newSettings)
		{
			base.AvatarSettingsChanged(newSettings);
			this.slider.SetValueWithoutNotify((float)newSettings[this.propertyName]);
		}

		// Token: 0x0400306E RID: 12398
		public Slider slider;
	}
}
