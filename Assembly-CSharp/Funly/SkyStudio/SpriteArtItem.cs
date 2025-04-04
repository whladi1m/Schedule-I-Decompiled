using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x0200018F RID: 399
	public class SpriteArtItem : ScriptableObject
	{
		// Token: 0x04000928 RID: 2344
		public Mesh mesh;

		// Token: 0x04000929 RID: 2345
		public Material material;

		// Token: 0x0400092A RID: 2346
		public int rows;

		// Token: 0x0400092B RID: 2347
		public int columns;

		// Token: 0x0400092C RID: 2348
		public int totalFrames;

		// Token: 0x0400092D RID: 2349
		public int animateSpeed;

		// Token: 0x0400092E RID: 2350
		[Tooltip("Color that will be multiplied against the base lightning bolt text color")]
		public Color tintColor = Color.white;
	}
}
