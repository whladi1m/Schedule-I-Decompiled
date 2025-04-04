using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property.Utilities.Water;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000345 RID: 837
	public class FillWateringCan : Task
	{
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x000525C2 File Offset: 0x000507C2
		// (set) Token: 0x060012CB RID: 4811 RVA: 0x000525CA File Offset: 0x000507CA
		public new string TaskName { get; protected set; } = "Fill watering can";

		// Token: 0x060012CC RID: 4812 RVA: 0x000525D4 File Offset: 0x000507D4
		public FillWateringCan(Tap _tap, WateringCanInstance _instance)
		{
			this.tap = _tap;
			this.instance = _instance;
			this.ClickDetectionEnabled = true;
			this.tap.SetPlayerUser(Player.Local.NetworkObject);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.tap.CameraPos.position, this.tap.CameraPos.rotation, 0.25f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.25f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			base.CurrentInstruction = "Click and hold tap to refill watering can";
			this.visuals = this.tap.CreateWateringCanModel_Local(this.instance.ID, true).GetComponent<WateringCanVisuals>();
			this.visuals.SetFillLevel(this.instance.CurrentFillAmount / 15f);
			this.visuals.FillSound.VolumeMultiplier = 0f;
			this.tap.SendWateringCanModel(this.instance.ID);
			this.tap.HandleClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.HandleClickStart));
			this.tap.HandleClickable.onClickEnd.AddListener(new UnityAction(this.HandleClickEnd));
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00052738 File Offset: 0x00050938
		public override void Update()
		{
			base.Update();
			if (this.tap.ActualFlowRate > 0f)
			{
				this.instance.ChangeFillAmount(this.tap.ActualFlowRate * Time.deltaTime);
				if (!this.visuals.FillSound.isPlaying && !this.audioPlayed)
				{
					this.visuals.FillSound.Play();
					this.audioPlayed = true;
				}
				this.visuals.FillSound.VolumeMultiplier = Mathf.MoveTowards(this.visuals.FillSound.VolumeMultiplier, 1f, Time.deltaTime * 4f);
			}
			else
			{
				this.audioPlayed = false;
				if (this.visuals.FillSound.isPlaying)
				{
					this.visuals.FillSound.VolumeMultiplier = Mathf.MoveTowards(this.visuals.FillSound.VolumeMultiplier, 0f, Time.deltaTime * 4f);
					if (this.visuals.FillSound.VolumeMultiplier <= 0f)
					{
						this.visuals.FillSound.Stop();
					}
				}
			}
			this.visuals.SetFillLevel(this.instance.CurrentFillAmount / 15f);
			if (this.instance.CurrentFillAmount >= 15f)
			{
				this.Success();
				return;
			}
			if (this.tap.ActualFlowRate > 0f && this.instance.CurrentFillAmount >= 15f)
			{
				this.visuals.SetOverflowParticles(true);
				return;
			}
			this.visuals.SetOverflowParticles(false);
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x000528CC File Offset: 0x00050ACC
		public override void StopTask()
		{
			this.tap.SetHeldOpen(false);
			this.tap.SetPlayerUser(null);
			this.tap.SendClearWateringCanModelModel();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.25f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.25f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			UnityEngine.Object.Destroy(this.visuals.gameObject);
			base.StopTask();
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00052952 File Offset: 0x00050B52
		private void HandleClickStart(RaycastHit hit)
		{
			this.tap.SetHeldOpen(true);
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00052960 File Offset: 0x00050B60
		private void HandleClickEnd()
		{
			this.tap.SetHeldOpen(false);
		}

		// Token: 0x04001226 RID: 4646
		protected Tap tap;

		// Token: 0x04001227 RID: 4647
		protected WateringCanInstance instance;

		// Token: 0x04001228 RID: 4648
		protected WateringCanVisuals visuals;

		// Token: 0x04001229 RID: 4649
		private bool audioPlayed;
	}
}
