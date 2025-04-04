using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.Map;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.AI;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x02000472 RID: 1138
	public class NPCSignal_DriveToCarPark : NPCSignal
	{
		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x0006D10D File Offset: 0x0006B30D
		public new string ActionName
		{
			get
			{
				return "Drive to car park";
			}
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x0006D114 File Offset: 0x0006B314
		public override string GetName()
		{
			if (this.ParkingLot == null)
			{
				return this.ActionName + " (No Parking Lot)";
			}
			return this.ActionName + " (" + this.ParkingLot.gameObject.name + ")";
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x0006D165 File Offset: 0x0006B365
		protected override void OnValidate()
		{
			base.OnValidate();
			this.priority = 12;
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x0006D175 File Offset: 0x0006B375
		public override void Started()
		{
			base.Started();
			this.isAtDestination = false;
			this.CheckValidForStart();
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x0006D18A File Offset: 0x0006B38A
		public override void End()
		{
			base.End();
			if (this.npc.CurrentVehicle != null)
			{
				this.npc.ExitVehicle();
			}
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x0006D1B0 File Offset: 0x0006B3B0
		public override void LateStarted()
		{
			base.LateStarted();
			this.isAtDestination = false;
			this.CheckValidForStart();
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x0006D1C5 File Offset: 0x0006B3C5
		private void CheckValidForStart()
		{
			if (this.Vehicle.CurrentParkingLot == this.ParkingLot)
			{
				this.End();
			}
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x0006D1E8 File Offset: 0x0006B3E8
		public override void Interrupt()
		{
			base.Interrupt();
			this.Park();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.npc.IsInVehicle)
			{
				this.Vehicle.Agent.StopNavigating();
				this.npc.ExitVehicle();
				return;
			}
			this.npc.Movement.Stop();
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x0006D242 File Offset: 0x0006B442
		public override void Resume()
		{
			base.Resume();
			this.isAtDestination = false;
			this.CheckValidForStart();
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x0006D257 File Offset: 0x0006B457
		public override void Skipped()
		{
			base.Skipped();
			this.Park();
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x0006D265 File Offset: 0x0006B465
		public override void ResumeFailed()
		{
			base.ResumeFailed();
			this.Park();
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x0006D273 File Offset: 0x0006B473
		public override void JumpTo()
		{
			base.JumpTo();
			this.isAtDestination = false;
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x0006D284 File Offset: 0x0006B484
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (this.npc.IsInVehicle)
			{
				this.timeInVehicle += 1f;
			}
			else
			{
				this.timeInVehicle = 0f;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.npc.IsInVehicle && this.npc.CurrentVehicle.CurrentParkingLot == this.ParkingLot)
			{
				this.timeAtDestination += 1f;
				if (this.timeAtDestination > 1f)
				{
					this.End();
				}
			}
			else
			{
				this.timeAtDestination = 0f;
			}
			if (!this.isAtDestination)
			{
				if (this.npc.IsInVehicle)
				{
					if (this.Vehicle.isParked)
					{
						if (this.timeInVehicle > 1f)
						{
							this.Vehicle.ExitPark_Networked(null, this.Vehicle.CurrentParkingLot.UseExitPoint);
							return;
						}
					}
					else if (!this.Vehicle.Agent.AutoDriving)
					{
						this.Vehicle.Agent.Navigate(this.ParkingLot.EntryPoint.position, null, new VehicleAgent.NavigationCallback(this.DriveCallback));
						return;
					}
				}
				else if ((!this.npc.Movement.IsMoving || Vector3.Distance(this.npc.Movement.CurrentDestination, this.GetWalkDestination()) > 1f) && this.npc.Movement.CanMove())
				{
					if (this.npc.Movement.CanGetTo(this.GetWalkDestination(), 2f))
					{
						base.SetDestination(this.GetWalkDestination(), true);
						return;
					}
					this.npc.EnterVehicle(null, this.Vehicle);
					Console.LogWarning(string.Concat(new string[]
					{
						"NPC ",
						this.npc.name,
						" was unable to reach vehicle ",
						this.Vehicle.name,
						" and was teleported to it."
					}), null);
					Debug.DrawLine(this.npc.transform.position, this.GetWalkDestination(), Color.red, 10f);
				}
			}
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x0006D4AF File Offset: 0x0006B6AF
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (result == NPCMovement.WalkResult.Success || result == NPCMovement.WalkResult.Partial)
			{
				this.npc.EnterVehicle(null, this.Vehicle);
			}
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x0006D4E4 File Offset: 0x0006B6E4
		private Vector3 GetWalkDestination()
		{
			if (!this.Vehicle.IsVisible && this.Vehicle.CurrentParkingLot != null && this.Vehicle.CurrentParkingLot.HiddenVehicleAccessPoint != null)
			{
				return this.Vehicle.CurrentParkingLot.HiddenVehicleAccessPoint.position;
			}
			return this.Vehicle.driverEntryPoint.position;
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x0006D54F File Offset: 0x0006B74F
		private void DriveCallback(VehicleAgent.ENavigationResult result)
		{
			if (!base.IsActive)
			{
				return;
			}
			this.isAtDestination = true;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.Park();
			base.StartCoroutine(this.<DriveCallback>g__Wait|23_0());
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x0006D57C File Offset: 0x0006B77C
		private void Park()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			int randomFreeSpotIndex = this.ParkingLot.GetRandomFreeSpotIndex();
			EParkingAlignment alignment = EParkingAlignment.FrontToKerb;
			if (randomFreeSpotIndex != -1)
			{
				alignment = (this.OverrideParkingType ? this.ParkingType : this.ParkingLot.ParkingSpots[randomFreeSpotIndex].Alignment);
			}
			this.Vehicle.Park(null, new ParkData
			{
				lotGUID = this.ParkingLot.GUID,
				alignment = alignment,
				spotIndex = randomFreeSpotIndex
			}, true);
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x0006D5FB File Offset: 0x0006B7FB
		private EParkingAlignment GetParkingType()
		{
			if (this.OverrideParkingType)
			{
				return this.ParkingType;
			}
			return this.ParkingLot.GetRandomFreeSpot().Alignment;
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x0006D624 File Offset: 0x0006B824
		[CompilerGenerated]
		private IEnumerator <DriveCallback>g__Wait|23_0()
		{
			yield return new WaitForSeconds(1f);
			this.End();
			yield break;
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x0006D633 File Offset: 0x0006B833
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_DriveToCarParkAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_DriveToCarParkAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x0006D64C File Offset: 0x0006B84C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_DriveToCarParkAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_DriveToCarParkAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0006D665 File Offset: 0x0006B865
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0006D673 File Offset: 0x0006B873
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040015DC RID: 5596
		public ParkingLot ParkingLot;

		// Token: 0x040015DD RID: 5597
		public LandVehicle Vehicle;

		// Token: 0x040015DE RID: 5598
		[Header("Parking Settings")]
		public bool OverrideParkingType;

		// Token: 0x040015DF RID: 5599
		public EParkingAlignment ParkingType;

		// Token: 0x040015E0 RID: 5600
		private bool isAtDestination;

		// Token: 0x040015E1 RID: 5601
		private float timeInVehicle;

		// Token: 0x040015E2 RID: 5602
		private float timeAtDestination;

		// Token: 0x040015E3 RID: 5603
		private bool dll_Excuted;

		// Token: 0x040015E4 RID: 5604
		private bool dll_Excuted;
	}
}
