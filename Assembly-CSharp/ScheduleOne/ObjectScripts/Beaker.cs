using System;
using ScheduleOne.PlayerTasks;
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B97 RID: 2967
	public class Beaker : StationItem
	{
		// Token: 0x060050A0 RID: 20640 RVA: 0x00153EEC File Offset: 0x001520EC
		private void Start()
		{
			this.Joint.connectedBody = this.Anchor;
			this.Draggable.Rb.centerOfMass = this.Draggable.Rb.transform.InverseTransformPoint(this.CenterOfMass.position);
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x00153F3C File Offset: 0x0015213C
		private void Update()
		{
			SoftJointLimit angularZLimit = this.Joint.angularZLimit;
			angularZLimit.limit = Mathf.Lerp(this.ClampAngle_MinLiquid, this.ClampAngle_MaxLiquid, this.Container.CurrentLiquidLevel);
			this.Joint.angularZLimit = angularZLimit;
			this.Pourable.AngleFromUpToPour = Mathf.Lerp(this.AngleToPour_MinLiquid, this.AngleToPour_MaxLiquid, this.Container.CurrentLiquidLevel);
		}

		// Token: 0x060050A2 RID: 20642 RVA: 0x00153FAB File Offset: 0x001521AB
		public void SetStatic(bool stat)
		{
			this.Draggable.ClickableEnabled = !stat;
			this.ConvexCollider.enabled = !stat;
			this.ConcaveCollider.enabled = stat;
			this.Draggable.Rb.isKinematic = stat;
		}

		// Token: 0x04003C8D RID: 15501
		public float ClampAngle_MaxLiquid = 50f;

		// Token: 0x04003C8E RID: 15502
		public float ClampAngle_MinLiquid = 100f;

		// Token: 0x04003C8F RID: 15503
		public float AngleToPour_MaxLiquid = 95f;

		// Token: 0x04003C90 RID: 15504
		public float AngleToPour_MinLiquid = 140f;

		// Token: 0x04003C91 RID: 15505
		[Header("References")]
		public Draggable Draggable;

		// Token: 0x04003C92 RID: 15506
		public DraggableConstraint Constraint;

		// Token: 0x04003C93 RID: 15507
		public Collider ConcaveCollider;

		// Token: 0x04003C94 RID: 15508
		public Collider ConvexCollider;

		// Token: 0x04003C95 RID: 15509
		public Transform CenterOfMass;

		// Token: 0x04003C96 RID: 15510
		public ConfigurableJoint Joint;

		// Token: 0x04003C97 RID: 15511
		public Rigidbody Anchor;

		// Token: 0x04003C98 RID: 15512
		public LiquidContainer Container;

		// Token: 0x04003C99 RID: 15513
		public Fillable Fillable;

		// Token: 0x04003C9A RID: 15514
		public PourableModule Pourable;

		// Token: 0x04003C9B RID: 15515
		public GameObject FilterPaper;
	}
}
