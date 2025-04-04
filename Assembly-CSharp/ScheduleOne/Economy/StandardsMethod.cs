using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Economy
{
	// Token: 0x02000667 RID: 1639
	public static class StandardsMethod
	{
		// Token: 0x06002D2C RID: 11564 RVA: 0x000BCFD0 File Offset: 0x000BB1D0
		public static string GetName(this ECustomerStandard property)
		{
			switch (property)
			{
			case ECustomerStandard.VeryLow:
				return "Very Low";
			case ECustomerStandard.Low:
				return "Low";
			case ECustomerStandard.Moderate:
				return "Moderate";
			case ECustomerStandard.High:
				return "High";
			case ECustomerStandard.VeryHigh:
				return "Very High";
			default:
				return "Standard";
			}
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000BD01C File Offset: 0x000BB21C
		public static EQuality GetCorrespondingQuality(this ECustomerStandard property)
		{
			switch (property)
			{
			case ECustomerStandard.VeryLow:
				return EQuality.Trash;
			case ECustomerStandard.Low:
				return EQuality.Poor;
			case ECustomerStandard.Moderate:
				return EQuality.Standard;
			case ECustomerStandard.High:
				return EQuality.Premium;
			case ECustomerStandard.VeryHigh:
				return EQuality.Heavenly;
			default:
				return EQuality.Standard;
			}
		}
	}
}
