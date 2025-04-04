using System;
using System.Collections.Generic;

namespace ScheduleOne.Police
{
	// Token: 0x02000332 RID: 818
	public class Offense
	{
		// Token: 0x060011F7 RID: 4599 RVA: 0x0004E290 File Offset: 0x0004C490
		public Offense(List<Offense.Charge> _charges)
		{
			this.charges.AddRange(_charges);
		}

		// Token: 0x0400116B RID: 4459
		public List<Offense.Charge> charges = new List<Offense.Charge>();

		// Token: 0x0400116C RID: 4460
		public List<string> penalties = new List<string>();

		// Token: 0x02000333 RID: 819
		public class Charge
		{
			// Token: 0x060011F8 RID: 4600 RVA: 0x0004E2BA File Offset: 0x0004C4BA
			public Charge(string _chargeName, int _crimeIndex, int _quantity)
			{
				this.chargeName = _chargeName;
				this.crimeIndex = _crimeIndex;
				this.quantity = _quantity;
			}

			// Token: 0x0400116D RID: 4461
			public string chargeName = "<ChargeName>";

			// Token: 0x0400116E RID: 4462
			public int crimeIndex = 1;

			// Token: 0x0400116F RID: 4463
			public int quantity = 1;
		}
	}
}
