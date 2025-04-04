using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management.Presets;
using ScheduleOne.Management.Presets.Options;
using ScheduleOne.Management.SetterScreens;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x02000578 RID: 1400
	public class PotPresetEditScreen : PresetEditScreen
	{
		// Token: 0x0600230C RID: 8972 RVA: 0x0008F888 File Offset: 0x0008DA88
		protected override void Awake()
		{
			base.Awake();
			this.SeedsUI.Button.onClick.AddListener(new UnityAction(this.SeedsUIClicked));
			this.AdditivesUI.Button.onClick.AddListener(new UnityAction(this.AdditivesUIClicked));
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x0008F8DD File Offset: 0x0008DADD
		protected virtual void Update()
		{
			if (base.isOpen)
			{
				this.UpdateUI();
			}
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0008F8ED File Offset: 0x0008DAED
		public override void Open(Preset preset)
		{
			base.Open(preset);
			this.castedPreset = (PotPreset)this.EditedPreset;
			this.UpdateUI();
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x0008F910 File Offset: 0x0008DB10
		private void UpdateUI()
		{
			this.SeedsUI.ValueLabel.text = this.castedPreset.Seeds.GetDisplayString();
			this.AdditivesUI.ValueLabel.text = this.castedPreset.Additives.GetDisplayString();
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x0008F95D File Offset: 0x0008DB5D
		public void SeedsUIClicked()
		{
			Singleton<ItemSetterScreen>.Instance.Open((this.EditedPreset as PotPreset).Seeds);
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x0008F979 File Offset: 0x0008DB79
		public void AdditivesUIClicked()
		{
			Singleton<ItemSetterScreen>.Instance.Open((this.EditedPreset as PotPreset).Additives);
		}

		// Token: 0x04001A40 RID: 6720
		public GenericOptionUI SeedsUI;

		// Token: 0x04001A41 RID: 6721
		public GenericOptionUI AdditivesUI;

		// Token: 0x04001A42 RID: 6722
		private PotPreset castedPreset;
	}
}
