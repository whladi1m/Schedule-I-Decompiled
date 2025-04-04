using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000061 RID: 97
[ExecuteAlways]
public class VolumetricFire : MonoBehaviour
{
	// Token: 0x06000229 RID: 553 RVA: 0x0000CD04 File Offset: 0x0000AF04
	private void Start()
	{
		this.materialPropertyBlock = new MaterialPropertyBlock();
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		component.enabled = false;
		this.material = component.sharedMaterial;
		this.mesh = base.GetComponent<MeshFilter>().sharedMesh;
		this.boundaryCollider = base.GetComponent<Collider>();
		this.randomStatic = UnityEngine.Random.Range(0f, 1f);
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000CD68 File Offset: 0x0000AF68
	private void OnEnable()
	{
		RenderPipelineManager.beginCameraRendering += this.RenderFlames;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000CD7B File Offset: 0x0000AF7B
	private void OnDisable()
	{
		RenderPipelineManager.beginCameraRendering -= this.RenderFlames;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000CD8E File Offset: 0x0000AF8E
	private static bool IsVisible(Camera camera, Bounds bounds)
	{
		return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), bounds);
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000CD9C File Offset: 0x0000AF9C
	private void RenderFlames(ScriptableRenderContext context, Camera camera)
	{
		VolumetricFire.IsVisible(camera, this.boundaryCollider.bounds);
		this.internalCount = (this.thickness - 1) * 2;
		float spacing = 0f;
		if (this.internalCount > 0)
		{
			spacing = this.spread / (float)this.internalCount;
		}
		for (int i = 0; i <= this.internalCount; i++)
		{
			float item = (float)i - (float)this.internalCount * 0.5f;
			this.SetupMaterialPropertyBlock(item);
			this.CreateItem(spacing, item, camera);
		}
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000CE1C File Offset: 0x0000B01C
	private void SetupMaterialPropertyBlock(float item)
	{
		if (this.materialPropertyBlock == null)
		{
			return;
		}
		this.materialPropertyBlock.SetFloat("_ITEMNUMBER", item);
		this.materialPropertyBlock.SetFloat("_INTERNALCOUNT", (float)this.internalCount);
		this.materialPropertyBlock.SetFloat("_INITIALPOSITIONINT", this.randomStatic);
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000CE70 File Offset: 0x0000B070
	private void CreateItem(float spacing, float item, Camera camera)
	{
		Quaternion quaternion = Quaternion.identity;
		Vector3 pos = Vector3.zero;
		if (this.billboard)
		{
			quaternion *= camera.transform.rotation;
			Vector3 normalized = (base.transform.position - camera.transform.position).normalized;
			pos = base.transform.position - normalized * item * spacing;
		}
		else
		{
			quaternion = base.transform.rotation;
			pos = base.transform.position - base.transform.forward * item * spacing;
		}
		Matrix4x4 matrix = Matrix4x4.TRS(pos, quaternion, base.transform.localScale);
		Graphics.DrawMesh(this.mesh, matrix, this.material, 0, camera, 0, this.materialPropertyBlock, false, false, false);
	}

	// Token: 0x04000254 RID: 596
	private Mesh mesh;

	// Token: 0x04000255 RID: 597
	private Material material;

	// Token: 0x04000256 RID: 598
	[SerializeField]
	[Range(1f, 20f)]
	[Tooltip("Controls the number of additional meshes to render in front of and behind the original mesh")]
	private int thickness = 1;

	// Token: 0x04000257 RID: 599
	[SerializeField]
	[Range(0.01f, 1f)]
	[Tooltip("Controls the total distance between the frontmost mesh and the backmost mesh")]
	private float spread = 0.2f;

	// Token: 0x04000258 RID: 600
	[SerializeField]
	private bool billboard = true;

	// Token: 0x04000259 RID: 601
	private MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x0400025A RID: 602
	private int internalCount;

	// Token: 0x0400025B RID: 603
	private float randomStatic;

	// Token: 0x0400025C RID: 604
	private Collider boundaryCollider;
}
