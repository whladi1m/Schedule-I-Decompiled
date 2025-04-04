using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x020001FC RID: 508
	[Serializable]
	public class ClothesAnchor
	{
		// Token: 0x06000B53 RID: 2899 RVA: 0x00033F1E File Offset: 0x0003211E
		public ClothesAnchor()
		{
			this.skinnedMesh = new List<SkinnedMeshRenderer>();
		}

		// Token: 0x04000C0C RID: 3084
		public CharacterElementType partType;

		// Token: 0x04000C0D RID: 3085
		public List<SkinnedMeshRenderer> skinnedMesh;
	}
}
