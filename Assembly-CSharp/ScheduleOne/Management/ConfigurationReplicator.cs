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
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x02000555 RID: 1365
	public class ConfigurationReplicator : NetworkBehaviour
	{
		// Token: 0x0600217A RID: 8570 RVA: 0x00089D30 File Offset: 0x00087F30
		public void ReplicateField(ConfigField field, NetworkConnection conn = null)
		{
			int num = this.Configuration.Fields.IndexOf(field);
			if (num == -1)
			{
				Console.LogError("Failed to find field in configuration", null);
				return;
			}
			if (field is ItemField)
			{
				ItemField itemField = (ItemField)field;
				this.SendItemField(num, (itemField.SelectedItem != null) ? itemField.SelectedItem.name : string.Empty);
				return;
			}
			if (field is NPCField)
			{
				NPCField npcfield = (NPCField)field;
				this.SendNPCField(num, (npcfield.SelectedNPC != null) ? npcfield.SelectedNPC.NetworkObject : null);
				return;
			}
			if (field is ObjectField)
			{
				ObjectField objectField = (ObjectField)field;
				NetworkObject obj = null;
				if (objectField.SelectedObject != null)
				{
					obj = objectField.SelectedObject.NetworkObject;
				}
				this.SendObjectField(num, obj);
				return;
			}
			if (field is ObjectListField)
			{
				ObjectListField objectListField = (ObjectListField)field;
				List<NetworkObject> list = new List<NetworkObject>();
				for (int i = 0; i < objectListField.SelectedObjects.Count; i++)
				{
					list.Add(objectListField.SelectedObjects[i].NetworkObject);
				}
				this.SendObjectListField(num, list);
				return;
			}
			if (field is StationRecipeField)
			{
				StationRecipeField stationRecipeField = (StationRecipeField)field;
				int recipeIndex = -1;
				if (stationRecipeField.SelectedRecipe != null)
				{
					recipeIndex = stationRecipeField.Options.IndexOf(stationRecipeField.SelectedRecipe);
				}
				this.SendRecipeField(num, recipeIndex);
				return;
			}
			if (field is NumberField)
			{
				NumberField numberField = (NumberField)field;
				this.SendNumberField(num, numberField.Value);
				return;
			}
			if (field is RouteListField)
			{
				RouteListField routeListField = (RouteListField)field;
				this.SendRouteListField(num, (from x in routeListField.Routes
				select x.GetData()).ToArray<AdvancedTransitRouteData>());
				return;
			}
			if (field is QualityField)
			{
				QualityField qualityField = (QualityField)field;
				this.SendQualityField(num, qualityField.Value);
				return;
			}
			string str = "Failed to find replication method for ";
			Type type = field.GetType();
			Console.LogError(str + ((type != null) ? type.ToString() : null), null);
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x00089F3F File Offset: 0x0008813F
		[ServerRpc(RequireOwnership = false)]
		private void SendItemField(int fieldIndex, string value)
		{
			this.RpcWriter___Server_SendItemField_2801973956(fieldIndex, value);
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x00089F50 File Offset: 0x00088150
		[ObserversRpc]
		private void ReceiveItemField(int fieldIndex, string value)
		{
			this.RpcWriter___Observers_ReceiveItemField_2801973956(fieldIndex, value);
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x00089F6B File Offset: 0x0008816B
		[ServerRpc(RequireOwnership = false)]
		private void SendNPCField(int fieldIndex, NetworkObject npcObject)
		{
			this.RpcWriter___Server_SendNPCField_1687693739(fieldIndex, npcObject);
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x00089F7C File Offset: 0x0008817C
		[ObserversRpc]
		private void ReceiveNPCField(int fieldIndex, NetworkObject npcObject)
		{
			this.RpcWriter___Observers_ReceiveNPCField_1687693739(fieldIndex, npcObject);
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x00089F97 File Offset: 0x00088197
		[ServerRpc(RequireOwnership = false)]
		private void SendObjectField(int fieldIndex, NetworkObject obj)
		{
			this.RpcWriter___Server_SendObjectField_1687693739(fieldIndex, obj);
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x00089FA8 File Offset: 0x000881A8
		[ObserversRpc]
		private void ReceiveObjectField(int fieldIndex, NetworkObject obj)
		{
			this.RpcWriter___Observers_ReceiveObjectField_1687693739(fieldIndex, obj);
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x00089FC3 File Offset: 0x000881C3
		[ServerRpc(RequireOwnership = false)]
		private void SendObjectListField(int fieldIndex, List<NetworkObject> objects)
		{
			this.RpcWriter___Server_SendObjectListField_690244341(fieldIndex, objects);
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x00089FD4 File Offset: 0x000881D4
		[ObserversRpc]
		private void ReceiveObjectListField(int fieldIndex, List<NetworkObject> objects)
		{
			this.RpcWriter___Observers_ReceiveObjectListField_690244341(fieldIndex, objects);
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x00089FEF File Offset: 0x000881EF
		[ServerRpc(RequireOwnership = false)]
		private void SendRecipeField(int fieldIndex, int recipeIndex)
		{
			this.RpcWriter___Server_SendRecipeField_1692629761(fieldIndex, recipeIndex);
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x0008A000 File Offset: 0x00088200
		[ObserversRpc]
		private void ReceiveRecipeField(int fieldIndex, int recipeIndex)
		{
			this.RpcWriter___Observers_ReceiveRecipeField_1692629761(fieldIndex, recipeIndex);
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x0008A01B File Offset: 0x0008821B
		[ServerRpc(RequireOwnership = false)]
		private void SendNumberField(int fieldIndex, float value)
		{
			this.RpcWriter___Server_SendNumberField_1293284375(fieldIndex, value);
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x0008A02B File Offset: 0x0008822B
		[ObserversRpc]
		private void ReceiveNumberField(int fieldIndex, float value)
		{
			this.RpcWriter___Observers_ReceiveNumberField_1293284375(fieldIndex, value);
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x0008A03B File Offset: 0x0008823B
		[ServerRpc(RequireOwnership = false)]
		private void SendRouteListField(int fieldIndex, AdvancedTransitRouteData[] value)
		{
			this.RpcWriter___Server_SendRouteListField_3226448297(fieldIndex, value);
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x0008A04C File Offset: 0x0008824C
		[ObserversRpc]
		private void ReceiveRouteListField(int fieldIndex, AdvancedTransitRouteData[] value)
		{
			this.RpcWriter___Observers_ReceiveRouteListField_3226448297(fieldIndex, value);
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0008A067 File Offset: 0x00088267
		[ServerRpc(RequireOwnership = false)]
		private void SendQualityField(int fieldIndex, EQuality quality)
		{
			this.RpcWriter___Server_SendQualityField_3536682170(fieldIndex, quality);
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x0008A077 File Offset: 0x00088277
		[ObserversRpc]
		private void ReceiveQualityField(int fieldIndex, EQuality value)
		{
			this.RpcWriter___Observers_ReceiveQualityField_3536682170(fieldIndex, value);
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x0008A088 File Offset: 0x00088288
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Management.ConfigurationReplicatorAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Management.ConfigurationReplicatorAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendItemField_2801973956));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveItemField_2801973956));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendNPCField_1687693739));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveNPCField_1687693739));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_SendObjectField_1687693739));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveObjectField_1687693739));
			base.RegisterServerRpc(6U, new ServerRpcDelegate(this.RpcReader___Server_SendObjectListField_690244341));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveObjectListField_690244341));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendRecipeField_1692629761));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveRecipeField_1692629761));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SendNumberField_1293284375));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveNumberField_1293284375));
			base.RegisterServerRpc(12U, new ServerRpcDelegate(this.RpcReader___Server_SendRouteListField_3226448297));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveRouteListField_3226448297));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_SendQualityField_3536682170));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveQualityField_3536682170));
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x0008A216 File Offset: 0x00088416
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Management.ConfigurationReplicatorAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Management.ConfigurationReplicatorAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x0008A229 File Offset: 0x00088429
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x0008A238 File Offset: 0x00088438
		private void RpcWriter___Server_SendItemField_2801973956(int fieldIndex, string value)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.WriteString(value);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x0008A2F1 File Offset: 0x000884F1
		private void RpcLogic___SendItemField_2801973956(int fieldIndex, string value)
		{
			this.ReceiveItemField(fieldIndex, value);
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x0008A2FC File Offset: 0x000884FC
		private void RpcReader___Server_SendItemField_2801973956(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			string value = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendItemField_2801973956(fieldIndex, value);
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x0008A344 File Offset: 0x00088544
		private void RpcWriter___Observers_ReceiveItemField_2801973956(int fieldIndex, string value)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.WriteString(value);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x0008A40C File Offset: 0x0008860C
		private void RpcLogic___ReceiveItemField_2801973956(int fieldIndex, string value)
		{
			ItemField itemField = this.Configuration.Fields[fieldIndex] as ItemField;
			ItemDefinition item = null;
			if (value != string.Empty)
			{
				item = Registry.GetItem(value);
			}
			itemField.SetItem(item, false);
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x0008A44C File Offset: 0x0008864C
		private void RpcReader___Observers_ReceiveItemField_2801973956(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			string value = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveItemField_2801973956(fieldIndex, value);
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x0008A494 File Offset: 0x00088694
		private void RpcWriter___Server_SendNPCField_1687693739(int fieldIndex, NetworkObject npcObject)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.WriteNetworkObject(npcObject);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x0008A54D File Offset: 0x0008874D
		private void RpcLogic___SendNPCField_1687693739(int fieldIndex, NetworkObject npcObject)
		{
			this.ReceiveNPCField(fieldIndex, npcObject);
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x0008A558 File Offset: 0x00088758
		private void RpcReader___Server_SendNPCField_1687693739(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			NetworkObject npcObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendNPCField_1687693739(fieldIndex, npcObject);
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x0008A5A0 File Offset: 0x000887A0
		private void RpcWriter___Observers_ReceiveNPCField_1687693739(int fieldIndex, NetworkObject npcObject)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.WriteNetworkObject(npcObject);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x0008A668 File Offset: 0x00088868
		private void RpcLogic___ReceiveNPCField_1687693739(int fieldIndex, NetworkObject npcObject)
		{
			NPCField npcfield = this.Configuration.Fields[fieldIndex] as NPCField;
			NPC npc = null;
			if (npcObject != null)
			{
				npc = npcObject.GetComponent<NPC>();
			}
			npcfield.SetNPC(npc, false);
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x0008A6A4 File Offset: 0x000888A4
		private void RpcReader___Observers_ReceiveNPCField_1687693739(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			NetworkObject npcObject = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveNPCField_1687693739(fieldIndex, npcObject);
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x0008A6EC File Offset: 0x000888EC
		private void RpcWriter___Server_SendObjectField_1687693739(int fieldIndex, NetworkObject obj)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.WriteNetworkObject(obj);
			base.SendServerRpc(4U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x0008A7A5 File Offset: 0x000889A5
		private void RpcLogic___SendObjectField_1687693739(int fieldIndex, NetworkObject obj)
		{
			this.ReceiveObjectField(fieldIndex, obj);
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x0008A7B0 File Offset: 0x000889B0
		private void RpcReader___Server_SendObjectField_1687693739(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			NetworkObject obj = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendObjectField_1687693739(fieldIndex, obj);
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x0008A7F8 File Offset: 0x000889F8
		private void RpcWriter___Observers_ReceiveObjectField_1687693739(int fieldIndex, NetworkObject obj)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.WriteNetworkObject(obj);
			base.SendObserversRpc(5U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x0008A8C0 File Offset: 0x00088AC0
		private void RpcLogic___ReceiveObjectField_1687693739(int fieldIndex, NetworkObject obj)
		{
			ObjectField objectField = this.Configuration.Fields[fieldIndex] as ObjectField;
			BuildableItem obj2 = null;
			if (obj != null)
			{
				obj2 = obj.GetComponent<BuildableItem>();
			}
			objectField.SetObject(obj2, false);
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x0008A8FC File Offset: 0x00088AFC
		private void RpcReader___Observers_ReceiveObjectField_1687693739(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			NetworkObject obj = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveObjectField_1687693739(fieldIndex, obj);
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x0008A944 File Offset: 0x00088B44
		private void RpcWriter___Server_SendObjectListField_690244341(int fieldIndex, List<NetworkObject> objects)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.Write___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generated(objects);
			base.SendServerRpc(6U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x0008A9FD File Offset: 0x00088BFD
		private void RpcLogic___SendObjectListField_690244341(int fieldIndex, List<NetworkObject> objects)
		{
			this.ReceiveObjectListField(fieldIndex, objects);
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x0008AA08 File Offset: 0x00088C08
		private void RpcReader___Server_SendObjectListField_690244341(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			List<NetworkObject> objects = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendObjectListField_690244341(fieldIndex, objects);
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x0008AA50 File Offset: 0x00088C50
		private void RpcWriter___Observers_ReceiveObjectListField_690244341(int fieldIndex, List<NetworkObject> objects)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.Write___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generated(objects);
			base.SendObserversRpc(7U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x0008AB18 File Offset: 0x00088D18
		private void RpcLogic___ReceiveObjectListField_690244341(int fieldIndex, List<NetworkObject> objects)
		{
			ObjectListField objectListField = this.Configuration.Fields[fieldIndex] as ObjectListField;
			List<BuildableItem> list = new List<BuildableItem>();
			for (int i = 0; i < objects.Count; i++)
			{
				list.Add(objects[i].GetComponent<BuildableItem>());
			}
			objectListField.SetList(list, false);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x0008AB70 File Offset: 0x00088D70
		private void RpcReader___Observers_ReceiveObjectListField_690244341(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			List<NetworkObject> objects = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveObjectListField_690244341(fieldIndex, objects);
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x0008ABB8 File Offset: 0x00088DB8
		private void RpcWriter___Server_SendRecipeField_1692629761(int fieldIndex, int recipeIndex)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.WriteInt32(recipeIndex, AutoPackType.Packed);
			base.SendServerRpc(8U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x0008AC76 File Offset: 0x00088E76
		private void RpcLogic___SendRecipeField_1692629761(int fieldIndex, int recipeIndex)
		{
			this.ReceiveRecipeField(fieldIndex, recipeIndex);
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x0008AC80 File Offset: 0x00088E80
		private void RpcReader___Server_SendRecipeField_1692629761(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int recipeIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendRecipeField_1692629761(fieldIndex, recipeIndex);
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x0008ACCC File Offset: 0x00088ECC
		private void RpcWriter___Observers_ReceiveRecipeField_1692629761(int fieldIndex, int recipeIndex)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.WriteInt32(recipeIndex, AutoPackType.Packed);
			base.SendObserversRpc(9U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x0008AD9C File Offset: 0x00088F9C
		private void RpcLogic___ReceiveRecipeField_1692629761(int fieldIndex, int recipeIndex)
		{
			StationRecipeField stationRecipeField = this.Configuration.Fields[fieldIndex] as StationRecipeField;
			StationRecipe recipe = null;
			if (recipeIndex != -1)
			{
				recipe = stationRecipeField.Options[recipeIndex];
			}
			stationRecipeField.SetRecipe(recipe, false);
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x0008ADDC File Offset: 0x00088FDC
		private void RpcReader___Observers_ReceiveRecipeField_1692629761(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int recipeIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveRecipeField_1692629761(fieldIndex, recipeIndex);
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x0008AE28 File Offset: 0x00089028
		private void RpcWriter___Server_SendNumberField_1293284375(int fieldIndex, float value)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.WriteSingle(value, AutoPackType.Unpacked);
			base.SendServerRpc(10U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x0008AEE6 File Offset: 0x000890E6
		private void RpcLogic___SendNumberField_1293284375(int fieldIndex, float value)
		{
			this.ReceiveNumberField(fieldIndex, value);
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x0008AEF0 File Offset: 0x000890F0
		private void RpcReader___Server_SendNumberField_1293284375(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendNumberField_1293284375(fieldIndex, value);
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x0008AF3C File Offset: 0x0008913C
		private void RpcWriter___Observers_ReceiveNumberField_1293284375(int fieldIndex, float value)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.WriteSingle(value, AutoPackType.Unpacked);
			base.SendObserversRpc(11U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x0008B009 File Offset: 0x00089209
		private void RpcLogic___ReceiveNumberField_1293284375(int fieldIndex, float value)
		{
			(this.Configuration.Fields[fieldIndex] as NumberField).SetValue(value, false);
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x0008B028 File Offset: 0x00089228
		private void RpcReader___Observers_ReceiveNumberField_1293284375(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveNumberField_1293284375(fieldIndex, value);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x0008B074 File Offset: 0x00089274
		private void RpcWriter___Server_SendRouteListField_3226448297(int fieldIndex, AdvancedTransitRouteData[] value)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.Write___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generated(value);
			base.SendServerRpc(12U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x0008B12D File Offset: 0x0008932D
		private void RpcLogic___SendRouteListField_3226448297(int fieldIndex, AdvancedTransitRouteData[] value)
		{
			this.ReceiveRouteListField(fieldIndex, value);
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x0008B138 File Offset: 0x00089338
		private void RpcReader___Server_SendRouteListField_3226448297(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			AdvancedTransitRouteData[] value = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendRouteListField_3226448297(fieldIndex, value);
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x0008B180 File Offset: 0x00089380
		private void RpcWriter___Observers_ReceiveRouteListField_3226448297(int fieldIndex, AdvancedTransitRouteData[] value)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.Write___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generated(value);
			base.SendObserversRpc(13U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x0008B248 File Offset: 0x00089448
		private void RpcLogic___ReceiveRouteListField_3226448297(int fieldIndex, AdvancedTransitRouteData[] value)
		{
			(this.Configuration.Fields[fieldIndex] as RouteListField).SetList((from x in value
			select new AdvancedTransitRoute(x)).ToList<AdvancedTransitRoute>(), false, false);
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x0008B29C File Offset: 0x0008949C
		private void RpcReader___Observers_ReceiveRouteListField_3226448297(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			AdvancedTransitRouteData[] value = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveRouteListField_3226448297(fieldIndex, value);
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x0008B2E4 File Offset: 0x000894E4
		private void RpcWriter___Server_SendQualityField_3536682170(int fieldIndex, EQuality quality)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(quality);
			base.SendServerRpc(14U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x0008B39D File Offset: 0x0008959D
		private void RpcLogic___SendQualityField_3536682170(int fieldIndex, EQuality quality)
		{
			this.ReceiveQualityField(fieldIndex, quality);
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x0008B3A8 File Offset: 0x000895A8
		private void RpcReader___Server_SendQualityField_3536682170(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			EQuality quality = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendQualityField_3536682170(fieldIndex, quality);
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x0008B3F0 File Offset: 0x000895F0
		private void RpcWriter___Observers_ReceiveQualityField_3536682170(int fieldIndex, EQuality value)
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
			writer.WriteInt32(fieldIndex, AutoPackType.Packed);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value);
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x0008B4B8 File Offset: 0x000896B8
		private void RpcLogic___ReceiveQualityField_3536682170(int fieldIndex, EQuality value)
		{
			(this.Configuration.Fields[fieldIndex] as QualityField).SetValue(value, false);
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x0008B4D8 File Offset: 0x000896D8
		private void RpcReader___Observers_ReceiveQualityField_3536682170(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			EQuality value = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveQualityField_3536682170(fieldIndex, value);
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x0008A229 File Offset: 0x00088429
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040019B7 RID: 6583
		public EntityConfiguration Configuration;

		// Token: 0x040019B8 RID: 6584
		private bool dll_Excuted;

		// Token: 0x040019B9 RID: 6585
		private bool dll_Excuted;
	}
}
