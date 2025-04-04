using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007B4 RID: 1972
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(BoxCollider))]
	public class PlayerPusher : MonoBehaviour
	{
		// Token: 0x0600360A RID: 13834 RVA: 0x000E384E File Offset: 0x000E1A4E
		private void Awake()
		{
			this.veh = base.GetComponentInParent<LandVehicle>();
			this.collider = base.GetComponent<Collider>();
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Ignore Raycast"));
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x000E387D File Offset: 0x000E1A7D
		private void FixedUpdate()
		{
			this.collider.enabled = !this.veh.Rb.isKinematic;
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x000E38A0 File Offset: 0x000E1AA0
		private void OnTriggerStay(Collider other)
		{
			if (this.veh.speed_Kmh < this.MinSpeedToPush)
			{
				return;
			}
			Player componentInParent = other.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent == Player.Local && componentInParent.CurrentVehicle == null)
			{
				Vector3 normalized = Vector3.Project((componentInParent.transform.position - base.transform.position).normalized, base.transform.right).normalized;
				float d = this.MinPushForce + Mathf.Clamp((this.veh.speed_Kmh - this.MinSpeedToPush) / this.MaxPushSpeed, 0f, 1f) * (this.MaxPushSpeed - this.MinPushForce);
				PlayerSingleton<PlayerMovement>.Instance.Controller.Move(normalized * d * Time.fixedDeltaTime);
			}
		}

		// Token: 0x040026DA RID: 9946
		private LandVehicle veh;

		// Token: 0x040026DB RID: 9947
		[Header("Settings")]
		public float MinSpeedToPush = 3f;

		// Token: 0x040026DC RID: 9948
		public float MaxPushSpeed = 20f;

		// Token: 0x040026DD RID: 9949
		public float MinPushForce = 0.5f;

		// Token: 0x040026DE RID: 9950
		public float MaxPushForce = 5f;

		// Token: 0x040026DF RID: 9951
		private Collider collider;
	}
}
