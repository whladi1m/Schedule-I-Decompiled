using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BE1 RID: 3041
	public class Grave : MonoBehaviour
	{
		// Token: 0x06005549 RID: 21833 RVA: 0x00166EB0 File Offset: 0x001650B0
		[Button]
		public void RandomizeGrave()
		{
			int num = UnityEngine.Random.Range(0, this.Surfaces.Length);
			int num2 = UnityEngine.Random.Range(0, this.HeadstoneObjects.Length);
			for (int i = 0; i < this.Surfaces.Length; i++)
			{
				this.Surfaces[i].Object.SetActive(i == num);
			}
			for (int j = 0; j < this.HeadstoneObjects.Length; j++)
			{
				this.HeadstoneObjects[j].SetActive(j == num2);
			}
			int num3 = UnityEngine.Random.Range(0, this.Surfaces[num].Materials.Length);
			int num4 = UnityEngine.Random.Range(0, this.HeadstoneMaterials.Length);
			this.Surfaces[num].Mesh.material = this.Surfaces[num].Materials[num3];
			for (int k = 0; k < this.HeadstoneMeshes.Length; k++)
			{
				this.HeadstoneMeshes[k].material = this.HeadstoneMaterials[num4];
			}
		}

		// Token: 0x04003F49 RID: 16201
		[Header("References")]
		public Grave.GraveSuface[] Surfaces;

		// Token: 0x04003F4A RID: 16202
		public GameObject[] HeadstoneObjects;

		// Token: 0x04003F4B RID: 16203
		public MeshRenderer[] HeadstoneMeshes;

		// Token: 0x04003F4C RID: 16204
		public Material[] HeadstoneMaterials;

		// Token: 0x02000BE2 RID: 3042
		[Serializable]
		public class GraveSuface
		{
			// Token: 0x04003F4D RID: 16205
			public GameObject Object;

			// Token: 0x04003F4E RID: 16206
			public MeshRenderer Mesh;

			// Token: 0x04003F4F RID: 16207
			public Material[] Materials;
		}
	}
}
