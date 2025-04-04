using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ScheduleOne.UI
{
	// Token: 0x020009AF RID: 2479
	public class ConsoleUI : MonoBehaviour
	{
		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060042EA RID: 17130 RVA: 0x001185A5 File Offset: 0x001167A5
		public bool IS_CONSOLE_ENABLED
		{
			get
			{
				return (NetworkSingleton<GameManager>.Instance.Settings.ConsoleEnabled && InstanceFinder.IsServer) || Application.isEditor;
			}
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x001185C8 File Offset: 0x001167C8
		private void Awake()
		{
			this.InputField.onSubmit.AddListener(new UnityAction<string>(this.Submit));
			this.Container.gameObject.SetActive(false);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x00118614 File Offset: 0x00116814
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.BackQuote) && !Singleton<PauseMenu>.Instance.IsPaused && this.IS_CONSOLE_ENABLED)
			{
				this.SetIsOpen(!this.canvas.enabled);
			}
			if (!this.canvas.enabled)
			{
				return;
			}
			if (!Player.Local.Health.IsAlive)
			{
				this.SetIsOpen(false);
			}
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x00118678 File Offset: 0x00116878
		private void Exit(ExitAction exitAction)
		{
			if (this.canvas == null)
			{
				return;
			}
			if (!this.canvas.enabled)
			{
				return;
			}
			if (exitAction.used)
			{
				return;
			}
			if (exitAction.exitType == ExitType.Escape)
			{
				exitAction.used = true;
				this.SetIsOpen(false);
			}
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x001186B8 File Offset: 0x001168B8
		public void SetIsOpen(bool open)
		{
			if (!InstanceFinder.IsHost && InstanceFinder.NetworkManager != null && !Application.isEditor && !Debug.isDebugBuild)
			{
				return;
			}
			this.canvas.enabled = open;
			this.Container.gameObject.SetActive(open);
			this.InputField.SetTextWithoutNotify("");
			if (open)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				GameInput.IsTyping = true;
				base.StartCoroutine(this.<SetIsOpen>g__Routine|8_0());
				return;
			}
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			GameInput.IsTyping = false;
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x00118752 File Offset: 0x00116952
		public void Submit(string val)
		{
			if (!this.canvas.enabled)
			{
				return;
			}
			Console.SubmitCommand(val);
			this.SetIsOpen(false);
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x0011876F File Offset: 0x0011696F
		[CompilerGenerated]
		private IEnumerator <SetIsOpen>g__Routine|8_0()
		{
			yield return null;
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(this.InputField.gameObject);
			yield break;
		}

		// Token: 0x040030DB RID: 12507
		[Header("References")]
		public Canvas canvas;

		// Token: 0x040030DC RID: 12508
		public TMP_InputField InputField;

		// Token: 0x040030DD RID: 12509
		public GameObject Container;
	}
}
