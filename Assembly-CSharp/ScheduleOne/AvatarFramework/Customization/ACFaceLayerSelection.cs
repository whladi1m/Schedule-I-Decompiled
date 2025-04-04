using System;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x02000987 RID: 2439
	public class ACFaceLayerSelection : ACSelection<FaceLayer>
	{
		// Token: 0x06004220 RID: 16928 RVA: 0x001156E5 File Offset: 0x001138E5
		public override string GetOptionLabel(int index)
		{
			return this.Options[index].Name;
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x001156F8 File Offset: 0x001138F8
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

		// Token: 0x06004222 RID: 16930 RVA: 0x0011576C File Offset: 0x0011396C
		public override int GetAssetPathIndex(string path)
		{
			FaceLayer faceLayer = this.Options.Find((FaceLayer x) => x.AssetPath == path);
			if (!(faceLayer != null))
			{
				return -1;
			}
			return this.Options.IndexOf(faceLayer);
		}
	}
}
