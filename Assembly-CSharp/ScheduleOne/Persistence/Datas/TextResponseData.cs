using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200042A RID: 1066
	[Serializable]
	public class TextResponseData
	{
		// Token: 0x06001590 RID: 5520 RVA: 0x0005FB60 File Offset: 0x0005DD60
		public TextResponseData(string text, string label)
		{
			this.Text = text;
			this.Label = label;
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0005FB76 File Offset: 0x0005DD76
		public TextResponseData()
		{
			this.Text = "";
			this.Label = "";
		}

		// Token: 0x0400145A RID: 5210
		public string Text;

		// Token: 0x0400145B RID: 5211
		public string Label;
	}
}
