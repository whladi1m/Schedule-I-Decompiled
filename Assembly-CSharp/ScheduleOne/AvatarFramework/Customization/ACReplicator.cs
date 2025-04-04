using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x02000998 RID: 2456
	public class ACReplicator : MonoBehaviour
	{
		// Token: 0x0600427A RID: 17018 RVA: 0x00116CD9 File Offset: 0x00114ED9
		private void Start()
		{
			CustomizationManager instance = Singleton<CustomizationManager>.Instance;
			instance.OnAvatarSettingsChanged = (CustomizationManager.AvatarSettingsChanged)Delegate.Combine(instance.OnAvatarSettingsChanged, new CustomizationManager.AvatarSettingsChanged(this.AvatarSettingsChanged));
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void AvatarSettingsChanged(AvatarSettings newSettings)
		{
		}

		// Token: 0x0400306D RID: 12397
		public string propertyName = string.Empty;
	}
}
