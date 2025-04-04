using System;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x020007E5 RID: 2021
	public class SteerPID
	{
		// Token: 0x060036F2 RID: 14066 RVA: 0x000E7A8C File Offset: 0x000E5C8C
		public float GetNewValue(float error, PID_Parameters pid_parameters)
		{
			float num = -pid_parameters.P * error;
			this.error_sum = SteerPID.AddValueToAverage(this.error_sum, Time.deltaTime * error, 1000f);
			float num2 = num - pid_parameters.I * this.error_sum;
			float num3 = (error - this.error_old) / Time.deltaTime;
			float result = num2 - pid_parameters.D * num3;
			this.error_old = error;
			return result;
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x000E7AEC File Offset: 0x000E5CEC
		public static float AddValueToAverage(float oldAverage, float valueToAdd, float count)
		{
			return (oldAverage * count + valueToAdd) / (count + 1f);
		}

		// Token: 0x040027EE RID: 10222
		private float error_old;

		// Token: 0x040027EF RID: 10223
		private float error_sum;
	}
}
