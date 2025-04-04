using System;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x02000984 RID: 2436
	public class ACAvatarLayerSelection : ACSelection<AvatarLayer>
	{
		// Token: 0x06004218 RID: 16920 RVA: 0x001155DC File Offset: 0x001137DC
		public override string GetOptionLabel(int index)
		{
			return this.Options[index].Name;
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x001155F0 File Offset: 0x001137F0
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

		// Token: 0x0600421A RID: 16922 RVA: 0x00115664 File Offset: 0x00113864
		public override int GetAssetPathIndex(string path)
		{
			AvatarLayer avatarLayer = this.Options.Find((AvatarLayer x) => x.AssetPath == path);
			if (!(avatarLayer != null))
			{
				return -1;
			}
			return this.Options.IndexOf(avatarLayer);
		}
	}
}
