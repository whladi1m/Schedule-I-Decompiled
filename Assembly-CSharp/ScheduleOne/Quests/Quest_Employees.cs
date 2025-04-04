using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.Employees;

namespace ScheduleOne.Quests
{
	// Token: 0x020002E9 RID: 745
	public abstract class Quest_Employees : Quest
	{
		// Token: 0x060010A9 RID: 4265
		public abstract List<Employee> GetEmployees();

		// Token: 0x060010AA RID: 4266 RVA: 0x0004AAB0 File Offset: 0x00048CB0
		protected override void MinPass()
		{
			base.MinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.AssignBedEntry.State == EQuestState.Active && this.AreAnyEmployeesAssignedBeds())
			{
				this.AssignBedEntry.Complete();
			}
			if (this.PayEntry.State == EQuestState.Active && this.AreAnyEmployeesPaid())
			{
				this.PayEntry.Complete();
			}
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0004AB10 File Offset: 0x00048D10
		protected bool AreAnyEmployeesAssignedBeds()
		{
			using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetBed() != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0004AB70 File Offset: 0x00048D70
		protected bool AreAnyEmployeesPaid()
		{
			using (List<Employee>.Enumerator enumerator = this.GetEmployees().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.PaidForToday)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040010DD RID: 4317
		public EEmployeeType EmployeeType;

		// Token: 0x040010DE RID: 4318
		public QuestEntry AssignBedEntry;

		// Token: 0x040010DF RID: 4319
		public QuestEntry PayEntry;
	}
}
