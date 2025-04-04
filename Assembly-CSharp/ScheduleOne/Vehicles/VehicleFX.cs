using System;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007BC RID: 1980
	public class VehicleFX : MonoBehaviour
	{
		// Token: 0x06003632 RID: 13874 RVA: 0x000E44A8 File Offset: 0x000E26A8
		public virtual void OnVehicleStart()
		{
			ParticleSystem[] array = this.exhaustFX;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Play();
			}
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000E44D4 File Offset: 0x000E26D4
		public virtual void OnVehicleStop()
		{
			ParticleSystem[] array = this.exhaustFX;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Stop();
			}
		}

		// Token: 0x04002706 RID: 9990
		public ParticleSystem[] exhaustFX;
	}
}
