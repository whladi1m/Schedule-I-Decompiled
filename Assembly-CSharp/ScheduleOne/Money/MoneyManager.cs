using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Money
{
	// Token: 0x02000B61 RID: 2913
	public class MoneyManager : NetworkSingleton<MoneyManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06004D7D RID: 19837 RVA: 0x00147063 File Offset: 0x00145263
		public float LifetimeEarnings
		{
			get
			{
				return this.SyncAccessor_lifetimeEarnings;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06004D7E RID: 19838 RVA: 0x0014706B File Offset: 0x0014526B
		// (set) Token: 0x06004D7F RID: 19839 RVA: 0x00147073 File Offset: 0x00145273
		public float LastCalculatedNetworth { get; protected set; }

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06004D80 RID: 19840 RVA: 0x0014707C File Offset: 0x0014527C
		public float cashBalance
		{
			get
			{
				return this.cashInstance.Balance;
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06004D81 RID: 19841 RVA: 0x00147089 File Offset: 0x00145289
		protected CashInstance cashInstance
		{
			get
			{
				return PlayerSingleton<PlayerInventory>.Instance.cashInstance;
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06004D82 RID: 19842 RVA: 0x00147095 File Offset: 0x00145295
		public string SaveFolderName
		{
			get
			{
				return "Money";
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06004D83 RID: 19843 RVA: 0x00147095 File Offset: 0x00145295
		public string SaveFileName
		{
			get
			{
				return "Money";
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06004D84 RID: 19844 RVA: 0x0014709C File Offset: 0x0014529C
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06004D85 RID: 19845 RVA: 0x00014002 File Offset: 0x00012202
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06004D86 RID: 19846 RVA: 0x001470A4 File Offset: 0x001452A4
		// (set) Token: 0x06004D87 RID: 19847 RVA: 0x001470AC File Offset: 0x001452AC
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06004D88 RID: 19848 RVA: 0x001470B5 File Offset: 0x001452B5
		// (set) Token: 0x06004D89 RID: 19849 RVA: 0x001470BD File Offset: 0x001452BD
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06004D8A RID: 19850 RVA: 0x001470C6 File Offset: 0x001452C6
		// (set) Token: 0x06004D8B RID: 19851 RVA: 0x001470CE File Offset: 0x001452CE
		public bool HasChanged { get; set; }

		// Token: 0x06004D8C RID: 19852 RVA: 0x001470D7 File Offset: 0x001452D7
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Money.MoneyManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x001470EC File Offset: 0x001452EC
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance2.onDayPass = (Action)Delegate.Combine(instance2.onDayPass, new Action(this.CheckNetworthAchievements));
			Singleton<HUD>.Instance.OnlineBalanceDisplay.SetBalance(this.SyncAccessor_onlineBalance);
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x0014717B File Offset: 0x0014537B
		public override void OnStartServer()
		{
			base.OnStartServer();
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("LifetimeEarnings", this.lifetimeEarnings.ToString(), true);
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x0014719E File Offset: 0x0014539E
		public override void OnStartClient()
		{
			base.OnStartClient();
			Singleton<HUD>.Instance.OnlineBalanceDisplay.SetBalance(this.SyncAccessor_onlineBalance);
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x001471BC File Offset: 0x001453BC
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
				TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance2.onDayPass = (Action)Delegate.Remove(instance2.onDayPass, new Action(this.CheckNetworthAchievements));
			}
			if (Singleton<LoadManager>.InstanceExists)
			{
				Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			}
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x00147244 File Offset: 0x00145444
		private void Loaded()
		{
			this.GetNetWorth();
			Singleton<HUD>.Instance.OnlineBalanceDisplay.SetBalance(this.SyncAccessor_onlineBalance);
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x00147262 File Offset: 0x00145462
		private void Update()
		{
			this.HasChanged = true;
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x0014726C File Offset: 0x0014546C
		private void MinPass()
		{
			if (NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Online_Balance", this.onlineBalance.ToString(), false);
				if (PlayerSingleton<PlayerInventory>.InstanceExists)
				{
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Cash_Balance", this.cashBalance.ToString(), false);
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Total_Money", (this.SyncAccessor_onlineBalance + this.cashBalance).ToString(), false);
				}
			}
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x001472E5 File Offset: 0x001454E5
		public CashInstance GetCashInstance(float amount)
		{
			CashInstance cashInstance = Registry.GetItem<CashDefinition>("cash").GetDefaultInstance(1) as CashInstance;
			cashInstance.SetBalance(amount, false);
			return cashInstance;
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x00147304 File Offset: 0x00145504
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void CreateOnlineTransaction(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
		{
			this.RpcWriter___Server_CreateOnlineTransaction_1419830531(_transaction_Name, _unit_Amount, _quantity, _transaction_Note);
			this.RpcLogic___CreateOnlineTransaction_1419830531(_transaction_Name, _unit_Amount, _quantity, _transaction_Note);
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x00147334 File Offset: 0x00145534
		[ObserversRpc]
		private void ReceiveOnlineTransaction(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
		{
			this.RpcWriter___Observers_ReceiveOnlineTransaction_1419830531(_transaction_Name, _unit_Amount, _quantity, _transaction_Note);
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x00147357 File Offset: 0x00145557
		protected IEnumerator ShowOnlineBalanceChange(RectTransform changeDisplay)
		{
			TextMeshProUGUI text = changeDisplay.GetComponent<TextMeshProUGUI>();
			float startVert = changeDisplay.anchoredPosition.y;
			float lerpTime = 2.5f;
			float vertOffset = startVert + 60f;
			for (float i = 0f; i < lerpTime; i += Time.unscaledDeltaTime)
			{
				text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1f, 0f, i / lerpTime));
				changeDisplay.anchoredPosition = new Vector2(changeDisplay.anchoredPosition.x, Mathf.Lerp(startVert, vertOffset, i / lerpTime));
				yield return new WaitForEndOfFrame();
			}
			UnityEngine.Object.Destroy(changeDisplay.gameObject);
			yield break;
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x00147366 File Offset: 0x00145566
		[ServerRpc(RequireOwnership = false)]
		public void ChangeLifetimeEarnings(float change)
		{
			this.RpcWriter___Server_ChangeLifetimeEarnings_431000436(change);
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x00147374 File Offset: 0x00145574
		public void ChangeCashBalance(float change, bool visualizeChange = true, bool playCashSound = false)
		{
			float num = Mathf.Clamp(this.cashInstance.Balance + change, 0f, float.MaxValue) - this.cashInstance.Balance;
			this.cashInstance.ChangeBalance(change);
			if (playCashSound && num != 0f)
			{
				this.CashSound.Play();
			}
			if (visualizeChange && num != 0f)
			{
				RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.cashChangePrefab, Singleton<HUD>.Instance.cashSlotContainer).GetComponent<RectTransform>();
				component.position = new Vector3(Singleton<HUD>.Instance.cashSlotUI.position.x, component.position.y);
				component.anchoredPosition = new Vector2(component.anchoredPosition.x, 10f);
				TextMeshProUGUI component2 = component.GetComponent<TextMeshProUGUI>();
				if (num > 0f)
				{
					component2.text = "+ " + MoneyManager.FormatAmount(num, false, false);
					component2.color = new Color32(25, 240, 30, byte.MaxValue);
				}
				else
				{
					component2.text = MoneyManager.FormatAmount(num, false, false);
					component2.color = new Color32(176, 63, 59, byte.MaxValue);
				}
				Singleton<CoroutineService>.Instance.StartCoroutine(this.ShowCashChange(component));
			}
		}

		// Token: 0x06004D9B RID: 19867 RVA: 0x001474C2 File Offset: 0x001456C2
		protected IEnumerator ShowCashChange(RectTransform changeDisplay)
		{
			TextMeshProUGUI text = changeDisplay.GetComponent<TextMeshProUGUI>();
			float startVert = changeDisplay.anchoredPosition.y;
			float lerpTime = 2.5f;
			float vertOffset = startVert + 60f;
			for (float i = 0f; i < lerpTime; i += Time.unscaledDeltaTime)
			{
				text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1f, 0f, i / lerpTime));
				changeDisplay.anchoredPosition = new Vector2(changeDisplay.anchoredPosition.x, Mathf.Lerp(startVert, vertOffset, i / lerpTime));
				yield return new WaitForEndOfFrame();
			}
			UnityEngine.Object.Destroy(changeDisplay.gameObject);
			yield break;
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x001474D4 File Offset: 0x001456D4
		public static string FormatAmount(float amount, bool showDecimals = false, bool includeColor = false)
		{
			string text = string.Empty;
			if (includeColor)
			{
				text += "<color=#54E717>";
			}
			if (amount < 0f)
			{
				text = "-";
			}
			if (showDecimals)
			{
				text += string.Format(new CultureInfo("en-US"), "{0:C}", Mathf.Abs(amount));
			}
			else
			{
				text += string.Format(new CultureInfo("en-US"), "{0:C0}", Mathf.RoundToInt(Mathf.Abs(amount)));
			}
			if (includeColor)
			{
				text += "</color>";
			}
			return text;
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x0014756A File Offset: 0x0014576A
		public virtual string GetSaveString()
		{
			return new MoneyData(this.SyncAccessor_onlineBalance, this.GetNetWorth(), this.SyncAccessor_lifetimeEarnings, ATM.WeeklyDepositSum).GetJson(true);
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x00147590 File Offset: 0x00145790
		public void Load(MoneyData data)
		{
			this.sync___set_value_onlineBalance(Mathf.Clamp(data.OnlineBalance, 0f, float.MaxValue), true);
			this.sync___set_value_lifetimeEarnings(Mathf.Clamp(data.LifetimeEarnings, 0f, float.MaxValue), true);
			Singleton<HUD>.Instance.OnlineBalanceDisplay.SetBalance(this.SyncAccessor_onlineBalance);
			ATM.WeeklyDepositSum = data.WeeklyDepositSum;
		}

		// Token: 0x06004D9F RID: 19871 RVA: 0x001475F5 File Offset: 0x001457F5
		public void CheckNetworthAchievements()
		{
			float netWorth = this.GetNetWorth();
			if (netWorth >= 100000f)
			{
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.BUSINESSMAN);
			}
			if (netWorth >= 1000000f)
			{
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.BIGWIG);
			}
			if (netWorth >= 10000000f)
			{
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.MAGNATE);
			}
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x00147638 File Offset: 0x00145838
		public float GetNetWorth()
		{
			float num = 0f;
			num += this.SyncAccessor_onlineBalance;
			if (this.onNetworthCalculation != null)
			{
				MoneyManager.FloatContainer floatContainer = new MoneyManager.FloatContainer();
				this.onNetworthCalculation(floatContainer);
				num += floatContainer.value;
			}
			this.LastCalculatedNetworth = num;
			return num;
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x001476B4 File Offset: 0x001458B4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Money.MoneyManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Money.MoneyManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___lifetimeEarnings = new SyncVar<float>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.lifetimeEarnings);
			this.syncVar___onlineBalance = new SyncVar<float>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.onlineBalance);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_CreateOnlineTransaction_1419830531));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveOnlineTransaction_1419830531));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_ChangeLifetimeEarnings_431000436));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Money.MoneyManager));
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x00147785 File Offset: 0x00145985
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Money.MoneyManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Money.MoneyManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___lifetimeEarnings.SetRegistered();
			this.syncVar___onlineBalance.SetRegistered();
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x001477B4 File Offset: 0x001459B4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x001477C4 File Offset: 0x001459C4
		private void RpcWriter___Server_CreateOnlineTransaction_1419830531(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
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
			writer.WriteString(_transaction_Name);
			writer.WriteSingle(_unit_Amount, AutoPackType.Unpacked);
			writer.WriteSingle(_quantity, AutoPackType.Unpacked);
			writer.WriteString(_transaction_Note);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x0014789C File Offset: 0x00145A9C
		public void RpcLogic___CreateOnlineTransaction_1419830531(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
		{
			this.ReceiveOnlineTransaction(_transaction_Name, _unit_Amount, _quantity, _transaction_Note);
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x001478AC File Offset: 0x00145AAC
		private void RpcReader___Server_CreateOnlineTransaction_1419830531(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string transaction_Name = PooledReader0.ReadString();
			float unit_Amount = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			float quantity = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			string transaction_Note = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CreateOnlineTransaction_1419830531(transaction_Name, unit_Amount, quantity, transaction_Note);
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x00147928 File Offset: 0x00145B28
		private void RpcWriter___Observers_ReceiveOnlineTransaction_1419830531(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
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
			writer.WriteString(_transaction_Name);
			writer.WriteSingle(_unit_Amount, AutoPackType.Unpacked);
			writer.WriteSingle(_quantity, AutoPackType.Unpacked);
			writer.WriteString(_transaction_Note);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x00147A10 File Offset: 0x00145C10
		private void RpcLogic___ReceiveOnlineTransaction_1419830531(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
		{
			Transaction transaction = new Transaction(_transaction_Name, _unit_Amount, _quantity, _transaction_Note);
			this.ledger.Add(transaction);
			this.sync___set_value_onlineBalance(this.SyncAccessor_onlineBalance + transaction.total_Amount, true);
			Singleton<HUD>.Instance.OnlineBalanceDisplay.SetBalance(this.SyncAccessor_onlineBalance);
			Singleton<HUD>.Instance.OnlineBalanceDisplay.Show();
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.moneyChangePrefab, Singleton<HUD>.Instance.cashSlotContainer).GetComponent<RectTransform>();
			component.position = new Vector3(Singleton<HUD>.Instance.onlineBalanceSlotUI.position.x, component.position.y);
			component.anchoredPosition = new Vector2(component.anchoredPosition.x, 10f);
			TextMeshProUGUI component2 = component.GetComponent<TextMeshProUGUI>();
			if (transaction.total_Amount > 0f)
			{
				component2.text = "+ " + MoneyManager.FormatAmount(transaction.total_Amount, false, false);
				component2.color = new Color32(25, 190, 240, byte.MaxValue);
			}
			else
			{
				component2.text = MoneyManager.FormatAmount(transaction.total_Amount, false, false);
				component2.color = new Color32(176, 63, 59, byte.MaxValue);
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(this.ShowOnlineBalanceChange(component));
			this.HasChanged = true;
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x00147B6C File Offset: 0x00145D6C
		private void RpcReader___Observers_ReceiveOnlineTransaction_1419830531(PooledReader PooledReader0, Channel channel)
		{
			string transaction_Name = PooledReader0.ReadString();
			float unit_Amount = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			float quantity = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			string transaction_Note = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveOnlineTransaction_1419830531(transaction_Name, unit_Amount, quantity, transaction_Note);
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x00147BDC File Offset: 0x00145DDC
		private void RpcWriter___Server_ChangeLifetimeEarnings_431000436(float change)
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
			writer.WriteSingle(change, AutoPackType.Unpacked);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x00147C88 File Offset: 0x00145E88
		public void RpcLogic___ChangeLifetimeEarnings_431000436(float change)
		{
			this.sync___set_value_lifetimeEarnings(Mathf.Clamp(this.SyncAccessor_lifetimeEarnings + change, 0f, float.MaxValue), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("LifetimeEarnings", this.lifetimeEarnings.ToString(), true);
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x00147CC4 File Offset: 0x00145EC4
		private void RpcReader___Server_ChangeLifetimeEarnings_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float change = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___ChangeLifetimeEarnings_431000436(change);
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06004DAE RID: 19886 RVA: 0x00147CFA File Offset: 0x00145EFA
		// (set) Token: 0x06004DAF RID: 19887 RVA: 0x00147D02 File Offset: 0x00145F02
		public float SyncAccessor_onlineBalance
		{
			get
			{
				return this.onlineBalance;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.onlineBalance = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___onlineBalance.SetValue(value, value);
				}
			}
		}

		// Token: 0x06004DB0 RID: 19888 RVA: 0x00147D40 File Offset: 0x00145F40
		public virtual bool MoneyManager(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_lifetimeEarnings(this.syncVar___lifetimeEarnings.GetValue(true), true);
					return true;
				}
				float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
				this.sync___set_value_lifetimeEarnings(value, Boolean2);
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
					this.sync___set_value_onlineBalance(this.syncVar___onlineBalance.GetValue(true), true);
					return true;
				}
				float value2 = PooledReader0.ReadSingle(AutoPackType.Unpacked);
				this.sync___set_value_onlineBalance(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06004DB1 RID: 19889 RVA: 0x00147DE0 File Offset: 0x00145FE0
		// (set) Token: 0x06004DB2 RID: 19890 RVA: 0x00147DE8 File Offset: 0x00145FE8
		public float SyncAccessor_lifetimeEarnings
		{
			get
			{
				return this.lifetimeEarnings;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.lifetimeEarnings = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___lifetimeEarnings.SetValue(value, value);
				}
			}
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x00147E24 File Offset: 0x00146024
		protected virtual void dll()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x04003AAF RID: 15023
		public const string MONEY_TEXT_COLOR = "#54E717";

		// Token: 0x04003AB0 RID: 15024
		public const string MONEY_TEXT_COLOR_DARKER = "#46CB4F";

		// Token: 0x04003AB1 RID: 15025
		public const string ONLINE_BALANCE_COLOR = "#4CBFFF";

		// Token: 0x04003AB2 RID: 15026
		public List<Transaction> ledger = new List<Transaction>();

		// Token: 0x04003AB3 RID: 15027
		[SyncVar(WritePermissions = WritePermission.ClientUnsynchronized)]
		public float onlineBalance;

		// Token: 0x04003AB4 RID: 15028
		[SyncVar(WritePermissions = WritePermission.ClientUnsynchronized)]
		public float lifetimeEarnings;

		// Token: 0x04003AB6 RID: 15030
		public AudioSourceController CashSound;

		// Token: 0x04003AB7 RID: 15031
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject moneyChangePrefab;

		// Token: 0x04003AB8 RID: 15032
		[SerializeField]
		protected GameObject cashChangePrefab;

		// Token: 0x04003AB9 RID: 15033
		public Sprite LaunderingNotificationIcon;

		// Token: 0x04003ABA RID: 15034
		public Action<MoneyManager.FloatContainer> onNetworthCalculation;

		// Token: 0x04003ABB RID: 15035
		private MoneyLoader loader = new MoneyLoader();

		// Token: 0x04003ABF RID: 15039
		public SyncVar<float> syncVar___onlineBalance;

		// Token: 0x04003AC0 RID: 15040
		public SyncVar<float> syncVar___lifetimeEarnings;

		// Token: 0x04003AC1 RID: 15041
		private bool dll_Excuted;

		// Token: 0x04003AC2 RID: 15042
		private bool dll_Excuted;

		// Token: 0x02000B62 RID: 2914
		public class FloatContainer
		{
			// Token: 0x17000AB6 RID: 2742
			// (get) Token: 0x06004DB4 RID: 19892 RVA: 0x00147E32 File Offset: 0x00146032
			// (set) Token: 0x06004DB5 RID: 19893 RVA: 0x00147E3A File Offset: 0x0014603A
			public float value { get; private set; }

			// Token: 0x06004DB6 RID: 19894 RVA: 0x00147E43 File Offset: 0x00146043
			public void ChangeValue(float value)
			{
				this.value += value;
			}
		}
	}
}
