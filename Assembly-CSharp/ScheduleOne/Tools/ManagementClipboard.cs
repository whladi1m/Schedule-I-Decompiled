using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.UI.Management;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x02000824 RID: 2084
	public class ManagementClipboard : Singleton<ManagementClipboard>
	{
		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x0600396C RID: 14700 RVA: 0x000F2FF9 File Offset: 0x000F11F9
		// (set) Token: 0x0600396D RID: 14701 RVA: 0x000F3001 File Offset: 0x000F1201
		public bool IsOpen { get; protected set; }

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x0600396E RID: 14702 RVA: 0x000F300A File Offset: 0x000F120A
		// (set) Token: 0x0600396F RID: 14703 RVA: 0x000F3012 File Offset: 0x000F1212
		public bool StatePreserved { get; protected set; }

		// Token: 0x06003970 RID: 14704 RVA: 0x000F301C File Offset: 0x000F121C
		protected override void Awake()
		{
			base.Awake();
			this.ClipboardTransform.gameObject.SetActive(false);
			this.ClipboardTransform.localPosition = new Vector3(this.ClipboardTransform.localPosition.x, this.ClosedOffset, this.ClipboardTransform.localPosition.z);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 10);
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x000F308C File Offset: 0x000F128C
		private void Update()
		{
			for (int i = 0; i < this.CurrentConfigurables.Count; i++)
			{
				if (this.CurrentConfigurables[i].IsBeingConfiguredByOtherPlayer)
				{
					this.Close(false);
				}
			}
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x000F30C9 File Offset: 0x000F12C9
		private void Exit(ExitAction exitAction)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (exitAction.used)
			{
				return;
			}
			this.Close(false);
			exitAction.used = true;
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x000F30EC File Offset: 0x000F12EC
		public void Open(List<IConfigurable> selection, ManagementClipboard_Equippable equippable)
		{
			this.IsOpen = true;
			this.OverlayCamera.enabled = true;
			this.OverlayLight.enabled = true;
			this.ClipboardTransform.gameObject.SetActive(true);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0.06f);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0.06f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			this.SelectionInfo.Set(selection);
			this.LerpToVerticalPosition(true, null);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			Singleton<ManagementInterface>.Instance.Open(selection, equippable);
			this.CurrentConfigurables.AddRange(selection);
			for (int i = 0; i < this.CurrentConfigurables.Count; i++)
			{
				this.CurrentConfigurables[i].SetConfigurer(Player.Local.NetworkObject);
			}
			if (this.onOpened != null)
			{
				this.onOpened.Invoke();
			}
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x000F31EC File Offset: 0x000F13EC
		public void Close(bool preserveState = false)
		{
			this.IsOpen = false;
			this.StatePreserved = preserveState;
			this.OverlayLight.enabled = false;
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.06f);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			Singleton<ManagementInterface>.Instance.Close(preserveState);
			if (this.onClosed != null)
			{
				this.onClosed.Invoke();
			}
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			for (int i = 0; i < this.CurrentConfigurables.Count; i++)
			{
				if (this.CurrentConfigurables[i].CurrentPlayerConfigurer == Player.Local.NetworkObject)
				{
					this.CurrentConfigurables[i].SetConfigurer(null);
				}
			}
			this.CurrentConfigurables.Clear();
			this.LerpToVerticalPosition(false, delegate
			{
				this.<Close>g__Done|25_1();
			});
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x000F32DC File Offset: 0x000F14DC
		private void LerpToVerticalPosition(bool open, Action callback)
		{
			ManagementClipboard.<>c__DisplayClass26_0 CS$<>8__locals1 = new ManagementClipboard.<>c__DisplayClass26_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.callback = callback;
			CS$<>8__locals1.endPos = new Vector3(this.ClipboardTransform.localPosition.x, open ? 0f : this.ClosedOffset, this.ClipboardTransform.localPosition.z);
			CS$<>8__locals1.startPos = this.ClipboardTransform.localPosition;
			if (this.lerpRoutine != null)
			{
				base.StopCoroutine(this.lerpRoutine);
			}
			this.lerpRoutine = base.StartCoroutine(CS$<>8__locals1.<LerpToVerticalPosition>g__Lerp|0());
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x000F3395 File Offset: 0x000F1595
		[CompilerGenerated]
		private void <Close>g__Done|25_1()
		{
			if (!Singleton<GameplayMenu>.Instance.IsOpen)
			{
				this.ClipboardTransform.gameObject.SetActive(false);
				this.OverlayCamera.enabled = false;
			}
		}

		// Token: 0x0400296E RID: 10606
		public bool IsEquipped;

		// Token: 0x04002971 RID: 10609
		public const float OpenTime = 0.06f;

		// Token: 0x04002972 RID: 10610
		[Header("References")]
		public Transform ClipboardTransform;

		// Token: 0x04002973 RID: 10611
		public Camera OverlayCamera;

		// Token: 0x04002974 RID: 10612
		public Light OverlayLight;

		// Token: 0x04002975 RID: 10613
		public SelectionInfoUI SelectionInfo;

		// Token: 0x04002976 RID: 10614
		[Header("Settings")]
		public float ClosedOffset = -0.2f;

		// Token: 0x04002977 RID: 10615
		public UnityEvent onClipboardEquipped;

		// Token: 0x04002978 RID: 10616
		public UnityEvent onClipboardUnequipped;

		// Token: 0x04002979 RID: 10617
		public UnityEvent onOpened;

		// Token: 0x0400297A RID: 10618
		public UnityEvent onClosed;

		// Token: 0x0400297B RID: 10619
		private Coroutine lerpRoutine;

		// Token: 0x0400297C RID: 10620
		private List<IConfigurable> CurrentConfigurables = new List<IConfigurable>();
	}
}
