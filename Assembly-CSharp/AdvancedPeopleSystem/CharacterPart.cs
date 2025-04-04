using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x020001FB RID: 507
	[Serializable]
	public class CharacterPart
	{
		// Token: 0x06000B52 RID: 2898 RVA: 0x00033F0B File Offset: 0x0003210B
		public CharacterPart()
		{
			this.skinnedMesh = new List<SkinnedMeshRenderer>();
		}

		// Token: 0x04000C0A RID: 3082
		public string name;

		// Token: 0x04000C0B RID: 3083
		public List<SkinnedMeshRenderer> skinnedMesh;
	}
}
