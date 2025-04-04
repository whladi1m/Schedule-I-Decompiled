using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ardenfall.Utilities
{
	// Token: 0x02000227 RID: 551
	[CreateAssetMenu(menuName = "Ardenfall/Foliage/Billboard Asset")]
	public class BillboardAsset : ScriptableObject
	{
		// Token: 0x04000D20 RID: 3360
		public GameObject prefab;

		// Token: 0x04000D21 RID: 3361
		public BillboardRenderSettings renderSettings;

		// Token: 0x04000D22 RID: 3362
		[Header("Values")]
		public int textureSize = 512;

		// Token: 0x04000D23 RID: 3363
		public float cutoff = 0.15f;

		// Token: 0x04000D24 RID: 3364
		[Header("LODs")]
		public bool pickLastLOD = true;

		// Token: 0x04000D25 RID: 3365
		public int LODIndex;

		// Token: 0x04000D26 RID: 3366
		[HideInInspector]
		public List<Texture2D> generatedTextures;

		// Token: 0x04000D27 RID: 3367
		[HideInInspector]
		public Mesh generatedMesh;

		// Token: 0x04000D28 RID: 3368
		[HideInInspector]
		public Material generatedMaterial;

		// Token: 0x04000D29 RID: 3369
		[HideInInspector]
		public GameObject generatedPrefab;
	}
}
