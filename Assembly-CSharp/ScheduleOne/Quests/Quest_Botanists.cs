using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;

namespace ScheduleOne.Quests
{
	// Token: 0x020002E3 RID: 739
	public class Quest_Botanists : Quest_Employees
	{
		// Token: 0x06001098 RID: 4248 RVA: 0x0004A5DC File Offset: 0x000487DC
		protected override void MinPass()
		{
			base.MinPass();
			if (this.AssignSuppliesEntry.State == EQuestState.Active)
			{
				using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((enumerator.Current as Botanist).Configuration as BotanistConfiguration).Supplies.SelectedObject != null)
						{
							this.AssignSuppliesEntry.Complete();
							break;
						}
					}
				}
			}
			if (this.AssignWorkEntry.State == EQuestState.Active)
			{
				using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((enumerator.Current as Botanist).Configuration as BotanistConfiguration).AssignedPots.Count > 0)
						{
							this.AssignWorkEntry.Complete();
							break;
						}
					}
				}
			}
			if (this.AssignDestinationEntry.State == EQuestState.Active)
			{
				foreach (Employee employee in this.GetEmployees())
				{
					using (List<Pot>.Enumerator enumerator2 = ((employee as Botanist).Configuration as BotanistConfiguration).AssignedPots.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if ((enumerator2.Current.Configuration as PotConfiguration).Destination.SelectedObject != null)
							{
								this.AssignDestinationEntry.Complete();
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0004A798 File Offset: 0x00048998
		public override List<Employee> GetEmployees()
		{
			return NetworkSingleton<EmployeeManager>.Instance.GetEmployeesByType(EEmployeeType.Botanist);
		}

		// Token: 0x040010D6 RID: 4310
		public QuestEntry AssignSuppliesEntry;

		// Token: 0x040010D7 RID: 4311
		public QuestEntry AssignWorkEntry;

		// Token: 0x040010D8 RID: 4312
		public QuestEntry AssignDestinationEntry;
	}
}
