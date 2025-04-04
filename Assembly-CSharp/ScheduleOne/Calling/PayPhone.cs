using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Lighting;
using ScheduleOne.PlayerScripts;
using ScheduleOne.ScriptableObjects;
using ScheduleOne.UI.Phone;
using UnityEngine;

namespace ScheduleOne.Calling
{
	// Token: 0x02000767 RID: 1895
	public class PayPhone : MonoBehaviour
	{
		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060033DD RID: 13277 RVA: 0x000D8E8A File Offset: 0x000D708A
		public PhoneCallData QueuedCall
		{
			get
			{
				return Singleton<CallManager>.Instance.QueuedCallData;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060033DE RID: 13278 RVA: 0x000D8E96 File Offset: 0x000D7096
		public PhoneCallData ActiveCall
		{
			get
			{
				return Singleton<CallInterface>.Instance.ActiveCallData;
			}
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x000D8EA4 File Offset: 0x000D70A4
		public void FixedUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			this.timeSinceLastRing += Time.fixedDeltaTime;
			float num = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, base.transform.position);
			this.Light.IsOn = (this.QueuedCall != null && this.ActiveCall == null);
			if (num < 9f && this.QueuedCall != null && this.timeSinceLastRing >= 4f && this.ActiveCall == null)
			{
				this.timeSinceLastRing = 0f;
				this.RingSound.Play();
			}
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x000D8F58 File Offset: 0x000D7158
		public void Hovered()
		{
			if (this.CanInteract())
			{
				this.IntObj.SetMessage("Answer phone");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x000D8F8C File Offset: 0x000D718C
		public void Interacted()
		{
			if (!this.CanInteract())
			{
				return;
			}
			Singleton<CallInterface>.Instance.StartCall(this.QueuedCall, this.QueuedCall.CallerID, 0);
			this.RingSound.Stop();
			this.AnswerSound.Play();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.2f);
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x000D900E File Offset: 0x000D720E
		private bool CanInteract()
		{
			return !(this.QueuedCall == null) && !(this.ActiveCall != null) && !Singleton<CallInterface>.Instance.IsOpen;
		}

		// Token: 0x04002534 RID: 9524
		public const float RING_INTERVAL = 4f;

		// Token: 0x04002535 RID: 9525
		public const float RING_RANGE = 9f;

		// Token: 0x04002536 RID: 9526
		public BlinkingLight Light;

		// Token: 0x04002537 RID: 9527
		public AudioSourceController RingSound;

		// Token: 0x04002538 RID: 9528
		public AudioSourceController AnswerSound;

		// Token: 0x04002539 RID: 9529
		public InteractableObject IntObj;

		// Token: 0x0400253A RID: 9530
		public Transform CameraPosition;

		// Token: 0x0400253B RID: 9531
		private float timeSinceLastRing = 100f;
	}
}
