using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001CF RID: 463
	[RequireComponent(typeof(Camera))]
	public class RenderCloudCubemap : MonoBehaviour
	{
		// Token: 0x04000B32 RID: 2866
		public const string kDefaultFilenamePrefix = "CloudCubemap";

		// Token: 0x04000B33 RID: 2867
		[Tooltip("Filename of the final output cubemap asset. It will be written to the same directory as the current scene.")]
		public string filenamePrefix = "CloudCubemap";

		// Token: 0x04000B34 RID: 2868
		[Tooltip("Resolution of each face of the cubemap.")]
		public int faceWidth = 1024;

		// Token: 0x04000B35 RID: 2869
		[Tooltip("Format for the exported cubemap. RGBColor (Additive texture), RGBAColor (Color with alpha channel), RGBANormal (Normal lighting data encoded).")]
		public RenderCloudCubemap.CubemapTextureFormat textureFormat = RenderCloudCubemap.CubemapTextureFormat.RGBALit;

		// Token: 0x04000B36 RID: 2870
		public bool exportFaces;

		// Token: 0x020001D0 RID: 464
		public enum CubemapTextureFormat
		{
			// Token: 0x04000B38 RID: 2872
			RGBColor,
			// Token: 0x04000B39 RID: 2873
			RGBAColor,
			// Token: 0x04000B3A RID: 2874
			RGBALit
		}
	}
}
