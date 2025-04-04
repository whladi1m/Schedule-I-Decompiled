using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200085A RID: 2138
	public class SetRendererMaterial : MonoBehaviour
	{
		// Token: 0x06003A3D RID: 14909 RVA: 0x000F5218 File Offset: 0x000F3418
		[Button]
		public void SetMaterial()
		{
			foreach (MeshRenderer meshRenderer in base.GetComponentsInChildren<MeshRenderer>())
			{
				Material[] sharedMaterials = meshRenderer.sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					sharedMaterials[j] = this.Material;
				}
				meshRenderer.sharedMaterials = sharedMaterials;
			}
		}

		// Token: 0x04002A0B RID: 10763
		public Material Material;
	}
}
