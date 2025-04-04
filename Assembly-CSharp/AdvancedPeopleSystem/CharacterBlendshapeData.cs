using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000218 RID: 536
	[Serializable]
	public class CharacterBlendshapeData
	{
		// Token: 0x06000B7F RID: 2943 RVA: 0x00035722 File Offset: 0x00033922
		public CharacterBlendshapeData(string name, CharacterBlendShapeType t, CharacterBlendShapeGroup g, float value = 0f)
		{
			this.blendshapeName = name;
			this.type = t;
			this.group = g;
			this.value = value;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0000494F File Offset: 0x00002B4F
		public CharacterBlendshapeData()
		{
		}

		// Token: 0x04000C9D RID: 3229
		public string blendshapeName;

		// Token: 0x04000C9E RID: 3230
		public CharacterBlendShapeType type;

		// Token: 0x04000C9F RID: 3231
		public CharacterBlendShapeGroup group;

		// Token: 0x04000CA0 RID: 3232
		[HideInInspector]
		public float value;
	}
}
