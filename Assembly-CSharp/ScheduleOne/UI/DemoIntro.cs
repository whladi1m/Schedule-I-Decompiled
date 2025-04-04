using System;
using System.Collections;
using System.Runtime.CompilerServices;
using EasyButtons;
using FishNet;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009BB RID: 2491
	public class DemoIntro : Singleton<DemoIntro>
	{
		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06004356 RID: 17238 RVA: 0x0011A3B5 File Offset: 0x001185B5
		// (set) Token: 0x06004357 RID: 17239 RVA: 0x0011A3BD File Offset: 0x001185BD
		public bool IsPlaying { get; protected set; }

		// Token: 0x06004358 RID: 17240 RVA: 0x0011A3C8 File Offset: 0x001185C8
		private void Update()
		{
			if (this.waitingForCutsceneEnd && !this.Anim.isPlaying)
			{
				this.CutsceneDone();
			}
			if (this.Anim.isPlaying)
			{
				if ((GameInput.GetButton(GameInput.ButtonCode.Jump) || GameInput.GetButton(GameInput.ButtonCode.Submit) || GameInput.GetButton(GameInput.ButtonCode.PrimaryClick)) && this.depressed && this.CurrentStep < this.SkipEvents - 1)
				{
					this.currentSkipTime += Time.deltaTime;
					if (this.currentSkipTime >= 0.5f)
					{
						this.currentSkipTime = 0f;
						if (this.IsPlaying)
						{
							Debug.Log("Skipping!");
							int num = this.CurrentStep + 1;
							float time = this.Anim.clip.events[num].time;
							this.Anim[this.Anim.clip.name].time = time;
							this.CurrentStep = num;
							this.depressed = false;
						}
					}
					this.SkipDial.fillAmount = this.currentSkipTime / 0.5f;
					this.SkipContainer.SetActive(true);
					return;
				}
				this.currentSkipTime = 0f;
				this.SkipContainer.SetActive(false);
				if (!GameInput.GetButton(GameInput.ButtonCode.Jump) && !GameInput.GetButton(GameInput.ButtonCode.Submit) && !GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
				{
					this.depressed = true;
				}
			}
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x0011A524 File Offset: 0x00118724
		[Button]
		public void Play()
		{
			this.IsPlaying = true;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<HUD>.Instance.canvas.enabled = false;
			this.Anim.Play();
			base.Invoke("PlayMusic", 1f);
			if (this.onStart != null)
			{
				this.onStart.Invoke();
			}
			this.waitingForCutsceneEnd = true;
			if (InstanceFinder.IsServer && this.onStartAsServer != null)
			{
				this.onStartAsServer.Invoke();
			}
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x0011A5C9 File Offset: 0x001187C9
		private void PlayMusic()
		{
			Singleton<MusicPlayer>.Instance.Tracks.Find((MusicTrack t) => t.TrackName == this.MusicName).GetComponent<AmbientTrack>().ForcePlay();
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x0011A5F0 File Offset: 0x001187F0
		public void ShowAvatar()
		{
			Singleton<CharacterCreator>.Instance.Open(Singleton<CharacterCreator>.Instance.DefaultSettings, false);
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x0011A608 File Offset: 0x00118808
		public void CutsceneDone()
		{
			this.waitingForCutsceneEnd = false;
			Singleton<CharacterCreator>.Instance.ShowUI();
			Singleton<CharacterCreator>.Instance.onComplete.AddListener(new UnityAction<BasicAvatarSettings>(this.CharacterCreationDone));
			if (this.onCutsceneDone != null)
			{
				this.onCutsceneDone.Invoke();
			}
			this.IsPlaying = false;
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x0011A65B File Offset: 0x0011885B
		public void PassedStep(int stepIndex)
		{
			this.CurrentStep = stepIndex;
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0011A664 File Offset: 0x00118864
		public void CharacterCreationDone(BasicAvatarSettings avatar)
		{
			base.StartCoroutine(this.<CharacterCreationDone>g__Wait|26_0());
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x0011A69C File Offset: 0x0011889C
		[CompilerGenerated]
		private IEnumerator <CharacterCreationDone>g__Wait|26_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.5f);
			Player.Local.transform.position = this.PlayerInitialPosition.position;
			Player.Local.transform.rotation = this.PlayerInitialPosition.rotation;
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, false, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
			Singleton<CharacterCreator>.Instance.DisableStuff();
			yield return new WaitForSeconds(0.5f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<HUD>.Instance.canvas.enabled = true;
			Singleton<BlackOverlay>.Instance.Close(1f);
			if (this.onIntroDone != null)
			{
				this.onIntroDone.Invoke();
			}
			if (InstanceFinder.IsServer)
			{
				if (this.onIntroDoneAsServer != null)
				{
					this.onIntroDoneAsServer.Invoke();
				}
				Singleton<SaveManager>.Instance.Save();
			}
			else
			{
				Player.Local.RequestSavePlayer();
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0400312B RID: 12587
		public const float SKIP_TIME = 0.5f;

		// Token: 0x0400312D RID: 12589
		public Animation Anim;

		// Token: 0x0400312E RID: 12590
		public Transform PlayerInitialPosition;

		// Token: 0x0400312F RID: 12591
		public GameObject SkipContainer;

		// Token: 0x04003130 RID: 12592
		public Image SkipDial;

		// Token: 0x04003131 RID: 12593
		public int SkipEvents = 3;

		// Token: 0x04003132 RID: 12594
		public UnityEvent onStart;

		// Token: 0x04003133 RID: 12595
		public UnityEvent onStartAsServer;

		// Token: 0x04003134 RID: 12596
		public UnityEvent onCutsceneDone;

		// Token: 0x04003135 RID: 12597
		public UnityEvent onIntroDone;

		// Token: 0x04003136 RID: 12598
		public UnityEvent onIntroDoneAsServer;

		// Token: 0x04003137 RID: 12599
		private int CurrentStep;

		// Token: 0x04003138 RID: 12600
		public string MusicName;

		// Token: 0x04003139 RID: 12601
		private float currentSkipTime;

		// Token: 0x0400313A RID: 12602
		private bool depressed = true;

		// Token: 0x0400313B RID: 12603
		private bool waitingForCutsceneEnd;
	}
}
