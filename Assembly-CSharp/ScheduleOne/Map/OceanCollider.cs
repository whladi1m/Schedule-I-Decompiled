using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using Pathfinding;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BFA RID: 3066
	public class OceanCollider : MonoBehaviour
	{
		// Token: 0x060055C5 RID: 21957 RVA: 0x0016877C File Offset: 0x0016697C
		private void OnTriggerEnter(Collider other)
		{
			Player componentInParent = other.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent == Player.Local && componentInParent.Health.IsAlive && componentInParent.CurrentVehicle == null && !this.localPlayerBeingWarped)
			{
				Console.Log("Player entered ocean: " + other.gameObject.name, null);
				this.localPlayerBeingWarped = true;
				base.StartCoroutine(this.WarpPlayer());
			}
			LandVehicle componentInParent2 = other.GetComponentInParent<LandVehicle>();
			if (componentInParent2 != null)
			{
				Debug.Log("Vehicle entered ocean");
				if ((componentInParent2.DriverPlayer == Player.Local || (componentInParent2.DriverPlayer == null && InstanceFinder.IsHost)) && !this.warpedVehicles.Contains(componentInParent2))
				{
					this.warpedVehicles.Add(componentInParent2);
					base.StartCoroutine(this.WarpVehicle(componentInParent2));
				}
			}
		}

		// Token: 0x060055C6 RID: 21958 RVA: 0x00168861 File Offset: 0x00166A61
		private IEnumerator WarpPlayer()
		{
			this.SplashSound.transform.SetParent(Player.Local.gameObject.transform);
			this.SplashSound.transform.localPosition = Vector3.zero;
			this.SplashSound.Play();
			Singleton<BlackOverlay>.Instance.Open(0.05f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			yield return new WaitForSeconds(0.12f);
			PlayerSingleton<PlayerMovement>.Instance.WarpToNavMesh();
			yield return new WaitForSeconds(0.2f);
			Singleton<BlackOverlay>.Instance.Close(0.3f);
			this.localPlayerBeingWarped = false;
			this.SplashSound.transform.SetParent(base.transform);
			yield return new WaitForSeconds(0.2f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			yield break;
		}

		// Token: 0x060055C7 RID: 21959 RVA: 0x00168870 File Offset: 0x00166A70
		private IEnumerator WarpVehicle(LandVehicle veh)
		{
			bool faded = false;
			if (veh.localPlayerIsDriver)
			{
				faded = true;
				Singleton<BlackOverlay>.Instance.Open(0.15f);
			}
			yield return new WaitForSeconds(0.16f);
			NNConstraint nnconstraint = new NNConstraint();
			nnconstraint.graphMask = GraphMask.FromGraphName("Road Nodes");
			NNInfo nearest = AstarPath.active.GetNearest(veh.transform.position, nnconstraint);
			veh.transform.position = nearest.position + base.transform.up * veh.boundingBoxDimensions.y / 2f;
			veh.transform.rotation = Quaternion.identity;
			veh.Rb.velocity = Vector3.zero;
			veh.Rb.angularVelocity = Vector3.zero;
			yield return new WaitForSeconds(0.2f);
			if (faded)
			{
				Singleton<BlackOverlay>.Instance.Close(0.3f);
			}
			this.warpedVehicles.Remove(veh);
			yield break;
		}

		// Token: 0x04003FB3 RID: 16307
		private bool localPlayerBeingWarped;

		// Token: 0x04003FB4 RID: 16308
		private List<LandVehicle> warpedVehicles = new List<LandVehicle>();

		// Token: 0x04003FB5 RID: 16309
		public AudioSourceController SplashSound;
	}
}
