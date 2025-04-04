using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200012D RID: 301
	public static class MeshGenerator
	{
		// Token: 0x06000513 RID: 1299 RVA: 0x00018928 File Offset: 0x00016B28
		private static float GetAngleOffset(int numSides)
		{
			if (numSides != 4)
			{
				return 0f;
			}
			return 0.7853982f;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00018939 File Offset: 0x00016B39
		private static float GetRadiiScale(int numSides)
		{
			if (numSides != 4)
			{
				return 1f;
			}
			return Mathf.Sqrt(2f);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00018950 File Offset: 0x00016B50
		public static Mesh GenerateConeZ_RadiusAndAngle(float lengthZ, float radiusStart, float coneAngle, int numSides, int numSegments, bool cap, bool doubleSided)
		{
			float radiusEnd = lengthZ * Mathf.Tan(coneAngle * 0.017453292f * 0.5f);
			return MeshGenerator.GenerateConeZ_Radii(lengthZ, radiusStart, radiusEnd, numSides, numSegments, cap, doubleSided);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00018981 File Offset: 0x00016B81
		public static Mesh GenerateConeZ_Angle(float lengthZ, float coneAngle, int numSides, int numSegments, bool cap, bool doubleSided)
		{
			return MeshGenerator.GenerateConeZ_RadiusAndAngle(lengthZ, 0f, coneAngle, numSides, numSegments, cap, doubleSided);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00018998 File Offset: 0x00016B98
		public static Mesh GenerateConeZ_Radii(float lengthZ, float radiusStart, float radiusEnd, int numSides, int numSegments, bool cap, bool doubleSided)
		{
			Mesh mesh = new Mesh();
			bool flag = cap && radiusStart > 0f;
			radiusStart = Mathf.Max(radiusStart, 0.001f);
			float radiiScale = MeshGenerator.GetRadiiScale(numSides);
			radiusStart *= radiiScale;
			radiusEnd *= radiiScale;
			int num = numSides * (numSegments + 2);
			int num2 = num;
			if (flag)
			{
				num2 += numSides + 1;
			}
			float angleOffset = MeshGenerator.GetAngleOffset(numSides);
			Vector3[] array = new Vector3[num2];
			for (int i = 0; i < numSides; i++)
			{
				float f = angleOffset + 6.2831855f * (float)i / (float)numSides;
				float num3 = Mathf.Cos(f);
				float num4 = Mathf.Sin(f);
				for (int j = 0; j < numSegments + 2; j++)
				{
					float num5 = (float)j / (float)(numSegments + 1);
					float num6 = Mathf.Lerp(radiusStart, radiusEnd, num5);
					array[i + j * numSides] = new Vector3(num6 * num3, num6 * num4, num5 * lengthZ);
				}
			}
			if (flag)
			{
				int num7 = num;
				array[num7] = Vector3.zero;
				num7++;
				for (int k = 0; k < numSides; k++)
				{
					float f2 = angleOffset + 6.2831855f * (float)k / (float)numSides;
					float num8 = Mathf.Cos(f2);
					float num9 = Mathf.Sin(f2);
					array[num7] = new Vector3(radiusStart * num8, radiusStart * num9, 0f);
					num7++;
				}
			}
			if (!doubleSided)
			{
				mesh.vertices = array;
			}
			else
			{
				Vector3[] array2 = new Vector3[array.Length * 2];
				array.CopyTo(array2, 0);
				array.CopyTo(array2, array.Length);
				mesh.vertices = array2;
			}
			Vector2[] array3 = new Vector2[num2];
			int num10 = 0;
			for (int l = 0; l < num; l++)
			{
				array3[num10++] = Vector2.zero;
			}
			if (flag)
			{
				for (int m = 0; m < numSides + 1; m++)
				{
					array3[num10++] = new Vector2(1f, 0f);
				}
			}
			if (!doubleSided)
			{
				mesh.uv = array3;
			}
			else
			{
				Vector2[] array4 = new Vector2[array3.Length * 2];
				array3.CopyTo(array4, 0);
				array3.CopyTo(array4, array3.Length);
				for (int n = 0; n < array3.Length; n++)
				{
					Vector2 vector = array4[n + array3.Length];
					array4[n + array3.Length] = new Vector2(vector.x, 1f);
				}
				mesh.uv = array4;
			}
			int num11 = numSides * 2 * Mathf.Max(numSegments + 1, 1) * 3;
			if (flag)
			{
				num11 += numSides * 3;
			}
			int[] array5 = new int[num11];
			int num12 = 0;
			for (int num13 = 0; num13 < numSides; num13++)
			{
				int num14 = num13 + 1;
				if (num14 == numSides)
				{
					num14 = 0;
				}
				for (int num15 = 0; num15 < numSegments + 1; num15++)
				{
					int num16 = num15 * numSides;
					array5[num12++] = num16 + num13;
					array5[num12++] = num16 + num14;
					array5[num12++] = num16 + num13 + numSides;
					array5[num12++] = num16 + num14 + numSides;
					array5[num12++] = num16 + num13 + numSides;
					array5[num12++] = num16 + num14;
				}
			}
			if (flag)
			{
				for (int num17 = 0; num17 < numSides - 1; num17++)
				{
					array5[num12++] = num;
					array5[num12++] = num + num17 + 2;
					array5[num12++] = num + num17 + 1;
				}
				array5[num12++] = num;
				array5[num12++] = num + 1;
				array5[num12++] = num + numSides;
			}
			if (!doubleSided)
			{
				mesh.triangles = array5;
			}
			else
			{
				int[] array6 = new int[array5.Length * 2];
				array5.CopyTo(array6, 0);
				for (int num18 = 0; num18 < array5.Length; num18 += 3)
				{
					array6[array5.Length + num18] = array5[num18] + num2;
					array6[array5.Length + num18 + 1] = array5[num18 + 2] + num2;
					array6[array5.Length + num18 + 2] = array5[num18 + 1] + num2;
				}
				mesh.triangles = array6;
			}
			mesh.bounds = MeshGenerator.ComputeBounds(lengthZ, radiusStart, radiusEnd);
			return mesh;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00018DAC File Offset: 0x00016FAC
		public static Mesh GenerateConeZ_Radii_DoubleCaps(float lengthZ, float radiusStart, float radiusEnd, int numSides, bool inverted)
		{
			MeshGenerator.<>c__DisplayClass6_0 CS$<>8__locals1 = new MeshGenerator.<>c__DisplayClass6_0();
			CS$<>8__locals1.numSides = numSides;
			Mesh mesh = new Mesh();
			radiusStart = Mathf.Max(radiusStart, 0.001f);
			CS$<>8__locals1.vertCountSides = CS$<>8__locals1.numSides * 2;
			int vertCountSides = CS$<>8__locals1.vertCountSides;
			CS$<>8__locals1.vertSidesStartFromSlide = ((int slideID) => CS$<>8__locals1.numSides * slideID);
			CS$<>8__locals1.vertCenterFromSlide = ((int slideID) => CS$<>8__locals1.vertCountSides + slideID);
			int num = vertCountSides + 2;
			float angleOffset = MeshGenerator.GetAngleOffset(CS$<>8__locals1.numSides);
			Vector3[] array = new Vector3[num];
			for (int i = 0; i < CS$<>8__locals1.numSides; i++)
			{
				float f = angleOffset + 6.2831855f * (float)i / (float)CS$<>8__locals1.numSides;
				float num2 = Mathf.Cos(f);
				float num3 = Mathf.Sin(f);
				for (int j = 0; j < 2; j++)
				{
					float num4 = (float)j;
					float num5 = Mathf.Lerp(radiusStart, radiusEnd, num4);
					array[i + CS$<>8__locals1.vertSidesStartFromSlide(j)] = new Vector3(num5 * num2, num5 * num3, num4 * lengthZ);
				}
			}
			array[CS$<>8__locals1.vertCenterFromSlide(0)] = Vector3.zero;
			array[CS$<>8__locals1.vertCenterFromSlide(1)] = new Vector3(0f, 0f, lengthZ);
			mesh.vertices = array;
			int num6 = CS$<>8__locals1.numSides * 2 * 3;
			num6 += CS$<>8__locals1.numSides * 3;
			num6 += CS$<>8__locals1.numSides * 3;
			int[] indices = new int[num6];
			int ind = 0;
			for (int k = 0; k < CS$<>8__locals1.numSides; k++)
			{
				int num7 = k + 1;
				if (num7 == CS$<>8__locals1.numSides)
				{
					num7 = 0;
				}
				for (int l = 0; l < 1; l++)
				{
					int num8 = l * CS$<>8__locals1.numSides;
					indices[ind] = num8 + k;
					indices[ind + (inverted ? 1 : 2)] = num8 + num7;
					indices[ind + (inverted ? 2 : 1)] = num8 + k + CS$<>8__locals1.numSides;
					indices[ind + 3] = num8 + num7 + CS$<>8__locals1.numSides;
					indices[ind + (inverted ? 4 : 5)] = num8 + k + CS$<>8__locals1.numSides;
					indices[ind + (inverted ? 5 : 4)] = num8 + num7;
					ind += 6;
				}
			}
			Action<int, bool> action = delegate(int slideID, bool invert)
			{
				int num9 = CS$<>8__locals1.vertSidesStartFromSlide(slideID);
				for (int m = 0; m < CS$<>8__locals1.numSides - 1; m++)
				{
					indices[ind] = CS$<>8__locals1.vertCenterFromSlide(slideID);
					indices[ind + (invert ? 1 : 2)] = num9 + m + 1;
					indices[ind + (invert ? 2 : 1)] = num9 + m;
					ind += 3;
				}
				indices[ind] = CS$<>8__locals1.vertCenterFromSlide(slideID);
				indices[ind + (invert ? 1 : 2)] = num9;
				indices[ind + (invert ? 2 : 1)] = num9 + CS$<>8__locals1.numSides - 1;
				ind += 3;
			};
			action(0, inverted);
			action(1, !inverted);
			mesh.triangles = indices;
			Bounds bounds = new Bounds(new Vector3(0f, 0f, lengthZ * 0.5f), new Vector3(Mathf.Max(radiusStart, radiusEnd) * 2f, Mathf.Max(radiusStart, radiusEnd) * 2f, lengthZ));
			mesh.bounds = bounds;
			return mesh;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000190F0 File Offset: 0x000172F0
		public static Bounds ComputeBounds(float lengthZ, float radiusStart, float radiusEnd)
		{
			float num = Mathf.Max(radiusStart, radiusEnd) * 2f;
			return new Bounds(new Vector3(0f, 0f, lengthZ * 0.5f), new Vector3(num, num, lengthZ));
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001912E File Offset: 0x0001732E
		private static int GetCapAdditionalVerticesCount(MeshGenerator.CapMode capMode, int numSides)
		{
			switch (capMode)
			{
			case MeshGenerator.CapMode.None:
				return 0;
			case MeshGenerator.CapMode.OneVertexPerCap_1Cap:
				return 1;
			case MeshGenerator.CapMode.OneVertexPerCap_2Caps:
				return 2;
			case MeshGenerator.CapMode.SpecificVerticesPerCap_1Cap:
				return numSides + 1;
			case MeshGenerator.CapMode.SpecificVerticesPerCap_2Caps:
				return 2 * (numSides + 1);
			default:
				return 0;
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001915D File Offset: 0x0001735D
		private static int GetCapAdditionalIndicesCount(MeshGenerator.CapMode capMode, int numSides)
		{
			switch (capMode)
			{
			case MeshGenerator.CapMode.None:
				return 0;
			case MeshGenerator.CapMode.OneVertexPerCap_1Cap:
			case MeshGenerator.CapMode.SpecificVerticesPerCap_1Cap:
				return numSides * 3;
			case MeshGenerator.CapMode.OneVertexPerCap_2Caps:
			case MeshGenerator.CapMode.SpecificVerticesPerCap_2Caps:
				return 2 * (numSides * 3);
			default:
				return 0;
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00019188 File Offset: 0x00017388
		public static int GetVertexCount(int numSides, int numSegments, MeshGenerator.CapMode capMode, bool doubleSided)
		{
			int num = numSides * (numSegments + 2);
			num += MeshGenerator.GetCapAdditionalVerticesCount(capMode, numSides);
			if (doubleSided)
			{
				num *= 2;
			}
			return num;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000191B0 File Offset: 0x000173B0
		public static int GetIndicesCount(int numSides, int numSegments, MeshGenerator.CapMode capMode, bool doubleSided)
		{
			int num = numSides * (numSegments + 1) * 2 * 3;
			num += MeshGenerator.GetCapAdditionalIndicesCount(capMode, numSides);
			if (doubleSided)
			{
				num *= 2;
			}
			return num;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000191D9 File Offset: 0x000173D9
		public static int GetSharedMeshVertexCount()
		{
			return MeshGenerator.GetVertexCount(Config.Instance.sharedMeshSides, Config.Instance.sharedMeshSegments, MeshGenerator.CapMode.SpecificVerticesPerCap_1Cap, Config.Instance.SD_requiresDoubleSidedMesh);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x000191FF File Offset: 0x000173FF
		public static int GetSharedMeshIndicesCount()
		{
			return MeshGenerator.GetIndicesCount(Config.Instance.sharedMeshSides, Config.Instance.sharedMeshSegments, MeshGenerator.CapMode.SpecificVerticesPerCap_1Cap, Config.Instance.SD_requiresDoubleSidedMesh);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00019225 File Offset: 0x00017425
		public static int GetSharedMeshHDVertexCount()
		{
			return MeshGenerator.GetVertexCount(Config.Instance.sharedMeshSides, 0, MeshGenerator.CapMode.OneVertexPerCap_2Caps, false);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00019239 File Offset: 0x00017439
		public static int GetSharedMeshHDIndicesCount()
		{
			return MeshGenerator.GetIndicesCount(Config.Instance.sharedMeshSides, 0, MeshGenerator.CapMode.OneVertexPerCap_2Caps, false);
		}

		// Token: 0x0400066B RID: 1643
		private const float kMinTruncatedRadius = 0.001f;

		// Token: 0x0200012E RID: 302
		public enum CapMode
		{
			// Token: 0x0400066D RID: 1645
			None,
			// Token: 0x0400066E RID: 1646
			OneVertexPerCap_1Cap,
			// Token: 0x0400066F RID: 1647
			OneVertexPerCap_2Caps,
			// Token: 0x04000670 RID: 1648
			SpecificVerticesPerCap_1Cap,
			// Token: 0x04000671 RID: 1649
			SpecificVerticesPerCap_2Caps
		}
	}
}
