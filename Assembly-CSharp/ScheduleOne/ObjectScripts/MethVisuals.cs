using System;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B84 RID: 2948
	public class MethVisuals : MonoBehaviour
	{
		// Token: 0x06004F7C RID: 20348 RVA: 0x0014F1D8 File Offset: 0x0014D3D8
		public void Setup(MethDefinition definition)
		{
			MeshRenderer[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].material = definition.CrystalMaterial;
			}
		}

		// Token: 0x04003BFA RID: 15354
		public MeshRenderer[] Meshes;
	}
}
