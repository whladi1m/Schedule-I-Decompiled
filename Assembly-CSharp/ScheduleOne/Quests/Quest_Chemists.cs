using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Management;

namespace ScheduleOne.Quests
{
	// Token: 0x020002E4 RID: 740
	public class Quest_Chemists : Quest_Employees
	{
		// Token: 0x0600109B RID: 4251 RVA: 0x0004A7B0 File Offset: 0x000489B0
		protected override void MinPass()
		{
			base.MinPass();
			if (this.AssignWorkEntry.State == EQuestState.Active)
			{
				using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((enumerator.Current as Chemist).Configuration as ChemistConfiguration).TotalStations > 0)
						{
							this.AssignWorkEntry.Complete();
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0004A834 File Offset: 0x00048A34
		public override List<Employee> GetEmployees()
		{
			return NetworkSingleton<EmployeeManager>.Instance.GetEmployeesByType(EEmployeeType.Chemist);
		}

		// Token: 0x040010D9 RID: 4313
		public QuestEntry AssignWorkEntry;
	}
}
