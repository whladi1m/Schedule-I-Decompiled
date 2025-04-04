using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003AA RID: 938
	public class EmployeeLoader : NPCLoader
	{
		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x0005CE39 File Offset: 0x0005B039
		public override string NPCType
		{
			get
			{
				return typeof(EmployeeData).Name;
			}
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0005CE54 File Offset: 0x0005B054
		public Employee LoadAndCreateEmployee(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, "NPC", out json))
			{
				EmployeeData employeeData = null;
				try
				{
					employeeData = JsonUtility.FromJson<EmployeeData>(json);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
					return null;
				}
				if (employeeData == null)
				{
					Console.LogWarning("Failed to load employee data", null);
					return null;
				}
				Property property = Singleton<PropertyManager>.Instance.GetProperty(employeeData.AssignedProperty);
				EEmployeeType type2 = EEmployeeType.Botanist;
				if (employeeData.DataType == typeof(PackagerData).Name)
				{
					type2 = EEmployeeType.Handler;
				}
				else if (employeeData.DataType == typeof(BotanistData).Name)
				{
					type2 = EEmployeeType.Botanist;
				}
				else if (employeeData.DataType == typeof(ChemistData).Name)
				{
					type2 = EEmployeeType.Chemist;
				}
				else if (employeeData.DataType == typeof(CleanerData).Name)
				{
					type2 = EEmployeeType.Cleaner;
				}
				else
				{
					Console.LogError("Failed to recognize employee type: " + employeeData.DataType, null);
				}
				Employee employee = NetworkSingleton<EmployeeManager>.Instance.CreateEmployee_Server(property, type2, employeeData.FirstName, employeeData.LastName, employeeData.ID, employeeData.IsMale, employeeData.AppearanceIndex, employeeData.Position, employeeData.Rotation, employeeData.GUID);
				if (employee == null)
				{
					Console.LogWarning("Failed to create employee", null);
					return null;
				}
				if (employeeData.PaidForToday)
				{
					employee.SetIsPaid();
				}
				base.TryLoadInventory(mainPath, employee);
				return employee;
			}
			return null;
		}
	}
}
