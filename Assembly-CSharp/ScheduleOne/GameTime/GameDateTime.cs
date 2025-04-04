using System;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.GameTime
{
	// Token: 0x020002A8 RID: 680
	[Serializable]
	public struct GameDateTime
	{
		// Token: 0x06000E3F RID: 3647 RVA: 0x0003F853 File Offset: 0x0003DA53
		public GameDateTime(int _elapsedDays, int _time)
		{
			this.elapsedDays = _elapsedDays;
			this.time = _time;
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0003F864 File Offset: 0x0003DA64
		public GameDateTime(int _minSum)
		{
			this.elapsedDays = _minSum / 1440;
			int minSum = _minSum % 1440;
			if (_minSum < 0)
			{
				minSum = -_minSum % 1440;
			}
			this.time = TimeManager.Get24HourTimeFromMinSum(minSum);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0003F89F File Offset: 0x0003DA9F
		public GameDateTime(GameDateTimeData data)
		{
			this.elapsedDays = data.ElapsedDays;
			this.time = data.Time;
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0003F8B9 File Offset: 0x0003DAB9
		public int GetMinSum()
		{
			return this.elapsedDays * 1440 + TimeManager.GetMinSumFrom24HourTime(this.time);
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0003F8D3 File Offset: 0x0003DAD3
		public GameDateTime AddMins(int mins)
		{
			return new GameDateTime(this.GetMinSum() + mins);
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0003F8E2 File Offset: 0x0003DAE2
		public static GameDateTime operator +(GameDateTime a, GameDateTime b)
		{
			return new GameDateTime(a.GetMinSum() + b.GetMinSum());
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0003F8F8 File Offset: 0x0003DAF8
		public static GameDateTime operator -(GameDateTime a, GameDateTime b)
		{
			return new GameDateTime(a.GetMinSum() - b.GetMinSum());
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0003F90E File Offset: 0x0003DB0E
		public static bool operator >(GameDateTime a, GameDateTime b)
		{
			return a.GetMinSum() > b.GetMinSum();
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0003F920 File Offset: 0x0003DB20
		public static bool operator <(GameDateTime a, GameDateTime b)
		{
			return a.GetMinSum() < b.GetMinSum();
		}

		// Token: 0x04000EE9 RID: 3817
		public int elapsedDays;

		// Token: 0x04000EEA RID: 3818
		public int time;
	}
}
