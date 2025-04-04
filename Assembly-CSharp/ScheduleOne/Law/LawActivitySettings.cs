using System;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x020005BE RID: 1470
	[Serializable]
	public class LawActivitySettings
	{
		// Token: 0x06002483 RID: 9347 RVA: 0x00093558 File Offset: 0x00091758
		public void Evaluate()
		{
			for (int i = 0; i < this.Patrols.Length; i++)
			{
				this.Patrols[i].Evaluate();
			}
			for (int j = 0; j < this.Checkpoints.Length; j++)
			{
				this.Checkpoints[j].Evaluate();
			}
			for (int k = 0; k < this.Curfews.Length; k++)
			{
				this.Curfews[k].Evaluate(false);
			}
			for (int l = 0; l < this.VehiclePatrols.Length; l++)
			{
				this.VehiclePatrols[l].Evaluate();
			}
			for (int m = 0; m < this.Sentries.Length; m++)
			{
				this.Sentries[m].Evaluate();
			}
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x0009360C File Offset: 0x0009180C
		public void End()
		{
			for (int i = 0; i < this.Curfews.Length; i++)
			{
				if (this.Curfews[i].Enabled)
				{
					this.Curfews[i].shouldDisable = true;
				}
			}
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x0009364C File Offset: 0x0009184C
		public void OnLoaded()
		{
			Debug.Log("Settings loaded");
			for (int i = 0; i < this.Curfews.Length; i++)
			{
				this.Curfews[i].Evaluate(true);
			}
		}

		// Token: 0x04001B29 RID: 6953
		public PatrolInstance[] Patrols;

		// Token: 0x04001B2A RID: 6954
		public CheckpointInstance[] Checkpoints;

		// Token: 0x04001B2B RID: 6955
		public CurfewInstance[] Curfews;

		// Token: 0x04001B2C RID: 6956
		public VehiclePatrolInstance[] VehiclePatrols;

		// Token: 0x04001B2D RID: 6957
		public SentryInstance[] Sentries;
	}
}
