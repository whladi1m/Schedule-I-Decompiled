using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000211 RID: 529
	[Serializable]
	public class PreBuiltBlendshape
	{
		// Token: 0x06000B78 RID: 2936 RVA: 0x000355AA File Offset: 0x000337AA
		public PreBuiltBlendshape(string name, float weight)
		{
			this.name = name;
			this.weight = weight;
		}

		// Token: 0x04000C75 RID: 3189
		[SerializeField]
		public string name;

		// Token: 0x04000C76 RID: 3190
		[SerializeField]
		public float weight;
	}
}
