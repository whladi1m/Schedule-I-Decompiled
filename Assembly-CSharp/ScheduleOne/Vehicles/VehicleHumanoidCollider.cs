using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007BD RID: 1981
	public class VehicleHumanoidCollider : MonoBehaviour
	{
		// Token: 0x06003635 RID: 13877 RVA: 0x000E44FE File Offset: 0x000E26FE
		private void Start()
		{
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Ignore Raycast"));
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x000E4515 File Offset: 0x000E2715
		private void OnCollisionStay(Collision collision)
		{
			Debug.Log("Collision Stay: " + collision.collider.gameObject.name);
		}

		// Token: 0x04002707 RID: 9991
		public LandVehicle vehicle;
	}
}
