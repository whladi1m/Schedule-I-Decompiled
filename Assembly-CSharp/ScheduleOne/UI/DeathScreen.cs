using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.Law;
using ScheduleOne.Map;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A01 RID: 2561
	public class DeathScreen : Singleton<DeathScreen>
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06004524 RID: 17700 RVA: 0x00121E19 File Offset: 0x00120019
		// (set) Token: 0x06004525 RID: 17701 RVA: 0x00121E21 File Offset: 0x00120021
		public bool isOpen { get; protected set; }

		// Token: 0x06004526 RID: 17702 RVA: 0x00121E2C File Offset: 0x0012002C
		protected override void Awake()
		{
			base.Awake();
			this.respawnButton.onClick.AddListener(new UnityAction(this.RespawnClicked));
			this.loadSaveButton.onClick.AddListener(new UnityAction(this.LoadSaveClicked));
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.group.alpha = 0f;
			this.group.interactable = false;
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x00121EB0 File Offset: 0x001200B0
		private void RespawnClicked()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.isOpen = false;
			base.StartCoroutine(this.<RespawnClicked>g__Routine|13_0());
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x00121ECF File Offset: 0x001200CF
		private void LoadSaveClicked()
		{
			this.Close();
			Singleton<LoadManager>.Instance.ExitToMenu(Singleton<LoadManager>.Instance.ActiveSaveInfo, null, false);
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x00121EF0 File Offset: 0x001200F0
		public void Open()
		{
			if (this.isOpen)
			{
				return;
			}
			this.isOpen = true;
			this.arrested = Player.Local.IsArrested;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.Sound.Play();
			this.respawnButton.gameObject.SetActive(this.CanRespawn());
			this.loadSaveButton.gameObject.SetActive(!this.respawnButton.gameObject.activeSelf);
			base.StartCoroutine(this.<Open>g__Routine|15_0());
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x00121F7E File Offset: 0x0012017E
		private bool CanRespawn()
		{
			return Player.PlayerList.Count > 1;
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x00121F90 File Offset: 0x00120190
		public void Close()
		{
			this.isOpen = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			Singleton<PostProcessingManager>.Instance.SetBlur(0f);
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x00121FE8 File Offset: 0x001201E8
		[CompilerGenerated]
		private IEnumerator <RespawnClicked>g__Routine|13_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.5f);
			this.Close();
			Singleton<HospitalBillScreen>.Instance.Open();
			Transform transform = Singleton<Map>.Instance.MedicalCentre.RespawnPoint;
			if (NetworkSingleton<CurfewManager>.Instance.IsCurrentlyActive && (Player.Local.LastVisitedProperty != null || Property.OwnedProperties.Count > 0))
			{
				if (Player.Local.LastVisitedProperty != null)
				{
					transform = Player.Local.LastVisitedProperty.InteriorSpawnPoint;
				}
				else
				{
					transform = Property.OwnedProperties[0].InteriorSpawnPoint;
				}
			}
			Player.Local.Health.SendRevive(transform.position + Vector3.up * 1f, transform.rotation);
			if (this.arrested)
			{
				Singleton<ArrestNoticeScreen>.Instance.RecordCrimes();
				Player.Local.Free();
			}
			yield return new WaitForSeconds(2f);
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			yield break;
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x00121FF7 File Offset: 0x001201F7
		[CompilerGenerated]
		private IEnumerator <Open>g__Routine|15_0()
		{
			yield return new WaitForSeconds(0.55f);
			this.Anim.Play();
			this.canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			float lerpTime = 0.75f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				Singleton<PostProcessingManager>.Instance.SetBlur(i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			Singleton<PostProcessingManager>.Instance.SetBlur(1f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			this.group.interactable = true;
			yield break;
		}

		// Token: 0x04003306 RID: 13062
		[Header("References")]
		public Canvas canvas;

		// Token: 0x04003307 RID: 13063
		public RectTransform Container;

		// Token: 0x04003308 RID: 13064
		public CanvasGroup group;

		// Token: 0x04003309 RID: 13065
		public Button respawnButton;

		// Token: 0x0400330A RID: 13066
		public Button loadSaveButton;

		// Token: 0x0400330B RID: 13067
		public Animation Anim;

		// Token: 0x0400330C RID: 13068
		public AudioSourceController Sound;

		// Token: 0x0400330D RID: 13069
		private bool arrested;
	}
}
