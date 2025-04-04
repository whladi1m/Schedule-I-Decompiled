using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x0200018A RID: 394
	[CreateAssetMenu(fileName = "lightningArtItem.asset", menuName = "Sky Studio/Lightning/Lightning Art Item")]
	public class LightningArtItem : SpriteArtItem
	{
		// Token: 0x0400091C RID: 2332
		[Tooltip("Adjust how the lightning bolt is positioned inside the spawn area container.")]
		public LightningArtItem.Alignment alignment;

		// Token: 0x0400091D RID: 2333
		[Tooltip("Thunder sound clip to play when this lighting bolt is rendered.")]
		public AudioClip thunderSound;

		// Token: 0x0400091E RID: 2334
		[Tooltip("Probability adjustment for this specific lightning bolt. This value is multiplied against the global lightning probability.")]
		[Range(0f, 1f)]
		public float strikeProbability = 1f;

		// Token: 0x0400091F RID: 2335
		[Range(0f, 60f)]
		[Tooltip("Size of the lighting bolt.")]
		public float size = 20f;

		// Token: 0x04000920 RID: 2336
		[Range(0f, 1f)]
		[Tooltip("The blending weight of the additive lighting bolt effect")]
		public float intensity = 1f;

		// Token: 0x0200018B RID: 395
		public enum Alignment
		{
			// Token: 0x04000922 RID: 2338
			ScaleToFit,
			// Token: 0x04000923 RID: 2339
			TopAlign
		}
	}
}
