using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000B5 RID: 181
	[Serializable]
	public class DepthOfFieldModel : PostProcessingModel
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00012F16 File Offset: 0x00011116
		// (set) Token: 0x0600031A RID: 794 RVA: 0x00012F1E File Offset: 0x0001111E
		public DepthOfFieldModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00012F27 File Offset: 0x00011127
		public override void Reset()
		{
			this.m_Settings = DepthOfFieldModel.Settings.defaultSettings;
		}

		// Token: 0x040003E9 RID: 1001
		[SerializeField]
		private DepthOfFieldModel.Settings m_Settings = DepthOfFieldModel.Settings.defaultSettings;

		// Token: 0x020000B6 RID: 182
		public enum KernelSize
		{
			// Token: 0x040003EB RID: 1003
			Small,
			// Token: 0x040003EC RID: 1004
			Medium,
			// Token: 0x040003ED RID: 1005
			Large,
			// Token: 0x040003EE RID: 1006
			VeryLarge
		}

		// Token: 0x020000B7 RID: 183
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000066 RID: 102
			// (get) Token: 0x0600031D RID: 797 RVA: 0x00012F48 File Offset: 0x00011148
			public static DepthOfFieldModel.Settings defaultSettings
			{
				get
				{
					return new DepthOfFieldModel.Settings
					{
						focusDistance = 10f,
						aperture = 5.6f,
						focalLength = 50f,
						useCameraFov = false,
						kernelSize = DepthOfFieldModel.KernelSize.Medium
					};
				}
			}

			// Token: 0x040003EF RID: 1007
			[Min(0.1f)]
			[Tooltip("Distance to the point of focus.")]
			public float focusDistance;

			// Token: 0x040003F0 RID: 1008
			[Range(0.05f, 32f)]
			[Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
			public float aperture;

			// Token: 0x040003F1 RID: 1009
			[Range(1f, 300f)]
			[Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
			public float focalLength;

			// Token: 0x040003F2 RID: 1010
			[Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera. Using this setting isn't recommended.")]
			public bool useCameraFov;

			// Token: 0x040003F3 RID: 1011
			[Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")]
			public DepthOfFieldModel.KernelSize kernelSize;
		}
	}
}
