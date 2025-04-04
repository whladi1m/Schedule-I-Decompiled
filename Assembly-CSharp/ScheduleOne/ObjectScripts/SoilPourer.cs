using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BC5 RID: 3013
	public class SoilPourer : GridItem
	{
		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x0600549A RID: 21658 RVA: 0x00164416 File Offset: 0x00162616
		// (set) Token: 0x0600549B RID: 21659 RVA: 0x0016441E File Offset: 0x0016261E
		public string SoilID { get; protected set; } = string.Empty;

		// Token: 0x0600549C RID: 21660 RVA: 0x00164428 File Offset: 0x00162628
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.SoilID != string.Empty)
			{
				SoilDefinition item = Registry.GetItem<SoilDefinition>(this.SoilID);
				this.DirtPlane.material = item.DrySoilMat;
				this.SetSoilLevel(1f);
			}
		}

		// Token: 0x0600549D RID: 21661 RVA: 0x00164476 File Offset: 0x00162676
		public void HandleHovered()
		{
			if (!string.IsNullOrEmpty(this.SoilID) && !this.isDispensing)
			{
				this.HandleIntObj.SetMessage("Dispense soil");
				this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x0600549E RID: 21662 RVA: 0x001644B6 File Offset: 0x001626B6
		public void HandleInteracted()
		{
			if (!string.IsNullOrEmpty(this.SoilID) && !this.isDispensing)
			{
				this.SendPourSoil();
				this.isDispensing = true;
			}
		}

		// Token: 0x0600549F RID: 21663 RVA: 0x001644DA File Offset: 0x001626DA
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendPourSoil()
		{
			this.RpcWriter___Server_SendPourSoil_2166136261();
			this.RpcLogic___SendPourSoil_2166136261();
		}

		// Token: 0x060054A0 RID: 21664 RVA: 0x001644E8 File Offset: 0x001626E8
		[ObserversRpc(RunLocally = true)]
		private void PourSoil()
		{
			this.RpcWriter___Observers_PourSoil_2166136261();
			this.RpcLogic___PourSoil_2166136261();
		}

		// Token: 0x060054A1 RID: 21665 RVA: 0x001644F8 File Offset: 0x001626F8
		private void ApplySoil(string ID)
		{
			Pot[] array = this.GetPots().ToArray();
			if (array != null && array.Length != 0 && array[0].SoilID == string.Empty)
			{
				array[0].SetSoilID(ID);
				array[0].SetSoilState(Pot.ESoilState.Flat);
				array[0].AddSoil(array[0].SoilCapacity);
				array[0].SetSoilUses(Registry.GetItem<SoilDefinition>(ID).Uses);
				if (InstanceFinder.IsServer)
				{
					array[0].PushSoilDataToServer();
				}
			}
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x00164570 File Offset: 0x00162770
		public void FillHovered()
		{
			bool flag = false;
			if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance.Definition is SoilDefinition)
			{
				flag = true;
			}
			if (this.SoilID == string.Empty && flag)
			{
				this.FillIntObj.SetMessage("Insert soil");
				this.FillIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.FillIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x001645E8 File Offset: 0x001627E8
		public void FillInteracted()
		{
			bool flag = false;
			if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance.Definition is SoilDefinition)
			{
				flag = true;
			}
			if (this.SoilID == string.Empty && flag)
			{
				this.FillSound.Play();
				this.SendSoil(PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance.Definition.ID);
				PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ChangeQuantity(-1, false);
			}
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x00164670 File Offset: 0x00162870
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendSoil(string ID)
		{
			this.RpcWriter___Server_SendSoil_3615296227(ID);
			this.RpcLogic___SendSoil_3615296227(ID);
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x00164688 File Offset: 0x00162888
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		protected void SetSoil(NetworkConnection conn, string ID)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSoil_2971853958(conn, ID);
				this.RpcLogic___SetSoil_2971853958(conn, ID);
			}
			else
			{
				this.RpcWriter___Target_SetSoil_2971853958(conn, ID);
			}
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x001646CC File Offset: 0x001628CC
		public void SetSoilLevel(float level)
		{
			this.DirtPlane.transform.localPosition = Vector3.Lerp(this.Dirt_Min.localPosition, this.Dirt_Max.localPosition, level);
			this.DirtPlane.gameObject.SetActive(level > 0f);
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x00164720 File Offset: 0x00162920
		protected virtual List<Pot> GetPots()
		{
			List<Pot> list = new List<Pot>();
			Coordinate coord = new Coordinate(this.OriginCoordinate) + Coordinate.RotateCoordinates(new Coordinate(0, 1), (float)this.Rotation);
			Coordinate coord2 = new Coordinate(this.OriginCoordinate) + Coordinate.RotateCoordinates(new Coordinate(1, 1), (float)this.Rotation);
			Tile tile = base.OwnerGrid.GetTile(coord);
			Tile tile2 = base.OwnerGrid.GetTile(coord2);
			if (tile != null && tile2 != null)
			{
				Pot pot = null;
				foreach (GridItem gridItem in tile.BuildableOccupants)
				{
					if (gridItem is Pot)
					{
						pot = (gridItem as Pot);
						break;
					}
				}
				if (pot != null && tile2.BuildableOccupants.Contains(pot))
				{
					list.Add(pot);
				}
			}
			return list;
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x00164824 File Offset: 0x00162A24
		public override string GetSaveString()
		{
			return new SoilPourerData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, this.SoilID).GetJson(true);
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x00164874 File Offset: 0x00162A74
		[CompilerGenerated]
		private IEnumerator <PourSoil>g__PourRoutine|20_0()
		{
			SoilDefinition item = Registry.GetItem<SoilDefinition>(this.SoilID);
			if (item == null)
			{
				Console.LogError("Soil definition not found for ID: " + this.SoilID, null);
				this.isDispensing = false;
				yield break;
			}
			this.ActivateSound.Play();
			this.PourParticles.startColor = item.ParticleColor;
			this.PourParticles.Play();
			this.PourAnimation.Play();
			this.DirtPourSound.Play();
			Pot targetPot = this.GetPots().FirstOrDefault<Pot>();
			if (targetPot != null)
			{
				targetPot.SetSoilID(this.SoilID);
				targetPot.SetSoilState(Pot.ESoilState.Flat);
				targetPot.SetSoilUses(item.Uses);
			}
			for (float i = 0f; i < this.AnimationDuration; i += Time.deltaTime)
			{
				float num = i / this.AnimationDuration;
				this.SetSoilLevel(1f - num);
				if (targetPot != null)
				{
					targetPot.AddSoil(targetPot.SoilCapacity * (Time.deltaTime / this.AnimationDuration));
				}
				yield return new WaitForEndOfFrame();
			}
			if (targetPot != null)
			{
				targetPot.AddSoil(targetPot.SoilCapacity - targetPot.SoilLevel);
			}
			this.ApplySoil(this.SoilID);
			this.SetSoil(null, string.Empty);
			this.PourParticles.Stop();
			this.isDispensing = false;
			yield return new WaitForSeconds(1f);
			this.DirtPourSound.Stop();
			yield break;
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x00164884 File Offset: 0x00162A84
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.SoilPourerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.SoilPourerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendPourSoil_2166136261));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_PourSoil_2166136261));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SendSoil_3615296227));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_SetSoil_2971853958));
			base.RegisterTargetRpc(12U, new ClientRpcDelegate(this.RpcReader___Target_SetSoil_2971853958));
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x0016491B File Offset: 0x00162B1B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.SoilPourerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.SoilPourerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x00164934 File Offset: 0x00162B34
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x00164944 File Offset: 0x00162B44
		private void RpcWriter___Server_SendPourSoil_2166136261()
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
			base.SendServerRpc(8U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x001649DE File Offset: 0x00162BDE
		private void RpcLogic___SendPourSoil_2166136261()
		{
			this.PourSoil();
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x001649E8 File Offset: 0x00162BE8
		private void RpcReader___Server_SendPourSoil_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPourSoil_2166136261();
		}

		// Token: 0x060054B1 RID: 21681 RVA: 0x00164A18 File Offset: 0x00162C18
		private void RpcWriter___Observers_PourSoil_2166136261()
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
			base.SendObserversRpc(9U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060054B2 RID: 21682 RVA: 0x00164AC1 File Offset: 0x00162CC1
		private void RpcLogic___PourSoil_2166136261()
		{
			if (this.isDispensing)
			{
				return;
			}
			this.isDispensing = true;
			base.StartCoroutine(this.<PourSoil>g__PourRoutine|20_0());
		}

		// Token: 0x060054B3 RID: 21683 RVA: 0x00164AE0 File Offset: 0x00162CE0
		private void RpcReader___Observers_PourSoil_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PourSoil_2166136261();
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x00164B0C File Offset: 0x00162D0C
		private void RpcWriter___Server_SendSoil_3615296227(string ID)
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
			writer.WriteString(ID);
			base.SendServerRpc(10U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x00164BB3 File Offset: 0x00162DB3
		public void RpcLogic___SendSoil_3615296227(string ID)
		{
			this.SetSoil(null, ID);
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x00164BC0 File Offset: 0x00162DC0
		private void RpcReader___Server_SendSoil_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendSoil_3615296227(id);
		}

		// Token: 0x060054B7 RID: 21687 RVA: 0x00164C00 File Offset: 0x00162E00
		private void RpcWriter___Observers_SetSoil_2971853958(NetworkConnection conn, string ID)
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
			writer.WriteString(ID);
			base.SendObserversRpc(11U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060054B8 RID: 21688 RVA: 0x00164CB8 File Offset: 0x00162EB8
		protected void RpcLogic___SetSoil_2971853958(NetworkConnection conn, string ID)
		{
			this.SoilID = ID;
			if (ID != string.Empty)
			{
				SoilDefinition item = Registry.GetItem<SoilDefinition>(this.SoilID);
				this.DirtPlane.material = item.DrySoilMat;
				this.SetSoilLevel(1f);
			}
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x00164D04 File Offset: 0x00162F04
		private void RpcReader___Observers_SetSoil_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSoil_2971853958(null, id);
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x00164D40 File Offset: 0x00162F40
		private void RpcWriter___Target_SetSoil_2971853958(NetworkConnection conn, string ID)
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
			writer.WriteString(ID);
			base.SendTargetRpc(12U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x00164DF8 File Offset: 0x00162FF8
		private void RpcReader___Target_SetSoil_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetSoil_2971853958(base.LocalConnection, id);
		}

		// Token: 0x060054BC RID: 21692 RVA: 0x00164E2F File Offset: 0x0016302F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003EBE RID: 16062
		public float AnimationDuration = 8f;

		// Token: 0x04003EBF RID: 16063
		[Header("References")]
		public InteractableObject HandleIntObj;

		// Token: 0x04003EC0 RID: 16064
		public InteractableObject FillIntObj;

		// Token: 0x04003EC1 RID: 16065
		public MeshRenderer DirtPlane;

		// Token: 0x04003EC2 RID: 16066
		public Transform Dirt_Min;

		// Token: 0x04003EC3 RID: 16067
		public Transform Dirt_Max;

		// Token: 0x04003EC4 RID: 16068
		public ParticleSystem PourParticles;

		// Token: 0x04003EC5 RID: 16069
		public Animation PourAnimation;

		// Token: 0x04003EC6 RID: 16070
		public AudioSourceController FillSound;

		// Token: 0x04003EC7 RID: 16071
		public AudioSourceController ActivateSound;

		// Token: 0x04003EC8 RID: 16072
		public AudioSourceController DirtPourSound;

		// Token: 0x04003EC9 RID: 16073
		private bool isDispensing;

		// Token: 0x04003ECA RID: 16074
		private bool dll_Excuted;

		// Token: 0x04003ECB RID: 16075
		private bool dll_Excuted;
	}
}
