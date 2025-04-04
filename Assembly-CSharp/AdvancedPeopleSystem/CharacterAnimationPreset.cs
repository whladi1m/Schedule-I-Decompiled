using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000217 RID: 535
	[Serializable]
	public class CharacterAnimationPreset
	{
		// Token: 0x04000C96 RID: 3222
		public string name;

		// Token: 0x04000C97 RID: 3223
		public List<BlendshapeEmotionValue> blendshapes = new List<BlendshapeEmotionValue>();

		// Token: 0x04000C98 RID: 3224
		public bool UseGlobalBlendCurve = true;

		// Token: 0x04000C99 RID: 3225
		public AnimationCurve GlobalBlendAnimationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000C9A RID: 3226
		[HideInInspector]
		public float AnimationPlayDuration = 1f;

		// Token: 0x04000C9B RID: 3227
		[HideInInspector]
		public float weightPower = 1f;

		// Token: 0x04000C9C RID: 3228
		[Header("May decrease performance")]
		public bool applyToAllCharacterMeshes;
	}
}
