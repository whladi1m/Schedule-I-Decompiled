using System;
using ScheduleOne.Dialogue;
using ScheduleOne.Interaction;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class TestNPC : MonoBehaviour
{
	// Token: 0x06000098 RID: 152 RVA: 0x000052E9 File Offset: 0x000034E9
	public void Hovered()
	{
		if (DialogueHandler.activeDialogue == null)
		{
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
			return;
		}
		this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00005311 File Offset: 0x00003511
	public void Interacted()
	{
		this.handler.InitializeDialogue("TestDialogue", true, "BRANCH_CHECKPASS");
	}

	// Token: 0x0400008B RID: 139
	[Header("References")]
	[SerializeField]
	protected InteractableObject intObj;

	// Token: 0x0400008C RID: 140
	[SerializeField]
	protected DialogueHandler handler;
}
