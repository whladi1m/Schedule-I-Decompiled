using System;
using ScheduleOne.NPCs;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000ACB RID: 2763
	public class AssignedWorkerDisplay : MonoBehaviour
	{
		// Token: 0x06004A2D RID: 18989 RVA: 0x00136FD1 File Offset: 0x001351D1
		public void Set(NPC npc)
		{
			if (npc != null)
			{
				this.Icon.sprite = npc.MugshotSprite;
			}
			base.gameObject.SetActive(npc != null);
		}

		// Token: 0x040037C5 RID: 14277
		public Image Icon;
	}
}
