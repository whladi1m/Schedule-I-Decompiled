using System;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200033B RID: 827
	public class DraggableConstraint : MonoBehaviour
	{
		// Token: 0x17000376 RID: 886
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x00050A2E File Offset: 0x0004EC2E
		private Vector3 RelativePos
		{
			get
			{
				if (!(this.Container != null))
				{
					return base.transform.localPosition;
				}
				return this.Container.InverseTransformPoint(base.transform.position);
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00050A60 File Offset: 0x0004EC60
		private void Start()
		{
			this.draggable = base.GetComponent<Draggable>();
			if (this.ClampUpDirection)
			{
				this.joint = this.draggable.Rb.gameObject.AddComponent<ConfigurableJoint>();
				if (this.Anchor == null && this.Container != null)
				{
					this.Container.gameObject.AddComponent<Rigidbody>();
					this.Anchor = this.Container.gameObject.GetComponent<Rigidbody>();
					this.Anchor.isKinematic = true;
					this.Anchor.useGravity = false;
				}
				this.joint.connectedBody = this.Anchor;
				this.joint.zMotion = ConfigurableJointMotion.Locked;
				this.joint.angularXMotion = ConfigurableJointMotion.Locked;
				this.joint.angularYMotion = ConfigurableJointMotion.Locked;
				this.joint.angularZMotion = ConfigurableJointMotion.Limited;
			}
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00050B3C File Offset: 0x0004ED3C
		public void SetContainer(Transform container)
		{
			this.Container = container;
			this.startLocalPos = this.RelativePos;
			if (this.joint != null && this.Anchor == null && this.Container != null)
			{
				this.Anchor = this.Container.gameObject.AddComponent<Rigidbody>();
				this.Anchor.isKinematic = true;
				this.Anchor.useGravity = false;
				this.joint.connectedBody = this.Anchor;
			}
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00050BC5 File Offset: 0x0004EDC5
		protected virtual void FixedUpdate()
		{
			if (this.AlignUpToContainerPlane)
			{
				this.AlignToContainerPlane();
			}
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00050BD5 File Offset: 0x0004EDD5
		protected virtual void LateUpdate()
		{
			if (this.ProportionalZClamp)
			{
				this.ProportionalClamp();
			}
			if (this.ClampUpDirection)
			{
				this.ClampUpRot();
			}
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00050BF4 File Offset: 0x0004EDF4
		private void ProportionalClamp()
		{
			if (this.Container == null)
			{
				return;
			}
			if (this.draggable == null)
			{
				return;
			}
			float num = Mathf.Clamp(Mathf.Abs(this.RelativePos.x) / this.startLocalPos.x, 0f, 1f);
			float num2 = Mathf.Abs(this.startLocalPos.z) * num;
			Vector3 vector = this.Container.InverseTransformPoint(this.draggable.originalHitPoint);
			vector.z = Mathf.Clamp(vector.z, -num2, num2);
			Vector3 originalHitPoint = this.Container.TransformPoint(vector);
			this.draggable.SetOriginalHitPoint(originalHitPoint);
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00050CA4 File Offset: 0x0004EEA4
		private void LockRotationX()
		{
			Vector3 eulerAngles = (base.transform.rotation * Quaternion.Inverse(this.Container.rotation)).eulerAngles;
			eulerAngles.x = 0f;
			base.transform.rotation = this.Container.rotation * Quaternion.Euler(eulerAngles);
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00050D08 File Offset: 0x0004EF08
		private void LockRotationY()
		{
			Vector3 eulerAngles = (base.transform.rotation * Quaternion.Inverse(this.Container.rotation)).eulerAngles;
			eulerAngles.y = 0f;
			base.transform.rotation = this.Container.rotation * Quaternion.Euler(eulerAngles);
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00050D6C File Offset: 0x0004EF6C
		private void AlignToContainerPlane()
		{
			Vector3 forward = this.Container.forward;
			Quaternion quaternion = Quaternion.LookRotation(forward, base.transform.up);
			Vector3 normalized = Vector3.ProjectOnPlane(base.transform.forward, forward).normalized;
			Quaternion.FromToRotation(base.transform.forward, normalized) * quaternion;
			base.transform.rotation = quaternion;
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00050DD8 File Offset: 0x0004EFD8
		private void ClampUpRot()
		{
			if (this.joint == null)
			{
				Console.LogWarning("No joint found on DraggableConstraint, cannot clamp up rotation", null);
				return;
			}
			Vector3.Angle(this.draggable.transform.up, Vector3.up);
			SoftJointLimit angularZLimit = this.joint.angularZLimit;
			angularZLimit.limit = this.UpDirectionMaxDifference;
			this.joint.angularZLimit = angularZLimit;
		}

		// Token: 0x040011D8 RID: 4568
		public Transform Container;

		// Token: 0x040011D9 RID: 4569
		public Rigidbody Anchor;

		// Token: 0x040011DA RID: 4570
		public bool ProportionalZClamp;

		// Token: 0x040011DB RID: 4571
		public bool AlignUpToContainerPlane;

		// Token: 0x040011DC RID: 4572
		[Header("Up Direction Clamping")]
		public bool ClampUpDirection;

		// Token: 0x040011DD RID: 4573
		public float UpDirectionMaxDifference = 45f;

		// Token: 0x040011DE RID: 4574
		private Vector3 startLocalPos;

		// Token: 0x040011DF RID: 4575
		private Draggable draggable;

		// Token: 0x040011E0 RID: 4576
		private ConfigurableJoint joint;
	}
}
