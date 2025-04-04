using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Management;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F1 RID: 753
	public class Quest_Packagers : Quest_Employees
	{
		// Token: 0x060010C0 RID: 4288 RVA: 0x0004AF6C File Offset: 0x0004916C
		protected override void MinPass()
		{
			base.MinPass();
			if (this.AssignWorkEntry.State == EQuestState.Active)
			{
				using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((enumerator.Current as Packager).Configuration as PackagerConfiguration).AssignedStationCount > 0)
						{
							this.AssignWorkEntry.Complete();
							break;
						}
					}
				}
			}
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0004AFF0 File Offset: 0x000491F0
		public override List<Employee> GetEmployees()
		{
			return NetworkSingleton<EmployeeManager>.Instance.GetEmployeesByType(EEmployeeType.Handler);
		}

		// Token: 0x040010F0 RID: 4336
		public QuestEntry AssignWorkEntry;
	}
}
