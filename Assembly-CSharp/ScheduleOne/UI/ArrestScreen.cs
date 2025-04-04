using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009FC RID: 2556
	public class ArrestScreen : Singleton<ArrestScreen>
	{
		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06004504 RID: 17668 RVA: 0x00121834 File Offset: 0x0011FA34
		// (set) Token: 0x06004505 RID: 17669 RVA: 0x0012183C File Offset: 0x0011FA3C
		public bool isOpen { get; protected set; }

		// Token: 0x06004506 RID: 17670 RVA: 0x00121845 File Offset: 0x0011FA45
		protected override void Awake()
		{
			base.Awake();
			this.canvas.enabled = false;
			this.group.alpha = 0f;
			this.group.interactable = false;
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x00121875 File Offset: 0x0011FA75
		private void Continue()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.isOpen = false;
			base.StartCoroutine(this.<Continue>g__Routine|9_0());
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x00121894 File Offset: 0x0011FA94
		private void LoadSaveClicked()
		{
			this.Close();
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x0012189C File Offset: 0x0011FA9C
		public void Open()
		{
			if (this.isOpen)
			{
				return;
			}
			this.isOpen = true;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.Sound.Play();
			base.StartCoroutine(this.<Open>g__Routine|11_0());
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x001218D6 File Offset: 0x0011FAD6
		public void Close()
		{
			this.isOpen = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			Singleton<PostProcessingManager>.Instance.SetBlur(0f);
			this.canvas.enabled = false;
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x00121912 File Offset: 0x0011FB12
		[CompilerGenerated]
		private IEnumerator <Continue>g__Routine|9_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.5f);
			this.Close();
			Singleton<ArrestNoticeScreen>.Instance.Open();
			Player.Local.Free();
			Player.Local.Health.SetHealth(100f);
			yield return new WaitForSeconds(2f);
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			yield break;
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x00121921 File Offset: 0x0011FB21
		[CompilerGenerated]
		private IEnumerator <Open>g__Routine|11_0()
		{
			yield return new WaitForSeconds(0.5f);
			this.Anim.Play();
			this.canvas.enabled = true;
			float lerpTime = 0.75f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				Singleton<PostProcessingManager>.Instance.SetBlur(i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			Singleton<PostProcessingManager>.Instance.SetBlur(1f);
			yield return new WaitForSeconds(3f);
			this.Continue();
			yield break;
		}

		// Token: 0x040032E8 RID: 13032
		[Header("References")]
		public Canvas canvas;

		// Token: 0x040032E9 RID: 13033
		public CanvasGroup group;

		// Token: 0x040032EA RID: 13034
		public AudioSourceController Sound;

		// Token: 0x040032EB RID: 13035
		public Animation Anim;
	}
}
