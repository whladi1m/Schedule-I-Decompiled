using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.GameTime;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Employees
{
	// Token: 0x02000636 RID: 1590
	public class Employee : NPC
	{
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002AAD RID: 10925 RVA: 0x000AFBDE File Offset: 0x000ADDDE
		// (set) Token: 0x06002AAE RID: 10926 RVA: 0x000AFBE6 File Offset: 0x000ADDE6
		public Property AssignedProperty { get; protected set; }

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002AAF RID: 10927 RVA: 0x000AFBEF File Offset: 0x000ADDEF
		// (set) Token: 0x06002AB0 RID: 10928 RVA: 0x000AFBF7 File Offset: 0x000ADDF7
		public int EmployeeIndex { get; protected set; }

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002AB1 RID: 10929 RVA: 0x000AFC00 File Offset: 0x000ADE00
		// (set) Token: 0x06002AB2 RID: 10930 RVA: 0x000AFC08 File Offset: 0x000ADE08
		public bool PaidForToday
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<PaidForToday>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.sync___set_value_<PaidForToday>k__BackingField(value, true);
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x000AFC12 File Offset: 0x000ADE12
		// (set) Token: 0x06002AB4 RID: 10932 RVA: 0x000AFC1A File Offset: 0x000ADE1A
		public bool Fired { get; private set; }

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000AFC23 File Offset: 0x000ADE23
		public bool IsWaitingOutside
		{
			get
			{
				return this.WaitOutside.Active;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06002AB6 RID: 10934 RVA: 0x000AFC30 File Offset: 0x000ADE30
		// (set) Token: 0x06002AB7 RID: 10935 RVA: 0x000AFC38 File Offset: 0x000ADE38
		public bool IsMale { get; private set; } = true;

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06002AB8 RID: 10936 RVA: 0x000AFC41 File Offset: 0x000ADE41
		// (set) Token: 0x06002AB9 RID: 10937 RVA: 0x000AFC49 File Offset: 0x000ADE49
		private protected int AppearanceIndex { protected get; private set; }

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06002ABA RID: 10938 RVA: 0x000AFC52 File Offset: 0x000ADE52
		public EEmployeeType EmployeeType
		{
			get
			{
				return this.Type;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06002ABB RID: 10939 RVA: 0x000AFC5A File Offset: 0x000ADE5A
		// (set) Token: 0x06002ABC RID: 10940 RVA: 0x000AFC62 File Offset: 0x000ADE62
		public int TimeSinceLastWorked { get; private set; }

		// Token: 0x06002ABD RID: 10941 RVA: 0x000AFC6C File Offset: 0x000ADE6C
		protected override void Start()
		{
			base.Start();
			DialogueController.DialogueChoice dialogueChoice = new DialogueController.DialogueChoice();
			dialogueChoice.ChoiceText = "Why aren't you working?";
			dialogueChoice.Enabled = true;
			dialogueChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.ShouldShowNoWorkDialogue);
			dialogueChoice.onChoosen.AddListener(new UnityAction(this.OnNotWorkingDialogue));
			this.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(dialogueChoice, 0);
			DialogueController.DialogueChoice dialogueChoice2 = new DialogueController.DialogueChoice();
			dialogueChoice2.ChoiceText = "Your services are no longer required.";
			dialogueChoice2.Enabled = true;
			dialogueChoice2.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.ShouldShowFireDialogue);
			dialogueChoice2.Conversation = this.FireDialogue;
			this.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(dialogueChoice2, 0);
			this.dialogueHandler.onDialogueChoiceChosen.AddListener(new UnityAction<string>(this.CheckDialogueChoice));
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000AFD39 File Offset: 0x000ADF39
		public override void OnStartServer()
		{
			base.OnStartServer();
			this.Health.onDie.AddListener(new UnityAction(this.SendFire));
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x000AFD60 File Offset: 0x000ADF60
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			this.Initialize(connection, this.FirstName, this.LastName, this.ID, base.GUID.ToString(), this.AssignedProperty.PropertyCode, this.IsMale, this.AppearanceIndex);
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000AFDC4 File Offset: 0x000ADFC4
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public virtual void Initialize(NetworkConnection conn, string firstName, string lastName, string id, string guid, string propertyID, bool male, int appearanceIndex)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Initialize_2260823878(conn, firstName, lastName, id, guid, propertyID, male, appearanceIndex);
				this.RpcLogic___Initialize_2260823878(conn, firstName, lastName, id, guid, propertyID, male, appearanceIndex);
			}
			else
			{
				this.RpcWriter___Target_Initialize_2260823878(conn, firstName, lastName, id, guid, propertyID, male, appearanceIndex);
			}
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x000AFE50 File Offset: 0x000AE050
		protected virtual void AssignProperty(Property prop)
		{
			this.AssignedProperty = prop;
			this.EmployeeIndex = this.AssignedProperty.RegisterEmployee(this);
			this.movement.Warp(prop.NPCSpawnPoint.position);
			this.WaitOutside.IdlePoint = prop.EmployeeIdlePoints[this.EmployeeIndex];
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000AFEA4 File Offset: 0x000AE0A4
		protected virtual void InitializeInfo(string firstName, string lastName, string id)
		{
			this.FirstName = firstName;
			this.LastName = lastName;
			this.ID = id;
			NetworkSingleton<EmployeeManager>.Instance.RegisterName(firstName + " " + lastName);
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x000AFED4 File Offset: 0x000AE0D4
		protected virtual void InitializeAppearance(bool male, int index)
		{
			this.IsMale = male;
			this.AppearanceIndex = index;
			EmployeeManager.EmployeeAppearance appearance = NetworkSingleton<EmployeeManager>.Instance.GetAppearance(male, index);
			appearance.Settings.BodyLayerSettings.Clear();
			this.Avatar.LoadNakedSettings(appearance.Settings, 100);
			this.MugshotSprite = appearance.Mugshot;
			this.VoiceOverEmitter.SetDatabase(NetworkSingleton<EmployeeManager>.Instance.GetVoice(male, index), true);
			int num = (this.FirstName + this.LastName).GetHashCode() / 1000;
			this.VoiceOverEmitter.PitchMultiplier = 0.9f + (float)(num % 10) / 10f * 0.2f;
			NetworkSingleton<EmployeeManager>.Instance.RegisterAppearance(male, index);
			float num2 = male ? 0.8f : 1.3f;
			float num3 = 0.2f;
			float num4 = -num3 / 2f + Mathf.Clamp01((float)(this.FirstName.GetHashCode() % 10) / 10f) * num3;
			num2 += num4;
			this.VoiceOverEmitter.PitchMultiplier = num2;
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x000AFFDC File Offset: 0x000AE1DC
		protected virtual void CheckDialogueChoice(string choiceLabel)
		{
			if (choiceLabel == "CONFIRM_FIRE")
			{
				this.SendFire();
			}
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x000AFFF1 File Offset: 0x000AE1F1
		[ServerRpc(RequireOwnership = false)]
		public void SendFire()
		{
			this.RpcWriter___Server_SendFire_2166136261();
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x000AFFF9 File Offset: 0x000AE1F9
		[ObserversRpc]
		private void ReceiveFire()
		{
			this.RpcWriter___Observers_ReceiveFire_2166136261();
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x000B0004 File Offset: 0x000AE204
		protected virtual void Fire()
		{
			Console.Log("Firing employee " + this.FirstName + " " + this.LastName, null);
			this.AssignedProperty.DeregisterEmployee(this);
			this.Avatar.EmotionManager.AddEmotionOverride("Concerned", "fired", 0f, 0);
			this.SetWaitOutside(false);
			this.Fired = true;
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x000B006C File Offset: 0x000AE26C
		protected bool CanWork()
		{
			return this.GetBed() != null && this.PaidForToday && !NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsEndOfDay;
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x000B0094 File Offset: 0x000AE294
		protected new virtual void OnDestroy()
		{
			if (InstanceFinder.IsServer)
			{
				ScheduleOne.GameTime.TimeManager.onSleepEnd = (Action<int>)Delegate.Remove(ScheduleOne.GameTime.TimeManager.onSleepEnd, new Action<int>(this.OnSleepEnd));
			}
			if (NetworkSingleton<EmployeeManager>.InstanceExists)
			{
				NetworkSingleton<EmployeeManager>.Instance.AllEmployees.Remove(this);
			}
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x000B00E0 File Offset: 0x000AE2E0
		protected virtual void UpdateBehaviour()
		{
			if (this.Fired)
			{
				return;
			}
			if (this.behaviour.activeBehaviour == null || this.behaviour.activeBehaviour == this.WaitOutside)
			{
				bool flag = false;
				bool flag2 = false;
				if (this.GetBed() == null)
				{
					flag = true;
					this.SubmitNoWorkReason("I haven't been assigned a bed", "You can use your management clipboard to assign me a bed.", 0);
				}
				else if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsEndOfDay)
				{
					flag = true;
					this.SubmitNoWorkReason("Sorry boss, my shift ends at 4AM.", string.Empty, 0);
				}
				else if (!this.PaidForToday)
				{
					if (this.IsPayAvailable())
					{
						flag2 = true;
					}
					else
					{
						flag = true;
						this.SubmitNoWorkReason("I haven't been paid yet", "You can place cash in my briefcase on my bed.", 0);
					}
				}
				if (flag)
				{
					this.SetWaitOutside(true);
					return;
				}
				if (InstanceFinder.IsServer && flag2 && this.IsPayAvailable())
				{
					this.RemoveDailyWage();
					this.SetIsPaid();
				}
			}
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x000B01BB File Offset: 0x000AE3BB
		protected void MarkIsWorking()
		{
			this.TimeSinceLastWorked = 0;
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x000B01C4 File Offset: 0x000AE3C4
		private void SetWaitOutside(bool wait)
		{
			if (wait)
			{
				if (!this.WaitOutside.Enabled)
				{
					this.WaitOutside.Enable_Networked(null);
					return;
				}
			}
			else if (this.WaitOutside.Enabled || this.WaitOutside.Active)
			{
				this.WaitOutside.Disable_Networked(null);
				this.WaitOutside.End_Networked(null);
			}
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x00014002 File Offset: 0x00012202
		protected virtual bool ShouldIdle()
		{
			return false;
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x00014002 File Offset: 0x00012202
		protected override bool ShouldNoticeGeneralCrime(Player player)
		{
			return false;
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x000B0220 File Offset: 0x000AE420
		protected override void MinPass()
		{
			base.MinPass();
			int timeSinceLastWorked = this.TimeSinceLastWorked;
			this.TimeSinceLastWorked = timeSinceLastWorked + 1;
			this.WorkIssues.Clear();
			this.UpdateBehaviour();
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000B0254 File Offset: 0x000AE454
		private void OnSleepEnd(int sleepTime)
		{
			this.PaidForToday = false;
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x000B025D File Offset: 0x000AE45D
		public void SetIsPaid()
		{
			this.PaidForToday = true;
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x00014002 File Offset: 0x00012202
		public override bool ShouldSave()
		{
			return false;
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x000B0268 File Offset: 0x000AE468
		public override string GetSaveString()
		{
			return new EmployeeData(this.ID, this.AssignedProperty.PropertyCode, this.FirstName, this.LastName, this.IsMale, this.AppearanceIndex, base.transform.position, base.transform.rotation, base.GUID, this.PaidForToday).GetJson(true);
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x000B02CB File Offset: 0x000AE4CB
		public virtual BedItem GetBed()
		{
			Console.LogError("GETBED NOT IMPLEMENTED", null);
			return null;
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000B02DC File Offset: 0x000AE4DC
		public bool IsPayAvailable()
		{
			BedItem bed = this.GetBed();
			return !(bed == null) && bed.GetCashSum() >= this.DailyWage;
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x000B030C File Offset: 0x000AE50C
		public void RemoveDailyWage()
		{
			Console.Log("Removing daily wage", null);
			BedItem bed = this.GetBed();
			if (bed == null)
			{
				return;
			}
			if (bed.GetCashSum() >= this.DailyWage)
			{
				bed.RemoveCash(this.DailyWage);
			}
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x000B0350 File Offset: 0x000AE550
		public virtual bool GetWorkIssue(out DialogueContainer notWorkingReason)
		{
			if (this.GetBed() == null)
			{
				notWorkingReason = this.BedNotAssignedDialogue;
				return true;
			}
			if (!this.PaidForToday)
			{
				notWorkingReason = this.NotPaidDialogue;
				return true;
			}
			if (this.TimeSinceLastWorked >= 5 && this.WorkIssues.Count > 0)
			{
				notWorkingReason = UnityEngine.Object.Instantiate<DialogueContainer>(this.WorkIssueDialogueTemplate);
				notWorkingReason.GetDialogueNodeByLabel("ENTRY").DialogueText = this.WorkIssues[0].Reason;
				if (!string.IsNullOrEmpty(this.WorkIssues[0].Fix))
				{
					notWorkingReason.GetDialogueNodeByLabel("FIX").DialogueText = this.WorkIssues[0].Fix;
				}
				else
				{
					notWorkingReason.GetDialogueNodeByLabel("ENTRY").choices = new DialogueChoiceData[0];
				}
				return true;
			}
			notWorkingReason = null;
			return false;
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x000B042C File Offset: 0x000AE62C
		public virtual void SetIdle(bool idle)
		{
			this.SetWaitOutside(idle);
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x000B0438 File Offset: 0x000AE638
		protected void LeavePropertyAndDespawn()
		{
			if (this.movement.IsMoving)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.movement.IsAsCloseAsPossible(this.AssignedProperty.NPCSpawnPoint.position, 1f))
			{
				base.Despawn(base.NetworkObject, null);
				return;
			}
			this.movement.SetDestination(this.AssignedProperty.NPCSpawnPoint.position);
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x000B04B0 File Offset: 0x000AE6B0
		[ObserversRpc(RunLocally = true)]
		public void SubmitNoWorkReason(string reason, string fix, int priority = 0)
		{
			this.RpcWriter___Observers_SubmitNoWorkReason_15643032(reason, fix, priority);
			this.RpcLogic___SubmitNoWorkReason_15643032(reason, fix, priority);
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x000B04E4 File Offset: 0x000AE6E4
		private bool ShouldShowNoWorkDialogue(bool enabled)
		{
			DialogueContainer dialogueContainer;
			return !this.Fired && this.WaitOutside.Active && this.GetWorkIssue(out dialogueContainer);
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x000B0514 File Offset: 0x000AE714
		private void OnNotWorkingDialogue()
		{
			DialogueContainer container;
			if (!this.GetWorkIssue(out container))
			{
				return;
			}
			this.dialogueHandler.InitializeDialogue(container);
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x000B0538 File Offset: 0x000AE738
		private bool ShouldShowFireDialogue(bool enabled)
		{
			return !this.Fired;
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x000B0578 File Offset: 0x000AE778
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.EmployeeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.EmployeeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<PaidForToday>k__BackingField = new SyncVar<bool>(this, 1U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<PaidForToday>k__BackingField);
			base.RegisterObserversRpc(35U, new ClientRpcDelegate(this.RpcReader___Observers_Initialize_2260823878));
			base.RegisterTargetRpc(36U, new ClientRpcDelegate(this.RpcReader___Target_Initialize_2260823878));
			base.RegisterServerRpc(37U, new ServerRpcDelegate(this.RpcReader___Server_SendFire_2166136261));
			base.RegisterObserversRpc(38U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveFire_2166136261));
			base.RegisterObserversRpc(39U, new ClientRpcDelegate(this.RpcReader___Observers_SubmitNoWorkReason_15643032));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Employees.Employee));
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000B064C File Offset: 0x000AE84C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.EmployeeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.EmployeeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<PaidForToday>k__BackingField.SetRegistered();
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000B0670 File Offset: 0x000AE870
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000B0680 File Offset: 0x000AE880
		private void RpcWriter___Observers_Initialize_2260823878(NetworkConnection conn, string firstName, string lastName, string id, string guid, string propertyID, bool male, int appearanceIndex)
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
			writer.WriteString(firstName);
			writer.WriteString(lastName);
			writer.WriteString(id);
			writer.WriteString(guid);
			writer.WriteString(propertyID);
			writer.WriteBoolean(male);
			writer.WriteInt32(appearanceIndex, AutoPackType.Packed);
			base.SendObserversRpc(35U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x000B078C File Offset: 0x000AE98C
		public virtual void RpcLogic___Initialize_2260823878(NetworkConnection conn, string firstName, string lastName, string id, string guid, string propertyID, bool male, int appearanceIndex)
		{
			if (this.initialized)
			{
				return;
			}
			NetworkSingleton<EmployeeManager>.Instance.AllEmployees.Add(this);
			this.initialized = true;
			base.SetGUID(new Guid(guid));
			this.InitializeInfo(firstName, lastName, id);
			this.InitializeAppearance(male, appearanceIndex);
			this.AssignProperty(Singleton<PropertyManager>.Instance.GetProperty(propertyID));
			this.movement.Agent.avoidancePriority = 10 + appearanceIndex;
			if (InstanceFinder.IsServer)
			{
				ScheduleOne.GameTime.TimeManager.onSleepEnd = (Action<int>)Delegate.Combine(ScheduleOne.GameTime.TimeManager.onSleepEnd, new Action<int>(this.OnSleepEnd));
			}
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x000B0828 File Offset: 0x000AEA28
		private void RpcReader___Observers_Initialize_2260823878(PooledReader PooledReader0, Channel channel)
		{
			string firstName = PooledReader0.ReadString();
			string lastName = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			string guid = PooledReader0.ReadString();
			string propertyID = PooledReader0.ReadString();
			bool male = PooledReader0.ReadBoolean();
			int appearanceIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Initialize_2260823878(null, firstName, lastName, id, guid, propertyID, male, appearanceIndex);
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x000B08D0 File Offset: 0x000AEAD0
		private void RpcWriter___Target_Initialize_2260823878(NetworkConnection conn, string firstName, string lastName, string id, string guid, string propertyID, bool male, int appearanceIndex)
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
			writer.WriteString(firstName);
			writer.WriteString(lastName);
			writer.WriteString(id);
			writer.WriteString(guid);
			writer.WriteString(propertyID);
			writer.WriteBoolean(male);
			writer.WriteInt32(appearanceIndex, AutoPackType.Packed);
			base.SendTargetRpc(36U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x000B09D8 File Offset: 0x000AEBD8
		private void RpcReader___Target_Initialize_2260823878(PooledReader PooledReader0, Channel channel)
		{
			string firstName = PooledReader0.ReadString();
			string lastName = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			string guid = PooledReader0.ReadString();
			string propertyID = PooledReader0.ReadString();
			bool male = PooledReader0.ReadBoolean();
			int appearanceIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Initialize_2260823878(base.LocalConnection, firstName, lastName, id, guid, propertyID, male, appearanceIndex);
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x000B0A7C File Offset: 0x000AEC7C
		private void RpcWriter___Server_SendFire_2166136261()
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
			base.SendServerRpc(37U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x000B0B16 File Offset: 0x000AED16
		public void RpcLogic___SendFire_2166136261()
		{
			this.ReceiveFire();
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x000B0B20 File Offset: 0x000AED20
		private void RpcReader___Server_SendFire_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendFire_2166136261();
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x000B0B40 File Offset: 0x000AED40
		private void RpcWriter___Observers_ReceiveFire_2166136261()
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
			base.SendObserversRpc(38U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x000B0BE9 File Offset: 0x000AEDE9
		private void RpcLogic___ReceiveFire_2166136261()
		{
			this.Fire();
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x000B0BF4 File Offset: 0x000AEDF4
		private void RpcReader___Observers_ReceiveFire_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveFire_2166136261();
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x000B0C14 File Offset: 0x000AEE14
		private void RpcWriter___Observers_SubmitNoWorkReason_15643032(string reason, string fix, int priority = 0)
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
			writer.WriteString(reason);
			writer.WriteString(fix);
			writer.WriteInt32(priority, AutoPackType.Packed);
			base.SendObserversRpc(39U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x000B0CEC File Offset: 0x000AEEEC
		public void RpcLogic___SubmitNoWorkReason_15643032(string reason, string fix, int priority = 0)
		{
			Employee.NoWorkReason noWorkReason = new Employee.NoWorkReason(reason, fix, priority);
			for (int i = 0; i < this.WorkIssues.Count; i++)
			{
				if (this.WorkIssues[i].Priority < noWorkReason.Priority)
				{
					this.WorkIssues.Insert(i, noWorkReason);
					return;
				}
			}
			this.WorkIssues.Add(noWorkReason);
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x000B0D4C File Offset: 0x000AEF4C
		private void RpcReader___Observers_SubmitNoWorkReason_15643032(PooledReader PooledReader0, Channel channel)
		{
			string reason = PooledReader0.ReadString();
			string fix = PooledReader0.ReadString();
			int priority = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SubmitNoWorkReason_15643032(reason, fix, priority);
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x000B0DAE File Offset: 0x000AEFAE
		// (set) Token: 0x06002AF1 RID: 10993 RVA: 0x000B0DB6 File Offset: 0x000AEFB6
		public bool SyncAccessor_<PaidForToday>k__BackingField
		{
			get
			{
				return this.<PaidForToday>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<PaidForToday>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<PaidForToday>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x000B0DF4 File Offset: 0x000AEFF4
		public virtual bool Employee(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 1U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_<PaidForToday>k__BackingField(this.syncVar___<PaidForToday>k__BackingField.GetValue(true), true);
				return true;
			}
			bool value = PooledReader0.ReadBoolean();
			this.sync___set_value_<PaidForToday>k__BackingField(value, Boolean2);
			return true;
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x000B0E46 File Offset: 0x000AF046
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001F04 RID: 7940
		public bool DEBUG;

		// Token: 0x04001F0B RID: 7947
		[SerializeField]
		protected EEmployeeType Type;

		// Token: 0x04001F0C RID: 7948
		[Header("Payment")]
		public float SigningFee = 500f;

		// Token: 0x04001F0D RID: 7949
		public float DailyWage = 100f;

		// Token: 0x04001F0E RID: 7950
		[Header("References")]
		public IdleBehaviour WaitOutside;

		// Token: 0x04001F0F RID: 7951
		public MoveItemBehaviour MoveItemBehaviour;

		// Token: 0x04001F10 RID: 7952
		public DialogueContainer BedNotAssignedDialogue;

		// Token: 0x04001F11 RID: 7953
		public DialogueContainer NotPaidDialogue;

		// Token: 0x04001F12 RID: 7954
		public DialogueContainer WorkIssueDialogueTemplate;

		// Token: 0x04001F13 RID: 7955
		public DialogueContainer FireDialogue;

		// Token: 0x04001F14 RID: 7956
		private List<Employee.NoWorkReason> WorkIssues = new List<Employee.NoWorkReason>();

		// Token: 0x04001F16 RID: 7958
		protected bool initialized;

		// Token: 0x04001F17 RID: 7959
		public SyncVar<bool> syncVar___<PaidForToday>k__BackingField;

		// Token: 0x04001F18 RID: 7960
		private bool dll_Excuted;

		// Token: 0x04001F19 RID: 7961
		private bool dll_Excuted;

		// Token: 0x02000637 RID: 1591
		public class NoWorkReason
		{
			// Token: 0x06002AF4 RID: 10996 RVA: 0x000B0E5A File Offset: 0x000AF05A
			public NoWorkReason(string reason, string fix, int priority)
			{
				this.Reason = reason;
				this.Fix = fix;
				this.Priority = priority;
			}

			// Token: 0x04001F1A RID: 7962
			public string Reason;

			// Token: 0x04001F1B RID: 7963
			public string Fix;

			// Token: 0x04001F1C RID: 7964
			public int Priority;
		}
	}
}
