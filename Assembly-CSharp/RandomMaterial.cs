using System;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class RandomMaterial : MonoBehaviour
{
	// Token: 0x06000153 RID: 339 RVA: 0x00007893 File Offset: 0x00005A93
	public void Start()
	{
		this.ChangeMaterial();
	}

	// Token: 0x06000154 RID: 340 RVA: 0x0000789B File Offset: 0x00005A9B
	public void ChangeMaterial()
	{
		this.targetRenderer.sharedMaterial = this.materials[UnityEngine.Random.Range(0, this.materials.Length)];
	}

	// Token: 0x04000129 RID: 297
	public Renderer targetRenderer;

	// Token: 0x0400012A RID: 298
	public Material[] materials;
}
