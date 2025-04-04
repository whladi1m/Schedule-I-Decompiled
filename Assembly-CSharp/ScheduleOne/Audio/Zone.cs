using System;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007A5 RID: 1957
	public class Zone : MonoBehaviour
	{
		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060034FD RID: 13565 RVA: 0x000DE912 File Offset: 0x000DCB12
		// (set) Token: 0x060034FE RID: 13566 RVA: 0x000DE91A File Offset: 0x000DCB1A
		public float LocalPlayerDistance { get; protected set; }

		// Token: 0x060034FF RID: 13567 RVA: 0x000DE923 File Offset: 0x000DCB23
		private void Awake()
		{
			this.points = this.GetPoints();
			base.InvokeRepeating("Recalculate", 0f, 0.25f);
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x000DE948 File Offset: 0x000DCB48
		public void Recalculate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			Vector3 position = PlayerSingleton<PlayerCamera>.Instance.transform.position;
			Vector3 a = Vector3.zero;
			float f;
			if (this.IsClosed && this.DoBoundsContainPoint(position) && this.IsPointInsidePolygon(this.points, position))
			{
				f = 0f;
			}
			else
			{
				a = this.GetClosestPointOnPolygon(this.points, position);
				a.y = position.y;
				f = Vector3.Distance(a, position);
			}
			float f2 = 0f;
			Vector3 vector = base.transform.InverseTransformPoint(position);
			if (vector.y > this.VerticalSize)
			{
				f2 = vector.y - this.VerticalSize;
			}
			this.LocalPlayerDistance = Mathf.Sqrt(Mathf.Pow(f, 2f) + Mathf.Pow(f2, 2f));
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x000DEA18 File Offset: 0x000DCC18
		private void OnDrawGizmos()
		{
			if (this.PointContainer.childCount < 2)
			{
				return;
			}
			Vector3[] array = this.GetPoints();
			for (int i = 0; i < array.Length - 1; i++)
			{
				Vector3 vector = array[i];
				Vector3 vector2 = array[i + 1];
				Debug.DrawLine(vector, vector2, this.ZoneColor);
				Debug.DrawLine(vector + Vector3.up * this.VerticalSize, vector2 + Vector3.up * this.VerticalSize, this.ZoneColor);
				Gizmos.color = this.ZoneColor;
				Gizmos.DrawSphere(vector, 0.5f);
			}
			if (this.IsClosed)
			{
				Debug.DrawLine(array[array.Length - 1], array[0], this.ZoneColor);
				Debug.DrawLine(array[array.Length - 1] + Vector3.up * this.VerticalSize, array[0] + Vector3.up * this.VerticalSize, this.ZoneColor);
			}
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000DEB24 File Offset: 0x000DCD24
		private Vector3[] GetPoints()
		{
			if (this.PointContainer == null)
			{
				return new Vector3[0];
			}
			Vector3[] array = new Vector3[this.PointContainer.childCount];
			for (int i = 0; i < this.PointContainer.childCount; i++)
			{
				array[i] = this.PointContainer.GetChild(i).position;
			}
			return array;
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x000DEB88 File Offset: 0x000DCD88
		private bool DoBoundsContainPoint(Vector3 point)
		{
			Tuple<Vector3, Vector3> boundingPoints = this.GetBoundingPoints();
			return point.x >= boundingPoints.Item1.x && point.x <= boundingPoints.Item2.x && point.z >= boundingPoints.Item1.z && point.z <= boundingPoints.Item2.z;
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x000DEBF0 File Offset: 0x000DCDF0
		private Tuple<Vector3, Vector3> GetBoundingPoints()
		{
			Vector3[] source = this.GetPoints();
			float x = (from p in source
			select p.x).Max();
			float x2 = (from p in source
			select p.x).Min();
			float z = (from p in source
			select p.z).Max();
			float z2 = (from p in source
			select p.z).Min();
			return new Tuple<Vector3, Vector3>(new Vector3(x2, 0f, z2), new Vector3(x, this.VerticalSize, z));
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x000DECCC File Offset: 0x000DCECC
		private bool IsPointInsidePolygon(Vector3[] polyPoints, Vector3 point)
		{
			Vector2[] array = new Vector2[polyPoints.Length];
			for (int i = 0; i < polyPoints.Length; i++)
			{
				array[i] = new Vector2(polyPoints[i].x, polyPoints[i].z);
			}
			return this.CalculateWindingNumber(array, new Vector2(point.x, point.z)) != 0;
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x000DED30 File Offset: 0x000DCF30
		private int CalculateWindingNumber(Vector2[] polygon, Vector2 point)
		{
			int num = 0;
			for (int i = 0; i < polygon.Length; i++)
			{
				Vector2 vector = polygon[i];
				Vector2 vector2 = polygon[(i + 1) % polygon.Length];
				if (Zone.<CalculateWindingNumber>g__IsPointOnSegment|17_0(vector, vector2, point))
				{
					return 0;
				}
				if (vector.y <= point.y)
				{
					if (vector2.y > point.y && Zone.<CalculateWindingNumber>g__IsLeft|17_3(vector, vector2, point) > 0)
					{
						num++;
					}
				}
				else if (vector2.y <= point.y && Zone.<CalculateWindingNumber>g__IsLeft|17_3(vector, vector2, point) < 0)
				{
					num--;
				}
			}
			return num;
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x000DEDBC File Offset: 0x000DCFBC
		private Vector3 GetClosestPointOnPolygon(Vector3[] polyPoints, Vector3 point)
		{
			Vector3 result = Vector3.zero;
			float num = float.PositiveInfinity;
			for (int i = 0; i < polyPoints.Length - 1; i++)
			{
				Vector3 lineStart = polyPoints[i];
				Vector3 lineEnd = polyPoints[i + 1];
				Vector3 vector = Zone.<GetClosestPointOnPolygon>g__ProjectPointOnLineSegment|18_0(lineStart, lineEnd, point);
				float num2 = Vector3.Distance(point, vector);
				if (num2 < num)
				{
					num = num2;
					result = vector;
				}
			}
			if (this.IsClosed)
			{
				Vector3 lineStart2 = polyPoints[polyPoints.Length - 1];
				Vector3 lineEnd2 = polyPoints[0];
				Vector3 vector2 = Zone.<GetClosestPointOnPolygon>g__ProjectPointOnLineSegment|18_0(lineStart2, lineEnd2, point);
				float num3 = Vector3.Distance(point, vector2);
				if (num3 < num)
				{
					result = vector2;
				}
			}
			return result;
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000DEE78 File Offset: 0x000DD078
		[CompilerGenerated]
		internal static bool <CalculateWindingNumber>g__IsPointOnSegment|17_0(Vector2 start, Vector2 end, Vector2 point)
		{
			if (Mathf.Abs(Zone.<CalculateWindingNumber>g__CrossProduct|17_1(start, end, point)) > 0.001f)
			{
				return false;
			}
			float num = Zone.<CalculateWindingNumber>g__DotProduct|17_2(start, end, point);
			if (num < 0f)
			{
				return false;
			}
			float sqrMagnitude = (end - start).sqrMagnitude;
			return num <= sqrMagnitude;
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000DEEC5 File Offset: 0x000DD0C5
		[CompilerGenerated]
		internal static float <CalculateWindingNumber>g__CrossProduct|17_1(Vector2 start, Vector2 end, Vector2 point)
		{
			return (point.x - start.x) * (end.y - start.y) - (point.y - start.y) * (end.x - start.x);
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000DEEFE File Offset: 0x000DD0FE
		[CompilerGenerated]
		internal static float <CalculateWindingNumber>g__DotProduct|17_2(Vector2 start, Vector2 end, Vector2 point)
		{
			return (point.x - start.x) * (end.x - start.x) + (point.y - start.y) * (end.y - start.y);
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000DEF38 File Offset: 0x000DD138
		[CompilerGenerated]
		internal static int <CalculateWindingNumber>g__IsLeft|17_3(Vector2 start, Vector2 end, Vector2 point)
		{
			float num = Zone.<CalculateWindingNumber>g__CrossProduct|17_1(start, end, point);
			if (Mathf.Abs(num) < 0.001f)
			{
				return 0;
			}
			if (num > 0f)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000DEF68 File Offset: 0x000DD168
		[CompilerGenerated]
		internal static Vector3 <GetClosestPointOnPolygon>g__ProjectPointOnLineSegment|18_0(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			Vector3 vector = lineEnd - lineStart;
			float magnitude = vector.magnitude;
			vector.Normalize();
			float num = Vector3.Dot(point - lineStart, vector);
			num = Mathf.Clamp(num, 0f, magnitude);
			return lineStart + vector * num;
		}

		// Token: 0x04002641 RID: 9793
		public const float UPDATE_INTERVAL = 0.25f;

		// Token: 0x04002642 RID: 9794
		public Transform PointContainer;

		// Token: 0x04002643 RID: 9795
		public bool IsClosed = true;

		// Token: 0x04002644 RID: 9796
		public float VerticalSize = 5f;

		// Token: 0x04002646 RID: 9798
		[Header("Debug")]
		public Color ZoneColor = Color.white;

		// Token: 0x04002647 RID: 9799
		private Vector3[] points;
	}
}
