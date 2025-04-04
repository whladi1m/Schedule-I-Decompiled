using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.Levelling;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BEB RID: 3051
	[Serializable]
	public class MapRegionData
	{
		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06005580 RID: 21888 RVA: 0x00167ABB File Offset: 0x00165CBB
		public bool IsUnlocked
		{
			get
			{
				return NetworkSingleton<LevelManager>.InstanceExists && NetworkSingleton<LevelManager>.Instance.GetFullRank() >= this.RankRequirement;
			}
		}

		// Token: 0x06005581 RID: 21889 RVA: 0x00167ADC File Offset: 0x00165CDC
		public DeliveryLocation GetRandomUnscheduledDeliveryLocation()
		{
			List<DeliveryLocation> list = (from x in this.RegionDeliveryLocations
			where x.ScheduledContracts.Count == 0
			select x).ToList<DeliveryLocation>();
			if (list.Count == 0)
			{
				Console.LogWarning("No unscheduled delivery locations found for " + this.Region.ToString(), null);
				return null;
			}
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		// Token: 0x06005582 RID: 21890 RVA: 0x00167B58 File Offset: 0x00165D58
		public void SetUnlocked()
		{
			foreach (NPC npc in this.StartingNPCs)
			{
				if (!npc.RelationData.Unlocked)
				{
					npc.RelationData.Unlock(NPCRelationData.EUnlockType.DirectApproach, false);
				}
			}
		}

		// Token: 0x04003F77 RID: 16247
		public EMapRegion Region;

		// Token: 0x04003F78 RID: 16248
		public string Name;

		// Token: 0x04003F79 RID: 16249
		public FullRank RankRequirement;

		// Token: 0x04003F7A RID: 16250
		public NPC[] StartingNPCs;

		// Token: 0x04003F7B RID: 16251
		public Sprite RegionSprite;

		// Token: 0x04003F7C RID: 16252
		public DeliveryLocation[] RegionDeliveryLocations;
	}
}
