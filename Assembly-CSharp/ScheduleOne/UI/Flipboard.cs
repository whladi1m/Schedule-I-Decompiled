using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009C7 RID: 2503
	public class Flipboard : MonoBehaviour
	{
		// Token: 0x060043AE RID: 17326 RVA: 0x0011BD1C File Offset: 0x00119F1C
		public void Update()
		{
			this.time += Time.deltaTime * this.SpeedMultiplier;
			if (this.time >= this.FlipTime)
			{
				this.time = 0f;
				this.index = (this.index + 1) % this.Sprites.Length;
				this.Image.sprite = this.Sprites[this.index];
			}
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x0011BD8A File Offset: 0x00119F8A
		public void SetIndex(int index)
		{
			this.index = index;
			this.time = 0f;
			this.Image.sprite = this.Sprites[index];
		}

		// Token: 0x04003179 RID: 12665
		public Sprite[] Sprites;

		// Token: 0x0400317A RID: 12666
		public Image Image;

		// Token: 0x0400317B RID: 12667
		public float FlipTime = 0.2f;

		// Token: 0x0400317C RID: 12668
		public float SpeedMultiplier = 1f;

		// Token: 0x0400317D RID: 12669
		private float time;

		// Token: 0x0400317E RID: 12670
		private int index;
	}
}
