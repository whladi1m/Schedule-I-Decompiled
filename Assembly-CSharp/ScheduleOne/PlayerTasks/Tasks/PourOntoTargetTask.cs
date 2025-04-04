using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.PlayerTasks.Tasks
{
	// Token: 0x02000359 RID: 857
	public class PourOntoTargetTask : PourIntoPotTask
	{
		// Token: 0x0600135D RID: 4957 RVA: 0x00056401 File Offset: 0x00054601
		public PourOntoTargetTask(Pot _pot, ItemInstance _itemInstance, Pourable _pourablePrefab) : base(_pot, _itemInstance, _pourablePrefab)
		{
			this.Target = _pot.Target;
			_pot.RandomizeTarget();
			_pot.SetTargetActive(true);
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x0005643C File Offset: 0x0005463C
		public override void Update()
		{
			base.Update();
			Vector3 vector = this.pourable.PourPoint.position - this.Target.position;
			vector.y = 0f;
			if (vector.magnitude < this.SUCCESS_THRESHOLD)
			{
				this.timeOverTarget += Time.deltaTime * this.pourable.NormalizedPourRate;
				if (this.timeOverTarget >= this.SUCCESS_TIME)
				{
					this.TargetReached();
					return;
				}
			}
			else
			{
				this.timeOverTarget = 0f;
			}
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x000564C9 File Offset: 0x000546C9
		public override void StopTask()
		{
			this.pot.SetTargetActive(false);
			base.StopTask();
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x000564DD File Offset: 0x000546DD
		public virtual void TargetReached()
		{
			this.pot.RandomizeTarget();
			this.timeOverTarget = 0f;
			Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
		}

		// Token: 0x04001292 RID: 4754
		public Transform Target;

		// Token: 0x04001293 RID: 4755
		public float SUCCESS_THRESHOLD = 0.12f;

		// Token: 0x04001294 RID: 4756
		public float SUCCESS_TIME = 0.4f;

		// Token: 0x04001295 RID: 4757
		private float timeOverTarget;
	}
}
