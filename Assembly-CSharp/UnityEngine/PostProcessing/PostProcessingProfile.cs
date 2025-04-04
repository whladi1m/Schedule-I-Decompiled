using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D6 RID: 214
	public class PostProcessingProfile : ScriptableObject
	{
		// Token: 0x0400045B RID: 1115
		public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();

		// Token: 0x0400045C RID: 1116
		public FogModel fog = new FogModel();

		// Token: 0x0400045D RID: 1117
		public AntialiasingModel antialiasing = new AntialiasingModel();

		// Token: 0x0400045E RID: 1118
		public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();

		// Token: 0x0400045F RID: 1119
		public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();

		// Token: 0x04000460 RID: 1120
		public DepthOfFieldModel depthOfField = new DepthOfFieldModel();

		// Token: 0x04000461 RID: 1121
		public MotionBlurModel motionBlur = new MotionBlurModel();

		// Token: 0x04000462 RID: 1122
		public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();

		// Token: 0x04000463 RID: 1123
		public BloomModel bloom = new BloomModel();

		// Token: 0x04000464 RID: 1124
		public ColorGradingModel colorGrading = new ColorGradingModel();

		// Token: 0x04000465 RID: 1125
		public UserLutModel userLut = new UserLutModel();

		// Token: 0x04000466 RID: 1126
		public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();

		// Token: 0x04000467 RID: 1127
		public GrainModel grain = new GrainModel();

		// Token: 0x04000468 RID: 1128
		public VignetteModel vignette = new VignetteModel();

		// Token: 0x04000469 RID: 1129
		public DitheringModel dithering = new DitheringModel();
	}
}
