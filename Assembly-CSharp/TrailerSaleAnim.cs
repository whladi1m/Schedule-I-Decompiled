using System;
using ScheduleOne.NPCs;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class TrailerSaleAnim : MonoBehaviour
{
	// Token: 0x060000AD RID: 173 RVA: 0x00005670 File Offset: 0x00003870
	public void PlayAnim()
	{
		Debug.Log("Playing");
		NPC[] npcs = this.NPCs;
		for (int i = 0; i < npcs.Length; i++)
		{
			npcs[i].Avatar.Anim.SetTrigger("GrabItem");
		}
	}

	// Token: 0x0400009F RID: 159
	public NPC[] NPCs;
}
