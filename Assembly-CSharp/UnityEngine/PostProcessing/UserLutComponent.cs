using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000090 RID: 144
	public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0001209C File Offset: 0x0001029C
		public override bool active
		{
			get
			{
				UserLutModel.Settings settings = base.model.settings;
				return base.model.enabled && settings.lut != null && settings.contribution > 0f && settings.lut.height == (int)Mathf.Sqrt((float)settings.lut.width) && !this.context.interrupted;
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0001210C File Offset: 0x0001030C
		public override void Prepare(Material uberMaterial)
		{
			UserLutModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("USER_LUT");
			uberMaterial.SetTexture(UserLutComponent.Uniforms._UserLut, settings.lut);
			uberMaterial.SetVector(UserLutComponent.Uniforms._UserLut_Params, new Vector4(1f / (float)settings.lut.width, 1f / (float)settings.lut.height, (float)settings.lut.height - 1f, settings.contribution));
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00012190 File Offset: 0x00010390
		public void OnGUI()
		{
			UserLutModel.Settings settings = base.model.settings;
			GUI.DrawTexture(new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)settings.lut.width, (float)settings.lut.height), settings.lut);
		}

		// Token: 0x02000091 RID: 145
		private static class Uniforms
		{
			// Token: 0x04000361 RID: 865
			internal static readonly int _UserLut = Shader.PropertyToID("_UserLut");

			// Token: 0x04000362 RID: 866
			internal static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");
		}
	}
}
