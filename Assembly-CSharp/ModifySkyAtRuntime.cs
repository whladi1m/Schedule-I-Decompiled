using System;
using Funly.SkyStudio;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class ModifySkyAtRuntime : MonoBehaviour
{
	// Token: 0x06000102 RID: 258 RVA: 0x000064E4 File Offset: 0x000046E4
	private void Update()
	{
		SkyProfile skyProfile = TimeOfDayController.instance.skyProfile;
		ColorKeyframe colorKeyframe = skyProfile.GetGroup<ColorKeyframeGroup>("SkyMiddleColorKey").keyframes[0];
		float h = Time.timeSinceLevelLoad * this.speed % 1f;
		colorKeyframe.color = Color.HSVToRGB(h, 0.8f, 0.8f);
		skyProfile.GetGroup<ColorKeyframeGroup>("SkyUpperColorKey").keyframes[0].color = colorKeyframe.color;
		TimeOfDayController.instance.UpdateSkyForCurrentTime();
	}

	// Token: 0x040000E6 RID: 230
	[Range(0f, 1f)]
	public float speed = 0.15f;
}
