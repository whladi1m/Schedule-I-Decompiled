using System;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200033E RID: 830
	public class RotateRigidbodyToTarget : MonoBehaviour
	{
		// Token: 0x06001299 RID: 4761 RVA: 0x0005147C File Offset: 0x0004F67C
		public void FixedUpdate()
		{
			this.CuntAssFuckingBitch.localRotation = Quaternion.Euler(this.TargetRotation);
			Quaternion rotation = this.CuntAssFuckingBitch.rotation;
			Quaternion quaternion = rotation * Quaternion.Inverse(base.transform.rotation);
			Vector3 a = Vector3.Normalize(new Vector3(quaternion.x, quaternion.y, quaternion.z)) * this.RotationForce;
			float d = Mathf.Clamp01(Quaternion.Angle(base.transform.rotation, rotation) / 90f);
			this.Rigidbody.AddTorque(a * d, ForceMode.Acceleration);
		}

		// Token: 0x040011FB RID: 4603
		public Rigidbody Rigidbody;

		// Token: 0x040011FC RID: 4604
		public Vector3 TargetRotation;

		// Token: 0x040011FD RID: 4605
		public float RotationForce = 1f;

		// Token: 0x040011FE RID: 4606
		public Transform CuntAssFuckingBitch;
	}
}
