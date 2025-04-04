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
using ScheduleOne.Vehicles.AI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007BF RID: 1983
	public class VehicleLights : NetworkBehaviour
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x0600363E RID: 13886 RVA: 0x000E45C1 File Offset: 0x000E27C1
		// (set) Token: 0x0600363F RID: 13887 RVA: 0x000E45C9 File Offset: 0x000E27C9
		public bool headLightsOn
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<headLightsOn>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true, RequireOwnership = false)]
			set
			{
				this.RpcWriter___Server_set_headLightsOn_1140765316(value);
				this.RpcLogic___set_headLightsOn_1140765316(value);
			}
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000E45DF File Offset: 0x000E27DF
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Vehicles.VehicleLights_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000E45F4 File Offset: 0x000E27F4
		protected virtual void Update()
		{
			if (this.vehicle.localPlayerIsDriver && this.hasHeadLights && GameInput.GetButtonDown(GameInput.ButtonCode.ToggleLights))
			{
				this.headLightsOn = !this.headLightsOn;
				if (this.headLightsOn)
				{
					if (this.onHeadlightsOn != null)
					{
						this.onHeadlightsOn.Invoke();
						return;
					}
				}
				else if (this.onHeadlightsOff != null)
				{
					this.onHeadlightsOff.Invoke();
				}
			}
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000E4660 File Offset: 0x000E2860
		protected virtual void FixedUpdate()
		{
			this.reverseLightsOn = this.vehicle.isReversing;
			if (this.agent == null || !this.agent.AutoDriving)
			{
				this.brakeLightsOn = this.vehicle.brakesApplied;
				return;
			}
			this.brakesAppliedHistory.Add(this.vehicle.brakesApplied);
			if (this.brakesAppliedHistory.Count > 60)
			{
				this.brakesAppliedHistory.RemoveAt(0);
			}
			int num = 0;
			for (int i = 0; i < this.brakesAppliedHistory.Count; i++)
			{
				if (this.brakesAppliedHistory[i])
				{
					num++;
				}
			}
			this.brakeLightsOn = ((float)num / (float)this.brakesAppliedHistory.Count > 0.2f);
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x000E4724 File Offset: 0x000E2924
		protected virtual void LateUpdate()
		{
			if (this.hasHeadLights && this.headLightsOn != this.headLightsApplied)
			{
				if (this.headLightsOn)
				{
					this.headLightsApplied = true;
					for (int i = 0; i < this.headLightMeshes.Length; i++)
					{
						this.headLightMeshes[i].material = this.headlightMat_On;
					}
					for (int j = 0; j < this.headLightSources.Length; j++)
					{
						this.headLightSources[j].Enabled = true;
					}
				}
				else
				{
					this.headLightsApplied = false;
					for (int k = 0; k < this.headLightMeshes.Length; k++)
					{
						this.headLightMeshes[k].material = this.headLightMat_Off;
					}
					for (int l = 0; l < this.headLightSources.Length; l++)
					{
						this.headLightSources[l].Enabled = false;
					}
				}
			}
			if (this.hasBrakeLights && this.brakeLightsOn != this.brakeLightsApplied)
			{
				if (this.brakeLightsOn)
				{
					this.brakeLightsApplied = true;
					for (int m = 0; m < this.brakeLightMeshes.Length; m++)
					{
						this.brakeLightMeshes[m].material = this.brakeLightMat_On;
					}
					if (this.vehicle.localPlayerIsInVehicle)
					{
						for (int n = 0; n < this.brakeLightSources.Length; n++)
						{
							this.brakeLightSources[n].enabled = true;
						}
					}
				}
				else
				{
					this.brakeLightsApplied = false;
					for (int num = 0; num < this.brakeLightMeshes.Length; num++)
					{
						this.brakeLightMeshes[num].material = this.brakeLightMat_Off;
					}
					for (int num2 = 0; num2 < this.brakeLightSources.Length; num2++)
					{
						this.brakeLightSources[num2].enabled = false;
					}
				}
			}
			if (this.hasReverseLights && this.reverseLightsOn != this.reverseLightsApplied)
			{
				if (this.reverseLightsOn)
				{
					this.reverseLightsApplied = true;
					for (int num3 = 0; num3 < this.reverseLightMeshes.Length; num3++)
					{
						this.reverseLightMeshes[num3].material = this.reverseLightMat_On;
					}
					if (this.vehicle.localPlayerIsInVehicle)
					{
						for (int num4 = 0; num4 < this.reverseLightSources.Length; num4++)
						{
							this.reverseLightSources[num4].enabled = true;
						}
						return;
					}
				}
				else
				{
					this.reverseLightsApplied = false;
					for (int num5 = 0; num5 < this.reverseLightMeshes.Length; num5++)
					{
						this.reverseLightMeshes[num5].material = this.reverseLightMat_Off;
					}
					for (int num6 = 0; num6 < this.reverseLightSources.Length; num6++)
					{
						this.reverseLightSources[num6].enabled = false;
					}
				}
			}
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x000E49DC File Offset: 0x000E2BDC
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleLightsAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleLightsAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<headLightsOn>k__BackingField = new SyncVar<bool>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, 0.25f, Channel.Unreliable, this.<headLightsOn>k__BackingField);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_set_headLightsOn_1140765316));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Vehicles.VehicleLights));
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x000E4A4E File Offset: 0x000E2C4E
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleLightsAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleLightsAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<headLightsOn>k__BackingField.SetRegistered();
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x000E4A6C File Offset: 0x000E2C6C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x000E4A7C File Offset: 0x000E2C7C
		private void RpcWriter___Server_set_headLightsOn_1140765316(bool value)
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
			writer.WriteBoolean(value);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x000E4B23 File Offset: 0x000E2D23
		public void RpcLogic___set_headLightsOn_1140765316(bool value)
		{
			this.sync___set_value_<headLightsOn>k__BackingField(value, true);
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x000E4B30 File Offset: 0x000E2D30
		private void RpcReader___Server_set_headLightsOn_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___set_headLightsOn_1140765316(value);
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x0600364B RID: 13899 RVA: 0x000E4B6E File Offset: 0x000E2D6E
		// (set) Token: 0x0600364C RID: 13900 RVA: 0x000E4B76 File Offset: 0x000E2D76
		public bool SyncAccessor_<headLightsOn>k__BackingField
		{
			get
			{
				return this.<headLightsOn>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<headLightsOn>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<headLightsOn>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000E4BB4 File Offset: 0x000E2DB4
		public virtual bool VehicleLights(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_<headLightsOn>k__BackingField(this.syncVar___<headLightsOn>k__BackingField.GetValue(true), true);
				return true;
			}
			bool value = PooledReader0.ReadBoolean();
			this.sync___set_value_<headLightsOn>k__BackingField(value, Boolean2);
			return true;
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x000E4C06 File Offset: 0x000E2E06
		protected virtual void dll()
		{
			this.agent = base.GetComponent<VehicleAgent>();
		}

		// Token: 0x0400270B RID: 9995
		public LandVehicle vehicle;

		// Token: 0x0400270C RID: 9996
		[Header("Headlights")]
		public bool hasHeadLights;

		// Token: 0x0400270D RID: 9997
		public MeshRenderer[] headLightMeshes;

		// Token: 0x0400270E RID: 9998
		public OptimizedLight[] headLightSources;

		// Token: 0x0400270F RID: 9999
		public Material headlightMat_On;

		// Token: 0x04002710 RID: 10000
		public Material headLightMat_Off;

		// Token: 0x04002712 RID: 10002
		protected bool headLightsApplied;

		// Token: 0x04002713 RID: 10003
		[Header("Brake lights")]
		public bool hasBrakeLights;

		// Token: 0x04002714 RID: 10004
		public MeshRenderer[] brakeLightMeshes;

		// Token: 0x04002715 RID: 10005
		public Light[] brakeLightSources;

		// Token: 0x04002716 RID: 10006
		public Material brakeLightMat_On;

		// Token: 0x04002717 RID: 10007
		public Material brakeLightMat_Off;

		// Token: 0x04002718 RID: 10008
		public Material brakeLightMat_Ambient;

		// Token: 0x04002719 RID: 10009
		protected bool brakeLightsOn;

		// Token: 0x0400271A RID: 10010
		protected bool brakeLightsApplied = true;

		// Token: 0x0400271B RID: 10011
		[Header("Reverse lights")]
		public bool hasReverseLights;

		// Token: 0x0400271C RID: 10012
		public MeshRenderer[] reverseLightMeshes;

		// Token: 0x0400271D RID: 10013
		public Light[] reverseLightSources;

		// Token: 0x0400271E RID: 10014
		public Material reverseLightMat_On;

		// Token: 0x0400271F RID: 10015
		public Material reverseLightMat_Off;

		// Token: 0x04002720 RID: 10016
		protected bool reverseLightsOn;

		// Token: 0x04002721 RID: 10017
		protected bool reverseLightsApplied = true;

		// Token: 0x04002722 RID: 10018
		public UnityEvent onHeadlightsOn;

		// Token: 0x04002723 RID: 10019
		public UnityEvent onHeadlightsOff;

		// Token: 0x04002724 RID: 10020
		private List<bool> brakesAppliedHistory = new List<bool>();

		// Token: 0x04002725 RID: 10021
		private VehicleAgent agent;

		// Token: 0x04002726 RID: 10022
		public SyncVar<bool> syncVar___<headLightsOn>k__BackingField;

		// Token: 0x04002727 RID: 10023
		private bool dll_Excuted;

		// Token: 0x04002728 RID: 10024
		private bool dll_Excuted;
	}
}
