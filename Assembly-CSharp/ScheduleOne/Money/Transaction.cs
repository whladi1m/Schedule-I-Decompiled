using System;

namespace ScheduleOne.Money
{
	// Token: 0x02000B65 RID: 2917
	public class Transaction
	{
		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06004DC4 RID: 19908 RVA: 0x0014814C File Offset: 0x0014634C
		public float total_Amount
		{
			get
			{
				return this.unit_Amount * this.quantity;
			}
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x0014815C File Offset: 0x0014635C
		public Transaction(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
		{
			this.transaction_Name = _transaction_Name;
			this.unit_Amount = _unit_Amount;
			this.quantity = _quantity;
			this.transaction_Note = _transaction_Note;
		}

		// Token: 0x04003AD4 RID: 15060
		public string transaction_Name = string.Empty;

		// Token: 0x04003AD5 RID: 15061
		public float unit_Amount;

		// Token: 0x04003AD6 RID: 15062
		public float quantity = 1f;

		// Token: 0x04003AD7 RID: 15063
		public string transaction_Note = string.Empty;
	}
}
