using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x0200018C RID: 396
	[CreateAssetMenu(fileName = "LightningArtSet.asset", menuName = "Sky Studio/Lightning/Lightning Art Set")]
	public class LightningArtSet : SpriteArtSet
	{
		// Token: 0x04000924 RID: 2340
		[Tooltip("List of lighting bolt art that will be used for customization.")]
		public List<LightningArtItem> lightingStyleItems;
	}
}
