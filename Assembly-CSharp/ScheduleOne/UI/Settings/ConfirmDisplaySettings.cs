using System;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000A6B RID: 2667
	public class ConfirmDisplaySettings : MonoBehaviour
	{
		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x060047F8 RID: 18424 RVA: 0x0012DFA4 File Offset: 0x0012C1A4
		public bool IsOpen
		{
			get
			{
				return this != null && base.gameObject != null && base.gameObject.activeSelf;
			}
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x0012DFCA File Offset: 0x0012C1CA
		public void Awake()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 6);
			base.gameObject.SetActive(false);
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x0012DFEA File Offset: 0x0012C1EA
		public void Open(DisplaySettings _oldSettings, DisplaySettings _newSettings)
		{
			base.gameObject.SetActive(true);
			this.oldSettings = _oldSettings;
			this.newSettings = _newSettings;
			this.timeUntilRevert = 15f;
			this.Update();
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x0012E017 File Offset: 0x0012C217
		public void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.Close(true);
			}
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x0012E044 File Offset: 0x0012C244
		public void Update()
		{
			this.timeUntilRevert -= Time.unscaledDeltaTime;
			this.SubtitleLabel.text = string.Format("Reverting in {0:0.0} seconds", this.timeUntilRevert);
			if (this.timeUntilRevert <= 0f)
			{
				this.Close(true);
			}
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0012E098 File Offset: 0x0012C298
		public void Close(bool revert)
		{
			if (revert)
			{
				Singleton<Settings>.Instance.ApplyDisplaySettings(this.oldSettings);
				Singleton<Settings>.Instance.DisplaySettings = this.oldSettings;
				Singleton<Settings>.Instance.UnappliedDisplaySettings = this.oldSettings;
			}
			else
			{
				Singleton<Settings>.Instance.WriteDisplaySettings(this.newSettings);
			}
			base.transform.parent.gameObject.SetActive(false);
			base.transform.parent.gameObject.SetActive(true);
			base.gameObject.SetActive(false);
		}

		// Token: 0x04003591 RID: 13713
		public const float RevertTime = 15f;

		// Token: 0x04003592 RID: 13714
		public TextMeshProUGUI SubtitleLabel;

		// Token: 0x04003593 RID: 13715
		private float timeUntilRevert;

		// Token: 0x04003594 RID: 13716
		private DisplaySettings oldSettings;

		// Token: 0x04003595 RID: 13717
		private DisplaySettings newSettings;
	}
}
