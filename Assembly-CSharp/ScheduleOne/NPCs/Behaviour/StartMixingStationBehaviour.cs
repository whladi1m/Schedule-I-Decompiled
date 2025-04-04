using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Employees;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000509 RID: 1289
	public class StartMixingStationBehaviour : Behaviour
	{
		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x0007E001 File Offset: 0x0007C201
		// (set) Token: 0x06001EB2 RID: 7858 RVA: 0x0007E009 File Offset: 0x0007C209
		public MixingStation targetStation { get; private set; }

		// Token: 0x06001EB3 RID: 7859 RVA: 0x0007E012 File Offset: 0x0007C212
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.StartMixingStationBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x0007E026 File Offset: 0x0007C226
		public void AssignStation(MixingStation station)
		{
			this.targetStation = station;
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x0007E02F File Offset: 0x0007C22F
		protected override void End()
		{
			base.End();
			if (this.startRoutine != null)
			{
				this.StopCook();
			}
			if (this.targetStation != null)
			{
				this.targetStation.SetNPCUser(null);
			}
			this.Disable();
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x0007E065 File Offset: 0x0007C265
		protected override void Pause()
		{
			base.Pause();
			if (this.targetStation != null)
			{
				this.targetStation.SetNPCUser(null);
			}
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x0007E088 File Offset: 0x0007C288
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.startRoutine != null)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.targetStation.UIPoint.position, 5, false);
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!base.Npc.Movement.IsMoving)
			{
				if (this.IsAtStation())
				{
					this.StartCook();
					return;
				}
				base.SetDestination(this.GetStationAccessPoint(), true);
			}
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x0007E101 File Offset: 0x0007C301
		[ObserversRpc(RunLocally = true)]
		private void StartCook()
		{
			this.RpcWriter___Observers_StartCook_2166136261();
			this.RpcLogic___StartCook_2166136261();
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x0007E110 File Offset: 0x0007C310
		private bool CanCookStart()
		{
			if (this.targetStation == null)
			{
				return false;
			}
			if (((IUsable)this.targetStation).IsInUse && ((IUsable)this.targetStation).NPCUserObject != base.Npc.NetworkObject)
			{
				return false;
			}
			MixingStationConfiguration mixingStationConfiguration = this.targetStation.Configuration as MixingStationConfiguration;
			return (float)this.targetStation.GetMixQuantity() >= mixingStationConfiguration.StartThrehold.Value;
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x0007E188 File Offset: 0x0007C388
		private void StopCook()
		{
			if (this.targetStation != null)
			{
				this.targetStation.SetNPCUser(null);
			}
			base.Npc.SetAnimationBool_Networked(null, "UseChemistryStation", false);
			if (this.startRoutine != null)
			{
				base.StopCoroutine(this.startRoutine);
				this.startRoutine = null;
			}
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x0007E1DC File Offset: 0x0007C3DC
		private Vector3 GetStationAccessPoint()
		{
			if (this.targetStation == null)
			{
				return base.Npc.transform.position;
			}
			return ((ITransitEntity)this.targetStation).AccessPoints[0].position;
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x0007E20F File Offset: 0x0007C40F
		private bool IsAtStation()
		{
			return !(this.targetStation == null) && Vector3.Distance(base.Npc.transform.position, this.GetStationAccessPoint()) < 1f;
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x0007E243 File Offset: 0x0007C443
		[CompilerGenerated]
		private IEnumerator <StartCook>g__CookRoutine|12_0()
		{
			base.Npc.Movement.FacePoint(this.targetStation.transform.position, 0.5f);
			yield return new WaitForSeconds(0.5f);
			if (!this.CanCookStart())
			{
				this.StopCook();
				base.End_Networked(null);
				yield break;
			}
			this.targetStation.SetNPCUser(base.Npc.NetworkObject);
			base.Npc.SetAnimationBool_Networked(null, "UseChemistryStation", true);
			QualityItemInstance product = this.targetStation.ProductSlot.ItemInstance as QualityItemInstance;
			ItemInstance mixer = this.targetStation.MixerSlot.ItemInstance;
			int mixQuantity = this.targetStation.GetMixQuantity();
			int num;
			for (int i = 0; i < mixQuantity; i = num + 1)
			{
				yield return new WaitForSeconds(1f);
				num = i;
			}
			if (InstanceFinder.IsServer)
			{
				this.targetStation.ProductSlot.ChangeQuantity(-mixQuantity, false);
				this.targetStation.MixerSlot.ChangeQuantity(-mixQuantity, false);
				MixOperation operation = new MixOperation(product.ID, product.Quality, mixer.ID, mixQuantity);
				this.targetStation.SendMixingOperation(operation, 0);
			}
			this.StopCook();
			base.End_Networked(null);
			yield break;
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x0007E252 File Offset: 0x0007C452
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartMixingStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartMixingStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_StartCook_2166136261));
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0007E282 File Offset: 0x0007C482
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartMixingStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartMixingStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0007E29B File Offset: 0x0007C49B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0007E2AC File Offset: 0x0007C4AC
		private void RpcWriter___Observers_StartCook_2166136261()
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

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0007E355 File Offset: 0x0007C555
		private void RpcLogic___StartCook_2166136261()
		{
			if (this.startRoutine != null)
			{
				return;
			}
			if (this.targetStation == null)
			{
				return;
			}
			this.startRoutine = base.StartCoroutine(this.<StartCook>g__CookRoutine|12_0());
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0007E384 File Offset: 0x0007C584
		private void RpcReader___Observers_StartCook_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartCook_2166136261();
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0007E3AE File Offset: 0x0007C5AE
		protected virtual void dll()
		{
			base.Awake();
			this.chemist = (base.Npc as Chemist);
		}

		// Token: 0x04001831 RID: 6193
		public const float INSERT_INGREDIENT_BASE_TIME = 1f;

		// Token: 0x04001833 RID: 6195
		private Chemist chemist;

		// Token: 0x04001834 RID: 6196
		private Coroutine startRoutine;

		// Token: 0x04001835 RID: 6197
		private bool dll_Excuted;

		// Token: 0x04001836 RID: 6198
		private bool dll_Excuted;
	}
}
