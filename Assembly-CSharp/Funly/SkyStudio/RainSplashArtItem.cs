using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x0200018D RID: 397
	[CreateAssetMenu(fileName = "rainSplashArtItem.asset", menuName = "Sky Studio/Rain/Rain Splash Art Item")]
	public class RainSplashArtItem : SpriteArtItem
	{
		// Token: 0x04000925 RID: 2341
		[Range(0f, 1f)]
		public float intensityMultiplier = 1f;

		// Token: 0x04000926 RID: 2342
		[Range(0f, 1f)]
		public float scaleMultiplier = 1f;
	}
}
