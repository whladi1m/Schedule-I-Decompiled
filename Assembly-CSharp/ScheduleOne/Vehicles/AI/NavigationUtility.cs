using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using ScheduleOne.DevUtilities;
using ScheduleOne.Math;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x020007D6 RID: 2006
	public class NavigationUtility
	{
		// Token: 0x060036B5 RID: 14005 RVA: 0x000E632C File Offset: 0x000E452C
		public static Coroutine CalculatePath(Vector3 startPosition, Vector3 destination, NavigationSettings navSettings, DriveFlags flags, Seeker generalSeeker, Seeker roadSeeker, NavigationUtility.NavigationCalculationCallback callback)
		{
			NavigationUtility.<>c__DisplayClass5_0 CS$<>8__locals1 = new NavigationUtility.<>c__DisplayClass5_0();
			CS$<>8__locals1.flags = flags;
			CS$<>8__locals1.startPosition = startPosition;
			CS$<>8__locals1.destination = destination;
			CS$<>8__locals1.generalSeeker = generalSeeker;
			CS$<>8__locals1.roadSeeker = roadSeeker;
			CS$<>8__locals1.navSettings = navSettings;
			CS$<>8__locals1.callback = callback;
			return Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<CalculatePath>g__Routine|2());
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x000E6384 File Offset: 0x000E4584
		private static void AdjustExitPoint(PathGroup group)
		{
			if (group.entryToExitPath.vectorPath.Count < 4 || group.exitToDestinationPath.vectorPath.Count < 2)
			{
				return;
			}
			if (group.exitToDestinationPath.GetTotalLength() < 5f)
			{
				return;
			}
			for (int i = Mathf.Min(5, group.exitToDestinationPath.vectorPath.Count - 1); i >= 0; i--)
			{
				Vector3 vector = group.exitToDestinationPath.vectorPath[i];
				Vector3 vector2 = Vector3.zero;
				float num = float.MaxValue;
				int num2 = 0;
				for (int j = 0; j < 3; j++)
				{
					int num3 = group.entryToExitPath.vectorPath.Count - 1 - j;
					int index = num3 - 1;
					Vector3 line_end = group.entryToExitPath.vectorPath[num3];
					Vector3 line_start = group.entryToExitPath.vectorPath[index];
					Vector3 closestPointOnFiniteLine = NavigationUtility.GetClosestPointOnFiniteLine(vector, line_start, line_end);
					if (Vector3.Distance(vector, closestPointOnFiniteLine) < num)
					{
						num = Vector3.Distance(vector, closestPointOnFiniteLine);
						vector2 = closestPointOnFiniteLine;
						num2 = num3;
					}
				}
				if (vector2 == Vector3.zero)
				{
					Debug.LogWarning("Failed to find closest entry-to-exit path point");
					return;
				}
				float num4 = 0f;
				for (int k = 0; k < i; k++)
				{
					num4 += Vector3.Distance(group.exitToDestinationPath.vectorPath[k], group.exitToDestinationPath.vectorPath[k + 1]);
				}
				num4 += Vector3.Distance(vector2, group.entryToExitPath.vectorPath[num2]);
				for (int l = num2; l < group.entryToExitPath.vectorPath.Count - 1; l++)
				{
					num4 += Vector3.Distance(group.entryToExitPath.vectorPath[l], group.entryToExitPath.vectorPath[l + 1]);
				}
				if (num < num4 * 0.5f)
				{
					for (int m = num2; m < group.entryToExitPath.vectorPath.Count; m++)
					{
						group.entryToExitPath.vectorPath.RemoveAt(num2);
					}
					group.entryToExitPath.vectorPath.Insert(num2, vector2);
					for (int n = 0; n < i; n++)
					{
						group.exitToDestinationPath.vectorPath.RemoveAt(0);
					}
					Debug.DrawLine(vector, vector2, Color.green, 1f);
					return;
				}
			}
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x000E65E4 File Offset: 0x000E47E4
		private static void AdjustEntryPoint(PathGroup group)
		{
			if (group.startToEntryPath == null || group.startToEntryPath.vectorPath.Count < 2)
			{
				return;
			}
			if (group.startToEntryPath.GetTotalLength() < 5f)
			{
				return;
			}
			if (group.entryToExitPath == null || group.entryToExitPath.vectorPath.Count < 2)
			{
				return;
			}
			if (group.entryToExitPath.GetTotalLength() < 5f)
			{
				return;
			}
			float d = 2f;
			Vector3 a = group.startToEntryPath.vectorPath[group.startToEntryPath.vectorPath.Count - 1];
			Vector3 b = group.startToEntryPath.vectorPath[group.startToEntryPath.vectorPath.Count - 2];
			Vector3 normalized = (a - b).normalized;
			Vector3 value = a - normalized * d;
			group.startToEntryPath.vectorPath[group.startToEntryPath.vectorPath.Count - 1] = value;
			Vector3 vector = group.entryToExitPath.vectorPath[0];
			normalized = (group.entryToExitPath.vectorPath[1] - vector).normalized;
			Vector3 value2 = vector + normalized * d;
			group.entryToExitPath.vectorPath[0] = value2;
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x000E6734 File Offset: 0x000E4934
		private static bool DoesCloseDistanceExist(List<Vector3> vectorList, Vector3 point, float thresholdDistance)
		{
			using (List<Vector3>.Enumerator enumerator = vectorList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (Vector3.Distance(enumerator.Current, point) <= thresholdDistance)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x000E678C File Offset: 0x000E498C
		private static IEnumerator GenerateNavigationGroup(Vector3 startPoint, Vector3 entryPoint, NodeLink exitLink, Vector3 exitPoint, Vector3 destination, Seeker generalSeeker, Seeker roadSeeker, NavigationUtility.PathGroupEvent callback)
		{
			NavigationUtility.<>c__DisplayClass9_0 CS$<>8__locals1 = new NavigationUtility.<>c__DisplayClass9_0();
			Vector3 closestPointOnGraph = AstarUtility.GetClosestPointOnGraph(startPoint, "General Vehicle Graph");
			Vector3 destinationOnGraph = AstarUtility.GetClosestPointOnGraph(destination, "General Vehicle Graph");
			CS$<>8__locals1.lastCalculatedPath = null;
			generalSeeker.StartPath(closestPointOnGraph, entryPoint, new OnPathDelegate(CS$<>8__locals1.<GenerateNavigationGroup>g__PathCompleted|0));
			yield return new WaitUntil(() => CS$<>8__locals1.lastCalculatedPath != null);
			if (CS$<>8__locals1.lastCalculatedPath.error)
			{
				callback(null);
				yield break;
			}
			Path path_StartToEntry = CS$<>8__locals1.lastCalculatedPath;
			CS$<>8__locals1.lastCalculatedPath = null;
			Vector3 position = NodeLink.GetClosestLinks(entryPoint)[0].Start.position;
			roadSeeker.StartPath(position, exitLink.Start.position, new OnPathDelegate(CS$<>8__locals1.<GenerateNavigationGroup>g__PathCompleted|0));
			yield return new WaitUntil(() => CS$<>8__locals1.lastCalculatedPath != null);
			if (CS$<>8__locals1.lastCalculatedPath.error)
			{
				callback(null);
				yield break;
			}
			CS$<>8__locals1.lastCalculatedPath.vectorPath[0] = entryPoint;
			CS$<>8__locals1.lastCalculatedPath.vectorPath.Add(exitPoint);
			Path path_EntryToExit = CS$<>8__locals1.lastCalculatedPath;
			CS$<>8__locals1.lastCalculatedPath = null;
			generalSeeker.StartPath(exitPoint, destinationOnGraph, new OnPathDelegate(CS$<>8__locals1.<GenerateNavigationGroup>g__PathCompleted|0));
			yield return new WaitUntil(() => CS$<>8__locals1.lastCalculatedPath != null);
			if (CS$<>8__locals1.lastCalculatedPath.error)
			{
				callback(null);
				yield break;
			}
			Path lastCalculatedPath = CS$<>8__locals1.lastCalculatedPath;
			callback(new PathGroup
			{
				entryPoint = entryPoint,
				startToEntryPath = path_StartToEntry,
				entryToExitPath = path_EntryToExit,
				exitToDestinationPath = lastCalculatedPath
			});
			yield break;
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x000E67DC File Offset: 0x000E49DC
		public static void DrawPath(PathGroup group, float duration = 10f)
		{
			if (group.startToEntryPath != null)
			{
				for (int i = 1; i < group.startToEntryPath.vectorPath.Count; i++)
				{
					Debug.DrawLine(group.startToEntryPath.vectorPath[i], group.startToEntryPath.vectorPath[i - 1], Color.red, duration);
				}
			}
			if (group.entryToExitPath != null)
			{
				for (int j = 1; j < group.entryToExitPath.vectorPath.Count; j++)
				{
					if (j % 2 == 0)
					{
						Debug.DrawLine(group.entryToExitPath.vectorPath[j], group.entryToExitPath.vectorPath[j - 1], Color.blue, duration);
					}
					else
					{
						Debug.DrawLine(group.entryToExitPath.vectorPath[j], group.entryToExitPath.vectorPath[j - 1], Color.white, duration);
					}
				}
			}
			if (group.exitToDestinationPath != null)
			{
				for (int k = 1; k < group.exitToDestinationPath.vectorPath.Count; k++)
				{
					Debug.DrawLine(group.exitToDestinationPath.vectorPath[k], group.exitToDestinationPath.vectorPath[k - 1], Color.yellow, duration);
				}
			}
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x000E6918 File Offset: 0x000E4B18
		private static PathSmoothingUtility.SmoothedPath GetSmoothedPath(PathGroup group)
		{
			List<Vector3> list = new List<Vector3>();
			if (group.startToEntryPath != null)
			{
				list.AddRange(group.startToEntryPath.vectorPath);
			}
			if (group.entryToExitPath != null)
			{
				list.AddRange(group.entryToExitPath.vectorPath);
			}
			if (group.exitToDestinationPath != null)
			{
				list.AddRange(group.exitToDestinationPath.vectorPath);
			}
			return PathSmoothingUtility.CalculateSmoothedPath(list, 5f);
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x000E6984 File Offset: 0x000E4B84
		public static Vector3 SampleVehicleGraph(Vector3 destination)
		{
			NNConstraint nnconstraint = new NNConstraint();
			nnconstraint.graphMask = GraphMask.FromGraphName("General Vehicle Graph");
			return AstarPath.active.GetNearest(destination, nnconstraint).position;
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x000E69B8 File Offset: 0x000E4BB8
		public static Vector3 GetClosestPointOnFiniteLine(Vector3 point, Vector3 line_start, Vector3 line_end)
		{
			Vector3 vector = line_end - line_start;
			float magnitude = vector.magnitude;
			vector.Normalize();
			float d = Mathf.Clamp(Vector3.Dot(point - line_start, vector), 0f, magnitude);
			return line_start + vector * d;
		}

		// Token: 0x040027AD RID: 10157
		public const float ROAD_MULTIPLIER = 1f;

		// Token: 0x040027AE RID: 10158
		public const float OFFROAD_MULTIPLIER = 3f;

		// Token: 0x020007D7 RID: 2007
		public enum ENavigationCalculationResult
		{
			// Token: 0x040027B0 RID: 10160
			Success,
			// Token: 0x040027B1 RID: 10161
			Failed
		}

		// Token: 0x020007D8 RID: 2008
		// (Invoke) Token: 0x060036C0 RID: 14016
		public delegate void NavigationCalculationCallback(NavigationUtility.ENavigationCalculationResult result, PathSmoothingUtility.SmoothedPath path);

		// Token: 0x020007D9 RID: 2009
		// (Invoke) Token: 0x060036C4 RID: 14020
		public delegate void PathGroupEvent(PathGroup calculatedGroup);
	}
}
