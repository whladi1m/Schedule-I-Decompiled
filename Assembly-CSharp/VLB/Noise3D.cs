using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000133 RID: 307
	public static class Noise3D
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0001947E File Offset: 0x0001767E
		public static bool isSupported
		{
			get
			{
				if (!Noise3D.ms_IsSupportedChecked)
				{
					Noise3D.ms_IsSupported = (SystemInfo.graphicsShaderLevel >= 35);
					if (!Noise3D.ms_IsSupported)
					{
						Debug.LogWarning(Noise3D.isNotSupportedString);
					}
					Noise3D.ms_IsSupportedChecked = true;
				}
				return Noise3D.ms_IsSupported;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x000194B4 File Offset: 0x000176B4
		public static bool isProperlyLoaded
		{
			get
			{
				return Noise3D.ms_NoiseTexture != null;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x000194C1 File Offset: 0x000176C1
		public static string isNotSupportedString
		{
			get
			{
				return string.Format("3D Noise requires higher shader capabilities (Shader Model 3.5 / OpenGL ES 3.0), which are not available on the current platform: graphicsShaderLevel (current/required) = {0} / {1}", SystemInfo.graphicsShaderLevel, 35);
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x000194DE File Offset: 0x000176DE
		[RuntimeInitializeOnLoadMethod]
		private static void OnStartUp()
		{
			Noise3D.LoadIfNeeded();
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x000194E8 File Offset: 0x000176E8
		public static void LoadIfNeeded()
		{
			if (!Noise3D.isSupported)
			{
				return;
			}
			if (Noise3D.ms_NoiseTexture == null)
			{
				Noise3D.ms_NoiseTexture = Config.Instance.noiseTexture3D;
				Shader.SetGlobalTexture(ShaderProperties.GlobalNoiseTex3D, Noise3D.ms_NoiseTexture);
				Shader.SetGlobalFloat(ShaderProperties.GlobalNoiseCustomTime, -1f);
			}
		}

		// Token: 0x0400067D RID: 1661
		private static bool ms_IsSupportedChecked;

		// Token: 0x0400067E RID: 1662
		private static bool ms_IsSupported;

		// Token: 0x0400067F RID: 1663
		private static Texture3D ms_NoiseTexture;

		// Token: 0x04000680 RID: 1664
		private const int kMinShaderLevel = 35;
	}
}
