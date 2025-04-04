using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.AvatarFramework.Animation;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x0200046B RID: 1131
	public class NPCEvent_Sit : NPCEvent
	{
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001868 RID: 6248 RVA: 0x0006B7CA File Offset: 0x000699CA
		public new string ActionName
		{
			get
			{
				return "Sit";
			}
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x0006B7D4 File Offset: 0x000699D4
		public override string GetName()
		{
			string text = this.ActionName;
			if (this.SeatSet == null)
			{
				text += "(no seat assigned)";
			}
			return text;
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x0006B804 File Offset: 0x00069A04
		public override void Started()
		{
			base.Started();
			this.seated = false;
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			this.targetSeat = this.SeatSet.GetRandomFreeSeat();
			base.SetDestination(this.targetSeat.AccessPoint.position, true);
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x0006B858 File Offset: 0x00069A58
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (!base.IsActive)
			{
				return;
			}
			if (this.seated)
			{
				this.StartAction(connection, this.SeatSet.Seats.IndexOf(this.npc.Avatar.Anim.CurrentSeat));
			}
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x0006B8AC File Offset: 0x00069AAC
		public override void LateStarted()
		{
			base.LateStarted();
			this.seated = false;
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			this.targetSeat = this.SeatSet.GetRandomFreeSeat();
			base.SetDestination(this.targetSeat.AccessPoint.position, true);
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0006B900 File Offset: 0x00069B00
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log("ActiveMinPassed");
				Debug.Log("Moving: " + this.npc.Movement.IsMoving.ToString());
				Debug.Log("At destination: " + this.IsAtDestination().ToString());
				Debug.Log("Seated: " + this.seated.ToString());
			}
			if (!base.IsActive)
			{
				return;
			}
			if (!this.npc.Movement.IsMoving)
			{
				if (this.IsAtDestination() || this.seated)
				{
					if (!this.seated)
					{
						if (!this.npc.Movement.FaceDirectionInProgress)
						{
							this.npc.Movement.FaceDirection(this.targetSeat.SittingPoint.forward, 0.5f);
						}
						if (Vector3.Angle(this.npc.Movement.transform.forward, this.targetSeat.SittingPoint.forward) < 10f)
						{
							this.StartAction(null, this.SeatSet.Seats.IndexOf(this.SeatSet.GetRandomFreeSeat()));
							return;
						}
					}
					else if (!this.npc.Movement.FaceDirectionInProgress && Vector3.Angle(this.npc.Movement.transform.forward, this.targetSeat.SittingPoint.forward) > 15f)
					{
						this.npc.Movement.FaceDirection(this.targetSeat.SittingPoint.forward, 0.5f);
						return;
					}
				}
				else
				{
					base.SetDestination(this.targetSeat.AccessPoint.position, true);
				}
			}
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x0006BADC File Offset: 0x00069CDC
		public override void JumpTo()
		{
			base.JumpTo();
			if (!this.IsAtDestination())
			{
				if (this.npc.Movement.IsMoving)
				{
					this.npc.Movement.Stop();
				}
				this.targetSeat = this.SeatSet.GetRandomFreeSeat();
				if (InstanceFinder.IsServer)
				{
					this.npc.Movement.Warp(this.targetSeat.AccessPoint.position);
					this.StartAction(null, this.SeatSet.Seats.IndexOf(this.SeatSet.GetRandomFreeSeat()));
				}
				this.npc.Movement.FaceDirection(this.targetSeat.AccessPoint.forward, 0f);
			}
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0006BB9B File Offset: 0x00069D9B
		public override void End()
		{
			base.End();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.seated)
			{
				this.EndAction();
			}
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x0006BBB9 File Offset: 0x00069DB9
		public override void Interrupt()
		{
			base.Interrupt();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
			if (this.seated)
			{
				this.EndAction();
			}
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x0006BBF9 File Offset: 0x00069DF9
		public override void Resume()
		{
			base.Resume();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			this.targetSeat = this.SeatSet.GetRandomFreeSeat();
			base.SetDestination(this.targetSeat.AccessPoint.position, true);
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x0006BC3C File Offset: 0x00069E3C
		public override void Skipped()
		{
			base.Skipped();
			if (this.WarpIfSkipped)
			{
				this.targetSeat = this.SeatSet.GetRandomFreeSeat();
				this.npc.Movement.Warp(this.targetSeat.AccessPoint.position);
			}
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x0006BC88 File Offset: 0x00069E88
		private bool IsAtDestination()
		{
			return !(this.targetSeat == null) && Vector3.Distance(this.npc.Movement.FootPosition, this.targetSeat.AccessPoint.position) < 1.5f;
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x0006BCC8 File Offset: 0x00069EC8
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (result != NPCMovement.WalkResult.Success)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.StartAction(null, this.SeatSet.Seats.IndexOf(this.SeatSet.GetRandomFreeSeat()));
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x0006BD14 File Offset: 0x00069F14
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		protected virtual void StartAction(NetworkConnection conn, int seatIndex)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_StartAction_2681120339(conn, seatIndex);
				this.RpcLogic___StartAction_2681120339(conn, seatIndex);
			}
			else
			{
				this.RpcWriter___Target_StartAction_2681120339(conn, seatIndex);
			}
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0006BD55 File Offset: 0x00069F55
		[ObserversRpc(RunLocally = true)]
		protected virtual void EndAction()
		{
			this.RpcWriter___Observers_EndAction_2166136261();
			this.RpcLogic___EndAction_2166136261();
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0006BD6C File Offset: 0x00069F6C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_SitAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_SitAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_StartAction_2681120339));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_StartAction_2681120339));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_EndAction_2166136261));
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x0006BDD5 File Offset: 0x00069FD5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_SitAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_SitAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x0006BDEE File Offset: 0x00069FEE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x0006BDFC File Offset: 0x00069FFC
		private void RpcWriter___Observers_StartAction_2681120339(NetworkConnection conn, int seatIndex)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(seatIndex, AutoPackType.Packed);
			base.SendObserversRpc(0U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x0006BEB8 File Offset: 0x0006A0B8
		protected virtual void RpcLogic___StartAction_2681120339(NetworkConnection conn, int seatIndex)
		{
			if (this.seated)
			{
				return;
			}
			this.seated = true;
			if (seatIndex >= 0 && seatIndex < this.SeatSet.Seats.Length)
			{
				this.targetSeat = this.SeatSet.Seats[seatIndex];
			}
			else
			{
				this.targetSeat = null;
			}
			this.npc.Movement.SetSeat(this.targetSeat);
			if (this.onSeated != null)
			{
				this.onSeated.Invoke();
			}
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x0006BF30 File Offset: 0x0006A130
		private void RpcReader___Observers_StartAction_2681120339(PooledReader PooledReader0, Channel channel)
		{
			int seatIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartAction_2681120339(null, seatIndex);
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x0006BF74 File Offset: 0x0006A174
		private void RpcWriter___Target_StartAction_2681120339(NetworkConnection conn, int seatIndex)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(seatIndex, AutoPackType.Packed);
			base.SendTargetRpc(1U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x0006C030 File Offset: 0x0006A230
		private void RpcReader___Target_StartAction_2681120339(PooledReader PooledReader0, Channel channel)
		{
			int seatIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___StartAction_2681120339(base.LocalConnection, seatIndex);
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0006C06C File Offset: 0x0006A26C
		private void RpcWriter___Observers_EndAction_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x0006C115 File Offset: 0x0006A315
		protected virtual void RpcLogic___EndAction_2166136261()
		{
			if (!this.seated)
			{
				return;
			}
			this.seated = false;
			this.npc.Movement.SetSeat(null);
			if (this.onStandUp != null)
			{
				this.onStandUp.Invoke();
			}
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0006C14C File Offset: 0x0006A34C
		private void RpcReader___Observers_EndAction_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EndAction_2166136261();
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x0006C176 File Offset: 0x0006A376
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040015B3 RID: 5555
		public const float DESTINATION_THRESHOLD = 1.5f;

		// Token: 0x040015B4 RID: 5556
		public AvatarSeatSet SeatSet;

		// Token: 0x040015B5 RID: 5557
		public bool WarpIfSkipped;

		// Token: 0x040015B6 RID: 5558
		private bool seated;

		// Token: 0x040015B7 RID: 5559
		private AvatarSeat targetSeat;

		// Token: 0x040015B8 RID: 5560
		public UnityEvent onSeated;

		// Token: 0x040015B9 RID: 5561
		public UnityEvent onStandUp;

		// Token: 0x040015BA RID: 5562
		private bool dll_Excuted;

		// Token: 0x040015BB RID: 5563
		private bool dll_Excuted;
	}
}
