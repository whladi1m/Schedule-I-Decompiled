using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.ConstructableScripts
{
	// Token: 0x02000912 RID: 2322
	public class LoadingDock : Constructable_GridBased
	{
		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06003EED RID: 16109 RVA: 0x0010999E File Offset: 0x00107B9E
		public bool isOccupied
		{
			get
			{
				return this.vehicleDetector.vehicles.Count > 0;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06003EEE RID: 16110 RVA: 0x001099B3 File Offset: 0x00107BB3
		// (set) Token: 0x06003EEF RID: 16111 RVA: 0x001099BB File Offset: 0x00107BBB
		public LandVehicle reservant { get; protected set; }

		// Token: 0x06003EF0 RID: 16112 RVA: 0x001099C4 File Offset: 0x00107BC4
		private void Start()
		{
			this.reservationBlocker.gameObject.SetActive(false);
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x001099D8 File Offset: 0x00107BD8
		protected virtual void Update()
		{
			if (this.vehicleDetector.vehicles.Count > 0 && !this.vehicleDetector.closestVehicle.isOccupied)
			{
				this.wallsOpen = true;
			}
			else
			{
				this.wallsOpen = false;
			}
			bool isOccupied = this.isOccupied;
			if (this.vehicleDetector.closestVehicle != null)
			{
				if (this.currentOccupant != this.vehicleDetector.closestVehicle && this.currentOccupant != null)
				{
					this.currentOccupant = this.vehicleDetector.closestVehicle;
					return;
				}
			}
			else if (this.currentOccupant != null)
			{
				this.currentOccupant = null;
			}
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x00109A84 File Offset: 0x00107C84
		protected virtual void LateUpdate()
		{
			if (this.isOccupied)
			{
				MeshRenderer[] array = this.redLightMeshes;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].material = this.redLightMat_On;
				}
				array = this.greenLightMeshes;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].material = this.greenLightMat_Off;
				}
			}
			else
			{
				MeshRenderer[] array = this.redLightMeshes;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].material = this.redLightMat_Off;
				}
				array = this.greenLightMeshes;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].material = this.greenLightMat_On;
				}
			}
			float max = 0.387487f;
			float min = -0.35f;
			if (this.wallsOpen)
			{
				foreach (Transform transform in this.sideWalls)
				{
					transform.transform.localPosition = new Vector3(transform.transform.localPosition.x, Mathf.Clamp(transform.transform.localPosition.y - Time.deltaTime, min, max), transform.transform.localPosition.z);
				}
				return;
			}
			foreach (Transform transform2 in this.sideWalls)
			{
				transform2.transform.localPosition = new Vector3(transform2.transform.localPosition.x, Mathf.Clamp(transform2.transform.localPosition.y + Time.deltaTime, min, max), transform2.transform.localPosition.z);
			}
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x00109C18 File Offset: 0x00107E18
		public override bool CanBeDestroyed(out string reason)
		{
			if (this.reservant != null)
			{
				reason = "Reserved for dealer";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x00109C38 File Offset: 0x00107E38
		public override void DestroyConstructable(bool callOnServer = true)
		{
			if (this.isOccupied && this.vehicleDetector.closestVehicle != null)
			{
				this.vehicleDetector.closestVehicle.Rb.isKinematic = false;
			}
			base.DestroyConstructable(callOnServer);
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x00109C74 File Offset: 0x00107E74
		public void SetReservant(LandVehicle _res)
		{
			if (this.reservant != null)
			{
				Collider[] componentsInChildren = this.reservant.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					Physics.IgnoreCollision(componentsInChildren[i], this.reservationBlocker, false);
				}
			}
			this.reservant = _res;
			if (this.reservant != null)
			{
				this.gateAnim.Play("LoadingDock_Gate_Close");
			}
			else
			{
				this.gateAnim.Play("LoadingDock_Gate_Open");
			}
			if (this.reservant != null)
			{
				Collider[] componentsInChildren2 = this.reservant.GetComponentsInChildren<Collider>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					Physics.IgnoreCollision(componentsInChildren2[j], this.reservationBlocker, true);
				}
			}
			this.reservationBlocker.gameObject.SetActive(this.reservant != null);
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x00109D44 File Offset: 0x00107F44
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.LoadingDockAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.LoadingDockAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x00109D5D File Offset: 0x00107F5D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ConstructableScripts.LoadingDockAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ConstructableScripts.LoadingDockAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x00109D76 File Offset: 0x00107F76
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x00109D84 File Offset: 0x00107F84
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002D68 RID: 11624
		[Header("References")]
		[SerializeField]
		protected VehicleDetector vehicleDetector;

		// Token: 0x04002D69 RID: 11625
		[SerializeField]
		protected MeshRenderer[] redLightMeshes;

		// Token: 0x04002D6A RID: 11626
		[SerializeField]
		protected MeshRenderer[] greenLightMeshes;

		// Token: 0x04002D6B RID: 11627
		[SerializeField]
		protected Transform[] sideWalls;

		// Token: 0x04002D6C RID: 11628
		[SerializeField]
		protected Animation gateAnim;

		// Token: 0x04002D6D RID: 11629
		[SerializeField]
		protected Collider reservationBlocker;

		// Token: 0x04002D6E RID: 11630
		public Transform vehiclePosition;

		// Token: 0x04002D6F RID: 11631
		[Header("Materials")]
		[SerializeField]
		protected Material redLightMat_On;

		// Token: 0x04002D70 RID: 11632
		[SerializeField]
		protected Material redLightMat_Off;

		// Token: 0x04002D71 RID: 11633
		[SerializeField]
		protected Material greenLightMat_On;

		// Token: 0x04002D72 RID: 11634
		[SerializeField]
		protected Material greenLightMat_Off;

		// Token: 0x04002D73 RID: 11635
		private bool wallsOpen;

		// Token: 0x04002D74 RID: 11636
		private LandVehicle currentOccupant;

		// Token: 0x04002D76 RID: 11638
		private bool dll_Excuted;

		// Token: 0x04002D77 RID: 11639
		private bool dll_Excuted;
	}
}
