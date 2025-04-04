using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x02000994 RID: 2452
	public class ACAssetPathReplicator<T> : ACReplicator where T : UnityEngine.Object
	{
		// Token: 0x06004273 RID: 17011 RVA: 0x00116C5D File Offset: 0x00114E5D
		protected virtual void Awake()
		{
			this.selection = base.GetComponent<ACSelection<T>>();
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x00116C6B File Offset: 0x00114E6B
		protected override void AvatarSettingsChanged(AvatarSettings newSettings)
		{
			base.AvatarSettingsChanged(newSettings);
			this.selection.SelectOption(this.selection.GetAssetPathIndex((string)newSettings[this.propertyName]), false);
		}

		// Token: 0x0400306B RID: 12395
		private ACSelection<T> selection;
	}
}
