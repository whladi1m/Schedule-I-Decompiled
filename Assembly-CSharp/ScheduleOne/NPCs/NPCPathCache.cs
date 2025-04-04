using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ScheduleOne.NPCs
{
	// Token: 0x0200045A RID: 1114
	public class NPCPathCache
	{
		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060017BE RID: 6078 RVA: 0x00068F4E File Offset: 0x0006714E
		// (set) Token: 0x060017BF RID: 6079 RVA: 0x00068F56 File Offset: 0x00067156
		public List<NPCPathCache.PathCache> Paths { get; private set; } = new List<NPCPathCache.PathCache>();

		// Token: 0x060017C0 RID: 6080 RVA: 0x00068F60 File Offset: 0x00067160
		public NavMeshPath GetPath(Vector3 start, Vector3 end, float sqrMaxDistance)
		{
			foreach (NPCPathCache.PathCache pathCache in this.Paths)
			{
				if ((pathCache.Start - start).sqrMagnitude < sqrMaxDistance && (pathCache.End - end).sqrMagnitude < sqrMaxDistance)
				{
					return pathCache.Path;
				}
			}
			return null;
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x00068FE8 File Offset: 0x000671E8
		public void AddPath(Vector3 start, Vector3 end, NavMeshPath path)
		{
			this.Paths.Add(new NPCPathCache.PathCache(start, end, path));
		}

		// Token: 0x0200045B RID: 1115
		[Serializable]
		public class PathCache
		{
			// Token: 0x060017C3 RID: 6083 RVA: 0x00069010 File Offset: 0x00067210
			public PathCache(Vector3 start, Vector3 end, NavMeshPath path)
			{
				this.Start = start;
				this.End = end;
				this.Path = path;
			}

			// Token: 0x04001568 RID: 5480
			public Vector3 Start;

			// Token: 0x04001569 RID: 5481
			public Vector3 End;

			// Token: 0x0400156A RID: 5482
			public NavMeshPath Path;
		}
	}
}
