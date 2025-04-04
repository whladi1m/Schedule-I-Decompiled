using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007A8 RID: 1960
	public class ForkliftCamera : VehicleCamera
	{
		// Token: 0x0600352C RID: 13612 RVA: 0x000DF7B0 File Offset: 0x000DD9B0
		protected override void Update()
		{
			base.Update();
			this.forkliftCamActive = false;
			if (this.vehicle.localPlayerIsDriver && Input.GetKey(KeyCode.LeftShift))
			{
				this.forkliftCamActive = true;
			}
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x000DF7E0 File Offset: 0x000DD9E0
		protected override void LateUpdate()
		{
			base.LateUpdate();
			this.guidanceLight.enabled = false;
			if (this.vehicle.localPlayerIsDriver && this.forkliftCamActive)
			{
				PlayerSingleton<PlayerCamera>.Instance.transform.position = this.forkCamPos.position;
				PlayerSingleton<PlayerCamera>.Instance.transform.rotation = this.forkCamPos.rotation;
				this.guidanceLight.enabled = true;
			}
		}

		// Token: 0x0400265B RID: 9819
		[Header("Forklift References")]
		[SerializeField]
		protected Transform forkCamPos;

		// Token: 0x0400265C RID: 9820
		[SerializeField]
		protected Light guidanceLight;

		// Token: 0x0400265D RID: 9821
		protected bool forkliftCamActive;
	}
}
