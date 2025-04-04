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
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000507 RID: 1287
	public class StartLabOvenBehaviour : Behaviour
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001E97 RID: 7831 RVA: 0x0007DA40 File Offset: 0x0007BC40
		// (set) Token: 0x06001E98 RID: 7832 RVA: 0x0007DA48 File Offset: 0x0007BC48
		public LabOven targetOven { get; private set; }

		// Token: 0x06001E99 RID: 7833 RVA: 0x0007DA51 File Offset: 0x0007BC51
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.StartLabOvenBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0007DA65 File Offset: 0x0007BC65
		public void SetTargetOven(LabOven oven)
		{
			this.targetOven = oven;
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0007DA6E File Offset: 0x0007BC6E
		protected override void End()
		{
			base.End();
			if (this.targetOven != null)
			{
				this.targetOven.Door.SetPosition(0f);
			}
			if (this.cookRoutine != null)
			{
				this.StopCook();
			}
			this.Disable();
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0007DAB0 File Offset: 0x0007BCB0
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.cookRoutine != null)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.targetOven.UIPoint.position, 5, false);
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

		// Token: 0x06001E9D RID: 7837 RVA: 0x0007DB29 File Offset: 0x0007BD29
		[ObserversRpc(RunLocally = true)]
		private void StartCook()
		{
			this.RpcWriter___Observers_StartCook_2166136261();
			this.RpcLogic___StartCook_2166136261();
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x0007DB38 File Offset: 0x0007BD38
		private bool CanCookStart()
		{
			return !(this.targetOven == null) && (!((IUsable)this.targetOven).IsInUse || !(((IUsable)this.targetOven).NPCUserObject != base.Npc.NetworkObject)) && this.targetOven.CurrentOperation == null && this.targetOven.IsIngredientCookable();
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x0007DBA0 File Offset: 0x0007BDA0
		private void StopCook()
		{
			if (this.targetOven != null)
			{
				this.targetOven.SetNPCUser(null);
			}
			if (this.cookRoutine != null)
			{
				base.StopCoroutine(this.cookRoutine);
				this.cookRoutine = null;
			}
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x0007DBD7 File Offset: 0x0007BDD7
		private Vector3 GetStationAccessPoint()
		{
			if (this.targetOven == null)
			{
				return base.Npc.transform.position;
			}
			return ((ITransitEntity)this.targetOven).AccessPoints[0].position;
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0007DC0A File Offset: 0x0007BE0A
		private bool IsAtStation()
		{
			return !(this.targetOven == null) && Vector3.Distance(base.Npc.transform.position, this.GetStationAccessPoint()) < 1f;
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0007DC3E File Offset: 0x0007BE3E
		[CompilerGenerated]
		private IEnumerator <StartCook>g__CookRoutine|11_0()
		{
			Console.Log("Starting cook...", null);
			this.targetOven.SetNPCUser(base.Npc.NetworkObject);
			base.Npc.Movement.FacePoint(this.targetOven.transform.position, 0.5f);
			yield return new WaitForSeconds(0.5f);
			if (!this.CanCookStart())
			{
				this.StopCook();
				base.End_Networked(null);
				yield break;
			}
			this.targetOven.Door.SetPosition(1f);
			yield return new WaitForSeconds(0.5f);
			this.targetOven.WireTray.SetPosition(1f);
			yield return new WaitForSeconds(5f);
			this.targetOven.Door.SetPosition(0f);
			yield return new WaitForSeconds(1f);
			ItemInstance itemInstance = this.targetOven.IngredientSlot.ItemInstance;
			if (itemInstance == null)
			{
				Console.LogWarning("No ingredient in oven!", null);
				this.StopCook();
				base.End_Networked(null);
				yield break;
			}
			int num = 1;
			if ((itemInstance.Definition as StorableItemDefinition).StationItem.GetModule<CookableModule>().CookType == CookableModule.ECookableType.Solid)
			{
				num = Mathf.Min(this.targetOven.IngredientSlot.Quantity, 10);
			}
			itemInstance.ChangeQuantity(-num);
			string id = (itemInstance.Definition as StorableItemDefinition).StationItem.GetModule<CookableModule>().Product.ID;
			EQuality ingredientQuality = EQuality.Standard;
			if (itemInstance is QualityItemInstance)
			{
				ingredientQuality = (itemInstance as QualityItemInstance).Quality;
			}
			this.targetOven.SendCookOperation(new OvenCookOperation(itemInstance.ID, ingredientQuality, num, id));
			this.StopCook();
			base.End_Networked(null);
			yield break;
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x0007DC4D File Offset: 0x0007BE4D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartLabOvenBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartLabOvenBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_StartCook_2166136261));
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x0007DC7D File Offset: 0x0007BE7D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartLabOvenBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartLabOvenBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x0007DC96 File Offset: 0x0007BE96
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x0007DCA4 File Offset: 0x0007BEA4
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

		// Token: 0x06001EA8 RID: 7848 RVA: 0x0007DD4D File Offset: 0x0007BF4D
		private void RpcLogic___StartCook_2166136261()
		{
			if (this.cookRoutine != null)
			{
				return;
			}
			if (this.targetOven == null)
			{
				return;
			}
			this.cookRoutine = base.StartCoroutine(this.<StartCook>g__CookRoutine|11_0());
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x0007DD7C File Offset: 0x0007BF7C
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

		// Token: 0x06001EAA RID: 7850 RVA: 0x0007DDA6 File Offset: 0x0007BFA6
		protected virtual void dll()
		{
			base.Awake();
			this.chemist = (base.Npc as Chemist);
		}

		// Token: 0x04001828 RID: 6184
		public const float POUR_TIME = 5f;

		// Token: 0x0400182A RID: 6186
		private Chemist chemist;

		// Token: 0x0400182B RID: 6187
		private Coroutine cookRoutine;

		// Token: 0x0400182C RID: 6188
		private bool dll_Excuted;

		// Token: 0x0400182D RID: 6189
		private bool dll_Excuted;
	}
}
