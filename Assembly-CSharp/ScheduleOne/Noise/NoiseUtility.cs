using System;
using UnityEngine;

namespace ScheduleOne.Noise
{
	// Token: 0x0200052F RID: 1327
	public static class NoiseUtility
	{
		// Token: 0x06002081 RID: 8321 RVA: 0x000859E0 File Offset: 0x00083BE0
		public static void EmitNoise(Vector3 origin, ENoiseType type, float range, GameObject source = null)
		{
			NoiseEvent nEvent = new NoiseEvent(origin, range, type, source);
			for (int i = 0; i < Listener.listeners.Count; i++)
			{
				if (Listener.listeners[i].enabled && Vector3.Magnitude(origin - Listener.listeners[i].HearingOrigin.position) <= Listener.listeners[i].Sensitivity * range)
				{
					Listener.listeners[i].Notify(nEvent);
				}
			}
		}
	}
}
