using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.UI.Input;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x02000576 RID: 1398
	public class ManagementMode : Singleton<ManagementMode>
	{
		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x060022FD RID: 8957 RVA: 0x0008F633 File Offset: 0x0008D833
		// (set) Token: 0x060022FE RID: 8958 RVA: 0x0008F63B File Offset: 0x0008D83B
		public Property CurrentProperty { get; private set; }

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060022FF RID: 8959 RVA: 0x0008F644 File Offset: 0x0008D844
		public bool isActive
		{
			get
			{
				return this.CurrentProperty != null;
			}
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x0008F652 File Offset: 0x0008D852
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
			this.Canvas.enabled = false;
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x0008F678 File Offset: 0x0008D878
		private void Update()
		{
			this.UpdateInput();
			if (this.isActive && Player.Local.CurrentProperty != this.CurrentProperty)
			{
				this.ExitManagementMode();
			}
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x0008F6A8 File Offset: 0x0008D8A8
		private void UpdateInput()
		{
			if (!Singleton<GameInput>.InstanceExists)
			{
				return;
			}
			this.ManagementModeInputPrompt.enabled = (this.isActive ? ManagementMode.CanExitManagementMode() : ManagementMode.CanEnterManagementMode());
			this.ManagementModeInputPrompt.Label = (this.isActive ? "Exit Management Mode" : "Enter Management Mode");
			if (GameInput.GetButtonDown(GameInput.ButtonCode.ManagementMode))
			{
				if (this.CurrentProperty != null)
				{
					this.ExitManagementMode();
					return;
				}
				if (Player.Local.CurrentProperty != null && Player.Local.CurrentProperty.IsOwned)
				{
					this.EnterManagementMode(Player.Local.CurrentProperty);
				}
			}
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x0008F74C File Offset: 0x0008D94C
		private void Exit(ExitAction exitAction)
		{
			if (!this.isActive)
			{
				return;
			}
			if (exitAction.used)
			{
				return;
			}
			if (exitAction.exitType == ExitType.Escape)
			{
				this.ExitManagementMode();
				exitAction.used = true;
			}
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x0008F778 File Offset: 0x0008D978
		public void EnterManagementMode(Property property)
		{
			this.CurrentProperty = property;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.Canvas.enabled = true;
			if (this.OnEnterManagementMode != null)
			{
				this.OnEnterManagementMode.Invoke();
			}
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x0008F7C8 File Offset: 0x0008D9C8
		public void ExitManagementMode()
		{
			this.CurrentProperty = null;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.Canvas.enabled = false;
			if (this.onExitManagementMode != null)
			{
				this.onExitManagementMode.Invoke();
			}
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x0008F816 File Offset: 0x0008DA16
		public static bool CanEnterManagementMode()
		{
			return !(Player.Local.CurrentProperty == null) && PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount <= 0;
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x000022C9 File Offset: 0x000004C9
		public static bool CanExitManagementMode()
		{
			return true;
		}

		// Token: 0x04001A39 RID: 6713
		[Header("References")]
		public InputPrompt ManagementModeInputPrompt;

		// Token: 0x04001A3A RID: 6714
		[Header("UI References")]
		public Canvas Canvas;

		// Token: 0x04001A3B RID: 6715
		public UnityEvent OnEnterManagementMode;

		// Token: 0x04001A3C RID: 6716
		public UnityEvent onExitManagementMode;
	}
}
