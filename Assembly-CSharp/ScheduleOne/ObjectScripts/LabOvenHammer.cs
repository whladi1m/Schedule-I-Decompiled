using System;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BAE RID: 2990
	public class LabOvenHammer : MonoBehaviour
	{
		// Token: 0x0600529C RID: 21148 RVA: 0x0015C5D1 File Offset: 0x0015A7D1
		private void Start()
		{
			this.Draggable.Rb.centerOfMass = this.CoM.localPosition;
		}

		// Token: 0x0600529D RID: 21149 RVA: 0x0015C5F0 File Offset: 0x0015A7F0
		private void Update()
		{
			this.Rotator.enabled = this.Draggable.IsHeld;
			if (this.Draggable.IsHeld)
			{
				this.Rotator.TargetRotation.z = Mathf.Lerp(this.MinAngle, this.MaxAngle, Mathf.Clamp01(Mathf.InverseLerp(this.MinHeight, this.MaxHeight, base.transform.localPosition.y)));
			}
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x0015C667 File Offset: 0x0015A867
		private void OnCollisionEnter(Collision collision)
		{
			if (this.onCollision != null)
			{
				this.onCollision.Invoke(collision);
			}
		}

		// Token: 0x04003DB8 RID: 15800
		public Draggable Draggable;

		// Token: 0x04003DB9 RID: 15801
		public DraggableConstraint Constraint;

		// Token: 0x04003DBA RID: 15802
		public RotateRigidbodyToTarget Rotator;

		// Token: 0x04003DBB RID: 15803
		public Transform CoM;

		// Token: 0x04003DBC RID: 15804
		public Transform ImpactPoint;

		// Token: 0x04003DBD RID: 15805
		public SmoothedVelocityCalculator VelocityCalculator;

		// Token: 0x04003DBE RID: 15806
		[Header("Settings")]
		public float MinHeight;

		// Token: 0x04003DBF RID: 15807
		public float MaxHeight = 0.3f;

		// Token: 0x04003DC0 RID: 15808
		public float MinAngle = 100f;

		// Token: 0x04003DC1 RID: 15809
		public float MaxAngle = 40f;

		// Token: 0x04003DC2 RID: 15810
		public UnityEvent<Collision> onCollision;
	}
}
