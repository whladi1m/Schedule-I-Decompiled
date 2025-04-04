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
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Money;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Property
{
	// Token: 0x020007F4 RID: 2036
	public class Business : Property, ISaveable
	{
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06003747 RID: 14151 RVA: 0x000EA94F File Offset: 0x000E8B4F
		public float currentLaunderTotal
		{
			get
			{
				return this.LaunderingOperations.Sum((LaunderingOperation x) => x.amount);
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06003748 RID: 14152 RVA: 0x000EA97B File Offset: 0x000E8B7B
		public float appliedLaunderLimit
		{
			get
			{
				return this.LaunderCapacity - this.currentLaunderTotal;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06003749 RID: 14153 RVA: 0x000EA98A File Offset: 0x000E8B8A
		public new string SaveFileName
		{
			get
			{
				return "Business";
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x0600374A RID: 14154 RVA: 0x000EA991 File Offset: 0x000E8B91
		public new Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x000EA999 File Offset: 0x000E8B99
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Property.Business_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x000EA9B0 File Offset: 0x000E8BB0
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance2.onTimeSkip = (Action<int>)Delegate.Combine(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x000EAA10 File Offset: 0x000E8C10
		protected override void OnDestroy()
		{
			Business.Businesses.Remove(this);
			Business.UnownedBusinesses.Remove(this);
			Business.OwnedBusinesses.Remove(this);
			base.OnDestroy();
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x000EAA3C File Offset: 0x000E8C3C
		protected override void GetNetworth(MoneyManager.FloatContainer container)
		{
			base.GetNetworth(container);
			container.ChangeValue(this.currentLaunderTotal);
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x000EAA54 File Offset: 0x000E8C54
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			for (int i = 0; i < this.LaunderingOperations.Count; i++)
			{
				this.ReceiveLaunderingOperation(connection, this.LaunderingOperations[i].amount, this.LaunderingOperations[i].minutesSinceStarted);
			}
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x000EAAA7 File Offset: 0x000E8CA7
		protected virtual void MinPass()
		{
			this.MinsPass(1);
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x000EAAB0 File Offset: 0x000E8CB0
		protected virtual void MinsPass(int mins)
		{
			for (int i = 0; i < this.LaunderingOperations.Count; i++)
			{
				this.LaunderingOperations[i].minutesSinceStarted += mins;
				if (this.LaunderingOperations[i].minutesSinceStarted >= this.LaunderingOperations[i].completionTime_Minutes)
				{
					this.CompleteOperation(this.LaunderingOperations[i]);
					i--;
				}
			}
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x000EAB26 File Offset: 0x000E8D26
		private void TimeSkipped(int minsPassed)
		{
			this.MinsPass(minsPassed);
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x000EAB30 File Offset: 0x000E8D30
		public override string GetSaveString()
		{
			bool[] array = new bool[this.Switches.Count];
			for (int i = 0; i < this.Switches.Count; i++)
			{
				array[i] = this.Switches[i].isOn;
			}
			LaunderOperationData[] array2 = new LaunderOperationData[this.LaunderingOperations.Count];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = new LaunderOperationData(this.LaunderingOperations[j].amount, this.LaunderingOperations[j].minutesSinceStarted);
			}
			bool[] array3 = new bool[this.Toggleables.Count];
			for (int k = 0; k < this.Toggleables.Count; k++)
			{
				array3[k] = this.Toggleables[k].IsActivated;
			}
			return new BusinessData(this.propertyCode, base.IsOwned, array, array2, array3).GetJson(true);
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x000EAC24 File Offset: 0x000E8E24
		public virtual void Load(BusinessData businessData, string containerPath)
		{
			if (businessData.IsOwned)
			{
				base.SetOwned();
			}
			for (int i = 0; i < businessData.LaunderingOperations.Length; i++)
			{
				this.StartLaunderingOperation(businessData.LaunderingOperations[i].Amount, businessData.LaunderingOperations[i].MinutesSinceStarted);
			}
		}

		// Token: 0x06003755 RID: 14165 RVA: 0x000EAC72 File Offset: 0x000E8E72
		protected override void RecieveOwned()
		{
			base.RecieveOwned();
			Business.UnownedBusinesses.Remove(this);
			if (!Business.OwnedBusinesses.Contains(this))
			{
				Business.OwnedBusinesses.Add(this);
			}
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x000EAC9E File Offset: 0x000E8E9E
		[ServerRpc(RequireOwnership = false)]
		public void StartLaunderingOperation(float amount, int minutesSinceStarted = 0)
		{
			this.RpcWriter___Server_StartLaunderingOperation_1481775633(amount, minutesSinceStarted);
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x000EACB0 File Offset: 0x000E8EB0
		[TargetRpc]
		[ObserversRpc]
		private void ReceiveLaunderingOperation(NetworkConnection conn, float amount, int minutesSinceStarted = 0)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveLaunderingOperation_1001022388(conn, amount, minutesSinceStarted);
			}
			else
			{
				this.RpcWriter___Target_ReceiveLaunderingOperation_1001022388(conn, amount, minutesSinceStarted);
			}
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x000EACEC File Offset: 0x000E8EEC
		protected void CompleteOperation(LaunderingOperation op)
		{
			if (InstanceFinder.IsServer)
			{
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Money laundering (" + this.propertyName + ")", op.amount, 1f, string.Empty);
				float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("LaunderingOperationsCompleted");
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("LaunderingOperationsCompleted", (value + 1f).ToString(), true);
			}
			Singleton<NotificationsManager>.Instance.SendNotification(this.propertyName, "<color=#16F01C>" + MoneyManager.FormatAmount(op.amount, false, false) + "</color> Laundered", NetworkSingleton<MoneyManager>.Instance.LaunderingNotificationIcon, 5f, true);
			this.LaunderingOperations.Remove(op);
			base.HasChanged = true;
			if (Business.onOperationFinished != null)
			{
				Business.onOperationFinished(op);
			}
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x000EAE0C File Offset: 0x000E900C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.BusinessAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.BusinessAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_StartLaunderingOperation_1481775633));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveLaunderingOperation_1001022388));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveLaunderingOperation_1001022388));
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x000EAE75 File Offset: 0x000E9075
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.BusinessAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.BusinessAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x000EAE8E File Offset: 0x000E908E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x000EAE9C File Offset: 0x000E909C
		private void RpcWriter___Server_StartLaunderingOperation_1481775633(float amount, int minutesSinceStarted = 0)
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
			writer.WriteSingle(amount, AutoPackType.Unpacked);
			writer.WriteInt32(minutesSinceStarted, AutoPackType.Packed);
			base.SendServerRpc(5U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x000EAF5A File Offset: 0x000E915A
		public void RpcLogic___StartLaunderingOperation_1481775633(float amount, int minutesSinceStarted = 0)
		{
			this.ReceiveLaunderingOperation(null, amount, minutesSinceStarted);
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x000EAF68 File Offset: 0x000E9168
		private void RpcReader___Server_StartLaunderingOperation_1481775633(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float amount = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			int minutesSinceStarted = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___StartLaunderingOperation_1481775633(amount, minutesSinceStarted);
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x000EAFB4 File Offset: 0x000E91B4
		private void RpcWriter___Target_ReceiveLaunderingOperation_1001022388(NetworkConnection conn, float amount, int minutesSinceStarted = 0)
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
			writer.WriteSingle(amount, AutoPackType.Unpacked);
			writer.WriteInt32(minutesSinceStarted, AutoPackType.Packed);
			base.SendTargetRpc(6U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x000EB080 File Offset: 0x000E9280
		private void RpcLogic___ReceiveLaunderingOperation_1001022388(NetworkConnection conn, float amount, int minutesSinceStarted = 0)
		{
			LaunderingOperation launderingOperation = new LaunderingOperation(this, amount, minutesSinceStarted);
			this.LaunderingOperations.Add(launderingOperation);
			base.HasChanged = true;
			if (Business.onOperationStarted != null)
			{
				Business.onOperationStarted(launderingOperation);
			}
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x000EB0BC File Offset: 0x000E92BC
		private void RpcReader___Target_ReceiveLaunderingOperation_1001022388(PooledReader PooledReader0, Channel channel)
		{
			float amount = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			int minutesSinceStarted = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveLaunderingOperation_1001022388(base.LocalConnection, amount, minutesSinceStarted);
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x000EB110 File Offset: 0x000E9310
		private void RpcWriter___Observers_ReceiveLaunderingOperation_1001022388(NetworkConnection conn, float amount, int minutesSinceStarted = 0)
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
			writer.WriteSingle(amount, AutoPackType.Unpacked);
			writer.WriteInt32(minutesSinceStarted, AutoPackType.Packed);
			base.SendObserversRpc(7U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x000EB1E0 File Offset: 0x000E93E0
		private void RpcReader___Observers_ReceiveLaunderingOperation_1001022388(PooledReader PooledReader0, Channel channel)
		{
			float amount = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			int minutesSinceStarted = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveLaunderingOperation_1001022388(null, amount, minutesSinceStarted);
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x000EB22D File Offset: 0x000E942D
		protected virtual void dll()
		{
			base.Awake();
			Business.Businesses.Add(this);
			Business.UnownedBusinesses.Remove(this);
			Business.UnownedBusinesses.Add(this);
		}

		// Token: 0x04002875 RID: 10357
		public static List<Business> Businesses = new List<Business>();

		// Token: 0x04002876 RID: 10358
		public static List<Business> UnownedBusinesses = new List<Business>();

		// Token: 0x04002877 RID: 10359
		public static List<Business> OwnedBusinesses = new List<Business>();

		// Token: 0x04002878 RID: 10360
		[Header("Settings")]
		public float LaunderCapacity = 1000f;

		// Token: 0x04002879 RID: 10361
		public List<LaunderingOperation> LaunderingOperations = new List<LaunderingOperation>();

		// Token: 0x0400287A RID: 10362
		public static Action<LaunderingOperation> onOperationStarted;

		// Token: 0x0400287B RID: 10363
		public static Action<LaunderingOperation> onOperationFinished;

		// Token: 0x0400287C RID: 10364
		private BusinessLoader loader = new BusinessLoader();

		// Token: 0x0400287D RID: 10365
		private bool dll_Excuted;

		// Token: 0x0400287E RID: 10366
		private bool dll_Excuted;
	}
}
