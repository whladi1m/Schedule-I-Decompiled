using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.DevUtilities;
using ScheduleOne.Doors;
using ScheduleOne.Map;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x0200046C RID: 1132
	public class NPCEvent_StayInBuilding : NPCEvent
	{
		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001884 RID: 6276 RVA: 0x0006C18A File Offset: 0x0006A38A
		public new string ActionName
		{
			get
			{
				return "Stay in Building";
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x0006C191 File Offset: 0x0006A391
		private bool InBuilding
		{
			get
			{
				return this.npc.CurrentBuilding == this.Building;
			}
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x0006C1A9 File Offset: 0x0006A3A9
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Schedules.NPCEvent_StayInBuilding_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x0006C1C0 File Offset: 0x0006A3C0
		public override string GetName()
		{
			if (this.Building == null)
			{
				return this.ActionName + " (No building set)";
			}
			return this.ActionName + " (" + this.Building.BuildingName + ")";
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0006C20C File Offset: 0x0006A40C
		public override void Started()
		{
			base.Started();
			if (!base.IsActive)
			{
				return;
			}
			if (this.Building == null)
			{
				return;
			}
			if (InstanceFinder.IsServer)
			{
				base.SetDestination(this.GetEntryPoint().position, true);
			}
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0006C248 File Offset: 0x0006A448
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (!base.IsActive)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.schedule.DEBUG_MODE)
			{
				Debug.Log("StayInBuilding: ActiveMinPassed");
				Debug.Log("In building: " + this.InBuilding.ToString());
				Debug.Log("Is entering: " + this.IsEntering.ToString());
			}
			if (this.Building == null || this.Building.Doors.Length == 0)
			{
				return;
			}
			if (!this.InBuilding && !this.IsEntering && (!this.npc.Movement.IsMoving || Vector3.Distance(this.npc.Movement.CurrentDestination, this.GetEntryPoint().position) > 2f))
			{
				if (Vector3.Distance(this.npc.transform.position, this.GetEntryPoint().position) < 0.5f)
				{
					this.PlayEnterAnimation();
					return;
				}
				if (this.npc.Movement.CanMove())
				{
					base.SetDestination(this.GetEntryPoint().position, true);
				}
			}
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0006C378 File Offset: 0x0006A578
		public override void LateStarted()
		{
			base.LateStarted();
			if (this.Building == null || this.Building.Doors.Length == 0)
			{
				return;
			}
			if (InstanceFinder.IsServer)
			{
				base.SetDestination(this.GetEntryPoint().position, true);
			}
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x0006C3B6 File Offset: 0x0006A5B6
		public override void JumpTo()
		{
			base.JumpTo();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
			this.PlayEnterAnimation();
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x0006C3EE File Offset: 0x0006A5EE
		public override void End()
		{
			base.End();
			this.CancelEnter();
			if (this.InBuilding)
			{
				this.ExitBuilding();
				return;
			}
			this.npc.Movement.Stop();
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x0006C41B File Offset: 0x0006A61B
		public override void Interrupt()
		{
			base.Interrupt();
			this.CancelEnter();
			if (this.InBuilding)
			{
				this.ExitBuilding();
				return;
			}
			this.npc.Movement.Stop();
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x0006C448 File Offset: 0x0006A648
		public override void Skipped()
		{
			base.Skipped();
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x0006C450 File Offset: 0x0006A650
		public override void Resume()
		{
			base.Resume();
			if (!this.InBuilding && InstanceFinder.IsServer)
			{
				base.SetDestination(this.GetEntryPoint().position, true);
			}
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x0006C479 File Offset: 0x0006A679
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
				this.PlayEnterAnimation();
			}
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x0006C4A1 File Offset: 0x0006A6A1
		[ObserversRpc(RunLocally = true)]
		private void PlayEnterAnimation()
		{
			this.RpcWriter___Observers_PlayEnterAnimation_2166136261();
			this.RpcLogic___PlayEnterAnimation_2166136261();
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x0006C4AF File Offset: 0x0006A6AF
		private void CancelEnter()
		{
			this.IsEntering = false;
			if (this.enterRoutine != null)
			{
				base.StopCoroutine(this.enterRoutine);
			}
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0006C4CC File Offset: 0x0006A6CC
		private void EnterBuilding(int doorIndex)
		{
			if (this.Building == null)
			{
				Console.LogWarning("Building is null in StayInBuilding event", null);
				return;
			}
			if (InstanceFinder.IsServer)
			{
				this.npc.EnterBuilding(null, this.Building.GUID.ToString(), doorIndex);
			}
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0006C520 File Offset: 0x0006A720
		private void ExitBuilding()
		{
			if (InstanceFinder.IsServer)
			{
				this.npc.ExitBuilding("");
			}
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x0006C53C File Offset: 0x0006A73C
		private Transform GetEntryPoint()
		{
			if (this.Door != null)
			{
				return this.Door.AccessPoint;
			}
			if (this.Building == null)
			{
				return null;
			}
			StaticDoor closestDoor = this.Building.GetClosestDoor(this.npc.Movement.FootPosition, true);
			if (closestDoor == null)
			{
				return null;
			}
			return closestDoor.AccessPoint;
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x0006C5A4 File Offset: 0x0006A7A4
		private StaticDoor GetDoor(out int doorIndex)
		{
			doorIndex = -1;
			if (this.Door != null)
			{
				return this.Door;
			}
			if (this.Building == null)
			{
				return null;
			}
			if (this.npc == null)
			{
				return null;
			}
			StaticDoor closestDoor = this.Building.GetClosestDoor(this.npc.Movement.FootPosition, true);
			doorIndex = this.Building.Doors.IndexOf(closestDoor);
			return closestDoor;
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0006C61A File Offset: 0x0006A81A
		[CompilerGenerated]
		private IEnumerator <PlayEnterAnimation>g__Enter|19_0()
		{
			this.IsEntering = true;
			yield return new WaitUntil(() => !this.npc.Movement.IsMoving);
			int doorIndex;
			StaticDoor door = this.GetDoor(out doorIndex);
			if (door != null)
			{
				Transform faceDir = door.transform;
				this.npc.Movement.FacePoint(faceDir.position, 0.5f);
				float t = 0f;
				while (Vector3.SignedAngle(this.npc.Avatar.transform.forward, faceDir.position - this.npc.Avatar.CenterPoint, Vector3.up) > 15f && t < 1f)
				{
					yield return new WaitForEndOfFrame();
					t += Time.deltaTime;
				}
				faceDir = null;
			}
			this.npc.Avatar.Anim.SetTrigger("GrabItem");
			yield return new WaitForSeconds(0.6f);
			this.IsEntering = false;
			this.enterRoutine = null;
			this.EnterBuilding(doorIndex);
			yield break;
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0006C63E File Offset: 0x0006A83E
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_StayInBuildingAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_StayInBuildingAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_PlayEnterAnimation_2166136261));
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0006C66E File Offset: 0x0006A86E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_StayInBuildingAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_StayInBuildingAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0006C687 File Offset: 0x0006A887
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0006C698 File Offset: 0x0006A898
		private void RpcWriter___Observers_PlayEnterAnimation_2166136261()
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
			base.SendObserversRpc(0U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x0006C741 File Offset: 0x0006A941
		private void RpcLogic___PlayEnterAnimation_2166136261()
		{
			if (this.IsEntering)
			{
				return;
			}
			this.enterRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<PlayEnterAnimation>g__Enter|19_0());
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x0006C764 File Offset: 0x0006A964
		private void RpcReader___Observers_PlayEnterAnimation_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PlayEnterAnimation_2166136261();
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0006C78E File Offset: 0x0006A98E
		protected virtual void dll()
		{
			base.Awake();
		}

		// Token: 0x040015BC RID: 5564
		public NPCEnterableBuilding Building;

		// Token: 0x040015BD RID: 5565
		[Header("Optionally specify door to use. Otherwise closest door will be used.")]
		public StaticDoor Door;

		// Token: 0x040015BE RID: 5566
		private bool IsEntering;

		// Token: 0x040015BF RID: 5567
		private Coroutine enterRoutine;

		// Token: 0x040015C0 RID: 5568
		private bool dll_Excuted;

		// Token: 0x040015C1 RID: 5569
		private bool dll_Excuted;
	}
}
