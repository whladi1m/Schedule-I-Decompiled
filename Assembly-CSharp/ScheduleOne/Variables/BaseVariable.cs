using System;
using FishNet.Connection;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Variables
{
	// Token: 0x02000288 RID: 648
	public abstract class BaseVariable
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x0003C26E File Offset: 0x0003A46E
		// (set) Token: 0x06000D7B RID: 3451 RVA: 0x0003C276 File Offset: 0x0003A476
		public Player Owner { get; private set; }

		// Token: 0x06000D7C RID: 3452 RVA: 0x0003C280 File Offset: 0x0003A480
		public BaseVariable(string name, EVariableReplicationMode replicationMode, bool persistent, EVariableMode mode, Player owner)
		{
			this.Name = name;
			this.ReplicationMode = replicationMode;
			if (mode == EVariableMode.Global)
			{
				NetworkSingleton<VariableDatabase>.Instance.AddVariable(this);
			}
			else
			{
				if (owner == null)
				{
					Console.LogError("Player variable created without owner", null);
					return;
				}
				owner.AddVariable(this);
			}
			this.Persistent = persistent;
			this.VariableMode = mode;
			this.Owner = owner;
		}

		// Token: 0x06000D7D RID: 3453
		public abstract object GetValue();

		// Token: 0x06000D7E RID: 3454
		public abstract void SetValue(object value, bool replicate = true);

		// Token: 0x06000D7F RID: 3455
		public abstract void ReplicateValue(NetworkConnection conn);

		// Token: 0x06000D80 RID: 3456 RVA: 0x00014002 File Offset: 0x00012202
		public virtual bool EvaluateCondition(Condition.EConditionType operation, string value)
		{
			return false;
		}

		// Token: 0x04000E14 RID: 3604
		public EVariableReplicationMode ReplicationMode;

		// Token: 0x04000E15 RID: 3605
		public string Name;

		// Token: 0x04000E16 RID: 3606
		public bool Persistent;

		// Token: 0x04000E17 RID: 3607
		public EVariableMode VariableMode;
	}
}
