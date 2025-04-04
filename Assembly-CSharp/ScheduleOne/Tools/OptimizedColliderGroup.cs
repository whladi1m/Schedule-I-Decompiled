using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200084E RID: 2126
	public class OptimizedColliderGroup : MonoBehaviour
	{
		// Token: 0x06003A0A RID: 14858 RVA: 0x000F48A4 File Offset: 0x000F2AA4
		private void OnEnable()
		{
			this.sqrColliderEnableMaxDistance = this.ColliderEnableMaxDistance * this.ColliderEnableMaxDistance;
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				this.RegisterEvent();
				return;
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.RegisterEvent));
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x000F48F2 File Offset: 0x000F2AF2
		private void OnDestroy()
		{
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				PlayerSingleton<PlayerMovement>.Instance.DeregisterMovementEvent(new Action(this.Refresh));
			}
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x000F4911 File Offset: 0x000F2B11
		private void RegisterEvent()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.RegisterEvent));
			PlayerSingleton<PlayerMovement>.Instance.RegisterMovementEvent(5, new Action(this.Refresh));
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x000F494A File Offset: 0x000F2B4A
		[Button]
		public void GetColliders()
		{
			this.Colliders = base.GetComponentsInChildren<Collider>();
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x000045B1 File Offset: 0x000027B1
		public void Start()
		{
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x000F4958 File Offset: 0x000F2B58
		private void Refresh()
		{
			if (Player.Local == null || Player.Local.Avatar == null)
			{
				return;
			}
			float sqrMagnitude = (Player.Local.Avatar.CenterPoint - base.transform.position).sqrMagnitude;
			this.SetCollidersEnabled(sqrMagnitude < this.sqrColliderEnableMaxDistance);
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x000F49BC File Offset: 0x000F2BBC
		private void SetCollidersEnabled(bool enabled)
		{
			if (this.collidersEnabled == enabled)
			{
				return;
			}
			this.collidersEnabled = enabled;
			foreach (Collider collider in this.Colliders)
			{
				if (!(collider == null))
				{
					collider.enabled = enabled;
				}
			}
		}

		// Token: 0x040029E9 RID: 10729
		public const int UPDATE_DISTANCE = 5;

		// Token: 0x040029EA RID: 10730
		public Collider[] Colliders;

		// Token: 0x040029EB RID: 10731
		public float ColliderEnableMaxDistance = 30f;

		// Token: 0x040029EC RID: 10732
		private float sqrColliderEnableMaxDistance;

		// Token: 0x040029ED RID: 10733
		private bool collidersEnabled = true;
	}
}
