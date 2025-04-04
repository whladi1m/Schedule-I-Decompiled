using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using ScheduleOne.Property;
using ScheduleOne.Quests;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Employees
{
	// Token: 0x02000638 RID: 1592
	public class EmployeeManager : NetworkSingleton<EmployeeManager>
	{
		// Token: 0x06002AF5 RID: 10997 RVA: 0x000B0E78 File Offset: 0x000AF078
		public void CreateNewEmployee(Property property, EEmployeeType type)
		{
			bool male = 0.67f > UnityEngine.Random.Range(0f, 1f);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("LifetimeEmployeesRecruited", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("LifetimeEmployeesRecruited") + 1f).ToString(), true);
			string text;
			string text2;
			this.GenerateRandomName(male, out text, out text2);
			string id = text.ToLower() + "_" + text2.ToLower();
			int appearanceIndex;
			AvatarSettings avatarSettings;
			this.GetRandomAppearance(male, out appearanceIndex, out avatarSettings);
			string guid = GUIDManager.GenerateUniqueGUID().ToString();
			this.CreateEmployee(property, type, text, text2, id, male, appearanceIndex, property.NPCSpawnPoint.position, property.NPCSpawnPoint.rotation, guid);
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x000B0F34 File Offset: 0x000AF134
		[ServerRpc(RequireOwnership = false)]
		public void CreateEmployee(Property property, EEmployeeType type, string firstName, string lastName, string id, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, string guid = "")
		{
			this.RpcWriter___Server_CreateEmployee_311954683(property, type, firstName, lastName, id, male, appearanceIndex, position, rotation, guid);
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x000B0F70 File Offset: 0x000AF170
		public Employee CreateEmployee_Server(Property property, EEmployeeType type, string firstName, string lastName, string id, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, string guid)
		{
			if (property.Employees.Count >= property.EmployeeCapacity)
			{
				Console.LogError("Property " + property.PropertyCode + " is at capacity.", null);
				return null;
			}
			Employee employeePrefab = this.GetEmployeePrefab(type);
			if (employeePrefab == null)
			{
				Console.LogError("Failed to find employee prefab for " + type.ToString(), null);
				return null;
			}
			guid = ((guid == "") ? Guid.NewGuid().ToString() : guid);
			if (!this.IsPositionValid(position))
			{
				position = property.NPCSpawnPoint.position;
			}
			if (!this.IsRotationValid(rotation))
			{
				rotation = property.NPCSpawnPoint.rotation;
			}
			Employee component = UnityEngine.Object.Instantiate<Employee>(employeePrefab, position, rotation).GetComponent<Employee>();
			component.Initialize(null, firstName, lastName, id, guid, property.PropertyCode, male, appearanceIndex);
			base.NetworkObject.Spawn(component.gameObject, null, default(Scene));
			component.Movement.Warp(position);
			component.Movement.transform.rotation = rotation;
			Quest quest = this.EmployeeQuests.FirstOrDefault((Quest_Employees x) => x.EmployeeType == type);
			if (quest != null && quest.QuestState == EQuestState.Inactive)
			{
				quest.Begin(true);
			}
			return component;
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x000B10DD File Offset: 0x000AF2DD
		private bool IsPositionValid(Vector3 position)
		{
			return this.IsFloatValid(position.x) && this.IsFloatValid(position.y) && this.IsFloatValid(position.z);
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x000B1109 File Offset: 0x000AF309
		private bool IsRotationValid(Quaternion rotation)
		{
			return this.IsFloatValid(rotation.x) && this.IsFloatValid(rotation.y) && this.IsFloatValid(rotation.z) && this.IsFloatValid(rotation.w);
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x000B1143 File Offset: 0x000AF343
		private bool IsFloatValid(float value)
		{
			return !float.IsNaN(value) && !float.IsInfinity(value);
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000B1158 File Offset: 0x000AF358
		public void RegisterName(string name)
		{
			this.takenNames.Add(name);
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x000B1166 File Offset: 0x000AF366
		public void RegisterAppearance(bool male, int index)
		{
			if (male)
			{
				this.takenMaleAppearances.Add(index);
				return;
			}
			this.takenFemaleAppearances.Add(index);
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x000B1184 File Offset: 0x000AF384
		public void GenerateRandomName(bool male, out string firstName, out string lastName)
		{
			do
			{
				if (male)
				{
					firstName = this.MaleFirstNames[UnityEngine.Random.Range(0, this.MaleFirstNames.Length)].ToString();
				}
				else
				{
					firstName = this.FemaleFirstNames[UnityEngine.Random.Range(0, this.FemaleFirstNames.Length)].ToString();
				}
				lastName = this.LastNames[UnityEngine.Random.Range(0, this.LastNames.Length)].ToString();
			}
			while (this.takenNames.Contains(firstName + " " + lastName));
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x000B1205 File Offset: 0x000AF405
		public EmployeeManager.EmployeeAppearance GetAppearance(bool male, int index)
		{
			if (!male)
			{
				return this.FemaleAppearances[index];
			}
			return this.MaleAppearances[index];
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000B1223 File Offset: 0x000AF423
		public VODatabase GetVoice(bool male, int index)
		{
			if (!male)
			{
				return this.FemaleVoices[index % this.FemaleVoices.Length];
			}
			return this.MaleVoices[index % this.MaleVoices.Length];
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000B124C File Offset: 0x000AF44C
		public void GetRandomAppearance(bool male, out int index, out AvatarSettings settings)
		{
			List<EmployeeManager.EmployeeAppearance> list = male ? this.MaleAppearances : this.FemaleAppearances;
			List<int> list2 = male ? this.takenMaleAppearances : this.takenFemaleAppearances;
			index = UnityEngine.Random.Range(0, list.Count);
			settings = list[index].Settings;
			if (list2.Count >= list.Count)
			{
				return;
			}
			int num = 0;
			while (list2.Contains(index))
			{
				index++;
				if (index >= list.Count)
				{
					index = 0;
				}
				num++;
				if (num >= list.Count)
				{
					settings = list[index].Settings;
					return;
				}
			}
			settings = list[index].Settings;
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x000B12F8 File Offset: 0x000AF4F8
		public Employee GetEmployeePrefab(EEmployeeType type)
		{
			switch (type)
			{
			case EEmployeeType.Botanist:
				return this.BotanistPrefab;
			case EEmployeeType.Handler:
				return this.PackagerPrefab;
			case EEmployeeType.Chemist:
				return this.ChemistPrefab;
			case EEmployeeType.Cleaner:
				return this.CleanerPrefab;
			default:
				Console.LogError("Employee type not found: " + type.ToString(), null);
				return null;
			}
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000B1358 File Offset: 0x000AF558
		public List<Employee> GetEmployeesByType(EEmployeeType type)
		{
			List<Employee> list = new List<Employee>();
			foreach (Employee employee in this.AllEmployees)
			{
				if (employee.EmployeeType == type)
				{
					list.Add(employee);
				}
			}
			return list;
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000B13F0 File Offset: 0x000AF5F0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.EmployeeManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.EmployeeManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_CreateEmployee_311954683));
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x000B1420 File Offset: 0x000AF620
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.EmployeeManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.EmployeeManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x000B1439 File Offset: 0x000AF639
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x000B1448 File Offset: 0x000AF648
		private void RpcWriter___Server_CreateEmployee_311954683(Property property, EEmployeeType type, string firstName, string lastName, string id, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, string guid = "")
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
			writer.Write___ScheduleOne.Property.PropertyFishNet.Serializing.Generated(property);
			writer.Write___ScheduleOne.Employees.EEmployeeTypeFishNet.Serializing.Generated(type);
			writer.WriteString(firstName);
			writer.WriteString(lastName);
			writer.WriteString(id);
			writer.WriteBoolean(male);
			writer.WriteInt32(appearanceIndex, AutoPackType.Packed);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteString(guid);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x000B1570 File Offset: 0x000AF770
		public void RpcLogic___CreateEmployee_311954683(Property property, EEmployeeType type, string firstName, string lastName, string id, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, string guid = "")
		{
			this.CreateEmployee_Server(property, type, firstName, lastName, id, male, appearanceIndex, position, rotation, guid);
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x000B1598 File Offset: 0x000AF798
		private void RpcReader___Server_CreateEmployee_311954683(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Property property = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Property.PropertyFishNet.Serializing.Generateds(PooledReader0);
			EEmployeeType type = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Employees.EEmployeeTypeFishNet.Serializing.Generateds(PooledReader0);
			string firstName = PooledReader0.ReadString();
			string lastName = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			bool male = PooledReader0.ReadBoolean();
			int appearanceIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			string guid = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___CreateEmployee_311954683(property, type, firstName, lastName, id, male, appearanceIndex, position, rotation, guid);
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x000B166C File Offset: 0x000AF86C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001F1D RID: 7965
		public const float MALE_EMPLOYEE_CHANCE = 0.67f;

		// Token: 0x04001F1E RID: 7966
		public List<Employee> AllEmployees = new List<Employee>();

		// Token: 0x04001F1F RID: 7967
		public Quest_Employees[] EmployeeQuests;

		// Token: 0x04001F20 RID: 7968
		[Header("Prefabs")]
		public Botanist BotanistPrefab;

		// Token: 0x04001F21 RID: 7969
		public Packager PackagerPrefab;

		// Token: 0x04001F22 RID: 7970
		public Chemist ChemistPrefab;

		// Token: 0x04001F23 RID: 7971
		public Cleaner CleanerPrefab;

		// Token: 0x04001F24 RID: 7972
		[Header("Appearances")]
		public List<EmployeeManager.EmployeeAppearance> MaleAppearances;

		// Token: 0x04001F25 RID: 7973
		public List<EmployeeManager.EmployeeAppearance> FemaleAppearances;

		// Token: 0x04001F26 RID: 7974
		[Header("Voices")]
		public VODatabase[] MaleVoices;

		// Token: 0x04001F27 RID: 7975
		public VODatabase[] FemaleVoices;

		// Token: 0x04001F28 RID: 7976
		[Header("Names")]
		public string[] MaleFirstNames;

		// Token: 0x04001F29 RID: 7977
		public string[] FemaleFirstNames;

		// Token: 0x04001F2A RID: 7978
		public string[] LastNames;

		// Token: 0x04001F2B RID: 7979
		private List<string> takenNames = new List<string>();

		// Token: 0x04001F2C RID: 7980
		private List<int> takenMaleAppearances = new List<int>();

		// Token: 0x04001F2D RID: 7981
		private List<int> takenFemaleAppearances = new List<int>();

		// Token: 0x04001F2E RID: 7982
		private bool dll_Excuted;

		// Token: 0x04001F2F RID: 7983
		private bool dll_Excuted;

		// Token: 0x02000639 RID: 1593
		[Serializable]
		public class EmployeeAppearance
		{
			// Token: 0x04001F30 RID: 7984
			public AvatarSettings Settings;

			// Token: 0x04001F31 RID: 7985
			public Sprite Mugshot;
		}
	}
}
