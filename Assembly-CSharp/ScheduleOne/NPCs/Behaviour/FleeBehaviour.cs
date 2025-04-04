using System;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.AI;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200050E RID: 1294
	public class FleeBehaviour : Behaviour
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001F01 RID: 7937 RVA: 0x0007EF43 File Offset: 0x0007D143
		// (set) Token: 0x06001F02 RID: 7938 RVA: 0x0007EF4B File Offset: 0x0007D14B
		public NetworkObject EntityToFlee { get; private set; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001F03 RID: 7939 RVA: 0x0007EF54 File Offset: 0x0007D154
		public Vector3 PointToFlee
		{
			get
			{
				if (this.FleeMode != FleeBehaviour.EFleeMode.Point)
				{
					return this.EntityToFlee.transform.position;
				}
				return this.FleeOrigin;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x0007EF76 File Offset: 0x0007D176
		// (set) Token: 0x06001F05 RID: 7941 RVA: 0x0007EF7E File Offset: 0x0007D17E
		public FleeBehaviour.EFleeMode FleeMode { get; private set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x0007EF87 File Offset: 0x0007D187
		// (set) Token: 0x06001F07 RID: 7943 RVA: 0x0007EF8F File Offset: 0x0007D18F
		public Vector3 FleeOrigin { get; private set; } = Vector3.zero;

		// Token: 0x06001F08 RID: 7944 RVA: 0x0007EF98 File Offset: 0x0007D198
		[ObserversRpc(RunLocally = true)]
		public void SetEntityToFlee(NetworkObject entity)
		{
			this.RpcWriter___Observers_SetEntityToFlee_3323014238(entity);
			this.RpcLogic___SetEntityToFlee_3323014238(entity);
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0007EFAE File Offset: 0x0007D1AE
		[ObserversRpc(RunLocally = true)]
		public void SetPointToFlee(Vector3 point)
		{
			this.RpcWriter___Observers_SetPointToFlee_4276783012(point);
			this.RpcLogic___SetPointToFlee_4276783012(point);
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x0007EFC4 File Offset: 0x0007D1C4
		protected override void Begin()
		{
			base.Begin();
			this.StartFlee();
			EVOLineType lineType = (UnityEngine.Random.Range(0, 2) == 0) ? EVOLineType.Scared : EVOLineType.Concerned;
			base.Npc.PlayVO(lineType);
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x0007EFF8 File Offset: 0x0007D1F8
		protected override void Resume()
		{
			base.Resume();
			this.StartFlee();
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x0007F006 File Offset: 0x0007D206
		protected override void End()
		{
			base.End();
			this.Stop();
			base.Npc.Avatar.EmotionManager.RemoveEmotionOverride("fleeing");
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x0007F02E File Offset: 0x0007D22E
		protected override void Pause()
		{
			base.Pause();
			this.Stop();
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x0007F03C File Offset: 0x0007D23C
		private void StartFlee()
		{
			this.Flee();
			base.Npc.Avatar.EmotionManager.AddEmotionOverride("Scared", "fleeing", 0f, 0);
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("fleeing", 2, 0.7f));
			this.nextVO = Time.time + UnityEngine.Random.Range(5f, 15f);
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x0007F0B4 File Offset: 0x0007D2B4
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.FleeMode == FleeBehaviour.EFleeMode.Entity && this.EntityToFlee == null)
			{
				this.End();
				return;
			}
			if (!base.Npc.Movement.IsMoving && Vector3.Distance(base.transform.position, this.currentFleeTarget) < 3f)
			{
				base.End_Networked(null);
				base.Disable_Networked(null);
				return;
			}
			Vector3 from = this.PointToFlee - base.transform.position;
			from.y = 0f;
			if (Vector3.Angle(from, base.Npc.Movement.Agent.desiredVelocity) < 30f)
			{
				Console.Log("Fleeing entity is in front, finding new flee position", null);
				this.Flee();
			}
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x0007F180 File Offset: 0x0007D380
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			if (Time.time > this.nextVO)
			{
				EVOLineType lineType = (UnityEngine.Random.Range(0, 2) == 0) ? EVOLineType.Scared : EVOLineType.Concerned;
				base.Npc.PlayVO(lineType);
				this.nextVO = Time.time + UnityEngine.Random.Range(5f, 15f);
			}
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x0007F1D6 File Offset: 0x0007D3D6
		private void Stop()
		{
			base.Npc.Movement.Stop();
			base.Npc.Movement.SpeedController.RemoveSpeedControl("fleeing");
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x0007F204 File Offset: 0x0007D404
		private void Flee()
		{
			Vector3 fleePosition = this.GetFleePosition();
			this.currentFleeTarget = fleePosition;
			base.Npc.Movement.SetDestination(fleePosition);
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x0007F230 File Offset: 0x0007D430
		public Vector3 GetFleePosition()
		{
			int num = 0;
			float num2 = 0f;
			while (this.FleeMode != FleeBehaviour.EFleeMode.Entity || !(this.EntityToFlee == null))
			{
				Vector3 point = base.transform.position - this.PointToFlee;
				point.y = 0f;
				point = Quaternion.AngleAxis(num2, Vector3.up) * point;
				float d = UnityEngine.Random.Range(20f, 40f);
				RaycastHit raycastHit;
				NavMeshHit navMeshHit;
				if (Physics.Raycast(base.transform.position + point.normalized * d + Vector3.up * 10f, Vector3.down, out raycastHit, 20f, LayerMask.GetMask(new string[]
				{
					"Default"
				})) && NavMeshUtility.SamplePosition(raycastHit.point, out navMeshHit, 2f, -1, true))
				{
					return navMeshHit.position;
				}
				if (num > 10)
				{
					Console.LogWarning("Failed to find a valid flee position, returning current position", null);
					return base.transform.position;
				}
				num2 += 15f;
				num++;
			}
			return Vector3.zero;
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x0007F368 File Offset: 0x0007D568
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FleeBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FleeBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetEntityToFlee_3323014238));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetPointToFlee_4276783012));
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0007F3BA File Offset: 0x0007D5BA
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FleeBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FleeBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x0007F3D3 File Offset: 0x0007D5D3
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x0007F3E4 File Offset: 0x0007D5E4
		private void RpcWriter___Observers_SetEntityToFlee_3323014238(NetworkObject entity)
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
			writer.WriteNetworkObject(entity);
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x0007F49A File Offset: 0x0007D69A
		public void RpcLogic___SetEntityToFlee_3323014238(NetworkObject entity)
		{
			this.EntityToFlee = entity;
			this.FleeMode = FleeBehaviour.EFleeMode.Entity;
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x0007F4AC File Offset: 0x0007D6AC
		private void RpcReader___Observers_SetEntityToFlee_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject entity = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetEntityToFlee_3323014238(entity);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0007F4E8 File Offset: 0x0007D6E8
		private void RpcWriter___Observers_SetPointToFlee_4276783012(Vector3 point)
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
			writer.WriteVector3(point);
			base.SendObserversRpc(16U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0007F59E File Offset: 0x0007D79E
		public void RpcLogic___SetPointToFlee_4276783012(Vector3 point)
		{
			this.FleeOrigin = point;
			this.FleeMode = FleeBehaviour.EFleeMode.Point;
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x0007F5B0 File Offset: 0x0007D7B0
		private void RpcReader___Observers_SetPointToFlee_4276783012(PooledReader PooledReader0, Channel channel)
		{
			Vector3 point = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetPointToFlee_4276783012(point);
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0007F5EB File Offset: 0x0007D7EB
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400184B RID: 6219
		public const float FLEE_DIST_MIN = 20f;

		// Token: 0x0400184C RID: 6220
		public const float FLEE_DIST_MAX = 40f;

		// Token: 0x0400184D RID: 6221
		public const float FLEE_SPEED = 0.7f;

		// Token: 0x04001851 RID: 6225
		private Vector3 currentFleeTarget = Vector3.zero;

		// Token: 0x04001852 RID: 6226
		private float nextVO;

		// Token: 0x04001853 RID: 6227
		private bool dll_Excuted;

		// Token: 0x04001854 RID: 6228
		private bool dll_Excuted;

		// Token: 0x0200050F RID: 1295
		public enum EFleeMode
		{
			// Token: 0x04001856 RID: 6230
			Entity,
			// Token: 0x04001857 RID: 6231
			Point
		}
	}
}
