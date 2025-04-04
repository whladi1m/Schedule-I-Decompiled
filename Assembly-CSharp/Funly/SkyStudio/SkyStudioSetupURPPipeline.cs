using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Funly.SkyStudio
{
	// Token: 0x020001E7 RID: 487
	[ExecuteInEditMode]
	public class SkyStudioSetupURPPipeline : MonoBehaviour
	{
		// Token: 0x06000ACF RID: 2767 RVA: 0x0002FDE5 File Offset: 0x0002DFE5
		private void Update()
		{
			if (this.pipelineAsset != GraphicsSettings.renderPipelineAsset)
			{
				GraphicsSettings.renderPipelineAsset = this.pipelineAsset;
				QualitySettings.renderPipeline = this.pipelineAsset;
			}
		}

		// Token: 0x04000BB1 RID: 2993
		[HelpBox("For URP projects, Sky Studio will assign this rendering pipeline into GraphicsSettings. We have to install this pipeline so that we can embed our own custom render features, which are required for certain Sky Studio features like rain splashes to work properly. If you need to add rendering features, or customize the rendering pipeline asset please update this reference, and ensure that the 'SkyStudio-WeatherDepthForwardRenderer' is assigned to render feature index 1. Feel free to add any custom render features after index 1.", HelpBoxMessageType.Info)]
		[Tooltip("The rendering pipeline that will be assigned into the graphics settings when this scene becomes active.")]
		public RenderPipelineAsset pipelineAsset;
	}
}
