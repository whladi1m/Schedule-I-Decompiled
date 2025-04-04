using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Law;
using ScheduleOne.Levelling;
using ScheduleOne.Map;
using ScheduleOne.NPCs;
using ScheduleOne.Police;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005E3 RID: 1507
	public class PlayerCrimeData : NetworkBehaviour
	{
		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x000A035C File Offset: 0x0009E55C
		// (set) Token: 0x0600272D RID: 10029 RVA: 0x000A0364 File Offset: 0x0009E564
		public PlayerCrimeData.EPursuitLevel CurrentPursuitLevel
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CurrentPursuitLevel>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true)]
			protected set
			{
				this.RpcWriter___Server_set_CurrentPursuitLevel_2979171596(value);
				this.RpcLogic___set_CurrentPursuitLevel_2979171596(value);
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x000A037A File Offset: 0x0009E57A
		// (set) Token: 0x0600272F RID: 10031 RVA: 0x000A0382 File Offset: 0x0009E582
		public Vector3 LastKnownPosition
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<LastKnownPosition>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true)]
			protected set
			{
				this.RpcWriter___Server_set_LastKnownPosition_4276783012(value);
				this.RpcLogic___set_LastKnownPosition_4276783012(value);
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06002730 RID: 10032 RVA: 0x000A0398 File Offset: 0x0009E598
		// (set) Token: 0x06002731 RID: 10033 RVA: 0x000A03A0 File Offset: 0x0009E5A0
		public float CurrentArrestProgress { get; protected set; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x000A03A9 File Offset: 0x0009E5A9
		// (set) Token: 0x06002733 RID: 10035 RVA: 0x000A03B1 File Offset: 0x0009E5B1
		public float CurrentBodySearchProgress { get; protected set; }

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x000A03BA File Offset: 0x0009E5BA
		// (set) Token: 0x06002735 RID: 10037 RVA: 0x000A03C2 File Offset: 0x0009E5C2
		public float TimeSinceLastBodySearch { get; set; }

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x000A03CB File Offset: 0x0009E5CB
		// (set) Token: 0x06002737 RID: 10039 RVA: 0x000A03D3 File Offset: 0x0009E5D3
		public bool EvadedArrest { get; protected set; }

		// Token: 0x06002738 RID: 10040 RVA: 0x000A03DC File Offset: 0x0009E5DC
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.PlayerScripts.PlayerCrimeData_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x000A03FB File Offset: 0x0009E5FB
		private void Start()
		{
			NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.OnSleepStart));
			NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.OnSleepStart));
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000A0433 File Offset: 0x0009E633
		private void OnDestroy()
		{
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.InstanceExists)
			{
				NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.OnSleepStart));
			}
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x000A0458 File Offset: 0x0009E658
		protected virtual void Update()
		{
			this.CurrentPursuitLevelDuration += Time.deltaTime;
			this.TimeSincePursuitStart += Time.deltaTime;
			this.TimeSinceSighted += Time.deltaTime;
			this.timeSinceLastShot += Time.deltaTime;
			this.TimeSinceLastBodySearch += Time.deltaTime;
			if (!this.Player.IsOwner)
			{
				return;
			}
			if (this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None && this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.Lethal)
			{
				this.UpdateEscalation();
			}
			if (this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.UpdateTimeout();
				this.UpdateMusic();
			}
			if (this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None && this.TimeSinceSighted > 2f)
			{
				this.Player.VisualState.ApplyState("SearchedFor", PlayerVisualState.EVisualState.SearchedFor, 0f);
			}
			else
			{
				this.Player.VisualState.RemoveState("SearchedFor", 0f);
			}
			for (int i = 0; i < this.Collisions.Count; i++)
			{
				this.Collisions[i].TimeSince += Time.deltaTime;
				if (this.Collisions[i].TimeSince > 30f)
				{
					this.Collisions.RemoveAt(i);
					i--;
				}
			}
			Singleton<HUD>.Instance.CrimeStatusUI.UpdateStatus();
			if ((float)this.Collisions.Count >= 3f)
			{
				this.RecordLastKnownPosition(true);
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Investigating);
				this.AddCrime(new VehicularAssault(), this.Collisions.Count - 1);
				Singleton<LawManager>.Instance.PoliceCalled(this.Player, new VehicularAssault());
				this.Collisions.Clear();
			}
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000A0608 File Offset: 0x0009E808
		protected virtual void LateUpdate()
		{
			if (this.CurrentArrestProgress > 0f)
			{
				Singleton<ProgressSlider>.Instance.Configure("Cuffing...", new Color32(75, 165, byte.MaxValue, byte.MaxValue));
				Singleton<ProgressSlider>.Instance.ShowProgress(this.CurrentArrestProgress);
			}
			else if (this.CurrentBodySearchProgress > 0f)
			{
				Singleton<ProgressSlider>.Instance.Configure("Being searched...", new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
				Singleton<ProgressSlider>.Instance.ShowProgress(this.CurrentBodySearchProgress);
			}
			this.CurrentArrestProgress = 0f;
			this.CurrentBodySearchProgress = 0f;
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000A06C0 File Offset: 0x0009E8C0
		public void SetPursuitLevel(PlayerCrimeData.EPursuitLevel level)
		{
			if (GameManager.IS_TUTORIAL)
			{
				return;
			}
			Debug.Log("New pursuit level: " + level.ToString());
			PlayerCrimeData.EPursuitLevel currentPursuitLevel = this.CurrentPursuitLevel;
			this.CurrentPursuitLevel = level;
			if (level != PlayerCrimeData.EPursuitLevel.None)
			{
				this.BodySearchPending = false;
			}
			if (currentPursuitLevel == PlayerCrimeData.EPursuitLevel.None && level != PlayerCrimeData.EPursuitLevel.None)
			{
				this.TimeSincePursuitStart = 0f;
				this.TimeSinceSighted = 0f;
				this.Player.VisualState.ApplyState("Wanted", PlayerVisualState.EVisualState.Wanted, 0f);
				if (this.Player.Owner.IsLocalClient)
				{
					this._lightCombatTrack.Enable();
				}
			}
			if (level == PlayerCrimeData.EPursuitLevel.Lethal && this.Player.Owner.IsLocalClient)
			{
				this._lightCombatTrack.Stop();
				this._heavyCombatTrack.Enable();
			}
			if (currentPursuitLevel != PlayerCrimeData.EPursuitLevel.None && level == PlayerCrimeData.EPursuitLevel.None)
			{
				this.ClearCrimes();
				this.Player.VisualState.RemoveState("Wanted", 0f);
				if (this.Player.Owner.IsLocalClient)
				{
					this._lightCombatTrack.Disable();
					this._lightCombatTrack.Stop();
					this._heavyCombatTrack.Disable();
					this._heavyCombatTrack.Stop();
				}
			}
			this.CurrentPursuitLevelDuration = 0f;
			if (this.Player.IsOwner)
			{
				Singleton<HUD>.Instance.CrimeStatusUI.UpdateStatus();
			}
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000A0814 File Offset: 0x0009EA14
		public void Escalate()
		{
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Investigating);
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Investigating)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Arresting)
			{
				this.SetEvaded();
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.NonLethal);
				if (PoliceStation.GetClosestPoliceStation(this.Player.Avatar.MiddleSpineRB.position).TimeSinceLastDispatch > 10f)
				{
					PoliceStation.GetClosestPoliceStation(this.Player.Avatar.MiddleSpineRB.position).Dispatch(1, this.Player, PoliceStation.EDispatchType.Auto, true);
					return;
				}
			}
			else if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.NonLethal)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Lethal);
				PoliceStation.GetClosestPoliceStation(this.Player.Avatar.MiddleSpineRB.position);
				PoliceStation.GetClosestPoliceStation(this.Player.Avatar.MiddleSpineRB.position).Dispatch(1, this.Player, PoliceStation.EDispatchType.Auto, true);
			}
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000A08FC File Offset: 0x0009EAFC
		public void Deescalate()
		{
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Investigating)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Arresting)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Investigating);
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.NonLethal)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Lethal)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.NonLethal);
			}
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x000A094C File Offset: 0x0009EB4C
		[ObserversRpc(RunLocally = true)]
		public void RecordLastKnownPosition(bool resetTimeSinceSighted)
		{
			this.RpcWriter___Observers_RecordLastKnownPosition_1140765316(resetTimeSinceSighted);
			this.RpcLogic___RecordLastKnownPosition_1140765316(resetTimeSinceSighted);
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000A0962 File Offset: 0x0009EB62
		public void SetArrestProgress(float progress)
		{
			this.CurrentArrestProgress = progress;
			if (progress >= 1f)
			{
				this.Player.Arrest();
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
			}
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000A0985 File Offset: 0x0009EB85
		public void ResetBodysearchCooldown()
		{
			this.TimeSinceLastBodySearch = 0f;
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000A0992 File Offset: 0x0009EB92
		public void SetBodySearchProgress(float progress)
		{
			this.CurrentBodySearchProgress = progress;
			if (this.CurrentBodySearchProgress >= 1f)
			{
				this.TimeSinceLastBodySearch = 0f;
				this.BodySearchPending = false;
			}
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000A09BA File Offset: 0x0009EBBA
		private void OnDie()
		{
			if (this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.SetArrestProgress(1f);
			}
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000A09D0 File Offset: 0x0009EBD0
		public void AddCrime(Crime crime, int quantity = 1)
		{
			if (crime == null)
			{
				return;
			}
			Debug.Log("Adding crime: " + ((crime != null) ? crime.ToString() : null));
			Crime[] array = this.Crimes.Keys.ToArray<Crime>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetType() == crime.GetType())
				{
					Dictionary<Crime, int> crimes = this.Crimes;
					Crime key = array[i];
					crimes[key] += quantity;
					return;
				}
			}
			this.Crimes.Add(crime, quantity);
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000A0A5A File Offset: 0x0009EC5A
		public void ClearCrimes()
		{
			this.Crimes.Clear();
			this.EvadedArrest = false;
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x000A0A70 File Offset: 0x0009EC70
		public bool IsCrimeOnRecord(Type crime)
		{
			Crime[] array = this.Crimes.Keys.ToArray<Crime>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetType() == crime)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x000A0AAF File Offset: 0x0009ECAF
		public void SetEvaded()
		{
			this.EvadedArrest = true;
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x000A0AB8 File Offset: 0x0009ECB8
		private void OnSleepStart()
		{
			if (this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
				this.ClearCrimes();
			}
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x000A0AD0 File Offset: 0x0009ECD0
		private void UpdateEscalation()
		{
			if (this.TimeSinceSighted > 1f)
			{
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Arresting)
			{
				if (this.CurrentPursuitLevelDuration > 25f)
				{
					this.Escalate();
					return;
				}
			}
			else if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.NonLethal && this.CurrentPursuitLevelDuration > 120f)
			{
				this.Escalate();
			}
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x000A0B24 File Offset: 0x0009ED24
		private void UpdateTimeout()
		{
			if (!this.Player.IsOwner)
			{
				return;
			}
			if (this.TimeSinceSighted > this.GetSearchTime() + 3f)
			{
				this.TimeoutPursuit();
			}
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x000A0B50 File Offset: 0x0009ED50
		private void UpdateMusic()
		{
			if (!this.Player.Owner.IsLocalClient)
			{
				return;
			}
			float num = this._lightCombatTrack.VolumeMultiplier;
			if (this.TimeSinceSighted > this.outOfSightTimeToDipMusic)
			{
				num -= this.musicChangeRate_Down * Time.deltaTime;
			}
			else
			{
				num += this.musicChangeRate_Up * Time.deltaTime;
			}
			num = Mathf.Clamp(num, this.minMusicVolume, 1f);
			this._lightCombatTrack.VolumeMultiplier = num;
			this._heavyCombatTrack.VolumeMultiplier = num;
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x000A0BD4 File Offset: 0x0009EDD4
		private void TimeoutPursuit()
		{
			switch (this.CurrentPursuitLevel)
			{
			case PlayerCrimeData.EPursuitLevel.Arresting:
				NetworkSingleton<LevelManager>.Instance.AddXP(20);
				break;
			case PlayerCrimeData.EPursuitLevel.NonLethal:
				NetworkSingleton<LevelManager>.Instance.AddXP(40);
				break;
			case PlayerCrimeData.EPursuitLevel.Lethal:
				NetworkSingleton<LevelManager>.Instance.AddXP(60);
				break;
			}
			this.onPursuitEscapedSound.Play();
			this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
			this.ClearCrimes();
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000A0C44 File Offset: 0x0009EE44
		public float GetSearchTime()
		{
			switch (this.CurrentPursuitLevel)
			{
			case PlayerCrimeData.EPursuitLevel.Investigating:
				return 60f;
			case PlayerCrimeData.EPursuitLevel.Arresting:
				return 25f;
			case PlayerCrimeData.EPursuitLevel.NonLethal:
				return 30f;
			case PlayerCrimeData.EPursuitLevel.Lethal:
				return 40f;
			default:
				return 0f;
			}
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x000A0C8F File Offset: 0x0009EE8F
		public void ResetShotAccuracy()
		{
			this.timeSinceLastShot = 0f;
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x000A0C9C File Offset: 0x0009EE9C
		public float GetShotAccuracyMultiplier()
		{
			float num = 1f;
			if (this.timeSinceLastShot < 2f)
			{
				num = 0f;
			}
			if (this.timeSinceLastShot < 8f)
			{
				num = 1f - (this.timeSinceLastShot - 2f) / 6f;
			}
			float t = Mathf.Clamp01(Mathf.InverseLerp(0f, PlayerMovement.WalkSpeed * PlayerMovement.SprintMultiplier, this.Player.VelocityCalculator.Velocity.magnitude));
			float num2 = Mathf.Lerp(2f, 0.5f, t);
			int num3 = 0;
			for (int i = 0; i < PoliceOfficer.Officers.Count; i++)
			{
				if (PoliceOfficer.Officers[i].PursuitBehaviour.Active && PoliceOfficer.Officers[i].TargetPlayerNOB == this.Player.NetworkObject && Vector3.Distance(PoliceOfficer.Officers[i].transform.position, this.Player.Avatar.CenterPoint) < 20f)
				{
					num3++;
				}
			}
			float num4 = Mathf.Lerp(1f, 0.6f, Mathf.Clamp01((float)num3 / 3f));
			return num * num2 * num4;
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x000A0DDC File Offset: 0x0009EFDC
		public void RecordVehicleCollision(NPC victim)
		{
			PlayerCrimeData.VehicleCollisionInstance item = new PlayerCrimeData.VehicleCollisionInstance(victim, 0f);
			this.Collisions.Add(item);
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000A0E01 File Offset: 0x0009F001
		private void CheckNearestOfficer()
		{
			if (this.Player == null)
			{
				return;
			}
			this.NearestOfficer = (from x in PoliceOfficer.Officers
			orderby Vector3.Distance(x.Avatar.CenterPoint, this.Player.Avatar.CenterPoint)
			select x).FirstOrDefault<PoliceOfficer>();
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000A0E34 File Offset: 0x0009F034
		public PlayerCrimeData()
		{
			this.<LastKnownPosition>k__BackingField = Vector3.zero;
			this.Pursuers = new List<PoliceOfficer>();
			this.TimeSinceSighted = 100000f;
			this.Crimes = new Dictionary<Crime, int>();
			this.TimeSinceLastBodySearch = 100000f;
			this.timeSinceLastShot = 1000f;
			this.Collisions = new List<PlayerCrimeData.VehicleCollisionInstance>();
			this.outOfSightTimeToDipMusic = 8f;
			this.minMusicVolume = 0.6f;
			this.musicChangeRate_Down = 0.04f;
			this.musicChangeRate_Up = 2f;
			base..ctor();
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000A0EEC File Offset: 0x0009F0EC
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerCrimeDataAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerCrimeDataAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<LastKnownPosition>k__BackingField = new SyncVar<Vector3>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, 0.5f, Channel.Reliable, this.<LastKnownPosition>k__BackingField);
			this.syncVar___<CurrentPursuitLevel>k__BackingField = new SyncVar<PlayerCrimeData.EPursuitLevel>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, 0.5f, Channel.Reliable, this.<CurrentPursuitLevel>k__BackingField);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_set_CurrentPursuitLevel_2979171596));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_set_LastKnownPosition_4276783012));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_RecordLastKnownPosition_1140765316));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.PlayerScripts.PlayerCrimeData));
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000A0FB7 File Offset: 0x0009F1B7
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerCrimeDataAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerCrimeDataAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<LastKnownPosition>k__BackingField.SetRegistered();
			this.syncVar___<CurrentPursuitLevel>k__BackingField.SetRegistered();
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000A0FE0 File Offset: 0x0009F1E0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000A0FF0 File Offset: 0x0009F1F0
		private void RpcWriter___Server_set_CurrentPursuitLevel_2979171596(PlayerCrimeData.EPursuitLevel value)
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
			writer.Write___ScheduleOne.PlayerScripts.PlayerCrimeData/EPursuitLevelFishNet.Serializing.Generated(value);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000A10F1 File Offset: 0x0009F2F1
		protected void RpcLogic___set_CurrentPursuitLevel_2979171596(PlayerCrimeData.EPursuitLevel value)
		{
			this.sync___set_value_<CurrentPursuitLevel>k__BackingField(value, true);
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000A10FC File Offset: 0x0009F2FC
		private void RpcReader___Server_set_CurrentPursuitLevel_2979171596(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			PlayerCrimeData.EPursuitLevel value = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerCrimeData/EPursuitLevelFishNet.Serializing.Generateds(PooledReader0);
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
			this.RpcLogic___set_CurrentPursuitLevel_2979171596(value);
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x000A114C File Offset: 0x0009F34C
		private void RpcWriter___Server_set_LastKnownPosition_4276783012(Vector3 value)
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
			base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x000A124D File Offset: 0x0009F44D
		protected void RpcLogic___set_LastKnownPosition_4276783012(Vector3 value)
		{
			this.sync___set_value_<LastKnownPosition>k__BackingField(value, true);
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000A1258 File Offset: 0x0009F458
		private void RpcReader___Server_set_LastKnownPosition_4276783012(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
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
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___set_LastKnownPosition_4276783012(value);
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x000A12A8 File Offset: 0x0009F4A8
		private void RpcWriter___Observers_RecordLastKnownPosition_1140765316(bool resetTimeSinceSighted)
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
			writer.WriteBoolean(resetTimeSinceSighted);
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x000A135E File Offset: 0x0009F55E
		public void RpcLogic___RecordLastKnownPosition_1140765316(bool resetTimeSinceSighted)
		{
			this.LastKnownPosition = this.Player.Avatar.CenterPoint;
			if (resetTimeSinceSighted)
			{
				this.TimeSinceSighted = 0f;
			}
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000A1384 File Offset: 0x0009F584
		private void RpcReader___Observers_RecordLastKnownPosition_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool resetTimeSinceSighted = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___RecordLastKnownPosition_1140765316(resetTimeSinceSighted);
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06002762 RID: 10082 RVA: 0x000A13BF File Offset: 0x0009F5BF
		// (set) Token: 0x06002763 RID: 10083 RVA: 0x000A13C7 File Offset: 0x0009F5C7
		public PlayerCrimeData.EPursuitLevel SyncAccessor_<CurrentPursuitLevel>k__BackingField
		{
			get
			{
				return this.<CurrentPursuitLevel>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CurrentPursuitLevel>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CurrentPursuitLevel>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000A1404 File Offset: 0x0009F604
		public virtual bool PlayerCrimeData(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<LastKnownPosition>k__BackingField(this.syncVar___<LastKnownPosition>k__BackingField.GetValue(true), true);
					return true;
				}
				Vector3 value = PooledReader0.ReadVector3();
				this.sync___set_value_<LastKnownPosition>k__BackingField(value, Boolean2);
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
					this.sync___set_value_<CurrentPursuitLevel>k__BackingField(this.syncVar___<CurrentPursuitLevel>k__BackingField.GetValue(true), true);
					return true;
				}
				PlayerCrimeData.EPursuitLevel value2 = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerCrimeData/EPursuitLevelFishNet.Serializing.Generateds(PooledReader0);
				this.sync___set_value_<CurrentPursuitLevel>k__BackingField(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06002765 RID: 10085 RVA: 0x000A149A File Offset: 0x0009F69A
		// (set) Token: 0x06002766 RID: 10086 RVA: 0x000A14A2 File Offset: 0x0009F6A2
		public Vector3 SyncAccessor_<LastKnownPosition>k__BackingField
		{
			get
			{
				return this.<LastKnownPosition>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<LastKnownPosition>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<LastKnownPosition>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000A14E0 File Offset: 0x0009F6E0
		private void dll()
		{
			this.Player.Health.onDie.AddListener(new UnityAction(this.OnDie));
			this.Player.onFreed.AddListener(new UnityAction(this.ClearCrimes));
			this.Player.onFreed.AddListener(delegate()
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
			});
			base.InvokeRepeating("CheckNearestOfficer", 0f, 0.2f);
			this._lightCombatTrack = Singleton<MusicPlayer>.Instance.Tracks.Find((MusicTrack t) => t.TrackName == "Light Combat");
			this._heavyCombatTrack = Singleton<MusicPlayer>.Instance.Tracks.Find((MusicTrack t) => t.TrackName == "Heavy Combat");
		}

		// Token: 0x04001C70 RID: 7280
		public const float SEARCH_TIME_INVESTIGATING = 60f;

		// Token: 0x04001C71 RID: 7281
		public const float SEARCH_TIME_ARRESTING = 25f;

		// Token: 0x04001C72 RID: 7282
		public const float SEARCH_TIME_NONLETHAL = 30f;

		// Token: 0x04001C73 RID: 7283
		public const float SEARCH_TIME_LETHAL = 40f;

		// Token: 0x04001C74 RID: 7284
		public const float ESCALATION_TIME_ARRESTING = 25f;

		// Token: 0x04001C75 RID: 7285
		public const float ESCALATION_TIME_NONLETHAL = 120f;

		// Token: 0x04001C76 RID: 7286
		public const float SHOT_COOLDOWN_MIN = 2f;

		// Token: 0x04001C77 RID: 7287
		public const float SHOT_COOLDOWN_MAX = 8f;

		// Token: 0x04001C78 RID: 7288
		public const float VEHICLE_COLLISION_LIFETIME = 30f;

		// Token: 0x04001C79 RID: 7289
		public const float VEHICLE_COLLISION_LIMIT = 3f;

		// Token: 0x04001C7A RID: 7290
		public PoliceOfficer NearestOfficer;

		// Token: 0x04001C7B RID: 7291
		public Player Player;

		// Token: 0x04001C7C RID: 7292
		public AudioSourceController onPursuitEscapedSound;

		// Token: 0x04001C7F RID: 7295
		public List<PoliceOfficer> Pursuers;

		// Token: 0x04001C82 RID: 7298
		public float TimeSincePursuitStart;

		// Token: 0x04001C83 RID: 7299
		public float CurrentPursuitLevelDuration;

		// Token: 0x04001C84 RID: 7300
		public float TimeSinceSighted;

		// Token: 0x04001C85 RID: 7301
		public Dictionary<Crime, int> Crimes;

		// Token: 0x04001C86 RID: 7302
		public bool BodySearchPending;

		// Token: 0x04001C89 RID: 7305
		public float timeSinceLastShot;

		// Token: 0x04001C8A RID: 7306
		protected List<PlayerCrimeData.VehicleCollisionInstance> Collisions;

		// Token: 0x04001C8B RID: 7307
		private MusicTrack _lightCombatTrack;

		// Token: 0x04001C8C RID: 7308
		private MusicTrack _heavyCombatTrack;

		// Token: 0x04001C8D RID: 7309
		private float outOfSightTimeToDipMusic;

		// Token: 0x04001C8E RID: 7310
		private float minMusicVolume;

		// Token: 0x04001C8F RID: 7311
		private float musicChangeRate_Down;

		// Token: 0x04001C90 RID: 7312
		private float musicChangeRate_Up;

		// Token: 0x04001C91 RID: 7313
		public SyncVar<PlayerCrimeData.EPursuitLevel> syncVar___<CurrentPursuitLevel>k__BackingField;

		// Token: 0x04001C92 RID: 7314
		public SyncVar<Vector3> syncVar___<LastKnownPosition>k__BackingField;

		// Token: 0x04001C93 RID: 7315
		private bool dll_Excuted;

		// Token: 0x04001C94 RID: 7316
		private bool dll_Excuted;

		// Token: 0x020005E4 RID: 1508
		public class VehicleCollisionInstance
		{
			// Token: 0x06002768 RID: 10088 RVA: 0x000A15C3 File Offset: 0x0009F7C3
			public VehicleCollisionInstance(NPC victim, float timeSince)
			{
				this.Victim = victim;
				this.TimeSince = timeSince;
			}

			// Token: 0x04001C95 RID: 7317
			public NPC Victim;

			// Token: 0x04001C96 RID: 7318
			public float TimeSince;
		}

		// Token: 0x020005E5 RID: 1509
		public enum EPursuitLevel
		{
			// Token: 0x04001C98 RID: 7320
			None,
			// Token: 0x04001C99 RID: 7321
			Investigating,
			// Token: 0x04001C9A RID: 7322
			Arresting,
			// Token: 0x04001C9B RID: 7323
			NonLethal,
			// Token: 0x04001C9C RID: 7324
			Lethal
		}
	}
}
