using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence
{
	// Token: 0x0200037F RID: 895
	public class SavePoint : MonoBehaviour
	{
		// Token: 0x06001459 RID: 5209 RVA: 0x0005AE30 File Offset: 0x00059030
		public void Awake()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0005AE6C File Offset: 0x0005906C
		public void Hovered()
		{
			if (!InstanceFinder.IsServer)
			{
				this.IntObj.SetMessage("Only host can save");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				return;
			}
			if (Singleton<SaveManager>.Instance.IsSaving)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			}
			string message;
			if (this.CanSave(out message))
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.IntObj.SetMessage("Save game");
				return;
			}
			if (Singleton<SaveManager>.Instance.SecondsSinceLastSave < 10f)
			{
				this.IntObj.SetMessage("Game saved!");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Label);
				return;
			}
			this.IntObj.SetMessage(message);
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0005AF24 File Offset: 0x00059124
		private bool CanSave(out string reason)
		{
			reason = string.Empty;
			if (Singleton<SaveManager>.Instance.SecondsSinceLastSave < 60f)
			{
				reason = "Wait " + Mathf.Ceil(60f - Singleton<SaveManager>.Instance.SecondsSinceLastSave).ToString() + "s";
				return false;
			}
			if (Singleton<SaveManager>.Instance.SecondsSinceLastSave < 60f)
			{
				reason = "Wait " + Mathf.Ceil(60f - Singleton<SaveManager>.Instance.SecondsSinceLastSave).ToString() + "s";
				return false;
			}
			return true;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0005AFBC File Offset: 0x000591BC
		public void Interacted()
		{
			string text;
			if (!this.CanSave(out text))
			{
				return;
			}
			this.Save();
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0005AFDC File Offset: 0x000591DC
		private void Save()
		{
			Singleton<SaveManager>.Instance.onSaveComplete.RemoveListener(new UnityAction(this.OnSaveComplete));
			Singleton<SaveManager>.Instance.onSaveComplete.AddListener(new UnityAction(this.OnSaveComplete));
			Singleton<SaveManager>.Instance.Save();
			if (this.onSaveStart != null)
			{
				this.onSaveStart.Invoke();
			}
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0005B03C File Offset: 0x0005923C
		public void OnSaveComplete()
		{
			if (this.onSaveComplete != null)
			{
				this.onSaveComplete.Invoke();
			}
		}

		// Token: 0x0400132D RID: 4909
		public const float SAVE_COOLDOWN = 60f;

		// Token: 0x0400132E RID: 4910
		public InteractableObject IntObj;

		// Token: 0x0400132F RID: 4911
		public UnityEvent onSaveStart;

		// Token: 0x04001330 RID: 4912
		public UnityEvent onSaveComplete;
	}
}
