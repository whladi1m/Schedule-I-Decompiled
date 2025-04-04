using System;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.DevUtilities;
using ScheduleOne.Law;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.WorldspacePopup;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004DB RID: 1243
	public class CallPoliceBehaviour : Behaviour
	{
		// Token: 0x06001C2A RID: 7210 RVA: 0x00074D18 File Offset: 0x00072F18
		protected override void Begin()
		{
			base.Begin();
			if (!this.IsTargetValid())
			{
				this.End();
				this.Disable();
				return;
			}
			if (this.ReportedCrime == null)
			{
				Console.LogError("CallPoliceBehaviour doesn't have a crime set, disabling.", null);
				this.Disable();
				this.End();
				return;
			}
			Console.Log("CallPoliceBehaviour started on player " + this.Target.PlayerName, null);
			this.currentCallTime = 0f;
			this.RefreshIcon();
			if (this.Target.Owner.IsLocalClient)
			{
				this.PhoneCallPopup.enabled = true;
			}
			this.CallSound.Play();
			if (InstanceFinder.IsServer)
			{
				base.Npc.SetEquippable_Networked(null, this.PhonePrefab.AssetPath);
			}
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x000045B1 File Offset: 0x000027B1
		public void SetData(NetworkObject player, Crime crime)
		{
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x00074DD4 File Offset: 0x00072FD4
		protected override void Resume()
		{
			base.Resume();
			if (!this.IsTargetValid())
			{
				this.End();
				this.Disable();
				return;
			}
			this.currentCallTime = 0f;
			this.RefreshIcon();
			if (this.Target.Owner.IsLocalClient)
			{
				this.PhoneCallPopup.enabled = true;
			}
			this.CallSound.Play();
			if (InstanceFinder.IsServer)
			{
				base.Npc.SetEquippable_Networked(null, this.PhonePrefab.AssetPath);
			}
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x00074E54 File Offset: 0x00073054
		protected override void End()
		{
			base.End();
			this.currentCallTime = 0f;
			this.PhoneCallPopup.enabled = false;
			this.CallSound.Stop();
			if (InstanceFinder.IsServer)
			{
				base.Npc.SetEquippable_Networked(null, string.Empty);
			}
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x00074EA4 File Offset: 0x000730A4
		protected override void Pause()
		{
			base.Pause();
			this.currentCallTime = 0f;
			this.PhoneCallPopup.enabled = false;
			this.CallSound.Stop();
			if (InstanceFinder.IsServer)
			{
				base.Npc.SetEquippable_Networked(null, string.Empty);
			}
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x00074EF4 File Offset: 0x000730F4
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			this.currentCallTime += Time.deltaTime;
			this.RefreshIcon();
			base.Npc.Avatar.LookController.OverrideLookTarget(this.Target.EyePosition, 1, true);
			if (this.currentCallTime >= 4f && InstanceFinder.IsServer)
			{
				this.FinalizeCall();
			}
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x00074F5B File Offset: 0x0007315B
		private void RefreshIcon()
		{
			this.PhoneCallPopup.CurrentFillLevel = this.currentCallTime / 4f;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x00074F74 File Offset: 0x00073174
		[ObserversRpc(RunLocally = true)]
		private void FinalizeCall()
		{
			this.RpcWriter___Observers_FinalizeCall_2166136261();
			this.RpcLogic___FinalizeCall_2166136261();
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x00074F8D File Offset: 0x0007318D
		private bool IsTargetValid()
		{
			return !(this.Target == null) && this.Target.Health.IsAlive && !this.Target.IsArrested;
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x00074FC3 File Offset: 0x000731C3
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CallPoliceBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CallPoliceBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_FinalizeCall_2166136261));
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x00074FF3 File Offset: 0x000731F3
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CallPoliceBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CallPoliceBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x0007500C File Offset: 0x0007320C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x0007501C File Offset: 0x0007321C
		private void RpcWriter___Observers_FinalizeCall_2166136261()
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
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000750C8 File Offset: 0x000732C8
		private void RpcLogic___FinalizeCall_2166136261()
		{
			if (!base.Active)
			{
				return;
			}
			if (!this.IsTargetValid())
			{
				this.End();
				this.Disable();
				return;
			}
			Debug.Log("Call finalized on player " + this.Target.PlayerName);
			this.Target.CrimeData.RecordLastKnownPosition(true);
			this.Target.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Investigating);
			this.Target.CrimeData.AddCrime(this.ReportedCrime, 1);
			if (InstanceFinder.IsServer)
			{
				Singleton<LawManager>.Instance.PoliceCalled(this.Target, this.ReportedCrime);
			}
			this.End();
			this.Disable();
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x00075170 File Offset: 0x00073370
		private void RpcReader___Observers_FinalizeCall_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___FinalizeCall_2166136261();
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x0007519A File Offset: 0x0007339A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400173E RID: 5950
		public const float CALL_POLICE_TIME = 4f;

		// Token: 0x0400173F RID: 5951
		[Header("References")]
		public WorldspacePopup PhoneCallPopup;

		// Token: 0x04001740 RID: 5952
		public AvatarEquippable PhonePrefab;

		// Token: 0x04001741 RID: 5953
		public AudioSourceController CallSound;

		// Token: 0x04001742 RID: 5954
		private float currentCallTime;

		// Token: 0x04001743 RID: 5955
		public Player Target;

		// Token: 0x04001744 RID: 5956
		public Crime ReportedCrime;

		// Token: 0x04001745 RID: 5957
		private bool dll_Excuted;

		// Token: 0x04001746 RID: 5958
		private bool dll_Excuted;
	}
}
