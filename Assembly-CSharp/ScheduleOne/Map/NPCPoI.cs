using System;
using ScheduleOne.NPCs;
using UnityEngine.UI;

namespace ScheduleOne.Map
{
	// Token: 0x02000BF8 RID: 3064
	public class NPCPoI : POI
	{
		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x060055BC RID: 21948 RVA: 0x0016862E File Offset: 0x0016682E
		// (set) Token: 0x060055BD RID: 21949 RVA: 0x00168636 File Offset: 0x00166836
		public NPC NPC { get; private set; }

		// Token: 0x060055BE RID: 21950 RVA: 0x00168640 File Offset: 0x00166840
		public override void InitializeUI()
		{
			base.InitializeUI();
			if (base.IconContainer != null && this.NPC != null)
			{
				base.IconContainer.Find("Outline/Icon").GetComponent<Image>().sprite = this.NPC.MugshotSprite;
			}
		}

		// Token: 0x060055BF RID: 21951 RVA: 0x00168694 File Offset: 0x00166894
		public void SetNPC(NPC npc)
		{
			this.NPC = npc;
			if (base.IconContainer != null && this.NPC != null)
			{
				base.IconContainer.Find("Outline/Icon").GetComponent<Image>().sprite = this.NPC.MugshotSprite;
			}
		}
	}
}
