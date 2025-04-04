using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Management;

namespace ScheduleOne.Quests
{
	// Token: 0x020002E6 RID: 742
	public class Quest_Cleaners : Quest_Employees
	{
		// Token: 0x060010A0 RID: 4256 RVA: 0x0004A914 File Offset: 0x00048B14
		protected override void MinPass()
		{
			base.MinPass();
			if (this.AssignWorkEntry.State == EQuestState.Active)
			{
				using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((enumerator.Current as Cleaner).Configuration as CleanerConfiguration).binItems.Count > 0)
						{
							this.AssignWorkEntry.Complete();
							break;
						}
					}
				}
			}
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0004A99C File Offset: 0x00048B9C
		public override List<Employee> GetEmployees()
		{
			return NetworkSingleton<EmployeeManager>.Instance.GetEmployeesByType(EEmployeeType.Cleaner);
		}

		// Token: 0x040010DC RID: 4316
		public QuestEntry AssignWorkEntry;
	}
}
