using System;
using System.Collections.Generic;

namespace ScheduleOne.Management.Presets.Options
{
	// Token: 0x02000591 RID: 1425
	public class ItemList : Option
	{
		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x0600238A RID: 9098 RVA: 0x00090B4F File Offset: 0x0008ED4F
		// (set) Token: 0x0600238B RID: 9099 RVA: 0x00090B57 File Offset: 0x0008ED57
		public bool CanBeAll { get; protected set; } = true;

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x0600238C RID: 9100 RVA: 0x00090B60 File Offset: 0x0008ED60
		// (set) Token: 0x0600238D RID: 9101 RVA: 0x00090B68 File Offset: 0x0008ED68
		public bool CanBeNone { get; protected set; } = true;

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x0600238E RID: 9102 RVA: 0x00090B71 File Offset: 0x0008ED71
		// (set) Token: 0x0600238F RID: 9103 RVA: 0x00090B79 File Offset: 0x0008ED79
		public List<string> OptionList { get; protected set; } = new List<string>();

		// Token: 0x06002390 RID: 9104 RVA: 0x00090B84 File Offset: 0x0008ED84
		public ItemList(string name, List<string> optionList, bool canBeAll, bool canBeNone) : base(name)
		{
			this.OptionList.AddRange(optionList);
			this.CanBeAll = canBeAll;
			this.CanBeNone = canBeNone;
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x00090BD8 File Offset: 0x0008EDD8
		public override void CopyTo(Option other)
		{
			base.CopyTo(other);
			ItemList itemList = other as ItemList;
			itemList.All = this.All;
			itemList.None = this.None;
			itemList.Selection = new List<string>(this.Selection);
			itemList.CanBeAll = this.CanBeAll;
			itemList.CanBeNone = this.CanBeNone;
			itemList.OptionList = new List<string>(this.OptionList);
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x00090C44 File Offset: 0x0008EE44
		public override string GetDisplayString()
		{
			if (this.All)
			{
				return "All";
			}
			if (this.None || this.Selection.Count == 0)
			{
				return "None";
			}
			List<string> list = new List<string>();
			for (int i = 0; i < this.Selection.Count; i++)
			{
				list.Add(Registry.GetItem(this.Selection[i]).Name);
			}
			return string.Join(", ", list);
		}

		// Token: 0x04001A89 RID: 6793
		public bool All;

		// Token: 0x04001A8A RID: 6794
		public bool None;

		// Token: 0x04001A8B RID: 6795
		public List<string> Selection = new List<string>();
	}
}
