using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x0200095C RID: 2396
	[RequireComponent(typeof(AvatarEquippable))]
	public class AvatarEquippableLookAt : MonoBehaviour
	{
		// Token: 0x0600413F RID: 16703 RVA: 0x00111D83 File Offset: 0x0010FF83
		private void Start()
		{
			this.avatar = base.GetComponentInParent<Avatar>();
			if (this.avatar == null)
			{
				Debug.LogError("AvatarEquippableLookAt must be a child of an Avatar object.");
				return;
			}
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x00111DAA File Offset: 0x0010FFAA
		private void LateUpdate()
		{
			if (this.avatar == null)
			{
				return;
			}
			this.avatar.LookController.OverrideLookTarget(this.avatar.CurrentEquippable.transform.position, this.Priority, false);
		}

		// Token: 0x04002F19 RID: 12057
		public int Priority;

		// Token: 0x04002F1A RID: 12058
		private Avatar avatar;
	}
}
