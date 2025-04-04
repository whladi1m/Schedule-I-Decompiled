using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000153 RID: 339
	public static class Utils
	{
		// Token: 0x06000657 RID: 1623 RVA: 0x0001CD7D File Offset: 0x0001AF7D
		public static float ComputeConeRadiusEnd(float fallOffEnd, float spotAngle)
		{
			return fallOffEnd * Mathf.Tan(spotAngle * 0.017453292f * 0.5f);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0001CD93 File Offset: 0x0001AF93
		public static float ComputeSpotAngle(float fallOffEnd, float coneRadiusEnd)
		{
			return Mathf.Atan2(coneRadiusEnd, fallOffEnd) * 57.29578f * 2f;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001CDA8 File Offset: 0x0001AFA8
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001CDCF File Offset: 0x0001AFCF
		public static string GetPath(Transform current)
		{
			if (current.parent == null)
			{
				return "/" + current.name;
			}
			return Utils.GetPath(current.parent) + "/" + current.name;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001CE0B File Offset: 0x0001B00B
		public static T NewWithComponent<T>(string name) where T : Component
		{
			return new GameObject(name, new Type[]
			{
				typeof(T)
			}).GetComponent<T>();
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001CE2C File Offset: 0x0001B02C
		public static T GetOrAddComponent<T>(this GameObject self) where T : Component
		{
			T t = self.GetComponent<T>();
			if (t == null)
			{
				t = self.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001CE56 File Offset: 0x0001B056
		public static T GetOrAddComponent<T>(this MonoBehaviour self) where T : Component
		{
			return self.gameObject.GetOrAddComponent<T>();
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001CE64 File Offset: 0x0001B064
		public static void ForeachComponentsInAnyChildrenOnly<T>(this GameObject self, Action<T> lambda, bool includeInactive = false) where T : Component
		{
			foreach (T t in self.GetComponentsInChildren<T>(includeInactive))
			{
				if (t.gameObject != self)
				{
					lambda(t);
				}
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001CEAC File Offset: 0x0001B0AC
		public static void ForeachComponentsInDirectChildrenOnly<T>(this GameObject self, Action<T> lambda, bool includeInactive = false) where T : Component
		{
			foreach (T t in self.GetComponentsInChildren<T>(includeInactive))
			{
				if (t.transform.parent == self.transform)
				{
					lambda(t);
				}
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001CEFC File Offset: 0x0001B0FC
		public static void SetupDepthCamera(Camera depthCamera, float coneApexOffsetZ, float maxGeometryDistance, float coneRadiusStart, float coneRadiusEnd, Vector3 beamLocalForward, Vector3 lossyScale, bool isScalable, Quaternion beamInternalLocalRotation, bool shouldScaleMinNearClipPlane)
		{
			if (!isScalable)
			{
				lossyScale.x = (lossyScale.y = 1f);
			}
			bool flag = coneApexOffsetZ >= 0f;
			float num = Mathf.Max(coneApexOffsetZ, 0f);
			depthCamera.orthographic = !flag;
			depthCamera.transform.localPosition = beamLocalForward * -num;
			Quaternion quaternion = beamInternalLocalRotation;
			if (Mathf.Sign(lossyScale.z) < 0f)
			{
				quaternion *= Quaternion.Euler(0f, 180f, 0f);
			}
			depthCamera.transform.localRotation = quaternion;
			if (!Mathf.Approximately(lossyScale.y * lossyScale.z, 0f))
			{
				float num2 = flag ? 0.1f : 0f;
				float num3 = Mathf.Abs(lossyScale.z);
				depthCamera.nearClipPlane = Mathf.Max(num * num3, num2 * (shouldScaleMinNearClipPlane ? num3 : 1f));
				depthCamera.farClipPlane = (maxGeometryDistance + num * (isScalable ? 1f : num3)) * (isScalable ? num3 : 1f);
				depthCamera.aspect = Mathf.Abs(lossyScale.x / lossyScale.y);
				if (flag)
				{
					float fieldOfView = Mathf.Atan2(coneRadiusEnd * Mathf.Abs(lossyScale.y), depthCamera.farClipPlane) * 57.29578f * 2f;
					depthCamera.fieldOfView = fieldOfView;
					return;
				}
				depthCamera.orthographicSize = coneRadiusStart * lossyScale.y;
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001D076 File Offset: 0x0001B276
		public static bool HasFlag(this Enum mask, Enum flags)
		{
			return ((int)mask & (int)flags) == (int)flags;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001D090 File Offset: 0x0001B290
		public static Vector3 Divide(this Vector3 aVector, Vector3 scale)
		{
			if (Mathf.Approximately(scale.x * scale.y * scale.z, 0f))
			{
				return Vector3.zero;
			}
			return new Vector3(aVector.x / scale.x, aVector.y / scale.y, aVector.z / scale.z);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001D0EF File Offset: 0x0001B2EF
		public static Vector2 xy(this Vector3 aVector)
		{
			return new Vector2(aVector.x, aVector.y);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001D102 File Offset: 0x0001B302
		public static Vector2 xz(this Vector3 aVector)
		{
			return new Vector2(aVector.x, aVector.z);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001D115 File Offset: 0x0001B315
		public static Vector2 yz(this Vector3 aVector)
		{
			return new Vector2(aVector.y, aVector.z);
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0001D128 File Offset: 0x0001B328
		public static Vector2 yx(this Vector3 aVector)
		{
			return new Vector2(aVector.y, aVector.x);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001D13B File Offset: 0x0001B33B
		public static Vector2 zx(this Vector3 aVector)
		{
			return new Vector2(aVector.z, aVector.x);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001D14E File Offset: 0x0001B34E
		public static Vector2 zy(this Vector3 aVector)
		{
			return new Vector2(aVector.z, aVector.y);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0001D161 File Offset: 0x0001B361
		public static bool Approximately(this float a, float b, float epsilon = 1E-05f)
		{
			return Mathf.Abs(a - b) < epsilon;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0001D16E File Offset: 0x0001B36E
		public static bool Approximately(this Vector2 a, Vector2 b, float epsilon = 1E-05f)
		{
			return Vector2.SqrMagnitude(a - b) < epsilon;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0001D17F File Offset: 0x0001B37F
		public static bool Approximately(this Vector3 a, Vector3 b, float epsilon = 1E-05f)
		{
			return Vector3.SqrMagnitude(a - b) < epsilon;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001D190 File Offset: 0x0001B390
		public static bool Approximately(this Vector4 a, Vector4 b, float epsilon = 1E-05f)
		{
			return Vector4.SqrMagnitude(a - b) < epsilon;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001D1A1 File Offset: 0x0001B3A1
		public static Vector4 AsVector4(this Vector3 vec3, float w)
		{
			return new Vector4(vec3.x, vec3.y, vec3.z, w);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0001D1BB File Offset: 0x0001B3BB
		public static Vector4 PlaneEquation(Vector3 normalizedNormal, Vector3 pt)
		{
			return normalizedNormal.AsVector4(-Vector3.Dot(normalizedNormal, pt));
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001D1CB File Offset: 0x0001B3CB
		public static float GetVolumeCubic(this Bounds self)
		{
			return self.size.x * self.size.y * self.size.z;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001D1F4 File Offset: 0x0001B3F4
		public static float GetMaxArea2D(this Bounds self)
		{
			return Mathf.Max(Mathf.Max(self.size.x * self.size.y, self.size.y * self.size.z), self.size.x * self.size.z);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001D256 File Offset: 0x0001B456
		public static Color Opaque(this Color self)
		{
			return new Color(self.r, self.g, self.b, 1f);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001D274 File Offset: 0x0001B474
		public static Color ComputeComplementaryColor(this Color self, bool blackAndWhite)
		{
			if (!blackAndWhite)
			{
				return new Color(1f - self.r, 1f - self.g, 1f - self.b);
			}
			if ((double)self.r * 0.299 + (double)self.g * 0.587 + (double)self.b * 0.114 <= 0.729411780834198)
			{
				return Color.white;
			}
			return Color.black;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001D2F9 File Offset: 0x0001B4F9
		public static Plane TranslateCustom(this Plane plane, Vector3 translation)
		{
			plane.distance += Vector3.Dot(translation.normalized, plane.normal) * translation.magnitude;
			return plane;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001D325 File Offset: 0x0001B525
		public static Vector3 ClosestPointOnPlaneCustom(this Plane plane, Vector3 point)
		{
			return point - plane.GetDistanceToPoint(point) * plane.normal;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001D341 File Offset: 0x0001B541
		public static bool IsAlmostZero(float f)
		{
			return Mathf.Abs(f) < 0.001f;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001D350 File Offset: 0x0001B550
		public static bool IsValid(this Plane plane)
		{
			return plane.normal.sqrMagnitude > 0.5f;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001D373 File Offset: 0x0001B573
		public static void SetKeywordEnabled(this Material mat, string name, bool enabled)
		{
			if (enabled)
			{
				mat.EnableKeyword(name);
				return;
			}
			mat.DisableKeyword(name);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001D387 File Offset: 0x0001B587
		public static void SetShaderKeywordEnabled(string name, bool enabled)
		{
			if (enabled)
			{
				Shader.EnableKeyword(name);
				return;
			}
			Shader.DisableKeyword(name);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001D39C File Offset: 0x0001B59C
		public static Matrix4x4 SampleInMatrix(this Gradient self, int floatPackingPrecision)
		{
			Matrix4x4 result = default(Matrix4x4);
			for (int i = 0; i < 16; i++)
			{
				Color color = self.Evaluate(Mathf.Clamp01((float)i / 15f));
				result[i] = color.PackToFloat(floatPackingPrecision);
			}
			return result;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001D3E4 File Offset: 0x0001B5E4
		public static Color[] SampleInArray(this Gradient self, int samplesCount)
		{
			Color[] array = new Color[samplesCount];
			for (int i = 0; i < samplesCount; i++)
			{
				array[i] = self.Evaluate(Mathf.Clamp01((float)i / (float)(samplesCount - 1)));
			}
			return array;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001D41E File Offset: 0x0001B61E
		private static Vector4 Vector4_Floor(Vector4 vec)
		{
			return new Vector4(Mathf.Floor(vec.x), Mathf.Floor(vec.y), Mathf.Floor(vec.z), Mathf.Floor(vec.w));
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001D454 File Offset: 0x0001B654
		public static float PackToFloat(this Color color, int floatPackingPrecision)
		{
			Vector4 vector = Utils.Vector4_Floor(color * (float)(floatPackingPrecision - 1));
			return 0f + vector.x * (float)floatPackingPrecision * (float)floatPackingPrecision * (float)floatPackingPrecision + vector.y * (float)floatPackingPrecision * (float)floatPackingPrecision + vector.z * (float)floatPackingPrecision + vector.w;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001D4A9 File Offset: 0x0001B6A9
		public static Utils.FloatPackingPrecision GetFloatPackingPrecision()
		{
			if (Utils.ms_FloatPackingPrecision == Utils.FloatPackingPrecision.Undef)
			{
				Utils.ms_FloatPackingPrecision = ((SystemInfo.graphicsShaderLevel >= 35) ? Utils.FloatPackingPrecision.High : Utils.FloatPackingPrecision.Low);
			}
			return Utils.ms_FloatPackingPrecision;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001D4CA File Offset: 0x0001B6CA
		public static bool HasAtLeastOneFlag(this Enum mask, Enum flags)
		{
			return ((int)mask & (int)flags) != 0;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x000045B1 File Offset: 0x000027B1
		public static void MarkCurrentSceneDirty()
		{
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x000045B1 File Offset: 0x000027B1
		public static void MarkObjectDirty(UnityEngine.Object obj)
		{
		}

		// Token: 0x0400074E RID: 1870
		private const float kEpsilon = 1E-05f;

		// Token: 0x0400074F RID: 1871
		private static Utils.FloatPackingPrecision ms_FloatPackingPrecision;

		// Token: 0x04000750 RID: 1872
		private const int kFloatPackingHighMinShaderLevel = 35;

		// Token: 0x02000154 RID: 340
		public enum FloatPackingPrecision
		{
			// Token: 0x04000752 RID: 1874
			High = 64,
			// Token: 0x04000753 RID: 1875
			Low = 8,
			// Token: 0x04000754 RID: 1876
			Undef = 0
		}
	}
}
