using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x020001FF RID: 511
	public class CharacterCustomizationCombiner
	{
		// Token: 0x06000B56 RID: 2902 RVA: 0x00033F98 File Offset: 0x00032198
		public static List<SkinnedMeshRenderer> MakeCombinedMeshes(CharacterCustomization character, GameObject exportInCustomObject = null, float blendshapeAddDelay = 0.001f, Action<List<SkinnedMeshRenderer>> callback = null)
		{
			CharacterCustomizationCombiner.returnSkinnedMeshes.Clear();
			if (character.IsBaked())
			{
				Debug.LogError("Character is already combined!");
				return null;
			}
			if (callback != null)
			{
				CharacterCustomizationCombiner._callback = callback;
			}
			CharacterCustomizationCombiner.BlendshapeTransferWork = false;
			CharacterCustomizationCombiner.useExportToAnotherObject = (exportInCustomObject != null);
			if (!CharacterCustomizationCombiner.useExportToAnotherObject)
			{
				character.CurrentCombinerState = CombinerState.InProgressCombineMesh;
			}
			CharacterCustomizationCombiner.currentCharacter = character;
			CharacterCustomizationCombiner.bindPoses = character.GetCharacterPart("Head").skinnedMesh[0].sharedMesh.bindposes;
			CharacterCustomizationCombiner.LODMeshInstances = new List<CharacterCustomizationCombiner.MeshInstance>();
			for (int i = 0; i < character.MaxLODLevels - character.MinLODLevels + 1; i++)
			{
				CharacterCustomizationCombiner.LODMeshInstances.Add(new CharacterCustomizationCombiner.MeshInstance());
				foreach (SkinnedMeshRenderer mesh in character.GetAllMeshesByLod(i))
				{
					CharacterCustomizationCombiner.<MakeCombinedMeshes>g__SelectMeshes|10_0(mesh, i);
				}
			}
			SkinnedMeshRenderer original = character.GetCharacterPart("Combined").skinnedMesh[0];
			List<SkinnedMeshRenderer> list = character.GetCharacterPart("Combined").skinnedMesh;
			if (exportInCustomObject != null)
			{
				List<SkinnedMeshRenderer> list2 = new List<SkinnedMeshRenderer>();
				for (int j = 0; j < character.MaxLODLevels - character.MinLODLevels + 1; j++)
				{
					SkinnedMeshRenderer item = UnityEngine.Object.Instantiate<SkinnedMeshRenderer>(original, exportInCustomObject.transform);
					list2.Add(item);
				}
				list = list2;
			}
			for (int k = 0; k < CharacterCustomizationCombiner.LODMeshInstances.Count; k++)
			{
				CharacterCustomizationCombiner.MeshInstance meshInstance = CharacterCustomizationCombiner.LODMeshInstances[k];
				for (int l = 0; l < meshInstance.unique_materials.Count; l++)
				{
					Material key = meshInstance.unique_materials[l];
					List<CharacterCustomizationCombiner.CombineInstanceWithSM> list3 = meshInstance.combine_instances[meshInstance.unique_materials[l]];
					for (int m = 0; m < list3.Count; m++)
					{
						CharacterCustomizationCombiner.CombineInstanceWithSM combineInstanceWithSM = list3[m];
						if (!meshInstance.vertex_offset_map.ContainsKey(combineInstanceWithSM.instance.mesh))
						{
							meshInstance.combined_vertices.AddRange(combineInstanceWithSM.instance.mesh.vertices);
							if (combineInstanceWithSM.instance.mesh.uv.Length == 0)
							{
								meshInstance.combined_uv.AddRange(new Vector2[combineInstanceWithSM.instance.mesh.vertexCount]);
							}
							else
							{
								meshInstance.combined_uv.AddRange(combineInstanceWithSM.instance.mesh.uv);
							}
							if (combineInstanceWithSM.instance.mesh.uv2.Length == 0)
							{
								meshInstance.combined_uv2.AddRange(new Vector2[combineInstanceWithSM.instance.mesh.vertexCount]);
							}
							else
							{
								meshInstance.combined_uv2.AddRange(combineInstanceWithSM.instance.mesh.uv2);
							}
							if (combineInstanceWithSM.instance.mesh.uv3.Length == 0)
							{
								meshInstance.combined_uv3.AddRange(new Vector2[combineInstanceWithSM.instance.mesh.vertexCount]);
							}
							else
							{
								meshInstance.combined_uv3.AddRange(combineInstanceWithSM.instance.mesh.uv3);
							}
							meshInstance.normals.AddRange(combineInstanceWithSM.instance.mesh.normals);
							meshInstance.combined_bone_weights.AddRange(combineInstanceWithSM.instance.mesh.boneWeights);
							meshInstance.vertex_offset_map[combineInstanceWithSM.instance.mesh] = meshInstance.vertex_index_offset;
							meshInstance.vertex_index_offset += combineInstanceWithSM.instance.mesh.vertexCount;
						}
						int num = meshInstance.vertex_offset_map[combineInstanceWithSM.instance.mesh];
						int[] triangles = combineInstanceWithSM.instance.mesh.GetTriangles(combineInstanceWithSM.instance.subMeshIndex);
						for (int n = 0; n < triangles.Length; n++)
						{
							triangles[n] += num;
						}
						if (!meshInstance.combined_submesh_indices.ContainsKey(key))
						{
							meshInstance.combined_submesh_indices.Add(key, triangles.ToList<int>());
						}
						else
						{
							meshInstance.combined_submesh_indices[key].AddRange(triangles);
						}
						for (int num2 = 0; num2 < combineInstanceWithSM.instance.mesh.blendShapeCount; num2++)
						{
							string blendShapeName = combineInstanceWithSM.instance.mesh.GetBlendShapeName(num2);
							if (!meshInstance.blendShapeNames.Contains(blendShapeName))
							{
								meshInstance.blendShapeNames.Add(blendShapeName);
								meshInstance.blendShapeValues.Add(combineInstanceWithSM.skinnedMesh.GetBlendShapeWeight(num2));
							}
						}
					}
				}
				meshInstance.combined_new_mesh.vertices = meshInstance.combined_vertices.ToArray();
				meshInstance.combined_new_mesh.uv = meshInstance.combined_uv.ToArray();
				if (meshInstance.combined_uv2.Count > 0)
				{
					meshInstance.combined_new_mesh.uv2 = meshInstance.combined_uv2.ToArray();
				}
				if (meshInstance.combined_uv3.Count > 0)
				{
					meshInstance.combined_new_mesh.uv3 = meshInstance.combined_uv3.ToArray();
				}
				if (meshInstance.combined_uv4.Count > 0)
				{
					meshInstance.combined_new_mesh.uv4 = meshInstance.combined_uv4.ToArray();
				}
				meshInstance.combined_new_mesh.boneWeights = meshInstance.combined_bone_weights.ToArray();
				meshInstance.combined_new_mesh.name = string.Format("APP_CombinedMesh_lod{0}", k);
				meshInstance.combined_new_mesh.subMeshCount = meshInstance.unique_materials.Count;
				for (int num3 = 0; num3 < meshInstance.unique_materials.Count; num3++)
				{
					meshInstance.combined_new_mesh.SetTriangles(meshInstance.combined_submesh_indices[meshInstance.unique_materials[num3]], num3);
				}
				meshInstance.combined_new_mesh.SetNormals(meshInstance.normals);
				meshInstance.combined_new_mesh.RecalculateTangents();
				if (!CharacterCustomizationCombiner.useExportToAnotherObject && character.CurrentCombinerState != CombinerState.InProgressBlendshapeTransfer)
				{
					character.CurrentCombinerState = CombinerState.InProgressBlendshapeTransfer;
				}
				character.StartCoroutine(CharacterCustomizationCombiner.BlendshapeTransfer(meshInstance, blendshapeAddDelay, list[k], k, exportInCustomObject == null));
			}
			for (int num4 = 0; num4 < list.Count; num4++)
			{
				list[num4].name = string.Format("combinemesh_lod{0}", num4);
				list[num4].sharedMesh = CharacterCustomizationCombiner.LODMeshInstances[num4].combined_new_mesh;
				list[num4].sharedMesh.bindposes = CharacterCustomizationCombiner.bindPoses;
				list[num4].sharedMaterials = CharacterCustomizationCombiner.LODMeshInstances[num4].unique_materials.ToArray();
				list[num4].updateWhenOffscreen = true;
			}
			CharacterCustomizationCombiner.returnSkinnedMeshes.AddRange(list);
			CharacterCustomizationCombiner.BlendshapeTransferWork = true;
			return CharacterCustomizationCombiner.returnSkinnedMeshes;
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00034698 File Offset: 0x00032898
		private static IEnumerator BlendshapeTransfer(CharacterCustomizationCombiner.MeshInstance meshInstance, float waitTime, SkinnedMeshRenderer smr, int lod, bool yieldUse = true)
		{
			yield return new WaitWhile(() => !CharacterCustomizationCombiner.BlendshapeTransferWork);
			CharacterCustomization characterSystem = CharacterCustomizationCombiner.currentCharacter;
			int num2;
			for (int bs = 0; bs < meshInstance.blendShapeNames.Count; bs = num2 + 1)
			{
				int num = 0;
				CharacterCustomizationCombiner.BlendWeightData blendWeightData = default(CharacterCustomizationCombiner.BlendWeightData);
				blendWeightData.deltaNormals = new Vector3[meshInstance.combined_new_mesh.vertexCount];
				blendWeightData.deltaTangents = new Vector3[meshInstance.combined_new_mesh.vertexCount];
				blendWeightData.deltaVerts = new Vector3[meshInstance.combined_new_mesh.vertexCount];
				foreach (KeyValuePair<Material, List<CharacterCustomizationCombiner.CombineInstanceWithSM>> keyValuePair in meshInstance.combine_instances)
				{
					foreach (CharacterCustomizationCombiner.CombineInstanceWithSM combineInstanceWithSM in keyValuePair.Value)
					{
						CombineInstance instance = combineInstanceWithSM.instance;
						if (instance.subMeshIndex <= 0)
						{
							instance = combineInstanceWithSM.instance;
							int vertexCount = instance.mesh.vertexCount;
							Vector3[] array = new Vector3[vertexCount];
							Vector3[] array2 = new Vector3[vertexCount];
							Vector3[] array3 = new Vector3[vertexCount];
							instance = combineInstanceWithSM.instance;
							if (instance.mesh.GetBlendShapeIndex(meshInstance.blendShapeNames[bs]) != -1)
							{
								instance = combineInstanceWithSM.instance;
								int blendShapeIndex = instance.mesh.GetBlendShapeIndex(meshInstance.blendShapeNames[bs]);
								instance = combineInstanceWithSM.instance;
								Mesh mesh = instance.mesh;
								int shapeIndex = blendShapeIndex;
								instance = combineInstanceWithSM.instance;
								mesh.GetBlendShapeFrameVertices(shapeIndex, instance.mesh.GetBlendShapeFrameCount(blendShapeIndex) - 1, array, array2, array3);
								Array.Copy(array, 0, blendWeightData.deltaVerts, num, vertexCount);
								Array.Copy(array2, 0, blendWeightData.deltaNormals, num, vertexCount);
								Array.Copy(array3, 0, blendWeightData.deltaTangents, num, vertexCount);
							}
							num += vertexCount;
						}
					}
				}
				smr.sharedMesh.AddBlendShapeFrame(meshInstance.blendShapeNames[bs], 100f, blendWeightData.deltaVerts, blendWeightData.deltaNormals, blendWeightData.deltaTangents);
				smr.SetBlendShapeWeight(bs, meshInstance.blendShapeValues[bs]);
				if (waitTime > 0f && yieldUse)
				{
					yield return new WaitForSecondsRealtime(waitTime);
				}
				num2 = bs;
			}
			if (lod == characterSystem.MaxLODLevels - characterSystem.MinLODLevels)
			{
				if (!CharacterCustomizationCombiner.useExportToAnotherObject)
				{
					characterSystem.CurrentCombinerState = CombinerState.Combined;
				}
				Action<List<SkinnedMeshRenderer>> callback = CharacterCustomizationCombiner._callback;
				if (callback != null)
				{
					callback(CharacterCustomizationCombiner.returnSkinnedMeshes);
				}
				CharacterCustomizationCombiner._callback = null;
			}
			yield break;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x000346DC File Offset: 0x000328DC
		[CompilerGenerated]
		internal static void <MakeCombinedMeshes>g__SelectMeshes|10_0(SkinnedMeshRenderer mesh, int LOD)
		{
			if (mesh != null)
			{
				for (int i = 0; i < mesh.sharedMaterials.Length; i++)
				{
					Material material = mesh.sharedMaterials[i];
					Mesh sharedMesh = mesh.sharedMesh;
					if (!(sharedMesh == null) && mesh.gameObject.activeSelf && mesh.enabled && sharedMesh.vertexCount != 0 && sharedMesh.subMeshCount - 1 >= i)
					{
						if (!CharacterCustomizationCombiner.LODMeshInstances[LOD].combine_instances.ContainsKey(material))
						{
							CharacterCustomizationCombiner.LODMeshInstances[LOD].combine_instances.Add(material, new List<CharacterCustomizationCombiner.CombineInstanceWithSM>());
							CharacterCustomizationCombiner.LODMeshInstances[LOD].unique_materials.Add(material);
						}
						CharacterCustomizationCombiner.CombineInstanceWithSM item = default(CharacterCustomizationCombiner.CombineInstanceWithSM);
						item.instance = new CombineInstance
						{
							transform = Matrix4x4.identity,
							subMeshIndex = i,
							mesh = sharedMesh
						};
						item.skinnedMesh = mesh;
						CharacterCustomizationCombiner.LODMeshInstances[LOD].combine_instances[material].Add(item);
					}
				}
			}
		}

		// Token: 0x04000C14 RID: 3092
		private static Matrix4x4[] bindPoses;

		// Token: 0x04000C15 RID: 3093
		private static List<CharacterCustomizationCombiner.MeshInstance> LODMeshInstances;

		// Token: 0x04000C16 RID: 3094
		private static CharacterCustomization currentCharacter;

		// Token: 0x04000C17 RID: 3095
		private static bool useExportToAnotherObject = false;

		// Token: 0x04000C18 RID: 3096
		private static bool BlendshapeTransferWork = false;

		// Token: 0x04000C19 RID: 3097
		private static Action<List<SkinnedMeshRenderer>> _callback;

		// Token: 0x04000C1A RID: 3098
		private static List<SkinnedMeshRenderer> returnSkinnedMeshes = new List<SkinnedMeshRenderer>();

		// Token: 0x02000200 RID: 512
		private class MeshInstance
		{
			// Token: 0x04000C1B RID: 3099
			public Dictionary<Material, List<CharacterCustomizationCombiner.CombineInstanceWithSM>> combine_instances = new Dictionary<Material, List<CharacterCustomizationCombiner.CombineInstanceWithSM>>();

			// Token: 0x04000C1C RID: 3100
			public List<Material> unique_materials = new List<Material>();

			// Token: 0x04000C1D RID: 3101
			public Mesh combined_new_mesh = new Mesh();

			// Token: 0x04000C1E RID: 3102
			public List<Vector3> combined_vertices = new List<Vector3>();

			// Token: 0x04000C1F RID: 3103
			public List<Vector2> combined_uv = new List<Vector2>();

			// Token: 0x04000C20 RID: 3104
			public List<Vector2> combined_uv2 = new List<Vector2>();

			// Token: 0x04000C21 RID: 3105
			public List<Vector2> combined_uv3 = new List<Vector2>();

			// Token: 0x04000C22 RID: 3106
			public List<Vector2> combined_uv4 = new List<Vector2>();

			// Token: 0x04000C23 RID: 3107
			public List<Vector3> normals = new List<Vector3>();

			// Token: 0x04000C24 RID: 3108
			public List<Vector4> tangents = new List<Vector4>();

			// Token: 0x04000C25 RID: 3109
			public Dictionary<Material, List<int>> combined_submesh_indices = new Dictionary<Material, List<int>>();

			// Token: 0x04000C26 RID: 3110
			public List<BoneWeight> combined_bone_weights = new List<BoneWeight>();

			// Token: 0x04000C27 RID: 3111
			public List<string> blendShapeNames = new List<string>();

			// Token: 0x04000C28 RID: 3112
			public List<float> blendShapeValues = new List<float>();

			// Token: 0x04000C29 RID: 3113
			public Dictionary<Mesh, int> vertex_offset_map = new Dictionary<Mesh, int>();

			// Token: 0x04000C2A RID: 3114
			public int vertex_index_offset;

			// Token: 0x04000C2B RID: 3115
			public int current_material_index;
		}

		// Token: 0x02000201 RID: 513
		private struct CombineInstanceWithSM
		{
			// Token: 0x04000C2C RID: 3116
			public SkinnedMeshRenderer skinnedMesh;

			// Token: 0x04000C2D RID: 3117
			public CombineInstance instance;
		}

		// Token: 0x02000202 RID: 514
		private struct BlendWeightData
		{
			// Token: 0x04000C2E RID: 3118
			public Vector3[] deltaVerts;

			// Token: 0x04000C2F RID: 3119
			public Vector3[] deltaNormals;

			// Token: 0x04000C30 RID: 3120
			public Vector3[] deltaTangents;
		}
	}
}
