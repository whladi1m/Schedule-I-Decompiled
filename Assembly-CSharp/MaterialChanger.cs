using System;
using UnityEngine;

// Token: 0x0200005F RID: 95
[ExecuteAlways]
public class MaterialChanger : MonoBehaviour
{
	// Token: 0x06000221 RID: 545 RVA: 0x0000CC26 File Offset: 0x0000AE26
	private void OnEnable()
	{
		this.FindAllMaterialInChild();
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000CC2E File Offset: 0x0000AE2E
	private void Update()
	{
		this._propBlock = new MaterialPropertyBlock();
		this.SetNewValueForAllMaterial(this._value);
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000CC47 File Offset: 0x0000AE47
	private void FindAllMaterialInChild()
	{
		this._renderers = base.transform.GetComponentsInChildren<Renderer>();
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000CC5C File Offset: 0x0000AE5C
	private void SetNewValueForAllMaterial(float value)
	{
		this.FindAllMaterialInChild();
		for (int i = 0; i < this._renderers.Length; i++)
		{
			this._renderers[i].GetPropertyBlock(this._propBlock);
			this._propBlock.SetFloat(this._changeMaterialSetting, value);
			this._renderers[i].SetPropertyBlock(this._propBlock);
		}
	}

	// Token: 0x0400024D RID: 589
	[SerializeField]
	[Range(0f, 5f)]
	private float _value = 1f;

	// Token: 0x0400024E RID: 590
	[SerializeField]
	private string _changeMaterialSetting = "_Worn_Level";

	// Token: 0x0400024F RID: 591
	private Renderer[] _renderers;

	// Token: 0x04000250 RID: 592
	private MaterialPropertyBlock _propBlock;
}
