using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Materials;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x0200097B RID: 2427
	public class AvatarFootstepDetector : MonoBehaviour
	{
		// Token: 0x060041EA RID: 16874 RVA: 0x00114654 File Offset: 0x00112854
		private void LateUpdate()
		{
			if (!this.Avatar.Anim.animator.enabled)
			{
				this.leftDown = false;
				this.rightDown = false;
				return;
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (!this.LeftBone.gameObject.activeInHierarchy)
			{
				return;
			}
			if (Vector3.Distance(this.ReferencePoint.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) > 20f)
			{
				this.leftDown = false;
				this.rightDown = false;
				return;
			}
			if (this.LeftBone.position.y - this.ReferencePoint.position.y < this.StepThreshold)
			{
				if (!this.leftDown)
				{
					this.leftDown = true;
					this.TriggerStep();
				}
			}
			else
			{
				this.leftDown = false;
			}
			if (this.RightBone.position.y - this.ReferencePoint.position.y < this.StepThreshold)
			{
				if (!this.rightDown)
				{
					this.rightDown = true;
					this.TriggerStep();
					return;
				}
			}
			else
			{
				this.rightDown = false;
			}
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x00114764 File Offset: 0x00112964
		public void TriggerStep()
		{
			EMaterialType arg;
			if (this.IsGrounded(out arg))
			{
				this.onStep.Invoke(arg, 1f);
			}
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x0011478C File Offset: 0x0011298C
		public bool IsGrounded(out EMaterialType surfaceType)
		{
			surfaceType = EMaterialType.Generic;
			RaycastHit raycastHit;
			if (Physics.Raycast(this.ReferencePoint.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, 0.25f, this.GroundDetectionMask, QueryTriggerInteraction.Ignore))
			{
				MaterialTag componentInParent = raycastHit.collider.GetComponentInParent<MaterialTag>();
				if (componentInParent != null)
				{
					surfaceType = componentInParent.MaterialType;
				}
				return true;
			}
			return false;
		}

		// Token: 0x04002FE0 RID: 12256
		public const float MAX_DETECTION_RANGE = 20f;

		// Token: 0x04002FE1 RID: 12257
		public const float GROUND_DETECTION_RANGE = 0.25f;

		// Token: 0x04002FE2 RID: 12258
		public Avatar Avatar;

		// Token: 0x04002FE3 RID: 12259
		public Transform ReferencePoint;

		// Token: 0x04002FE4 RID: 12260
		public Transform LeftBone;

		// Token: 0x04002FE5 RID: 12261
		public Transform RightBone;

		// Token: 0x04002FE6 RID: 12262
		public float StepThreshold = 0.1f;

		// Token: 0x04002FE7 RID: 12263
		public LayerMask GroundDetectionMask;

		// Token: 0x04002FE8 RID: 12264
		private bool leftDown;

		// Token: 0x04002FE9 RID: 12265
		private bool rightDown;

		// Token: 0x04002FEA RID: 12266
		public UnityEvent<EMaterialType, float> onStep = new UnityEvent<EMaterialType, float>();
	}
}
