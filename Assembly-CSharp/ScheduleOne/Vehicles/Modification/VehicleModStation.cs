using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Vehicles.Modification
{
	// Token: 0x020007D1 RID: 2001
	public class VehicleModStation : MonoBehaviour
	{
		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060036A6 RID: 13990 RVA: 0x000E5FED File Offset: 0x000E41ED
		// (set) Token: 0x060036A7 RID: 13991 RVA: 0x000E5FF5 File Offset: 0x000E41F5
		public LandVehicle currentVehicle { get; protected set; }

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060036A8 RID: 13992 RVA: 0x000E5FFE File Offset: 0x000E41FE
		public bool isOpen
		{
			get
			{
				return this.currentVehicle != null;
			}
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x000E600C File Offset: 0x000E420C
		public void Open(LandVehicle vehicle)
		{
			this.orbitCam.Enable();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.currentVehicle = vehicle;
			vehicle.transform.rotation = this.vehiclePosition.rotation;
			vehicle.transform.position = this.vehiclePosition.position;
			vehicle.transform.position -= vehicle.transform.InverseTransformPoint(vehicle.boundingBox.transform.position);
			vehicle.transform.position += Vector3.up * vehicle.boundingBox.transform.localScale.y * 0.5f;
			Singleton<VehicleModMenu>.Instance.Open(this.currentVehicle);
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x000E611D File Offset: 0x000E431D
		protected virtual void Update()
		{
			if (this.isOpen && GameInput.GetButtonDown(GameInput.ButtonCode.Escape))
			{
				this.Close();
			}
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000E6136 File Offset: 0x000E4336
		public void Close()
		{
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.orbitCam.Disable();
			Singleton<VehicleModMenu>.Instance.Close();
			this.currentVehicle = null;
		}

		// Token: 0x04002796 RID: 10134
		[Header("References")]
		[SerializeField]
		protected Transform vehiclePosition;

		// Token: 0x04002797 RID: 10135
		[SerializeField]
		protected OrbitCamera orbitCam;
	}
}
