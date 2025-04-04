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
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Growing;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.Lighting;
using ScheduleOne.Management;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.Tiles;
using ScheduleOne.Tools;
using ScheduleOne.UI.Management;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B75 RID: 2933
	public class Pot : GridItem, IUsable, IConfigurable, ITransitEntity
	{
		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06004EA2 RID: 20130 RVA: 0x0014B7AC File Offset: 0x001499AC
		// (set) Token: 0x06004EA3 RID: 20131 RVA: 0x0014B7B4 File Offset: 0x001499B4
		public float SoilLevel
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<SoilLevel>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<SoilLevel>k__BackingField(value, true);
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06004EA4 RID: 20132 RVA: 0x0014B7BE File Offset: 0x001499BE
		// (set) Token: 0x06004EA5 RID: 20133 RVA: 0x0014B7C6 File Offset: 0x001499C6
		public string SoilID
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<SoilID>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<SoilID>k__BackingField(value, true);
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06004EA6 RID: 20134 RVA: 0x0014B7D0 File Offset: 0x001499D0
		// (set) Token: 0x06004EA7 RID: 20135 RVA: 0x0014B7D8 File Offset: 0x001499D8
		public int RemainingSoilUses
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<RemainingSoilUses>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<RemainingSoilUses>k__BackingField(value, true);
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06004EA8 RID: 20136 RVA: 0x0014B7E2 File Offset: 0x001499E2
		// (set) Token: 0x06004EA9 RID: 20137 RVA: 0x0014B7EA File Offset: 0x001499EA
		public float WaterLevel
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<WaterLevel>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<WaterLevel>k__BackingField(value, true);
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06004EAA RID: 20138 RVA: 0x0014B7F4 File Offset: 0x001499F4
		public float NormalizedWaterLevel
		{
			get
			{
				return this.WaterLevel / this.WaterCapacity;
			}
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06004EAB RID: 20139 RVA: 0x0014B803 File Offset: 0x00149A03
		public bool IsFilledWithSoil
		{
			get
			{
				return this.SoilLevel >= this.SoilCapacity;
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06004EAC RID: 20140 RVA: 0x0014B816 File Offset: 0x00149A16
		// (set) Token: 0x06004EAD RID: 20141 RVA: 0x0014B81E File Offset: 0x00149A1E
		public Plant Plant { get; protected set; }

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06004EAE RID: 20142 RVA: 0x0014B827 File Offset: 0x00149A27
		// (set) Token: 0x06004EAF RID: 20143 RVA: 0x0014B82F File Offset: 0x00149A2F
		public NetworkObject NPCUserObject
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<NPCUserObject>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<NPCUserObject>k__BackingField(value, true);
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06004EB0 RID: 20144 RVA: 0x0014B839 File Offset: 0x00149A39
		// (set) Token: 0x06004EB1 RID: 20145 RVA: 0x0014B841 File Offset: 0x00149A41
		public NetworkObject PlayerUserObject
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<PlayerUserObject>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<PlayerUserObject>k__BackingField(value, true);
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06004EB2 RID: 20146 RVA: 0x0014B84B File Offset: 0x00149A4B
		public EntityConfiguration Configuration
		{
			get
			{
				return this.potConfiguration;
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06004EB3 RID: 20147 RVA: 0x0014B853 File Offset: 0x00149A53
		// (set) Token: 0x06004EB4 RID: 20148 RVA: 0x0014B85B File Offset: 0x00149A5B
		protected PotConfiguration potConfiguration { get; set; }

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06004EB5 RID: 20149 RVA: 0x0014B864 File Offset: 0x00149A64
		public ConfigurationReplicator ConfigReplicator
		{
			get
			{
				return this.configReplicator;
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06004EB6 RID: 20150 RVA: 0x00014002 File Offset: 0x00012202
		public EConfigurableType ConfigurableType
		{
			get
			{
				return EConfigurableType.Pot;
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06004EB7 RID: 20151 RVA: 0x0014B86C File Offset: 0x00149A6C
		// (set) Token: 0x06004EB8 RID: 20152 RVA: 0x0014B874 File Offset: 0x00149A74
		public WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06004EB9 RID: 20153 RVA: 0x0014B87D File Offset: 0x00149A7D
		// (set) Token: 0x06004EBA RID: 20154 RVA: 0x0014B885 File Offset: 0x00149A85
		public NetworkObject CurrentPlayerConfigurer
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CurrentPlayerConfigurer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(value, true);
			}
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x0014B88F File Offset: 0x00149A8F
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConfigurer(NetworkObject player)
		{
			this.RpcWriter___Server_SetConfigurer_3323014238(player);
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06004EBC RID: 20156 RVA: 0x0014B8A5 File Offset: 0x00149AA5
		public Sprite TypeIcon
		{
			get
			{
				return this.typeIcon;
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06004EBD RID: 20157 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06004EBE RID: 20158 RVA: 0x0014B8AD File Offset: 0x00149AAD
		public Transform UIPoint
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06004EBF RID: 20159 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanBeSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06004EC0 RID: 20160 RVA: 0x0014AAAB File Offset: 0x00148CAB
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06004EC1 RID: 20161 RVA: 0x0014B8B5 File Offset: 0x00149AB5
		// (set) Token: 0x06004EC2 RID: 20162 RVA: 0x0014B8BD File Offset: 0x00149ABD
		public List<ItemSlot> InputSlots { get; set; }

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06004EC3 RID: 20163 RVA: 0x0014B8C6 File Offset: 0x00149AC6
		// (set) Token: 0x06004EC4 RID: 20164 RVA: 0x0014B8CE File Offset: 0x00149ACE
		public List<ItemSlot> OutputSlots { get; set; }

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06004EC5 RID: 20165 RVA: 0x0014B8D7 File Offset: 0x00149AD7
		public Transform LinkOrigin
		{
			get
			{
				return this.UIPoint;
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06004EC6 RID: 20166 RVA: 0x0014B8DF File Offset: 0x00149ADF
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06004EC7 RID: 20167 RVA: 0x0014B8E7 File Offset: 0x00149AE7
		public bool Selectable { get; }

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06004EC8 RID: 20168 RVA: 0x0014B8EF File Offset: 0x00149AEF
		// (set) Token: 0x06004EC9 RID: 20169 RVA: 0x0014B8F7 File Offset: 0x00149AF7
		public bool IsAcceptingItems { get; set; }

		// Token: 0x06004ECA RID: 20170 RVA: 0x0014B900 File Offset: 0x00149B00
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.Pot_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x0014B914 File Offset: 0x00149B14
		protected override void Start()
		{
			base.Start();
			this.WaterLoggedVisuals.gameObject.SetActive(false);
			this.SetSoilState(Pot.ESoilState.Flat);
			this.UpdateSoilScale();
			this.UpdateSoilMaterial();
			this.WaterLevelSlider.value = this.WaterLevel / this.WaterCapacity;
			this.NoWaterIcon.gameObject.SetActive(this.WaterLevel <= 0f);
			this.WaterLevelCanvas.gameObject.SetActive(false);
			this.TaskBounds.gameObject.SetActive(false);
			SoilChunk[] soilChunks = this.SoilChunks;
			for (int i = 0; i < soilChunks.Length; i++)
			{
				soilChunks[i].ClickableEnabled = false;
			}
		}

		// Token: 0x06004ECC RID: 20172 RVA: 0x0014B9C4 File Offset: 0x00149BC4
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			foreach (Additive additive in this.AppliedAdditives)
			{
				this.ApplyAdditive(connection, additive.AssetPath, false);
			}
			if (this.Plant != null)
			{
				this.PlantSeed(connection, this.Plant.SeedDefinition.ID, this.Plant.NormalizedGrowthProgress, this.Plant.YieldLevel, this.Plant.QualityLevel);
				for (int i = 0; i < this.Plant.ActiveHarvestables.Count; i++)
				{
					this.SetHarvestableActive(connection, this.Plant.ActiveHarvestables[i], true);
				}
			}
			this.SendConfigurationToClient(connection);
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x0014BAA8 File Offset: 0x00149CA8
		public void SendConfigurationToClient(NetworkConnection conn)
		{
			Pot.<>c__DisplayClass143_0 CS$<>8__locals1 = new Pot.<>c__DisplayClass143_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.conn = conn;
			if (CS$<>8__locals1.conn.IsHost)
			{
				return;
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendConfigurationToClient>g__WaitForConfig|0());
		}

		// Token: 0x06004ECE RID: 20174 RVA: 0x0014BAE8 File Offset: 0x00149CE8
		public override void InitializeGridItem(ItemInstance instance, Grid grid, Vector2 originCoordinate, int rotation, string GUID)
		{
			bool initialized = base.Initialized;
			base.InitializeGridItem(instance, grid, originCoordinate, rotation, GUID);
			if (initialized)
			{
				return;
			}
			if (!this.isGhost)
			{
				TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.OnMinPass));
				TimeManager instance3 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance3.onTimeSkip = (Action<int>)Delegate.Combine(instance3.onTimeSkip, new Action<int>(this.TimeSkipped));
				base.ParentProperty.AddConfigurable(this);
				this.potConfiguration = new PotConfiguration(this.configReplicator, this, this);
				this.CreateWorldspaceUI();
				this.outputSlot = new ItemSlot();
				this.OutputSlots.Add(this.outputSlot);
			}
		}

		// Token: 0x06004ECF RID: 20175 RVA: 0x0014BBA4 File Offset: 0x00149DA4
		public override void DestroyItem(bool callOnServer = true)
		{
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.OnMinPass));
			TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance2.onTimeSkip = (Action<int>)Delegate.Remove(instance2.onTimeSkip, new Action<int>(this.TimeSkipped));
			if (this.Plant != null)
			{
				this.Plant.Destroy(false);
			}
			if (this.Configuration != null)
			{
				this.Configuration.Destroy();
				this.DestroyWorldspaceUI();
				base.ParentProperty.RemoveConfigurable(this);
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x0014BC44 File Offset: 0x00149E44
		protected virtual void LateUpdate()
		{
			if (!this.intObjSetThisFrame)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				if (this.Plant != null && (!Singleton<ManagementClipboard>.InstanceExists || !Singleton<ManagementClipboard>.Instance.IsEquipped))
				{
					if (this.Plant.IsFullyGrown)
					{
						this.IntObj.SetMessage("Use trimmers to harvest");
					}
					else
					{
						this.IntObj.SetMessage(Mathf.FloorToInt(this.Plant.NormalizedGrowthProgress * 100f).ToString() + "% grown");
					}
					this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Label);
				}
			}
			this.intObjSetThisFrame = false;
			if (this.rotationOverridden)
			{
				this.ModelTransform.localRotation = Quaternion.Lerp(this.ModelTransform.localRotation, Quaternion.Euler(0f, this.rotation, 0f), Time.deltaTime * 10f);
			}
			else if (Mathf.Abs(this.ModelTransform.localEulerAngles.y) > 0.1f)
			{
				this.ModelTransform.localRotation = Quaternion.Lerp(this.ModelTransform.localRotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 10f);
			}
			this.UpdateCanvas();
			this.rotationOverridden = false;
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x0014BD98 File Offset: 0x00149F98
		protected void UpdateCanvas()
		{
			if (Player.Local == null)
			{
				return;
			}
			if (Player.Local.CurrentProperty != base.ParentProperty)
			{
				this.WaterLevelCanvas.gameObject.SetActive(false);
				return;
			}
			if (!this.IsFilledWithSoil)
			{
				this.WaterLevelCanvas.gameObject.SetActive(false);
				return;
			}
			float num = Vector3.Distance(this.WaterLevelCanvas.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			if (num > 2.75f)
			{
				this.WaterLevelCanvas.gameObject.SetActive(false);
				return;
			}
			Vector3 normalized = Vector3.ProjectOnPlane(PlayerSingleton<PlayerCamera>.Instance.transform.position - this.WaterCanvasContainer.position, Vector3.up).normalized;
			this.WaterCanvasContainer.forward = normalized;
			this.WaterLevelCanvas.transform.rotation = Quaternion.LookRotation((PlayerSingleton<PlayerCamera>.Instance.transform.position - this.WaterLevelCanvas.transform.position).normalized, PlayerSingleton<PlayerCamera>.Instance.transform.up);
			float num2 = 0.5f;
			float a = 1f - Mathf.Clamp01(Mathf.InverseLerp(2.75f - num2, 2.75f, num));
			float b = Mathf.Clamp01(Mathf.InverseLerp(0.5f, 0.75f, num));
			this.WaterLevelCanvasGroup.alpha = Mathf.Min(a, b);
			this.WaterLevelCanvas.gameObject.SetActive(true);
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x0014BF24 File Offset: 0x0014A124
		private void OnMinPass()
		{
			float num = this.WaterDrainPerHour * this.WaterCapacity / 60f * this.MoistureDrainMultiplier;
			this.WaterLevel = Mathf.Clamp(this.WaterLevel - num, 0f, this.WaterCapacity);
			this.UpdateSoilMaterial();
			if (this.Plant != null)
			{
				this.Plant.MinPass();
			}
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x0014BF8C File Offset: 0x0014A18C
		private void TimeSkipped(int minsSkippped)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			for (int i = 0; i < minsSkippped; i++)
			{
				this.OnMinPass();
			}
			if (this.Plant != null)
			{
				this.SetGrowProgress(this.Plant.NormalizedGrowthProgress);
			}
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x0014BFD2 File Offset: 0x0014A1D2
		public void ConfigureInteraction(string message, InteractableObject.EInteractableState state, bool useHighLabelPos = false)
		{
			this.intObjSetThisFrame = true;
			this.IntObj.SetMessage(message);
			this.IntObj.SetInteractableState(state);
			this.IntObj.displayLocationPoint = (useHighLabelPos ? this.IntObjLabel_High : this.IntObjLabel_Low);
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x0014C010 File Offset: 0x0014A210
		public void PositionCameraContainer()
		{
			if (!this.AutoRotateCameraContainer)
			{
				return;
			}
			Vector3 vector = this.CameraContainer.parent.TransformPoint(new Vector3(0f, 0.75f, 0f));
			Vector3 a = PlayerSingleton<PlayerCamera>.Instance.transform.position - vector;
			a.y = 0f;
			a = a.normalized;
			this.CameraContainer.localPosition = new Vector3(0f, 0.75f, 0f);
			this.CameraContainer.position += a * 0.7f;
			Vector3 normalized = (vector - PlayerSingleton<PlayerCamera>.Instance.transform.position).normalized;
			normalized.y = 0f;
			this.CameraContainer.rotation = Quaternion.LookRotation(normalized, Vector3.up);
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x0014C0F8 File Offset: 0x0014A2F8
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x0014C119 File Offset: 0x0014A319
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x0014C130 File Offset: 0x0014A330
		[ObserversRpc(RunLocally = true)]
		public virtual void ResetPot()
		{
			this.RpcWriter___Observers_ResetPot_2166136261();
			this.RpcLogic___ResetPot_2166136261();
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x0014C14C File Offset: 0x0014A34C
		public float GetAverageLightExposure(out float growSpeedMultiplier)
		{
			growSpeedMultiplier = 1f;
			float num = 0f;
			if (this.LightSourceOverride != null)
			{
				return this.LightSourceOverride.GrowSpeedMultiplier;
			}
			for (int i = 0; i < this.CoordinatePairs.Count; i++)
			{
				float num2;
				num += base.OwnerGrid.GetTile(this.CoordinatePairs[i].coord2).LightExposureNode.GetTotalExposure(out num2);
				growSpeedMultiplier += num2;
			}
			growSpeedMultiplier /= (float)this.CoordinatePairs.Count;
			return num / (float)this.CoordinatePairs.Count;
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x0014C1E8 File Offset: 0x0014A3E8
		public bool CanAcceptSeed(out string reason)
		{
			if (this.SoilLevel < this.SoilCapacity)
			{
				reason = "No soil";
				return false;
			}
			if (this.NormalizedWaterLevel >= 1f)
			{
				reason = "Waterlogged";
				return false;
			}
			if (this.Plant != null)
			{
				reason = "Already contains seed";
				return false;
			}
			reason = string.Empty;
			return this.SoilLevel >= this.SoilCapacity;
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x0014C254 File Offset: 0x0014A454
		public bool IsReadyForHarvest(out string reason)
		{
			if (this.Plant == null)
			{
				reason = "No plant in this pot";
				return false;
			}
			if (!this.Plant.IsFullyGrown)
			{
				reason = Mathf.Floor(this.Plant.NormalizedGrowthProgress * 100f).ToString() + "% grown";
				return false;
			}
			reason = string.Empty;
			return true;
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x0014C2B9 File Offset: 0x0014A4B9
		public override bool CanBeDestroyed(out string reason)
		{
			if (this.Plant != null)
			{
				reason = "Contains plant";
				return false;
			}
			if (((IUsable)this).IsInUse)
			{
				reason = "In use by other player";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x0014C2EA File Offset: 0x0014A4EA
		public void OverrideRotation(float angle)
		{
			this.rotationOverridden = true;
			this.rotation = angle;
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x0014C2FA File Offset: 0x0014A4FA
		public Transform GetCameraPosition(Pot.ECameraPosition pos)
		{
			switch (pos)
			{
			case Pot.ECameraPosition.Closeup:
				return this.CloseupPosition;
			case Pot.ECameraPosition.Midshot:
				return this.MidshotPosition;
			case Pot.ECameraPosition.Fullshot:
				return this.FullshotPosition;
			case Pot.ECameraPosition.BirdsEye:
				return this.BirdsEyePosition;
			default:
				return null;
			}
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x0014C331 File Offset: 0x0014A531
		public virtual void AddSoil(float amount)
		{
			this.SoilLevel = Mathf.Clamp(this.SoilLevel + amount, 0f, this.SoilCapacity);
			this.UpdateSoilScale();
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x0014C357 File Offset: 0x0014A557
		private void SoilLevelChanged(float _prev, float _new, bool asServer)
		{
			this.UpdateSoilScale();
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x0014C360 File Offset: 0x0014A560
		protected virtual void UpdateSoilScale()
		{
			Vector3 localScale = Vector3.Lerp(this.DirtMinScale, this.DirtMaxScale, this.SoilLevel / this.SoilCapacity);
			this.Dirt_Flat.localScale = localScale;
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x0014C398 File Offset: 0x0014A598
		public virtual void SetSoilID(string id)
		{
			this.SoilID = id;
			this.appliedSoilDefinition = (Registry.GetItem(this.SoilID) as SoilDefinition);
			this.UpdateSoilMaterial();
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x0014C3BD File Offset: 0x0014A5BD
		public virtual void SetSoilUses(int uses)
		{
			this.RemainingSoilUses = uses;
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x0014C3C6 File Offset: 0x0014A5C6
		public void PushSoilDataToServer()
		{
			this.SendSoilData(this.SoilID, this.SoilLevel, this.RemainingSoilUses);
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x0014C3E0 File Offset: 0x0014A5E0
		[ServerRpc(RequireOwnership = false)]
		public void SendSoilData(string soilID, float soilLevel, int soilUses)
		{
			this.RpcWriter___Server_SendSoilData_3104499779(soilID, soilLevel, soilUses);
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x0014C400 File Offset: 0x0014A600
		public void SetSoilState(Pot.ESoilState state)
		{
			if (state == Pot.ESoilState.Flat && this.Plant == null)
			{
				this.Dirt_Parted.gameObject.SetActive(false);
				this.Dirt_Flat.gameObject.SetActive(true);
				return;
			}
			if (state == Pot.ESoilState.Parted || state == Pot.ESoilState.Packed)
			{
				this.Dirt_Parted.gameObject.SetActive(true);
				this.Dirt_Flat.gameObject.SetActive(false);
				if (state == Pot.ESoilState.Packed)
				{
					for (int i = 0; i < this.SoilChunks.Length; i++)
					{
						this.SoilChunks[i].SetLerpedTransform(1f);
					}
					return;
				}
				for (int j = 0; j < this.SoilChunks.Length; j++)
				{
					this.SoilChunks[j].SetLerpedTransform(0f);
				}
			}
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x0014C4BC File Offset: 0x0014A6BC
		protected virtual void UpdateSoilMaterial()
		{
			if (this.SoilID == string.Empty)
			{
				return;
			}
			if (this.appliedSoilDefinition == null)
			{
				this.appliedSoilDefinition = (Registry.GetItem(this.SoilID) as SoilDefinition);
			}
			Material material = this.appliedSoilDefinition.WetSoilMat;
			if (this.NormalizedWaterLevel <= 0f)
			{
				material = this.appliedSoilDefinition.DrySoilMat;
			}
			for (int i = 0; i < this.DirtRenderers.Count; i++)
			{
				if (!(this.DirtRenderers[i] == null))
				{
					this.DirtRenderers[i].material = material;
				}
			}
			this.WaterLoggedVisuals.SetActive(this.NormalizedWaterLevel > 1f);
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x0014C57C File Offset: 0x0014A77C
		public void ChangeWaterAmount(float change)
		{
			this.WaterLevel = Mathf.Clamp(this.WaterLevel + change, 0f, this.WaterCapacity);
			this.UpdateSoilMaterial();
			this.WaterLevelSlider.value = this.WaterLevel / this.WaterCapacity;
			this.NoWaterIcon.gameObject.SetActive(this.WaterLevel <= 0f);
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x0014C5E5 File Offset: 0x0014A7E5
		public void PushWaterDataToServer()
		{
			this.SendWaterData(this.WaterLevel);
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x0014C5F3 File Offset: 0x0014A7F3
		[ServerRpc(RequireOwnership = false)]
		public void SendWaterData(float waterLevel)
		{
			this.RpcWriter___Server_SendWaterData_431000436(waterLevel);
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x0014C5FF File Offset: 0x0014A7FF
		private void WaterLevelChanged(float _prev, float _new, bool asServer)
		{
			this.UpdateSoilMaterial();
			this.WaterLevelSlider.value = this.WaterLevel / this.WaterCapacity;
			this.NoWaterIcon.gameObject.SetActive(this.WaterLevel <= 0f);
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x0014C63F File Offset: 0x0014A83F
		public void SetTargetActive(bool active)
		{
			this.Target.gameObject.SetActive(active);
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x0014C654 File Offset: 0x0014A854
		public void RandomizeTarget()
		{
			int num = 0;
			Vector3 vector;
			do
			{
				Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
				insideUnitSphere.y = 0f;
				vector = base.transform.position + insideUnitSphere * (this.PotRadius * 0.85f);
				vector.y = this.Target.position.y;
				num++;
			}
			while (Vector3.Distance(this.Target.position, vector) < 0.15f && num < 100);
			this.Target.position = vector;
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x0014C6DC File Offset: 0x0014A8DC
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendAdditive(string additiveAssetPath, bool initial)
		{
			this.RpcWriter___Server_SendAdditive_310431262(additiveAssetPath, initial);
			this.RpcLogic___SendAdditive_310431262(additiveAssetPath, initial);
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x0014C6FC File Offset: 0x0014A8FC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void ApplyAdditive(NetworkConnection conn, string additiveAssetPath, bool initial)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ApplyAdditive_619441887(conn, additiveAssetPath, initial);
				this.RpcLogic___ApplyAdditive_619441887(conn, additiveAssetPath, initial);
			}
			else
			{
				this.RpcWriter___Target_ApplyAdditive_619441887(conn, additiveAssetPath, initial);
			}
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x0014C74C File Offset: 0x0014A94C
		public float GetAdditiveGrowthMultiplier()
		{
			float num = 1f;
			foreach (Additive additive in this.AppliedAdditives)
			{
				num *= additive.GrowSpeedMultiplier;
			}
			return num;
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x0014C7A8 File Offset: 0x0014A9A8
		public float GetNetYieldChange()
		{
			float num = 0f;
			foreach (Additive additive in this.AppliedAdditives)
			{
				num += additive.YieldChange;
			}
			return num;
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x0014C804 File Offset: 0x0014AA04
		public float GetNetQualityChange()
		{
			float num = 0f;
			foreach (Additive additive in this.AppliedAdditives)
			{
				num += additive.QualityChange;
			}
			return num;
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x0014C860 File Offset: 0x0014AA60
		public Additive GetAdditive(string additiveName)
		{
			return this.AppliedAdditives.Find((Additive x) => x.AdditiveName.ToLower() == additiveName.ToLower());
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x0014C891 File Offset: 0x0014AA91
		[ObserversRpc(RunLocally = true)]
		public void FullyGrowPlant()
		{
			this.RpcWriter___Observers_FullyGrowPlant_2166136261();
			this.RpcLogic___FullyGrowPlant_2166136261();
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x0014C89F File Offset: 0x0014AA9F
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPlantSeed(string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
		{
			this.RpcWriter___Server_SendPlantSeed_2530605204(seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
			this.RpcLogic___SendPlantSeed_2530605204(seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x0014C8D0 File Offset: 0x0014AAD0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void PlantSeed(NetworkConnection conn, string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_PlantSeed_709433087(conn, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
				this.RpcLogic___PlantSeed_709433087(conn, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
			}
			else
			{
				this.RpcWriter___Target_PlantSeed_709433087(conn, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
			}
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x0014C935 File Offset: 0x0014AB35
		[ObserversRpc]
		private void SetGrowProgress(float progress)
		{
			this.RpcWriter___Observers_SetGrowProgress_431000436(progress);
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x0014C944 File Offset: 0x0014AB44
		private void PlantSeed(string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
		{
			if (this.Plant != null)
			{
				return;
			}
			if (this.SoilLevel < this.SoilCapacity)
			{
				Console.LogWarning("Pot not full of soil!", null);
				return;
			}
			SeedDefinition seedDefinition = Registry.GetItem(seedID) as SeedDefinition;
			if (seedDefinition == null)
			{
				string str = "PlantSeed: seed not found with ID '";
				SeedDefinition seedDefinition2 = seedDefinition;
				Console.LogWarning(str + ((seedDefinition2 != null) ? seedDefinition2.ToString() : null) + "'", null);
				return;
			}
			this.SetSoilState(Pot.ESoilState.Packed);
			this.Plant = UnityEngine.Object.Instantiate<GameObject>(seedDefinition.PlantPrefab.gameObject, this.PlantContainer).GetComponent<Plant>();
			this.Plant.transform.localEulerAngles = new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 0f);
			this.Plant.Initialize(base.NetworkObject, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x0014CA20 File Offset: 0x0014AC20
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetHarvestableActive(NetworkConnection conn, int harvestableIndex, bool active)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetHarvestableActive_338960014(conn, harvestableIndex, active);
				this.RpcLogic___SetHarvestableActive_338960014(conn, harvestableIndex, active);
			}
			else
			{
				this.RpcWriter___Target_SetHarvestableActive_338960014(conn, harvestableIndex, active);
			}
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x0014CA70 File Offset: 0x0014AC70
		public void SetHarvestableActive_Local(int harvestableIndex, bool active)
		{
			if (this.Plant == null)
			{
				Console.LogWarning("SetHarvestableActive called but plant is null!", null);
				return;
			}
			if (this.Plant.IsHarvestableActive(harvestableIndex) == active)
			{
				return;
			}
			int count = this.Plant.ActiveHarvestables.Count;
			this.Plant.SetHarvestableActive(harvestableIndex, active);
			if (count > 0 && this.Plant.ActiveHarvestables.Count == 0)
			{
				if (InstanceFinder.IsServer)
				{
					float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("HarvestedPlantCount");
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("HarvestedPlantCount", (value + 1f).ToString(), true);
					NetworkSingleton<LevelManager>.Instance.AddXP(5);
				}
				this.ResetPot();
			}
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x0014CB20 File Offset: 0x0014AD20
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendHarvestableActive(int harvestableIndex, bool active)
		{
			this.RpcWriter___Server_SendHarvestableActive_3658436649(harvestableIndex, active);
			this.RpcLogic___SendHarvestableActive_3658436649(harvestableIndex, active);
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x0014CB3E File Offset: 0x0014AD3E
		public void SendHarvestableActive_Local(int harvestableIndex, bool active)
		{
			this.SetHarvestableActive_Local(harvestableIndex, active);
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x0014CB48 File Offset: 0x0014AD48
		public WorldspaceUIElement CreateWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				Console.LogWarning(base.gameObject.name + " already has a worldspace UI element!", null);
			}
			if (base.ParentProperty == null)
			{
				Property parentProperty = base.ParentProperty;
				Console.LogError(((parentProperty != null) ? parentProperty.ToString() : null) + " is not a child of a property!", null);
				return null;
			}
			PotUIElement component = UnityEngine.Object.Instantiate<PotUIElement>(this.WorldspaceUIPrefab, base.ParentProperty.WorldspaceUIContainer).GetComponent<PotUIElement>();
			component.Initialize(this);
			this.WorldspaceUI = component;
			return component;
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x0014CBDB File Offset: 0x0014ADDB
		public void DestroyWorldspaceUI()
		{
			if (this.WorldspaceUI != null)
			{
				this.WorldspaceUI.Destroy();
			}
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x0014CBF8 File Offset: 0x0014ADF8
		public override string GetSaveString()
		{
			PlantData plantData = null;
			if (this.Plant != null)
			{
				plantData = this.Plant.GetPlantData();
			}
			return new PotData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, this.SoilID, this.SoilLevel, this.RemainingSoilUses, this.WaterLevel, this.AppliedAdditives.ConvertAll<string>((Additive x) => x.AssetPath).ToArray(), plantData).GetJson(true);
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x0014CC94 File Offset: 0x0014AE94
		public override List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			if (this.Configuration.ShouldSave())
			{
				list.Add("Configuration.json");
				((ISaveable)this).WriteSubfile(parentFolderPath, "Configuration", this.Configuration.GetSaveString());
			}
			list.AddRange(base.WriteData(parentFolderPath));
			return list;
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x0014CCE8 File Offset: 0x0014AEE8
		public virtual void LoadPlant(PlantData data)
		{
			Pot.<>c__DisplayClass196_0 CS$<>8__locals1 = new Pot.<>c__DisplayClass196_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.data = data;
			if (string.IsNullOrEmpty(CS$<>8__locals1.data.SeedID))
			{
				return;
			}
			base.StartCoroutine(CS$<>8__locals1.<LoadPlant>g__Wait|0());
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x0014CD2C File Offset: 0x0014AF2C
		public Pot()
		{
			this.<SoilID>k__BackingField = string.Empty;
			this.AppliedAdditives = new List<Additive>();
			this.InputSlots = new List<ItemSlot>();
			this.OutputSlots = new List<ItemSlot>();
			this.IsAcceptingItems = true;
			base..ctor();
		}

		// Token: 0x06004F03 RID: 20227 RVA: 0x0014CDE4 File Offset: 0x0014AFE4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PotAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PotAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField = new SyncVar<NetworkObject>(this, 6U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<CurrentPlayerConfigurer>k__BackingField);
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 5U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 4U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<NPCUserObject>k__BackingField);
			this.syncVar___<WaterLevel>k__BackingField = new SyncVar<float>(this, 3U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<WaterLevel>k__BackingField);
			this.syncVar___<WaterLevel>k__BackingField.OnChange += this.WaterLevelChanged;
			this.syncVar___<RemainingSoilUses>k__BackingField = new SyncVar<int>(this, 2U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<RemainingSoilUses>k__BackingField);
			this.syncVar___<SoilID>k__BackingField = new SyncVar<string>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<SoilID>k__BackingField);
			this.syncVar___<SoilLevel>k__BackingField = new SyncVar<float>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<SoilLevel>k__BackingField);
			this.syncVar___<SoilLevel>k__BackingField.OnChange += this.SoilLevelChanged;
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SetConfigurer_3323014238));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_ResetPot_2166136261));
			base.RegisterServerRpc(12U, new ServerRpcDelegate(this.RpcReader___Server_SendSoilData_3104499779));
			base.RegisterServerRpc(13U, new ServerRpcDelegate(this.RpcReader___Server_SendWaterData_431000436));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_SendAdditive_310431262));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_ApplyAdditive_619441887));
			base.RegisterTargetRpc(16U, new ClientRpcDelegate(this.RpcReader___Target_ApplyAdditive_619441887));
			base.RegisterObserversRpc(17U, new ClientRpcDelegate(this.RpcReader___Observers_FullyGrowPlant_2166136261));
			base.RegisterServerRpc(18U, new ServerRpcDelegate(this.RpcReader___Server_SendPlantSeed_2530605204));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_PlantSeed_709433087));
			base.RegisterTargetRpc(20U, new ClientRpcDelegate(this.RpcReader___Target_PlantSeed_709433087));
			base.RegisterObserversRpc(21U, new ClientRpcDelegate(this.RpcReader___Observers_SetGrowProgress_431000436));
			base.RegisterObserversRpc(22U, new ClientRpcDelegate(this.RpcReader___Observers_SetHarvestableActive_338960014));
			base.RegisterTargetRpc(23U, new ClientRpcDelegate(this.RpcReader___Target_SetHarvestableActive_338960014));
			base.RegisterServerRpc(24U, new ServerRpcDelegate(this.RpcReader___Server_SendHarvestableActive_3658436649));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.ObjectScripts.Pot));
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x0014D0FC File Offset: 0x0014B2FC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.PotAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.PotAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetRegistered();
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
			this.syncVar___<WaterLevel>k__BackingField.SetRegistered();
			this.syncVar___<RemainingSoilUses>k__BackingField.SetRegistered();
			this.syncVar___<SoilID>k__BackingField.SetRegistered();
			this.syncVar___<SoilLevel>k__BackingField.SetRegistered();
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x0014D16D File Offset: 0x0014B36D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x0014D17C File Offset: 0x0014B37C
		private void RpcWriter___Server_SetConfigurer_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendServerRpc(8U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x0014D223 File Offset: 0x0014B423
		public void RpcLogic___SetConfigurer_3323014238(NetworkObject player)
		{
			this.CurrentPlayerConfigurer = player;
		}

		// Token: 0x06004F08 RID: 20232 RVA: 0x0014D22C File Offset: 0x0014B42C
		private void RpcReader___Server_SetConfigurer_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetConfigurer_3323014238(player);
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x0014D26C File Offset: 0x0014B46C
		private void RpcWriter___Server_SetPlayerUser_3323014238(NetworkObject playerObject)
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
			writer.WriteNetworkObject(playerObject);
			base.SendServerRpc(9U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x0014D314 File Offset: 0x0014B514
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			if (this.PlayerUserObject != null && this.PlayerUserObject.Owner.IsLocalClient && playerObject != null && !playerObject.Owner.IsLocalClient)
			{
				Singleton<GameInput>.Instance.ExitAll();
			}
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x0014D368 File Offset: 0x0014B568
		private void RpcReader___Server_SetPlayerUser_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x0014D3A8 File Offset: 0x0014B5A8
		private void RpcWriter___Server_SetNPCUser_3323014238(NetworkObject npcObject)
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
			writer.WriteNetworkObject(npcObject);
			base.SendServerRpc(10U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x0014D44F File Offset: 0x0014B64F
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06004F0E RID: 20238 RVA: 0x0014D458 File Offset: 0x0014B658
		private void RpcReader___Server_SetNPCUser_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject npcObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x0014D498 File Offset: 0x0014B698
		private void RpcWriter___Observers_ResetPot_2166136261()
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
			base.SendObserversRpc(11U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x0014D544 File Offset: 0x0014B744
		public virtual void RpcLogic___ResetPot_2166136261()
		{
			if (this.Plant != null)
			{
				this.Plant.Destroy(true);
			}
			this.Plant = null;
			if (InstanceFinder.IsServer)
			{
				int remainingSoilUses = this.RemainingSoilUses;
				this.RemainingSoilUses = remainingSoilUses - 1;
			}
			if (this.RemainingSoilUses <= 0)
			{
				this.WaterLevel = 0f;
				this.appliedSoilDefinition = null;
				this.SoilID = string.Empty;
				this.SoilLevel = 0f;
			}
			foreach (Additive additive in this.AppliedAdditives)
			{
				UnityEngine.Object.Destroy(additive.gameObject);
			}
			this.AppliedAdditives.Clear();
			this.SetSoilState(Pot.ESoilState.Flat);
			this.UpdateSoilScale();
			this.UpdateSoilMaterial();
			base.HasChanged = true;
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x0014D628 File Offset: 0x0014B828
		private void RpcReader___Observers_ResetPot_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ResetPot_2166136261();
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x0014D654 File Offset: 0x0014B854
		private void RpcWriter___Server_SendSoilData_3104499779(string soilID, float soilLevel, int soilUses)
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
			writer.WriteString(soilID);
			writer.WriteSingle(soilLevel, AutoPackType.Unpacked);
			writer.WriteInt32(soilUses, AutoPackType.Packed);
			base.SendServerRpc(12U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004F13 RID: 20243 RVA: 0x0014D720 File Offset: 0x0014B920
		public void RpcLogic___SendSoilData_3104499779(string soilID, float soilLevel, int soilUses)
		{
			this.SoilID = soilID;
			if (soilID != string.Empty)
			{
				this.appliedSoilDefinition = (Registry.GetItem(this.SoilID) as SoilDefinition);
			}
			else
			{
				this.appliedSoilDefinition = null;
			}
			this.SoilLevel = soilLevel;
			this.RemainingSoilUses = soilUses;
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x0014D770 File Offset: 0x0014B970
		private void RpcReader___Server_SendSoilData_3104499779(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string soilID = PooledReader0.ReadString();
			float soilLevel = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			int soilUses = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendSoilData_3104499779(soilID, soilLevel, soilUses);
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x0014D7D0 File Offset: 0x0014B9D0
		private void RpcWriter___Server_SendWaterData_431000436(float waterLevel)
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
			writer.WriteSingle(waterLevel, AutoPackType.Unpacked);
			base.SendServerRpc(13U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004F16 RID: 20246 RVA: 0x0014D87C File Offset: 0x0014BA7C
		public void RpcLogic___SendWaterData_431000436(float waterLevel)
		{
			this.WaterLevel = waterLevel;
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x0014D888 File Offset: 0x0014BA88
		private void RpcReader___Server_SendWaterData_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float waterLevel = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendWaterData_431000436(waterLevel);
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x0014D8C0 File Offset: 0x0014BAC0
		private void RpcWriter___Server_SendAdditive_310431262(string additiveAssetPath, bool initial)
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
			writer.WriteString(additiveAssetPath);
			writer.WriteBoolean(initial);
			base.SendServerRpc(14U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x0014D974 File Offset: 0x0014BB74
		public void RpcLogic___SendAdditive_310431262(string additiveAssetPath, bool initial)
		{
			this.ApplyAdditive(null, additiveAssetPath, initial);
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x0014D980 File Offset: 0x0014BB80
		private void RpcReader___Server_SendAdditive_310431262(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string additiveAssetPath = PooledReader0.ReadString();
			bool initial = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAdditive_310431262(additiveAssetPath, initial);
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x0014D9D0 File Offset: 0x0014BBD0
		private void RpcWriter___Observers_ApplyAdditive_619441887(NetworkConnection conn, string additiveAssetPath, bool initial)
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
			writer.WriteString(additiveAssetPath);
			writer.WriteBoolean(initial);
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x0014DA94 File Offset: 0x0014BC94
		public void RpcLogic___ApplyAdditive_619441887(NetworkConnection conn, string additiveAssetPath, bool initial)
		{
			if (this.AppliedAdditives.Find((Additive x) => x.AssetPath == additiveAssetPath))
			{
				Console.Log("Pot already contains additive at " + additiveAssetPath, null);
				return;
			}
			GameObject gameObject = Resources.Load(additiveAssetPath) as GameObject;
			if (gameObject == null)
			{
				Console.LogWarning("Failed to load additive at path: " + additiveAssetPath, null);
				return;
			}
			Additive component = UnityEngine.Object.Instantiate<GameObject>(gameObject, this.AdditivesContainer).GetComponent<Additive>();
			component.transform.localPosition = Vector3.zero;
			component.transform.localRotation = Quaternion.identity;
			if (this.Plant != null)
			{
				this.Plant.QualityLevel += component.QualityChange;
				this.Plant.YieldLevel += component.YieldChange;
				if (initial)
				{
					this.Plant.SetNormalizedGrowthProgress(this.Plant.NormalizedGrowthProgress + component.InstantGrowth);
					if (component.InstantGrowth > 0f)
					{
						this.PoofParticles.Play();
						this.PoofSound.Play();
					}
				}
			}
			this.AppliedAdditives.Add(component);
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x0014DBD4 File Offset: 0x0014BDD4
		private void RpcReader___Observers_ApplyAdditive_619441887(PooledReader PooledReader0, Channel channel)
		{
			string additiveAssetPath = PooledReader0.ReadString();
			bool initial = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ApplyAdditive_619441887(null, additiveAssetPath, initial);
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x0014DC24 File Offset: 0x0014BE24
		private void RpcWriter___Target_ApplyAdditive_619441887(NetworkConnection conn, string additiveAssetPath, bool initial)
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
			writer.WriteString(additiveAssetPath);
			writer.WriteBoolean(initial);
			base.SendTargetRpc(16U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x0014DCE8 File Offset: 0x0014BEE8
		private void RpcReader___Target_ApplyAdditive_619441887(PooledReader PooledReader0, Channel channel)
		{
			string additiveAssetPath = PooledReader0.ReadString();
			bool initial = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ApplyAdditive_619441887(base.LocalConnection, additiveAssetPath, initial);
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x0014DD30 File Offset: 0x0014BF30
		private void RpcWriter___Observers_FullyGrowPlant_2166136261()
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
			base.SendObserversRpc(17U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x0014DDD9 File Offset: 0x0014BFD9
		public void RpcLogic___FullyGrowPlant_2166136261()
		{
			if (this.Plant == null)
			{
				Console.LogWarning("FullyGrowPlant called but plant is null!", null);
				return;
			}
			this.Plant.SetNormalizedGrowthProgress(1f);
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x0014DE08 File Offset: 0x0014C008
		private void RpcReader___Observers_FullyGrowPlant_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___FullyGrowPlant_2166136261();
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x0014DE34 File Offset: 0x0014C034
		private void RpcWriter___Server_SendPlantSeed_2530605204(string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
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
			writer.WriteString(seedID);
			writer.WriteSingle(normalizedSeedProgress, AutoPackType.Unpacked);
			writer.WriteSingle(yieldLevel, AutoPackType.Unpacked);
			writer.WriteSingle(qualityLevel, AutoPackType.Unpacked);
			base.SendServerRpc(18U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x0014DF11 File Offset: 0x0014C111
		public void RpcLogic___SendPlantSeed_2530605204(string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
		{
			this.PlantSeed(null, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x0014DF20 File Offset: 0x0014C120
		private void RpcReader___Server_SendPlantSeed_2530605204(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string seedID = PooledReader0.ReadString();
			float normalizedSeedProgress = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			float yieldLevel = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			float qualityLevel = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPlantSeed_2530605204(seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x0014DFA0 File Offset: 0x0014C1A0
		private void RpcWriter___Observers_PlantSeed_709433087(NetworkConnection conn, string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
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
			writer.WriteString(seedID);
			writer.WriteSingle(normalizedSeedProgress, AutoPackType.Unpacked);
			writer.WriteSingle(yieldLevel, AutoPackType.Unpacked);
			writer.WriteSingle(qualityLevel, AutoPackType.Unpacked);
			base.SendObserversRpc(19U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x0014E08C File Offset: 0x0014C28C
		public void RpcLogic___PlantSeed_709433087(NetworkConnection conn, string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
		{
			this.PlantSeed(seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x0014E09C File Offset: 0x0014C29C
		private void RpcReader___Observers_PlantSeed_709433087(PooledReader PooledReader0, Channel channel)
		{
			string seedID = PooledReader0.ReadString();
			float normalizedSeedProgress = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			float yieldLevel = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			float qualityLevel = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PlantSeed_709433087(null, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x0014E11C File Offset: 0x0014C31C
		private void RpcWriter___Target_PlantSeed_709433087(NetworkConnection conn, string seedID, float normalizedSeedProgress, float yieldLevel, float qualityLevel)
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
			writer.WriteString(seedID);
			writer.WriteSingle(normalizedSeedProgress, AutoPackType.Unpacked);
			writer.WriteSingle(yieldLevel, AutoPackType.Unpacked);
			writer.WriteSingle(qualityLevel, AutoPackType.Unpacked);
			base.SendTargetRpc(20U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x0014E208 File Offset: 0x0014C408
		private void RpcReader___Target_PlantSeed_709433087(PooledReader PooledReader0, Channel channel)
		{
			string seedID = PooledReader0.ReadString();
			float normalizedSeedProgress = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			float yieldLevel = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			float qualityLevel = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___PlantSeed_709433087(base.LocalConnection, seedID, normalizedSeedProgress, yieldLevel, qualityLevel);
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x0014E284 File Offset: 0x0014C484
		private void RpcWriter___Observers_SetGrowProgress_431000436(float progress)
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
			writer.WriteSingle(progress, AutoPackType.Unpacked);
			base.SendObserversRpc(21U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x0014E33F File Offset: 0x0014C53F
		private void RpcLogic___SetGrowProgress_431000436(float progress)
		{
			if (this.Plant == null)
			{
				Console.LogWarning("SetGrowProgress called but plant is null!", null);
				return;
			}
			this.Plant.SetNormalizedGrowthProgress(progress);
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x0014E368 File Offset: 0x0014C568
		private void RpcReader___Observers_SetGrowProgress_431000436(PooledReader PooledReader0, Channel channel)
		{
			float progress = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetGrowProgress_431000436(progress);
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x0014E3A0 File Offset: 0x0014C5A0
		private void RpcWriter___Observers_SetHarvestableActive_338960014(NetworkConnection conn, int harvestableIndex, bool active)
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
			writer.WriteInt32(harvestableIndex, AutoPackType.Packed);
			writer.WriteBoolean(active);
			base.SendObserversRpc(22U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x0014E468 File Offset: 0x0014C668
		public void RpcLogic___SetHarvestableActive_338960014(NetworkConnection conn, int harvestableIndex, bool active)
		{
			this.SetHarvestableActive_Local(harvestableIndex, active);
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x0014E474 File Offset: 0x0014C674
		private void RpcReader___Observers_SetHarvestableActive_338960014(PooledReader PooledReader0, Channel channel)
		{
			int harvestableIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool active = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetHarvestableActive_338960014(null, harvestableIndex, active);
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x0014E4C8 File Offset: 0x0014C6C8
		private void RpcWriter___Target_SetHarvestableActive_338960014(NetworkConnection conn, int harvestableIndex, bool active)
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
			writer.WriteInt32(harvestableIndex, AutoPackType.Packed);
			writer.WriteBoolean(active);
			base.SendTargetRpc(23U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x0014E590 File Offset: 0x0014C790
		private void RpcReader___Target_SetHarvestableActive_338960014(PooledReader PooledReader0, Channel channel)
		{
			int harvestableIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool active = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetHarvestableActive_338960014(base.LocalConnection, harvestableIndex, active);
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x0014E5E0 File Offset: 0x0014C7E0
		private void RpcWriter___Server_SendHarvestableActive_3658436649(int harvestableIndex, bool active)
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
			writer.WriteInt32(harvestableIndex, AutoPackType.Packed);
			writer.WriteBoolean(active);
			base.SendServerRpc(24U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x0014E699 File Offset: 0x0014C899
		public void RpcLogic___SendHarvestableActive_3658436649(int harvestableIndex, bool active)
		{
			this.SetHarvestableActive(null, harvestableIndex, active);
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x0014E6A4 File Offset: 0x0014C8A4
		private void RpcReader___Server_SendHarvestableActive_3658436649(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int harvestableIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool active = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendHarvestableActive_3658436649(harvestableIndex, active);
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06004F36 RID: 20278 RVA: 0x0014E6F8 File Offset: 0x0014C8F8
		// (set) Token: 0x06004F37 RID: 20279 RVA: 0x0014E700 File Offset: 0x0014C900
		public float SyncAccessor_<SoilLevel>k__BackingField
		{
			get
			{
				return this.<SoilLevel>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<SoilLevel>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<SoilLevel>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x0014E73C File Offset: 0x0014C93C
		public virtual bool Pot(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 6U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(this.syncVar___<CurrentPlayerConfigurer>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<CurrentPlayerConfigurer>k__BackingField(value, Boolean2);
				return true;
			}
			else if (UInt321 == 5U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<PlayerUserObject>k__BackingField(this.syncVar___<PlayerUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value2 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<PlayerUserObject>k__BackingField(value2, Boolean2);
				return true;
			}
			else if (UInt321 == 4U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<NPCUserObject>k__BackingField(this.syncVar___<NPCUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value3 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<NPCUserObject>k__BackingField(value3, Boolean2);
				return true;
			}
			else if (UInt321 == 3U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<WaterLevel>k__BackingField(this.syncVar___<WaterLevel>k__BackingField.GetValue(true), true);
					return true;
				}
				float value4 = PooledReader0.ReadSingle(AutoPackType.Unpacked);
				this.sync___set_value_<WaterLevel>k__BackingField(value4, Boolean2);
				return true;
			}
			else if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<RemainingSoilUses>k__BackingField(this.syncVar___<RemainingSoilUses>k__BackingField.GetValue(true), true);
					return true;
				}
				int value5 = PooledReader0.ReadInt32(AutoPackType.Packed);
				this.sync___set_value_<RemainingSoilUses>k__BackingField(value5, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<SoilID>k__BackingField(this.syncVar___<SoilID>k__BackingField.GetValue(true), true);
					return true;
				}
				string value6 = PooledReader0.ReadString();
				this.sync___set_value_<SoilID>k__BackingField(value6, Boolean2);
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
					this.sync___set_value_<SoilLevel>k__BackingField(this.syncVar___<SoilLevel>k__BackingField.GetValue(true), true);
					return true;
				}
				float value7 = PooledReader0.ReadSingle(AutoPackType.Unpacked);
				this.sync___set_value_<SoilLevel>k__BackingField(value7, Boolean2);
				return true;
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06004F39 RID: 20281 RVA: 0x0014E935 File Offset: 0x0014CB35
		// (set) Token: 0x06004F3A RID: 20282 RVA: 0x0014E93D File Offset: 0x0014CB3D
		public string SyncAccessor_<SoilID>k__BackingField
		{
			get
			{
				return this.<SoilID>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<SoilID>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<SoilID>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06004F3B RID: 20283 RVA: 0x0014E979 File Offset: 0x0014CB79
		// (set) Token: 0x06004F3C RID: 20284 RVA: 0x0014E981 File Offset: 0x0014CB81
		public int SyncAccessor_<RemainingSoilUses>k__BackingField
		{
			get
			{
				return this.<RemainingSoilUses>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<RemainingSoilUses>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<RemainingSoilUses>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06004F3D RID: 20285 RVA: 0x0014E9BD File Offset: 0x0014CBBD
		// (set) Token: 0x06004F3E RID: 20286 RVA: 0x0014E9C5 File Offset: 0x0014CBC5
		public float SyncAccessor_<WaterLevel>k__BackingField
		{
			get
			{
				return this.<WaterLevel>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<WaterLevel>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<WaterLevel>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06004F3F RID: 20287 RVA: 0x0014EA01 File Offset: 0x0014CC01
		// (set) Token: 0x06004F40 RID: 20288 RVA: 0x0014EA09 File Offset: 0x0014CC09
		public NetworkObject SyncAccessor_<NPCUserObject>k__BackingField
		{
			get
			{
				return this.<NPCUserObject>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<NPCUserObject>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<NPCUserObject>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06004F41 RID: 20289 RVA: 0x0014EA45 File Offset: 0x0014CC45
		// (set) Token: 0x06004F42 RID: 20290 RVA: 0x0014EA4D File Offset: 0x0014CC4D
		public NetworkObject SyncAccessor_<PlayerUserObject>k__BackingField
		{
			get
			{
				return this.<PlayerUserObject>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<PlayerUserObject>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<PlayerUserObject>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06004F43 RID: 20291 RVA: 0x0014EA89 File Offset: 0x0014CC89
		// (set) Token: 0x06004F44 RID: 20292 RVA: 0x0014EA91 File Offset: 0x0014CC91
		public NetworkObject SyncAccessor_<CurrentPlayerConfigurer>k__BackingField
		{
			get
			{
				return this.<CurrentPlayerConfigurer>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CurrentPlayerConfigurer>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CurrentPlayerConfigurer>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x0014EACD File Offset: 0x0014CCCD
		protected virtual void dll()
		{
			base.Awake();
			this.SoilCover.gameObject.SetActive(false);
			this.SetTargetActive(false);
		}

		// Token: 0x04003B75 RID: 15221
		public const float DryThreshold = 0f;

		// Token: 0x04003B76 RID: 15222
		public const float WaterloggedThreshold = 1f;

		// Token: 0x04003B77 RID: 15223
		public const float ROTATION_SPEED = 10f;

		// Token: 0x04003B78 RID: 15224
		public const float MAX_CAMERA_DISTANCE = 2.75f;

		// Token: 0x04003B79 RID: 15225
		public const float MIN_CAMERA_DISTANCE = 0.5f;

		// Token: 0x04003B7A RID: 15226
		[Header("References")]
		public Transform ModelTransform;

		// Token: 0x04003B7B RID: 15227
		public InteractableObject IntObj;

		// Token: 0x04003B7C RID: 15228
		public Transform PourableStartPoint;

		// Token: 0x04003B7D RID: 15229
		public Transform SeedStartPoint;

		// Token: 0x04003B7E RID: 15230
		public Transform SeedRestingPoint;

		// Token: 0x04003B7F RID: 15231
		public GameObject WaterLoggedVisuals;

		// Token: 0x04003B80 RID: 15232
		public Transform LookAtPoint;

		// Token: 0x04003B81 RID: 15233
		public Transform AdditivesContainer;

		// Token: 0x04003B82 RID: 15234
		public Transform PlantContainer;

		// Token: 0x04003B83 RID: 15235
		public Transform IntObjLabel_Low;

		// Token: 0x04003B84 RID: 15236
		public Transform IntObjLabel_High;

		// Token: 0x04003B85 RID: 15237
		public Transform uiPoint;

		// Token: 0x04003B86 RID: 15238
		[SerializeField]
		protected ConfigurationReplicator configReplicator;

		// Token: 0x04003B87 RID: 15239
		public Transform[] accessPoints;

		// Token: 0x04003B88 RID: 15240
		public Transform TaskBounds;

		// Token: 0x04003B89 RID: 15241
		public PotSoilCover SoilCover;

		// Token: 0x04003B8A RID: 15242
		public Transform LeafDropPoint;

		// Token: 0x04003B8B RID: 15243
		public ParticleSystem PoofParticles;

		// Token: 0x04003B8C RID: 15244
		public AudioSourceController PoofSound;

		// Token: 0x04003B8D RID: 15245
		[Header("UI")]
		public Transform WaterCanvasContainer;

		// Token: 0x04003B8E RID: 15246
		public Canvas WaterLevelCanvas;

		// Token: 0x04003B8F RID: 15247
		public CanvasGroup WaterLevelCanvasGroup;

		// Token: 0x04003B90 RID: 15248
		public Slider WaterLevelSlider;

		// Token: 0x04003B91 RID: 15249
		public GameObject NoWaterIcon;

		// Token: 0x04003B92 RID: 15250
		public PotUIElement WorldspaceUIPrefab;

		// Token: 0x04003B93 RID: 15251
		public Sprite typeIcon;

		// Token: 0x04003B94 RID: 15252
		[Header("Camera References")]
		public Transform CameraContainer;

		// Token: 0x04003B95 RID: 15253
		public Transform MidshotPosition;

		// Token: 0x04003B96 RID: 15254
		public Transform CloseupPosition;

		// Token: 0x04003B97 RID: 15255
		public Transform FullshotPosition;

		// Token: 0x04003B98 RID: 15256
		public Transform BirdsEyePosition;

		// Token: 0x04003B99 RID: 15257
		public bool AutoRotateCameraContainer = true;

		// Token: 0x04003B9A RID: 15258
		[Header("Dirt references")]
		public Transform Dirt_Flat;

		// Token: 0x04003B9B RID: 15259
		public Transform Dirt_Parted;

		// Token: 0x04003B9C RID: 15260
		public SoilChunk[] SoilChunks;

		// Token: 0x04003B9D RID: 15261
		public List<MeshRenderer> DirtRenderers = new List<MeshRenderer>();

		// Token: 0x04003B9E RID: 15262
		[Header("Pot Settings")]
		public float PotRadius = 0.2f;

		// Token: 0x04003B9F RID: 15263
		[Range(0.2f, 2f)]
		public float YieldMultiplier = 1f;

		// Token: 0x04003BA0 RID: 15264
		[Range(0.2f, 2f)]
		public float GrowSpeedMultiplier = 1f;

		// Token: 0x04003BA1 RID: 15265
		[Range(0.2f, 2f)]
		public float MoistureDrainMultiplier = 1f;

		// Token: 0x04003BA2 RID: 15266
		public bool AlignLeafDropToPlayer = true;

		// Token: 0x04003BA3 RID: 15267
		[Header("Capacity Settings")]
		public float SoilCapacity = 20f;

		// Token: 0x04003BA4 RID: 15268
		public float WaterCapacity = 5f;

		// Token: 0x04003BA5 RID: 15269
		public float WaterDrainPerHour = 2f;

		// Token: 0x04003BA6 RID: 15270
		[Header("Dirt Settings")]
		[SerializeField]
		protected Vector3 DirtMinScale;

		// Token: 0x04003BA7 RID: 15271
		[SerializeField]
		protected Vector3 DirtMaxScale = Vector3.one;

		// Token: 0x04003BA8 RID: 15272
		[Header("Pour Target")]
		public Transform Target;

		// Token: 0x04003BA9 RID: 15273
		[Header("Lighting")]
		public UsableLightSource LightSourceOverride;

		// Token: 0x04003BAF RID: 15279
		public List<Additive> AppliedAdditives;

		// Token: 0x04003BB9 RID: 15289
		private bool intObjSetThisFrame;

		// Token: 0x04003BBA RID: 15290
		private ItemSlot outputSlot;

		// Token: 0x04003BBB RID: 15291
		private float rotation;

		// Token: 0x04003BBC RID: 15292
		private bool rotationOverridden;

		// Token: 0x04003BBD RID: 15293
		private SoilDefinition appliedSoilDefinition;

		// Token: 0x04003BBE RID: 15294
		public SyncVar<float> syncVar___<SoilLevel>k__BackingField;

		// Token: 0x04003BBF RID: 15295
		public SyncVar<string> syncVar___<SoilID>k__BackingField;

		// Token: 0x04003BC0 RID: 15296
		public SyncVar<int> syncVar___<RemainingSoilUses>k__BackingField;

		// Token: 0x04003BC1 RID: 15297
		public SyncVar<float> syncVar___<WaterLevel>k__BackingField;

		// Token: 0x04003BC2 RID: 15298
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x04003BC3 RID: 15299
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x04003BC4 RID: 15300
		public SyncVar<NetworkObject> syncVar___<CurrentPlayerConfigurer>k__BackingField;

		// Token: 0x04003BC5 RID: 15301
		private bool dll_Excuted;

		// Token: 0x04003BC6 RID: 15302
		private bool dll_Excuted;

		// Token: 0x02000B76 RID: 2934
		public enum ECameraPosition
		{
			// Token: 0x04003BC8 RID: 15304
			Closeup,
			// Token: 0x04003BC9 RID: 15305
			Midshot,
			// Token: 0x04003BCA RID: 15306
			Fullshot,
			// Token: 0x04003BCB RID: 15307
			BirdsEye
		}

		// Token: 0x02000B77 RID: 2935
		public enum ESoilState
		{
			// Token: 0x04003BCD RID: 15309
			Flat,
			// Token: 0x04003BCE RID: 15310
			Parted,
			// Token: 0x04003BCF RID: 15311
			Packed
		}
	}
}
