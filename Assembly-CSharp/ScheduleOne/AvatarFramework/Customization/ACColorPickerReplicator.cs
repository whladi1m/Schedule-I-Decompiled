using System;
using HSVPicker;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x02000996 RID: 2454
	public class ACColorPickerReplicator : ACReplicator
	{
		// Token: 0x06004277 RID: 17015 RVA: 0x00116CAC File Offset: 0x00114EAC
		protected override void AvatarSettingsChanged(AvatarSettings newSettings)
		{
			base.AvatarSettingsChanged(newSettings);
			this.picker.CurrentColor = (Color)newSettings[this.propertyName];
		}

		// Token: 0x0400306C RID: 12396
		public HSVPicker.ColorPicker picker;
	}
}
