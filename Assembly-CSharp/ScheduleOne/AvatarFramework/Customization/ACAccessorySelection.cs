using System;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x02000982 RID: 2434
	public class ACAccessorySelection : ACSelection<Accessory>
	{
		// Token: 0x06004212 RID: 16914 RVA: 0x001154EF File Offset: 0x001136EF
		public override string GetOptionLabel(int index)
		{
			return this.Options[index].Name;
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x00115504 File Offset: 0x00113704
		public override void CallValueChange()
		{
			if (this.onValueChange != null)
			{
				this.onValueChange.Invoke((this.SelectedOptionIndex == -1) ? null : this.Options[this.SelectedOptionIndex]);
			}
			if (this.onValueChangeWithIndex != null)
			{
				this.onValueChangeWithIndex.Invoke((this.SelectedOptionIndex == -1) ? null : this.Options[this.SelectedOptionIndex], this.PropertyIndex);
			}
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x00115578 File Offset: 0x00113778
		public override int GetAssetPathIndex(string path)
		{
			Accessory accessory = this.Options.Find((Accessory x) => x.AssetPath == path);
			if (!(accessory != null))
			{
				return -1;
			}
			return this.Options.IndexOf(accessory);
		}
	}
}
