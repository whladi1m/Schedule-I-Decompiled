using System;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x02000864 RID: 2148
	public class VialCap : Clickable
	{
		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06003A5C RID: 14940 RVA: 0x000F5B75 File Offset: 0x000F3D75
		// (set) Token: 0x06003A5D RID: 14941 RVA: 0x000F5B7D File Offset: 0x000F3D7D
		public bool Removed { get; protected set; }

		// Token: 0x06003A5E RID: 14942 RVA: 0x000F5B86 File Offset: 0x000F3D86
		public override void StartClick(RaycastHit hit)
		{
			base.StartClick(hit);
			this.Pop();
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x000F5B98 File Offset: 0x000F3D98
		private void Pop()
		{
			this.RigidBody = base.gameObject.AddComponent<Rigidbody>();
			this.Removed = true;
			this.Collider.enabled = false;
			this.RigidBody.isKinematic = false;
			this.RigidBody.useGravity = true;
			base.transform.SetParent(null);
			this.RigidBody.AddRelativeForce(Vector3.forward * 1.5f, ForceMode.VelocityChange);
			this.RigidBody.AddRelativeForce(Vector3.up * 0.5f, ForceMode.VelocityChange);
			this.RigidBody.AddTorque(Vector3.up * 1.5f, ForceMode.VelocityChange);
			UnityEngine.Object.Destroy(base.gameObject, 3f);
			base.enabled = false;
		}

		// Token: 0x04002A37 RID: 10807
		public Collider Collider;

		// Token: 0x04002A38 RID: 10808
		private Rigidbody RigidBody;
	}
}
