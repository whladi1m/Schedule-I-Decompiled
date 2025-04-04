using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x020005C3 RID: 1475
	public static class PenaltyHandler
	{
		// Token: 0x060024B8 RID: 9400 RVA: 0x00093D7C File Offset: 0x00091F7C
		public static List<string> ProcessCrimeList(Dictionary<Crime, int> crimes)
		{
			List<string> list = new List<string>();
			float num = 0f;
			Crime[] array = crimes.Keys.ToArray<Crime>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] is PossessingControlledSubstances)
				{
					float num2 = 5f * (float)crimes[array[i]];
					num += num2;
					list.Add(crimes[array[i]].ToString() + " controlled substances confiscated");
				}
				else if (array[i] is PossessingLowSeverityDrug)
				{
					float num3 = 10f * (float)crimes[array[i]];
					num += num3;
					list.Add(crimes[array[i]].ToString() + " low-severity drugs confiscated");
				}
				else if (array[i] is PossessingModerateSeverityDrug)
				{
					float num4 = 20f * (float)crimes[array[i]];
					num += num4;
					list.Add(crimes[array[i]].ToString() + " moderate-severity drugs confiscated");
				}
				else if (array[i] is PossessingHighSeverityDrug)
				{
					float num5 = 30f * (float)crimes[array[i]];
					num += num5;
					list.Add(crimes[array[i]].ToString() + " high-severity drugs confiscated");
				}
				else if (array[i] is Evading)
				{
					num += 50f;
				}
				else if (array[i] is FailureToComply)
				{
					num += 50f;
				}
				else if (array[i] is ViolatingCurfew)
				{
					num += 100f;
				}
				else if (array[i] is AttemptingToSell)
				{
					num += 150f;
				}
				else if (array[i] is Assault)
				{
					num += 75f;
				}
				else if (array[i] is DeadlyAssault)
				{
					num += 150f;
				}
				else if (array[i] is Vandalism)
				{
					num += 50f;
				}
				else if (array[i] is Theft)
				{
					num += 50f;
				}
				else if (array[i] is BrandishingWeapon)
				{
					num += 50f;
				}
				else if (array[i] is DischargeFirearm)
				{
					num += 50f;
				}
			}
			if (num > 0f)
			{
				float num6 = Mathf.Min(num, NetworkSingleton<MoneyManager>.Instance.cashBalance);
				string text = MoneyManager.FormatAmount(num, true, false) + " fine";
				if (num6 == num)
				{
					text += " (paid in cash)";
				}
				else
				{
					text = text + " (" + MoneyManager.FormatAmount(num6, true, false) + " paid";
					text += " - insufficient cash)";
				}
				list.Add(text);
				if (num6 > 0f)
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-num6, true, false);
				}
			}
			return list;
		}

		// Token: 0x04001B4B RID: 6987
		public const float CONTROLLED_SUBSTANCE_FINE = 5f;

		// Token: 0x04001B4C RID: 6988
		public const float LOW_SEVERITY_DRUG_FINE = 10f;

		// Token: 0x04001B4D RID: 6989
		public const float MED_SEVERITY_DRUG_FINE = 20f;

		// Token: 0x04001B4E RID: 6990
		public const float HIGH_SEVERITY_DRUG_FINE = 30f;

		// Token: 0x04001B4F RID: 6991
		public const float FAILURE_TO_COMPLY_FINE = 50f;

		// Token: 0x04001B50 RID: 6992
		public const float EVADING_ARREST_FINE = 50f;

		// Token: 0x04001B51 RID: 6993
		public const float VIOLATING_CURFEW_TIME = 100f;

		// Token: 0x04001B52 RID: 6994
		public const float ATTEMPT_TO_SELL_FINE = 150f;

		// Token: 0x04001B53 RID: 6995
		public const float ASSAULT_FINE = 75f;

		// Token: 0x04001B54 RID: 6996
		public const float DEADLY_ASSAULT_FINE = 150f;

		// Token: 0x04001B55 RID: 6997
		public const float VANDALISM_FINE = 50f;

		// Token: 0x04001B56 RID: 6998
		public const float THEFT_FINE = 50f;

		// Token: 0x04001B57 RID: 6999
		public const float BRANDISHING_FINE = 50f;

		// Token: 0x04001B58 RID: 7000
		public const float DISCHARGE_FIREARM_FINE = 50f;
	}
}
