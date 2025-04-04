using System;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000BCC RID: 3020
	public class WateringCanVisuals : MonoBehaviour
	{
		// Token: 0x060054DA RID: 21722 RVA: 0x00165358 File Offset: 0x00163558
		public virtual void SetFillLevel(float normalizedFillLevel)
		{
			this.WaterTransform.localPosition = new Vector3(this.WaterTransform.localPosition.x, Mathf.Lerp(this.WaterMinY, this.WaterMaxY, normalizedFillLevel), this.WaterTransform.localPosition.z);
			this.SideWaterTransform.localScale = new Vector3(Mathf.Lerp(this.SideWaterMinScale, this.SideWaterMaxScale, normalizedFillLevel), this.SideWaterTransform.localScale.y, this.SideWaterTransform.localScale.z);
			this.SideWaterTransform.localPosition = new Vector3(this.SideWaterTransform.localPosition.x, this.SideWaterTransform.localPosition.y, -this.SideWaterTransform.localScale.x * 0.5f);
		}

		// Token: 0x060054DB RID: 21723 RVA: 0x00165430 File Offset: 0x00163630
		public void SetOverflowParticles(bool enabled)
		{
			if (enabled)
			{
				if (!this.OverflowParticles.isPlaying)
				{
					this.OverflowParticles.Play();
					return;
				}
			}
			else if (this.OverflowParticles.isPlaying)
			{
				this.OverflowParticles.Stop();
			}
		}

		// Token: 0x04003ED7 RID: 16087
		public ParticleSystem OverflowParticles;

		// Token: 0x04003ED8 RID: 16088
		public Transform WaterTransform;

		// Token: 0x04003ED9 RID: 16089
		public float WaterMaxY;

		// Token: 0x04003EDA RID: 16090
		public float WaterMinY;

		// Token: 0x04003EDB RID: 16091
		public Transform SideWaterTransform;

		// Token: 0x04003EDC RID: 16092
		public float SideWaterMinScale;

		// Token: 0x04003EDD RID: 16093
		public float SideWaterMaxScale;

		// Token: 0x04003EDE RID: 16094
		public AudioSourceController FillSound;
	}
}
