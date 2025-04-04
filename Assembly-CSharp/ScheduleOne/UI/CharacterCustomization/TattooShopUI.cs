using System;
using ScheduleOne.AvatarFramework;

namespace ScheduleOne.UI.CharacterCustomization
{
	// Token: 0x02000B31 RID: 2865
	public class TattooShopUI : CharacterCustomizationUI
	{
		// Token: 0x06004C51 RID: 19537 RVA: 0x00141838 File Offset: 0x0013FA38
		public override bool IsOptionCurrentlyApplied(CharacterCustomizationOption option)
		{
			Console.Log("Checking if tattoo is applied: " + option.Label, null);
			Console.Log((this.currentSettings.Tattoos != null) ? string.Join(", ", this.currentSettings.Tattoos.ToArray()) : "No tattoos applied", null);
			return this.currentSettings.Tattoos != null && this.currentSettings.Tattoos.Contains(option.Label);
		}

		// Token: 0x06004C52 RID: 19538 RVA: 0x001418B4 File Offset: 0x0013FAB4
		public override void OptionSelected(CharacterCustomizationOption option)
		{
			base.OptionSelected(option);
			if (!this.currentSettings.Tattoos.Contains(option.Label))
			{
				this.currentSettings.Tattoos.Add(option.Label);
			}
			AvatarSettings avatarSettings = this.currentSettings.GetAvatarSettings();
			this.AvatarRig.ApplyBodyLayerSettings(avatarSettings, 19);
			this.AvatarRig.ApplyFaceLayerSettings(avatarSettings);
		}

		// Token: 0x06004C53 RID: 19539 RVA: 0x0014191C File Offset: 0x0013FB1C
		public override void OptionDeselected(CharacterCustomizationOption option)
		{
			base.OptionDeselected(option);
			this.currentSettings.Tattoos.Remove(option.Label);
			AvatarSettings avatarSettings = this.currentSettings.GetAvatarSettings();
			this.AvatarRig.ApplyBodyLayerSettings(avatarSettings, 19);
			this.AvatarRig.ApplyFaceLayerSettings(avatarSettings);
		}
	}
}
