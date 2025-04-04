using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x02000943 RID: 2371
	[CreateAssetMenu(fileName = "Avatar Layer", menuName = "ScriptableObjects/Avatar Layer", order = 1)]
	[Serializable]
	public class AvatarLayer : ScriptableObject
	{
		// Token: 0x04002E7B RID: 11899
		public string Name;

		// Token: 0x04002E7C RID: 11900
		public string AssetPath;

		// Token: 0x04002E7D RID: 11901
		public Texture2D Texture;

		// Token: 0x04002E7E RID: 11902
		public Texture2D Normal;

		// Token: 0x04002E7F RID: 11903
		public Texture2D Normal_DefaultFormat;

		// Token: 0x04002E80 RID: 11904
		public int Order;

		// Token: 0x04002E81 RID: 11905
		public Material CombinedMaterial;
	}
}
