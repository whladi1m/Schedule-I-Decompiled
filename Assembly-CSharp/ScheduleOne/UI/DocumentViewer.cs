using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x020009C1 RID: 2497
	public class DocumentViewer : Singleton<DocumentViewer>
	{
		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06004384 RID: 17284 RVA: 0x0011B45F File Offset: 0x0011965F
		// (set) Token: 0x06004385 RID: 17285 RVA: 0x0011B467 File Offset: 0x00119667
		public bool IsOpen { get; protected set; }

		// Token: 0x06004386 RID: 17286 RVA: 0x0011B470 File Offset: 0x00119670
		protected override void Start()
		{
			base.Start();
			this.IsOpen = false;
			this.Canvas.enabled = false;
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 15);
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x0011B49E File Offset: 0x0011969E
		private void Exit(ExitAction action)
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
				this.Close();
			}
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x0011B4C8 File Offset: 0x001196C8
		public void Open(string documentName)
		{
			this.IsOpen = true;
			for (int i = 0; i < this.Documents.Length; i++)
			{
				this.Documents[i].gameObject.SetActive(this.Documents[i].name == documentName);
			}
			this.Canvas.enabled = true;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			Singleton<HUD>.Instance.canvas.enabled = false;
			if (this.onOpen != null)
			{
				this.onOpen.Invoke();
			}
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x0011B5A0 File Offset: 0x001197A0
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			Singleton<HUD>.Instance.canvas.enabled = true;
		}

		// Token: 0x04003164 RID: 12644
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003165 RID: 12645
		public RectTransform[] Documents;

		// Token: 0x04003166 RID: 12646
		public UnityEvent onOpen;
	}
}
