using System;
using System.Collections;
using System.Runtime.CompilerServices;
using EasyButtons;
using ScheduleOne.NPCs;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class NPCWalkTo : MonoBehaviour
{
	// Token: 0x0600008A RID: 138 RVA: 0x0000518B File Offset: 0x0000338B
	private void Start()
	{
		this.NPC = base.GetComponent<NPC>();
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00005199 File Offset: 0x00003399
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			this.Walk();
		}
	}

	// Token: 0x0600008C RID: 140 RVA: 0x000051AC File Offset: 0x000033AC
	[Button]
	public void Walk()
	{
		this.NPC.Movement.Warp(this.StartPoint.position);
		this.NPC.Movement.SetDestination(this.End.position);
		base.StartCoroutine(this.<Walk>g__WalkRoutine|5_0());
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000051FC File Offset: 0x000033FC
	[CompilerGenerated]
	private IEnumerator <Walk>g__WalkRoutine|5_0()
	{
		yield return new WaitUntil(() => !this.NPC.Movement.IsMoving);
		this.NPC.Movement.FaceDirection(this.End.forward, 0.5f);
		yield break;
	}

	// Token: 0x04000083 RID: 131
	public Transform StartPoint;

	// Token: 0x04000084 RID: 132
	public Transform End;

	// Token: 0x04000085 RID: 133
	public NPC NPC;
}
