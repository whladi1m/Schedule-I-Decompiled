using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BEF RID: 3055
	public class ManorGate : Gate
	{
		// Token: 0x06005591 RID: 21905 RVA: 0x00167F3E File Offset: 0x0016613E
		protected virtual void Start()
		{
			this.SetIntercomActive(false);
			this.SetEnterable(false);
			base.InvokeRepeating("UpdateDetection", 0f, 0.25f);
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x00167F64 File Offset: 0x00166164
		private void UpdateDetection()
		{
			bool flag = false;
			if (this.ExteriorVehicleDetector.AreAnyVehiclesOccupied())
			{
				flag = true;
			}
			if (this.ExteriorPlayerDetector.DetectedPlayers.Count > 0)
			{
				flag = true;
			}
			if (this.InteriorVehicleDetector.AreAnyVehiclesOccupied())
			{
				flag = true;
			}
			if (this.InteriorPlayerDetector.DetectedPlayers.Count > 0)
			{
				flag = true;
			}
			if (flag != base.IsOpen)
			{
				if (flag)
				{
					base.Open();
					return;
				}
				base.Close();
			}
		}

		// Token: 0x06005593 RID: 21907 RVA: 0x00167FD4 File Offset: 0x001661D4
		public void IntercomBuzzed()
		{
			this.SetIntercomActive(false);
		}

		// Token: 0x06005594 RID: 21908 RVA: 0x00167FDD File Offset: 0x001661DD
		public void SetEnterable(bool enterable)
		{
			this.ExteriorPlayerDetector.SetIgnoreNewCollisions(!enterable);
			this.ExteriorVehicleDetector.SetIgnoreNewCollisions(!enterable);
			this.ExteriorVehicleDetector.vehicles.Clear();
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x0016800D File Offset: 0x0016620D
		[Button]
		public void ActivateIntercom()
		{
			this.SetIntercomActive(true);
		}

		// Token: 0x06005596 RID: 21910 RVA: 0x00168016 File Offset: 0x00166216
		public void SetIntercomActive(bool active)
		{
			this.intercomActive = active;
			this.UpdateIntercom();
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x00168025 File Offset: 0x00166225
		private void UpdateIntercom()
		{
			this.IntercomInt.SetInteractableState(this.intercomActive ? InteractableObject.EInteractableState.Default : InteractableObject.EInteractableState.Disabled);
			this.IntercomLight.enabled = this.intercomActive;
		}

		// Token: 0x04003F92 RID: 16274
		[Header("References")]
		public InteractableObject IntercomInt;

		// Token: 0x04003F93 RID: 16275
		public Light IntercomLight;

		// Token: 0x04003F94 RID: 16276
		public VehicleDetector ExteriorVehicleDetector;

		// Token: 0x04003F95 RID: 16277
		public PlayerDetector ExteriorPlayerDetector;

		// Token: 0x04003F96 RID: 16278
		public VehicleDetector InteriorVehicleDetector;

		// Token: 0x04003F97 RID: 16279
		public PlayerDetector InteriorPlayerDetector;

		// Token: 0x04003F98 RID: 16280
		private bool intercomActive;
	}
}
