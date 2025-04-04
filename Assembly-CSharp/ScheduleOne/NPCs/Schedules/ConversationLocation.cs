using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x02000464 RID: 1124
	public class ConversationLocation : MonoBehaviour
	{
		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x00069C94 File Offset: 0x00067E94
		public bool NPCsReady
		{
			get
			{
				return (from npcReady in this.npcReady
				where npcReady.Value
				select npcReady).Count<KeyValuePair<NPC, bool>>() >= 2;
			}
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00069CCC File Offset: 0x00067ECC
		public void Awake()
		{
			if (this.StandPoints.Length < this.NPCs.Count)
			{
				Console.LogError("ConversationLocation has less StandPoints than NPCs", null);
			}
			foreach (NPC key in this.NPCs)
			{
				this.npcReady.Add(key, false);
			}
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00069D48 File Offset: 0x00067F48
		public Transform GetStandPoint(NPC npc)
		{
			if (!this.NPCs.Contains(npc))
			{
				Console.LogWarning("NPC is not part of this conversation", null);
				return this.StandPoints[0];
			}
			return this.StandPoints[this.NPCs.IndexOf(npc)];
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00069D7F File Offset: 0x00067F7F
		public void SetNPCReady(NPC npc, bool ready)
		{
			if (!this.NPCs.Contains(npc))
			{
				Console.LogWarning("NPC is not part of this conversation", null);
				return;
			}
			this.npcReady[npc] = ready;
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00069DA8 File Offset: 0x00067FA8
		public NPC GetOtherNPC(NPC npc)
		{
			if (!this.NPCs.Contains(npc))
			{
				Console.LogWarning("NPC is not part of this conversation", null);
				return null;
			}
			return (from otherNPC in this.NPCs
			where otherNPC != npc
			select otherNPC).FirstOrDefault<NPC>();
		}

		// Token: 0x04001587 RID: 5511
		public Transform[] StandPoints;

		// Token: 0x04001588 RID: 5512
		[HideInInspector]
		public List<NPC> NPCs = new List<NPC>();

		// Token: 0x04001589 RID: 5513
		private Dictionary<NPC, bool> npcReady = new Dictionary<NPC, bool>();
	}
}
