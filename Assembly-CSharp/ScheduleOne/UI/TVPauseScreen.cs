using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.TV;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x0200099E RID: 2462
	public class TVPauseScreen : MonoBehaviour
	{
		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06004287 RID: 17031 RVA: 0x00116F17 File Offset: 0x00115117
		// (set) Token: 0x06004288 RID: 17032 RVA: 0x00116F1F File Offset: 0x0011511F
		public bool IsPaused { get; private set; }

		// Token: 0x06004289 RID: 17033 RVA: 0x00116F28 File Offset: 0x00115128
		private void Awake()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x00116F3C File Offset: 0x0011513C
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.IsPaused)
			{
				return;
			}
			if (!this.App.IsOpen)
			{
				return;
			}
			action.used = true;
			this.Back();
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x00116F6B File Offset: 0x0011516B
		public void Pause()
		{
			this.IsPaused = true;
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x00116F80 File Offset: 0x00115180
		public void Resume()
		{
			this.IsPaused = false;
			base.gameObject.SetActive(false);
			this.App.Resume();
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x00116FA0 File Offset: 0x001151A0
		public void Back()
		{
			this.App.Close();
		}

		// Token: 0x0400307F RID: 12415
		public TVApp App;
	}
}
