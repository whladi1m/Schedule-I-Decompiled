using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A04 RID: 2564
	public class EyelidOverlay : Singleton<EyelidOverlay>
	{
		// Token: 0x0600453B RID: 17723 RVA: 0x001222A8 File Offset: 0x001204A8
		protected override void Awake()
		{
			base.Awake();
			this.OpenMultiplier.Initialize();
			this.SetOpen(1f);
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x001222C8 File Offset: 0x001204C8
		private void Update()
		{
			if (Player.Local == null)
			{
				return;
			}
			if (this.AutoUpdate)
			{
				if (Player.Local.Energy.CurrentEnergy < 20f)
				{
					this.CurrentOpen = Mathf.Lerp(0.625f, 1f, Player.Local.Energy.CurrentEnergy / 20f);
				}
				else
				{
					this.CurrentOpen = 1f;
				}
			}
			this.SetOpen(this.CurrentOpen * this.OpenMultiplier.CurrentValue);
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x00122350 File Offset: 0x00120550
		public void SetOpen(float openness)
		{
			this.CurrentOpen = openness;
			this.Upper.anchoredPosition = new Vector2(0f, Mathf.Lerp(this.Closed, this.Open, openness));
			this.Lower.anchoredPosition = new Vector2(0f, -Mathf.Lerp(this.Closed, this.Open, openness));
			this.Canvas.enabled = (openness < 1f);
		}

		// Token: 0x04003316 RID: 13078
		public const float MaxTiredOpenAmount = 0.625f;

		// Token: 0x04003317 RID: 13079
		public bool AutoUpdate = true;

		// Token: 0x04003318 RID: 13080
		[Header("Settings")]
		public float Open = 400f;

		// Token: 0x04003319 RID: 13081
		public float Closed = 30f;

		// Token: 0x0400331A RID: 13082
		[Header("References")]
		public RectTransform Upper;

		// Token: 0x0400331B RID: 13083
		public RectTransform Lower;

		// Token: 0x0400331C RID: 13084
		public Canvas Canvas;

		// Token: 0x0400331D RID: 13085
		[Range(0f, 1f)]
		public float CurrentOpen = 1f;

		// Token: 0x0400331E RID: 13086
		public FloatSmoother OpenMultiplier;
	}
}
