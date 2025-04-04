using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200003E RID: 62
public static class SmartCombineUtilities
{
	// Token: 0x06000144 RID: 324 RVA: 0x00007340 File Offset: 0x00005540
	public static void CombineMeshesSmart(this Mesh mesh, SmartMeshData[] meshData, out Material[] materials)
	{
		IDictionary<Material, SmartCombineUtilities.SmartSubmeshData> dictionary = new Dictionary<Material, SmartCombineUtilities.SmartSubmeshData>();
		IList<CombineInstance> list = new List<CombineInstance>();
		foreach (SmartMeshData smartMeshData in meshData)
		{
			IList<Material> materials2 = smartMeshData.materials;
			for (int j = 0; j < smartMeshData.mesh.subMeshCount; j++)
			{
				SmartCombineUtilities.SmartSubmeshData smartSubmeshData;
				if (dictionary.ContainsKey(materials2[j]))
				{
					smartSubmeshData = dictionary[materials2[j]];
				}
				else
				{
					smartSubmeshData = new SmartCombineUtilities.SmartSubmeshData();
					dictionary.Add(materials2[j], smartSubmeshData);
				}
				CombineInstance item = default(CombineInstance);
				item.mesh = smartMeshData.mesh;
				item.subMeshIndex = j;
				item.transform = smartMeshData.transform;
				smartSubmeshData.combineInstances.Add(item);
			}
		}
		foreach (SmartCombineUtilities.SmartSubmeshData smartSubmeshData2 in dictionary.Values)
		{
			smartSubmeshData2.CombineSubmeshes();
			list.Add(new CombineInstance
			{
				mesh = smartSubmeshData2.mesh,
				subMeshIndex = 0
			});
		}
		mesh.Clear();
		mesh.CombineMeshes(list.ToArray<CombineInstance>(), false, false);
		mesh.Optimize();
		materials = dictionary.Keys.ToArray<Material>();
	}

	// Token: 0x0200003F RID: 63
	private class SmartSubmeshData
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000074AC File Offset: 0x000056AC
		// (set) Token: 0x06000146 RID: 326 RVA: 0x000074B4 File Offset: 0x000056B4
		public Mesh mesh { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000074BD File Offset: 0x000056BD
		// (set) Token: 0x06000148 RID: 328 RVA: 0x000074C5 File Offset: 0x000056C5
		public IList<CombineInstance> combineInstances { get; private set; }

		// Token: 0x06000149 RID: 329 RVA: 0x000074CE File Offset: 0x000056CE
		public SmartSubmeshData()
		{
			this.combineInstances = new List<CombineInstance>();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000074E1 File Offset: 0x000056E1
		public void CombineSubmeshes()
		{
			if (this.mesh == null)
			{
				this.mesh = new Mesh();
			}
			else
			{
				this.mesh.Clear();
			}
			this.mesh.CombineMeshes(this.combineInstances.ToArray<CombineInstance>(), true, true);
		}
	}
}
