using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
[ExecuteInEditMode]
public class RenderPipelineLighting : MonoBehaviour
{
	// Token: 0x06000235 RID: 565 RVA: 0x0000D05C File Offset: 0x0000B25C
	private void OnValidate()
	{
		this.Awake();
	}

	// Token: 0x06000236 RID: 566 RVA: 0x000045B1 File Offset: 0x000027B1
	private void Awake()
	{
	}

	// Token: 0x04000261 RID: 609
	[SerializeField]
	private GameObject _standardLighting;

	// Token: 0x04000262 RID: 610
	[SerializeField]
	private Material _standardSky;

	// Token: 0x04000263 RID: 611
	[SerializeField]
	private Material _standardTerrain;

	// Token: 0x04000264 RID: 612
	[SerializeField]
	private GameObject _universalLighting;

	// Token: 0x04000265 RID: 613
	[SerializeField]
	private Material _universalSky;

	// Token: 0x04000266 RID: 614
	[SerializeField]
	private Material _universalTerrain;

	// Token: 0x04000267 RID: 615
	[SerializeField]
	private GameObject _highDefinitionLighting;

	// Token: 0x04000268 RID: 616
	[SerializeField]
	private Material _highDefinitionSky;

	// Token: 0x04000269 RID: 617
	[SerializeField]
	private Material _highDefinitionTerrain;
}
