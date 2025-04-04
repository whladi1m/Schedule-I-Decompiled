using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using EPOOutline;
using FishNet;
using FishNet.Component.Ownership;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using Pathfinding;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Map;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using ScheduleOne.Vehicles.AI;
using ScheduleOne.Vehicles.Modification;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007AB RID: 1963
	[RequireComponent(typeof(VehicleCamera))]
	[RequireComponent(typeof(NetworkTransform))]
	[RequireComponent(typeof(PredictedOwner))]
	[RequireComponent(typeof(VehicleCollisionDetector))]
	[RequireComponent(typeof(PhysicsDamageable))]
	public class LandVehicle : NetworkBehaviour, IGUIDRegisterable, ISaveable
	{
		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06003530 RID: 13616 RVA: 0x000DF85C File Offset: 0x000DDA5C
		public string VehicleName
		{
			get
			{
				return this.vehicleName;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06003531 RID: 13617 RVA: 0x000DF864 File Offset: 0x000DDA64
		public string VehicleCode
		{
			get
			{
				return this.vehicleCode;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06003532 RID: 13618 RVA: 0x000DF86C File Offset: 0x000DDA6C
		public float VehiclePrice
		{
			get
			{
				return this.vehiclePrice;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06003533 RID: 13619 RVA: 0x000DF874 File Offset: 0x000DDA74
		// (set) Token: 0x06003534 RID: 13620 RVA: 0x000DF87C File Offset: 0x000DDA7C
		public bool IsPlayerOwned { get; protected set; }

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06003535 RID: 13621 RVA: 0x000DF885 File Offset: 0x000DDA85
		// (set) Token: 0x06003536 RID: 13622 RVA: 0x000DF88D File Offset: 0x000DDA8D
		public bool IsVisible { get; protected set; } = true;

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06003537 RID: 13623 RVA: 0x000DF896 File Offset: 0x000DDA96
		// (set) Token: 0x06003538 RID: 13624 RVA: 0x000DF89E File Offset: 0x000DDA9E
		public Guid GUID { get; protected set; }

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06003539 RID: 13625 RVA: 0x000DF8A7 File Offset: 0x000DDAA7
		// (set) Token: 0x0600353A RID: 13626 RVA: 0x000DF8AF File Offset: 0x000DDAAF
		public float DistanceToLocalCamera { get; private set; }

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x0600353B RID: 13627 RVA: 0x000DF8B8 File Offset: 0x000DDAB8
		public Vector3 boundingBoxDimensions
		{
			get
			{
				return new Vector3(this.boundingBox.size.x * this.boundingBox.transform.localScale.x, this.boundingBox.size.y * this.boundingBox.transform.localScale.y, this.boundingBox.size.z * this.boundingBox.transform.localScale.z);
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x0600353C RID: 13628 RVA: 0x000DF93C File Offset: 0x000DDB3C
		public Transform driverEntryPoint
		{
			get
			{
				return this.exitPoints[0];
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600353D RID: 13629 RVA: 0x000DF94A File Offset: 0x000DDB4A
		public Rigidbody Rb
		{
			get
			{
				return this.rb;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600353E RID: 13630 RVA: 0x000DF952 File Offset: 0x000DDB52
		public float ActualMaxSteeringAngle
		{
			get
			{
				if (!this.MaxSteerAngleOverridden)
				{
					return this.maxSteeringAngle;
				}
				return this.OverriddenMaxSteerAngle;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600353F RID: 13631 RVA: 0x000DF969 File Offset: 0x000DDB69
		// (set) Token: 0x06003540 RID: 13632 RVA: 0x000DF971 File Offset: 0x000DDB71
		public bool MaxSteerAngleOverridden { get; private set; }

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06003541 RID: 13633 RVA: 0x000DF97A File Offset: 0x000DDB7A
		// (set) Token: 0x06003542 RID: 13634 RVA: 0x000DF982 File Offset: 0x000DDB82
		public float OverriddenMaxSteerAngle { get; private set; }

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06003543 RID: 13635 RVA: 0x000DF98B File Offset: 0x000DDB8B
		// (set) Token: 0x06003544 RID: 13636 RVA: 0x000DF993 File Offset: 0x000DDB93
		public EVehicleColor OwnedColor { get; private set; } = EVehicleColor.White;

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06003545 RID: 13637 RVA: 0x000DF99C File Offset: 0x000DDB9C
		public int Capacity
		{
			get
			{
				return this.Seats.Length;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06003546 RID: 13638 RVA: 0x000DF9A6 File Offset: 0x000DDBA6
		public int CurrentPlayerOccupancy
		{
			get
			{
				return this.Seats.Count((VehicleSeat s) => s.isOccupied);
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06003547 RID: 13639 RVA: 0x000DF9D2 File Offset: 0x000DDBD2
		// (set) Token: 0x06003548 RID: 13640 RVA: 0x000DF9DA File Offset: 0x000DDBDA
		public bool localPlayerIsDriver { get; protected set; }

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06003549 RID: 13641 RVA: 0x000DF9E3 File Offset: 0x000DDBE3
		// (set) Token: 0x0600354A RID: 13642 RVA: 0x000DF9EB File Offset: 0x000DDBEB
		public bool localPlayerIsInVehicle { get; protected set; }

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x0600354B RID: 13643 RVA: 0x000DF9F4 File Offset: 0x000DDBF4
		// (set) Token: 0x0600354C RID: 13644 RVA: 0x000DF9FC File Offset: 0x000DDBFC
		public bool isOccupied { get; private set; }

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x0600354D RID: 13645 RVA: 0x000DFA05 File Offset: 0x000DDC05
		public Player DriverPlayer
		{
			get
			{
				if (this.Seats[0].Occupant != null)
				{
					return this.Seats[0].Occupant;
				}
				return null;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x0600354E RID: 13646 RVA: 0x000DFA2C File Offset: 0x000DDC2C
		public List<Player> OccupantPlayers
		{
			get
			{
				return (from s in this.Seats
				where s.isOccupied
				select s.Occupant).ToList<Player>();
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x0600354F RID: 13647 RVA: 0x000DFA8C File Offset: 0x000DDC8C
		// (set) Token: 0x06003550 RID: 13648 RVA: 0x000DFA94 File Offset: 0x000DDC94
		public NPC[] OccupantNPCs { get; protected set; } = new NPC[0];

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06003551 RID: 13649 RVA: 0x000DFA9D File Offset: 0x000DDC9D
		// (set) Token: 0x06003552 RID: 13650 RVA: 0x000DFAA5 File Offset: 0x000DDCA5
		public float speed_Kmh { get; protected set; }

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06003553 RID: 13651 RVA: 0x000DFAAE File Offset: 0x000DDCAE
		public float speed_Ms
		{
			get
			{
				return this.speed_Kmh / 3.6f;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06003554 RID: 13652 RVA: 0x000DFABC File Offset: 0x000DDCBC
		public float speed_Mph
		{
			get
			{
				return this.speed_Kmh * 0.621371f;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06003555 RID: 13653 RVA: 0x000DFACA File Offset: 0x000DDCCA
		// (set) Token: 0x06003556 RID: 13654 RVA: 0x000DFAD2 File Offset: 0x000DDCD2
		public float currentThrottle { get; protected set; }

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06003557 RID: 13655 RVA: 0x000DFADB File Offset: 0x000DDCDB
		// (set) Token: 0x06003558 RID: 13656 RVA: 0x000DFAE3 File Offset: 0x000DDCE3
		public bool brakesApplied
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<brakesApplied>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<brakesApplied>k__BackingField(value, true);
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06003559 RID: 13657 RVA: 0x000DFAED File Offset: 0x000DDCED
		// (set) Token: 0x0600355A RID: 13658 RVA: 0x000DFAF5 File Offset: 0x000DDCF5
		public bool isReversing
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<isReversing>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<isReversing>k__BackingField(value, true);
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x0600355B RID: 13659 RVA: 0x000DFAFF File Offset: 0x000DDCFF
		// (set) Token: 0x0600355C RID: 13660 RVA: 0x000DFB07 File Offset: 0x000DDD07
		public bool isStatic { get; protected set; }

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x0600355D RID: 13661 RVA: 0x000DFB10 File Offset: 0x000DDD10
		// (set) Token: 0x0600355E RID: 13662 RVA: 0x000DFB18 File Offset: 0x000DDD18
		public bool handbrakeApplied { get; protected set; }

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x0600355F RID: 13663 RVA: 0x000DFB21 File Offset: 0x000DDD21
		public float boundingBaseOffset
		{
			get
			{
				return base.transform.InverseTransformPoint(this.boundingBox.transform.position).y + this.boundingBox.size.y * 0.5f;
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06003560 RID: 13664 RVA: 0x000DFB5A File Offset: 0x000DDD5A
		public bool isParked
		{
			get
			{
				return this.CurrentParkingLot != null;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06003561 RID: 13665 RVA: 0x000DFB68 File Offset: 0x000DDD68
		// (set) Token: 0x06003562 RID: 13666 RVA: 0x000DFB70 File Offset: 0x000DDD70
		public ParkingLot CurrentParkingLot { get; protected set; }

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06003563 RID: 13667 RVA: 0x000DFB79 File Offset: 0x000DDD79
		// (set) Token: 0x06003564 RID: 13668 RVA: 0x000DFB81 File Offset: 0x000DDD81
		public ParkingSpot CurrentParkingSpot { get; protected set; }

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06003565 RID: 13669 RVA: 0x000DFB8C File Offset: 0x000DDD8C
		public string SaveFolderName
		{
			get
			{
				return this.vehicleCode + "_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06003566 RID: 13670 RVA: 0x000DFBC4 File Offset: 0x000DDDC4
		public string SaveFileName
		{
			get
			{
				return "Vehicle";
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06003567 RID: 13671 RVA: 0x000DFBCB File Offset: 0x000DDDCB
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06003568 RID: 13672 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06003569 RID: 13673 RVA: 0x000DFBD3 File Offset: 0x000DDDD3
		// (set) Token: 0x0600356A RID: 13674 RVA: 0x000DFBDB File Offset: 0x000DDDDB
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x0600356B RID: 13675 RVA: 0x000DFBE4 File Offset: 0x000DDDE4
		// (set) Token: 0x0600356C RID: 13676 RVA: 0x000DFBEC File Offset: 0x000DDDEC
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600356D RID: 13677 RVA: 0x000DFBF5 File Offset: 0x000DDDF5
		// (set) Token: 0x0600356E RID: 13678 RVA: 0x000DFBFD File Offset: 0x000DDDFD
		public bool HasChanged { get; set; }

		// Token: 0x0600356F RID: 13679 RVA: 0x000DFC08 File Offset: 0x000DDE08
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Vehicles.LandVehicle_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000DFC28 File Offset: 0x000DDE28
		public override void OnStartServer()
		{
			base.OnStartServer();
			base.NetworkObject.GiveOwnership(base.LocalConnection);
			this.rb.isKinematic = false;
			this.rb.interpolation = RigidbodyInterpolation.Interpolate;
			if (this.SpawnAsPlayerOwned)
			{
				this.IsPlayerOwned = true;
				this.SetIsPlayerOwned(null, true);
			}
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x000DFC7C File Offset: 0x000DDE7C
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsHost)
			{
				return;
			}
			this.SetOwnedColor(connection, this.OwnedColor);
			for (int i = 0; i < this.Seats.Length; i++)
			{
				if (this.Seats[i].Occupant != null)
				{
					this.SetSeatOccupant(connection, i, this.Seats[i].Occupant.Connection);
				}
			}
			if (this.isParked)
			{
				this.Park_Networked(connection, this.CurrentParkData);
			}
			if (this.IsPlayerOwned)
			{
				this.SetIsPlayerOwned(connection, true);
			}
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x000DFD0D File Offset: 0x000DDF0D
		public override void OnStartClient()
		{
			base.OnStartClient();
			this.rb.isKinematic = false;
			if (!base.IsOwner && !InstanceFinder.IsHost)
			{
				this.rb.isKinematic = true;
			}
			this.rb.interpolation = RigidbodyInterpolation.Interpolate;
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x000DFD48 File Offset: 0x000DDF48
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetIsPlayerOwned(NetworkConnection conn, bool playerOwned)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetIsPlayerOwned_214505783(conn, playerOwned);
				this.RpcLogic___SetIsPlayerOwned_214505783(conn, playerOwned);
			}
			else
			{
				this.RpcWriter___Target_SetIsPlayerOwned_214505783(conn, playerOwned);
			}
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x000DFD80 File Offset: 0x000DDF80
		private void RefreshPoI()
		{
			if (this.POI != null)
			{
				if (this.IsPlayerOwned)
				{
					this.POI.SetMainText(string.Concat(new string[]
					{
						"Owned Vehicle\n(",
						Singleton<VehicleColors>.Instance.GetColorName(this.OwnedColor),
						" ",
						this.VehicleName,
						")"
					}));
					this.POI.enabled = true;
					return;
				}
				this.POI.enabled = false;
			}
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x000DFE06 File Offset: 0x000DE006
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x000DFE18 File Offset: 0x000DE018
		protected virtual void Start()
		{
			this.intObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.intObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			if (this.centerOfMass != null)
			{
				this.rb.centerOfMass = base.transform.InverseTransformPoint(this.centerOfMass.transform.position);
			}
			this.ApplyOwnedColor();
			if (this.GUID == Guid.Empty)
			{
				this.GUID = GUIDManager.GenerateUniqueGUID();
			}
			MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
			instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Combine(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
			if (this.UseHumanoidCollider)
			{
				this.HumanoidColliderContainer.vehicle = this;
				this.HumanoidColliderContainer.transform.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
				Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>(true);
				Collider[] componentsInChildren2 = this.HumanoidColliderContainer.GetComponentsInChildren<Collider>(true);
				foreach (Collider collider in componentsInChildren)
				{
					foreach (Collider collider2 in componentsInChildren2)
					{
						if (this.DEBUG)
						{
							Debug.Log("Ignoring collision between " + collider.name + " and " + collider2.name);
						}
						Physics.IgnoreCollision(collider, collider2, true);
					}
				}
			}
			else
			{
				this.HumanoidColliderContainer.gameObject.SetActive(false);
			}
			TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Remove(instance2.onMinutePass, new Action(this.OnMinPass));
			TimeManager instance3 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance3.onMinutePass = (Action)Delegate.Combine(instance3.onMinutePass, new Action(this.OnMinPass));
			if (!NetworkSingleton<VehicleManager>.Instance.AllVehicles.Contains(this))
			{
				NetworkSingleton<VehicleManager>.Instance.AllVehicles.Add(this);
			}
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x000E0016 File Offset: 0x000DE216
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			if (this.localPlayerIsInVehicle)
			{
				action.used = true;
				this.ExitVehicle();
			}
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x000E0040 File Offset: 0x000DE240
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<MoneyManager>.InstanceExists)
			{
				MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
				instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Remove(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			}
			if (this.HumanoidColliderContainer != null)
			{
				UnityEngine.Object.Destroy(this.HumanoidColliderContainer.gameObject);
			}
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.InstanceExists)
			{
				TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Remove(instance2.onMinutePass, new Action(this.OnMinPass));
			}
			if (NetworkSingleton<VehicleManager>.InstanceExists)
			{
				NetworkSingleton<VehicleManager>.Instance.AllVehicles.Remove(this);
			}
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x000E00DE File Offset: 0x000DE2DE
		private void GetNetworth(MoneyManager.FloatContainer container)
		{
			if (this.IsPlayerOwned)
			{
				container.ChangeValue(this.GetVehicleValue());
			}
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x000E00F4 File Offset: 0x000DE2F4
		protected virtual void Update()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			bool flag = this.localPlayerIsDriver || base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost);
			this.rb.interpolation = (flag ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None);
			this.HasChanged = true;
			if (this.localPlayerIsInVehicle && GameInput.GetButtonDown(GameInput.ButtonCode.Interact) && !GameInput.IsTyping)
			{
				this.ExitVehicle();
			}
			if (this.IsPlayerOwned)
			{
				if (!this.localPlayerIsDriver && (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.SleepInProgress || Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.Camera.transform.position) > 30f))
				{
					this.rb.isKinematic = true;
				}
				else if (base.NetworkObject.Owner == null || base.NetworkObject.OwnerId == -1 || base.NetworkObject.Owner == base.LocalConnection)
				{
					this.rb.isKinematic = false;
				}
			}
			if (this.overrideControls)
			{
				this.currentThrottle = this.throttleOverride;
				this.sync___set_value_currentSteerAngle(this.steerOverride * this.ActualMaxSteeringAngle, true);
			}
			else
			{
				this.UpdateThrottle();
				this.UpdateSteerAngle();
			}
			this.ApplySteerAngle();
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnDrawGizmos()
		{
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x000E0240 File Offset: 0x000DE440
		protected virtual void FixedUpdate()
		{
			float item = base.transform.InverseTransformDirection(base.transform.position - this.lastFramePosition).z / Time.fixedDeltaTime * 3.6f;
			this.previousSpeeds.Add(item);
			if (this.previousSpeeds.Count > this.previousSpeedsSampleSize)
			{
				this.previousSpeeds.RemoveAt(0);
			}
			if (this.isStatic || !this.localPlayerIsDriver)
			{
				float num = 0f;
				foreach (float num2 in this.previousSpeeds)
				{
					num += num2;
				}
				float speed_Kmh = num / (float)this.previousSpeeds.Count;
				this.speed_Kmh = speed_Kmh;
			}
			else
			{
				this.speed_Kmh = base.transform.InverseTransformDirection(this.rb.velocity).z * 3.6f;
			}
			this.lastFramePosition = base.transform.position;
			if (!this.isStatic && !this.Rb.isKinematic)
			{
				this.ApplyThrottle();
				this.rb.AddForce(-base.transform.up * this.speed_Kmh * this.downforce);
			}
			else
			{
				if (this.brakesApplied)
				{
					this.brakesApplied = false;
				}
				this.sync___set_value_currentSteerAngle(0f, true);
			}
			if (!this.isStatic)
			{
				if ((base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost)) && base.transform.position.y < -20f)
				{
					if (this.rb != null)
					{
						this.rb.velocity = Vector3.zero;
						this.rb.angularVelocity = Vector3.zero;
					}
					float num3 = 0f;
					if (MapHeightSampler.Sample(base.transform.position.x, out num3, base.transform.position.z))
					{
						this.SetTransform(new Vector3(base.transform.position.x, num3 + 3f, base.transform.position.z), Quaternion.identity);
					}
					else
					{
						this.SetTransform(MapHeightSampler.ResetPosition, Quaternion.identity);
					}
				}
				if (this.localPlayerIsDriver && Mathf.Abs(this.speed_Kmh) < 5f)
				{
					int num4 = 0;
					for (int i = 0; i < this.wheels.Count; i++)
					{
						if (!this.wheels[i].IsWheelGrounded())
						{
							num4++;
						}
					}
					if (num4 >= 2)
					{
						this.rb.AddRelativeTorque(Vector3.forward * 8f * -Mathf.Clamp(this.SyncAccessor_currentSteerAngle / this.ActualMaxSteeringAngle, -1f, 1f), ForceMode.Acceleration);
					}
				}
			}
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x000E0540 File Offset: 0x000DE740
		protected virtual void OnMinPass()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists || this == null || base.transform == null)
			{
				this.DistanceToLocalCamera = 100000f;
				return;
			}
			this.DistanceToLocalCamera = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x000E059C File Offset: 0x000DE79C
		protected virtual void LateUpdate()
		{
			if (this.HumanoidColliderContainer != null)
			{
				this.HumanoidColliderContainer.transform.position = base.transform.position;
				this.HumanoidColliderContainer.transform.rotation = base.transform.rotation;
			}
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x000E05ED File Offset: 0x000DE7ED
		private void OnCollisionEnter(Collision collision)
		{
			if (this.onCollision != null)
			{
				this.onCollision.Invoke(collision);
			}
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x000E0603 File Offset: 0x000DE803
		[ServerRpc(RequireOwnership = false)]
		protected virtual void SetOwner(NetworkConnection conn)
		{
			this.RpcWriter___Server_SetOwner_328543758(conn);
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x000E0610 File Offset: 0x000DE810
		[ObserversRpc]
		protected virtual void OnOwnerChanged()
		{
			this.RpcWriter___Observers_OnOwnerChanged_2166136261();
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x000E0623 File Offset: 0x000DE823
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetTransform_Server(Vector3 pos, Quaternion rot)
		{
			this.RpcWriter___Server_SetTransform_Server_3848837105(pos, rot);
			this.RpcLogic___SetTransform_Server_3848837105(pos, rot);
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x000E0641 File Offset: 0x000DE841
		[ObserversRpc(RunLocally = true)]
		public void SetTransform(Vector3 pos, Quaternion rot)
		{
			this.RpcWriter___Observers_SetTransform_3848837105(pos, rot);
			this.RpcLogic___SetTransform_3848837105(pos, rot);
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x000E0660 File Offset: 0x000DE860
		public void DestroyVehicle()
		{
			if (!InstanceFinder.IsServer)
			{
				Console.LogWarning("DestroyVehicle called on client!", null);
				return;
			}
			if (this.isOccupied)
			{
				Console.LogError("Can't destroy vehicle while occupied.", base.gameObject);
				return;
			}
			if (this.isParked)
			{
				this.ExitPark_Networked(null, false);
			}
			if (this.HumanoidColliderContainer != null)
			{
				UnityEngine.Object.Destroy(this.HumanoidColliderContainer.gameObject);
			}
			base.Despawn(null);
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x000E06D8 File Offset: 0x000DE8D8
		protected virtual void UpdateThrottle()
		{
			this.currentThrottle = 0f;
			if (this.localPlayerIsDriver)
			{
				this.currentThrottle = GameInput.MotionAxis.y;
				if (this.DriverPlayer.IsTased)
				{
					this.currentThrottle = 0f;
				}
			}
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x000E0720 File Offset: 0x000DE920
		protected virtual void ApplyThrottle()
		{
			bool handbrakeApplied = this.handbrakeApplied;
			this.handbrakeApplied = false;
			if (this.localPlayerIsDriver || this.overrideControls)
			{
				if (this.brakesApplied)
				{
					this.brakesApplied = false;
				}
				if (this.isReversing)
				{
					this.isReversing = false;
				}
				foreach (Wheel wheel in this.wheels)
				{
					wheel.wheelCollider.motorTorque = 0.0001f;
					wheel.wheelCollider.brakeTorque = 0f;
				}
				if (this.localPlayerIsDriver)
				{
					this.handbrakeApplied = GameInput.GetButton(GameInput.ButtonCode.Handbrake);
				}
				if (this.handbrakeApplied && Mathf.Abs(this.speed_Kmh) > 4f)
				{
					this.brakesApplied = true;
					if (!handbrakeApplied && this.onHandbrakeApplied != null)
					{
						this.onHandbrakeApplied.Invoke();
					}
				}
				if (this.currentThrottle != 0f && (Mathf.Abs(this.speed_Kmh) < 4f || Mathf.Sign(this.speed_Kmh) == Mathf.Sign(this.currentThrottle)))
				{
					if (this.speed_Kmh < -0.1f && this.currentThrottle < 0f && !this.isReversing)
					{
						this.isReversing = true;
					}
					float num = this.motorTorque.Evaluate(Mathf.Abs(this.speed_Kmh));
					if (this.isReversing)
					{
						num = this.motorTorque.Evaluate(Mathf.Abs(this.speed_Kmh) / this.reverseMultiplier);
					}
					WheelCollider[] array = this.driveWheels;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].motorTorque = this.currentThrottle * num * this.diffGearing / 2f;
					}
					goto IL_2B7;
				}
				if (this.currentThrottle == 0f)
				{
					goto IL_2B7;
				}
				if (Mathf.Abs(this.currentThrottle) > 0.05f && !this.brakesApplied)
				{
					this.brakesApplied = true;
				}
				using (List<Wheel>.Enumerator enumerator = this.wheels.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Wheel wheel2 = enumerator.Current;
						wheel2.wheelCollider.brakeTorque = Mathf.Abs(this.currentThrottle) * this.brakeForce.Evaluate(Mathf.Abs(this.speed_Kmh));
					}
					goto IL_2B7;
				}
			}
			foreach (Wheel wheel3 in this.wheels)
			{
				wheel3.wheelCollider.motorTorque = 0f;
			}
			if (!this.isOccupied)
			{
				if (!this.handbrakeApplied)
				{
					this.handbrakeApplied = true;
				}
				if (this.isReversing)
				{
					this.isReversing = false;
				}
				if (this.brakesApplied)
				{
					this.brakesApplied = false;
				}
			}
			IL_2B7:
			if (this.handbrakeApplied)
			{
				foreach (WheelCollider wheelCollider in this.handbrakeWheels)
				{
					wheelCollider.motorTorque = 0f;
					wheelCollider.brakeTorque = this.handBrakeForce;
				}
			}
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x000E0A48 File Offset: 0x000DEC48
		public void ApplyHandbrake()
		{
			this.handbrakeApplied = true;
			foreach (WheelCollider wheelCollider in this.handbrakeWheels)
			{
				wheelCollider.motorTorque = 0f;
				wheelCollider.brakeTorque = this.handBrakeForce;
			}
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x000E0A8A File Offset: 0x000DEC8A
		[ServerRpc(RequireOwnership = false)]
		private void SetSteeringAngle(float sa)
		{
			this.RpcWriter___Server_SetSteeringAngle_431000436(sa);
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x000E0A98 File Offset: 0x000DEC98
		protected virtual void UpdateSteerAngle()
		{
			if (this.localPlayerIsDriver)
			{
				this.sync___set_value_currentSteerAngle(this.lastFrameSteerAngle, true);
				if (this.DriverPlayer.IsTased || Player.Local.Seizure)
				{
					this.sync___set_value_currentSteerAngle(Mathf.MoveTowards(this.SyncAccessor_currentSteerAngle, UnityEngine.Random.Range(-this.ActualMaxSteeringAngle, this.ActualMaxSteeringAngle), this.steerRate * Time.deltaTime), true);
				}
				else
				{
					float num = 1f;
					if (Player.Local.Disoriented)
					{
						num = -1f;
					}
					if (GameInput.GetButton(GameInput.ButtonCode.Left))
					{
						this.sync___set_value_currentSteerAngle(Mathf.Clamp(this.SyncAccessor_currentSteerAngle - this.steerRate * Time.deltaTime * num, -this.ActualMaxSteeringAngle, this.ActualMaxSteeringAngle), true);
					}
					if (GameInput.GetButton(GameInput.ButtonCode.Right))
					{
						this.sync___set_value_currentSteerAngle(Mathf.Clamp(this.SyncAccessor_currentSteerAngle + this.steerRate * Time.deltaTime * num, -this.ActualMaxSteeringAngle, this.ActualMaxSteeringAngle), true);
					}
					if (!GameInput.GetButton(GameInput.ButtonCode.Left) && !GameInput.GetButton(GameInput.ButtonCode.Right))
					{
						this.sync___set_value_currentSteerAngle(Mathf.MoveTowards(this.SyncAccessor_currentSteerAngle, 0f, this.steerRate * Time.deltaTime), true);
					}
				}
				if (Mathf.Abs(this.lastReplicatedSteerAngle - this.SyncAccessor_currentSteerAngle) > 3f)
				{
					this.lastReplicatedSteerAngle = this.SyncAccessor_currentSteerAngle;
					this.SetSteeringAngle(this.SyncAccessor_currentSteerAngle);
				}
				this.lastFrameSteerAngle = this.SyncAccessor_currentSteerAngle;
			}
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x000E0C00 File Offset: 0x000DEE00
		protected virtual void ApplySteerAngle()
		{
			float num = this.SyncAccessor_currentSteerAngle;
			if (this.flipSteer)
			{
				num *= -1f;
			}
			WheelCollider[] array = this.steerWheels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].steerAngle = num;
			}
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x000E0C44 File Offset: 0x000DEE44
		private void DelaySetStatic(bool stat)
		{
			LandVehicle.<>c__DisplayClass224_0 CS$<>8__locals1 = new LandVehicle.<>c__DisplayClass224_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.stat = stat;
			base.StartCoroutine(CS$<>8__locals1.<DelaySetStatic>g__Wait|0());
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x000E0C74 File Offset: 0x000DEE74
		public virtual void SetIsStatic(bool stat)
		{
			this.isStatic = stat;
			if (this.isStatic)
			{
				this.rb.isKinematic = true;
			}
			else
			{
				this.rb.isKinematic = false;
			}
			foreach (Wheel wheel in this.wheels)
			{
				wheel.SetIsStatic(this.isStatic);
			}
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x000E0CF4 File Offset: 0x000DEEF4
		public void AlignTo(Transform target, EParkingAlignment type, bool network = false)
		{
			Tuple<Vector3, Quaternion> alignmentTransform = this.GetAlignmentTransform(target, type);
			base.transform.rotation = alignmentTransform.Item2;
			base.transform.position = alignmentTransform.Item1;
			this.rb.position = alignmentTransform.Item1;
			this.rb.rotation = alignmentTransform.Item2;
			if (network)
			{
				this.SetTransform_Server(alignmentTransform.Item1, alignmentTransform.Item2);
			}
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000E0D64 File Offset: 0x000DEF64
		public Tuple<Vector3, Quaternion> GetAlignmentTransform(Transform target, EParkingAlignment type)
		{
			Quaternion quaternion = target.rotation;
			if (type == EParkingAlignment.FrontToKerb)
			{
				quaternion *= Quaternion.Euler(0f, 180f, 0f);
			}
			Vector3 vector = target.position + target.up * (this.boundingBoxDimensions.y / 2f - this.boundingBox.transform.localPosition.y);
			if (type == EParkingAlignment.FrontToKerb)
			{
				vector += target.forward * (this.boundingBoxDimensions.z / 2f - this.boundingBox.transform.localPosition.y);
			}
			else
			{
				vector += target.forward * (this.boundingBoxDimensions.z / 2f - this.boundingBox.transform.localPosition.y);
			}
			return new Tuple<Vector3, Quaternion>(vector, quaternion);
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x000E0E52 File Offset: 0x000DF052
		public float GetVehicleValue()
		{
			return this.VehiclePrice;
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x000E0E5A File Offset: 0x000DF05A
		public void OverrideMaxSteerAngle(float maxAngle)
		{
			this.OverriddenMaxSteerAngle = maxAngle;
			this.MaxSteerAngleOverridden = true;
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x000E0E6A File Offset: 0x000DF06A
		public void ResetMaxSteerAngle()
		{
			this.MaxSteerAngleOverridden = false;
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x000E0E73 File Offset: 0x000DF073
		public void SetObstaclesActive(bool active)
		{
			this.NavmeshCut.enabled = active;
			this.NavMeshObstacle.carving = active;
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x000E0E90 File Offset: 0x000DF090
		public VehicleSeat GetFirstFreeSeat()
		{
			for (int i = 0; i < this.Seats.Length; i++)
			{
				if (!this.Seats[i].isOccupied)
				{
					return this.Seats[i];
				}
			}
			return null;
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000E0ECC File Offset: 0x000DF0CC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetSeatOccupant(NetworkConnection conn, int seatIndex, NetworkConnection occupant)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSeatOccupant_3428404692(conn, seatIndex, occupant);
				this.RpcLogic___SetSeatOccupant_3428404692(conn, seatIndex, occupant);
			}
			else
			{
				this.RpcWriter___Target_SetSeatOccupant_3428404692(conn, seatIndex, occupant);
			}
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x000E0F19 File Offset: 0x000DF119
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SetSeatOccupant_Server(int seatIndex, NetworkConnection conn)
		{
			this.RpcWriter___Server_SetSeatOccupant_Server_3266232555(seatIndex, conn);
			this.RpcLogic___SetSeatOccupant_Server_3266232555(seatIndex, conn);
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x000E0F38 File Offset: 0x000DF138
		private void Hovered()
		{
			if (!this.IsPlayerOwned)
			{
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			if (this.CurrentPlayerOccupancy < this.Capacity)
			{
				this.intObj.SetMessage("Enter vehicle");
				this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.intObj.SetMessage("Vehicle full");
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x000E0FA1 File Offset: 0x000DF1A1
		private void Interacted()
		{
			if (this.justExitedVehicle)
			{
				return;
			}
			if (!this.IsPlayerOwned)
			{
				return;
			}
			if (this.CurrentPlayerOccupancy < this.Capacity)
			{
				this.EnterVehicle();
			}
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x000E0FCC File Offset: 0x000DF1CC
		private void EnterVehicle()
		{
			if (this.justExitedVehicle)
			{
				return;
			}
			this.localPlayerIsInVehicle = true;
			this.localPlayerSeat = this.GetFirstFreeSeat();
			this.localPlayerIsDriver = this.localPlayerSeat.isDriverSeat;
			this.SetSeatOccupant_Server(Array.IndexOf<VehicleSeat>(this.Seats, this.localPlayerSeat), Player.Local.Connection);
			this.closestExitPoint = this.GetClosestExitPoint(this.localPlayerSeat.transform.position);
			Player.Local.EnterVehicle(this);
			PlayerSingleton<PlayerCamera>.Instance.SetCameraMode(PlayerCamera.ECameraMode.Vehicle);
			if (PlayerSingleton<PlayerInventory>.InstanceExists)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			}
			if (this.localPlayerIsDriver)
			{
				base.NetworkObject.SetLocalOwnership(Player.Local.Connection);
				this.SetOwner(Player.Local.Connection);
			}
			this.SetObstaclesActive(!this.isOccupied);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x000E10B4 File Offset: 0x000DF2B4
		public void ExitVehicle()
		{
			if (this.localPlayerIsDriver)
			{
				this.SetOwner(null);
			}
			this.localPlayerIsInVehicle = false;
			this.localPlayerIsDriver = false;
			if (this.localPlayerSeat != null)
			{
				this.SetSeatOccupant_Server(Array.IndexOf<VehicleSeat>(this.Seats, this.localPlayerSeat), null);
				this.localPlayerSeat = null;
			}
			List<Transform> list = new List<Transform>();
			list.Add(this.closestExitPoint);
			list.AddRange(this.exitPoints);
			Transform validExitPoint = this.GetValidExitPoint(list);
			Player.Local.ExitVehicle(validExitPoint);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.ResetRotation();
			PlayerSingleton<PlayerCamera>.Instance.SetCameraMode(PlayerCamera.ECameraMode.Default);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			this.SetObstaclesActive(!this.isOccupied);
			this.justExitedVehicle = true;
			base.Invoke("EndJustExited", 0.05f);
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x000E119E File Offset: 0x000DF39E
		private void EndJustExited()
		{
			this.justExitedVehicle = false;
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000E11A7 File Offset: 0x000DF3A7
		public Transform GetExitPoint(int seatIndex = 0)
		{
			return this.exitPoints[seatIndex];
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x000E11B8 File Offset: 0x000DF3B8
		private Transform GetClosestExitPoint(Vector3 pos)
		{
			Transform transform = null;
			for (int i = 0; i < this.exitPoints.Count; i++)
			{
				if (transform == null || Vector3.Distance(this.exitPoints[i].position, pos) < Vector3.Distance(transform.transform.position, pos))
				{
					transform = this.exitPoints[i];
				}
			}
			return transform;
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x000E1220 File Offset: 0x000DF420
		private Transform GetValidExitPoint(List<Transform> possibleExitPoints)
		{
			LayerMask mask = default(LayerMask) | 1 << LayerMask.NameToLayer("Default");
			mask |= 1 << LayerMask.NameToLayer("Vehicle");
			mask |= 1 << LayerMask.NameToLayer("Terrain");
			for (int i = 0; i < possibleExitPoints.Count; i++)
			{
				if (Physics.OverlapSphere(possibleExitPoints[i].position, 0.35f, mask).Length == 0)
				{
					return possibleExitPoints[i];
				}
			}
			Console.LogWarning("Unable to find clear exit point for vehicle. Using first exit point.", null);
			return possibleExitPoints[0];
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x000E12D4 File Offset: 0x000DF4D4
		public void AddNPCOccupant(NPC npc)
		{
			int num = (from x in this.OccupantNPCs
			where x != null
			select x).Count<NPC>();
			if (!this.OccupantNPCs.Contains(npc))
			{
				for (int i = 0; i < this.OccupantNPCs.Length; i++)
				{
					if (this.OccupantNPCs[i] == null)
					{
						this.OccupantNPCs[i] = npc;
						break;
					}
				}
			}
			this.isOccupied = true;
			this.SetObstaclesActive(!this.isOccupied);
			if (num == 0 && this.onVehicleStart != null)
			{
				this.onVehicleStart.Invoke();
			}
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x000E137C File Offset: 0x000DF57C
		public void RemoveNPCOccupant(NPC npc)
		{
			for (int i = 0; i < this.OccupantNPCs.Length; i++)
			{
				if (this.OccupantNPCs[i] == npc)
				{
					this.OccupantNPCs[i] = null;
				}
			}
			if ((from x in this.OccupantNPCs
			where x != null
			select x).Count<NPC>() == 0)
			{
				this.isOccupied = false;
				if (this.onVehicleStop != null)
				{
					this.onVehicleStop.Invoke();
				}
			}
			this.SetObstaclesActive(!this.isOccupied);
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x000E140F File Offset: 0x000DF60F
		public virtual bool CanBeRecovered()
		{
			return this.IsPlayerOwned && !this.isOccupied && !this.isStatic;
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x000E142C File Offset: 0x000DF62C
		public virtual void RecoverVehicle()
		{
			VehicleRecoveryPoint closestRecoveryPoint = VehicleRecoveryPoint.GetClosestRecoveryPoint(base.transform.position);
			base.transform.position = closestRecoveryPoint.transform.position + Vector3.up * 2f;
			base.transform.up = Vector3.up;
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x000E1484 File Offset: 0x000DF684
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendOwnedColor(EVehicleColor col)
		{
			this.RpcWriter___Server_SendOwnedColor_911055161(col);
			this.RpcLogic___SendOwnedColor_911055161(col);
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x000E149A File Offset: 0x000DF69A
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		protected virtual void SetOwnedColor(NetworkConnection conn, EVehicleColor col)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetOwnedColor_1679996372(conn, col);
				this.RpcLogic___SetOwnedColor_1679996372(conn, col);
			}
			else
			{
				this.RpcWriter___Target_SetOwnedColor_1679996372(conn, col);
			}
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x000E14D0 File Offset: 0x000DF6D0
		public virtual void ApplyColor(EVehicleColor col)
		{
			if (col == EVehicleColor.Custom)
			{
				this.DisplayedColor = col;
				return;
			}
			this.DisplayedColor = col;
			Material material = Singleton<VehicleColors>.Instance.colorLibrary.Find((VehicleColors.VehicleColorData x) => x.color == this.DisplayedColor).material;
			for (int i = 0; i < this.BodyMeshes.Length; i++)
			{
				this.BodyMeshes[i].Renderer.materials[this.BodyMeshes[i].MaterialIndex].color = material.color;
			}
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x000E1550 File Offset: 0x000DF750
		public void ApplyOwnedColor()
		{
			this.ApplyColor(this.OwnedColor);
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x000E1560 File Offset: 0x000DF760
		public void ShowOutline(BuildableItem.EOutlineColor color)
		{
			if (this.outlineEffect == null)
			{
				this.outlineEffect = base.gameObject.AddComponent<Outlinable>();
				this.outlineEffect.OutlineParameters.BlurShift = 0f;
				this.outlineEffect.OutlineParameters.DilateShift = 0.5f;
				this.outlineEffect.OutlineParameters.FillPass.Shader = Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
				foreach (GameObject gameObject in this.outlineRenderers)
				{
					MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						OutlineTarget target = new OutlineTarget(componentsInChildren[i], 0);
						this.outlineEffect.TryAddTarget(target);
					}
				}
			}
			this.outlineEffect.OutlineParameters.Color = BuildableItem.GetColorFromOutlineColorEnum(color);
			Color32 colorFromOutlineColorEnum = BuildableItem.GetColorFromOutlineColorEnum(color);
			colorFromOutlineColorEnum.a = 9;
			this.outlineEffect.OutlineParameters.FillPass.SetColor("_PublicColor", colorFromOutlineColorEnum);
			this.outlineEffect.enabled = true;
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x000E169C File Offset: 0x000DF89C
		public void HideOutline()
		{
			if (this.outlineEffect != null)
			{
				this.outlineEffect.enabled = false;
			}
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x000E16B8 File Offset: 0x000DF8B8
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void Park_Networked(NetworkConnection conn, ParkData parkData)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Park_Networked_2633993806(conn, parkData);
				this.RpcLogic___Park_Networked_2633993806(conn, parkData);
			}
			else
			{
				this.RpcWriter___Target_Park_Networked_2633993806(conn, parkData);
			}
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x000E16F0 File Offset: 0x000DF8F0
		public void Park(NetworkConnection conn, ParkData parkData, bool network)
		{
			if (this.isParked)
			{
				this.ExitPark(true);
			}
			if (network)
			{
				this.Park_Networked(conn, parkData);
				return;
			}
			this.CurrentParkingLot = GUIDManager.GetObject<ParkingLot>(parkData.lotGUID);
			if (this.CurrentParkingLot == null)
			{
				Console.LogWarning("LandVehicle.Park: parking lot not found with the given GUID.", null);
				return;
			}
			this.CurrentParkData = parkData;
			if (parkData.spotIndex < 0 || parkData.spotIndex >= this.CurrentParkingLot.ParkingSpots.Count)
			{
				this.SetVisible(false);
			}
			else
			{
				this.CurrentParkingSpot = this.CurrentParkingLot.ParkingSpots[parkData.spotIndex];
				this.CurrentParkingSpot.SetOccupant(this);
				this.AlignTo(this.CurrentParkingSpot.AlignmentPoint, parkData.alignment, false);
			}
			this.SetIsStatic(true);
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x000E17BB File Offset: 0x000DF9BB
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void ExitPark_Networked(NetworkConnection conn, bool moveToExitPoint = true)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ExitPark_Networked_214505783(conn, moveToExitPoint);
				this.RpcLogic___ExitPark_Networked_214505783(conn, moveToExitPoint);
			}
			else
			{
				this.RpcWriter___Target_ExitPark_Networked_214505783(conn, moveToExitPoint);
			}
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x000E17F4 File Offset: 0x000DF9F4
		public void ExitPark(bool moveToExitPoint = true)
		{
			if (this.CurrentParkingLot == null)
			{
				return;
			}
			if (this.CurrentParkingLot.ExitPoint != null && moveToExitPoint)
			{
				this.AlignTo(this.CurrentParkingLot.ExitPoint, this.CurrentParkingLot.ExitAlignment, false);
			}
			this.CurrentParkData = null;
			this.CurrentParkingLot = null;
			if (this.CurrentParkingSpot != null)
			{
				this.CurrentParkingSpot.SetOccupant(null);
				this.CurrentParkingSpot = null;
			}
			this.SetIsStatic(false);
			this.SetVisible(true);
			base.gameObject.SetActive(true);
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x000E188B File Offset: 0x000DFA8B
		public void SetVisible(bool vis)
		{
			this.IsVisible = vis;
			this.vehicleModel.gameObject.SetActive(vis);
			this.HumanoidColliderContainer.gameObject.SetActive(vis);
			this.boundingBox.gameObject.SetActive(vis);
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x000E18C8 File Offset: 0x000DFAC8
		public List<ItemInstance> GetContents()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			if (this.Storage != null)
			{
				list.AddRange(this.Storage.GetAllItems());
			}
			return list;
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x000E18FB File Offset: 0x000DFAFB
		public virtual string GetSaveString()
		{
			return new VehicleData(this.GUID, this.vehicleCode, base.transform.position, base.transform.rotation, this.OwnedColor).GetJson(true);
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x000E1930 File Offset: 0x000DFB30
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			if (this.Storage != null && this.Storage.ItemCount > 0)
			{
				string json = new ItemSet(this.Storage.ItemSlots).GetJSON();
				list.Add("Contents.json");
				((ISaveable)this).WriteSubfile(parentFolderPath, "Contents", json);
			}
			return list;
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x000E1990 File Offset: 0x000DFB90
		public virtual void Load(VehicleData data, string containerPath)
		{
			this.SetGUID(new Guid(data.GUID));
			this.SetTransform(data.Position, data.Rotation);
			this.SetOwnedColor(null, Enum.Parse<EVehicleColor>(data.Color));
			string json;
			if (this.Storage != null && File.Exists(System.IO.Path.Combine(containerPath, "Contents.json")) && this.Loader.TryLoadFile(containerPath, "Contents", out json))
			{
				ItemInstance[] array = ItemSet.Deserialize(json);
				for (int i = 0; i < array.Length; i++)
				{
					this.Storage.ItemSlots[i].SetStoredItem(array[i], false);
				}
			}
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x000E1C0C File Offset: 0x000DFE0C
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.LandVehicleAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.LandVehicleAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<isReversing>k__BackingField = new SyncVar<bool>(this, 2U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, 0.1f, Channel.Unreliable, this.<isReversing>k__BackingField);
			this.syncVar___<brakesApplied>k__BackingField = new SyncVar<bool>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, 0.1f, Channel.Unreliable, this.<brakesApplied>k__BackingField);
			this.syncVar___currentSteerAngle = new SyncVar<float>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, 0.05f, Channel.Unreliable, this.currentSteerAngle);
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsPlayerOwned_214505783));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_SetIsPlayerOwned_214505783));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SetOwner_328543758));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_OnOwnerChanged_2166136261));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_SetTransform_Server_3848837105));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_SetTransform_3848837105));
			base.RegisterServerRpc(6U, new ServerRpcDelegate(this.RpcReader___Server_SetSteeringAngle_431000436));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_SetSeatOccupant_3428404692));
			base.RegisterTargetRpc(8U, new ClientRpcDelegate(this.RpcReader___Target_SetSeatOccupant_3428404692));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SetSeatOccupant_Server_3266232555));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SendOwnedColor_911055161));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_SetOwnedColor_1679996372));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_SetOwnedColor_1679996372));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_Park_Networked_2633993806));
			base.RegisterTargetRpc(14U, new ClientRpcDelegate(this.RpcReader___Target_Park_Networked_2633993806));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_ExitPark_Networked_214505783));
			base.RegisterTargetRpc(16U, new ClientRpcDelegate(this.RpcReader___Target_ExitPark_Networked_214505783));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Vehicles.LandVehicle));
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x000E1E44 File Offset: 0x000E0044
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.LandVehicleAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.LandVehicleAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<isReversing>k__BackingField.SetRegistered();
			this.syncVar___<brakesApplied>k__BackingField.SetRegistered();
			this.syncVar___currentSteerAngle.SetRegistered();
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x000E1E78 File Offset: 0x000E0078
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x000E1E88 File Offset: 0x000E0088
		private void RpcWriter___Observers_SetIsPlayerOwned_214505783(NetworkConnection conn, bool playerOwned)
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
			writer.WriteBoolean(playerOwned);
			base.SendObserversRpc(0U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x000E1F3E File Offset: 0x000E013E
		public void RpcLogic___SetIsPlayerOwned_214505783(NetworkConnection conn, bool playerOwned)
		{
			this.IsPlayerOwned = playerOwned;
			if (base.GetComponent<StorageEntity>() != null)
			{
				base.GetComponent<StorageEntity>().AccessSettings = (playerOwned ? StorageEntity.EAccessSettings.Full : StorageEntity.EAccessSettings.Closed);
			}
			this.RefreshPoI();
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000E1F70 File Offset: 0x000E0170
		private void RpcReader___Observers_SetIsPlayerOwned_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool playerOwned = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsPlayerOwned_214505783(null, playerOwned);
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x000E1FAC File Offset: 0x000E01AC
		private void RpcWriter___Target_SetIsPlayerOwned_214505783(NetworkConnection conn, bool playerOwned)
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
			writer.WriteBoolean(playerOwned);
			base.SendTargetRpc(1U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x000E2064 File Offset: 0x000E0264
		private void RpcReader___Target_SetIsPlayerOwned_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool playerOwned = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsPlayerOwned_214505783(base.LocalConnection, playerOwned);
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x000E209C File Offset: 0x000E029C
		private void RpcWriter___Server_SetOwner_328543758(NetworkConnection conn)
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
			writer.WriteNetworkConnection(conn);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x000E2143 File Offset: 0x000E0343
		protected virtual void RpcLogic___SetOwner_328543758(NetworkConnection conn)
		{
			base.NetworkObject.GiveOwnership(conn);
			this.OnOwnerChanged();
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x000E2158 File Offset: 0x000E0358
		private void RpcReader___Server_SetOwner_328543758(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetOwner_328543758(conn2);
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x000E218C File Offset: 0x000E038C
		private void RpcWriter___Observers_OnOwnerChanged_2166136261()
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
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x000E2238 File Offset: 0x000E0438
		protected virtual void RpcLogic___OnOwnerChanged_2166136261()
		{
			if (base.NetworkObject.Owner == base.LocalConnection || (base.NetworkObject.OwnerId == -1 && InstanceFinder.IsHost))
			{
				Console.Log("Local client owns vehicle", null);
				this.rb.isKinematic = false;
				this.rb.interpolation = RigidbodyInterpolation.Interpolate;
				base.GetComponent<NetworkTransform>().ClearReplicateCache();
				base.GetComponent<NetworkTransform>().ForceSend();
				return;
			}
			Console.Log("Local client no longer owns vehicle", null);
			if (!InstanceFinder.IsHost || (InstanceFinder.IsHost && !this.localPlayerIsDriver && this.CurrentPlayerOccupancy > 0))
			{
				this.rb.interpolation = RigidbodyInterpolation.None;
				Debug.Log("No interpolation");
				this.rb.isKinematic = false;
				this.rb.isKinematic = true;
			}
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x000E2304 File Offset: 0x000E0504
		private void RpcReader___Observers_OnOwnerChanged_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___OnOwnerChanged_2166136261();
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x000E2324 File Offset: 0x000E0524
		private void RpcWriter___Server_SetTransform_Server_3848837105(Vector3 pos, Quaternion rot)
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
			writer.WriteVector3(pos);
			writer.WriteQuaternion(rot, AutoPackType.Packed);
			base.SendServerRpc(4U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x000E23DD File Offset: 0x000E05DD
		public void RpcLogic___SetTransform_Server_3848837105(Vector3 pos, Quaternion rot)
		{
			this.SetTransform(pos, rot);
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x000E23E8 File Offset: 0x000E05E8
		private void RpcReader___Server_SetTransform_Server_3848837105(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Vector3 pos = PooledReader0.ReadVector3();
			Quaternion rot = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetTransform_Server_3848837105(pos, rot);
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x000E243C File Offset: 0x000E063C
		private void RpcWriter___Observers_SetTransform_3848837105(Vector3 pos, Quaternion rot)
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
			writer.WriteVector3(pos);
			writer.WriteQuaternion(rot, AutoPackType.Packed);
			base.SendObserversRpc(5U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x000E2504 File Offset: 0x000E0704
		public void RpcLogic___SetTransform_3848837105(Vector3 pos, Quaternion rot)
		{
			base.transform.position = pos;
			base.transform.rotation = rot;
			this.rb.position = pos;
			this.rb.rotation = rot;
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x000E2538 File Offset: 0x000E0738
		private void RpcReader___Observers_SetTransform_3848837105(PooledReader PooledReader0, Channel channel)
		{
			Vector3 pos = PooledReader0.ReadVector3();
			Quaternion rot = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetTransform_3848837105(pos, rot);
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x000E258C File Offset: 0x000E078C
		private void RpcWriter___Server_SetSteeringAngle_431000436(float sa)
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
			writer.WriteSingle(sa, AutoPackType.Unpacked);
			base.SendServerRpc(6U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x000E2638 File Offset: 0x000E0838
		private void RpcLogic___SetSteeringAngle_431000436(float sa)
		{
			this.sync___set_value_currentSteerAngle(sa, true);
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000E2644 File Offset: 0x000E0844
		private void RpcReader___Server_SetSteeringAngle_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float sa = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetSteeringAngle_431000436(sa);
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x000E267C File Offset: 0x000E087C
		private void RpcWriter___Observers_SetSeatOccupant_3428404692(NetworkConnection conn, int seatIndex, NetworkConnection occupant)
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
			writer.WriteInt32(seatIndex, AutoPackType.Packed);
			writer.WriteNetworkConnection(occupant);
			base.SendObserversRpc(7U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x000E2744 File Offset: 0x000E0944
		private void RpcLogic___SetSeatOccupant_3428404692(NetworkConnection conn, int seatIndex, NetworkConnection occupant)
		{
			Player occupant2 = this.Seats[seatIndex].Occupant;
			this.Seats[seatIndex].Occupant = Player.GetPlayer(occupant);
			occupant != null;
			if (seatIndex == 0)
			{
				if (occupant != null)
				{
					if (this.onVehicleStart != null)
					{
						this.onVehicleStart.Invoke();
					}
				}
				else if (this.onVehicleStop != null)
				{
					this.onVehicleStop.Invoke();
				}
			}
			if (occupant != null)
			{
				if (this.onPlayerEnterVehicle != null)
				{
					this.onPlayerEnterVehicle(this.Seats[seatIndex].Occupant);
				}
			}
			else if (this.onPlayerExitVehicle != null)
			{
				this.onPlayerExitVehicle(occupant2);
			}
			this.isOccupied = (this.Seats.Count((VehicleSeat s) => s.isOccupied) > 0);
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x000E2820 File Offset: 0x000E0A20
		private void RpcReader___Observers_SetSeatOccupant_3428404692(PooledReader PooledReader0, Channel channel)
		{
			int seatIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			NetworkConnection occupant = PooledReader0.ReadNetworkConnection();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSeatOccupant_3428404692(null, seatIndex, occupant);
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x000E2874 File Offset: 0x000E0A74
		private void RpcWriter___Target_SetSeatOccupant_3428404692(NetworkConnection conn, int seatIndex, NetworkConnection occupant)
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
			writer.WriteInt32(seatIndex, AutoPackType.Packed);
			writer.WriteNetworkConnection(occupant);
			base.SendTargetRpc(8U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x000E293C File Offset: 0x000E0B3C
		private void RpcReader___Target_SetSeatOccupant_3428404692(PooledReader PooledReader0, Channel channel)
		{
			int seatIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			NetworkConnection occupant = PooledReader0.ReadNetworkConnection();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetSeatOccupant_3428404692(base.LocalConnection, seatIndex, occupant);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x000E298C File Offset: 0x000E0B8C
		private void RpcWriter___Server_SetSeatOccupant_Server_3266232555(int seatIndex, NetworkConnection conn)
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
			writer.WriteInt32(seatIndex, AutoPackType.Packed);
			writer.WriteNetworkConnection(conn);
			base.SendServerRpc(9U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x000E2A45 File Offset: 0x000E0C45
		private void RpcLogic___SetSeatOccupant_Server_3266232555(int seatIndex, NetworkConnection conn)
		{
			this.SetSeatOccupant(null, seatIndex, conn);
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x000E2A50 File Offset: 0x000E0C50
		private void RpcReader___Server_SetSeatOccupant_Server_3266232555(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int seatIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetSeatOccupant_Server_3266232555(seatIndex, conn2);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x000E2AA4 File Offset: 0x000E0CA4
		private void RpcWriter___Server_SendOwnedColor_911055161(EVehicleColor col)
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
			writer.Write___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generated(col);
			base.SendServerRpc(10U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x000E2B4B File Offset: 0x000E0D4B
		public void RpcLogic___SendOwnedColor_911055161(EVehicleColor col)
		{
			this.SetOwnedColor(null, col);
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x000E2B58 File Offset: 0x000E0D58
		private void RpcReader___Server_SendOwnedColor_911055161(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			EVehicleColor col = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendOwnedColor_911055161(col);
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x000E2B98 File Offset: 0x000E0D98
		private void RpcWriter___Target_SetOwnedColor_1679996372(NetworkConnection conn, EVehicleColor col)
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
			writer.Write___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generated(col);
			base.SendTargetRpc(11U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x000E2C4D File Offset: 0x000E0E4D
		protected virtual void RpcLogic___SetOwnedColor_1679996372(NetworkConnection conn, EVehicleColor col)
		{
			this.OwnedColor = col;
			this.ApplyOwnedColor();
			this.RefreshPoI();
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x000E2C64 File Offset: 0x000E0E64
		private void RpcReader___Target_SetOwnedColor_1679996372(PooledReader PooledReader0, Channel channel)
		{
			EVehicleColor col = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetOwnedColor_1679996372(base.LocalConnection, col);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x000E2C9C File Offset: 0x000E0E9C
		private void RpcWriter___Observers_SetOwnedColor_1679996372(NetworkConnection conn, EVehicleColor col)
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
			writer.Write___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generated(col);
			base.SendObserversRpc(12U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x000E2D54 File Offset: 0x000E0F54
		private void RpcReader___Observers_SetOwnedColor_1679996372(PooledReader PooledReader0, Channel channel)
		{
			EVehicleColor col = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.Modification.EVehicleColorFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetOwnedColor_1679996372(null, col);
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x000E2D90 File Offset: 0x000E0F90
		private void RpcWriter___Observers_Park_Networked_2633993806(NetworkConnection conn, ParkData parkData)
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
			writer.Write___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generated(parkData);
			base.SendObserversRpc(13U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x000E2E46 File Offset: 0x000E1046
		private void RpcLogic___Park_Networked_2633993806(NetworkConnection conn, ParkData parkData)
		{
			this.Park(conn, parkData, false);
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x000E2E54 File Offset: 0x000E1054
		private void RpcReader___Observers_Park_Networked_2633993806(PooledReader PooledReader0, Channel channel)
		{
			ParkData parkData = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Park_Networked_2633993806(null, parkData);
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x000E2E90 File Offset: 0x000E1090
		private void RpcWriter___Target_Park_Networked_2633993806(NetworkConnection conn, ParkData parkData)
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
			writer.Write___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generated(parkData);
			base.SendTargetRpc(14U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x000E2F48 File Offset: 0x000E1148
		private void RpcReader___Target_Park_Networked_2633993806(PooledReader PooledReader0, Channel channel)
		{
			ParkData parkData = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.ParkDataFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Park_Networked_2633993806(base.LocalConnection, parkData);
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x000E2F80 File Offset: 0x000E1180
		private void RpcWriter___Observers_ExitPark_Networked_214505783(NetworkConnection conn, bool moveToExitPoint = true)
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
			writer.WriteBoolean(moveToExitPoint);
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x000E3036 File Offset: 0x000E1236
		public void RpcLogic___ExitPark_Networked_214505783(NetworkConnection conn, bool moveToExitPoint = true)
		{
			this.ExitPark(moveToExitPoint);
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x000E3040 File Offset: 0x000E1240
		private void RpcReader___Observers_ExitPark_Networked_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool moveToExitPoint = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ExitPark_Networked_214505783(null, moveToExitPoint);
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x000E307C File Offset: 0x000E127C
		private void RpcWriter___Target_ExitPark_Networked_214505783(NetworkConnection conn, bool moveToExitPoint = true)
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
			writer.WriteBoolean(moveToExitPoint);
			base.SendTargetRpc(16U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x000E3134 File Offset: 0x000E1334
		private void RpcReader___Target_ExitPark_Networked_214505783(PooledReader PooledReader0, Channel channel)
		{
			bool moveToExitPoint = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ExitPark_Networked_214505783(base.LocalConnection, moveToExitPoint);
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060035E5 RID: 13797 RVA: 0x000E316B File Offset: 0x000E136B
		// (set) Token: 0x060035E6 RID: 13798 RVA: 0x000E3173 File Offset: 0x000E1373
		public float SyncAccessor_currentSteerAngle
		{
			get
			{
				return this.currentSteerAngle;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.currentSteerAngle = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___currentSteerAngle.SetValue(value, value);
				}
			}
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x000E31B0 File Offset: 0x000E13B0
		public virtual bool LandVehicle(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<isReversing>k__BackingField(this.syncVar___<isReversing>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value = PooledReader0.ReadBoolean();
				this.sync___set_value_<isReversing>k__BackingField(value, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<brakesApplied>k__BackingField(this.syncVar___<brakesApplied>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value2 = PooledReader0.ReadBoolean();
				this.sync___set_value_<brakesApplied>k__BackingField(value2, Boolean2);
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
					this.sync___set_value_currentSteerAngle(this.syncVar___currentSteerAngle.GetValue(true), true);
					return true;
				}
				float value3 = PooledReader0.ReadSingle(AutoPackType.Unpacked);
				this.sync___set_value_currentSteerAngle(value3, Boolean2);
				return true;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060035E8 RID: 13800 RVA: 0x000E328F File Offset: 0x000E148F
		// (set) Token: 0x060035E9 RID: 13801 RVA: 0x000E3297 File Offset: 0x000E1497
		public bool SyncAccessor_<brakesApplied>k__BackingField
		{
			get
			{
				return this.<brakesApplied>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<brakesApplied>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<brakesApplied>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060035EA RID: 13802 RVA: 0x000E32D3 File Offset: 0x000E14D3
		// (set) Token: 0x060035EB RID: 13803 RVA: 0x000E32DB File Offset: 0x000E14DB
		public bool SyncAccessor_<isReversing>k__BackingField
		{
			get
			{
				return this.<isReversing>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<isReversing>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<isReversing>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x000E3318 File Offset: 0x000E1518
		protected virtual void dll()
		{
			this.OccupantNPCs = new NPC[this.Seats.Length];
			this.boundingBox.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			for (int i = 0; i < this.driveWheels.Length; i++)
			{
				this.wheels.Add(this.driveWheels[i].GetComponent<Wheel>());
			}
			for (int j = 0; j < this.steerWheels.Length; j++)
			{
				if (!this.wheels.Contains(this.steerWheels[j].GetComponent<Wheel>()))
				{
					this.wheels.Add(this.steerWheels[j].GetComponent<Wheel>());
				}
			}
			this.OwnedColor = this.DefaultColor;
			this.InitializeSaveable();
			if (base.GetComponent<StorageEntity>() != null)
			{
				base.GetComponent<StorageEntity>().AccessSettings = StorageEntity.EAccessSettings.Closed;
			}
			this.SetObstaclesActive(true);
			this.RefreshPoI();
		}

		// Token: 0x04002662 RID: 9826
		public const float KINEMATIC_THRESHOLD_DISTANCE = 30f;

		// Token: 0x04002663 RID: 9827
		public const float MAX_TURNOVER_SPEED = 5f;

		// Token: 0x04002664 RID: 9828
		public const float TURNOVER_FORCE = 8f;

		// Token: 0x04002665 RID: 9829
		public const bool USE_WHEEL = false;

		// Token: 0x04002666 RID: 9830
		public const float SPEED_DISPLAY_MULTIPLIER = 1.4f;

		// Token: 0x04002667 RID: 9831
		public bool DEBUG;

		// Token: 0x04002668 RID: 9832
		[Header("Settings")]
		[SerializeField]
		protected string vehicleName = "Vehicle";

		// Token: 0x04002669 RID: 9833
		[SerializeField]
		protected string vehicleCode = "vehicle_code";

		// Token: 0x0400266A RID: 9834
		[SerializeField]
		protected float vehiclePrice = 1000f;

		// Token: 0x0400266D RID: 9837
		public bool UseHumanoidCollider = true;

		// Token: 0x0400266F RID: 9839
		public bool SpawnAsPlayerOwned;

		// Token: 0x04002671 RID: 9841
		[Header("References")]
		[SerializeField]
		protected GameObject vehicleModel;

		// Token: 0x04002672 RID: 9842
		[SerializeField]
		protected WheelCollider[] driveWheels;

		// Token: 0x04002673 RID: 9843
		[SerializeField]
		protected WheelCollider[] steerWheels;

		// Token: 0x04002674 RID: 9844
		[SerializeField]
		protected WheelCollider[] handbrakeWheels;

		// Token: 0x04002675 RID: 9845
		[HideInInspector]
		public List<Wheel> wheels = new List<Wheel>();

		// Token: 0x04002676 RID: 9846
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x04002677 RID: 9847
		[SerializeField]
		protected List<Transform> exitPoints = new List<Transform>();

		// Token: 0x04002678 RID: 9848
		[SerializeField]
		protected Rigidbody rb;

		// Token: 0x04002679 RID: 9849
		public VehicleSeat[] Seats;

		// Token: 0x0400267A RID: 9850
		public BoxCollider boundingBox;

		// Token: 0x0400267B RID: 9851
		public VehicleAgent Agent;

		// Token: 0x0400267C RID: 9852
		public SmoothedVelocityCalculator VelocityCalculator;

		// Token: 0x0400267D RID: 9853
		public StorageDoorAnimation Trunk;

		// Token: 0x0400267E RID: 9854
		public NavMeshObstacle NavMeshObstacle;

		// Token: 0x0400267F RID: 9855
		public NavmeshCut NavmeshCut;

		// Token: 0x04002680 RID: 9856
		public VehicleHumanoidCollider HumanoidColliderContainer;

		// Token: 0x04002681 RID: 9857
		public POI POI;

		// Token: 0x04002682 RID: 9858
		[SerializeField]
		protected Transform centerOfMass;

		// Token: 0x04002683 RID: 9859
		[SerializeField]
		protected Transform cameraOrigin;

		// Token: 0x04002684 RID: 9860
		[SerializeField]
		protected VehicleLights lights;

		// Token: 0x04002685 RID: 9861
		[Header("Steer settings")]
		[SerializeField]
		protected float maxSteeringAngle = 25f;

		// Token: 0x04002686 RID: 9862
		[SerializeField]
		protected float steerRate = 50f;

		// Token: 0x04002687 RID: 9863
		[SerializeField]
		protected bool flipSteer;

		// Token: 0x0400268A RID: 9866
		[Header("Drive settings")]
		[SerializeField]
		protected AnimationCurve motorTorque = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 200f),
			new Keyframe(50f, 300f),
			new Keyframe(200f, 0f)
		});

		// Token: 0x0400268B RID: 9867
		public float TopSpeed = 60f;

		// Token: 0x0400268C RID: 9868
		[Range(2f, 16f)]
		[SerializeField]
		protected float diffGearing = 4f;

		// Token: 0x0400268D RID: 9869
		[SerializeField]
		protected float handBrakeForce = 300f;

		// Token: 0x0400268E RID: 9870
		[SerializeField]
		protected AnimationCurve brakeForce = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 200f),
			new Keyframe(50f, 300f),
			new Keyframe(200f, 0f)
		});

		// Token: 0x0400268F RID: 9871
		[Range(0.5f, 10f)]
		[SerializeField]
		protected float downforce = 1f;

		// Token: 0x04002690 RID: 9872
		[Range(0f, 1f)]
		[SerializeField]
		protected float reverseMultiplier = 0.35f;

		// Token: 0x04002691 RID: 9873
		[Header("Color Settings")]
		[SerializeField]
		protected LandVehicle.BodyMesh[] BodyMeshes;

		// Token: 0x04002692 RID: 9874
		public EVehicleColor DefaultColor = EVehicleColor.White;

		// Token: 0x04002694 RID: 9876
		private EVehicleColor DisplayedColor = EVehicleColor.White;

		// Token: 0x04002695 RID: 9877
		[Header("Outline settings")]
		[SerializeField]
		protected List<GameObject> outlineRenderers = new List<GameObject>();

		// Token: 0x04002696 RID: 9878
		protected Outlinable outlineEffect;

		// Token: 0x04002697 RID: 9879
		[Header("Control overrides")]
		public bool overrideControls;

		// Token: 0x04002698 RID: 9880
		public float throttleOverride;

		// Token: 0x04002699 RID: 9881
		public float steerOverride;

		// Token: 0x0400269A RID: 9882
		[Header("Storage settings")]
		public StorageEntity Storage;

		// Token: 0x0400269B RID: 9883
		private VehicleSeat localPlayerSeat;

		// Token: 0x040026A1 RID: 9889
		private List<float> previousSpeeds = new List<float>();

		// Token: 0x040026A2 RID: 9890
		private int previousSpeedsSampleSize = 20;

		// Token: 0x040026A4 RID: 9892
		[SyncVar(Channel = Channel.Unreliable, SendRate = 0.05f, WritePermissions = WritePermission.ClientUnsynchronized)]
		public float currentSteerAngle;

		// Token: 0x040026A5 RID: 9893
		private float lastFrameSteerAngle;

		// Token: 0x040026A6 RID: 9894
		private float lastReplicatedSteerAngle;

		// Token: 0x040026A7 RID: 9895
		private bool justExitedVehicle;

		// Token: 0x040026AC RID: 9900
		private Vector3 lastFramePosition = Vector3.zero;

		// Token: 0x040026AD RID: 9901
		private Transform closestExitPoint;

		// Token: 0x040026AE RID: 9902
		[HideInInspector]
		public ParkData CurrentParkData;

		// Token: 0x040026B1 RID: 9905
		private VehicleLoader loader = new VehicleLoader();

		// Token: 0x040026B5 RID: 9909
		public LandVehicle.VehiclePlayerEvent onPlayerEnterVehicle;

		// Token: 0x040026B6 RID: 9910
		public LandVehicle.VehiclePlayerEvent onPlayerExitVehicle;

		// Token: 0x040026B7 RID: 9911
		public UnityEvent onVehicleStart;

		// Token: 0x040026B8 RID: 9912
		public UnityEvent onVehicleStop;

		// Token: 0x040026B9 RID: 9913
		public UnityEvent onHandbrakeApplied;

		// Token: 0x040026BA RID: 9914
		public UnityEvent<Collision> onCollision = new UnityEvent<Collision>();

		// Token: 0x040026BB RID: 9915
		public SyncVar<float> syncVar___currentSteerAngle;

		// Token: 0x040026BC RID: 9916
		public SyncVar<bool> syncVar___<brakesApplied>k__BackingField;

		// Token: 0x040026BD RID: 9917
		public SyncVar<bool> syncVar___<isReversing>k__BackingField;

		// Token: 0x040026BE RID: 9918
		private bool dll_Excuted;

		// Token: 0x040026BF RID: 9919
		private bool dll_Excuted;

		// Token: 0x020007AC RID: 1964
		[Serializable]
		public class BodyMesh
		{
			// Token: 0x040026C0 RID: 9920
			public MeshRenderer Renderer;

			// Token: 0x040026C1 RID: 9921
			public int MaterialIndex;
		}

		// Token: 0x020007AD RID: 1965
		// (Invoke) Token: 0x060035EF RID: 13807
		public delegate void VehiclePlayerEvent(Player player);
	}
}
