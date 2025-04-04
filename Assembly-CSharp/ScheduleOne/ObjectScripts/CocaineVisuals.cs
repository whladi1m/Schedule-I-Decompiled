using System;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B83 RID: 2947
	public class CocaineVisuals : MonoBehaviour
	{
		// Token: 0x06004F7A RID: 20346 RVA: 0x0014F1A8 File Offset: 0x0014D3A8
		public void Setup(CocaineDefinition definition)
		{
			MeshRenderer[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].material = definition.RockMaterial;
			}
		}

		// Token: 0x04003BF9 RID: 15353
		public MeshRenderer[] Meshes;
	}
}
