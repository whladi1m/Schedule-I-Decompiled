using System;
using HSVPicker;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCustomization
{
	// Token: 0x02000B29 RID: 2857
	public class BarbershopUI : CharacterCustomizationUI
	{
		// Token: 0x06004C0C RID: 19468 RVA: 0x00140917 File Offset: 0x0013EB17
		public override bool IsOptionCurrentlyApplied(CharacterCustomizationOption option)
		{
			return this.currentSettings.HairStyle == option.Label;
		}

		// Token: 0x06004C0D RID: 19469 RVA: 0x0014092F File Offset: 0x0013EB2F
		public override void OptionSelected(CharacterCustomizationOption option)
		{
			base.OptionSelected(option);
			this.currentSettings.HairStyle = option.Label;
			this.AvatarRig.ApplyHairSettings(this.currentSettings.GetAvatarSettings());
		}

		// Token: 0x06004C0E RID: 19470 RVA: 0x0014095F File Offset: 0x0013EB5F
		protected override void Update()
		{
			base.Update();
			if (!base.IsOpen)
			{
				return;
			}
			this.currentSettings == null;
		}

		// Token: 0x06004C0F RID: 19471 RVA: 0x0014097D File Offset: 0x0013EB7D
		public override void Open()
		{
			base.Open();
			this.ColorPicker.CurrentColor = this.currentSettings.HairColor;
			this.appliedColor = this.currentSettings.HairColor;
			this.ApplyColorButton.interactable = false;
		}

		// Token: 0x06004C10 RID: 19472 RVA: 0x001409B8 File Offset: 0x0013EBB8
		public void ColorFieldChanged(Color color)
		{
			this.currentSettings.HairColor = color;
			this.AvatarRig.ApplyHairColorSettings(this.currentSettings.GetAvatarSettings());
			this.ApplyColorButton.interactable = true;
		}

		// Token: 0x06004C11 RID: 19473 RVA: 0x001409E8 File Offset: 0x0013EBE8
		public void ApplyColorChange()
		{
			this.appliedColor = this.ColorPicker.CurrentColor;
			this.currentSettings.HairColor = this.appliedColor;
			this.AvatarRig.ApplyHairSettings(this.currentSettings.GetAvatarSettings());
			this.ApplyColorButton.interactable = false;
		}

		// Token: 0x06004C12 RID: 19474 RVA: 0x00140A3C File Offset: 0x0013EC3C
		public void RevertColorChange()
		{
			this.ColorPicker.CurrentColor = this.currentSettings.HairColor;
			this.currentSettings.HairColor = this.appliedColor;
			this.AvatarRig.ApplyHairSettings(this.currentSettings.GetAvatarSettings());
			this.ApplyColorButton.interactable = false;
		}

		// Token: 0x04003975 RID: 14709
		public HSVPicker.ColorPicker ColorPicker;

		// Token: 0x04003976 RID: 14710
		public Button ApplyColorButton;

		// Token: 0x04003977 RID: 14711
		private Color appliedColor = Color.black;
	}
}
