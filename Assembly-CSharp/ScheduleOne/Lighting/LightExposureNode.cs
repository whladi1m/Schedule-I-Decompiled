using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x02000599 RID: 1433
	public class LightExposureNode : MonoBehaviour
	{
		// Token: 0x060023B2 RID: 9138 RVA: 0x00091018 File Offset: 0x0008F218
		public float GetTotalExposure(out float growSpeedMultiplier)
		{
			float num = this.ambientExposure;
			int num2 = 0;
			growSpeedMultiplier = 0f;
			foreach (UsableLightSource usableLightSource in this.sources.Keys)
			{
				if (usableLightSource != null && usableLightSource.isEmitting)
				{
					num2++;
					num += this.sources[usableLightSource];
					growSpeedMultiplier += usableLightSource.GrowSpeedMultiplier;
				}
			}
			if (num2 > 0)
			{
				growSpeedMultiplier /= (float)num2;
			}
			return num;
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x000910B4 File Offset: 0x0008F2B4
		public void AddSource(UsableLightSource source, float lightAmount)
		{
			if (this.sources.ContainsKey(source))
			{
				this.sources[source] = lightAmount;
				return;
			}
			this.sources.Add(source, lightAmount);
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000910DF File Offset: 0x0008F2DF
		public void RemoveSource(UsableLightSource source)
		{
			this.sources.Remove(source);
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000910F0 File Offset: 0x0008F2F0
		private void OnDrawGizmos()
		{
			float num;
			float totalExposure = this.GetTotalExposure(out num);
			if (totalExposure > this.ambientExposure)
			{
				Gizmos.color = new Color(1f, 1f, 1f, totalExposure);
				Gizmos.DrawSphere(base.transform.position, 0.1f);
			}
		}

		// Token: 0x04001AA2 RID: 6818
		public float ambientExposure;

		// Token: 0x04001AA3 RID: 6819
		public Dictionary<UsableLightSource, float> sources = new Dictionary<UsableLightSource, float>();
	}
}
