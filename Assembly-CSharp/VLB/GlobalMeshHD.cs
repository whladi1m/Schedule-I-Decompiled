using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200010E RID: 270
	public static class GlobalMeshHD
	{
		// Token: 0x0600042C RID: 1068 RVA: 0x00016D30 File Offset: 0x00014F30
		public static Mesh Get()
		{
			if (GlobalMeshHD.ms_Mesh == null)
			{
				GlobalMeshHD.Destroy();
				GlobalMeshHD.ms_Mesh = MeshGenerator.GenerateConeZ_Radii_DoubleCaps(1f, 1f, 1f, Config.Instance.sharedMeshSides, true);
				GlobalMeshHD.ms_Mesh.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
			}
			return GlobalMeshHD.ms_Mesh;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00016D87 File Offset: 0x00014F87
		public static void Destroy()
		{
			if (GlobalMeshHD.ms_Mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(GlobalMeshHD.ms_Mesh);
				GlobalMeshHD.ms_Mesh = null;
			}
		}

		// Token: 0x040005E2 RID: 1506
		private static Mesh ms_Mesh;
	}
}
