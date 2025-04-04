using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000213 RID: 531
	public class CharacterPreBuilt : ScriptableObject
	{
		// Token: 0x04000C7A RID: 3194
		[SerializeField]
		public CharacterSettings settings;

		// Token: 0x04000C7B RID: 3195
		[SerializeField]
		public List<PreBuiltData> preBuiltDatas = new List<PreBuiltData>();

		// Token: 0x04000C7C RID: 3196
		[SerializeField]
		public List<PreBuiltBlendshape> blendshapes = new List<PreBuiltBlendshape>();
	}
}
