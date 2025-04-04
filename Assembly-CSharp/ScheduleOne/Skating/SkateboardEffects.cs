using System;
using UnityEngine;

namespace ScheduleOne.Skating
{
	// Token: 0x020002D0 RID: 720
	[RequireComponent(typeof(Skateboard))]
	public class SkateboardEffects : MonoBehaviour
	{
		// Token: 0x06000F92 RID: 3986 RVA: 0x00045575 File Offset: 0x00043775
		private void Awake()
		{
			this.skateboard = base.GetComponent<Skateboard>();
			this.trailsOpacity = this.Trails[0].startColor.a;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0004559C File Offset: 0x0004379C
		private void FixedUpdate()
		{
			foreach (TrailRenderer trailRenderer in this.Trails)
			{
				Color startColor = trailRenderer.startColor;
				startColor.a = this.trailsOpacity * Mathf.Clamp01(this.skateboard.CurrentSpeed_Kmh / this.skateboard.TopSpeed_Kmh);
				trailRenderer.startColor = startColor;
			}
		}

		// Token: 0x0400103E RID: 4158
		private Skateboard skateboard;

		// Token: 0x0400103F RID: 4159
		[Header("References")]
		public TrailRenderer[] Trails;

		// Token: 0x04001040 RID: 4160
		private float trailsOpacity;
	}
}
