using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.GameTime;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006EB RID: 1771
	[RequireComponent(typeof(Rigidbody))]
	public class VehicleDetector : MonoBehaviour
	{
		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06003018 RID: 12312 RVA: 0x000C846A File Offset: 0x000C666A
		// (set) Token: 0x06003019 RID: 12313 RVA: 0x000C8472 File Offset: 0x000C6672
		public bool IgnoreNewDetections { get; protected set; }

		// Token: 0x0600301A RID: 12314 RVA: 0x000C847C File Offset: 0x000C667C
		private void Awake()
		{
			Rigidbody rigidbody = base.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = base.gameObject.AddComponent<Rigidbody>();
			}
			this.detectionColliders = base.GetComponentsInChildren<Collider>();
			rigidbody.isKinematic = true;
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x000C84B8 File Offset: 0x000C66B8
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onTick = (Action)Delegate.Combine(instance.onTick, new Action(this.MinPass));
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x000C84E0 File Offset: 0x000C66E0
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onTick = (Action)Delegate.Remove(instance.onTick, new Action(this.MinPass));
			}
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x000C8510 File Offset: 0x000C6710
		private void OnTriggerEnter(Collider other)
		{
			if (this.IgnoreNewDetections)
			{
				return;
			}
			LandVehicle componentInParent = other.GetComponentInParent<LandVehicle>();
			if (componentInParent != null && other == componentInParent.boundingBox && !this.vehicles.Contains(componentInParent))
			{
				this.vehicles.Add(componentInParent);
				this.SortVehicles();
			}
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x000C8564 File Offset: 0x000C6764
		private void MinPass()
		{
			bool flag = false;
			for (int i = 0; i < NetworkSingleton<VehicleManager>.Instance.AllVehicles.Count; i++)
			{
				if (Vector3.SqrMagnitude(NetworkSingleton<VehicleManager>.Instance.AllVehicles[i].transform.position - base.transform.position) < 400f)
				{
					flag = true;
					break;
				}
			}
			if (flag != this.collidersEnabled)
			{
				this.collidersEnabled = flag;
				for (int j = 0; j < this.detectionColliders.Length; j++)
				{
					this.detectionColliders[j].enabled = this.collidersEnabled;
				}
			}
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x000C8600 File Offset: 0x000C6800
		private void OnTriggerExit(Collider other)
		{
			if (this.ignoreExit)
			{
				return;
			}
			LandVehicle componentInParent = other.GetComponentInParent<LandVehicle>();
			if (componentInParent != null && other == componentInParent.boundingBox && this.vehicles.Contains(componentInParent))
			{
				this.vehicles.Remove(componentInParent);
				this.SortVehicles();
			}
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x000C8658 File Offset: 0x000C6858
		private void SortVehicles()
		{
			if (this.vehicles.Count > 1)
			{
				from x in this.vehicles
				orderby Vector3.Distance(base.transform.position, x.transform.position)
				select x;
			}
			if (this.vehicles.Count > 0)
			{
				this.closestVehicle = this.vehicles[0];
				return;
			}
			this.closestVehicle = null;
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x000C86B4 File Offset: 0x000C68B4
		public void SetIgnoreNewCollisions(bool ignore)
		{
			this.IgnoreNewDetections = ignore;
			if (!ignore)
			{
				this.ignoreExit = true;
				Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].isTrigger)
					{
						componentsInChildren[i].enabled = false;
						componentsInChildren[i].enabled = true;
					}
				}
				this.ignoreExit = false;
			}
		}

		// Token: 0x06003022 RID: 12322 RVA: 0x000C870C File Offset: 0x000C690C
		public bool AreAnyVehiclesOccupied()
		{
			for (int i = 0; i < this.vehicles.Count; i++)
			{
				if (this.vehicles[i].isOccupied)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x000C8745 File Offset: 0x000C6945
		public void Clear()
		{
			this.vehicles.Clear();
			this.SortVehicles();
		}

		// Token: 0x04002253 RID: 8787
		public const float ACTIVATION_DISTANCE_SQ = 400f;

		// Token: 0x04002254 RID: 8788
		public List<LandVehicle> vehicles = new List<LandVehicle>();

		// Token: 0x04002255 RID: 8789
		public LandVehicle closestVehicle;

		// Token: 0x04002257 RID: 8791
		private bool ignoreExit;

		// Token: 0x04002258 RID: 8792
		private Collider[] detectionColliders;

		// Token: 0x04002259 RID: 8793
		private bool collidersEnabled = true;
	}
}
