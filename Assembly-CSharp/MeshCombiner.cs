using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000034 RID: 52
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x0600010B RID: 267 RVA: 0x00006604 File Offset: 0x00004804
	// (set) Token: 0x0600010C RID: 268 RVA: 0x0000660C File Offset: 0x0000480C
	public bool CreateMultiMaterialMesh
	{
		get
		{
			return this.createMultiMaterialMesh;
		}
		set
		{
			this.createMultiMaterialMesh = value;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600010D RID: 269 RVA: 0x00006615 File Offset: 0x00004815
	// (set) Token: 0x0600010E RID: 270 RVA: 0x0000661D File Offset: 0x0000481D
	public bool CombineInactiveChildren
	{
		get
		{
			return this.combineInactiveChildren;
		}
		set
		{
			this.combineInactiveChildren = value;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x0600010F RID: 271 RVA: 0x00006626 File Offset: 0x00004826
	// (set) Token: 0x06000110 RID: 272 RVA: 0x0000662E File Offset: 0x0000482E
	public bool DeactivateCombinedChildren
	{
		get
		{
			return this.deactivateCombinedChildren;
		}
		set
		{
			this.deactivateCombinedChildren = value;
			this.CheckDeactivateCombinedChildren();
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000111 RID: 273 RVA: 0x0000663D File Offset: 0x0000483D
	// (set) Token: 0x06000112 RID: 274 RVA: 0x00006645 File Offset: 0x00004845
	public bool DeactivateCombinedChildrenMeshRenderers
	{
		get
		{
			return this.deactivateCombinedChildrenMeshRenderers;
		}
		set
		{
			this.deactivateCombinedChildrenMeshRenderers = value;
			this.CheckDeactivateCombinedChildren();
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000113 RID: 275 RVA: 0x00006654 File Offset: 0x00004854
	// (set) Token: 0x06000114 RID: 276 RVA: 0x0000665C File Offset: 0x0000485C
	public bool GenerateUVMap
	{
		get
		{
			return this.generateUVMap;
		}
		set
		{
			this.generateUVMap = value;
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000115 RID: 277 RVA: 0x00006665 File Offset: 0x00004865
	// (set) Token: 0x06000116 RID: 278 RVA: 0x0000666D File Offset: 0x0000486D
	public bool DestroyCombinedChildren
	{
		get
		{
			return this.destroyCombinedChildren;
		}
		set
		{
			this.destroyCombinedChildren = value;
			this.CheckDestroyCombinedChildren();
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000117 RID: 279 RVA: 0x0000667C File Offset: 0x0000487C
	// (set) Token: 0x06000118 RID: 280 RVA: 0x00006684 File Offset: 0x00004884
	public string FolderPath
	{
		get
		{
			return this.folderPath;
		}
		set
		{
			this.folderPath = value;
		}
	}

	// Token: 0x06000119 RID: 281 RVA: 0x0000668D File Offset: 0x0000488D
	private void CheckDeactivateCombinedChildren()
	{
		if (this.deactivateCombinedChildren || this.deactivateCombinedChildrenMeshRenderers)
		{
			this.destroyCombinedChildren = false;
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x000066A6 File Offset: 0x000048A6
	private void CheckDestroyCombinedChildren()
	{
		if (this.destroyCombinedChildren)
		{
			this.deactivateCombinedChildren = false;
			this.deactivateCombinedChildrenMeshRenderers = false;
		}
	}

	// Token: 0x0600011B RID: 283 RVA: 0x000066C0 File Offset: 0x000048C0
	public void CombineMeshes(bool showCreatedMeshInfo)
	{
		Vector3 localScale = base.transform.localScale;
		int siblingIndex = base.transform.GetSiblingIndex();
		Transform parent = base.transform.parent;
		base.transform.parent = null;
		Quaternion rotation = base.transform.rotation;
		Vector3 position = base.transform.position;
		Vector3 localScale2 = base.transform.localScale;
		base.transform.rotation = Quaternion.identity;
		base.transform.position = Vector3.zero;
		base.transform.localScale = Vector3.one;
		if (!this.createMultiMaterialMesh)
		{
			this.CombineMeshesWithSingleMaterial(showCreatedMeshInfo);
		}
		else
		{
			this.CombineMeshesWithMutliMaterial(showCreatedMeshInfo);
		}
		base.transform.rotation = rotation;
		base.transform.position = position;
		base.transform.localScale = localScale2;
		base.transform.parent = parent;
		base.transform.SetSiblingIndex(siblingIndex);
		base.transform.localScale = localScale;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x000067B8 File Offset: 0x000049B8
	private MeshFilter[] GetMeshFiltersToCombine()
	{
		MeshCombiner.<>c__DisplayClass33_0 CS$<>8__locals1 = new MeshCombiner.<>c__DisplayClass33_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.meshFilters = base.GetComponentsInChildren<MeshFilter>(this.combineInactiveChildren);
		this.meshFiltersToSkip = (from meshFilter in this.meshFiltersToSkip
		where meshFilter != CS$<>8__locals1.meshFilters[0]
		select meshFilter).ToArray<MeshFilter>();
		this.meshFiltersToSkip = (from meshFilter in this.meshFiltersToSkip
		where meshFilter != null
		select meshFilter).ToArray<MeshFilter>();
		int i;
		int j;
		for (i = 0; i < this.meshFiltersToSkip.Length; i = j + 1)
		{
			CS$<>8__locals1.meshFilters = (from meshFilter in CS$<>8__locals1.meshFilters
			where meshFilter != CS$<>8__locals1.<>4__this.meshFiltersToSkip[i]
			select meshFilter).ToArray<MeshFilter>();
			j = i;
		}
		return CS$<>8__locals1.meshFilters;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x000068A4 File Offset: 0x00004AA4
	private void CombineMeshesWithSingleMaterial(bool showCreatedMeshInfo)
	{
		MeshFilter[] meshFiltersToCombine = this.GetMeshFiltersToCombine();
		CombineInstance[] array = new CombineInstance[meshFiltersToCombine.Length - 1];
		long num = 0L;
		for (int i = 0; i < meshFiltersToCombine.Length - 1; i++)
		{
			array[i].subMeshIndex = 0;
			array[i].mesh = meshFiltersToCombine[i + 1].sharedMesh;
			array[i].transform = meshFiltersToCombine[i + 1].transform.localToWorldMatrix;
			num += (long)array[i].mesh.vertices.Length;
		}
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>(this.combineInactiveChildren);
		if (componentsInChildren.Length >= 2)
		{
			componentsInChildren[0].sharedMaterials = new Material[1];
			componentsInChildren[0].sharedMaterial = componentsInChildren[1].sharedMaterial;
		}
		else
		{
			componentsInChildren[0].sharedMaterials = new Material[0];
		}
		Mesh mesh = new Mesh();
		mesh.name = base.name;
		if (num > 65535L)
		{
			mesh.indexFormat = IndexFormat.UInt32;
		}
		mesh.CombineMeshes(array);
		this.GenerateUV(mesh);
		meshFiltersToCombine[0].sharedMesh = mesh;
		this.DeactivateCombinedGameObjects(meshFiltersToCombine);
		if (showCreatedMeshInfo)
		{
			if (num <= 65535L)
			{
				Debug.Log(string.Concat(new string[]
				{
					"<color=#00cc00><b>Mesh \"",
					base.name,
					"\" was created from ",
					array.Length.ToString(),
					" children meshes and has ",
					num.ToString(),
					" vertices.</b></color>"
				}));
				return;
			}
			Debug.Log(string.Concat(new string[]
			{
				"<color=#ff3300><b>Mesh \"",
				base.name,
				"\" was created from ",
				array.Length.ToString(),
				" children meshes and has ",
				num.ToString(),
				" vertices. Some old devices, like Android with Mali-400 GPU, do not support over 65535 vertices.</b></color>"
			}));
		}
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00006A70 File Offset: 0x00004C70
	private void CombineMeshesWithMutliMaterial(bool showCreatedMeshInfo)
	{
		MeshFilter[] meshFiltersToCombine = this.GetMeshFiltersToCombine();
		MeshRenderer[] array = new MeshRenderer[meshFiltersToCombine.Length];
		array[0] = base.GetComponent<MeshRenderer>();
		List<Material> list = new List<Material>();
		for (int i = 0; i < meshFiltersToCombine.Length - 1; i++)
		{
			array[i + 1] = meshFiltersToCombine[i + 1].GetComponent<MeshRenderer>();
			if (array[i + 1] != null)
			{
				Material[] sharedMaterials = array[i + 1].sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					if (!list.Contains(sharedMaterials[j]))
					{
						list.Add(sharedMaterials[j]);
					}
				}
			}
		}
		List<CombineInstance> list2 = new List<CombineInstance>();
		long num = 0L;
		for (int k = 0; k < list.Count; k++)
		{
			List<CombineInstance> list3 = new List<CombineInstance>();
			for (int l = 0; l < meshFiltersToCombine.Length - 1; l++)
			{
				if (array[l + 1] != null)
				{
					Material[] sharedMaterials2 = array[l + 1].sharedMaterials;
					for (int m = 0; m < sharedMaterials2.Length; m++)
					{
						if (list[k] == sharedMaterials2[m])
						{
							CombineInstance item = new CombineInstance
							{
								subMeshIndex = m,
								mesh = meshFiltersToCombine[l + 1].sharedMesh,
								transform = meshFiltersToCombine[l + 1].transform.localToWorldMatrix
							};
							list3.Add(item);
							num += (long)item.mesh.vertices.Length;
						}
					}
				}
			}
			Mesh mesh = new Mesh();
			if (num > 65535L)
			{
				mesh.indexFormat = IndexFormat.UInt32;
			}
			mesh.CombineMeshes(list3.ToArray(), true);
			list2.Add(new CombineInstance
			{
				subMeshIndex = 0,
				mesh = mesh,
				transform = Matrix4x4.identity
			});
		}
		array[0].sharedMaterials = list.ToArray();
		Mesh mesh2 = new Mesh();
		mesh2.name = base.name;
		if (num > 65535L)
		{
			mesh2.indexFormat = IndexFormat.UInt32;
		}
		mesh2.CombineMeshes(list2.ToArray(), false);
		this.GenerateUV(mesh2);
		meshFiltersToCombine[0].sharedMesh = mesh2;
		this.DeactivateCombinedGameObjects(meshFiltersToCombine);
		if (showCreatedMeshInfo)
		{
			if (num <= 65535L)
			{
				Debug.Log(string.Concat(new string[]
				{
					"<color=#00cc00><b>Mesh \"",
					base.name,
					"\" was created from ",
					(meshFiltersToCombine.Length - 1).ToString(),
					" children meshes and has ",
					list2.Count.ToString(),
					" submeshes, and ",
					num.ToString(),
					" vertices.</b></color>"
				}));
				return;
			}
			Debug.Log(string.Concat(new string[]
			{
				"<color=#ff3300><b>Mesh \"",
				base.name,
				"\" was created from ",
				(meshFiltersToCombine.Length - 1).ToString(),
				" children meshes and has ",
				list2.Count.ToString(),
				" submeshes, and ",
				num.ToString(),
				" vertices. Some old devices, like Android with Mali-400 GPU, do not support over 65535 vertices.</b></color>"
			}));
		}
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00006D88 File Offset: 0x00004F88
	private void DeactivateCombinedGameObjects(MeshFilter[] meshFilters)
	{
		for (int i = 0; i < meshFilters.Length - 1; i++)
		{
			if (!this.destroyCombinedChildren)
			{
				if (this.deactivateCombinedChildren)
				{
					meshFilters[i + 1].gameObject.SetActive(false);
				}
				if (this.deactivateCombinedChildrenMeshRenderers)
				{
					MeshRenderer component = meshFilters[i + 1].gameObject.GetComponent<MeshRenderer>();
					if (component != null)
					{
						component.enabled = false;
					}
				}
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(meshFilters[i + 1].gameObject);
			}
		}
	}

	// Token: 0x06000120 RID: 288 RVA: 0x000045B1 File Offset: 0x000027B1
	private void GenerateUV(Mesh combinedMesh)
	{
	}

	// Token: 0x040000EE RID: 238
	private const int Mesh16BitBufferVertexLimit = 65535;

	// Token: 0x040000EF RID: 239
	[SerializeField]
	private bool createMultiMaterialMesh = true;

	// Token: 0x040000F0 RID: 240
	[SerializeField]
	private bool combineInactiveChildren;

	// Token: 0x040000F1 RID: 241
	[SerializeField]
	private bool deactivateCombinedChildren;

	// Token: 0x040000F2 RID: 242
	[SerializeField]
	private bool deactivateCombinedChildrenMeshRenderers;

	// Token: 0x040000F3 RID: 243
	[SerializeField]
	private bool generateUVMap;

	// Token: 0x040000F4 RID: 244
	[SerializeField]
	private bool destroyCombinedChildren = true;

	// Token: 0x040000F5 RID: 245
	[SerializeField]
	private string folderPath = "Prefabs/CombinedMeshes";

	// Token: 0x040000F6 RID: 246
	[SerializeField]
	[Tooltip("MeshFilters with Meshes which we don't want to combine into one Mesh.")]
	private MeshFilter[] meshFiltersToSkip = new MeshFilter[0];
}
