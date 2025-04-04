using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000209 RID: 521
	[CreateAssetMenu(fileName = "NewCharacterGenerator", menuName = "Advanced People Pack/CharacterGenerator", order = 1)]
	public class CharacterGeneratorSettings : ScriptableObject
	{
		// Token: 0x04000C4D RID: 3149
		public MinMaxIndex hair;

		// Token: 0x04000C4E RID: 3150
		public MinMaxIndex beard;

		// Token: 0x04000C4F RID: 3151
		public MinMaxIndex hat;

		// Token: 0x04000C50 RID: 3152
		public MinMaxIndex accessory;

		// Token: 0x04000C51 RID: 3153
		public MinMaxIndex shirt;

		// Token: 0x04000C52 RID: 3154
		public MinMaxIndex pants;

		// Token: 0x04000C53 RID: 3155
		public MinMaxIndex shoes;

		// Token: 0x04000C54 RID: 3156
		[Space(10f)]
		public MinMaxColor skinColors = new MinMaxColor();

		// Token: 0x04000C55 RID: 3157
		public MinMaxColor eyeColors = new MinMaxColor();

		// Token: 0x04000C56 RID: 3158
		public MinMaxColor hairColors = new MinMaxColor();

		// Token: 0x04000C57 RID: 3159
		[Space(10f)]
		public MinMaxBlendshapes headSize;

		// Token: 0x04000C58 RID: 3160
		public MinMaxBlendshapes headOffset;

		// Token: 0x04000C59 RID: 3161
		public MinMaxBlendshapes height;

		// Token: 0x04000C5A RID: 3162
		public MinMaxBlendshapes fat;

		// Token: 0x04000C5B RID: 3163
		public MinMaxBlendshapes muscles;

		// Token: 0x04000C5C RID: 3164
		public MinMaxBlendshapes thin;

		// Token: 0x04000C5D RID: 3165
		[Space(15f)]
		public List<MinMaxFacialBlendshapes> facialBlendshapes = new List<MinMaxFacialBlendshapes>();

		// Token: 0x04000C5E RID: 3166
		[Space(15f)]
		public List<GeneratorExclude> excludes = new List<GeneratorExclude>();
	}
}
