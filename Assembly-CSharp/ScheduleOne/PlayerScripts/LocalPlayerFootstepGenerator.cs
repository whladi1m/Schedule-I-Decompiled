using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Materials;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005CD RID: 1485
	public class LocalPlayerFootstepGenerator : MonoBehaviour
	{
		// Token: 0x060024D6 RID: 9430 RVA: 0x000945E4 File Offset: 0x000927E4
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				return;
			}
			if (!PlayerSingleton<PlayerMovement>.Instance.canMove)
			{
				this.currentDistance = 0f;
				this.lastFramePosition = base.transform.position;
				return;
			}
			Vector3 position = base.transform.position;
			this.currentDistance += Vector3.Distance(position, this.lastFramePosition) * (PlayerSingleton<PlayerMovement>.Instance.isSprinting ? 0.75f : 1f);
			if (this.currentDistance >= this.DistancePerStep)
			{
				this.currentDistance = 0f;
				this.lastFramePosition = position;
				this.TriggerStep();
			}
			this.lastFramePosition = position;
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x00094690 File Offset: 0x00092890
		public void TriggerStep()
		{
			EMaterialType arg;
			if (this.IsGrounded(out arg))
			{
				this.onStep.Invoke(arg, PlayerSingleton<PlayerMovement>.Instance.isSprinting ? 1f : 0.5f);
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x000946CC File Offset: 0x000928CC
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

		// Token: 0x04001B77 RID: 7031
		public float DistancePerStep = 0.75f;

		// Token: 0x04001B78 RID: 7032
		public Transform ReferencePoint;

		// Token: 0x04001B79 RID: 7033
		public LayerMask GroundDetectionMask;

		// Token: 0x04001B7A RID: 7034
		public UnityEvent<EMaterialType, float> onStep = new UnityEvent<EMaterialType, float>();

		// Token: 0x04001B7B RID: 7035
		private float currentDistance;

		// Token: 0x04001B7C RID: 7036
		private Vector3 lastFramePosition = Vector3.zero;
	}
}
