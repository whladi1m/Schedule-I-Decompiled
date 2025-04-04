using System;
using System.Collections.Generic;
using ScheduleOne.EntityFramework;
using ScheduleOne.Management;
using ScheduleOne.NPCs;
using UnityEngine;
using UnityEngine.AI;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006D5 RID: 1749
	public static class NavMeshUtility
	{
		// Token: 0x06002FA2 RID: 12194 RVA: 0x000C651C File Offset: 0x000C471C
		public static float GetPathLength(NavMeshPath path)
		{
			if (path == null)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 1; i < path.corners.Length; i++)
			{
				num += Vector3.Distance(path.corners[i - 1], path.corners[i]);
			}
			return num;
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x000C6570 File Offset: 0x000C4770
		public static Transform GetAccessPoint(ITransitEntity entity, NPC npc)
		{
			if (entity == null)
			{
				return null;
			}
			float num = float.MaxValue;
			Transform result = null;
			BuildableItem buildableItem = entity as BuildableItem;
			for (int i = 0; i < entity.AccessPoints.Length; i++)
			{
				NavMeshPath navMeshPath;
				if ((!(buildableItem != null) || buildableItem.ParentProperty.DoBoundsContainPoint(entity.AccessPoints[i].position)) && npc.Movement.CanGetTo(entity.AccessPoints[i].position, 1f, out navMeshPath))
				{
					float num2 = (navMeshPath != null) ? NavMeshUtility.GetPathLength(navMeshPath) : Vector3.Distance(npc.transform.position, entity.AccessPoints[i].position);
					if (num2 < num)
					{
						num = num2;
						result = entity.AccessPoints[i];
					}
				}
			}
			return result;
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x000C662C File Offset: 0x000C482C
		public static bool IsAtTransitEntity(ITransitEntity entity, NPC npc, float distanceThreshold = 0.4f)
		{
			if (entity == null)
			{
				Console.LogWarning("IsAtTransitEntity: Entity is null!", null);
			}
			for (int i = 0; i < entity.AccessPoints.Length; i++)
			{
				if (Vector3.Distance(npc.transform.position, entity.AccessPoints[i].position) < distanceThreshold)
				{
					return true;
				}
				if (npc.Movement.IsAsCloseAsPossible(entity.AccessPoints[i].transform.position, distanceThreshold))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x000C66A0 File Offset: 0x000C48A0
		public static int GetNavMeshAgentID(string name)
		{
			for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
			{
				NavMeshBuildSettings settingsByIndex = NavMesh.GetSettingsByIndex(i);
				if (name == NavMesh.GetSettingsNameFromID(settingsByIndex.agentTypeID))
				{
					return settingsByIndex.agentTypeID;
				}
			}
			return -1;
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x000C66E4 File Offset: 0x000C48E4
		public static bool SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask, bool useCache = true)
		{
			if (useCache)
			{
				for (int i = 0; i < NavMeshUtility.sampleCacheKeys.Count; i++)
				{
					if (Vector3.SqrMagnitude(sourcePosition - NavMeshUtility.sampleCacheKeys[i]) < 1f)
					{
						hit = default(NavMeshHit);
						hit.position = NavMeshUtility.SampleCache[NavMeshUtility.sampleCacheKeys[i]];
						return true;
					}
				}
			}
			bool flag = NavMesh.SamplePosition(sourcePosition, out hit, maxDistance, areaMask);
			if (flag)
			{
				if ((float)NavMeshUtility.sampleCacheKeys.Count >= 10000f)
				{
					Console.LogWarning("Sample cache is full! Clearing cache...", null);
					NavMeshUtility.ClearCache();
				}
				Vector3 vector = NavMeshUtility.Quantize(sourcePosition, 0.1f);
				NavMeshUtility.sampleCacheKeys.Add(vector);
				NavMeshUtility.SampleCache.Add(vector, hit.position);
			}
			return flag;
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x000C67A1 File Offset: 0x000C49A1
		private static Vector3 Quantize(Vector3 position, float precision = 0.1f)
		{
			return new Vector3(Mathf.Round(position.x / precision) * precision, Mathf.Round(position.y / precision) * precision, Mathf.Round(position.z / precision) * precision);
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x000C67D5 File Offset: 0x000C49D5
		public static void ClearCache()
		{
			NavMeshUtility.SampleCache.Clear();
			NavMeshUtility.sampleCacheKeys.Clear();
		}

		// Token: 0x040021F9 RID: 8697
		public const float SAMPLE_MAX_DISTANCE = 2f;

		// Token: 0x040021FA RID: 8698
		public static Dictionary<Vector3, Vector3> SampleCache = new Dictionary<Vector3, Vector3>();

		// Token: 0x040021FB RID: 8699
		public static List<Vector3> sampleCacheKeys = new List<Vector3>();

		// Token: 0x040021FC RID: 8700
		public const float SAMPLE_CACHE_MAX_SQR_DIST = 1f;

		// Token: 0x040021FD RID: 8701
		public const float MAX_CACHE_SIZE = 10000f;
	}
}
