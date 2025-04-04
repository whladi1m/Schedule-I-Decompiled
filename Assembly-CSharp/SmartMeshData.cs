using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class SmartMeshData
{
	// Token: 0x17000019 RID: 25
	// (get) Token: 0x0600013A RID: 314 RVA: 0x0000722A File Offset: 0x0000542A
	// (set) Token: 0x0600013B RID: 315 RVA: 0x00007232 File Offset: 0x00005432
	public Mesh mesh { get; private set; }

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x0600013C RID: 316 RVA: 0x0000723B File Offset: 0x0000543B
	// (set) Token: 0x0600013D RID: 317 RVA: 0x00007243 File Offset: 0x00005443
	public Matrix4x4 transform { get; private set; }

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x0600013E RID: 318 RVA: 0x0000724C File Offset: 0x0000544C
	public IList<Material> materials
	{
		get
		{
			return new ReadOnlyCollection<Material>(this._materials);
		}
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0000725C File Offset: 0x0000545C
	public SmartMeshData(Mesh inMesh, Material[] inMaterials, Matrix4x4 inTransform)
	{
		this.mesh = inMesh;
		this._materials = inMaterials;
		this.transform = inTransform;
		if (this._materials.Length != this.mesh.subMeshCount)
		{
			Debug.LogWarning("SmartMeshData has incorrect number of materials. Resizing to match submesh count");
			Material[] array = new Material[this.mesh.subMeshCount];
			for (int i = 0; i < this._materials.Length; i++)
			{
				if (i < this._materials.Length)
				{
					array[i] = this._materials[i];
				}
				else
				{
					array[i] = null;
				}
			}
			this._materials = array;
		}
	}

	// Token: 0x06000140 RID: 320 RVA: 0x000072EA File Offset: 0x000054EA
	public SmartMeshData(Mesh inputMesh, Material[] inputMaterials) : this(inputMesh, inputMaterials, Matrix4x4.identity)
	{
	}

	// Token: 0x06000141 RID: 321 RVA: 0x000072F9 File Offset: 0x000054F9
	public SmartMeshData(Mesh inputMesh, Material[] inputMaterials, Vector3 position) : this(inputMesh, inputMaterials, Matrix4x4.TRS(position, Quaternion.identity, Vector3.one))
	{
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00007313 File Offset: 0x00005513
	public SmartMeshData(Mesh inputMesh, Material[] inputMaterials, Vector3 position, Quaternion rotation) : this(inputMesh, inputMaterials, Matrix4x4.TRS(position, rotation, Vector3.one))
	{
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000732A File Offset: 0x0000552A
	public SmartMeshData(Mesh inputMesh, Material[] inputMaterials, Vector3 position, Quaternion rotation, Vector3 scale) : this(inputMesh, inputMaterials, Matrix4x4.TRS(position, rotation, scale))
	{
	}

	// Token: 0x04000112 RID: 274
	private Material[] _materials;
}
