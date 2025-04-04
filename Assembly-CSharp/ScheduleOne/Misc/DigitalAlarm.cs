using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using TMPro;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000BD8 RID: 3032
	public class DigitalAlarm : MonoBehaviour
	{
		// Token: 0x0600550F RID: 21775 RVA: 0x00165E82 File Offset: 0x00164082
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06005510 RID: 21776 RVA: 0x00165EAA File Offset: 0x001640AA
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.Instance != null)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06005511 RID: 21777 RVA: 0x00165EE0 File Offset: 0x001640E0
		public void SetScreenLit(bool lit)
		{
			Material[] materials = this.ScreenMesh.materials;
			materials[this.ScreenMeshMaterialIndex] = (lit ? this.ScreenOnMat : this.ScreenOffMat);
			this.ScreenMesh.materials = materials;
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x00165F1E File Offset: 0x0016411E
		public void DisplayText(string text)
		{
			this.ScreenText.text = text;
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x00165F2C File Offset: 0x0016412C
		public void DisplayMinutes(int mins)
		{
			int num = mins / 60;
			mins %= 60;
			this.DisplayText(string.Format("{0:D2}:{1:D2}", num, mins));
		}

		// Token: 0x06005514 RID: 21780 RVA: 0x00165F60 File Offset: 0x00164160
		private void MinPass()
		{
			if (this.DisplayCurrentTime)
			{
				this.DisplayText(TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, false));
			}
		}

		// Token: 0x06005515 RID: 21781 RVA: 0x00165F84 File Offset: 0x00164184
		private void FixedUpdate()
		{
			if (this.FlashScreen)
			{
				float num = Mathf.Sin(Time.timeSinceLevelLoad * 4f);
				this.SetScreenLit(num > 0f);
			}
		}

		// Token: 0x04003F10 RID: 16144
		public const float FLASH_FREQUENCY = 4f;

		// Token: 0x04003F11 RID: 16145
		public MeshRenderer ScreenMesh;

		// Token: 0x04003F12 RID: 16146
		public int ScreenMeshMaterialIndex;

		// Token: 0x04003F13 RID: 16147
		public TextMeshPro ScreenText;

		// Token: 0x04003F14 RID: 16148
		public bool FlashScreen;

		// Token: 0x04003F15 RID: 16149
		[Header("Settings")]
		public bool DisplayCurrentTime;

		// Token: 0x04003F16 RID: 16150
		public Material ScreenOffMat;

		// Token: 0x04003F17 RID: 16151
		public Material ScreenOnMat;
	}
}
