using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.NPCs
{
	// Token: 0x0200045C RID: 1116
	public class NPCSpeedController : MonoBehaviour
	{
		// Token: 0x060017C4 RID: 6084 RVA: 0x0006902D File Offset: 0x0006722D
		private void Awake()
		{
			this.AddSpeedControl(new NPCSpeedController.SpeedControl("default", 0, this.DefaultWalkSpeed));
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x00069048 File Offset: 0x00067248
		private void FixedUpdate()
		{
			NPCSpeedController.SpeedControl highestPriorityControl = this.GetHighestPriorityControl();
			this.ActiveSpeedControl = highestPriorityControl;
			if (this.Movement.DEBUG)
			{
				Debug.Log("Active speed control: " + highestPriorityControl.id + ", speed : " + highestPriorityControl.speed.ToString());
			}
			this.Movement.MovementSpeedScale = highestPriorityControl.speed * this.SpeedMultiplier;
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x000690AD File Offset: 0x000672AD
		private NPCSpeedController.SpeedControl GetHighestPriorityControl()
		{
			return this.speedControlStack[0];
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x000690BC File Offset: 0x000672BC
		public void AddSpeedControl(NPCSpeedController.SpeedControl control)
		{
			NPCSpeedController.SpeedControl speedControl = this.speedControlStack.Find((NPCSpeedController.SpeedControl x) => x.id == control.id);
			if (speedControl != null)
			{
				speedControl.priority = control.priority;
				speedControl.speed = control.speed;
				return;
			}
			for (int i = 0; i < this.speedControlStack.Count; i++)
			{
				if (control.priority >= this.speedControlStack[i].priority)
				{
					this.speedControlStack.Insert(i, control);
					return;
				}
			}
			this.speedControlStack.Add(control);
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0006916C File Offset: 0x0006736C
		public NPCSpeedController.SpeedControl GetSpeedControl(string id)
		{
			return this.speedControlStack.Find((NPCSpeedController.SpeedControl x) => x.id == id);
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0006919D File Offset: 0x0006739D
		public bool DoesSpeedControlExist(string id)
		{
			return this.GetSpeedControl(id) != null;
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x000691AC File Offset: 0x000673AC
		public void RemoveSpeedControl(string id)
		{
			NPCSpeedController.SpeedControl speedControl = this.speedControlStack.Find((NPCSpeedController.SpeedControl x) => x.id == id);
			if (speedControl != null)
			{
				this.speedControlStack.Remove(speedControl);
			}
		}

		// Token: 0x0400156B RID: 5483
		[Header("Settings")]
		[Range(0f, 1f)]
		public float DefaultWalkSpeed = 0.08f;

		// Token: 0x0400156C RID: 5484
		public float SpeedMultiplier = 1f;

		// Token: 0x0400156D RID: 5485
		[Header("References")]
		public NPCMovement Movement;

		// Token: 0x0400156E RID: 5486
		protected List<NPCSpeedController.SpeedControl> speedControlStack = new List<NPCSpeedController.SpeedControl>();

		// Token: 0x0400156F RID: 5487
		[Header("Debug")]
		public NPCSpeedController.SpeedControl ActiveSpeedControl;

		// Token: 0x0200045D RID: 1117
		[Serializable]
		public class SpeedControl
		{
			// Token: 0x060017CC RID: 6092 RVA: 0x00069217 File Offset: 0x00067417
			public SpeedControl(string id, int priority, float speed)
			{
				this.id = id;
				this.priority = priority;
				this.speed = speed;
			}

			// Token: 0x04001570 RID: 5488
			public string id;

			// Token: 0x04001571 RID: 5489
			public int priority;

			// Token: 0x04001572 RID: 5490
			public float speed;
		}
	}
}
