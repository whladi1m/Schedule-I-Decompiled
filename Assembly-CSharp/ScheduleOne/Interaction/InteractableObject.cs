using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Interaction
{
	// Token: 0x02000604 RID: 1540
	public class InteractableObject : MonoBehaviour
	{
		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x000A7030 File Offset: 0x000A5230
		public InteractableObject.EInteractionType _interactionType
		{
			get
			{
				return this.interactionType;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x0600288A RID: 10378 RVA: 0x000A7038 File Offset: 0x000A5238
		public InteractableObject.EInteractableState _interactionState
		{
			get
			{
				return this.interactionState;
			}
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x000A7040 File Offset: 0x000A5240
		public void SetInteractionType(InteractableObject.EInteractionType type)
		{
			this.interactionType = type;
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000A7049 File Offset: 0x000A5249
		public void SetInteractableState(InteractableObject.EInteractableState state)
		{
			this.interactionState = state;
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000A7052 File Offset: 0x000A5252
		public void SetMessage(string _message)
		{
			this.message = _message;
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x000A705B File Offset: 0x000A525B
		public virtual void Hovered()
		{
			if (this.onHovered != null)
			{
				this.onHovered.Invoke();
			}
			if (this.interactionState != InteractableObject.EInteractableState.Disabled)
			{
				this.ShowMessage();
			}
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x000A707F File Offset: 0x000A527F
		public virtual void StartInteract()
		{
			if (this.interactionState == InteractableObject.EInteractableState.Invalid)
			{
				return;
			}
			if (this.onInteractStart != null)
			{
				this.onInteractStart.Invoke();
			}
			Singleton<InteractionManager>.Instance.LerpDisplayScale(0.9f);
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000A70AD File Offset: 0x000A52AD
		public virtual void EndInteract()
		{
			if (this.onInteractEnd != null)
			{
				this.onInteractEnd.Invoke();
			}
			Singleton<InteractionManager>.Instance.LerpDisplayScale(1f);
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000A70D4 File Offset: 0x000A52D4
		protected virtual void ShowMessage()
		{
			Vector3 pos = base.transform.position;
			if (this.displayLocationCollider != null)
			{
				pos = this.displayLocationCollider.ClosestPoint(PlayerSingleton<PlayerCamera>.Instance.transform.position);
			}
			else if (this.displayLocationPoint != null)
			{
				pos = this.displayLocationPoint.position;
			}
			Sprite icon = null;
			string spriteText = string.Empty;
			Color iconColor = Color.white;
			Color messageColor = Color.white;
			switch (this.interactionState)
			{
			case InteractableObject.EInteractableState.Default:
			{
				messageColor = Singleton<InteractionManager>.Instance.messageColor_Default;
				InteractableObject.EInteractionType einteractionType = this.interactionType;
				if (einteractionType != InteractableObject.EInteractionType.Key_Press)
				{
					if (einteractionType != InteractableObject.EInteractionType.LeftMouse_Click)
					{
						Console.LogWarning("EInteractionType not accounted for!", null);
					}
					else
					{
						icon = Singleton<InteractionManager>.Instance.icon_LeftMouse;
						iconColor = Singleton<InteractionManager>.Instance.iconColor_Default;
					}
				}
				else
				{
					icon = Singleton<InteractionManager>.Instance.icon_Key;
					spriteText = Singleton<InteractionManager>.Instance.InteractKey;
					iconColor = Singleton<InteractionManager>.Instance.iconColor_Default_Key;
				}
				break;
			}
			case InteractableObject.EInteractableState.Invalid:
				icon = Singleton<InteractionManager>.Instance.icon_Cross;
				iconColor = Singleton<InteractionManager>.Instance.iconColor_Invalid;
				messageColor = Singleton<InteractionManager>.Instance.messageColor_Invalid;
				break;
			case InteractableObject.EInteractableState.Disabled:
				return;
			case InteractableObject.EInteractableState.Label:
				icon = null;
				messageColor = Singleton<InteractionManager>.Instance.messageColor_Default;
				break;
			default:
				Console.LogWarning("EInteractableState not accounted for!", null);
				return;
			}
			Singleton<InteractionManager>.Instance.EnableInteractionDisplay(pos, icon, spriteText, this.message, messageColor, iconColor);
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000A722C File Offset: 0x000A542C
		public bool CheckAngleLimit(Vector3 interactionSource)
		{
			if (!this.LimitInteractionAngle)
			{
				return true;
			}
			Vector3 normalized = (interactionSource - base.transform.position).normalized;
			return Mathf.Abs(Vector3.SignedAngle(base.transform.forward, normalized, Vector3.up)) < this.AngleLimit;
		}

		// Token: 0x04001DA5 RID: 7589
		[Header("Settings")]
		[SerializeField]
		protected string message = "<Message>";

		// Token: 0x04001DA6 RID: 7590
		[SerializeField]
		protected InteractableObject.EInteractionType interactionType;

		// Token: 0x04001DA7 RID: 7591
		[SerializeField]
		protected InteractableObject.EInteractableState interactionState;

		// Token: 0x04001DA8 RID: 7592
		public float MaxInteractionRange = 5f;

		// Token: 0x04001DA9 RID: 7593
		public bool RequiresUniqueClick = true;

		// Token: 0x04001DAA RID: 7594
		public int Priority;

		// Token: 0x04001DAB RID: 7595
		[SerializeField]
		protected Collider displayLocationCollider;

		// Token: 0x04001DAC RID: 7596
		public Transform displayLocationPoint;

		// Token: 0x04001DAD RID: 7597
		[Header("Angle Limits")]
		public bool LimitInteractionAngle;

		// Token: 0x04001DAE RID: 7598
		public float AngleLimit = 90f;

		// Token: 0x04001DAF RID: 7599
		[Header("Events")]
		public UnityEvent onHovered = new UnityEvent();

		// Token: 0x04001DB0 RID: 7600
		public UnityEvent onInteractStart = new UnityEvent();

		// Token: 0x04001DB1 RID: 7601
		public UnityEvent onInteractEnd = new UnityEvent();

		// Token: 0x02000605 RID: 1541
		public enum EInteractionType
		{
			// Token: 0x04001DB3 RID: 7603
			Key_Press,
			// Token: 0x04001DB4 RID: 7604
			LeftMouse_Click
		}

		// Token: 0x02000606 RID: 1542
		public enum EInteractableState
		{
			// Token: 0x04001DB6 RID: 7606
			Default,
			// Token: 0x04001DB7 RID: 7607
			Invalid,
			// Token: 0x04001DB8 RID: 7608
			Disabled,
			// Token: 0x04001DB9 RID: 7609
			Label
		}
	}
}
