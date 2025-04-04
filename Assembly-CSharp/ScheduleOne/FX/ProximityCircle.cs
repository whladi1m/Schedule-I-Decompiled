using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.FX
{
	// Token: 0x02000618 RID: 1560
	public class ProximityCircle : MonoBehaviour
	{
		// Token: 0x06002910 RID: 10512 RVA: 0x000A963A File Offset: 0x000A783A
		private void LateUpdate()
		{
			if (!this.enabledThisFrame)
			{
				this.SetAlpha(0f);
				this.enabledThisFrame = false;
			}
			this.enabledThisFrame = false;
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x000A965D File Offset: 0x000A785D
		public void SetRadius(float rad)
		{
			this.Circle.size = new Vector3(rad * 2f, rad * 2f, 3f);
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x000A9682 File Offset: 0x000A7882
		public void SetAlpha(float alpha)
		{
			this.enabledThisFrame = true;
			this.Circle.fadeFactor = alpha;
			this.Circle.enabled = (alpha > 0f);
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x000A96AA File Offset: 0x000A78AA
		public void SetColor(Color col)
		{
			this.Circle.material.color = col;
		}

		// Token: 0x04001E5A RID: 7770
		[Header("References")]
		public DecalProjector Circle;

		// Token: 0x04001E5B RID: 7771
		private bool enabledThisFrame;
	}
}
