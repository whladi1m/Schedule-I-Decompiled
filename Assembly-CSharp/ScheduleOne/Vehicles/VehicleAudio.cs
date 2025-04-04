using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007B8 RID: 1976
	public class VehicleAudio : MonoBehaviour
	{
		// Token: 0x0600361C RID: 13852 RVA: 0x000E3BD8 File Offset: 0x000E1DD8
		protected virtual void Awake()
		{
			if (this.Vehicle != null)
			{
				this.Vehicle.onVehicleStart.AddListener(new UnityAction(this.EngineStart));
				this.Vehicle.onVehicleStop.AddListener(new UnityAction(this.EngineStart));
			}
			if (this.Lights != null)
			{
				this.Lights.onHeadlightsOn.AddListener(new UnityAction(this.HeadlightsToggledOn));
				this.Lights.onHeadlightsOff.AddListener(new UnityAction(this.HeadlightsToggledOff));
			}
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void EngineStart()
		{
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void EngineStop()
		{
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void HeadlightsToggledOn()
		{
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void HeadlightsToggledOff()
		{
		}

		// Token: 0x040026E8 RID: 9960
		[Header("Refererences")]
		public LandVehicle Vehicle;

		// Token: 0x040026E9 RID: 9961
		public VehicleLights Lights;

		// Token: 0x040026EA RID: 9962
		[Header("Sounds")]
		public AudioSource EngineStartSound;

		// Token: 0x040026EB RID: 9963
		public AudioSource EngineStopSound;

		// Token: 0x040026EC RID: 9964
		public AudioSource HeadlightsOnSound;

		// Token: 0x040026ED RID: 9965
		public AudioSource HeadlightsOffSound;

		// Token: 0x040026EE RID: 9966
		public AudioSource HornSound;
	}
}
