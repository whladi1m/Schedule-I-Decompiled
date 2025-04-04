using System;
using FishNet;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Networking
{
	// Token: 0x02000532 RID: 1330
	public class NetworkConditionalObject : MonoBehaviour
	{
		// Token: 0x0600209F RID: 8351 RVA: 0x00086298 File Offset: 0x00084498
		private void Awake()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.Check));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.Check));
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x000862E8 File Offset: 0x000844E8
		public void Check()
		{
			NetworkConditionalObject.ECondition econdition = this.condition;
			if (econdition != NetworkConditionalObject.ECondition.All && econdition == NetworkConditionalObject.ECondition.HostOnly && !InstanceFinder.IsHost)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x04001939 RID: 6457
		public NetworkConditionalObject.ECondition condition;

		// Token: 0x02000533 RID: 1331
		public enum ECondition
		{
			// Token: 0x0400193B RID: 6459
			All,
			// Token: 0x0400193C RID: 6460
			HostOnly
		}
	}
}
