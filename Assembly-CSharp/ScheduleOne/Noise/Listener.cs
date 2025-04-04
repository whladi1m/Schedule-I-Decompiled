using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Noise
{
	// Token: 0x0200052B RID: 1323
	public class Listener : MonoBehaviour
	{
		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06002074 RID: 8308 RVA: 0x0008591B File Offset: 0x00083B1B
		// (set) Token: 0x06002075 RID: 8309 RVA: 0x00085923 File Offset: 0x00083B23
		public float SquaredHearingRange { get; protected set; }

		// Token: 0x06002076 RID: 8310 RVA: 0x0008592C File Offset: 0x00083B2C
		public void Awake()
		{
			this.SquaredHearingRange = Mathf.Pow(this.Sensitivity, 2f);
			if (this.HearingOrigin == null)
			{
				this.HearingOrigin = base.transform;
			}
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x0008595E File Offset: 0x00083B5E
		public void OnEnable()
		{
			if (!Listener.listeners.Contains(this))
			{
				Listener.listeners.Add(this);
			}
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x00085978 File Offset: 0x00083B78
		public void OnDisable()
		{
			Listener.listeners.Remove(this);
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x00085986 File Offset: 0x00083B86
		public void Notify(NoiseEvent nEvent)
		{
			if (this.onNoiseHeard != null)
			{
				this.onNoiseHeard(nEvent);
			}
		}

		// Token: 0x0400191A RID: 6426
		public static List<Listener> listeners = new List<Listener>();

		// Token: 0x0400191B RID: 6427
		[Header("Settings")]
		[Range(0.1f, 5f)]
		public float Sensitivity = 1f;

		// Token: 0x0400191C RID: 6428
		public Transform HearingOrigin;

		// Token: 0x0400191E RID: 6430
		public Listener.HearingEvent onNoiseHeard;

		// Token: 0x0200052C RID: 1324
		// (Invoke) Token: 0x0600207D RID: 8317
		public delegate void HearingEvent(NoiseEvent nEvent);
	}
}
