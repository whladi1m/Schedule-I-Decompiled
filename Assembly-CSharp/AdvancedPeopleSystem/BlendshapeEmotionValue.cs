using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x020001FD RID: 509
	[Serializable]
	public class BlendshapeEmotionValue
	{
		// Token: 0x04000C0E RID: 3086
		public CharacterBlendShapeType BlendType;

		// Token: 0x04000C0F RID: 3087
		[Range(-100f, 100f)]
		public float BlendValue;

		// Token: 0x04000C10 RID: 3088
		public AnimationCurve BlendAnimationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 0f)
		});
	}
}
