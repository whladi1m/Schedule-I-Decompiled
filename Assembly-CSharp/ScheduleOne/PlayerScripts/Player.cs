using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using EasyButtons;
using FishNet;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Logging;
using FishNet.Managing.Server;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using FishySteamworks;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Animation;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Intro;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.Map;
using ScheduleOne.Money;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts.Health;
using ScheduleOne.Product;
using ScheduleOne.Property;
using ScheduleOne.Skating;
using ScheduleOne.Stealth;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using ScheduleOne.UI.MainMenu;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005CE RID: 1486
	public class Player : NetworkBehaviour, ISaveable, IDamageable
	{
		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x060024DA RID: 9434 RVA: 0x00094764 File Offset: 0x00092964
		public bool IsLocalPlayer
		{
			get
			{
				return base.IsOwner;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x060024DB RID: 9435 RVA: 0x0009476C File Offset: 0x0009296C
		// (set) Token: 0x060024DC RID: 9436 RVA: 0x00094774 File Offset: 0x00092974
		public string PlayerName
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<PlayerName>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<PlayerName>k__BackingField(value, true);
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x060024DD RID: 9437 RVA: 0x0009477E File Offset: 0x0009297E
		// (set) Token: 0x060024DE RID: 9438 RVA: 0x00094786 File Offset: 0x00092986
		public string PlayerCode
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<PlayerCode>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<PlayerCode>k__BackingField(value, true);
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x060024DF RID: 9439 RVA: 0x00094790 File Offset: 0x00092990
		// (set) Token: 0x060024E0 RID: 9440 RVA: 0x00094798 File Offset: 0x00092998
		public NetworkObject CurrentVehicle
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CurrentVehicle>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true)]
			set
			{
				this.RpcWriter___Server_set_CurrentVehicle_3323014238(value);
				this.RpcLogic___set_CurrentVehicle_3323014238(value);
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x060024E1 RID: 9441 RVA: 0x000947AE File Offset: 0x000929AE
		// (set) Token: 0x060024E2 RID: 9442 RVA: 0x000947B6 File Offset: 0x000929B6
		public float TimeSinceVehicleExit { get; protected set; }

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x060024E3 RID: 9443 RVA: 0x000947BF File Offset: 0x000929BF
		// (set) Token: 0x060024E4 RID: 9444 RVA: 0x000947C7 File Offset: 0x000929C7
		public bool Crouched { get; private set; }

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060024E5 RID: 9445 RVA: 0x000947D0 File Offset: 0x000929D0
		// (set) Token: 0x060024E6 RID: 9446 RVA: 0x000947D8 File Offset: 0x000929D8
		public NetworkObject CurrentBed
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CurrentBed>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc]
			set
			{
				this.RpcWriter___Server_set_CurrentBed_3323014238(value);
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060024E7 RID: 9447 RVA: 0x000947E4 File Offset: 0x000929E4
		// (set) Token: 0x060024E8 RID: 9448 RVA: 0x000947EC File Offset: 0x000929EC
		public bool IsReadyToSleep
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<IsReadyToSleep>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.sync___set_value_<IsReadyToSleep>k__BackingField(value, true);
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x060024E9 RID: 9449 RVA: 0x000947F6 File Offset: 0x000929F6
		// (set) Token: 0x060024EA RID: 9450 RVA: 0x000947FE File Offset: 0x000929FE
		public bool IsSkating
		{
			[CompilerGenerated]
			get
			{
				return this.<IsSkating>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc]
			set
			{
				this.RpcWriter___Server_set_IsSkating_1140765316(value);
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x060024EB RID: 9451 RVA: 0x0009480A File Offset: 0x00092A0A
		// (set) Token: 0x060024EC RID: 9452 RVA: 0x00094812 File Offset: 0x00092A12
		public Skateboard ActiveSkateboard { get; private set; }

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x060024ED RID: 9453 RVA: 0x0009481B File Offset: 0x00092A1B
		// (set) Token: 0x060024EE RID: 9454 RVA: 0x00094823 File Offset: 0x00092A23
		public bool IsSleeping { get; protected set; }

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x060024EF RID: 9455 RVA: 0x0009482C File Offset: 0x00092A2C
		// (set) Token: 0x060024F0 RID: 9456 RVA: 0x00094834 File Offset: 0x00092A34
		public bool IsRagdolled { get; protected set; }

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x0009483D File Offset: 0x00092A3D
		// (set) Token: 0x060024F2 RID: 9458 RVA: 0x00094845 File Offset: 0x00092A45
		public bool IsArrested { get; protected set; }

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x060024F3 RID: 9459 RVA: 0x0009484E File Offset: 0x00092A4E
		// (set) Token: 0x060024F4 RID: 9460 RVA: 0x00094856 File Offset: 0x00092A56
		public bool IsTased { get; protected set; }

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x060024F5 RID: 9461 RVA: 0x0009485F File Offset: 0x00092A5F
		// (set) Token: 0x060024F6 RID: 9462 RVA: 0x00094867 File Offset: 0x00092A67
		public bool IsUnconscious { get; protected set; }

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x060024F7 RID: 9463 RVA: 0x00094870 File Offset: 0x00092A70
		// (set) Token: 0x060024F8 RID: 9464 RVA: 0x00094878 File Offset: 0x00092A78
		public float Scale { get; private set; }

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x060024F9 RID: 9465 RVA: 0x00094881 File Offset: 0x00092A81
		// (set) Token: 0x060024FA RID: 9466 RVA: 0x00094889 File Offset: 0x00092A89
		public Property CurrentProperty { get; protected set; }

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060024FB RID: 9467 RVA: 0x00094892 File Offset: 0x00092A92
		// (set) Token: 0x060024FC RID: 9468 RVA: 0x0009489A File Offset: 0x00092A9A
		public Property LastVisitedProperty { get; protected set; }

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x000948A3 File Offset: 0x00092AA3
		// (set) Token: 0x060024FE RID: 9470 RVA: 0x000948AB File Offset: 0x00092AAB
		public Business CurrentBusiness { get; protected set; }

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060024FF RID: 9471 RVA: 0x000948B4 File Offset: 0x00092AB4
		public Vector3 PlayerBasePosition
		{
			get
			{
				return base.transform.position - base.transform.up * (this.CharacterController.height / 2f);
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002500 RID: 9472 RVA: 0x000948E7 File Offset: 0x00092AE7
		// (set) Token: 0x06002501 RID: 9473 RVA: 0x000948EF File Offset: 0x00092AEF
		public Vector3 CameraPosition
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CameraPosition>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc]
			set
			{
				this.RpcWriter___Server_set_CameraPosition_4276783012(value);
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06002502 RID: 9474 RVA: 0x000948FB File Offset: 0x00092AFB
		// (set) Token: 0x06002503 RID: 9475 RVA: 0x00094903 File Offset: 0x00092B03
		public Quaternion CameraRotation
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CameraRotation>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc]
			set
			{
				this.RpcWriter___Server_set_CameraRotation_3429297120(value);
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x0009490F File Offset: 0x00092B0F
		// (set) Token: 0x06002505 RID: 9477 RVA: 0x00094917 File Offset: 0x00092B17
		public BasicAvatarSettings CurrentAvatarSettings { get; protected set; }

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06002506 RID: 9478 RVA: 0x00094920 File Offset: 0x00092B20
		// (set) Token: 0x06002507 RID: 9479 RVA: 0x00094928 File Offset: 0x00092B28
		public ProductItemInstance ConsumedProduct { get; private set; }

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x00094931 File Offset: 0x00092B31
		// (set) Token: 0x06002509 RID: 9481 RVA: 0x00094939 File Offset: 0x00092B39
		public int TimeSinceProductConsumed { get; private set; }

		// Token: 0x0600250A RID: 9482 RVA: 0x00094942 File Offset: 0x00092B42
		[Button]
		public void LoadDebugAvatarSettings()
		{
			this.SetAppearance(this.DebugAvatarSettings, false);
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x0600250B RID: 9483 RVA: 0x00094951 File Offset: 0x00092B51
		public string SaveFolderName
		{
			get
			{
				if (InstanceFinder.IsServer && base.IsOwner)
				{
					return "Player_0";
				}
				return "Player_" + this.PlayerCode;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x0600250C RID: 9484 RVA: 0x00094978 File Offset: 0x00092B78
		public string SaveFileName
		{
			get
			{
				return "Player";
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x0600250D RID: 9485 RVA: 0x0009497F File Offset: 0x00092B7F
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x0600250E RID: 9486 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x0600250F RID: 9487 RVA: 0x00094987 File Offset: 0x00092B87
		// (set) Token: 0x06002510 RID: 9488 RVA: 0x0009498F File Offset: 0x00092B8F
		public List<string> LocalExtraFiles { get; set; }

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06002511 RID: 9489 RVA: 0x00094998 File Offset: 0x00092B98
		// (set) Token: 0x06002512 RID: 9490 RVA: 0x000949A0 File Offset: 0x00092BA0
		public List<string> LocalExtraFolders { get; set; }

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06002513 RID: 9491 RVA: 0x000949A9 File Offset: 0x00092BA9
		// (set) Token: 0x06002514 RID: 9492 RVA: 0x000949B1 File Offset: 0x00092BB1
		public bool HasChanged { get; set; }

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06002515 RID: 9493 RVA: 0x000949BA File Offset: 0x00092BBA
		// (set) Token: 0x06002516 RID: 9494 RVA: 0x000949C2 File Offset: 0x00092BC2
		public bool avatarVisibleToLocalPlayer { get; private set; }

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06002517 RID: 9495 RVA: 0x000949CB File Offset: 0x00092BCB
		// (set) Token: 0x06002518 RID: 9496 RVA: 0x000949D3 File Offset: 0x00092BD3
		public bool playerDataRetrieveReturned { get; private set; }

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06002519 RID: 9497 RVA: 0x000949DC File Offset: 0x00092BDC
		// (set) Token: 0x0600251A RID: 9498 RVA: 0x000949E4 File Offset: 0x00092BE4
		public bool playerSaveRequestReturned { get; private set; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600251B RID: 9499 RVA: 0x000949ED File Offset: 0x00092BED
		// (set) Token: 0x0600251C RID: 9500 RVA: 0x000949F5 File Offset: 0x00092BF5
		public bool PlayerInitializedOverNetwork { get; private set; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x0600251D RID: 9501 RVA: 0x000949FE File Offset: 0x00092BFE
		// (set) Token: 0x0600251E RID: 9502 RVA: 0x00094A06 File Offset: 0x00092C06
		public bool Paranoid { get; set; }

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x0600251F RID: 9503 RVA: 0x00094A0F File Offset: 0x00092C0F
		// (set) Token: 0x06002520 RID: 9504 RVA: 0x00094A17 File Offset: 0x00092C17
		public bool Sneaky { get; set; }

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x00094A20 File Offset: 0x00092C20
		// (set) Token: 0x06002522 RID: 9506 RVA: 0x00094A28 File Offset: 0x00092C28
		public bool Disoriented { get; set; }

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06002523 RID: 9507 RVA: 0x00094A31 File Offset: 0x00092C31
		// (set) Token: 0x06002524 RID: 9508 RVA: 0x00094A39 File Offset: 0x00092C39
		public bool Seizure { get; set; }

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06002525 RID: 9509 RVA: 0x00094A42 File Offset: 0x00092C42
		// (set) Token: 0x06002526 RID: 9510 RVA: 0x00094A4A File Offset: 0x00092C4A
		public bool Slippery { get; set; }

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06002527 RID: 9511 RVA: 0x00094A53 File Offset: 0x00092C53
		// (set) Token: 0x06002528 RID: 9512 RVA: 0x00094A5B File Offset: 0x00092C5B
		public bool Schizophrenic { get; set; }

		// Token: 0x06002529 RID: 9513 RVA: 0x00094A64 File Offset: 0x00092C64
		public static Player GetPlayer(NetworkConnection conn)
		{
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if (Player.PlayerList[i].Connection == conn)
				{
					return Player.PlayerList[i];
				}
			}
			return null;
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x00094AAC File Offset: 0x00092CAC
		public static Player GetRandomPlayer(bool excludeArrestedOrDead = true, bool excludeSleeping = true)
		{
			List<Player> list = new List<Player>();
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if ((!excludeArrestedOrDead || (!Player.PlayerList[i].IsArrested && Player.PlayerList[i].Health.IsAlive)) && (!excludeSleeping || !Player.PlayerList[i].IsSleeping))
				{
					list.Add(Player.PlayerList[i]);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			return list[index];
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x00094B44 File Offset: 0x00092D44
		public static Player GetPlayer(string playerCode)
		{
			return Player.PlayerList.Find((Player x) => x.PlayerCode == playerCode);
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x00094B74 File Offset: 0x00092D74
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.PlayerScripts.Player_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x00094B94 File Offset: 0x00092D94
		protected virtual void Start()
		{
			MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
			instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Combine(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x00094BF0 File Offset: 0x00092DF0
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
			if (NetworkSingleton<MoneyManager>.InstanceExists)
			{
				MoneyManager instance2 = NetworkSingleton<MoneyManager>.Instance;
				instance2.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Remove(instance2.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			}
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x00094C58 File Offset: 0x00092E58
		public override void OnStartClient()
		{
			base.OnStartClient();
			this.Connection = base.Owner;
			if (base.IsOwner)
			{
				if (Application.isEditor)
				{
					this.LoadDebugAvatarSettings();
				}
				this.LocalGameObject.gameObject.SetActive(true);
				Player.Local = this;
				if (Player.onLocalPlayerSpawned != null)
				{
					Player.onLocalPlayerSpawned();
				}
				LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Invisible"));
				if (Singleton<Lobby>.Instance.IsInLobby && !Singleton<Lobby>.Instance.IsHost)
				{
					InstanceFinder.TransportManager.GetTransport<FishySteamworks>().OnClientConnectionState += this.ClientConnectionStateChanged;
				}
				this.FootstepDetector.enabled = false;
				this.PoI.SetMainText("You");
				if (this.PoI.UI != null)
				{
					this.PoI.UI.GetComponentInChildren<Animation>().Play();
				}
				this.NameLabel.gameObject.SetActive(false);
				if (base.IsHost)
				{
					if (Singleton<LoadManager>.Instance.IsGameLoaded)
					{
						this.PlayerLoaded();
					}
					else
					{
						Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.PlayerLoaded));
					}
				}
				CSteamID csteamID = CSteamID.Nil;
				if (SteamManager.Initialized)
				{
					csteamID = SteamUser.GetSteamID();
					this.PlayerName = SteamFriends.GetPersonaName();
				}
				this.SendPlayerNameData(this.PlayerName, csteamID.m_SteamID);
				if (!InstanceFinder.IsServer)
				{
					this.RequestPlayerData(this.PlayerCode);
				}
			}
			else
			{
				this.LocalFootstepDetector.enabled = false;
				this.CapCol.isTrigger = true;
				base.gameObject.name = this.PlayerName + " (" + this.PlayerCode + ")";
				this.PoI.SetMainText(this.PlayerName);
			}
			if (base.IsOwner || InstanceFinder.IsServer || (Singleton<Lobby>.Instance.IsInLobby && Singleton<Lobby>.Instance.IsHost))
			{
				this.CreatePlayerVariables();
			}
			if (Player.onPlayerSpawned != null)
			{
				Player.onPlayerSpawned(this);
			}
			Console.Log("Player spawned (" + this.PlayerName + ")", null);
			this.CrimeData.RecordLastKnownPosition(false);
			if (!base.IsOwner && this.CurrentVehicle != null)
			{
				Console.Log("This player is in a vehicle!", null);
				this.EnterVehicle(this.CurrentVehicle.GetComponent<LandVehicle>());
			}
			Player.PlayerList.Add(this);
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x00094EC8 File Offset: 0x000930C8
		private void PlayerLoaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.PlayerLoaded));
			if (!base.IsOwner)
			{
				return;
			}
			if (this.PoI != null)
			{
				this.PoI.SetMainText("You");
				if (this.PoI.UI != null)
				{
					this.PoI.UI.GetComponentInChildren<Animation>().Play();
				}
			}
			this.MarkPlayerInitialized();
			if (!this.HasCompletedIntro && !Singleton<LoadManager>.Instance.DebugMode && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Main")
			{
				PlayerSingleton<PlayerMovement>.Instance.Teleport(NetworkSingleton<GameManager>.Instance.SpawnPoint.position);
				base.transform.forward = NetworkSingleton<GameManager>.Instance.SpawnPoint.forward;
				Console.Log("Player has not completed intro; playing intro", null);
				Singleton<IntroManager>.Instance.Play();
				Singleton<CharacterCreator>.Instance.onComplete.AddListener(new UnityAction<BasicAvatarSettings>(this.MarkIntroCompleted));
			}
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x00094FDC File Offset: 0x000931DC
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (base.Owner != connection)
			{
				PlayerData data = new PlayerData(this.PlayerCode, base.transform.position, base.transform.eulerAngles.y, this.HasCompletedIntro);
				string empty = string.Empty;
				string appearanceString = (this.CurrentAvatarSettings != null) ? this.CurrentAvatarSettings.GetJson(true) : string.Empty;
				string clothingString = this.GetClothingString();
				if (this.Crouched)
				{
					this.ReceiveCrouched(connection, true);
				}
				this.ReceivePlayerData(connection, data, empty, appearanceString, clothingString, null);
				this.ReceivePlayerNameData(connection, this.PlayerName, this.PlayerCode);
			}
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x0009508C File Offset: 0x0009328C
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void RequestSavePlayer()
		{
			this.RpcWriter___Server_RequestSavePlayer_2166136261();
			this.RpcLogic___RequestSavePlayer_2166136261();
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x0009509A File Offset: 0x0009329A
		[ObserversRpc]
		[TargetRpc]
		private void ReturnSaveRequest(NetworkConnection conn, bool successful)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReturnSaveRequest_214505783(conn, successful);
			}
			else
			{
				this.RpcWriter___Target_ReturnSaveRequest_214505783(conn, successful);
			}
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000950C4 File Offset: 0x000932C4
		[ObserversRpc(RunLocally = true)]
		public void HostExitedGame()
		{
			this.RpcWriter___Observers_HostExitedGame_2166136261();
			this.RpcLogic___HostExitedGame_2166136261();
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000950DD File Offset: 0x000932DD
		private void ClientConnectionStateChanged(ClientConnectionStateArgs args)
		{
			Console.Log("Client connection state changed: " + args.ConnectionState.ToString(), null);
			if (args.ConnectionState == LocalConnectionState.Stopping || args.ConnectionState == LocalConnectionState.Stopped)
			{
				this.HostExitedGame();
			}
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x00095118 File Offset: 0x00093318
		[ServerRpc(RunLocally = true)]
		public void SendPlayerNameData(string playerName, ulong id)
		{
			this.RpcWriter___Server_SendPlayerNameData_586648380(playerName, id);
			this.RpcLogic___SendPlayerNameData_586648380(playerName, id);
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x00095144 File Offset: 0x00093344
		[ServerRpc(RequireOwnership = false)]
		public void RequestPlayerData(string playerCode)
		{
			this.RpcWriter___Server_RequestPlayerData_3615296227(playerCode);
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x0009515B File Offset: 0x0009335B
		[ServerRpc(RunLocally = true)]
		public void MarkPlayerInitialized()
		{
			this.RpcWriter___Server_MarkPlayerInitialized_2166136261();
			this.RpcLogic___MarkPlayerInitialized_2166136261();
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x0009516C File Offset: 0x0009336C
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void ReceivePlayerData(NetworkConnection conn, PlayerData data, string inventoryString, string appearanceString, string clothigString, VariableData[] vars)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceivePlayerData_3244732873(conn, data, inventoryString, appearanceString, clothigString, vars);
				this.RpcLogic___ReceivePlayerData_3244732873(conn, data, inventoryString, appearanceString, clothigString, vars);
			}
			else
			{
				this.RpcWriter___Target_ReceivePlayerData_3244732873(conn, data, inventoryString, appearanceString, clothigString, vars);
			}
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000951E0 File Offset: 0x000933E0
		public void SetGravityMultiplier(float multiplier)
		{
			if (base.IsOwner)
			{
				PlayerMovement.GravityMultiplier = multiplier;
			}
			foreach (ConstantForce constantForce in this.ragdollForceComponents)
			{
				constantForce.force = Physics.gravity * multiplier * constantForce.GetComponent<Rigidbody>().mass;
			}
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x0009525C File Offset: 0x0009345C
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void ReceivePlayerNameData(NetworkConnection conn, string playerName, string id)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceivePlayerNameData_3895153758(conn, playerName, id);
				this.RpcLogic___ReceivePlayerNameData_3895153758(conn, playerName, id);
			}
			else
			{
				this.RpcWriter___Target_ReceivePlayerNameData_3895153758(conn, playerName, id);
			}
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000952A9 File Offset: 0x000934A9
		public void SendFlashlightOn(bool on)
		{
			this.SendFlashlightOnNetworked(on);
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000952B2 File Offset: 0x000934B2
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		private void SendFlashlightOnNetworked(bool on)
		{
			this.RpcWriter___Server_SendFlashlightOnNetworked_1140765316(on);
			this.RpcLogic___SendFlashlightOnNetworked_1140765316(on);
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x000952C8 File Offset: 0x000934C8
		[ObserversRpc(RunLocally = true)]
		private void SetFlashlightOn(bool on)
		{
			this.RpcWriter___Observers_SetFlashlightOn_1140765316(on);
			this.RpcLogic___SetFlashlightOn_1140765316(on);
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x000952DE File Offset: 0x000934DE
		public override void OnStopClient()
		{
			base.OnStopClient();
			Player.PlayerList.Remove(this);
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x000952F2 File Offset: 0x000934F2
		public override void OnStartServer()
		{
			base.OnStartServer();
			base.ServerManager.Objects.OnPreDestroyClientObjects += this.PreDestroyClientObjects;
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x00095318 File Offset: 0x00093518
		protected virtual void Update()
		{
			this.HasChanged = true;
			if (this.CurrentVehicle != null)
			{
				this.TimeSinceVehicleExit = 0f;
			}
			else
			{
				this.TimeSinceVehicleExit += Time.deltaTime;
			}
			if (!base.IsOwner)
			{
				return;
			}
			if (base.transform.position.y < -20f)
			{
				float y = 0f;
				if (MapHeightSampler.Sample(base.transform.position.x, out y, base.transform.position.z))
				{
					PlayerSingleton<PlayerMovement>.Instance.Teleport(new Vector3(base.transform.position.x, y, base.transform.position.z));
				}
				else
				{
					PlayerSingleton<PlayerMovement>.Instance.Teleport(MapHeightSampler.ResetPosition);
				}
			}
			if (this.ActiveSkateboard != null)
			{
				this.SetCapsuleColliderHeight(1f - this.ActiveSkateboard.Animation.CurrentCrouchShift * 0.3f);
			}
			if (NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Player_In_Vehicle", (this.CurrentVehicle != null).ToString(), true);
			}
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x00095444 File Offset: 0x00093644
		protected virtual void MinPass()
		{
			if (this.ConsumedProduct != null)
			{
				int timeSinceProductConsumed = this.TimeSinceProductConsumed;
				this.TimeSinceProductConsumed = timeSinceProductConsumed + 1;
				if (this.TimeSinceProductConsumed >= (this.ConsumedProduct.Definition as ProductDefinition).EffectsDuration)
				{
					this.ClearProduct();
				}
			}
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x0009548C File Offset: 0x0009368C
		protected virtual void LateUpdate()
		{
			if (base.IsOwner)
			{
				this.CameraPosition = PlayerSingleton<PlayerCamera>.Instance.transform.position;
				this.CameraRotation = PlayerSingleton<PlayerCamera>.Instance.transform.rotation;
			}
			if (this.Seizure)
			{
				for (int i = 0; i < this.Avatar.RagdollRBs.Length; i++)
				{
					if (this.seizureRotations.Count <= this.Avatar.RagdollRBs.Length)
					{
						this.seizureRotations.Add(Quaternion.identity);
					}
					this.seizureRotations[i] = Quaternion.Lerp(this.seizureRotations[i], Quaternion.Euler(UnityEngine.Random.insideUnitSphere * 30f), Time.deltaTime * 10f);
					this.Avatar.RagdollRBs[i].transform.localRotation *= this.seizureRotations[i];
				}
			}
			this.MimicCamera.transform.position = this.CameraPosition;
			this.MimicCamera.transform.rotation = this.CameraRotation;
			this.EyePosition = this.Avatar.Eyes.transform.position;
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x000955D0 File Offset: 0x000937D0
		private void RecalculateCurrentProperty()
		{
			Property property = (from x in Property.Properties
			orderby Vector3.Distance(x.BoundingBox.transform.position, this.Avatar.CenterPoint)
			select x).FirstOrDefault<Property>();
			Business business = (from x in Business.Businesses
			orderby Vector3.Distance(x.BoundingBox.transform.position, this.Avatar.CenterPoint)
			select x).FirstOrDefault<Business>();
			if (property == null)
			{
				this.CurrentProperty = null;
			}
			else if (property.DoBoundsContainPoint(this.Avatar.CenterPoint))
			{
				this.CurrentProperty = property;
				this.LastVisitedProperty = this.CurrentProperty;
			}
			else
			{
				this.CurrentProperty = null;
			}
			if (business == null)
			{
				this.CurrentBusiness = null;
				return;
			}
			if (business.DoBoundsContainPoint(this.Avatar.CenterPoint))
			{
				this.CurrentBusiness = business;
				return;
			}
			this.CurrentBusiness = null;
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x00095689 File Offset: 0x00093889
		private void FixedUpdate()
		{
			this.ApplyMovementVisuals();
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x00095694 File Offset: 0x00093894
		private void ApplyMovementVisuals()
		{
			if (this.IsSkating)
			{
				this.Anim.SetTimeAirborne(0f);
				this.Anim.SetGrounded(true);
				this.Anim.SetDirection(0f);
				this.Anim.SetStrafe(0f);
				return;
			}
			bool isGrounded = this.GetIsGrounded();
			if (isGrounded)
			{
				this.timeAirborne = 0f;
			}
			else
			{
				this.timeAirborne += Time.deltaTime;
			}
			this.Anim.SetTimeAirborne(this.timeAirborne);
			if (this.Crouched)
			{
				this.standingScale = Mathf.MoveTowards(this.standingScale, 0f, Time.deltaTime / PlayerMovement.CrouchTime);
			}
			else
			{
				this.standingScale = Mathf.MoveTowards(this.standingScale, 1f, Time.deltaTime / PlayerMovement.CrouchTime);
			}
			this.Anim.SetGrounded(isGrounded);
			this.Anim.SetCrouched(this.Crouched);
			this.Avatar.transform.localPosition = new Vector3(0f, Mathf.Lerp(this.AvatarOffset_Crouched, this.AvatarOffset_Standing, this.standingScale), 0f);
			Vector3 vector = base.transform.InverseTransformVector(this.VelocityCalculator.Velocity) / (PlayerMovement.WalkSpeed * PlayerMovement.SprintMultiplier);
			if (this.Crouched)
			{
				this.Anim.SetDirection(this.CrouchWalkMapCurve.Evaluate(Mathf.Abs(vector.z)) * Mathf.Sign(vector.z));
				this.Anim.SetStrafe(this.CrouchWalkMapCurve.Evaluate(Mathf.Abs(vector.x)) * Mathf.Sign(vector.x));
				return;
			}
			this.Anim.SetDirection(this.WalkingMapCurve.Evaluate(Mathf.Abs(vector.z)) * Mathf.Sign(vector.z));
			this.Anim.SetStrafe(this.WalkingMapCurve.Evaluate(Mathf.Abs(vector.x)) * Mathf.Sign(vector.x));
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000958A3 File Offset: 0x00093AA3
		public void SetVisible(bool vis, bool network = false)
		{
			this.Avatar.SetVisible(vis);
			this.CapCol.enabled = vis;
			if (network)
			{
				this.SetVisible_Networked(vis);
			}
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000958C7 File Offset: 0x00093AC7
		[ObserversRpc]
		public void PlayJumpAnimation()
		{
			this.RpcWriter___Observers_PlayJumpAnimation_2166136261();
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x000958D0 File Offset: 0x00093AD0
		public bool GetIsGrounded()
		{
			float maxDistance = PlayerMovement.StandingControllerHeight * (this.Crouched ? PlayerMovement.CrouchHeightMultiplier : 1f) / 2f + 0.1f;
			RaycastHit raycastHit;
			return Physics.SphereCast(base.transform.position, PlayerMovement.ControllerRadius * 0.75f, Vector3.down, out raycastHit, maxDistance, this.GroundDetectionMask);
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x00095932 File Offset: 0x00093B32
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SendCrouched(bool crouched)
		{
			this.RpcWriter___Server_SendCrouched_1140765316(crouched);
			this.RpcLogic___SendCrouched_1140765316(crouched);
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x00095948 File Offset: 0x00093B48
		public void SetCrouchedLocal(bool crouched)
		{
			this.Crouched = crouched;
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x00095951 File Offset: 0x00093B51
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		private void ReceiveCrouched(NetworkConnection conn, bool crouched)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveCrouched_214505783(conn, crouched);
				this.RpcLogic___ReceiveCrouched_214505783(conn, crouched);
			}
			else
			{
				this.RpcWriter___Target_ReceiveCrouched_214505783(conn, crouched);
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x00095987 File Offset: 0x00093B87
		[ServerRpc(RunLocally = true)]
		public void SendAvatarSettings(AvatarSettings settings)
		{
			this.RpcWriter___Server_SendAvatarSettings_4281687581(settings);
			this.RpcLogic___SendAvatarSettings_4281687581(settings);
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x0009599D File Offset: 0x00093B9D
		[ObserversRpc(BufferLast = true, RunLocally = true)]
		public void SetAvatarSettings(AvatarSettings settings)
		{
			this.RpcWriter___Observers_SetAvatarSettings_4281687581(settings);
			this.RpcLogic___SetAvatarSettings_4281687581(settings);
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x000959B3 File Offset: 0x00093BB3
		[ObserversRpc]
		private void SetVisible_Networked(bool vis)
		{
			this.RpcWriter___Observers_SetVisible_Networked_1140765316(vis);
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000959C0 File Offset: 0x00093BC0
		public void EnterVehicle(LandVehicle vehicle)
		{
			this.CurrentVehicle = vehicle.NetworkObject;
			this.LastDrivenVehicle = vehicle;
			this.Avatar.transform.SetParent(vehicle.transform);
			this.Avatar.transform.localPosition = Vector3.zero;
			this.Avatar.transform.localRotation = Quaternion.identity;
			if (this.onEnterVehicle != null)
			{
				this.onEnterVehicle(vehicle);
			}
			this.SetVisible(false, true);
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x00095A3C File Offset: 0x00093C3C
		public void ExitVehicle(Transform exitPoint)
		{
			if (this.CurrentVehicle == null)
			{
				return;
			}
			this.Avatar.transform.SetParent(base.transform);
			this.Avatar.transform.localPosition = Vector3.zero;
			this.Avatar.transform.localRotation = Quaternion.identity;
			Player.Local.transform.position = exitPoint.position;
			Player.Local.transform.rotation = exitPoint.rotation;
			Player.Local.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
			base.GetComponent<NetworkTransform>().ClearReplicateCache();
			if (this.onExitVehicle != null)
			{
				this.onExitVehicle(this.CurrentVehicle.GetComponent<LandVehicle>(), exitPoint);
			}
			this.SetVisible(true, false);
			this.CurrentVehicle = null;
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x00095B2C File Offset: 0x00093D2C
		private void PreDestroyClientObjects(NetworkConnection conn)
		{
			if (this.CurrentVehicle != null)
			{
				this.CurrentVehicle.RemoveOwnership();
				this.CurrentVehicle.GetComponent<LandVehicle>().ExitVehicle();
			}
			int count = this.objectsTemporarilyOwnedByPlayer.Count;
			for (int i = 0; i < count; i++)
			{
				Debug.Log("Stripping object ownership back to server: " + this.objectsTemporarilyOwnedByPlayer[i].gameObject.name);
				this.objectsTemporarilyOwnedByPlayer[i].RemoveOwnership();
			}
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x00095BB0 File Offset: 0x00093DB0
		private void CurrentVehicleChanged(NetworkObject oldVeh, NetworkObject newVeh, bool asServer)
		{
			if (base.IsOwner)
			{
				return;
			}
			if (oldVeh == newVeh)
			{
				return;
			}
			if (newVeh != null)
			{
				this.Avatar.transform.SetParent(newVeh.transform);
				this.Avatar.transform.localPosition = Vector3.zero;
				this.Avatar.transform.localRotation = Quaternion.identity;
				this.SetVisible(false, false);
				return;
			}
			this.Avatar.transform.SetParent(base.transform);
			this.Avatar.transform.localPosition = Vector3.zero;
			this.Avatar.transform.localRotation = Quaternion.identity;
			this.SetVisible(true, false);
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x00095C6C File Offset: 0x00093E6C
		public static bool AreAllPlayersReadyToSleep()
		{
			if (Player.PlayerList.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if (!(Player.PlayerList[i] == null) && !Player.PlayerList[i].IsReadyToSleep)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x00095CC4 File Offset: 0x00093EC4
		private void SleepStart()
		{
			this.IsSleeping = true;
			this.ClearProduct();
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x00095CD3 File Offset: 0x00093ED3
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetReadyToSleep(bool ready)
		{
			this.RpcWriter___Server_SetReadyToSleep_1140765316(ready);
			this.RpcLogic___SetReadyToSleep_1140765316(ready);
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x00095CE9 File Offset: 0x00093EE9
		private void SleepEnd(int minsSlept)
		{
			this.IsSleeping = false;
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x00095CF2 File Offset: 0x00093EF2
		public static void Activate()
		{
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x00095D2A File Offset: 0x00093F2A
		public static void Deactivate(bool freeMouse)
		{
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.ResetRotation();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			if (freeMouse)
			{
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			}
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x00095D64 File Offset: 0x00093F64
		public void ExitAll()
		{
			if (this.CurrentVehicle != null)
			{
				this.CurrentVehicle.GetComponent<LandVehicle>().ExitVehicle();
				this.SetVisible(true, false);
			}
			Singleton<GameInput>.Instance.ExitAll();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			Singleton<HUD>.Instance.canvas.enabled = false;
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x00095DE0 File Offset: 0x00093FE0
		public void SetVisibleToLocalPlayer(bool vis)
		{
			this.avatarVisibleToLocalPlayer = vis;
			if (vis)
			{
				LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Player"));
				return;
			}
			LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Invisible"));
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x00095E2C File Offset: 0x0009402C
		[ObserversRpc(RunLocally = true)]
		public void SetPlayerCode(string code)
		{
			this.RpcWriter___Observers_SetPlayerCode_3615296227(code);
			this.RpcLogic___SetPlayerCode_3615296227(code);
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x00095E42 File Offset: 0x00094042
		[ServerRpc]
		public void SendPunch()
		{
			this.RpcWriter___Server_SendPunch_2166136261();
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x00095E4A File Offset: 0x0009404A
		[ObserversRpc]
		private void Punch()
		{
			this.RpcWriter___Observers_Punch_2166136261();
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x00095E52 File Offset: 0x00094052
		[ServerRpc(RunLocally = true)]
		private void MarkIntroCompleted(BasicAvatarSettings appearance)
		{
			this.RpcWriter___Server_MarkIntroCompleted_3281254764(appearance);
			this.RpcLogic___MarkIntroCompleted_3281254764(appearance);
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x00095E68 File Offset: 0x00094068
		public bool IsPointVisibleToPlayer(Vector3 point, float maxDistance_Visible = 30f, float minDistance_Invisible = 5f)
		{
			float num = Vector3.Distance(point, this.MimicCamera.transform.position);
			RaycastHit raycastHit;
			return num <= maxDistance_Visible && (num < minDistance_Invisible || (this.MimicCamera.InverseTransformPoint(point).z >= 0f && !Physics.Raycast(this.MimicCamera.transform.position, (point - this.MimicCamera.transform.position).normalized, out raycastHit, Mathf.Min(maxDistance_Visible, num - 0.5f), 1 << LayerMask.NameToLayer("Default"))));
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x00095F08 File Offset: 0x00094108
		public static Player GetClosestPlayer(Vector3 point, out float distance, List<Player> exclude = null)
		{
			distance = 0f;
			List<Player> list = new List<Player>();
			list.AddRange(Player.PlayerList);
			if (exclude != null)
			{
				list = list.Except(exclude).ToList<Player>();
			}
			Player player = (from x in list
			orderby Vector3.SqrMagnitude(point - x.Avatar.CenterPoint)
			select x).FirstOrDefault<Player>();
			if (player != null)
			{
				distance = Vector3.Distance(point, player.Avatar.CenterPoint);
				return player;
			}
			return null;
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x00095F88 File Offset: 0x00094188
		public void SetCapsuleColliderHeight(float normalizedHeight)
		{
			this.CapCol.height = 2f * normalizedHeight;
			this.CapCol.center = new Vector3(0f, -(2f - this.CapCol.height) / 2f, 0f);
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x00095FD9 File Offset: 0x000941D9
		public void SetScale(float scale)
		{
			this.Scale = scale;
			this.ApplyScale();
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x00095FE8 File Offset: 0x000941E8
		public void SetScale(float scale, float lerpTime)
		{
			Player.<>c__DisplayClass281_0 CS$<>8__locals1 = new Player.<>c__DisplayClass281_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.scale = scale;
			CS$<>8__locals1.lerpTime = lerpTime;
			if (this.lerpScaleRoutine != null)
			{
				base.StopCoroutine(this.lerpScaleRoutine);
			}
			CS$<>8__locals1.startScale = this.Scale;
			this.lerpScaleRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetScale>g__LerpScale|0());
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x00096048 File Offset: 0x00094248
		protected virtual void ApplyScale()
		{
			if (this.ActiveSkateboard != null)
			{
				this.ActiveSkateboard.ApplyPlayerScale();
				base.transform.localScale = Vector3.one;
				return;
			}
			base.transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000960A1 File Offset: 0x000942A1
		public virtual string GetSaveString()
		{
			return this.GetPlayerData().GetJson(true);
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000960AF File Offset: 0x000942AF
		public PlayerData GetPlayerData()
		{
			return new PlayerData(this.PlayerCode, base.transform.position, base.transform.eulerAngles.y, this.HasCompletedIntro);
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000960E0 File Offset: 0x000942E0
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> result = new List<string>();
			((ISaveable)this).WriteSubfile(parentFolderPath, "Inventory", this.GetInventoryString());
			if (this.CurrentAvatarSettings != null)
			{
				string appearanceString = this.GetAppearanceString();
				((ISaveable)this).WriteSubfile(parentFolderPath, "Appearance", appearanceString);
			}
			((ISaveable)this).WriteSubfile(parentFolderPath, "Clothing", this.GetClothingString());
			string path = ((ISaveable)this).WriteFolder(parentFolderPath, "Variables");
			for (int i = 0; i < this.PlayerVariables.Count; i++)
			{
				if (this.PlayerVariables[i] != null && this.PlayerVariables[i].Persistent)
				{
					string json = new VariableData(this.PlayerVariables[i].Name, this.PlayerVariables[i].GetValue().ToString()).GetJson(true);
					string path2 = SaveManager.MakeFileSafe(this.PlayerVariables[i].Name) + ".json";
					string text = Path.Combine(path, path2);
					try
					{
						File.WriteAllText(text, json);
					}
					catch (Exception ex)
					{
						Console.LogWarning("Failed to write player variable file: " + text + " - " + ex.Message, null);
					}
				}
			}
			return result;
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x0009622C File Offset: 0x0009442C
		public string GetInventoryString()
		{
			return new ItemSet(this.Inventory.ToList<ItemSlot>()).GetJSON();
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x00096243 File Offset: 0x00094443
		public string GetAppearanceString()
		{
			if (this.CurrentAvatarSettings != null)
			{
				return this.CurrentAvatarSettings.GetJson(true);
			}
			return string.Empty;
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x00096265 File Offset: 0x00094465
		public string GetClothingString()
		{
			return new ItemSet(this.Clothing.ItemSlots.ToList<ItemSlot>()).GetJSON();
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x00096284 File Offset: 0x00094484
		public virtual void Load(PlayerData data, string containerPath)
		{
			this.Load(data);
			string contentsString;
			if (this.Loader.TryLoadFile(containerPath, "Inventory", out contentsString))
			{
				this.LoadInventory(contentsString);
			}
			else
			{
				Console.LogWarning("Failed to load player inventory under " + containerPath, null);
			}
			string appearanceString;
			if (this.Loader.TryLoadFile(containerPath, "Appearance", out appearanceString))
			{
				this.LoadAppearance(appearanceString);
			}
			else
			{
				Console.LogWarning("Failed to load player appearance under " + containerPath, null);
			}
			string contentsString2;
			if (this.Loader.TryLoadFile(containerPath, "Clothing", out contentsString2))
			{
				this.LoadClothing(contentsString2);
			}
			else
			{
				Console.LogWarning("Failed to load player clothing under " + containerPath, null);
			}
			string path = Path.Combine(containerPath, "Variables");
			if (Directory.Exists(path))
			{
				string[] files = Directory.GetFiles(path);
				PlayerLoader playerLoader = new PlayerLoader();
				for (int i = 0; i < files.Length; i++)
				{
					string json;
					if (playerLoader.TryLoadFile(files[i], out json, false))
					{
						VariableData data2 = null;
						try
						{
							data2 = JsonUtility.FromJson<VariableData>(json);
						}
						catch (Exception ex)
						{
							Debug.LogError("Error loading variable data: " + ex.Message);
						}
						if (data != null)
						{
							this.LoadVariable(data2);
						}
					}
				}
			}
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000963B0 File Offset: 0x000945B0
		public virtual void Load(PlayerData data)
		{
			this.playerDataRetrieveReturned = true;
			if (base.IsOwner)
			{
				PlayerSingleton<PlayerMovement>.Instance.Teleport(data.Position);
				base.transform.eulerAngles = new Vector3(0f, data.Rotation, 0f);
			}
			this.HasCompletedIntro = data.IntroCompleted;
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x00096408 File Offset: 0x00094608
		public virtual void LoadInventory(string contentsString)
		{
			if (string.IsNullOrEmpty(contentsString))
			{
				Console.LogWarning("Empty inventory string", null);
				return;
			}
			if (!base.IsOwner)
			{
				return;
			}
			ItemInstance[] array = ItemSet.Deserialize(contentsString);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] is CashInstance)
				{
					PlayerSingleton<PlayerInventory>.Instance.cashInstance.SetBalance((array[i] as CashInstance).Balance, false);
				}
				else if (i < 8)
				{
					PlayerSingleton<PlayerInventory>.Instance.hotbarSlots[i].SetStoredItem(array[i], false);
				}
				else
				{
					Console.LogWarning("Hotbar slot out of range", null);
				}
			}
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x0009649C File Offset: 0x0009469C
		public virtual void LoadAppearance(string appearanceString)
		{
			if (string.IsNullOrEmpty(appearanceString))
			{
				Console.LogWarning("Empty appearance string", null);
				return;
			}
			BasicAvatarSettings basicAvatarSettings = ScriptableObject.CreateInstance<BasicAvatarSettings>();
			JsonUtility.FromJsonOverwrite(appearanceString, basicAvatarSettings);
			this.SetAppearance(basicAvatarSettings, false);
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000964D4 File Offset: 0x000946D4
		public virtual void LoadClothing(string contentsString)
		{
			if (string.IsNullOrEmpty(contentsString))
			{
				Console.LogWarning("Empty clothing string", null);
				return;
			}
			if (!base.IsOwner)
			{
				return;
			}
			ItemInstance[] array = ItemSet.Deserialize(contentsString);
			for (int i = 0; i < array.Length; i++)
			{
				if (i < this.Clothing.ItemSlots.Count)
				{
					this.Clothing.ItemSlots[i].SetStoredItem(array[i], false);
				}
				else
				{
					Console.LogWarning("Clothing slot out of range", null);
				}
			}
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x00096550 File Offset: 0x00094750
		public void SetRagdolled(bool ragdolled)
		{
			if (ragdolled == this.IsRagdolled)
			{
				return;
			}
			this.IsRagdolled = ragdolled;
			this.Avatar.SetRagdollPhysicsEnabled(ragdolled, false);
			this.Avatar.transform.localEulerAngles = Vector3.zero;
			if (base.IsOwner)
			{
				if (this.IsRagdolled)
				{
					LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Player"));
				}
				else
				{
					LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Invisible"));
				}
			}
			if (this.IsRagdolled)
			{
				if (this.onRagdoll != null)
				{
					this.onRagdoll.Invoke();
					return;
				}
			}
			else if (this.onRagdollEnd != null)
			{
				this.onRagdollEnd.Invoke();
			}
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x00096605 File Offset: 0x00094805
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public virtual void SendImpact(Impact impact)
		{
			this.RpcWriter___Server_SendImpact_427288424(impact);
			this.RpcLogic___SendImpact_427288424(impact);
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x0009661C File Offset: 0x0009481C
		[ObserversRpc(RunLocally = true)]
		public virtual void ReceiveImpact(Impact impact)
		{
			this.RpcWriter___Observers_ReceiveImpact_427288424(impact);
			this.RpcLogic___ReceiveImpact_427288424(impact);
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x0009663D File Offset: 0x0009483D
		public virtual void ProcessImpactForce(Vector3 forcePoint, Vector3 forceDirection, float force)
		{
			if (force >= 50f)
			{
				this.Avatar.Anim.Flinch(forceDirection, AvatarAnimation.EFlinchType.Light);
			}
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x0009665C File Offset: 0x0009485C
		public virtual void OnDied()
		{
			if (base.Owner.IsLocalClient)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
				Singleton<HUD>.Instance.canvas.enabled = false;
				this.ExitAll();
				PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(PlayerSingleton<PlayerCamera>.Instance.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.rotation, 0f, false);
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement("Dead");
			}
			this.ClearProduct();
			this.NameLabel.gameObject.SetActive(false);
			this.CapCol.enabled = false;
			this.SetRagdolled(true);
			this.Avatar.MiddleSpineRB.AddForce(base.transform.forward * 30f, ForceMode.VelocityChange);
			this.Avatar.MiddleSpineRB.AddRelativeTorque(new Vector3(0f, UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * 10f, ForceMode.VelocityChange);
			if (this.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.IsArrested = true;
			}
			if (base.Owner.IsLocalClient)
			{
				Singleton<DeathScreen>.Instance.Open();
			}
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x00096794 File Offset: 0x00094994
		public virtual void OnRevived()
		{
			this.SetRagdolled(false);
			if (!base.Owner.IsLocalClient)
			{
				this.NameLabel.gameObject.SetActive(true);
			}
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, false, false);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement("Dead");
			this.CapCol.enabled = true;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000967F4 File Offset: 0x000949F4
		[ObserversRpc(RunLocally = true)]
		public void Arrest()
		{
			this.RpcWriter___Observers_Arrest_2166136261();
			this.RpcLogic___Arrest_2166136261();
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x00096810 File Offset: 0x00094A10
		public void Free()
		{
			if (!this.IsArrested)
			{
				return;
			}
			if (base.IsOwner)
			{
				Transform transform = Singleton<Map>.Instance.PoliceStation.SpawnPoint;
				if (NetworkSingleton<CurfewManager>.Instance.IsCurrentlyActive)
				{
					if (Player.Local.LastVisitedProperty != null)
					{
						transform = Player.Local.LastVisitedProperty.InteriorSpawnPoint;
					}
					else if (Property.OwnedProperties.Count > 0)
					{
						transform = Property.OwnedProperties[0].InteriorSpawnPoint;
					}
					else
					{
						transform = Property.UnownedProperties[0].InteriorSpawnPoint;
					}
				}
				PlayerSingleton<PlayerMovement>.Instance.Teleport(transform.position + Vector3.up * 1f);
				base.transform.forward = transform.forward;
				Singleton<HUD>.Instance.canvas.enabled = true;
			}
			this.IsArrested = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement("Arrested");
			if (this.onFreed != null)
			{
				this.onFreed.Invoke();
			}
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x00096910 File Offset: 0x00094B10
		[ServerRpc(RunLocally = true)]
		public void SendPassOut()
		{
			this.RpcWriter___Server_SendPassOut_2166136261();
			this.RpcLogic___SendPassOut_2166136261();
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x00096920 File Offset: 0x00094B20
		[ObserversRpc(RunLocally = true, ExcludeOwner = true)]
		public void PassOut()
		{
			this.RpcWriter___Observers_PassOut_2166136261();
			this.RpcLogic___PassOut_2166136261();
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x00096939 File Offset: 0x00094B39
		[ServerRpc(RunLocally = true)]
		public void SendPassOutRecovery()
		{
			this.RpcWriter___Server_SendPassOutRecovery_2166136261();
			this.RpcLogic___SendPassOutRecovery_2166136261();
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00096948 File Offset: 0x00094B48
		[ObserversRpc(RunLocally = true, ExcludeOwner = true)]
		public void PassOutRecovery()
		{
			this.RpcWriter___Observers_PassOutRecovery_2166136261();
			this.RpcLogic___PassOutRecovery_2166136261();
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00096961 File Offset: 0x00094B61
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SendEquippable_Networked(string assetPath)
		{
			this.RpcWriter___Server_SendEquippable_Networked_3615296227(assetPath);
			this.RpcLogic___SendEquippable_Networked_3615296227(assetPath);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00096977 File Offset: 0x00094B77
		[ObserversRpc(RunLocally = true)]
		private void SetEquippable_Networked(string assetPath)
		{
			this.RpcWriter___Observers_SetEquippable_Networked_3615296227(assetPath);
			this.RpcLogic___SetEquippable_Networked_3615296227(assetPath);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x0009698D File Offset: 0x00094B8D
		[ServerRpc(RunLocally = true)]
		public void SendEquippableMessage_Networked(string message, int receipt)
		{
			this.RpcWriter___Server_SendEquippableMessage_Networked_3643459082(message, receipt);
			this.RpcLogic___SendEquippableMessage_Networked_3643459082(message, receipt);
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000969AB File Offset: 0x00094BAB
		[ObserversRpc(RunLocally = true)]
		private void ReceiveEquippableMessage_Networked(string message, int receipt)
		{
			this.RpcWriter___Observers_ReceiveEquippableMessage_Networked_3643459082(message, receipt);
			this.RpcLogic___ReceiveEquippableMessage_Networked_3643459082(message, receipt);
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000969C9 File Offset: 0x00094BC9
		[ServerRpc(RunLocally = true)]
		public void SendEquippableMessage_Networked_Vector(string message, int receipt, Vector3 data)
		{
			this.RpcWriter___Server_SendEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
			this.RpcLogic___SendEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000969EF File Offset: 0x00094BEF
		[ObserversRpc(RunLocally = true)]
		private void ReceiveEquippableMessage_Networked_Vector(string message, int receipt, Vector3 data)
		{
			this.RpcWriter___Observers_ReceiveEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
			this.RpcLogic___ReceiveEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x00096A15 File Offset: 0x00094C15
		[ServerRpc(RunLocally = true)]
		public void SendAnimationTrigger(string trigger)
		{
			this.RpcWriter___Server_SendAnimationTrigger_3615296227(trigger);
			this.RpcLogic___SendAnimationTrigger_3615296227(trigger);
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x00096A2B File Offset: 0x00094C2B
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetAnimationTrigger_Networked(NetworkConnection conn, string trigger)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetAnimationTrigger_Networked_2971853958(conn, trigger);
				this.RpcLogic___SetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
			else
			{
				this.RpcWriter___Target_SetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x00096A61 File Offset: 0x00094C61
		public void SetAnimationTrigger(string trigger)
		{
			this.Avatar.Anim.SetTrigger(trigger);
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x00096A74 File Offset: 0x00094C74
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void ResetAnimationTrigger_Networked(NetworkConnection conn, string trigger)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ResetAnimationTrigger_Networked_2971853958(conn, trigger);
				this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
			else
			{
				this.RpcWriter___Target_ResetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x00096AAA File Offset: 0x00094CAA
		public void ResetAnimationTrigger(string trigger)
		{
			this.Avatar.Anim.ResetTrigger(trigger);
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x00096ABD File Offset: 0x00094CBD
		[ServerRpc(RunLocally = true)]
		public void SendAnimationBool(string name, bool val)
		{
			this.RpcWriter___Server_SendAnimationBool_310431262(name, val);
			this.RpcLogic___SendAnimationBool_310431262(name, val);
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x00096ADB File Offset: 0x00094CDB
		[ObserversRpc(RunLocally = true)]
		public void SetAnimationBool(string name, bool val)
		{
			this.RpcWriter___Observers_SetAnimationBool_310431262(name, val);
			this.RpcLogic___SetAnimationBool_310431262(name, val);
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x00096AFC File Offset: 0x00094CFC
		[ObserversRpc]
		public void Taze()
		{
			this.RpcWriter___Observers_Taze_2166136261();
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x00096B0F File Offset: 0x00094D0F
		[ServerRpc(RunLocally = true)]
		public void SetInventoryItem(int index, ItemInstance item)
		{
			this.RpcWriter___Server_SetInventoryItem_2317364410(index, item);
			this.RpcLogic___SetInventoryItem_2317364410(index, item);
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x00096B30 File Offset: 0x00094D30
		private void GetNetworth(MoneyManager.FloatContainer container)
		{
			for (int i = 0; i < this.Inventory.Length; i++)
			{
				if (this.Inventory[i].ItemInstance != null)
				{
					container.ChangeValue(this.Inventory[i].ItemInstance.GetMonetaryValue());
				}
			}
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x00096B77 File Offset: 0x00094D77
		[ServerRpc(RunLocally = true)]
		public void SendAppearance(BasicAvatarSettings settings)
		{
			this.RpcWriter___Server_SendAppearance_3281254764(settings);
			this.RpcLogic___SendAppearance_3281254764(settings);
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x00096B90 File Offset: 0x00094D90
		[ObserversRpc(RunLocally = true)]
		public void SetAppearance(BasicAvatarSettings settings, bool refreshClothing)
		{
			this.RpcWriter___Observers_SetAppearance_2139595489(settings, refreshClothing);
			this.RpcLogic___SetAppearance_2139595489(settings, refreshClothing);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x00096BBC File Offset: 0x00094DBC
		public void MountSkateboard(Skateboard board)
		{
			this.SendMountedSkateboard(board.NetworkObject);
			foreach (Collider collider in base.GetComponentsInChildren<Collider>(true))
			{
				foreach (Collider collider2 in board.MainColliders)
				{
					Physics.IgnoreCollision(collider, collider2, true);
				}
			}
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(PlayerSingleton<PlayerCamera>.Instance.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.SetCameraMode(PlayerCamera.ECameraMode.Skateboard);
			PlayerSingleton<PlayerCamera>.Instance.transform.position = PlayerSingleton<PlayerCamera>.Instance.transform.transform.position - base.transform.forward * 0.5f;
			this.SetVisibleToLocalPlayer(true);
			this.CapCol.enabled = true;
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerMovement>.Instance.Controller.enabled = false;
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("skateboard");
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x00096CE0 File Offset: 0x00094EE0
		[ServerRpc(RunLocally = true)]
		private void SendMountedSkateboard(NetworkObject skateboardObj)
		{
			this.RpcWriter___Server_SendMountedSkateboard_3323014238(skateboardObj);
			this.RpcLogic___SendMountedSkateboard_3323014238(skateboardObj);
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x00096CF8 File Offset: 0x00094EF8
		[ObserversRpc(RunLocally = true)]
		private void SetMountedSkateboard(NetworkObject skateboardObj)
		{
			this.RpcWriter___Observers_SetMountedSkateboard_3323014238(skateboardObj);
			this.RpcLogic___SetMountedSkateboard_3323014238(skateboardObj);
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x00096D1C File Offset: 0x00094F1C
		public void DismountSkateboard()
		{
			this.SendMountedSkateboard(null);
			this.SetVisibleToLocalPlayer(false);
			this.CapCol.enabled = true;
			this.SetCapsuleColliderHeight(1f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerMovement>.Instance.Controller.enabled = true;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, false);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
			PlayerSingleton<PlayerCamera>.Instance.SetCameraMode(PlayerCamera.ECameraMode.Default);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x00096DB4 File Offset: 0x00094FB4
		public void ConsumeProduct(ProductItemInstance product)
		{
			this.SendConsumeProduct(product);
			this.ConsumeProductInternal(product);
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x00096DC4 File Offset: 0x00094FC4
		[ServerRpc(RequireOwnership = false)]
		private void SendConsumeProduct(ProductItemInstance product)
		{
			this.RpcWriter___Server_SendConsumeProduct_2622925554(product);
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x00096DD0 File Offset: 0x00094FD0
		[ObserversRpc]
		private void ReceiveConsumeProduct(ProductItemInstance product)
		{
			this.RpcWriter___Observers_ReceiveConsumeProduct_2622925554(product);
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x00096DDC File Offset: 0x00094FDC
		private void ConsumeProductInternal(ProductItemInstance product)
		{
			if (this.ConsumedProduct != null)
			{
				this.ClearProduct();
			}
			this.ConsumedProduct = product;
			this.TimeSinceProductConsumed = 0;
			product.ApplyEffectsToPlayer(this);
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x00096E01 File Offset: 0x00095001
		public void ClearProduct()
		{
			if (this.ConsumedProduct != null)
			{
				this.ConsumedProduct.ClearEffectsFromPlayer(this);
				this.ConsumedProduct = null;
			}
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x00096E20 File Offset: 0x00095020
		private void CreatePlayerVariables()
		{
			if (this.VariableDict.Count > 0)
			{
				return;
			}
			Console.Log(string.Concat(new string[]
			{
				"Creating player variables for ",
				this.PlayerName,
				" (",
				this.PlayerCode,
				")"
			}), null);
			NetworkSingleton<VariableDatabase>.Instance.CreatePlayerVariables(this);
			if (InstanceFinder.IsServer)
			{
				this.SetVariableValue("IsServer", true.ToString(), true);
			}
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x00096E9E File Offset: 0x0009509E
		public BaseVariable GetVariable(string variableName)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				return this.VariableDict[variableName];
			}
			Console.LogWarning("Failed to find variable with name: " + variableName, null);
			return null;
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x00096ED8 File Offset: 0x000950D8
		public T GetValue<T>(string variableName)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				return (T)((object)this.VariableDict[variableName].GetValue());
			}
			Console.LogError("Variable with name " + variableName + " does not exist in the database.", null);
			return default(T);
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x00096F31 File Offset: 0x00095131
		public void SetVariableValue(string variableName, string value, bool network = true)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				this.VariableDict[variableName].SetValue(value, network);
				return;
			}
			Console.LogWarning("Failed to find variable with name: " + variableName, null);
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x00096F70 File Offset: 0x00095170
		public void AddVariable(BaseVariable variable)
		{
			if (this.VariableDict.ContainsKey(variable.Name.ToLower()))
			{
				Console.LogError("Variable with name " + variable.Name + " already exists in the database.", null);
				return;
			}
			this.PlayerVariables.Add(variable);
			this.VariableDict.Add(variable.Name.ToLower(), variable);
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x00096FD4 File Offset: 0x000951D4
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendValue(string variableName, string value, bool sendToOwner)
		{
			this.RpcWriter___Server_SendValue_3589193952(variableName, value, sendToOwner);
			this.RpcLogic___SendValue_3589193952(variableName, value, sendToOwner);
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x00096FFA File Offset: 0x000951FA
		[TargetRpc]
		private void ReceiveValue(NetworkConnection conn, string variableName, string value)
		{
			this.RpcWriter___Target_ReceiveValue_3895153758(conn, variableName, value);
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x0009700E File Offset: 0x0009520E
		private void ReceiveValue(string variableName, string value)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				this.VariableDict[variableName].SetValue(value, false);
				return;
			}
			Console.LogWarning("Failed to find player variable with name: " + variableName, null);
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x0009704C File Offset: 0x0009524C
		public void LoadVariable(VariableData data)
		{
			BaseVariable variable = this.GetVariable(data.Name);
			if (variable == null)
			{
				Console.LogWarning("Failed to find variable with name: " + data.Name, null);
				return;
			}
			variable.SetValue(data.Value, true);
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x00097090 File Offset: 0x00095290
		public Player()
		{
			this.<PlayerName>k__BackingField = "Player";
			this.<PlayerCode>k__BackingField = string.Empty;
			this.Scale = 1f;
			this.<CameraPosition>k__BackingField = Vector3.zero;
			this.<CameraRotation>k__BackingField = Quaternion.identity;
			this.Inventory = new ItemSlot[9];
			this.loader = new PlayerLoader();
			this.LocalExtraFiles = new List<string>
			{
				"Inventory",
				"Appearance",
				"Clothing"
			};
			this.LocalExtraFolders = new List<string>
			{
				"Variables"
			};
			this.PlayerVariables = new List<BaseVariable>();
			this.VariableDict = new Dictionary<string, BaseVariable>();
			this.standingScale = 1f;
			this.ragdollForceComponents = new List<ConstantForce>();
			this.impactHistory = new List<int>();
			this.seizureRotations = new List<Quaternion>();
			this.equippableMessageIDHistory = new List<int>();
			base..ctor();
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x000971DB File Offset: 0x000953DB
		[CompilerGenerated]
		private IEnumerator <Taze>g__Tase|319_0()
		{
			this.Avatar.Effects.SetZapped(true, true);
			yield return new WaitForSeconds(2f);
			this.Avatar.Effects.SetZapped(false, true);
			this.IsTased = false;
			if (this.onTasedEnd != null)
			{
				this.onTasedEnd.Invoke();
			}
			yield break;
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x000971EC File Offset: 0x000953EC
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<CameraRotation>k__BackingField = new SyncVar<Quaternion>(this, 6U, WritePermission.ServerOnly, ReadPermission.Observers, 0.1f, Channel.Unreliable, this.<CameraRotation>k__BackingField);
			this.syncVar___<CameraPosition>k__BackingField = new SyncVar<Vector3>(this, 5U, WritePermission.ServerOnly, ReadPermission.Observers, 0.1f, Channel.Unreliable, this.<CameraPosition>k__BackingField);
			this.syncVar___<IsReadyToSleep>k__BackingField = new SyncVar<bool>(this, 4U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<IsReadyToSleep>k__BackingField);
			this.syncVar___<CurrentBed>k__BackingField = new SyncVar<NetworkObject>(this, 3U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentBed>k__BackingField);
			this.syncVar___<CurrentVehicle>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentVehicle>k__BackingField);
			this.syncVar___<CurrentVehicle>k__BackingField.OnChange += this.CurrentVehicleChanged;
			this.syncVar___<PlayerCode>k__BackingField = new SyncVar<string>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<PlayerCode>k__BackingField);
			this.syncVar___<PlayerName>k__BackingField = new SyncVar<string>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<PlayerName>k__BackingField);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_set_CurrentVehicle_3323014238));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_set_CurrentBed_3323014238));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_set_IsSkating_1140765316));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_set_CameraPosition_4276783012));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_set_CameraRotation_3429297120));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_RequestSavePlayer_2166136261));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_ReturnSaveRequest_214505783));
			base.RegisterTargetRpc(7U, new ClientRpcDelegate(this.RpcReader___Target_ReturnSaveRequest_214505783));
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_HostExitedGame_2166136261));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SendPlayerNameData_586648380));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_RequestPlayerData_3615296227));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_MarkPlayerInitialized_2166136261));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePlayerData_3244732873));
			base.RegisterTargetRpc(13U, new ClientRpcDelegate(this.RpcReader___Target_ReceivePlayerData_3244732873));
			base.RegisterObserversRpc(14U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePlayerNameData_3895153758));
			base.RegisterTargetRpc(15U, new ClientRpcDelegate(this.RpcReader___Target_ReceivePlayerNameData_3895153758));
			base.RegisterServerRpc(16U, new ServerRpcDelegate(this.RpcReader___Server_SendFlashlightOnNetworked_1140765316));
			base.RegisterObserversRpc(17U, new ClientRpcDelegate(this.RpcReader___Observers_SetFlashlightOn_1140765316));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_PlayJumpAnimation_2166136261));
			base.RegisterServerRpc(19U, new ServerRpcDelegate(this.RpcReader___Server_SendCrouched_1140765316));
			base.RegisterTargetRpc(20U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveCrouched_214505783));
			base.RegisterObserversRpc(21U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveCrouched_214505783));
			base.RegisterServerRpc(22U, new ServerRpcDelegate(this.RpcReader___Server_SendAvatarSettings_4281687581));
			base.RegisterObserversRpc(23U, new ClientRpcDelegate(this.RpcReader___Observers_SetAvatarSettings_4281687581));
			base.RegisterObserversRpc(24U, new ClientRpcDelegate(this.RpcReader___Observers_SetVisible_Networked_1140765316));
			base.RegisterServerRpc(25U, new ServerRpcDelegate(this.RpcReader___Server_SetReadyToSleep_1140765316));
			base.RegisterObserversRpc(26U, new ClientRpcDelegate(this.RpcReader___Observers_SetPlayerCode_3615296227));
			base.RegisterServerRpc(27U, new ServerRpcDelegate(this.RpcReader___Server_SendPunch_2166136261));
			base.RegisterObserversRpc(28U, new ClientRpcDelegate(this.RpcReader___Observers_Punch_2166136261));
			base.RegisterServerRpc(29U, new ServerRpcDelegate(this.RpcReader___Server_MarkIntroCompleted_3281254764));
			base.RegisterServerRpc(30U, new ServerRpcDelegate(this.RpcReader___Server_SendImpact_427288424));
			base.RegisterObserversRpc(31U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveImpact_427288424));
			base.RegisterObserversRpc(32U, new ClientRpcDelegate(this.RpcReader___Observers_Arrest_2166136261));
			base.RegisterServerRpc(33U, new ServerRpcDelegate(this.RpcReader___Server_SendPassOut_2166136261));
			base.RegisterObserversRpc(34U, new ClientRpcDelegate(this.RpcReader___Observers_PassOut_2166136261));
			base.RegisterServerRpc(35U, new ServerRpcDelegate(this.RpcReader___Server_SendPassOutRecovery_2166136261));
			base.RegisterObserversRpc(36U, new ClientRpcDelegate(this.RpcReader___Observers_PassOutRecovery_2166136261));
			base.RegisterServerRpc(37U, new ServerRpcDelegate(this.RpcReader___Server_SendEquippable_Networked_3615296227));
			base.RegisterObserversRpc(38U, new ClientRpcDelegate(this.RpcReader___Observers_SetEquippable_Networked_3615296227));
			base.RegisterServerRpc(39U, new ServerRpcDelegate(this.RpcReader___Server_SendEquippableMessage_Networked_3643459082));
			base.RegisterObserversRpc(40U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveEquippableMessage_Networked_3643459082));
			base.RegisterServerRpc(41U, new ServerRpcDelegate(this.RpcReader___Server_SendEquippableMessage_Networked_Vector_3190074053));
			base.RegisterObserversRpc(42U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveEquippableMessage_Networked_Vector_3190074053));
			base.RegisterServerRpc(43U, new ServerRpcDelegate(this.RpcReader___Server_SendAnimationTrigger_3615296227));
			base.RegisterObserversRpc(44U, new ClientRpcDelegate(this.RpcReader___Observers_SetAnimationTrigger_Networked_2971853958));
			base.RegisterTargetRpc(45U, new ClientRpcDelegate(this.RpcReader___Target_SetAnimationTrigger_Networked_2971853958));
			base.RegisterObserversRpc(46U, new ClientRpcDelegate(this.RpcReader___Observers_ResetAnimationTrigger_Networked_2971853958));
			base.RegisterTargetRpc(47U, new ClientRpcDelegate(this.RpcReader___Target_ResetAnimationTrigger_Networked_2971853958));
			base.RegisterServerRpc(48U, new ServerRpcDelegate(this.RpcReader___Server_SendAnimationBool_310431262));
			base.RegisterObserversRpc(49U, new ClientRpcDelegate(this.RpcReader___Observers_SetAnimationBool_310431262));
			base.RegisterObserversRpc(50U, new ClientRpcDelegate(this.RpcReader___Observers_Taze_2166136261));
			base.RegisterServerRpc(51U, new ServerRpcDelegate(this.RpcReader___Server_SetInventoryItem_2317364410));
			base.RegisterServerRpc(52U, new ServerRpcDelegate(this.RpcReader___Server_SendAppearance_3281254764));
			base.RegisterObserversRpc(53U, new ClientRpcDelegate(this.RpcReader___Observers_SetAppearance_2139595489));
			base.RegisterServerRpc(54U, new ServerRpcDelegate(this.RpcReader___Server_SendMountedSkateboard_3323014238));
			base.RegisterObserversRpc(55U, new ClientRpcDelegate(this.RpcReader___Observers_SetMountedSkateboard_3323014238));
			base.RegisterServerRpc(56U, new ServerRpcDelegate(this.RpcReader___Server_SendConsumeProduct_2622925554));
			base.RegisterObserversRpc(57U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveConsumeProduct_2622925554));
			base.RegisterServerRpc(58U, new ServerRpcDelegate(this.RpcReader___Server_SendValue_3589193952));
			base.RegisterTargetRpc(59U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveValue_3895153758));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.PlayerScripts.Player));
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x000978C4 File Offset: 0x00095AC4
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<CameraRotation>k__BackingField.SetRegistered();
			this.syncVar___<CameraPosition>k__BackingField.SetRegistered();
			this.syncVar___<IsReadyToSleep>k__BackingField.SetRegistered();
			this.syncVar___<CurrentBed>k__BackingField.SetRegistered();
			this.syncVar___<CurrentVehicle>k__BackingField.SetRegistered();
			this.syncVar___<PlayerCode>k__BackingField.SetRegistered();
			this.syncVar___<PlayerName>k__BackingField.SetRegistered();
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x0009792F File Offset: 0x00095B2F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x00097940 File Offset: 0x00095B40
		private void RpcWriter___Server_set_CurrentVehicle_3323014238(NetworkObject value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(value);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x00097A41 File Offset: 0x00095C41
		public void RpcLogic___set_CurrentVehicle_3323014238(NetworkObject value)
		{
			this.sync___set_value_<CurrentVehicle>k__BackingField(value, true);
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x00097A4C File Offset: 0x00095C4C
		private void RpcReader___Server_set_CurrentVehicle_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject value = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___set_CurrentVehicle_3323014238(value);
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x00097A9C File Offset: 0x00095C9C
		private void RpcWriter___Server_set_CurrentBed_3323014238(NetworkObject value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(value);
			base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x00097B9D File Offset: 0x00095D9D
		public void RpcLogic___set_CurrentBed_3323014238(NetworkObject value)
		{
			this.sync___set_value_<CurrentBed>k__BackingField(value, true);
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x00097BA8 File Offset: 0x00095DA8
		private void RpcReader___Server_set_CurrentBed_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject value = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			this.RpcLogic___set_CurrentBed_3323014238(value);
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x00097BEC File Offset: 0x00095DEC
		private void RpcWriter___Server_set_IsSkating_1140765316(bool value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteBoolean(value);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x00097CED File Offset: 0x00095EED
		public void RpcLogic___set_IsSkating_1140765316(bool value)
		{
			this.<IsSkating>k__BackingField = value;
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x00097CF8 File Offset: 0x00095EF8
		private void RpcReader___Server_set_IsSkating_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			this.RpcLogic___set_IsSkating_1140765316(value);
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x00097D3C File Offset: 0x00095F3C
		private void RpcWriter___Server_set_CameraPosition_4276783012(Vector3 value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteVector3(value);
			base.SendServerRpc(3U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x00097E3D File Offset: 0x0009603D
		public void RpcLogic___set_CameraPosition_4276783012(Vector3 value)
		{
			this.sync___set_value_<CameraPosition>k__BackingField(value, true);
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x00097E48 File Offset: 0x00096048
		private void RpcReader___Server_set_CameraPosition_4276783012(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Vector3 value = PooledReader0.ReadVector3();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			this.RpcLogic___set_CameraPosition_4276783012(value);
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x00097E8C File Offset: 0x0009608C
		private void RpcWriter___Server_set_CameraRotation_3429297120(Quaternion value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteQuaternion(value, AutoPackType.Packed);
			base.SendServerRpc(4U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x00097F92 File Offset: 0x00096192
		public void RpcLogic___set_CameraRotation_3429297120(Quaternion value)
		{
			this.sync___set_value_<CameraRotation>k__BackingField(value, true);
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x00097F9C File Offset: 0x0009619C
		private void RpcReader___Server_set_CameraRotation_3429297120(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Quaternion value = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			this.RpcLogic___set_CameraRotation_3429297120(value);
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x00097FE4 File Offset: 0x000961E4
		private void RpcWriter___Server_RequestSavePlayer_2166136261()
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
			base.SendServerRpc(5U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x0009807E File Offset: 0x0009627E
		public void RpcLogic___RequestSavePlayer_2166136261()
		{
			this.playerSaveRequestReturned = false;
			if (InstanceFinder.IsServer)
			{
				Console.Log("Save request received", null);
				Singleton<PlayerManager>.Instance.SavePlayer(this);
				this.ReturnSaveRequest(base.Owner, true);
			}
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x000980B4 File Offset: 0x000962B4
		private void RpcReader___Server_RequestSavePlayer_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___RequestSavePlayer_2166136261();
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000980E4 File Offset: 0x000962E4
		private void RpcWriter___Observers_ReturnSaveRequest_214505783(NetworkConnection conn, bool successful)
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
			writer.WriteBoolean(successful);
			base.SendObserversRpc(6U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x0009819A File Offset: 0x0009639A
		private void RpcLogic___ReturnSaveRequest_214505783(NetworkConnection conn, bool successful)
		{
			Console.Log("Save request returned. Successful: " + successful.ToString(), null);
			this.playerSaveRequestReturned = true;
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x000981BC File Offset: 0x000963BC
		private void RpcReader___Observers_ReturnSaveRequest_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool successful = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReturnSaveRequest_214505783(null, successful);
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x000981F0 File Offset: 0x000963F0
		private void RpcWriter___Target_ReturnSaveRequest_214505783(NetworkConnection conn, bool successful)
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
			writer.WriteBoolean(successful);
			base.SendTargetRpc(7U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x000982A8 File Offset: 0x000964A8
		private void RpcReader___Target_ReturnSaveRequest_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool successful = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReturnSaveRequest_214505783(base.LocalConnection, successful);
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x000982E0 File Offset: 0x000964E0
		private void RpcWriter___Observers_HostExitedGame_2166136261()
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
			base.SendObserversRpc(8U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x0009838C File Offset: 0x0009658C
		public void RpcLogic___HostExitedGame_2166136261()
		{
			if (InstanceFinder.IsServer)
			{
				return;
			}
			if (Singleton<LoadManager>.InstanceExists && (Singleton<LoadManager>.Instance.IsLoading || !Singleton<LoadManager>.Instance.IsGameLoaded))
			{
				return;
			}
			Console.Log("Host exited game", null);
			Singleton<LoadManager>.Instance.ExitToMenu(null, new MainMenuPopup.Data("Exited Game", "Host left the game", false), false);
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x000983E8 File Offset: 0x000965E8
		private void RpcReader___Observers_HostExitedGame_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___HostExitedGame_2166136261();
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x00098414 File Offset: 0x00096614
		private void RpcWriter___Server_SendPlayerNameData_586648380(string playerName, ulong id)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(playerName);
			writer.WriteUInt64(id, AutoPackType.Packed);
			base.SendServerRpc(9U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x00098528 File Offset: 0x00096728
		public void RpcLogic___SendPlayerNameData_586648380(string playerName, ulong id)
		{
			this.ReceivePlayerNameData(null, playerName, id.ToString());
			if (SteamManager.Initialized && SteamFriends.GetFriendRelationship(new CSteamID(id)) != EFriendRelationship.k_EFriendRelationshipFriend && !base.Owner.IsLocalClient)
			{
				Console.LogError("Player " + playerName + " is not friends with the host. Kicking from game.", null);
				base.Owner.Kick(KickReason.Unset, LoggingType.Warning, "Not friends with host");
				return;
			}
			this.PlayerName = playerName;
			this.PlayerCode = id.ToString();
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000985A4 File Offset: 0x000967A4
		private void RpcReader___Server_SendPlayerNameData_586648380(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string playerName = PooledReader0.ReadString();
			ulong id = PooledReader0.ReadUInt64(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPlayerNameData_586648380(playerName, id);
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x0009860C File Offset: 0x0009680C
		private void RpcWriter___Server_RequestPlayerData_3615296227(string playerCode)
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
			writer.WriteString(playerCode);
			base.SendServerRpc(10U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000986B4 File Offset: 0x000968B4
		public void RpcLogic___RequestPlayerData_3615296227(string playerCode)
		{
			PlayerData playerData;
			string inventoryString;
			string appearanceString;
			string clothigString;
			VariableData[] vars;
			Singleton<PlayerManager>.Instance.TryGetPlayerData(playerCode, out playerData, out inventoryString, out appearanceString, out clothigString, out vars);
			string[] array = new string[5];
			array[0] = "Sending player data for ";
			array[1] = playerCode;
			array[2] = " (";
			int num = 3;
			PlayerData playerData2 = playerData;
			array[num] = ((playerData2 != null) ? playerData2.ToString() : null);
			array[4] = ")";
			Console.Log(string.Concat(array), null);
			this.ReceivePlayerData(null, playerData, inventoryString, appearanceString, clothigString, vars);
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00098724 File Offset: 0x00096924
		private void RpcReader___Server_RequestPlayerData_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string playerCode = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___RequestPlayerData_3615296227(playerCode);
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x00098758 File Offset: 0x00096958
		private void RpcWriter___Server_MarkPlayerInitialized_2166136261()
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(11U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x0009884C File Offset: 0x00096A4C
		public void RpcLogic___MarkPlayerInitialized_2166136261()
		{
			Console.Log(this.PlayerName + " initialized over network", null);
			this.PlayerInitializedOverNetwork = true;
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x0009886C File Offset: 0x00096A6C
		private void RpcReader___Server_MarkPlayerInitialized_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___MarkPlayerInitialized_2166136261();
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000988AC File Offset: 0x00096AAC
		private void RpcWriter___Observers_ReceivePlayerData_3244732873(NetworkConnection conn, PlayerData data, string inventoryString, string appearanceString, string clothigString, VariableData[] vars)
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
			writer.Write___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generated(data);
			writer.WriteString(inventoryString);
			writer.WriteString(appearanceString);
			writer.WriteString(clothigString);
			writer.Write___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generated(vars);
			base.SendObserversRpc(12U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x00098998 File Offset: 0x00096B98
		public void RpcLogic___ReceivePlayerData_3244732873(NetworkConnection conn, PlayerData data, string inventoryString, string appearanceString, string clothigString, VariableData[] vars)
		{
			this.playerDataRetrieveReturned = true;
			if (data != null)
			{
				this.Load(data);
				if (!string.IsNullOrEmpty(inventoryString))
				{
					this.LoadInventory(inventoryString);
				}
				if (!string.IsNullOrEmpty(appearanceString))
				{
					this.LoadAppearance(appearanceString);
				}
				if (!string.IsNullOrEmpty(clothigString))
				{
					this.LoadClothing(clothigString);
				}
			}
			else if (base.IsOwner)
			{
				Console.Log("No player data found for this player; first time joining", null);
			}
			if (base.IsOwner)
			{
				if (vars != null)
				{
					foreach (VariableData data2 in vars)
					{
						this.LoadVariable(data2);
					}
				}
				this.PlayerLoaded();
			}
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x00098A2C File Offset: 0x00096C2C
		private void RpcReader___Observers_ReceivePlayerData_3244732873(PooledReader PooledReader0, Channel channel)
		{
			PlayerData data = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generateds(PooledReader0);
			string inventoryString = PooledReader0.ReadString();
			string appearanceString = PooledReader0.ReadString();
			string clothigString = PooledReader0.ReadString();
			VariableData[] vars = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerData_3244732873(null, data, inventoryString, appearanceString, clothigString, vars);
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x00098AAC File Offset: 0x00096CAC
		private void RpcWriter___Target_ReceivePlayerData_3244732873(NetworkConnection conn, PlayerData data, string inventoryString, string appearanceString, string clothigString, VariableData[] vars)
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
			writer.Write___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generated(data);
			writer.WriteString(inventoryString);
			writer.WriteString(appearanceString);
			writer.WriteString(clothigString);
			writer.Write___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generated(vars);
			base.SendTargetRpc(13U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x00098B98 File Offset: 0x00096D98
		private void RpcReader___Target_ReceivePlayerData_3244732873(PooledReader PooledReader0, Channel channel)
		{
			PlayerData data = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.PlayerDataFishNet.Serializing.Generateds(PooledReader0);
			string inventoryString = PooledReader0.ReadString();
			string appearanceString = PooledReader0.ReadString();
			string clothigString = PooledReader0.ReadString();
			VariableData[] vars = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.VariableData[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerData_3244732873(base.LocalConnection, data, inventoryString, appearanceString, clothigString, vars);
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00098C14 File Offset: 0x00096E14
		private void RpcWriter___Observers_ReceivePlayerNameData_3895153758(NetworkConnection conn, string playerName, string id)
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
			writer.WriteString(playerName);
			writer.WriteString(id);
			base.SendObserversRpc(14U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x00098CD8 File Offset: 0x00096ED8
		private void RpcLogic___ReceivePlayerNameData_3895153758(NetworkConnection conn, string playerName, string id)
		{
			this.PlayerName = playerName;
			this.PlayerCode = id;
			base.gameObject.name = this.PlayerName + " (" + id + ")";
			this.PoI.SetMainText(this.PlayerName);
			this.NameLabel.ShowText(this.PlayerName, 0f);
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x00098D3C File Offset: 0x00096F3C
		private void RpcReader___Observers_ReceivePlayerNameData_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string playerName = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerNameData_3895153758(null, playerName, id);
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x00098D8C File Offset: 0x00096F8C
		private void RpcWriter___Target_ReceivePlayerNameData_3895153758(NetworkConnection conn, string playerName, string id)
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
			writer.WriteString(playerName);
			writer.WriteString(id);
			base.SendTargetRpc(15U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x00098E50 File Offset: 0x00097050
		private void RpcReader___Target_ReceivePlayerNameData_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string playerName = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerNameData_3895153758(base.LocalConnection, playerName, id);
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00098E98 File Offset: 0x00097098
		private void RpcWriter___Server_SendFlashlightOnNetworked_1140765316(bool on)
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
			writer.WriteBoolean(on);
			base.SendServerRpc(16U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x00098F3F File Offset: 0x0009713F
		private void RpcLogic___SendFlashlightOnNetworked_1140765316(bool on)
		{
			this.SetFlashlightOn(on);
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x00098F48 File Offset: 0x00097148
		private void RpcReader___Server_SendFlashlightOnNetworked_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool on = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendFlashlightOnNetworked_1140765316(on);
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x00098F88 File Offset: 0x00097188
		private void RpcWriter___Observers_SetFlashlightOn_1140765316(bool on)
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
			writer.WriteBoolean(on);
			base.SendObserversRpc(17U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x0009903E File Offset: 0x0009723E
		private void RpcLogic___SetFlashlightOn_1140765316(bool on)
		{
			this.ThirdPersonFlashlight.gameObject.SetActive(on && !base.IsOwner);
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x00099060 File Offset: 0x00097260
		private void RpcReader___Observers_SetFlashlightOn_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool on = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetFlashlightOn_1140765316(on);
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x0009909C File Offset: 0x0009729C
		private void RpcWriter___Observers_PlayJumpAnimation_2166136261()
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
			base.SendObserversRpc(18U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x00099145 File Offset: 0x00097345
		public void RpcLogic___PlayJumpAnimation_2166136261()
		{
			this.Anim.Jump();
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x00099154 File Offset: 0x00097354
		private void RpcReader___Observers_PlayJumpAnimation_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___PlayJumpAnimation_2166136261();
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x00099174 File Offset: 0x00097374
		private void RpcWriter___Server_SendCrouched_1140765316(bool crouched)
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
			writer.WriteBoolean(crouched);
			base.SendServerRpc(19U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x0009921B File Offset: 0x0009741B
		public void RpcLogic___SendCrouched_1140765316(bool crouched)
		{
			this.ReceiveCrouched(null, crouched);
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x00099228 File Offset: 0x00097428
		private void RpcReader___Server_SendCrouched_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool crouched = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCrouched_1140765316(crouched);
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x00099268 File Offset: 0x00097468
		private void RpcWriter___Target_ReceiveCrouched_214505783(NetworkConnection conn, bool crouched)
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
			writer.WriteBoolean(crouched);
			base.SendTargetRpc(20U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x0009931D File Offset: 0x0009751D
		private void RpcLogic___ReceiveCrouched_214505783(NetworkConnection conn, bool crouched)
		{
			if (base.Owner.IsLocalClient)
			{
				return;
			}
			this.Crouched = crouched;
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x00099334 File Offset: 0x00097534
		private void RpcReader___Target_ReceiveCrouched_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool crouched = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveCrouched_214505783(base.LocalConnection, crouched);
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x0009936C File Offset: 0x0009756C
		private void RpcWriter___Observers_ReceiveCrouched_214505783(NetworkConnection conn, bool crouched)
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
			writer.WriteBoolean(crouched);
			base.SendObserversRpc(21U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x00099424 File Offset: 0x00097624
		private void RpcReader___Observers_ReceiveCrouched_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool crouched = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveCrouched_214505783(null, crouched);
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x00099460 File Offset: 0x00097660
		private void RpcWriter___Server_SendAvatarSettings_4281687581(AvatarSettings settings)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generated(settings);
			base.SendServerRpc(22U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x00099561 File Offset: 0x00097761
		public void RpcLogic___SendAvatarSettings_4281687581(AvatarSettings settings)
		{
			this.SetAvatarSettings(settings);
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x0009956C File Offset: 0x0009776C
		private void RpcReader___Server_SendAvatarSettings_4281687581(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			AvatarSettings settings = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAvatarSettings_4281687581(settings);
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000995BC File Offset: 0x000977BC
		private void RpcWriter___Observers_SetAvatarSettings_4281687581(AvatarSettings settings)
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
			writer.Write___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generated(settings);
			base.SendObserversRpc(23U, writer, channel, DataOrderType.Default, true, false, false);
			writer.Store();
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x00099672 File Offset: 0x00097872
		public void RpcLogic___SetAvatarSettings_4281687581(AvatarSettings settings)
		{
			this.Avatar.LoadAvatarSettings(settings);
			if (base.IsOwner)
			{
				LayerUtility.SetLayerRecursively(this.Avatar.gameObject, LayerMask.NameToLayer("Invisible"));
			}
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000996A4 File Offset: 0x000978A4
		private void RpcReader___Observers_SetAvatarSettings_4281687581(PooledReader PooledReader0, Channel channel)
		{
			AvatarSettings settings = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.AvatarSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAvatarSettings_4281687581(settings);
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000996E0 File Offset: 0x000978E0
		private void RpcWriter___Observers_SetVisible_Networked_1140765316(bool vis)
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
			writer.WriteBoolean(vis);
			base.SendObserversRpc(24U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x00099796 File Offset: 0x00097996
		private void RpcLogic___SetVisible_Networked_1140765316(bool vis)
		{
			this.Avatar.SetVisible(vis);
			this.CapCol.enabled = vis;
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000997B0 File Offset: 0x000979B0
		private void RpcReader___Observers_SetVisible_Networked_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool vis = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetVisible_Networked_1140765316(vis);
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000997E4 File Offset: 0x000979E4
		private void RpcWriter___Server_SetReadyToSleep_1140765316(bool ready)
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
			writer.WriteBoolean(ready);
			base.SendServerRpc(25U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x0009988B File Offset: 0x00097A8B
		public void RpcLogic___SetReadyToSleep_1140765316(bool ready)
		{
			this.IsReadyToSleep = ready;
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x00099894 File Offset: 0x00097A94
		private void RpcReader___Server_SetReadyToSleep_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool ready = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetReadyToSleep_1140765316(ready);
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000998D4 File Offset: 0x00097AD4
		private void RpcWriter___Observers_SetPlayerCode_3615296227(string code)
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
			writer.WriteString(code);
			base.SendObserversRpc(26U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x0009998A File Offset: 0x00097B8A
		public void RpcLogic___SetPlayerCode_3615296227(string code)
		{
			this.PlayerCode = code;
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x00099994 File Offset: 0x00097B94
		private void RpcReader___Observers_SetPlayerCode_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string code = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetPlayerCode_3615296227(code);
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000999D0 File Offset: 0x00097BD0
		private void RpcWriter___Server_SendPunch_2166136261()
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(27U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x00099AC4 File Offset: 0x00097CC4
		public void RpcLogic___SendPunch_2166136261()
		{
			this.Punch();
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x00099ACC File Offset: 0x00097CCC
		private void RpcReader___Server_SendPunch_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			this.RpcLogic___SendPunch_2166136261();
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x00099B00 File Offset: 0x00097D00
		private void RpcWriter___Observers_Punch_2166136261()
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
			base.SendObserversRpc(28U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x00099BA9 File Offset: 0x00097DA9
		private void RpcLogic___Punch_2166136261()
		{
			this.Avatar.Anim.SetTrigger("Punch");
			if (!base.IsOwner)
			{
				this.PunchSound.Play();
			}
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x00099BD4 File Offset: 0x00097DD4
		private void RpcReader___Observers_Punch_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Punch_2166136261();
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x00099BF4 File Offset: 0x00097DF4
		private void RpcWriter___Server_MarkIntroCompleted_3281254764(BasicAvatarSettings appearance)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generated(appearance);
			base.SendServerRpc(29U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x00099CF5 File Offset: 0x00097EF5
		private void RpcLogic___MarkIntroCompleted_3281254764(BasicAvatarSettings appearance)
		{
			this.HasCompletedIntro = true;
			Console.Log(this.PlayerName + " has completed intro", null);
			this.SetAppearance(appearance, false);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x00099D1C File Offset: 0x00097F1C
		private void RpcReader___Server_MarkIntroCompleted_3281254764(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			BasicAvatarSettings appearance = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___MarkIntroCompleted_3281254764(appearance);
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x00099D6C File Offset: 0x00097F6C
		private void RpcWriter___Server_SendImpact_427288424(Impact impact)
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
			writer.Write___ScheduleOne.Combat.ImpactFishNet.Serializing.Generated(impact);
			base.SendServerRpc(30U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x00099E13 File Offset: 0x00098013
		public virtual void RpcLogic___SendImpact_427288424(Impact impact)
		{
			this.ReceiveImpact(impact);
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x00099E1C File Offset: 0x0009801C
		private void RpcReader___Server_SendImpact_427288424(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Impact impact = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Combat.ImpactFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendImpact_427288424(impact);
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x00099E5C File Offset: 0x0009805C
		private void RpcWriter___Observers_ReceiveImpact_427288424(Impact impact)
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
			writer.Write___ScheduleOne.Combat.ImpactFishNet.Serializing.Generated(impact);
			base.SendObserversRpc(31U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x00099F14 File Offset: 0x00098114
		public virtual void RpcLogic___ReceiveImpact_427288424(Impact impact)
		{
			if (this.impactHistory.Contains(impact.ImpactID))
			{
				return;
			}
			this.impactHistory.Add(impact.ImpactID);
			float num = 1f;
			this.Health.TakeDamage(impact.ImpactDamage, true, impact.ImpactDamage > 0f);
			if (impact.ImpactType == EImpactType.Punch)
			{
				Singleton<SFXManager>.Instance.PlayImpactSound(ImpactSoundEntity.EMaterial.Punch, impact.HitPoint, impact.ImpactForce);
			}
			else if (impact.ImpactType == EImpactType.BluntMetal)
			{
				Singleton<SFXManager>.Instance.PlayImpactSound(ImpactSoundEntity.EMaterial.BaseballBat, impact.HitPoint, impact.ImpactForce);
			}
			this.ProcessImpactForce(impact.HitPoint, impact.ImpactForceDirection, impact.ImpactForce * num);
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x00099FC8 File Offset: 0x000981C8
		private void RpcReader___Observers_ReceiveImpact_427288424(PooledReader PooledReader0, Channel channel)
		{
			Impact impact = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Combat.ImpactFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveImpact_427288424(impact);
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x0009A004 File Offset: 0x00098204
		private void RpcWriter___Observers_Arrest_2166136261()
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
			base.SendObserversRpc(32U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x0009A0B0 File Offset: 0x000982B0
		public void RpcLogic___Arrest_2166136261()
		{
			if (this.IsArrested)
			{
				return;
			}
			if (this.onArrested != null)
			{
				this.onArrested.Invoke();
			}
			this.IsArrested = true;
			Debug.Log("Player arrested");
			if (!this.Health.IsAlive)
			{
				return;
			}
			if (base.IsOwner)
			{
				this.ExitAll();
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement("Arrested");
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.LONG_ARM_OF_THE_LAW);
				Singleton<ArrestScreen>.Instance.Open();
			}
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x0009A12C File Offset: 0x0009832C
		private void RpcReader___Observers_Arrest_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Arrest_2166136261();
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x0009A158 File Offset: 0x00098358
		private void RpcWriter___Server_SendPassOut_2166136261()
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(33U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x0009A24C File Offset: 0x0009844C
		public void RpcLogic___SendPassOut_2166136261()
		{
			this.PassOut();
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x0009A254 File Offset: 0x00098454
		private void RpcReader___Server_SendPassOut_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPassOut_2166136261();
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x0009A294 File Offset: 0x00098494
		private void RpcWriter___Observers_PassOut_2166136261()
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
			base.SendObserversRpc(34U, writer, channel, DataOrderType.Default, false, false, true);
			writer.Store();
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x0009A340 File Offset: 0x00098540
		public void RpcLogic___PassOut_2166136261()
		{
			this.IsUnconscious = true;
			if (this.onPassedOut != null)
			{
				this.onPassedOut.Invoke();
			}
			this.CapCol.enabled = false;
			this.SetRagdolled(true);
			this.Avatar.MiddleSpineRB.AddForce(base.transform.forward * 30f, ForceMode.VelocityChange);
			this.Avatar.MiddleSpineRB.AddRelativeTorque(new Vector3(0f, UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * 10f, ForceMode.VelocityChange);
			if (!this.Health.IsAlive)
			{
				return;
			}
			if (this.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.IsArrested = true;
			}
			if (base.IsOwner)
			{
				this.ExitAll();
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement("Passed out");
				Singleton<PassOutScreen>.Instance.Open();
			}
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x0009A42C File Offset: 0x0009862C
		private void RpcReader___Observers_PassOut_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PassOut_2166136261();
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x0009A458 File Offset: 0x00098658
		private void RpcWriter___Server_SendPassOutRecovery_2166136261()
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(35U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x0009A54C File Offset: 0x0009874C
		public void RpcLogic___SendPassOutRecovery_2166136261()
		{
			this.PassOutRecovery();
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x0009A554 File Offset: 0x00098754
		private void RpcReader___Server_SendPassOutRecovery_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPassOutRecovery_2166136261();
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x0009A594 File Offset: 0x00098794
		private void RpcWriter___Observers_PassOutRecovery_2166136261()
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
			base.SendObserversRpc(36U, writer, channel, DataOrderType.Default, false, false, true);
			writer.Store();
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x0009A640 File Offset: 0x00098840
		public void RpcLogic___PassOutRecovery_2166136261()
		{
			Debug.Log("Player recovered from pass out");
			this.IsUnconscious = false;
			this.SetRagdolled(false);
			this.CapCol.enabled = true;
			if (base.IsOwner)
			{
				Singleton<HUD>.Instance.canvas.enabled = true;
				this.Energy.RestoreEnergy();
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement("Passed out");
			}
			if (this.onPassOutRecovery != null)
			{
				this.onPassOutRecovery.Invoke();
			}
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x0009A6B8 File Offset: 0x000988B8
		private void RpcReader___Observers_PassOutRecovery_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PassOutRecovery_2166136261();
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x0009A6E4 File Offset: 0x000988E4
		private void RpcWriter___Server_SendEquippable_Networked_3615296227(string assetPath)
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
			writer.WriteString(assetPath);
			base.SendServerRpc(37U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x0009A78B File Offset: 0x0009898B
		public void RpcLogic___SendEquippable_Networked_3615296227(string assetPath)
		{
			this.SetEquippable_Networked(assetPath);
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x0009A794 File Offset: 0x00098994
		private void RpcReader___Server_SendEquippable_Networked_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string assetPath = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEquippable_Networked_3615296227(assetPath);
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x0009A7D4 File Offset: 0x000989D4
		private void RpcWriter___Observers_SetEquippable_Networked_3615296227(string assetPath)
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
			writer.WriteString(assetPath);
			base.SendObserversRpc(38U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x0009A88A File Offset: 0x00098A8A
		private void RpcLogic___SetEquippable_Networked_3615296227(string assetPath)
		{
			this.Avatar.SetEquippable(assetPath);
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x0009A89C File Offset: 0x00098A9C
		private void RpcReader___Observers_SetEquippable_Networked_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string assetPath = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetEquippable_Networked_3615296227(assetPath);
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x0009A8D8 File Offset: 0x00098AD8
		private void RpcWriter___Server_SendEquippableMessage_Networked_3643459082(string message, int receipt)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(message);
			writer.WriteInt32(receipt, AutoPackType.Packed);
			base.SendServerRpc(39U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x0009A9EB File Offset: 0x00098BEB
		public void RpcLogic___SendEquippableMessage_Networked_3643459082(string message, int receipt)
		{
			this.ReceiveEquippableMessage_Networked(message, receipt);
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x0009A9F8 File Offset: 0x00098BF8
		private void RpcReader___Server_SendEquippableMessage_Networked_3643459082(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string message = PooledReader0.ReadString();
			int receipt = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_3643459082(message, receipt);
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x0009AA60 File Offset: 0x00098C60
		private void RpcWriter___Observers_ReceiveEquippableMessage_Networked_3643459082(string message, int receipt)
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
			writer.WriteString(message);
			writer.WriteInt32(receipt, AutoPackType.Packed);
			base.SendObserversRpc(40U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x0009AB28 File Offset: 0x00098D28
		private void RpcLogic___ReceiveEquippableMessage_Networked_3643459082(string message, int receipt)
		{
			if (this.equippableMessageIDHistory.Contains(receipt))
			{
				return;
			}
			this.equippableMessageIDHistory.Add(receipt);
			this.Avatar.ReceiveEquippableMessage(message, null);
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x0009AB54 File Offset: 0x00098D54
		private void RpcReader___Observers_ReceiveEquippableMessage_Networked_3643459082(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			int receipt = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveEquippableMessage_Networked_3643459082(message, receipt);
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x0009ABA8 File Offset: 0x00098DA8
		private void RpcWriter___Server_SendEquippableMessage_Networked_Vector_3190074053(string message, int receipt, Vector3 data)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(message);
			writer.WriteInt32(receipt, AutoPackType.Packed);
			writer.WriteVector3(data);
			base.SendServerRpc(41U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x0009ACC8 File Offset: 0x00098EC8
		public void RpcLogic___SendEquippableMessage_Networked_Vector_3190074053(string message, int receipt, Vector3 data)
		{
			this.ReceiveEquippableMessage_Networked_Vector(message, receipt, data);
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x0009ACD4 File Offset: 0x00098ED4
		private void RpcReader___Server_SendEquippableMessage_Networked_Vector_3190074053(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string message = PooledReader0.ReadString();
			int receipt = PooledReader0.ReadInt32(AutoPackType.Packed);
			Vector3 data = PooledReader0.ReadVector3();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x0009AD4C File Offset: 0x00098F4C
		private void RpcWriter___Observers_ReceiveEquippableMessage_Networked_Vector_3190074053(string message, int receipt, Vector3 data)
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
			writer.WriteString(message);
			writer.WriteInt32(receipt, AutoPackType.Packed);
			writer.WriteVector3(data);
			base.SendObserversRpc(42U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x0009AE21 File Offset: 0x00099021
		private void RpcLogic___ReceiveEquippableMessage_Networked_Vector_3190074053(string message, int receipt, Vector3 data)
		{
			if (this.equippableMessageIDHistory.Contains(receipt))
			{
				return;
			}
			this.equippableMessageIDHistory.Add(receipt);
			this.Avatar.ReceiveEquippableMessage(message, data);
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x0009AE50 File Offset: 0x00099050
		private void RpcReader___Observers_ReceiveEquippableMessage_Networked_Vector_3190074053(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			int receipt = PooledReader0.ReadInt32(AutoPackType.Packed);
			Vector3 data = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveEquippableMessage_Networked_Vector_3190074053(message, receipt, data);
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x0009AEB4 File Offset: 0x000990B4
		private void RpcWriter___Server_SendAnimationTrigger_3615296227(string trigger)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(trigger);
			base.SendServerRpc(43U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x0009AFB5 File Offset: 0x000991B5
		public void RpcLogic___SendAnimationTrigger_3615296227(string trigger)
		{
			this.SetAnimationTrigger_Networked(null, trigger);
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x0009AFC0 File Offset: 0x000991C0
		private void RpcReader___Server_SendAnimationTrigger_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAnimationTrigger_3615296227(trigger);
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x0009B010 File Offset: 0x00099210
		private void RpcWriter___Observers_SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendObserversRpc(44U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x0009B0C6 File Offset: 0x000992C6
		public void RpcLogic___SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			this.SetAnimationTrigger(trigger);
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x0009B0D0 File Offset: 0x000992D0
		private void RpcReader___Observers_SetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAnimationTrigger_Networked_2971853958(null, trigger);
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x0009B10C File Offset: 0x0009930C
		private void RpcWriter___Target_SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendTargetRpc(45U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x0009B1C4 File Offset: 0x000993C4
		private void RpcReader___Target_SetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetAnimationTrigger_Networked_2971853958(base.LocalConnection, trigger);
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x0009B1FC File Offset: 0x000993FC
		private void RpcWriter___Observers_ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendObserversRpc(46U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x0009B2B2 File Offset: 0x000994B2
		public void RpcLogic___ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			this.ResetAnimationTrigger(trigger);
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x0009B2BC File Offset: 0x000994BC
		private void RpcReader___Observers_ResetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(null, trigger);
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x0009B2F8 File Offset: 0x000994F8
		private void RpcWriter___Target_ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendTargetRpc(47U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x0009B3B0 File Offset: 0x000995B0
		private void RpcReader___Target_ResetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(base.LocalConnection, trigger);
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x0009B3E8 File Offset: 0x000995E8
		private void RpcWriter___Server_SendAnimationBool_310431262(string name, bool val)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteBoolean(val);
			base.SendServerRpc(48U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x0009B4F6 File Offset: 0x000996F6
		public void RpcLogic___SendAnimationBool_310431262(string name, bool val)
		{
			this.SetAnimationBool(name, val);
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x0009B500 File Offset: 0x00099700
		private void RpcReader___Server_SendAnimationBool_310431262(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string name = PooledReader0.ReadString();
			bool val = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAnimationBool_310431262(name, val);
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x0009B560 File Offset: 0x00099760
		private void RpcWriter___Observers_SetAnimationBool_310431262(string name, bool val)
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
			writer.WriteString(name);
			writer.WriteBoolean(val);
			base.SendObserversRpc(49U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x0009B623 File Offset: 0x00099823
		public void RpcLogic___SetAnimationBool_310431262(string name, bool val)
		{
			this.Avatar.Anim.SetBool(name, val);
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x0009B638 File Offset: 0x00099838
		private void RpcReader___Observers_SetAnimationBool_310431262(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			bool val = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAnimationBool_310431262(name, val);
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x0009B684 File Offset: 0x00099884
		private void RpcWriter___Observers_Taze_2166136261()
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
			base.SendObserversRpc(50U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x0009B730 File Offset: 0x00099930
		public void RpcLogic___Taze_2166136261()
		{
			this.IsTased = true;
			if (this.onTased != null)
			{
				this.onTased.Invoke();
			}
			if (this.taseCoroutine != null)
			{
				base.StopCoroutine(this.taseCoroutine);
			}
			this.taseCoroutine = base.StartCoroutine(this.<Taze>g__Tase|319_0());
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x0009B780 File Offset: 0x00099980
		private void RpcReader___Observers_Taze_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Taze_2166136261();
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x0009B7A0 File Offset: 0x000999A0
		private void RpcWriter___Server_SetInventoryItem_2317364410(int index, ItemInstance item)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(index, AutoPackType.Packed);
			writer.WriteItemInstance(item);
			base.SendServerRpc(51U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x0009B8B3 File Offset: 0x00099AB3
		public void RpcLogic___SetInventoryItem_2317364410(int index, ItemInstance item)
		{
			this.Inventory[index].SetStoredItem(item, false);
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x0009B8C4 File Offset: 0x00099AC4
		private void RpcReader___Server_SetInventoryItem_2317364410(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int index = PooledReader0.ReadInt32(AutoPackType.Packed);
			ItemInstance item = PooledReader0.ReadItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetInventoryItem_2317364410(index, item);
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x0009B92C File Offset: 0x00099B2C
		private void RpcWriter___Server_SendAppearance_3281254764(BasicAvatarSettings settings)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generated(settings);
			base.SendServerRpc(52U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x0009BA2D File Offset: 0x00099C2D
		public void RpcLogic___SendAppearance_3281254764(BasicAvatarSettings settings)
		{
			this.SetAppearance(settings, true);
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x0009BA38 File Offset: 0x00099C38
		private void RpcReader___Server_SendAppearance_3281254764(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			BasicAvatarSettings settings = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAppearance_3281254764(settings);
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x0009BA88 File Offset: 0x00099C88
		private void RpcWriter___Observers_SetAppearance_2139595489(BasicAvatarSettings settings, bool refreshClothing)
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
			writer.Write___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generated(settings);
			writer.WriteBoolean(refreshClothing);
			base.SendObserversRpc(53U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x0009BB4C File Offset: 0x00099D4C
		public void RpcLogic___SetAppearance_2139595489(BasicAvatarSettings settings, bool refreshClothing)
		{
			this.CurrentAvatarSettings = settings;
			AvatarSettings avatarSettings = this.CurrentAvatarSettings.GetAvatarSettings();
			this.Avatar.LoadAvatarSettings(avatarSettings);
			if (refreshClothing)
			{
				this.Clothing.RefreshAppearance();
			}
			this.SetVisibleToLocalPlayer(!base.IsOwner);
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x0009BB98 File Offset: 0x00099D98
		private void RpcReader___Observers_SetAppearance_2139595489(PooledReader PooledReader0, Channel channel)
		{
			BasicAvatarSettings settings = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.AvatarFramework.Customization.BasicAvatarSettingsFishNet.Serializing.Generateds(PooledReader0);
			bool refreshClothing = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAppearance_2139595489(settings, refreshClothing);
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x0009BBE4 File Offset: 0x00099DE4
		private void RpcWriter___Server_SendMountedSkateboard_3323014238(NetworkObject skateboardObj)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(skateboardObj);
			base.SendServerRpc(54U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x0009BCE5 File Offset: 0x00099EE5
		private void RpcLogic___SendMountedSkateboard_3323014238(NetworkObject skateboardObj)
		{
			this.SetMountedSkateboard(skateboardObj);
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x0009BCF0 File Offset: 0x00099EF0
		private void RpcReader___Server_SendMountedSkateboard_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject skateboardObj = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMountedSkateboard_3323014238(skateboardObj);
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x0009BD40 File Offset: 0x00099F40
		private void RpcWriter___Observers_SetMountedSkateboard_3323014238(NetworkObject skateboardObj)
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
			writer.WriteNetworkObject(skateboardObj);
			base.SendObserversRpc(55U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x0009BDF8 File Offset: 0x00099FF8
		private void RpcLogic___SetMountedSkateboard_3323014238(NetworkObject skateboardObj)
		{
			if (skateboardObj != null)
			{
				if (this.ActiveSkateboard != null)
				{
					return;
				}
				Skateboard component = skateboardObj.GetComponent<Skateboard>();
				this.IsSkating = true;
				this.ActiveSkateboard = component;
				base.transform.SetParent(component.PlayerContainer);
				base.transform.localPosition = Vector3.zero;
				base.transform.localRotation = Quaternion.identity;
				if (this.onSkateboardMounted != null)
				{
					this.onSkateboardMounted(component);
					return;
				}
			}
			else
			{
				if (this.ActiveSkateboard == null)
				{
					return;
				}
				this.IsSkating = false;
				this.ActiveSkateboard = null;
				base.transform.SetParent(null);
				base.transform.rotation = Quaternion.LookRotation(base.transform.forward, Vector3.up);
				base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
				if (this.onSkateboardDismounted != null)
				{
					this.onSkateboardDismounted();
				}
			}
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x0009BF04 File Offset: 0x0009A104
		private void RpcReader___Observers_SetMountedSkateboard_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject skateboardObj = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetMountedSkateboard_3323014238(skateboardObj);
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x0009BF40 File Offset: 0x0009A140
		private void RpcWriter___Server_SendConsumeProduct_2622925554(ProductItemInstance product)
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
			base.SendServerRpc(56U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x0009BFE7 File Offset: 0x0009A1E7
		private void RpcLogic___SendConsumeProduct_2622925554(ProductItemInstance product)
		{
			this.ReceiveConsumeProduct(product);
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x0009BFF0 File Offset: 0x0009A1F0
		private void RpcReader___Server_SendConsumeProduct_2622925554(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ProductItemInstance product = PooledReader0.ReadProductItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendConsumeProduct_2622925554(product);
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x0009C024 File Offset: 0x0009A224
		private void RpcWriter___Observers_ReceiveConsumeProduct_2622925554(ProductItemInstance product)
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
			writer.WriteProductItemInstance(product);
			base.SendObserversRpc(57U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x0009C0DA File Offset: 0x0009A2DA
		private void RpcLogic___ReceiveConsumeProduct_2622925554(ProductItemInstance product)
		{
			if (base.IsOwner)
			{
				return;
			}
			this.ConsumeProductInternal(product);
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x0009C0EC File Offset: 0x0009A2EC
		private void RpcReader___Observers_ReceiveConsumeProduct_2622925554(PooledReader PooledReader0, Channel channel)
		{
			ProductItemInstance product = PooledReader0.ReadProductItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveConsumeProduct_2622925554(product);
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x0009C120 File Offset: 0x0009A320
		private void RpcWriter___Server_SendValue_3589193952(string variableName, string value, bool sendToOwner)
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
			writer.WriteString(variableName);
			writer.WriteString(value);
			writer.WriteBoolean(sendToOwner);
			base.SendServerRpc(58U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x0009C1E1 File Offset: 0x0009A3E1
		public void RpcLogic___SendValue_3589193952(string variableName, string value, bool sendToOwner)
		{
			if (sendToOwner || !base.IsOwner)
			{
				this.ReceiveValue(variableName, value);
			}
			if (sendToOwner)
			{
				this.ReceiveValue(base.Owner, variableName, value);
			}
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x0009C208 File Offset: 0x0009A408
		private void RpcReader___Server_SendValue_3589193952(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string variableName = PooledReader0.ReadString();
			string value = PooledReader0.ReadString();
			bool sendToOwner = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendValue_3589193952(variableName, value, sendToOwner);
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x0009C268 File Offset: 0x0009A468
		private void RpcWriter___Target_ReceiveValue_3895153758(NetworkConnection conn, string variableName, string value)
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
			writer.WriteString(variableName);
			writer.WriteString(value);
			base.SendTargetRpc(59U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x0009C32A File Offset: 0x0009A52A
		private void RpcLogic___ReceiveValue_3895153758(NetworkConnection conn, string variableName, string value)
		{
			this.ReceiveValue(variableName, value);
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x0009C334 File Offset: 0x0009A534
		private void RpcReader___Target_ReceiveValue_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string variableName = PooledReader0.ReadString();
			string value = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveValue_3895153758(base.LocalConnection, variableName, value);
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06002658 RID: 9816 RVA: 0x0009C37C File Offset: 0x0009A57C
		// (set) Token: 0x06002659 RID: 9817 RVA: 0x0009C384 File Offset: 0x0009A584
		public string SyncAccessor_<PlayerName>k__BackingField
		{
			get
			{
				return this.<PlayerName>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<PlayerName>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<PlayerName>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x0009C3C0 File Offset: 0x0009A5C0
		public virtual bool Player(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 6U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CameraRotation>k__BackingField(this.syncVar___<CameraRotation>k__BackingField.GetValue(true), true);
					return true;
				}
				Quaternion value = PooledReader0.ReadQuaternion(AutoPackType.Packed);
				this.sync___set_value_<CameraRotation>k__BackingField(value, Boolean2);
				return true;
			}
			else if (UInt321 == 5U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CameraPosition>k__BackingField(this.syncVar___<CameraPosition>k__BackingField.GetValue(true), true);
					return true;
				}
				Vector3 value2 = PooledReader0.ReadVector3();
				this.sync___set_value_<CameraPosition>k__BackingField(value2, Boolean2);
				return true;
			}
			else if (UInt321 == 4U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<IsReadyToSleep>k__BackingField(this.syncVar___<IsReadyToSleep>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value3 = PooledReader0.ReadBoolean();
				this.sync___set_value_<IsReadyToSleep>k__BackingField(value3, Boolean2);
				return true;
			}
			else if (UInt321 == 3U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CurrentBed>k__BackingField(this.syncVar___<CurrentBed>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value4 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<CurrentBed>k__BackingField(value4, Boolean2);
				return true;
			}
			else if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CurrentVehicle>k__BackingField(this.syncVar___<CurrentVehicle>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value5 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<CurrentVehicle>k__BackingField(value5, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<PlayerCode>k__BackingField(this.syncVar___<PlayerCode>k__BackingField.GetValue(true), true);
					return true;
				}
				string value6 = PooledReader0.ReadString();
				this.sync___set_value_<PlayerCode>k__BackingField(value6, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 0U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<PlayerName>k__BackingField(this.syncVar___<PlayerName>k__BackingField.GetValue(true), true);
					return true;
				}
				string value7 = PooledReader0.ReadString();
				this.sync___set_value_<PlayerName>k__BackingField(value7, Boolean2);
				return true;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x0600265B RID: 9819 RVA: 0x0009C5AF File Offset: 0x0009A7AF
		// (set) Token: 0x0600265C RID: 9820 RVA: 0x0009C5B7 File Offset: 0x0009A7B7
		public string SyncAccessor_<PlayerCode>k__BackingField
		{
			get
			{
				return this.<PlayerCode>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<PlayerCode>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<PlayerCode>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x0600265D RID: 9821 RVA: 0x0009C5F3 File Offset: 0x0009A7F3
		// (set) Token: 0x0600265E RID: 9822 RVA: 0x0009C5FB File Offset: 0x0009A7FB
		public NetworkObject SyncAccessor_<CurrentVehicle>k__BackingField
		{
			get
			{
				return this.<CurrentVehicle>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CurrentVehicle>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CurrentVehicle>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x0600265F RID: 9823 RVA: 0x0009C637 File Offset: 0x0009A837
		// (set) Token: 0x06002660 RID: 9824 RVA: 0x0009C63F File Offset: 0x0009A83F
		public NetworkObject SyncAccessor_<CurrentBed>k__BackingField
		{
			get
			{
				return this.<CurrentBed>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CurrentBed>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CurrentBed>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06002661 RID: 9825 RVA: 0x0009C67B File Offset: 0x0009A87B
		// (set) Token: 0x06002662 RID: 9826 RVA: 0x0009C683 File Offset: 0x0009A883
		public bool SyncAccessor_<IsReadyToSleep>k__BackingField
		{
			get
			{
				return this.<IsReadyToSleep>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<IsReadyToSleep>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<IsReadyToSleep>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002663 RID: 9827 RVA: 0x0009C6BF File Offset: 0x0009A8BF
		// (set) Token: 0x06002664 RID: 9828 RVA: 0x0009C6C7 File Offset: 0x0009A8C7
		public Vector3 SyncAccessor_<CameraPosition>k__BackingField
		{
			get
			{
				return this.<CameraPosition>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CameraPosition>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CameraPosition>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06002665 RID: 9829 RVA: 0x0009C703 File Offset: 0x0009A903
		// (set) Token: 0x06002666 RID: 9830 RVA: 0x0009C70B File Offset: 0x0009A90B
		public Quaternion SyncAccessor_<CameraRotation>k__BackingField
		{
			get
			{
				return this.<CameraRotation>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CameraRotation>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CameraRotation>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x0009C748 File Offset: 0x0009A948
		protected virtual void dll()
		{
			if (InstanceFinder.NetworkManager == null)
			{
				Player.Local = this;
			}
			ScheduleOne.GameTime.TimeManager.onSleepStart = (Action)Delegate.Combine(ScheduleOne.GameTime.TimeManager.onSleepStart, new Action(this.SleepStart));
			ScheduleOne.GameTime.TimeManager.onSleepEnd = (Action<int>)Delegate.Combine(ScheduleOne.GameTime.TimeManager.onSleepEnd, new Action<int>(this.SleepEnd));
			this.Health.onDie.AddListener(new UnityAction(this.OnDied));
			this.Health.onRevive.AddListener(new UnityAction(this.OnRevived));
			this.Energy.onEnergyDepleted.AddListener(new UnityAction(this.SendPassOut));
			base.InvokeRepeating("RecalculateCurrentProperty", 0f, 0.5f);
			this.InitializeSaveable();
			this.Inventory = new ItemSlot[9];
			for (int i = 0; i < this.Inventory.Length; i++)
			{
				this.Inventory[i] = new ItemSlot();
			}
			foreach (Rigidbody rigidbody in this.Avatar.RagdollRBs)
			{
				Physics.IgnoreCollision(rigidbody.GetComponent<Collider>(), this.CapCol, true);
				this.ragdollForceComponents.Add(rigidbody.gameObject.AddComponent<ConstantForce>());
			}
			this.EyePosition = this.Avatar.Eyes.transform.position;
			this.SetGravityMultiplier(1f);
		}

		// Token: 0x04001B7D RID: 7037
		public const string OWNER_PLAYER_CODE = "Local";

		// Token: 0x04001B7E RID: 7038
		public const float CapColDefaultHeight = 2f;

		// Token: 0x04001B7F RID: 7039
		public List<NetworkObject> objectsTemporarilyOwnedByPlayer = new List<NetworkObject>();

		// Token: 0x04001B80 RID: 7040
		public static Action onLocalPlayerSpawned;

		// Token: 0x04001B81 RID: 7041
		public static Action<Player> onPlayerSpawned;

		// Token: 0x04001B82 RID: 7042
		public static Action<Player> onPlayerDespawned;

		// Token: 0x04001B83 RID: 7043
		public static Player Local;

		// Token: 0x04001B84 RID: 7044
		public static List<Player> PlayerList = new List<Player>();

		// Token: 0x04001B85 RID: 7045
		[Header("References")]
		public GameObject LocalGameObject;

		// Token: 0x04001B86 RID: 7046
		public ScheduleOne.AvatarFramework.Avatar Avatar;

		// Token: 0x04001B87 RID: 7047
		public AvatarAnimation Anim;

		// Token: 0x04001B88 RID: 7048
		public SmoothedVelocityCalculator VelocityCalculator;

		// Token: 0x04001B89 RID: 7049
		public Vector3 EyePosition = Vector3.zero;

		// Token: 0x04001B8A RID: 7050
		public AvatarSettings TestAvatarSettings;

		// Token: 0x04001B8B RID: 7051
		public PlayerVisualState VisualState;

		// Token: 0x04001B8C RID: 7052
		public PlayerVisibility Visibility;

		// Token: 0x04001B8D RID: 7053
		public CapsuleCollider CapCol;

		// Token: 0x04001B8E RID: 7054
		public POI PoI;

		// Token: 0x04001B8F RID: 7055
		public PlayerHealth Health;

		// Token: 0x04001B90 RID: 7056
		public PlayerCrimeData CrimeData;

		// Token: 0x04001B91 RID: 7057
		public PlayerEnergy Energy;

		// Token: 0x04001B92 RID: 7058
		public Transform MimicCamera;

		// Token: 0x04001B93 RID: 7059
		public AvatarFootstepDetector FootstepDetector;

		// Token: 0x04001B94 RID: 7060
		public LocalPlayerFootstepGenerator LocalFootstepDetector;

		// Token: 0x04001B95 RID: 7061
		public CharacterController CharacterController;

		// Token: 0x04001B96 RID: 7062
		public AudioSourceController PunchSound;

		// Token: 0x04001B97 RID: 7063
		public OptimizedLight ThirdPersonFlashlight;

		// Token: 0x04001B98 RID: 7064
		public WorldspaceDialogueRenderer NameLabel;

		// Token: 0x04001B99 RID: 7065
		public PlayerClothing Clothing;

		// Token: 0x04001B9A RID: 7066
		[Header("Settings")]
		public LayerMask GroundDetectionMask;

		// Token: 0x04001B9B RID: 7067
		public float AvatarOffset_Standing = -0.97f;

		// Token: 0x04001B9C RID: 7068
		public float AvatarOffset_Crouched = -0.45f;

		// Token: 0x04001B9D RID: 7069
		[Header("Movement mapping")]
		public AnimationCurve WalkingMapCurve;

		// Token: 0x04001B9E RID: 7070
		public AnimationCurve CrouchWalkMapCurve;

		// Token: 0x04001BA0 RID: 7072
		public NetworkConnection Connection;

		// Token: 0x04001BA3 RID: 7075
		public Player.VehicleEvent onEnterVehicle;

		// Token: 0x04001BA4 RID: 7076
		public Player.VehicleTransformEvent onExitVehicle;

		// Token: 0x04001BA5 RID: 7077
		public LandVehicle LastDrivenVehicle;

		// Token: 0x04001BAC RID: 7084
		public Action<Skateboard> onSkateboardMounted;

		// Token: 0x04001BAD RID: 7085
		public Action onSkateboardDismounted;

		// Token: 0x04001BB7 RID: 7095
		public bool HasCompletedIntro;

		// Token: 0x04001BBA RID: 7098
		public ItemSlot[] Inventory;

		// Token: 0x04001BBE RID: 7102
		[Header("Appearance debugging")]
		public BasicAvatarSettings DebugAvatarSettings;

		// Token: 0x04001BBF RID: 7103
		private PlayerLoader loader;

		// Token: 0x04001BC3 RID: 7107
		public UnityEvent onRagdoll;

		// Token: 0x04001BC4 RID: 7108
		public UnityEvent onRagdollEnd;

		// Token: 0x04001BC5 RID: 7109
		public UnityEvent onArrested;

		// Token: 0x04001BC6 RID: 7110
		public UnityEvent onFreed;

		// Token: 0x04001BC7 RID: 7111
		public UnityEvent onTased;

		// Token: 0x04001BC8 RID: 7112
		public UnityEvent onTasedEnd;

		// Token: 0x04001BC9 RID: 7113
		public UnityEvent onPassedOut;

		// Token: 0x04001BCA RID: 7114
		public UnityEvent onPassOutRecovery;

		// Token: 0x04001BCB RID: 7115
		public List<BaseVariable> PlayerVariables;

		// Token: 0x04001BCC RID: 7116
		public Dictionary<string, BaseVariable> VariableDict;

		// Token: 0x04001BCD RID: 7117
		private float standingScale;

		// Token: 0x04001BCE RID: 7118
		private float timeAirborne;

		// Token: 0x04001BD1 RID: 7121
		private Coroutine taseCoroutine;

		// Token: 0x04001BD2 RID: 7122
		private List<ConstantForce> ragdollForceComponents;

		// Token: 0x04001BD5 RID: 7125
		private List<int> impactHistory;

		// Token: 0x04001BDA RID: 7130
		private List<Quaternion> seizureRotations;

		// Token: 0x04001BDD RID: 7133
		private List<int> equippableMessageIDHistory;

		// Token: 0x04001BDE RID: 7134
		private Coroutine lerpScaleRoutine;

		// Token: 0x04001BDF RID: 7135
		public SyncVar<string> syncVar___<PlayerName>k__BackingField;

		// Token: 0x04001BE0 RID: 7136
		public SyncVar<string> syncVar___<PlayerCode>k__BackingField;

		// Token: 0x04001BE1 RID: 7137
		public SyncVar<NetworkObject> syncVar___<CurrentVehicle>k__BackingField;

		// Token: 0x04001BE2 RID: 7138
		public SyncVar<NetworkObject> syncVar___<CurrentBed>k__BackingField;

		// Token: 0x04001BE3 RID: 7139
		public SyncVar<bool> syncVar___<IsReadyToSleep>k__BackingField;

		// Token: 0x04001BE4 RID: 7140
		public SyncVar<Vector3> syncVar___<CameraPosition>k__BackingField;

		// Token: 0x04001BE5 RID: 7141
		public SyncVar<Quaternion> syncVar___<CameraRotation>k__BackingField;

		// Token: 0x04001BE6 RID: 7142
		private bool dll_Excuted;

		// Token: 0x04001BE7 RID: 7143
		private bool dll_Excuted;

		// Token: 0x020005CF RID: 1487
		// (Invoke) Token: 0x06002669 RID: 9833
		public delegate void VehicleEvent(LandVehicle vehicle);

		// Token: 0x020005D0 RID: 1488
		// (Invoke) Token: 0x0600266D RID: 9837
		public delegate void VehicleTransformEvent(LandVehicle vehicle, Transform exitPoint);
	}
}
