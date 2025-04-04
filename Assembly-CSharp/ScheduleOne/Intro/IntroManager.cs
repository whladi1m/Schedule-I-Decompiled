using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EasyButtons;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.Intro
{
	// Token: 0x020005FF RID: 1535
	public class IntroManager : Singleton<IntroManager>
	{
		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x0600286C RID: 10348 RVA: 0x000A683D File Offset: 0x000A4A3D
		// (set) Token: 0x0600286D RID: 10349 RVA: 0x000A6845 File Offset: 0x000A4A45
		public bool IsPlaying { get; protected set; }

		// Token: 0x0600286E RID: 10350 RVA: 0x000A684E File Offset: 0x000A4A4E
		protected override void Awake()
		{
			base.Awake();
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000A6868 File Offset: 0x000A4A68
		private void Update()
		{
			if (this.Anim.isPlaying)
			{
				if ((GameInput.GetButton(GameInput.ButtonCode.Jump) || GameInput.GetButton(GameInput.ButtonCode.Submit) || GameInput.GetButton(GameInput.ButtonCode.PrimaryClick)) && this.depressed)
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

		// Token: 0x06002870 RID: 10352 RVA: 0x000A6994 File Offset: 0x000A4B94
		[Button]
		public void Play()
		{
			this.IsPlaying = true;
			NetworkSingleton<TimeManager>.Instance.SetTimeOverridden(true, this.TimeOfDayOverride);
			Console.Log("Starting Intro...", null);
			this.Container.SetActive(true);
			this.rv.ModelContainer.gameObject.SetActive(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<HUD>.Instance.canvas.enabled = false;
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraContainer.position, this.CameraContainer.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.CameraContainer.transform.SetParent(this.CameraContainer);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			GameObject[] disableDuringIntro = this.DisableDuringIntro;
			for (int i = 0; i < disableDuringIntro.Length; i++)
			{
				disableDuringIntro[i].gameObject.SetActive(false);
			}
			base.StartCoroutine(this.<Play>g__Wait|23_0());
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000A6AAA File Offset: 0x000A4CAA
		private void PlayMusic()
		{
			Singleton<MusicPlayer>.Instance.Tracks.Find((MusicTrack t) => t.TrackName == this.MusicName).GetComponent<MusicTrack>().Enable();
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000A6AD4 File Offset: 0x000A4CD4
		public void CharacterCreationDone(BasicAvatarSettings avatar, List<ClothingInstance> clothes)
		{
			IntroManager.<>c__DisplayClass25_0 CS$<>8__locals1 = new IntroManager.<>c__DisplayClass25_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.clothes = clothes;
			base.StartCoroutine(CS$<>8__locals1.<CharacterCreationDone>g__Wait|0());
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000A6B02 File Offset: 0x000A4D02
		public void PassedStep(int stepIndex)
		{
			this.CurrentStep = stepIndex;
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000A6B25 File Offset: 0x000A4D25
		[CompilerGenerated]
		private IEnumerator <Play>g__Wait|23_0()
		{
			yield return new WaitUntil(() => Singleton<LoadManager>.Instance.IsGameLoaded);
			this.Anim.Play();
			this.PlayMusic();
			yield return new WaitForSeconds(0.1f);
			yield return new WaitUntil(() => !this.Anim.isPlaying);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, false, false);
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(2f);
			Singleton<CharacterCreator>.Instance.Open(Singleton<CharacterCreator>.Instance.DefaultSettings, true);
			Singleton<CharacterCreator>.Instance.onCompleteWithClothing.AddListener(new UnityAction<BasicAvatarSettings, List<ClothingInstance>>(this.CharacterCreationDone));
			yield return new WaitForSeconds(0.05f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.Container.gameObject.SetActive(false);
			this.rv.ModelContainer.gameObject.SetActive(true);
			PlayerSingleton<PlayerMovement>.Instance.Teleport(NetworkSingleton<GameManager>.Instance.SpawnPoint.position);
			base.transform.forward = NetworkSingleton<GameManager>.Instance.SpawnPoint.forward;
			GameObject[] disableDuringIntro = this.DisableDuringIntro;
			for (int i = 0; i < disableDuringIntro.Length; i++)
			{
				disableDuringIntro[i].gameObject.SetActive(true);
			}
			yield return new WaitForSeconds(1f);
			Singleton<BlackOverlay>.Instance.Close(1f);
			yield break;
		}

		// Token: 0x04001D89 RID: 7561
		public const float SKIP_TIME = 0.5f;

		// Token: 0x04001D8B RID: 7563
		public int CurrentStep;

		// Token: 0x04001D8C RID: 7564
		[Header("Settings")]
		public int TimeOfDayOverride = 2000;

		// Token: 0x04001D8D RID: 7565
		[Header("References")]
		public GameObject Container;

		// Token: 0x04001D8E RID: 7566
		public Transform PlayerInitialPosition;

		// Token: 0x04001D8F RID: 7567
		public Transform PlayerInitialPosition_AfterRVExplosion;

		// Token: 0x04001D90 RID: 7568
		public Transform CameraContainer;

		// Token: 0x04001D91 RID: 7569
		public Animation Anim;

		// Token: 0x04001D92 RID: 7570
		public GameObject SkipContainer;

		// Token: 0x04001D93 RID: 7571
		public Image SkipDial;

		// Token: 0x04001D94 RID: 7572
		public GameObject[] DisableDuringIntro;

		// Token: 0x04001D95 RID: 7573
		public RV rv;

		// Token: 0x04001D96 RID: 7574
		public UnityEvent onIntroDone;

		// Token: 0x04001D97 RID: 7575
		public UnityEvent onIntroDoneAsServer;

		// Token: 0x04001D98 RID: 7576
		public string MusicName;

		// Token: 0x04001D99 RID: 7577
		private float currentSkipTime;

		// Token: 0x04001D9A RID: 7578
		private bool depressed = true;
	}
}
