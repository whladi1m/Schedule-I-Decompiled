using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000140 RID: 320
	public static class GlobalMeshSD
	{
		// Token: 0x060005C2 RID: 1474 RVA: 0x0001B484 File Offset: 0x00019684
		public static Mesh Get()
		{
			bool sd_requiresDoubleSidedMesh = Config.Instance.SD_requiresDoubleSidedMesh;
			if (GlobalMeshSD.ms_Mesh == null || GlobalMeshSD.ms_DoubleSided != sd_requiresDoubleSidedMesh)
			{
				GlobalMeshSD.Destroy();
				GlobalMeshSD.ms_Mesh = MeshGenerator.GenerateConeZ_Radii(1f, 1f, 1f, Config.Instance.sharedMeshSides, Config.Instance.sharedMeshSegments, true, sd_requiresDoubleSidedMesh);
				GlobalMeshSD.ms_Mesh.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
				GlobalMeshSD.ms_DoubleSided = sd_requiresDoubleSidedMesh;
			}
			return GlobalMeshSD.ms_Mesh;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001B4FF File Offset: 0x000196FF
		public static void Destroy()
		{
			if (GlobalMeshSD.ms_Mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(GlobalMeshSD.ms_Mesh);
				GlobalMeshSD.ms_Mesh = null;
			}
		}

		// Token: 0x040006BC RID: 1724
		private static Mesh ms_Mesh;

		// Token: 0x040006BD RID: 1725
		private static bool ms_DoubleSided;
	}
}
