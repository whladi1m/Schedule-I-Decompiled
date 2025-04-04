using System;
using UnityEngine;

namespace Beautify.Universal
{
	// Token: 0x020001EF RID: 495
	[ExecuteInEditMode]
	public class LUTBlending : MonoBehaviour
	{
		// Token: 0x06000AED RID: 2797 RVA: 0x00030223 File Offset: 0x0002E423
		private void OnEnable()
		{
			this.UpdateBeautifyLUT();
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0003022B File Offset: 0x0002E42B
		private void OnValidate()
		{
			this.oldPhase = -1f;
			this.UpdateBeautifyLUT();
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0003023E File Offset: 0x0002E43E
		private void OnDestroy()
		{
			if (this.rt != null)
			{
				this.rt.Release();
			}
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00030223 File Offset: 0x0002E423
		private void LateUpdate()
		{
			this.UpdateBeautifyLUT();
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0003025C File Offset: 0x0002E45C
		private void UpdateBeautifyLUT()
		{
			if (this.oldPhase == this.phase || this.LUT1 == null || this.LUT2 == null || this.lerpShader == null)
			{
				return;
			}
			this.oldPhase = this.phase;
			if (this.rt == null)
			{
				this.rt = new RenderTexture(this.LUT1.width, this.LUT1.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
				this.rt.filterMode = FilterMode.Point;
			}
			if (this.lerpMat == null)
			{
				this.lerpMat = new Material(this.lerpShader);
			}
			this.lerpMat.SetTexture(LUTBlending.ShaderParams.LUT2, this.LUT2);
			this.lerpMat.SetFloat(LUTBlending.ShaderParams.Phase, this.phase);
			Graphics.Blit(this.LUT1, this.rt, this.lerpMat);
			BeautifySettings.settings.lut.Override(true);
			float x = Mathf.Lerp(this.LUT1Intensity, this.LUT2Intensity, this.phase);
			BeautifySettings.settings.lutIntensity.Override(x);
			BeautifySettings.settings.lutTexture.Override(this.rt);
		}

		// Token: 0x04000BB6 RID: 2998
		public Texture2D LUT1;

		// Token: 0x04000BB7 RID: 2999
		public Texture2D LUT2;

		// Token: 0x04000BB8 RID: 3000
		[Range(0f, 1f)]
		public float LUT1Intensity = 1f;

		// Token: 0x04000BB9 RID: 3001
		[Range(0f, 1f)]
		public float LUT2Intensity = 1f;

		// Token: 0x04000BBA RID: 3002
		[Range(0f, 1f)]
		public float phase;

		// Token: 0x04000BBB RID: 3003
		public Shader lerpShader;

		// Token: 0x04000BBC RID: 3004
		private float oldPhase = -1f;

		// Token: 0x04000BBD RID: 3005
		private RenderTexture rt;

		// Token: 0x04000BBE RID: 3006
		private Material lerpMat;

		// Token: 0x020001F0 RID: 496
		private static class ShaderParams
		{
			// Token: 0x04000BBF RID: 3007
			public static int LUT2 = Shader.PropertyToID("_LUT2");

			// Token: 0x04000BC0 RID: 3008
			public static int Phase = Shader.PropertyToID("_Phase");
		}
	}
}
