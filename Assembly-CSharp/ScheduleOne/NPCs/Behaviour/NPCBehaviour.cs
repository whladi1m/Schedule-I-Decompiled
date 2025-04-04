using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.Doors;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Map;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000516 RID: 1302
	public class NPCBehaviour : NetworkBehaviour
	{
		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001F5F RID: 8031 RVA: 0x00080518 File Offset: 0x0007E718
		// (set) Token: 0x06001F60 RID: 8032 RVA: 0x00080520 File Offset: 0x0007E720
		public Behaviour activeBehaviour { get; set; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001F61 RID: 8033 RVA: 0x00080529 File Offset: 0x0007E729
		// (set) Token: 0x06001F62 RID: 8034 RVA: 0x00080531 File Offset: 0x0007E731
		public NPC Npc { get; private set; }

		// Token: 0x06001F63 RID: 8035 RVA: 0x0008053C File Offset: 0x0007E73C
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.NPCBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0008055C File Offset: 0x0007E75C
		protected virtual void Start()
		{
			this.Npc.Avatar.Anim.onHeavyFlinch.AddListener(new UnityAction(this.HeavyFlinchBehaviour.Flinch));
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			for (int i = 0; i < this.behaviourStack.Count; i++)
			{
				Behaviour b = this.behaviourStack[i];
				if (b.Enabled)
				{
					this.enabledBehaviours.Add(b);
				}
				b.onEnable.AddListener(delegate()
				{
					this.AddEnabledBehaviour(b);
				});
				b.onDisable.AddListener(delegate()
				{
					this.RemoveEnabledBehaviour(b);
				});
			}
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x00080670 File Offset: 0x0007E870
		private void OnDestroy()
		{
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000806A0 File Offset: 0x0007E8A0
		protected override void OnValidate()
		{
			base.OnValidate();
			this.behaviourStack = base.GetComponentsInChildren<Behaviour>().ToList<Behaviour>();
			this.SortBehaviourStack();
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x000806BF File Offset: 0x0007E8BF
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.activeBehaviour != null)
			{
				this.activeBehaviour.Begin_Networked(connection);
			}
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x000806E4 File Offset: 0x0007E8E4
		[ServerRpc(RequireOwnership = false)]
		public void Summon(string buildingGUID, int doorIndex, float duration)
		{
			this.RpcWriter___Server_Summon_900355577(buildingGUID, doorIndex, duration);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x00080703 File Offset: 0x0007E903
		[ServerRpc(RequireOwnership = false)]
		public void ConsumeProduct(ProductItemInstance product)
		{
			this.RpcWriter___Server_ConsumeProduct_2622925554(product);
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x00080710 File Offset: 0x0007E910
		protected virtual void OnKnockOut()
		{
			this.CoweringBehaviour.Disable_Networked(null);
			this.RagdollBehaviour.Disable_Networked(null);
			this.CallPoliceBehaviour.Disable_Networked(null);
			this.GenericDialogueBehaviour.Disable_Networked(null);
			this.HeavyFlinchBehaviour.Disable_Networked(null);
			this.FacePlayerBehaviour.Disable_Networked(null);
			this.SummonBehaviour.Disable_Networked(null);
			this.ConsumeProductBehaviour.Disable_Networked(null);
			this.CombatBehaviour.Disable_Networked(null);
			this.FleeBehaviour.Disable_Networked(null);
			this.StationaryBehaviour.Disable_Networked(null);
			this.RequestProductBehaviour.Disable_Networked(null);
			foreach (Behaviour behaviour in this.behaviourStack)
			{
				if (!(behaviour == this.DeadBehaviour) && !(behaviour == this.UnconsciousBehaviour) && behaviour.Active)
				{
					behaviour.End_Networked(null);
				}
			}
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x00080818 File Offset: 0x0007EA18
		protected virtual void OnDie()
		{
			this.OnKnockOut();
			this.UnconsciousBehaviour.Disable_Networked(null);
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x0008082C File Offset: 0x0007EA2C
		public Behaviour GetBehaviour(string BehaviourName)
		{
			Behaviour behaviour = this.behaviourStack.Find((Behaviour x) => x.Name.ToLower() == BehaviourName.ToLower());
			if (behaviour == null)
			{
				Console.LogWarning("No behaviour found with name '" + BehaviourName + "'", null);
			}
			return behaviour;
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x00080884 File Offset: 0x0007EA84
		public virtual void Update()
		{
			if (this.DEBUG_MODE && this.activeBehaviour != null)
			{
				Debug.Log("Active behaviour: " + this.activeBehaviour.Name);
			}
			if (InstanceFinder.IsHost)
			{
				Behaviour enabledBehaviour = this.GetEnabledBehaviour();
				if (enabledBehaviour != this.activeBehaviour)
				{
					if (this.activeBehaviour != null)
					{
						this.activeBehaviour.Pause_Networked(null);
					}
					if (enabledBehaviour != null)
					{
						if (enabledBehaviour.Started)
						{
							enabledBehaviour.Resume_Networked(null);
						}
						else
						{
							enabledBehaviour.Begin_Networked(null);
						}
					}
				}
			}
			if (this.activeBehaviour != null && this.activeBehaviour.Active)
			{
				this.activeBehaviour.BehaviourUpdate();
			}
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x0008093E File Offset: 0x0007EB3E
		public virtual void LateUpdate()
		{
			if (this.activeBehaviour != null && this.activeBehaviour.Active)
			{
				this.activeBehaviour.BehaviourLateUpdate();
			}
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x00080966 File Offset: 0x0007EB66
		protected virtual void MinPass()
		{
			if (this.activeBehaviour != null && this.activeBehaviour.Active)
			{
				this.activeBehaviour.ActiveMinPass();
			}
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0008098E File Offset: 0x0007EB8E
		public void SortBehaviourStack()
		{
			this.behaviourStack = (from x in this.behaviourStack
			orderby x.Priority descending
			select x).ToList<Behaviour>();
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x000809C5 File Offset: 0x0007EBC5
		private Behaviour GetEnabledBehaviour()
		{
			return this.enabledBehaviours.FirstOrDefault<Behaviour>();
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x000809D4 File Offset: 0x0007EBD4
		private void AddEnabledBehaviour(Behaviour b)
		{
			if (!this.enabledBehaviours.Contains(b))
			{
				this.enabledBehaviours.Add(b);
				this.enabledBehaviours = (from x in this.enabledBehaviours
				orderby x.Priority descending
				select x).ToList<Behaviour>();
			}
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x00080A30 File Offset: 0x0007EC30
		private void RemoveEnabledBehaviour(Behaviour b)
		{
			if (this.enabledBehaviours.Contains(b))
			{
				this.enabledBehaviours.Remove(b);
				this.enabledBehaviours = (from x in this.enabledBehaviours
				orderby x.Priority descending
				select x).ToList<Behaviour>();
			}
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x00080AAC File Offset: 0x0007ECAC
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.NPCBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.NPCBehaviourAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_Summon_900355577));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_ConsumeProduct_2622925554));
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x00080AF8 File Offset: 0x0007ECF8
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.NPCBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.NPCBehaviourAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x00080B0B File Offset: 0x0007ED0B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x00080B1C File Offset: 0x0007ED1C
		private void RpcWriter___Server_Summon_900355577(string buildingGUID, int doorIndex, float duration)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(buildingGUID);
			writer.WriteInt32(doorIndex, AutoPackType.Packed);
			writer.WriteSingle(duration, AutoPackType.Unpacked);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x00080BE8 File Offset: 0x0007EDE8
		public void RpcLogic___Summon_900355577(string buildingGUID, int doorIndex, float duration)
		{
			NPCBehaviour.<>c__DisplayClass32_0 CS$<>8__locals1 = new NPCBehaviour.<>c__DisplayClass32_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.duration = duration;
			NPCEnterableBuilding @object = GUIDManager.GetObject<NPCEnterableBuilding>(new Guid(buildingGUID));
			if (@object == null)
			{
				Console.LogError("Failed to find building with GUID: " + buildingGUID, null);
				return;
			}
			StaticDoor lastEnteredDoor = @object.Doors[doorIndex];
			this.Npc.LastEnteredDoor = lastEnteredDoor;
			this.SummonBehaviour.Enable_Networked(null);
			if (this.summonRoutine != null)
			{
				base.StopCoroutine(this.summonRoutine);
			}
			this.summonRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<Summon>g__Routine|0());
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x00080C7C File Offset: 0x0007EE7C
		private void RpcReader___Server_Summon_900355577(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string buildingGUID = PooledReader0.ReadString();
			int doorIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			float duration = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___Summon_900355577(buildingGUID, doorIndex, duration);
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x00080CDC File Offset: 0x0007EEDC
		private void RpcWriter___Server_ConsumeProduct_2622925554(ProductItemInstance product)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteProductItemInstance(product);
			base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x00080D83 File Offset: 0x0007EF83
		public void RpcLogic___ConsumeProduct_2622925554(ProductItemInstance product)
		{
			this.ConsumeProductBehaviour.SendProduct(product);
			this.ConsumeProductBehaviour.Enable_Networked(null);
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x00080DA0 File Offset: 0x0007EFA0
		private void RpcReader___Server_ConsumeProduct_2622925554(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ProductItemInstance product = PooledReader0.ReadProductItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___ConsumeProduct_2622925554(product);
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x00080DD4 File Offset: 0x0007EFD4
		protected virtual void dll()
		{
			this.Npc = base.GetComponentInParent<NPC>();
			this.Npc.Health.onKnockedOut.AddListener(new UnityAction(this.OnKnockOut));
			this.Npc.Health.onDie.AddListener(new UnityAction(this.OnDie));
		}

		// Token: 0x04001873 RID: 6259
		public bool DEBUG_MODE;

		// Token: 0x04001874 RID: 6260
		[Header("References")]
		public NPCScheduleManager ScheduleManager;

		// Token: 0x04001875 RID: 6261
		[Header("Default Behaviours")]
		public CoweringBehaviour CoweringBehaviour;

		// Token: 0x04001876 RID: 6262
		public RagdollBehaviour RagdollBehaviour;

		// Token: 0x04001877 RID: 6263
		public CallPoliceBehaviour CallPoliceBehaviour;

		// Token: 0x04001878 RID: 6264
		public GenericDialogueBehaviour GenericDialogueBehaviour;

		// Token: 0x04001879 RID: 6265
		public HeavyFlinchBehaviour HeavyFlinchBehaviour;

		// Token: 0x0400187A RID: 6266
		public FacePlayerBehaviour FacePlayerBehaviour;

		// Token: 0x0400187B RID: 6267
		public DeadBehaviour DeadBehaviour;

		// Token: 0x0400187C RID: 6268
		public UnconsciousBehaviour UnconsciousBehaviour;

		// Token: 0x0400187D RID: 6269
		public Behaviour SummonBehaviour;

		// Token: 0x0400187E RID: 6270
		public ConsumeProductBehaviour ConsumeProductBehaviour;

		// Token: 0x0400187F RID: 6271
		public CombatBehaviour CombatBehaviour;

		// Token: 0x04001880 RID: 6272
		public FleeBehaviour FleeBehaviour;

		// Token: 0x04001881 RID: 6273
		public StationaryBehaviour StationaryBehaviour;

		// Token: 0x04001882 RID: 6274
		public RequestProductBehaviour RequestProductBehaviour;

		// Token: 0x04001883 RID: 6275
		[SerializeField]
		protected List<Behaviour> behaviourStack = new List<Behaviour>();

		// Token: 0x04001886 RID: 6278
		private Coroutine summonRoutine;

		// Token: 0x04001887 RID: 6279
		[SerializeField]
		private List<Behaviour> enabledBehaviours = new List<Behaviour>();

		// Token: 0x04001888 RID: 6280
		private bool dll_Excuted;

		// Token: 0x04001889 RID: 6281
		private bool dll_Excuted;
	}
}
