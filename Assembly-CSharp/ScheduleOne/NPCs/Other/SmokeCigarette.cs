using System;
using ScheduleOne.AvatarFramework.Animation;
using UnityEngine;

namespace ScheduleOne.NPCs.Other
{
	// Token: 0x0200048A RID: 1162
	public class SmokeCigarette : MonoBehaviour
	{
		// Token: 0x060019DE RID: 6622 RVA: 0x0006FEB1 File Offset: 0x0006E0B1
		private void Awake()
		{
			if (this.Npc == null)
			{
				this.Npc = base.GetComponentInParent<NPC>();
			}
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0006FED0 File Offset: 0x0006E0D0
		public void Begin()
		{
			this.Anim.SetBool("Smoking", true);
			this.cigarette = UnityEngine.Object.Instantiate<GameObject>(this.CigarettePrefab, this.Anim.RightHandContainer);
			this.Npc.Avatar.LookController.OverrideIKWeight(0.3f);
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x0006FF24 File Offset: 0x0006E124
		public void End()
		{
			this.Anim.SetBool("Smoking", false);
			if (this.cigarette != null)
			{
				UnityEngine.Object.Destroy(this.cigarette.gameObject);
				this.cigarette = null;
			}
			this.Npc.Avatar.LookController.OverrideIKWeight(0.2f);
		}

		// Token: 0x04001646 RID: 5702
		public NPC Npc;

		// Token: 0x04001647 RID: 5703
		public GameObject CigarettePrefab;

		// Token: 0x04001648 RID: 5704
		public AvatarAnimation Anim;

		// Token: 0x04001649 RID: 5705
		private GameObject cigarette;
	}
}
