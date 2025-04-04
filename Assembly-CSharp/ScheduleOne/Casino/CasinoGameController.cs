using System;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Compass;
using UnityEngine;

namespace ScheduleOne.Casino
{
	// Token: 0x02000747 RID: 1863
	public class CasinoGameController : NetworkBehaviour
	{
		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x0600328E RID: 12942 RVA: 0x000D2F54 File Offset: 0x000D1154
		// (set) Token: 0x0600328F RID: 12943 RVA: 0x000D2F5C File Offset: 0x000D115C
		public bool IsOpen { get; private set; }

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06003290 RID: 12944 RVA: 0x000D2F65 File Offset: 0x000D1165
		public CasinoGamePlayerData LocalPlayerData
		{
			get
			{
				return this.Players.GetPlayerData();
			}
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x000D2F72 File Offset: 0x000D1172
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.CasinoGameController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x000D2F86 File Offset: 0x000D1186
		protected virtual void OnLocalPlayerRequestJoin(Player player)
		{
			this.Open();
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x000D2F8E File Offset: 0x000D118E
		protected virtual void Exit(ExitAction action)
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
				this.Close();
				action.used = true;
			}
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Update()
		{
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void FixedUpdate()
		{
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x000D2FB8 File Offset: 0x000D11B8
		protected virtual void Open()
		{
			this.IsOpen = true;
			this.Players.AddPlayer(Player.Local);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.localDefaultCameraTransform = this.DefaultCameraTransforms[this.Players.GetPlayerIndex(Player.Local)];
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.localDefaultCameraTransform.position, this.localDefaultCameraTransform.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			Singleton<CompassManager>.Instance.SetVisible(false);
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x000D307C File Offset: 0x000D127C
		protected virtual void Close()
		{
			this.IsOpen = false;
			this.Players.RemovePlayer(Player.Local);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			Singleton<CompassManager>.Instance.SetVisible(true);
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x000D30FB File Offset: 0x000D12FB
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.CasinoGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.CasinoGameControllerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x000D310E File Offset: 0x000D130E
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.CasinoGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.CasinoGameControllerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x000D3121 File Offset: 0x000D1321
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x000D312F File Offset: 0x000D132F
		protected virtual void dll()
		{
			CasinoGameInteraction interaction = this.Interaction;
			interaction.onLocalPlayerRequestJoin = (Action<Player>)Delegate.Combine(interaction.onLocalPlayerRequestJoin, new Action<Player>(this.OnLocalPlayerRequestJoin));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
		}

		// Token: 0x04002441 RID: 9281
		public const float FOV = 65f;

		// Token: 0x04002442 RID: 9282
		public const float CAMERA_LERP_TIME = 0.2f;

		// Token: 0x04002444 RID: 9284
		[Header("References")]
		public CasinoGamePlayers Players;

		// Token: 0x04002445 RID: 9285
		public CasinoGameInteraction Interaction;

		// Token: 0x04002446 RID: 9286
		public Transform[] DefaultCameraTransforms;

		// Token: 0x04002447 RID: 9287
		protected Transform localDefaultCameraTransform;

		// Token: 0x04002448 RID: 9288
		private bool dll_Excuted;

		// Token: 0x04002449 RID: 9289
		private bool dll_Excuted;
	}
}
