using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.Interaction;
using ScheduleOne.Management;
using ScheduleOne.Map;
using ScheduleOne.Misc;
using ScheduleOne.Money;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Management;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Property
{
	// Token: 0x020007FB RID: 2043
	public class Property : NetworkBehaviour, ISaveable
	{
		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06003788 RID: 14216 RVA: 0x000EB603 File Offset: 0x000E9803
		// (set) Token: 0x06003789 RID: 14217 RVA: 0x000EB60B File Offset: 0x000E980B
		public bool IsOwned { get; protected set; }

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x0600378A RID: 14218 RVA: 0x000EB614 File Offset: 0x000E9814
		// (set) Token: 0x0600378B RID: 14219 RVA: 0x000EB61C File Offset: 0x000E981C
		public List<Employee> Employees { get; protected set; } = new List<Employee>();

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x0600378C RID: 14220 RVA: 0x000EB625 File Offset: 0x000E9825
		// (set) Token: 0x0600378D RID: 14221 RVA: 0x000EB62D File Offset: 0x000E982D
		public RectTransform WorldspaceUIContainer { get; protected set; }

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x0600378E RID: 14222 RVA: 0x000EB636 File Offset: 0x000E9836
		// (set) Token: 0x0600378F RID: 14223 RVA: 0x000EB63E File Offset: 0x000E983E
		public bool IsContentCulled { get; set; }

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06003790 RID: 14224 RVA: 0x000EB647 File Offset: 0x000E9847
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06003791 RID: 14225 RVA: 0x000EB64F File Offset: 0x000E984F
		public string PropertyCode
		{
			get
			{
				return this.propertyCode;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06003792 RID: 14226 RVA: 0x000EB657 File Offset: 0x000E9857
		public int LoadingDockCount
		{
			get
			{
				return this.LoadingDocks.Length;
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06003793 RID: 14227 RVA: 0x000EB647 File Offset: 0x000E9847
		public string SaveFolderName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06003794 RID: 14228 RVA: 0x000EB661 File Offset: 0x000E9861
		public string SaveFileName
		{
			get
			{
				return "Property";
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06003795 RID: 14229 RVA: 0x000EB668 File Offset: 0x000E9868
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06003796 RID: 14230 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06003797 RID: 14231 RVA: 0x000EB670 File Offset: 0x000E9870
		// (set) Token: 0x06003798 RID: 14232 RVA: 0x000EB678 File Offset: 0x000E9878
		public List<string> LocalExtraFiles { get; set; } = new List<string>
		{
			"Safe"
		};

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06003799 RID: 14233 RVA: 0x000EB681 File Offset: 0x000E9881
		// (set) Token: 0x0600379A RID: 14234 RVA: 0x000EB689 File Offset: 0x000E9889
		public List<string> LocalExtraFolders { get; set; } = new List<string>
		{
			"Objects",
			"Employees"
		};

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600379B RID: 14235 RVA: 0x000EB692 File Offset: 0x000E9892
		// (set) Token: 0x0600379C RID: 14236 RVA: 0x000EB69A File Offset: 0x000E989A
		public bool HasChanged { get; set; }

		// Token: 0x0600379D RID: 14237 RVA: 0x000EB6A4 File Offset: 0x000E98A4
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Property.Property_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x000EB6C3 File Offset: 0x000E98C3
		protected virtual void Start()
		{
			MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
			instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Combine(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x000EB6EC File Offset: 0x000E98EC
		protected virtual void FixedUpdate()
		{
			this.UpdateCulling();
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x000EB6F4 File Offset: 0x000E98F4
		public void AddConfigurable(IConfigurable configurable)
		{
			if (this.Configurables.Contains(configurable))
			{
				return;
			}
			this.Configurables.Add(configurable);
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x000EB711 File Offset: 0x000E9911
		public void RemoveConfigurable(IConfigurable configurable)
		{
			if (!this.Configurables.Contains(configurable))
			{
				return;
			}
			this.Configurables.Remove(configurable);
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x000EB730 File Offset: 0x000E9930
		private void UpdateCulling()
		{
			if (!Singleton<LoadManager>.InstanceExists || Singleton<LoadManager>.Instance.IsLoading)
			{
				return;
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (!this.ContentCullingEnabled)
			{
				this.SetContentCulled(false);
			}
			float num = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, base.transform.position);
			if (num < this.MinimumCullingDistance)
			{
				this.SetContentCulled(false);
				return;
			}
			if (num > this.MinimumCullingDistance + 5f)
			{
				this.SetContentCulled(true);
			}
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x000EB7B0 File Offset: 0x000E99B0
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			for (int i = 0; i < this.Toggleables.Count; i++)
			{
				if (this.Toggleables[i].IsActivated)
				{
					this.SetToggleableState(connection, i, this.Toggleables[i].IsActivated);
				}
			}
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x000EB808 File Offset: 0x000E9A08
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<MoneyManager>.InstanceExists)
			{
				MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
				instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Remove(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			}
			Property.Properties.Remove(this);
			Property.UnownedProperties.Remove(this);
			Property.OwnedProperties.Remove(this);
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x000EB867 File Offset: 0x000E9A67
		protected virtual void GetNetworth(MoneyManager.FloatContainer container)
		{
			if (this.IsOwned)
			{
				container.ChangeValue(this.Price);
			}
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x000EB880 File Offset: 0x000E9A80
		public override void OnStartServer()
		{
			base.OnStartServer();
			if ((Application.isEditor || Debug.isDebugBuild) && this.DEBUG_SET_OWNED)
			{
				this.SetOwned_Server();
			}
			else if (this.OwnedByDefault)
			{
				this.SetOwned_Server();
			}
			if (base.NetworkObject.GetInitializeOrder() == 0)
			{
				Console.LogError("Property " + this.PropertyName + " has an initialize order of 0. This will cause issues.", null);
			}
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x000EB8E7 File Offset: 0x000E9AE7
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		protected void SetOwned_Server()
		{
			this.RpcWriter___Server_SetOwned_Server_2166136261();
			this.RpcLogic___SetOwned_Server_2166136261();
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x000EB8F5 File Offset: 0x000E9AF5
		[ObserversRpc(RunLocally = true, BufferLast = true)]
		private void ReceiveOwned_Networked()
		{
			this.RpcWriter___Observers_ReceiveOwned_Networked_2166136261();
			this.RpcLogic___ReceiveOwned_Networked_2166136261();
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x000EB904 File Offset: 0x000E9B04
		protected virtual void RecieveOwned()
		{
			if (this.IsOwned)
			{
				return;
			}
			this.IsOwned = true;
			this.HasChanged = true;
			if (this.IsOwnedVariable != string.Empty && NetworkSingleton<VariableDatabase>.InstanceExists && InstanceFinder.IsServer)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.IsOwnedVariable, "true", true);
			}
			if (Property.UnownedProperties.Contains(this))
			{
				Property.UnownedProperties.Remove(this);
				Property.OwnedProperties.Add(this);
			}
			if (Property.onPropertyAcquired != null)
			{
				Property.onPropertyAcquired(this);
			}
			if (this.onThisPropertyAcquired != null)
			{
				this.onThisPropertyAcquired.Invoke();
			}
			this.ForSaleSign.gameObject.SetActive(false);
			if (this.ListingPoster != null)
			{
				this.ListingPoster.gameObject.SetActive(false);
			}
			this.PoI.gameObject.SetActive(true);
			this.PoI.SetMainText(this.propertyName + " (Owned)");
			base.StartCoroutine(this.<RecieveOwned>g__Wait|93_0());
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x000EBA10 File Offset: 0x000E9C10
		public virtual bool ShouldSave()
		{
			return this.IsOwned || this.Container.transform.childCount > 0;
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x000EBA2F File Offset: 0x000E9C2F
		public void SetOwned()
		{
			this.SetOwned_Server();
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x000045B1 File Offset: 0x000027B1
		public void SetBoundsVisible(bool vis)
		{
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x000EBA38 File Offset: 0x000E9C38
		public virtual void SetContentCulled(bool culled)
		{
			if (this.IsContentCulled == culled)
			{
				return;
			}
			this.IsContentCulled = culled;
			foreach (BuildableItem buildableItem in this.BuildableItems)
			{
				if (!(buildableItem == null))
				{
					buildableItem.SetCulled(culled);
				}
			}
			foreach (GameObject gameObject in this.ObjectsToCull)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(!culled);
				}
			}
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x000EBAD4 File Offset: 0x000E9CD4
		public int RegisterEmployee(Employee emp)
		{
			this.Employees.Add(emp);
			return this.Employees.IndexOf(emp);
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x000EBAEE File Offset: 0x000E9CEE
		public void DeregisterEmployee(Employee emp)
		{
			this.Employees.Remove(emp);
		}

		// Token: 0x060037B1 RID: 14257 RVA: 0x000EBAFD File Offset: 0x000E9CFD
		private void ToggleableActioned(InteractableToggleable toggleable)
		{
			this.HasChanged = true;
			this.SendToggleableState(this.Toggleables.IndexOf(toggleable), toggleable.IsActivated);
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x000EBB1E File Offset: 0x000E9D1E
		[ServerRpc(RequireOwnership = false)]
		public void SendToggleableState(int index, bool state)
		{
			this.RpcWriter___Server_SendToggleableState_3658436649(index, state);
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x000EBB2E File Offset: 0x000E9D2E
		[ObserversRpc]
		[TargetRpc]
		public void SetToggleableState(NetworkConnection conn, int index, bool state)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetToggleableState_338960014(conn, index, state);
			}
			else
			{
				this.RpcWriter___Target_SetToggleableState_338960014(conn, index, state);
			}
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x000EBB60 File Offset: 0x000E9D60
		public virtual string GetSaveString()
		{
			bool[] array = new bool[this.Switches.Count];
			for (int i = 0; i < this.Switches.Count; i++)
			{
				if (!(this.Switches[i] == null))
				{
					array[i] = this.Switches[i].isOn;
				}
			}
			bool[] array2 = new bool[this.Toggleables.Count];
			for (int j = 0; j < this.Toggleables.Count; j++)
			{
				if (!(this.Toggleables[j] == null))
				{
					array2[j] = this.Toggleables[j].IsActivated;
				}
			}
			return new PropertyData(this.propertyCode, this.IsOwned, array, array2).GetJson(true);
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x000EBC24 File Offset: 0x000E9E24
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> result = new List<string>();
			this.savedObjectPaths.Clear();
			this.savedEmployeePaths.Clear();
			string parentFolderPath2 = ((ISaveable)this).WriteFolder(parentFolderPath, "Objects");
			foreach (BuildableItem buildableItem in this.BuildableItems)
			{
				try
				{
					new SaveRequest(buildableItem, parentFolderPath2);
					this.savedObjectPaths.Add(buildableItem.SaveFolderName);
				}
				catch (Exception ex)
				{
					Console.LogError("Error saving object: " + ex.Message, null);
					SaveManager.ReportSaveError();
				}
			}
			string parentFolderPath3 = ((ISaveable)this).WriteFolder(parentFolderPath, "Employees");
			foreach (Employee employee in this.Employees)
			{
				try
				{
					new SaveRequest(employee, parentFolderPath3);
					this.savedEmployeePaths.Add(employee.SaveFolderName);
				}
				catch (Exception ex2)
				{
					Console.LogError("Error saving employees: " + ex2.Message, null);
					SaveManager.ReportSaveError();
				}
			}
			return result;
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x000EBD78 File Offset: 0x000E9F78
		public virtual void DeleteUnapprovedFiles(string parentFolderPath)
		{
			string path = ((ISaveable)this).WriteFolder(parentFolderPath, "Objects");
			string path2 = ((ISaveable)this).WriteFolder(parentFolderPath, "Employees");
			string[] directories = Directory.GetDirectories(path);
			for (int i = 0; i < directories.Length; i++)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(directories[i]);
				if (!this.savedObjectPaths.Contains(directoryInfo.Name))
				{
					Directory.Delete(directories[i], true);
				}
			}
			directories = Directory.GetDirectories(path2);
			for (int j = 0; j < directories.Length; j++)
			{
				DirectoryInfo directoryInfo2 = new DirectoryInfo(directories[j]);
				if (!this.savedEmployeePaths.Contains(directoryInfo2.Name))
				{
					Directory.Delete(directories[j], true);
				}
			}
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x000EBE1C File Offset: 0x000EA01C
		public virtual void Load(PropertyData propertyData, string containerPath)
		{
			if (propertyData.IsOwned)
			{
				this.SetOwned();
			}
			int num = 0;
			while (num < propertyData.SwitchStates.Length && num < this.Switches.Count)
			{
				if (propertyData.SwitchStates[num] && this.Switches.Count > num)
				{
					this.Switches[num].SwitchOn();
				}
				num++;
			}
			if (propertyData.ToggleableStates != null)
			{
				int num2 = 0;
				while (num2 < propertyData.ToggleableStates.Length && num2 < this.Toggleables.Count)
				{
					if (propertyData.ToggleableStates[num2] && this.Toggleables.Count > num2)
					{
						this.Toggleables[num2].Toggle();
					}
					num2++;
				}
			}
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x000EBED4 File Offset: 0x000EA0D4
		public bool DoBoundsContainPoint(Vector3 point)
		{
			foreach (BoxCollider boxCollider in this.propertyBoundsColliders)
			{
				if (!(boxCollider == null) && this.IsPointInsideBox(point, boxCollider))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x000EBF10 File Offset: 0x000EA110
		private bool IsPointInsideBox(Vector3 worldPoint, BoxCollider box)
		{
			if (box == null)
			{
				Console.LogWarning("BoxCollider is null.", null);
				return false;
			}
			Vector3 vector = box.transform.InverseTransformPoint(worldPoint);
			vector -= box.center;
			Vector3 vector2 = box.size * 0.5f;
			return Mathf.Abs(vector.x) <= vector2.x && Mathf.Abs(vector.y) <= vector2.y && Mathf.Abs(vector.z) <= vector2.z;
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x000EBF9C File Offset: 0x000EA19C
		public List<Bed> GetUnassignedBeds()
		{
			return (from x in this.Container.GetComponentsInChildren<Bed>()
			where x.AssignedEmployee == null
			select x).ToList<Bed>();
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x000EC0D6 File Offset: 0x000EA2D6
		[CompilerGenerated]
		private IEnumerator <RecieveOwned>g__Wait|93_0()
		{
			yield return new WaitUntil(() => this.PoI.UISetup);
			this.PoI.IconContainer.Find("Unowned").gameObject.SetActive(false);
			this.PoI.IconContainer.Find("Owned").gameObject.SetActive(true);
			yield break;
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x000EC0F4 File Offset: 0x000EA2F4
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.PropertyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.PropertyAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetOwned_Server_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveOwned_Networked_2166136261));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendToggleableState_3658436649));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetToggleableState_338960014));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetToggleableState_338960014));
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x000EC185 File Offset: 0x000EA385
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.PropertyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.PropertyAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x000EC198 File Offset: 0x000EA398
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x000EC1A8 File Offset: 0x000EA3A8
		private void RpcWriter___Server_SetOwned_Server_2166136261()
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
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x000EC242 File Offset: 0x000EA442
		protected void RpcLogic___SetOwned_Server_2166136261()
		{
			this.ReceiveOwned_Networked();
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x000EC24C File Offset: 0x000EA44C
		private void RpcReader___Server_SetOwned_Server_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetOwned_Server_2166136261();
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x000EC27C File Offset: 0x000EA47C
		private void RpcWriter___Observers_ReceiveOwned_Networked_2166136261()
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
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, true, false, false);
			writer.Store();
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x000EC325 File Offset: 0x000EA525
		private void RpcLogic___ReceiveOwned_Networked_2166136261()
		{
			this.RecieveOwned();
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x000EC330 File Offset: 0x000EA530
		private void RpcReader___Observers_ReceiveOwned_Networked_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveOwned_Networked_2166136261();
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x000EC35C File Offset: 0x000EA55C
		private void RpcWriter___Server_SendToggleableState_3658436649(int index, bool state)
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
			writer.WriteInt32(index, AutoPackType.Packed);
			writer.WriteBoolean(state);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x000EC415 File Offset: 0x000EA615
		public void RpcLogic___SendToggleableState_3658436649(int index, bool state)
		{
			this.SetToggleableState(null, index, state);
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x000EC420 File Offset: 0x000EA620
		private void RpcReader___Server_SendToggleableState_3658436649(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int index = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool state = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendToggleableState_3658436649(index, state);
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x000EC468 File Offset: 0x000EA668
		private void RpcWriter___Observers_SetToggleableState_338960014(NetworkConnection conn, int index, bool state)
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
			writer.WriteInt32(index, AutoPackType.Packed);
			writer.WriteBoolean(state);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x000EC530 File Offset: 0x000EA730
		public void RpcLogic___SetToggleableState_338960014(NetworkConnection conn, int index, bool state)
		{
			this.Toggleables[index].SetState(state);
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x000EC544 File Offset: 0x000EA744
		private void RpcReader___Observers_SetToggleableState_338960014(PooledReader PooledReader0, Channel channel)
		{
			int index = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool state = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetToggleableState_338960014(null, index, state);
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x000EC58C File Offset: 0x000EA78C
		private void RpcWriter___Target_SetToggleableState_338960014(NetworkConnection conn, int index, bool state)
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
			writer.WriteInt32(index, AutoPackType.Packed);
			writer.WriteBoolean(state);
			base.SendTargetRpc(4U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x000EC654 File Offset: 0x000EA854
		private void RpcReader___Target_SetToggleableState_338960014(PooledReader PooledReader0, Channel channel)
		{
			int index = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool state = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetToggleableState_338960014(base.LocalConnection, index, state);
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x000EC6A4 File Offset: 0x000EA8A4
		protected virtual void dll()
		{
			if (!(this is Business))
			{
				Property.Properties.Add(this);
				Property.UnownedProperties.Remove(this);
				Property.UnownedProperties.Add(this);
			}
			this.Container.Property = this;
			this.PoI.SetMainText(this.propertyName + " (Unowned)");
			this.SetBoundsVisible(false);
			this.ForSaleSign.transform.Find("Name").GetComponent<TextMeshPro>().text = this.propertyName;
			this.ForSaleSign.transform.Find("Price").GetComponent<TextMeshPro>().text = MoneyManager.FormatAmount(this.Price, false, false);
			this.propertyBoundsColliders = this.BoundingBox.GetComponentsInChildren<BoxCollider>();
			foreach (BoxCollider boxCollider in this.propertyBoundsColliders)
			{
				boxCollider.isTrigger = true;
				boxCollider.gameObject.layer = LayerMask.NameToLayer("Invisible");
			}
			if (this.DisposalArea == null)
			{
				Console.LogWarning("Property " + this.PropertyName + " has no disposal area.", null);
			}
			if (this.EmployeeIdlePoints.Length < this.EmployeeCapacity)
			{
				Debug.LogWarning("Property " + this.PropertyName + " has less idle points than employee capacity.");
			}
			if (!GameManager.IS_TUTORIAL)
			{
				this.WorldspaceUIContainer = new GameObject(this.propertyName + " Worldspace UI Container").AddComponent<RectTransform>();
				this.WorldspaceUIContainer.SetParent(Singleton<ManagementWorldspaceCanvas>.Instance.Canvas.transform);
				this.WorldspaceUIContainer.gameObject.SetActive(false);
			}
			if (this.ListingPoster != null)
			{
				this.ListingPoster.Find("Title").GetComponent<TextMeshPro>().text = this.propertyName;
				this.ListingPoster.Find("Price").GetComponent<TextMeshPro>().text = MoneyManager.FormatAmount(this.Price, false, false);
				this.ListingPoster.Find("Parking/Text").GetComponent<TextMeshPro>().text = this.LoadingDockCount.ToString();
				this.ListingPoster.Find("Employee/Text").GetComponent<TextMeshPro>().text = this.EmployeeCapacity.ToString();
			}
			this.PoI.gameObject.SetActive(false);
			foreach (ModularSwitch modularSwitch in this.Switches)
			{
				if (!(modularSwitch == null))
				{
					ModularSwitch modularSwitch2 = modularSwitch;
					modularSwitch2.onToggled = (ModularSwitch.ButtonChange)Delegate.Combine(modularSwitch2.onToggled, new ModularSwitch.ButtonChange(delegate(bool <p0>)
					{
						this.HasChanged = true;
					}));
				}
			}
			foreach (InteractableToggleable interactableToggleable in this.Toggleables)
			{
				if (!(interactableToggleable == null))
				{
					InteractableToggleable toggleable1 = interactableToggleable;
					interactableToggleable.onToggle.AddListener(delegate()
					{
						this.ToggleableActioned(toggleable1);
					});
				}
			}
			this.InitializeSaveable();
		}

		// Token: 0x0400288E RID: 10382
		public static List<Property> Properties = new List<Property>();

		// Token: 0x0400288F RID: 10383
		public static List<Property> UnownedProperties = new List<Property>();

		// Token: 0x04002890 RID: 10384
		public static List<Property> OwnedProperties = new List<Property>();

		// Token: 0x04002891 RID: 10385
		public static Property.PropertyChange onPropertyAcquired;

		// Token: 0x04002892 RID: 10386
		public UnityEvent onThisPropertyAcquired;

		// Token: 0x04002897 RID: 10391
		[Header("Settings")]
		[SerializeField]
		protected string propertyName = "Property Name";

		// Token: 0x04002898 RID: 10392
		public bool AvailableInDemo = true;

		// Token: 0x04002899 RID: 10393
		[SerializeField]
		protected string propertyCode = "propertycode";

		// Token: 0x0400289A RID: 10394
		public float Price = 1f;

		// Token: 0x0400289B RID: 10395
		public float DefaultRotation;

		// Token: 0x0400289C RID: 10396
		public int EmployeeCapacity = 10;

		// Token: 0x0400289D RID: 10397
		public bool OwnedByDefault;

		// Token: 0x0400289E RID: 10398
		public bool DEBUG_SET_OWNED;

		// Token: 0x0400289F RID: 10399
		public string IsOwnedVariable = string.Empty;

		// Token: 0x040028A0 RID: 10400
		[Header("Culling Settings")]
		public bool ContentCullingEnabled = true;

		// Token: 0x040028A1 RID: 10401
		public float MinimumCullingDistance = 50f;

		// Token: 0x040028A2 RID: 10402
		public GameObject[] ObjectsToCull;

		// Token: 0x040028A3 RID: 10403
		[Header("References")]
		public PropertyContentsContainer Container;

		// Token: 0x040028A4 RID: 10404
		public Transform EmployeeContainer;

		// Token: 0x040028A5 RID: 10405
		public Transform SpawnPoint;

		// Token: 0x040028A6 RID: 10406
		public Transform InteriorSpawnPoint;

		// Token: 0x040028A7 RID: 10407
		public GameObject ForSaleSign;

		// Token: 0x040028A8 RID: 10408
		public GameObject BoundingBox;

		// Token: 0x040028A9 RID: 10409
		public POI PoI;

		// Token: 0x040028AA RID: 10410
		public Transform ListingPoster;

		// Token: 0x040028AB RID: 10411
		public Transform NPCSpawnPoint;

		// Token: 0x040028AC RID: 10412
		public Transform[] EmployeeIdlePoints;

		// Token: 0x040028AD RID: 10413
		public List<ModularSwitch> Switches;

		// Token: 0x040028AE RID: 10414
		public List<InteractableToggleable> Toggleables;

		// Token: 0x040028AF RID: 10415
		public PropertyDisposalArea DisposalArea;

		// Token: 0x040028B0 RID: 10416
		public LoadingDock[] LoadingDocks;

		// Token: 0x040028B1 RID: 10417
		[HideInInspector]
		public List<BuildableItem> BuildableItems = new List<BuildableItem>();

		// Token: 0x040028B2 RID: 10418
		public List<IConfigurable> Configurables = new List<IConfigurable>();

		// Token: 0x040028B3 RID: 10419
		private BoxCollider[] propertyBoundsColliders;

		// Token: 0x040028B4 RID: 10420
		private PropertyLoader loader = new PropertyLoader();

		// Token: 0x040028B8 RID: 10424
		private List<string> savedObjectPaths = new List<string>();

		// Token: 0x040028B9 RID: 10425
		private List<string> savedEmployeePaths = new List<string>();

		// Token: 0x040028BA RID: 10426
		private bool dll_Excuted;

		// Token: 0x040028BB RID: 10427
		private bool dll_Excuted;

		// Token: 0x020007FC RID: 2044
		// (Invoke) Token: 0x060037D3 RID: 14291
		public delegate void PropertyChange(Property property);
	}
}
