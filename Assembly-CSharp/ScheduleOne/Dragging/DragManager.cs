using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.Dragging
{
	// Token: 0x02000672 RID: 1650
	public class DragManager : NetworkSingleton<DragManager>
	{
		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002DAB RID: 11691 RVA: 0x000BF6D9 File Offset: 0x000BD8D9
		// (set) Token: 0x06002DAC RID: 11692 RVA: 0x000BF6E1 File Offset: 0x000BD8E1
		public Draggable CurrentDraggable { get; protected set; }

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002DAD RID: 11693 RVA: 0x000BF6EA File Offset: 0x000BD8EA
		public bool IsDragging
		{
			get
			{
				return this.CurrentDraggable != null;
			}
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000BF6F8 File Offset: 0x000BD8F8
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			foreach (Draggable draggable in this.AllDraggables)
			{
				if (draggable.InitialReplicationMode != Draggable.EInitialReplicationMode.Off && (draggable.InitialReplicationMode == Draggable.EInitialReplicationMode.Full || Vector3.Distance(draggable.initialPosition, draggable.transform.position) > 1f))
				{
					this.SetDraggableTransformData(connection, draggable.GUID.ToString(), draggable.transform.position, draggable.transform.rotation, draggable.Rigidbody.velocity);
				}
			}
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000BF7B8 File Offset: 0x000BD9B8
		public void Update()
		{
			if (this.IsDragging)
			{
				bool flag = false;
				LayerMask mask = default(LayerMask);
				mask.value = 1 << LayerMask.NameToLayer("Default");
				mask.value |= 1 << LayerMask.NameToLayer("NPC");
				RaycastHit raycastHit;
				if (Physics.Raycast(PlayerSingleton<PlayerMovement>.Instance.transform.position - PlayerSingleton<PlayerMovement>.Instance.Controller.height * Vector3.up * 0.5f, Vector3.down, out raycastHit, 0.5f, mask))
				{
					flag = (raycastHit.collider.GetComponentInParent<Draggable>() == this.CurrentDraggable);
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact) || !this.IsDraggingAllowed() || Vector3.Distance(this.GetTargetPosition(), this.CurrentDraggable.transform.position) > 1.5f || flag)
				{
					this.StopDragging(this.CurrentDraggable.Rigidbody.velocity);
					return;
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
				{
					Vector3 a = PlayerSingleton<PlayerCamera>.Instance.transform.forward * this.ThrowForce;
					float d = Mathf.Lerp(1f, Mathf.Sqrt(this.CurrentDraggable.Rigidbody.mass), this.MassInfluence);
					Vector3 velocity = this.CurrentDraggable.Rigidbody.velocity + a / d;
					this.CurrentDraggable.Rigidbody.velocity = velocity;
					this.lastThrownDraggable = this.CurrentDraggable;
					this.ThrowSound.transform.position = this.lastThrownDraggable.transform.position;
					float value = Mathf.Sqrt(this.CurrentDraggable.Rigidbody.mass / 30f);
					this.ThrowSound.VolumeMultiplier = Mathf.Clamp(value, 0.4f, 1f);
					this.ThrowSound.PitchMultiplier = Mathf.Lerp(0.6f, 0.4f, Mathf.Clamp01(value));
					this.ThrowSound.Play();
					this.StopDragging(velocity);
				}
			}
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000BF9E0 File Offset: 0x000BDBE0
		public void FixedUpdate()
		{
			if (this.lastThrownDraggable != null)
			{
				this.ThrowSound.transform.position = this.lastThrownDraggable.transform.position;
			}
			if (this.IsDragging)
			{
				this.CurrentDraggable.ApplyDragForces(this.GetTargetPosition());
			}
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000BFA34 File Offset: 0x000BDC34
		public bool IsDraggingAllowed()
		{
			if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount > 0)
			{
				return false;
			}
			if (!Player.Local.Health.IsAlive)
			{
				return false;
			}
			if (Player.Local.IsSkating)
			{
				return false;
			}
			if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped)
			{
				if (PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance.ID == "trashgrabber")
				{
					return false;
				}
				if (PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance.ID == "trashbag" && TrashBag_Equippable.IsHoveringTrash)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000BFAC9 File Offset: 0x000BDCC9
		public void RegisterDraggable(Draggable draggable)
		{
			if (this.AllDraggables.Contains(draggable))
			{
				return;
			}
			this.AllDraggables.Add(draggable);
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x000BFAE6 File Offset: 0x000BDCE6
		public void Deregister(Draggable draggable)
		{
			if (this.AllDraggables.Contains(draggable))
			{
				this.AllDraggables.Remove(draggable);
			}
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x000BFB04 File Offset: 0x000BDD04
		public void StartDragging(Draggable draggable)
		{
			if (this.CurrentDraggable != null)
			{
				this.CurrentDraggable.StopDragging();
			}
			this.CurrentDraggable = draggable;
			this.lastHeldDraggable = draggable;
			draggable.StartDragging(Player.Local);
			this.SendDragger(draggable.GUID.ToString(), Player.Local.NetworkObject, draggable.transform.position);
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x000BFB72 File Offset: 0x000BDD72
		[ServerRpc(RequireOwnership = false)]
		private void SendDragger(string draggableGUID, NetworkObject dragger, Vector3 position)
		{
			this.RpcWriter___Server_SendDragger_807933219(draggableGUID, dragger, position);
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000BFB88 File Offset: 0x000BDD88
		[ObserversRpc]
		private void SetDragger(string draggableGUID, NetworkObject dragger, Vector3 position)
		{
			this.RpcWriter___Observers_SetDragger_807933219(draggableGUID, dragger, position);
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x000BFBA8 File Offset: 0x000BDDA8
		public void StopDragging(Vector3 velocity)
		{
			if (this.CurrentDraggable != null)
			{
				this.CurrentDraggable.StopDragging();
				this.SendDragger(this.CurrentDraggable.GUID.ToString(), null, this.CurrentDraggable.transform.position);
				this.SendDraggableTransformData(this.CurrentDraggable.GUID.ToString(), this.CurrentDraggable.transform.position, this.CurrentDraggable.transform.rotation, velocity);
				this.CurrentDraggable = null;
			}
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x000BFC48 File Offset: 0x000BDE48
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendDraggableTransformData(string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
		{
			this.RpcWriter___Server_SendDraggableTransformData_4062762274(guid, position, rotation, velocity);
			this.RpcLogic___SendDraggableTransformData_4062762274(guid, position, rotation, velocity);
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000BFC78 File Offset: 0x000BDE78
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetDraggableTransformData(NetworkConnection conn, string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetDraggableTransformData_3831223955(conn, guid, position, rotation, velocity);
				this.RpcLogic___SetDraggableTransformData_3831223955(conn, guid, position, rotation, velocity);
			}
			else
			{
				this.RpcWriter___Target_SetDraggableTransformData_3831223955(conn, guid, position, rotation, velocity);
			}
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x000BFCDD File Offset: 0x000BDEDD
		private Vector3 GetTargetPosition()
		{
			return PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * 1.25f * this.CurrentDraggable.HoldDistanceMultiplier;
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x000BFD7C File Offset: 0x000BDF7C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Dragging.DragManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Dragging.DragManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendDragger_807933219));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetDragger_807933219));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendDraggableTransformData_4062762274));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetDraggableTransformData_3831223955));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetDraggableTransformData_3831223955));
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x000BFE13 File Offset: 0x000BE013
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Dragging.DragManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Dragging.DragManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x000BFE2C File Offset: 0x000BE02C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000BFE3C File Offset: 0x000BE03C
		private void RpcWriter___Server_SendDragger_807933219(string draggableGUID, NetworkObject dragger, Vector3 position)
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
			writer.WriteString(draggableGUID);
			writer.WriteNetworkObject(dragger);
			writer.WriteVector3(position);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000BFEFD File Offset: 0x000BE0FD
		private void RpcLogic___SendDragger_807933219(string draggableGUID, NetworkObject dragger, Vector3 position)
		{
			this.SetDragger(draggableGUID, dragger, position);
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x000BFF08 File Offset: 0x000BE108
		private void RpcReader___Server_SendDragger_807933219(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string draggableGUID = PooledReader0.ReadString();
			NetworkObject dragger = PooledReader0.ReadNetworkObject();
			Vector3 position = PooledReader0.ReadVector3();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendDragger_807933219(draggableGUID, dragger, position);
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000BFF5C File Offset: 0x000BE15C
		private void RpcWriter___Observers_SetDragger_807933219(string draggableGUID, NetworkObject dragger, Vector3 position)
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
			writer.WriteString(draggableGUID);
			writer.WriteNetworkObject(dragger);
			writer.WriteVector3(position);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000C002C File Offset: 0x000BE22C
		private void RpcLogic___SetDragger_807933219(string draggableGUID, NetworkObject dragger, Vector3 position)
		{
			Draggable @object = GUIDManager.GetObject<Draggable>(new Guid(draggableGUID));
			Player x = (dragger != null) ? dragger.GetComponent<Player>() : null;
			if (@object != null)
			{
				if (this.CurrentDraggable != @object && this.lastHeldDraggable != @object)
				{
					@object.Rigidbody.position = position;
				}
				if (dragger != null)
				{
					if (x != null)
					{
						@object.StartDragging(dragger.GetComponent<Player>());
						return;
					}
				}
				else
				{
					@object.StopDragging();
				}
			}
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000C00B0 File Offset: 0x000BE2B0
		private void RpcReader___Observers_SetDragger_807933219(PooledReader PooledReader0, Channel channel)
		{
			string draggableGUID = PooledReader0.ReadString();
			NetworkObject dragger = PooledReader0.ReadNetworkObject();
			Vector3 position = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetDragger_807933219(draggableGUID, dragger, position);
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x000C0104 File Offset: 0x000BE304
		private void RpcWriter___Server_SendDraggableTransformData_4062762274(string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
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
			writer.WriteString(guid);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteVector3(velocity);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000C01D7 File Offset: 0x000BE3D7
		private void RpcLogic___SendDraggableTransformData_4062762274(string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
		{
			this.SetDraggableTransformData(null, guid, position, rotation, velocity);
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000C01E8 File Offset: 0x000BE3E8
		private void RpcReader___Server_SendDraggableTransformData_4062762274(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string guid = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			Vector3 velocity = PooledReader0.ReadVector3();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendDraggableTransformData_4062762274(guid, position, rotation, velocity);
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000C0260 File Offset: 0x000BE460
		private void RpcWriter___Observers_SetDraggableTransformData_3831223955(NetworkConnection conn, string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
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
			writer.WriteString(guid);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteVector3(velocity);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x000C0344 File Offset: 0x000BE544
		private void RpcLogic___SetDraggableTransformData_3831223955(NetworkConnection conn, string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
		{
			Draggable @object = GUIDManager.GetObject<Draggable>(new Guid(guid));
			if (@object == null)
			{
				Console.LogWarning("Failed to find draggable with GUID " + guid, null);
			}
			if (@object == this.lastThrownDraggable)
			{
				return;
			}
			if (@object == this.lastHeldDraggable)
			{
				return;
			}
			if (@object != null)
			{
				@object.Rigidbody.position = position;
				@object.Rigidbody.rotation = rotation;
				@object.Rigidbody.velocity = velocity;
			}
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x000C03C4 File Offset: 0x000BE5C4
		private void RpcReader___Observers_SetDraggableTransformData_3831223955(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			Vector3 velocity = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetDraggableTransformData_3831223955(null, guid, position, rotation, velocity);
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000C0438 File Offset: 0x000BE638
		private void RpcWriter___Target_SetDraggableTransformData_3831223955(NetworkConnection conn, string guid, Vector3 position, Quaternion rotation, Vector3 velocity)
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
			writer.WriteString(guid);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteVector3(velocity);
			base.SendTargetRpc(4U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000C051C File Offset: 0x000BE71C
		private void RpcReader___Target_SetDraggableTransformData_3831223955(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			Vector3 velocity = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetDraggableTransformData_3831223955(base.LocalConnection, guid, position, rotation, velocity);
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000C058B File Offset: 0x000BE78B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400208E RID: 8334
		public const float DRAGGABLE_OFFSET = 1.25f;

		// Token: 0x0400208F RID: 8335
		public AudioSourceController ThrowSound;

		// Token: 0x04002090 RID: 8336
		[Header("Settings")]
		public float DragForce = 10f;

		// Token: 0x04002091 RID: 8337
		public float DampingFactor = 0.5f;

		// Token: 0x04002092 RID: 8338
		public float TorqueForce = 10f;

		// Token: 0x04002093 RID: 8339
		public float TorqueDampingFactor = 0.5f;

		// Token: 0x04002094 RID: 8340
		public float ThrowForce = 10f;

		// Token: 0x04002095 RID: 8341
		public float MassInfluence = 0.6f;

		// Token: 0x04002097 RID: 8343
		private List<Draggable> AllDraggables = new List<Draggable>();

		// Token: 0x04002098 RID: 8344
		private Draggable lastThrownDraggable;

		// Token: 0x04002099 RID: 8345
		private Draggable lastHeldDraggable;

		// Token: 0x0400209A RID: 8346
		private bool dll_Excuted;

		// Token: 0x0400209B RID: 8347
		private bool dll_Excuted;
	}
}
