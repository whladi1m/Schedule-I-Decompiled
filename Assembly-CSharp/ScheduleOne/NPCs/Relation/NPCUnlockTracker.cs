using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Relation
{
	// Token: 0x02000487 RID: 1159
	public class NPCUnlockTracker : MonoBehaviour
	{
		// Token: 0x060019CF RID: 6607 RVA: 0x0006FD04 File Offset: 0x0006DF04
		private void Awake()
		{
			if (this.Npc.RelationData.Unlocked)
			{
				this.Invoke(this.Npc.RelationData.UnlockType, false);
			}
			NPCRelationData relationData = this.Npc.RelationData;
			relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.Invoke));
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x0006FD66 File Offset: 0x0006DF66
		private void Invoke(NPCRelationData.EUnlockType type, bool t)
		{
			if (this.onUnlocked != null)
			{
				this.onUnlocked.Invoke();
			}
		}

		// Token: 0x0400163E RID: 5694
		public NPC Npc;

		// Token: 0x0400163F RID: 5695
		public UnityEvent onUnlocked;
	}
}
