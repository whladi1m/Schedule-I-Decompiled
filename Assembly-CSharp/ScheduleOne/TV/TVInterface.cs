using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.UI.Compass;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x020002A6 RID: 678
	public class TVInterface : MonoBehaviour
	{
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0003F420 File Offset: 0x0003D620
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x0003F428 File Offset: 0x0003D628
		public bool IsOpen { get; private set; }

		// Token: 0x06000E30 RID: 3632 RVA: 0x0003F431 File Offset: 0x0003D631
		public void Awake()
		{
			this.Canvas.enabled = false;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0003F465 File Offset: 0x0003D665
		public void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 2);
			this.MinPass();
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0003F47F File Offset: 0x0003D67F
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0003F4B0 File Offset: 0x0003D6B0
		private void MinPass()
		{
			this.TimeLabel.text = TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
			this.Daylabel.text = NetworkSingleton<TimeManager>.Instance.CurrentDay.ToString();
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0003F4FC File Offset: 0x0003D6FC
		public void Open()
		{
			if (this.IsOpen)
			{
				return;
			}
			this.IsOpen = true;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0.15f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.15f);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			Singleton<CompassManager>.Instance.SetVisible(false);
			this.AddPlayer(Player.Local);
			this.Canvas.enabled = true;
			this.TimeLabel.gameObject.SetActive(false);
			this.HomeScreen.Open();
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0003F5D8 File Offset: 0x0003D7D8
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.15f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.15f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			this.RemovePlayer(Player.Local);
			this.Canvas.enabled = false;
			this.TimeLabel.gameObject.SetActive(true);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			Singleton<CompassManager>.Instance.SetVisible(true);
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0003F683 File Offset: 0x0003D883
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
			action.used = true;
			this.Close();
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0003F6A4 File Offset: 0x0003D8A4
		public bool CanOpen()
		{
			return !this.IsOpen;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0003F6AF File Offset: 0x0003D8AF
		public void AddPlayer(Player player)
		{
			if (!this.Players.Contains(player))
			{
				this.Players.Add(player);
				if (this.onPlayerAdded != null)
				{
					this.onPlayerAdded.Invoke(player);
				}
			}
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0003F6DF File Offset: 0x0003D8DF
		public void RemovePlayer(Player player)
		{
			if (this.Players.Contains(player))
			{
				this.Players.Remove(player);
				if (this.onPlayerRemoved != null)
				{
					this.onPlayerRemoved.Invoke(player);
				}
			}
		}

		// Token: 0x04000ED9 RID: 3801
		public const float OPEN_TIME = 0.15f;

		// Token: 0x04000EDA RID: 3802
		public const float FOV = 60f;

		// Token: 0x04000EDC RID: 3804
		public List<Player> Players = new List<Player>();

		// Token: 0x04000EDD RID: 3805
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04000EDE RID: 3806
		public Transform CameraPosition;

		// Token: 0x04000EDF RID: 3807
		public TVHomeScreen HomeScreen;

		// Token: 0x04000EE0 RID: 3808
		public TextMeshPro TimeLabel;

		// Token: 0x04000EE1 RID: 3809
		public TextMeshPro Daylabel;

		// Token: 0x04000EE2 RID: 3810
		public UnityEvent<Player> onPlayerAdded = new UnityEvent<Player>();

		// Token: 0x04000EE3 RID: 3811
		public UnityEvent<Player> onPlayerRemoved = new UnityEvent<Player>();
	}
}
