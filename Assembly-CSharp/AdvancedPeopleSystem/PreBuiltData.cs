using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000212 RID: 530
	[Serializable]
	public class PreBuiltData
	{
		// Token: 0x04000C77 RID: 3191
		[SerializeField]
		public string GroupName;

		// Token: 0x04000C78 RID: 3192
		[SerializeField]
		public List<Mesh> meshes = new List<Mesh>();

		// Token: 0x04000C79 RID: 3193
		[SerializeField]
		public List<Material> materials = new List<Material>();
	}
}
