using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Doors
{
	// Token: 0x02000677 RID: 1655
	public class DoorController : NetworkBehaviour
	{
		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002DD2 RID: 11730 RVA: 0x000C061D File Offset: 0x000BE81D
		// (set) Token: 0x06002DD3 RID: 11731 RVA: 0x000C0625 File Offset: 0x000BE825
		public bool IsOpen { get; protected set; }

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002DD4 RID: 11732 RVA: 0x000C062E File Offset: 0x000BE82E
		// (set) Token: 0x06002DD5 RID: 11733 RVA: 0x000C0636 File Offset: 0x000BE836
		public bool openedByNPC { get; protected set; }

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002DD6 RID: 11734 RVA: 0x000C063F File Offset: 0x000BE83F
		// (set) Token: 0x06002DD7 RID: 11735 RVA: 0x000C0647 File Offset: 0x000BE847
		public float timeSinceNPCSensed { get; protected set; } = float.MaxValue;

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002DD8 RID: 11736 RVA: 0x000C0650 File Offset: 0x000BE850
		// (set) Token: 0x06002DD9 RID: 11737 RVA: 0x000C0658 File Offset: 0x000BE858
		public bool playerDetectedSinceOpened { get; protected set; }

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002DDA RID: 11738 RVA: 0x000C0661 File Offset: 0x000BE861
		// (set) Token: 0x06002DDB RID: 11739 RVA: 0x000C0669 File Offset: 0x000BE869
		public float timeSincePlayerSensed { get; protected set; } = float.MaxValue;

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002DDC RID: 11740 RVA: 0x000C0672 File Offset: 0x000BE872
		// (set) Token: 0x06002DDD RID: 11741 RVA: 0x000C067A File Offset: 0x000BE87A
		public float timeInCurrentState { get; protected set; }

		// Token: 0x06002DDE RID: 11742 RVA: 0x000C0684 File Offset: 0x000BE884
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Doors.DoorController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x000C06A3 File Offset: 0x000BE8A3
		protected virtual void Start()
		{
			if (this.AutoCloseOnSleep)
			{
				ScheduleOne.GameTime.TimeManager.onSleepStart = (Action)Delegate.Combine(ScheduleOne.GameTime.TimeManager.onSleepStart, new Action(delegate()
				{
					if (this.IsOpen)
					{
						this.SetIsOpen(false, EDoorSide.Interior);
					}
				}));
			}
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x000C06D0 File Offset: 0x000BE8D0
		protected virtual void Update()
		{
			this.timeSinceNPCSensed += Time.deltaTime;
			this.timeSincePlayerSensed += Time.deltaTime;
			this.timeInCurrentState += Time.deltaTime;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.IsOpen && ((this.openedByNPC && this.timeSinceNPCSensed > this.ReturnToOriginalTime) || (this.autoOpenedForPlayer && this.timeSincePlayerSensed > this.ReturnToOriginalTime)))
			{
				this.openedByNPC = false;
				this.autoOpenedForPlayer = false;
				this.PlayerBlocker.enabled = false;
				this.SetIsOpen_Server(false, EDoorSide.Interior, false);
			}
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000C0772 File Offset: 0x000BE972
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.IsOpen)
			{
				this.SetIsOpen(connection, true, this.lastOpenSide);
			}
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000C0794 File Offset: 0x000BE994
		public virtual void InteriorHandleHovered()
		{
			string text;
			if (this.CanPlayerAccess(EDoorSide.Interior, out text))
			{
				foreach (InteractableObject interactableObject in this.InteriorIntObjs)
				{
					interactableObject.SetMessage(this.IsOpen ? "Close" : "Open");
					interactableObject.SetInteractableState(InteractableObject.EInteractableState.Default);
				}
				return;
			}
			foreach (InteractableObject interactableObject2 in this.InteriorIntObjs)
			{
				if (text != string.Empty)
				{
					interactableObject2.SetMessage(text);
					interactableObject2.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				}
				else
				{
					interactableObject2.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				}
			}
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000C0822 File Offset: 0x000BEA22
		public virtual void InteriorHandleInteracted()
		{
			if (this.CanPlayerAccess(EDoorSide.Interior))
			{
				if (!this.IsOpen && this.InteriorDoorHandleAnimation != null)
				{
					this.InteriorDoorHandleAnimation.Play();
				}
				this.SetIsOpen_Server(!this.IsOpen, EDoorSide.Interior, false);
			}
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000C0860 File Offset: 0x000BEA60
		public virtual void ExteriorHandleHovered()
		{
			string text;
			if (this.CanPlayerAccess(EDoorSide.Exterior, out text))
			{
				foreach (InteractableObject interactableObject in this.ExteriorIntObjs)
				{
					interactableObject.SetMessage(this.IsOpen ? "Close" : "Open");
					interactableObject.SetInteractableState(InteractableObject.EInteractableState.Default);
				}
				return;
			}
			foreach (InteractableObject interactableObject2 in this.ExteriorIntObjs)
			{
				if (text != string.Empty)
				{
					interactableObject2.SetMessage(text);
					interactableObject2.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				}
				else
				{
					interactableObject2.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				}
			}
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000C08EE File Offset: 0x000BEAEE
		public virtual void ExteriorHandleInteracted()
		{
			if (this.CanPlayerAccess(EDoorSide.Exterior))
			{
				if (!this.IsOpen && this.ExteriorDoorHandleAnimation != null)
				{
					this.ExteriorDoorHandleAnimation.Play();
				}
				this.SetIsOpen_Server(!this.IsOpen, EDoorSide.Exterior, false);
			}
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000C092C File Offset: 0x000BEB2C
		public bool CanPlayerAccess(EDoorSide side)
		{
			string text;
			return this.CanPlayerAccess(side, out text);
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000C0942 File Offset: 0x000BEB42
		protected virtual bool CanPlayerAccess(EDoorSide side, out string reason)
		{
			reason = this.noAccessErrorMessage;
			if (side != EDoorSide.Interior)
			{
				return side == EDoorSide.Exterior && (this.PlayerAccess == EDoorAccess.Open || this.PlayerAccess == EDoorAccess.EnterOnly);
			}
			return this.PlayerAccess == EDoorAccess.Open || this.PlayerAccess == EDoorAccess.ExitOnly;
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000C0980 File Offset: 0x000BEB80
		public virtual void NPCVicinityDetected(EDoorSide side)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.timeSinceNPCSensed = 0f;
			if (this.OpenableByNPCs && this.PlayerAccess != EDoorAccess.Open)
			{
				this.PlayerBlocker.enabled = true;
			}
			if (!this.IsOpen && this.OpenableByNPCs)
			{
				this.openedByNPC = true;
				this.SetIsOpen_Server(true, side, false);
			}
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000C09DC File Offset: 0x000BEBDC
		public virtual void PlayerVicinityDetected(EDoorSide side)
		{
			this.timeSincePlayerSensed = 0f;
			if (this.IsOpen)
			{
				this.playerDetectedSinceOpened = true;
			}
			if (!this.IsOpen && this.AutoOpenForPlayer && this.CanPlayerAccess(side))
			{
				this.autoOpenedForPlayer = true;
				this.SetIsOpen_Server(true, side, true);
			}
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000C0A2C File Offset: 0x000BEC2C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetIsOpen_Server(bool open, EDoorSide accessSide, bool openedForPlayer)
		{
			this.RpcWriter___Server_SetIsOpen_Server_1319291243(open, accessSide, openedForPlayer);
			this.RpcLogic___SetIsOpen_Server_1319291243(open, accessSide, openedForPlayer);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000C0A54 File Offset: 0x000BEC54
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetIsOpen(NetworkConnection conn, bool open, EDoorSide openSide)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetIsOpen_3381113727(conn, open, openSide);
				this.RpcLogic___SetIsOpen_3381113727(conn, open, openSide);
			}
			else
			{
				this.RpcWriter___Target_SetIsOpen_3381113727(conn, open, openSide);
			}
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000C0AA4 File Offset: 0x000BECA4
		public virtual void SetIsOpen(bool open, EDoorSide openSide)
		{
			if (this.IsOpen != open)
			{
				this.timeInCurrentState = 0f;
			}
			this.IsOpen = open;
			if (this.IsOpen)
			{
				this.playerDetectedSinceOpened = false;
			}
			this.lastOpenSide = openSide;
			if (this.IsOpen)
			{
				this.onDoorOpened.Invoke(openSide);
				return;
			}
			this.onDoorClosed.Invoke();
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000C0B04 File Offset: 0x000BED04
		protected virtual void CheckAutoCloseForDistantPlayer()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (this.timeSinceNPCSensed < this.ReturnToOriginalTime)
			{
				return;
			}
			if (this.timeSincePlayerSensed < this.ReturnToOriginalTime)
			{
				return;
			}
			float num;
			Player.GetClosestPlayer(base.transform.position, out num, null);
			if (num > 40f)
			{
				this.SetIsOpen_Server(false, EDoorSide.Interior, false);
			}
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000C0BD8 File Offset: 0x000BEDD8
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Doors.DoorControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Doors.DoorControllerAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetIsOpen_Server_1319291243));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsOpen_3381113727));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetIsOpen_3381113727));
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000C0C3B File Offset: 0x000BEE3B
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Doors.DoorControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Doors.DoorControllerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000C0C4E File Offset: 0x000BEE4E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000C0C5C File Offset: 0x000BEE5C
		private void RpcWriter___Server_SetIsOpen_Server_1319291243(bool open, EDoorSide accessSide, bool openedForPlayer)
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
			writer.WriteBoolean(open);
			writer.Write___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generated(accessSide);
			writer.WriteBoolean(openedForPlayer);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000C0D1D File Offset: 0x000BEF1D
		public void RpcLogic___SetIsOpen_Server_1319291243(bool open, EDoorSide accessSide, bool openedForPlayer)
		{
			this.autoOpenedForPlayer = openedForPlayer;
			if (openedForPlayer)
			{
				this.timeSincePlayerSensed = 0f;
			}
			this.SetIsOpen(null, open, accessSide);
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x000C0D40 File Offset: 0x000BEF40
		private void RpcReader___Server_SetIsOpen_Server_1319291243(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool open = PooledReader0.ReadBoolean();
			EDoorSide accessSide = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generateds(PooledReader0);
			bool openedForPlayer = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetIsOpen_Server_1319291243(open, accessSide, openedForPlayer);
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000C0DA0 File Offset: 0x000BEFA0
		private void RpcWriter___Observers_SetIsOpen_3381113727(NetworkConnection conn, bool open, EDoorSide openSide)
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
			writer.WriteBoolean(open);
			writer.Write___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generated(openSide);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000C0E63 File Offset: 0x000BF063
		public void RpcLogic___SetIsOpen_3381113727(NetworkConnection conn, bool open, EDoorSide openSide)
		{
			this.SetIsOpen(open, openSide);
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000C0E70 File Offset: 0x000BF070
		private void RpcReader___Observers_SetIsOpen_3381113727(PooledReader PooledReader0, Channel channel)
		{
			bool open = PooledReader0.ReadBoolean();
			EDoorSide openSide = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsOpen_3381113727(null, open, openSide);
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000C0EC0 File Offset: 0x000BF0C0
		private void RpcWriter___Target_SetIsOpen_3381113727(NetworkConnection conn, bool open, EDoorSide openSide)
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
			writer.WriteBoolean(open);
			writer.Write___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generated(openSide);
			base.SendTargetRpc(2U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000C0F84 File Offset: 0x000BF184
		private void RpcReader___Target_SetIsOpen_3381113727(PooledReader PooledReader0, Channel channel)
		{
			bool open = PooledReader0.ReadBoolean();
			EDoorSide openSide = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Doors.EDoorSideFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsOpen_3381113727(base.LocalConnection, open, openSide);
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x000C0FCC File Offset: 0x000BF1CC
		protected virtual void dll()
		{
			this.PlayerBlocker.enabled = false;
			foreach (InteractableObject interactableObject in this.InteriorIntObjs)
			{
				interactableObject.onHovered.AddListener(new UnityAction(this.InteriorHandleHovered));
				interactableObject.onInteractStart.AddListener(new UnityAction(this.InteriorHandleInteracted));
				interactableObject.SetMessage(this.IsOpen ? "Close" : "Open");
			}
			foreach (InteractableObject interactableObject2 in this.ExteriorIntObjs)
			{
				interactableObject2.onHovered.AddListener(new UnityAction(this.ExteriorHandleHovered));
				interactableObject2.onInteractStart.AddListener(new UnityAction(this.ExteriorHandleInteracted));
				interactableObject2.SetMessage(this.IsOpen ? "Close" : "Open");
			}
			if (base.gameObject.isStatic)
			{
				Console.LogError("DoorController is static! Doors should not be static!", base.gameObject);
			}
			if (this.AutoCloseOnDistantPlayer)
			{
				base.InvokeRepeating("CheckAutoCloseForDistantPlayer", 2f, 2f);
			}
		}

		// Token: 0x040020A5 RID: 8357
		public const float DISTANT_PLAYER_THRESHOLD = 40f;

		// Token: 0x040020A7 RID: 8359
		public EDoorAccess PlayerAccess;

		// Token: 0x040020A8 RID: 8360
		public bool AutoOpenForPlayer;

		// Token: 0x040020A9 RID: 8361
		[Header("References")]
		[SerializeField]
		protected InteractableObject[] InteriorIntObjs;

		// Token: 0x040020AA RID: 8362
		[SerializeField]
		protected InteractableObject[] ExteriorIntObjs;

		// Token: 0x040020AB RID: 8363
		[Tooltip("Used to block player from entering when the door is open for an NPC, but player isn't permitted access.")]
		[SerializeField]
		protected BoxCollider PlayerBlocker;

		// Token: 0x040020AC RID: 8364
		[Header("Animation")]
		[SerializeField]
		protected Animation InteriorDoorHandleAnimation;

		// Token: 0x040020AD RID: 8365
		[SerializeField]
		protected Animation ExteriorDoorHandleAnimation;

		// Token: 0x040020AE RID: 8366
		[Header("Settings")]
		[SerializeField]
		protected bool AutoCloseOnSleep = true;

		// Token: 0x040020AF RID: 8367
		[SerializeField]
		protected bool AutoCloseOnDistantPlayer = true;

		// Token: 0x040020B0 RID: 8368
		[Header("NPC Access")]
		[SerializeField]
		protected bool OpenableByNPCs = true;

		// Token: 0x040020B1 RID: 8369
		[Tooltip("How many seconds to wait after NPC passes through to return to original state")]
		[SerializeField]
		protected float ReturnToOriginalTime = 0.5f;

		// Token: 0x040020B2 RID: 8370
		public UnityEvent<EDoorSide> onDoorOpened;

		// Token: 0x040020B3 RID: 8371
		public UnityEvent onDoorClosed;

		// Token: 0x040020B4 RID: 8372
		private EDoorSide lastOpenSide = EDoorSide.Exterior;

		// Token: 0x040020B7 RID: 8375
		private bool autoOpenedForPlayer;

		// Token: 0x040020BB RID: 8379
		[HideInInspector]
		public string noAccessErrorMessage = string.Empty;

		// Token: 0x040020BC RID: 8380
		private bool dll_Excuted;

		// Token: 0x040020BD RID: 8381
		private bool dll_Excuted;
	}
}
