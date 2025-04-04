using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.WorldspacePopup;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Vision
{
	// Token: 0x0200027C RID: 636
	public class VisionCone : NetworkBehaviour
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x0003AC1C File Offset: 0x00038E1C
		protected float effectiveRange
		{
			get
			{
				return this.Range * this.RangeMultiplier;
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0003AC2C File Offset: 0x00038E2C
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Vision.VisionCone_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0003AC4C File Offset: 0x00038E4C
		private void PlayerSpawned(Player plr)
		{
			Dictionary<PlayerVisualState.EVisualState, VisionCone.StateContainer> dictionary = new Dictionary<PlayerVisualState.EVisualState, VisionCone.StateContainer>();
			for (int i = 0; i < this.StatesOfInterest.Count; i++)
			{
				dictionary.Add(this.StatesOfInterest[i].state, this.StatesOfInterest[i]);
			}
			this.StateSettings.Add(plr, dictionary);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0003ACA5 File Offset: 0x00038EA5
		private void OnDisable()
		{
			while (this.activeVisionEvents.Count > 0)
			{
				this.activeVisionEvents[0].EndEvent();
			}
			this.playerSightDatas.Clear();
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0003ACD3 File Offset: 0x00038ED3
		protected virtual void Update()
		{
			if (this.DEBUG_FRUSTRUM)
			{
				this.GetFrustumVertices();
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0003ACE4 File Offset: 0x00038EE4
		protected virtual void FixedUpdate()
		{
			if (!this.VisionEnabled)
			{
				foreach (VisionEvent visionEvent in this.activeVisionEvents)
				{
					visionEvent.EndEvent();
				}
				return;
			}
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0003AD40 File Offset: 0x00038F40
		protected virtual void VisionUpdate()
		{
			if (!base.enabled || !this.VisionEnabled)
			{
				return;
			}
			this.UpdateVision(0.1f);
			this.UpdateEvents(0.1f);
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0003AD6C File Offset: 0x00038F6C
		protected virtual void UpdateEvents(float tickTime)
		{
			foreach (Player player in this.playerSightDatas.Keys)
			{
				if (!(player != Player.Local) && player.Health.IsAlive && !player.IsArrested)
				{
					foreach (PlayerVisualState.VisualState visualState in player.VisualState.visualStates)
					{
						if (this.StateSettings[player].ContainsKey(visualState.state) && this.StateSettings[player][visualState.state].Enabled)
						{
							VisionCone.StateContainer stateContainer = this.StateSettings[player][visualState.state];
							if (this.GetEvent(player, visualState) == null)
							{
								VisionEvent visionEvent = new VisionEvent(this, player, visualState, stateContainer.RequiredNoticeTime);
								visionEvent.UpdateEvent(this.playerSightDatas[player].VisionDelta, tickTime);
								this.activeVisionEvents.Add(visionEvent);
								if (this.onVisionEventStarted != null)
								{
									VisionEventReceipt @event = new VisionEventReceipt(player.NetworkObject, visualState.state);
									this.onVisionEventStarted(@event);
								}
							}
						}
					}
				}
			}
			List<VisionEvent> list = new List<VisionEvent>();
			list.AddRange(this.activeVisionEvents);
			foreach (VisionEvent visionEvent2 in list)
			{
				if (!this.StateSettings[visionEvent2.Target].ContainsKey(visionEvent2.State.state) || !this.StateSettings[visionEvent2.Target][visionEvent2.State.state].Enabled)
				{
					visionEvent2.EndEvent();
				}
			}
			List<VisionEvent> list2 = this.activeVisionEvents.FindAll((VisionEvent x) => x.Target == Player.Local);
			float num = 0f;
			for (int i = 0; i < list2.Count; i++)
			{
				if (this.playerSightDatas.ContainsKey(Player.Local))
				{
					list2[i].UpdateEvent(this.playerSightDatas[Player.Local].VisionDelta, tickTime);
				}
				else
				{
					list2[i].UpdateEvent(0f, tickTime);
				}
				if (list2[i].NormalizedNoticeLevel > num)
				{
					num = list2[i].NormalizedNoticeLevel;
				}
			}
			if (num > 0f && this.WorldspaceIconsEnabled)
			{
				this.QuestionMarkPopup.enabled = true;
				this.QuestionMarkPopup.CurrentFillLevel = num;
				return;
			}
			this.QuestionMarkPopup.enabled = false;
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0003B0A0 File Offset: 0x000392A0
		protected virtual void UpdateVision(float tickTime)
		{
			if (this.npc != null && !this.npc.IsConscious)
			{
				return;
			}
			this.sightedPlayers = new List<Player>();
			if (!this.DisableSightUpdates)
			{
				for (int i = 0; i < Player.PlayerList.Count; i++)
				{
					Player player = Player.PlayerList[i];
					if (this.IsPointWithinSight(player.Avatar.CenterPoint, true, null))
					{
						float num = player.Visibility.CalculateExposureToPoint(this.VisionOrigin.position, this.effectiveRange, this.npc);
						if (player.CurrentVehicle != null && this.IsPointWithinSight(player.CurrentVehicle.transform.position, false, player.CurrentVehicle.GetComponent<LandVehicle>()))
						{
							num = 1f;
						}
						if (num > 0f)
						{
							float num2 = num * this.VisionFalloff.Evaluate(Mathf.Clamp01(Vector3.Distance(this.VisionOrigin.position, player.Avatar.CenterPoint) / this.effectiveRange)) * player.Visibility.CurrentVisibility / 100f;
							if (num2 > this.MinVisionDelta)
							{
								this.sightedPlayers.Add(player);
								VisionCone.PlayerSightData playerSightData;
								if (this.IsPlayerVisible(player, out playerSightData))
								{
									playerSightData.TimeVisible += tickTime;
									playerSightData.VisionDelta = num2;
								}
								else
								{
									playerSightData = new VisionCone.PlayerSightData();
									playerSightData.Player = player;
									playerSightData.TimeVisible = 0f;
									playerSightData.VisionDelta = num2;
									this.playerSightDatas.Add(player, playerSightData);
								}
							}
						}
					}
				}
			}
			foreach (Player player2 in new List<Player>(this.playerSightDatas.Keys))
			{
				if (!this.sightedPlayers.Contains(player2))
				{
					this.playerSightDatas.Remove(player2);
				}
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0003B2A0 File Offset: 0x000394A0
		public virtual void EventReachedZero(VisionEvent _event)
		{
			this.activeVisionEvents.Remove(_event);
			VisionEventReceipt receipt = new VisionEventReceipt(_event.Target.NetworkObject, _event.State.state);
			this.SendEventReceipt(receipt, VisionCone.EEventLevel.Zero);
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0003B2E0 File Offset: 0x000394E0
		public virtual void EventHalfNoticed(VisionEvent _event)
		{
			VisionEventReceipt receipt = new VisionEventReceipt(_event.Target.NetworkObject, _event.State.state);
			this.SendEventReceipt(receipt, VisionCone.EEventLevel.Half);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0003B314 File Offset: 0x00039514
		public virtual void EventFullyNoticed(VisionEvent _event)
		{
			this.activeVisionEvents.Remove(_event);
			if (this.WorldspaceIconsEnabled && _event.Target.Owner.IsLocalClient)
			{
				this.ExclamationPointPopup.Popup();
				this.ExclamationSound.Play();
			}
			VisionEventReceipt receipt = new VisionEventReceipt(_event.Target.NetworkObject, _event.State.state);
			this.SendEventReceipt(receipt, VisionCone.EEventLevel.Full);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0003B382 File Offset: 0x00039582
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SendEventReceipt(VisionEventReceipt receipt, VisionCone.EEventLevel level)
		{
			this.RpcWriter___Server_SendEventReceipt_3486014028(receipt, level);
			this.RpcLogic___SendEventReceipt_3486014028(receipt, level);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0003B3A0 File Offset: 0x000395A0
		[ObserversRpc(RunLocally = true, ExcludeOwner = true)]
		public virtual void ReceiveEventReceipt(VisionEventReceipt receipt, VisionCone.EEventLevel level)
		{
			this.RpcWriter___Observers_ReceiveEventReceipt_3486014028(receipt, level);
			this.RpcLogic___ReceiveEventReceipt_3486014028(receipt, level);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0003B3CC File Offset: 0x000395CC
		public virtual bool IsPointWithinSight(Vector3 point, bool ignoreLoS = false, LandVehicle vehicleToIgnore = null)
		{
			if (Vector3.Distance(point, this.VisionOrigin.position) > this.effectiveRange)
			{
				return false;
			}
			if (Vector3.SignedAngle(this.VisionOrigin.forward, (point - this.VisionOrigin.position).normalized, this.VisionOrigin.up) > 90f)
			{
				return false;
			}
			if (Vector3.SignedAngle(this.VisionOrigin.forward, (point - this.VisionOrigin.position).normalized, this.VisionOrigin.right) > 90f)
			{
				return false;
			}
			Plane[] frustumPlanes = this.GetFrustumPlanes();
			for (int i = 0; i < 6; i++)
			{
				if (frustumPlanes[i].GetDistanceToPoint(point) > 0f)
				{
					return false;
				}
			}
			RaycastHit raycastHit;
			return ignoreLoS || !Physics.Raycast(this.VisionOrigin.position, point - this.VisionOrigin.position, out raycastHit, Vector3.Distance(point, this.VisionOrigin.position), this.VisibilityBlockingLayers) || (vehicleToIgnore != null && raycastHit.collider.GetComponentInParent<LandVehicle>() == vehicleToIgnore);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0003B500 File Offset: 0x00039700
		public VisionEvent GetEvent(Player target, PlayerVisualState.VisualState state)
		{
			return this.activeVisionEvents.Find((VisionEvent x) => x.Target == target && x.State == state);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0003B538 File Offset: 0x00039738
		public bool IsPlayerVisible(Player player)
		{
			return this.playerSightDatas.ContainsKey(player) && this.playerSightDatas[player].VisionDelta > this.MinVisionDelta;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0003B563 File Offset: 0x00039763
		public float GetPlayerVisibility(Player player)
		{
			if (this.playerSightDatas.ContainsKey(player))
			{
				return this.playerSightDatas[player].VisionDelta;
			}
			return 0f;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0003B58A File Offset: 0x0003978A
		public bool IsPlayerVisible(Player player, out VisionCone.PlayerSightData data)
		{
			if (this.playerSightDatas.ContainsKey(player))
			{
				data = this.playerSightDatas[player];
				return true;
			}
			data = null;
			return false;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0003B5AE File Offset: 0x000397AE
		public virtual void SetGeneralCrimeResponseActive(Player player, bool active)
		{
			if (this.generalCrimeResponseActive == active)
			{
				return;
			}
			this.StateSettings[player][PlayerVisualState.EVisualState.PettyCrime].Enabled = active;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0003B5D2 File Offset: 0x000397D2
		private void OnDie()
		{
			this.ClearEvents();
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0003B5DC File Offset: 0x000397DC
		public void ClearEvents()
		{
			this.ExclamationPointPopup.enabled = false;
			this.QuestionMarkPopup.enabled = false;
			VisionEvent[] array = this.activeVisionEvents.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].EndEvent();
			}
			this.activeVisionEvents.Clear();
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0003B630 File Offset: 0x00039830
		private Vector3[] GetFrustumVertices()
		{
			Vector3 position = this.VisionOrigin.position;
			Quaternion rotation = this.VisionOrigin.rotation;
			float z = 0f;
			float effectiveRange = this.effectiveRange;
			float minorWidth = this.MinorWidth;
			float minorHeight = this.MinorHeight;
			float num = minorWidth + 2f * this.effectiveRange * Mathf.Tan(this.HorizontalFOV * 0.017453292f / 2f);
			float num2 = minorHeight + 2f * this.effectiveRange * Mathf.Tan(this.VerticalFOV * 0.017453292f / 2f);
			Vector3[] array = new Vector3[8];
			Vector3 vector = position + rotation * new Vector3(-minorWidth / 2f, minorHeight / 2f, z);
			Vector3 vector2 = position + rotation * new Vector3(minorWidth / 2f, minorHeight / 2f, z);
			Vector3 vector3 = position + rotation * new Vector3(-minorWidth / 2f, -minorHeight / 2f, z);
			Vector3 vector4 = position + rotation * new Vector3(minorWidth / 2f, -minorHeight / 2f, z);
			Vector3 vector5 = position + rotation * new Vector3(-num / 2f, num2 / 2f, effectiveRange);
			Vector3 vector6 = position + rotation * new Vector3(num / 2f, num2 / 2f, effectiveRange);
			Vector3 vector7 = position + rotation * new Vector3(-num / 2f, -num2 / 2f, effectiveRange);
			Vector3 vector8 = position + rotation * new Vector3(num / 2f, -num2 / 2f, effectiveRange);
			array[0] = vector;
			array[1] = vector2;
			array[2] = vector3;
			array[3] = vector4;
			array[4] = vector5;
			array[5] = vector6;
			array[6] = vector7;
			array[7] = vector8;
			Debug.DrawLine(vector, vector5, Color.red);
			Debug.DrawLine(vector2, vector6, Color.green);
			Debug.DrawLine(vector3, vector7, Color.blue);
			Debug.DrawLine(vector4, vector8, Color.magenta);
			return array;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0003B878 File Offset: 0x00039A78
		private Plane[] GetFrustumPlanes()
		{
			Vector3 position = this.VisionOrigin.position;
			Quaternion rotation = this.VisionOrigin.rotation;
			float z = 0f;
			float effectiveRange = this.effectiveRange;
			float minorWidth = this.MinorWidth;
			float minorHeight = this.MinorHeight;
			float num = minorWidth + 2f * this.effectiveRange * Mathf.Tan(this.HorizontalFOV * 0.017453292f / 2f);
			float num2 = minorHeight + 2f * this.effectiveRange * Mathf.Tan(this.VerticalFOV * 0.017453292f / 2f);
			Plane[] array = new Plane[6];
			Vector3 vector = position + rotation * new Vector3(-minorWidth / 2f, minorHeight / 2f, z);
			Vector3 vector2 = position + rotation * new Vector3(minorWidth / 2f, minorHeight / 2f, z);
			Vector3 vector3 = position + rotation * new Vector3(-minorWidth / 2f, -minorHeight / 2f, z);
			Vector3 vector4 = position + rotation * new Vector3(minorWidth / 2f, -minorHeight / 2f, z);
			Vector3 vector5 = position + rotation * new Vector3(-num / 2f, num2 / 2f, effectiveRange);
			Vector3 vector6 = position + rotation * new Vector3(num / 2f, num2 / 2f, effectiveRange);
			Vector3 c = position + rotation * new Vector3(-num / 2f, -num2 / 2f, effectiveRange);
			Vector3 c2 = position + rotation * new Vector3(num / 2f, -num2 / 2f, effectiveRange);
			array[0] = new Plane(vector2, vector, vector5);
			array[1] = new Plane(vector3, vector4, c2);
			array[2] = new Plane(vector, vector3, c);
			array[3] = new Plane(vector4, vector2, vector6);
			array[4] = new Plane(vector, vector2, vector4);
			array[5] = new Plane(vector6, vector5, c);
			return array;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0003BB7C File Offset: 0x00039D7C
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vision.VisionConeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vision.VisionConeAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendEventReceipt_3486014028));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveEventReceipt_3486014028));
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0003BBC8 File Offset: 0x00039DC8
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vision.VisionConeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vision.VisionConeAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0003BBDB File Offset: 0x00039DDB
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0003BBEC File Offset: 0x00039DEC
		private void RpcWriter___Server_SendEventReceipt_3486014028(VisionEventReceipt receipt, VisionCone.EEventLevel level)
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
			writer.Write___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generated(receipt);
			writer.Write___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generated(level);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0003BCA0 File Offset: 0x00039EA0
		public void RpcLogic___SendEventReceipt_3486014028(VisionEventReceipt receipt, VisionCone.EEventLevel level)
		{
			this.ReceiveEventReceipt(receipt, level);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0003BCAC File Offset: 0x00039EAC
		private void RpcReader___Server_SendEventReceipt_3486014028(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			VisionEventReceipt receipt = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generateds(PooledReader0);
			VisionCone.EEventLevel level = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendEventReceipt_3486014028(receipt, level);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0003BCFC File Offset: 0x00039EFC
		private void RpcWriter___Observers_ReceiveEventReceipt_3486014028(VisionEventReceipt receipt, VisionCone.EEventLevel level)
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
			writer.Write___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generated(receipt);
			writer.Write___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generated(level);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, true);
			writer.Store();
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0003BDC0 File Offset: 0x00039FC0
		public virtual void RpcLogic___ReceiveEventReceipt_3486014028(VisionEventReceipt receipt, VisionCone.EEventLevel level)
		{
			switch (level)
			{
			case VisionCone.EEventLevel.Start:
				if (this.onVisionEventStarted != null)
				{
					this.onVisionEventStarted(receipt);
					return;
				}
				break;
			case VisionCone.EEventLevel.Half:
				if (this.onVisionEventHalf != null)
				{
					this.onVisionEventHalf(receipt);
					return;
				}
				break;
			case VisionCone.EEventLevel.Full:
				if (this.onVisionEventFull != null)
				{
					this.onVisionEventFull(receipt);
					return;
				}
				break;
			case VisionCone.EEventLevel.Zero:
				if (this.onVisionEventExpired != null)
				{
					this.onVisionEventExpired(receipt);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0003BE38 File Offset: 0x0003A038
		private void RpcReader___Observers_ReceiveEventReceipt_3486014028(PooledReader PooledReader0, Channel channel)
		{
			VisionEventReceipt receipt = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionEventReceiptFishNet.Serializing.Generateds(PooledReader0);
			VisionCone.EEventLevel level = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Vision.VisionCone/EEventLevelFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveEventReceipt_3486014028(receipt, level);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0003BE84 File Offset: 0x0003A084
		protected virtual void dll()
		{
			if (this.VisionOrigin == null)
			{
				this.VisionOrigin = base.transform;
			}
			this.npc = base.GetComponentInParent<NPC>();
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				this.PlayerSpawned(Player.PlayerList[i]);
			}
			Player.onPlayerSpawned = (Action<Player>)Delegate.Combine(Player.onPlayerSpawned, new Action<Player>(this.PlayerSpawned));
			if (this.npc != null)
			{
				this.npc.Health.onDie.AddListener(new UnityAction(this.OnDie));
				this.npc.Health.onKnockedOut.AddListener(new UnityAction(this.OnDie));
			}
			base.InvokeRepeating("VisionUpdate", UnityEngine.Random.Range(0f, 0.1f), 0.1f);
		}

		// Token: 0x04000DD2 RID: 3538
		public const float VISION_UPDATE_INTERVAL = 0.1f;

		// Token: 0x04000DD3 RID: 3539
		public static float UniversalAttentivenessScale = 1f;

		// Token: 0x04000DD4 RID: 3540
		public static float UniversalMemoryScale = 1f;

		// Token: 0x04000DD5 RID: 3541
		public bool DisableSightUpdates;

		// Token: 0x04000DD6 RID: 3542
		[Header("Frustrum Settings")]
		public float HorizontalFOV = 90f;

		// Token: 0x04000DD7 RID: 3543
		public float VerticalFOV = 30f;

		// Token: 0x04000DD8 RID: 3544
		public float Range = 40f;

		// Token: 0x04000DD9 RID: 3545
		public float MinorWidth = 3f;

		// Token: 0x04000DDA RID: 3546
		public float MinorHeight = 1.5f;

		// Token: 0x04000DDB RID: 3547
		public Transform VisionOrigin;

		// Token: 0x04000DDC RID: 3548
		public bool DEBUG_FRUSTRUM;

		// Token: 0x04000DDD RID: 3549
		[Header("Vision Settings")]
		public bool VisionEnabled = true;

		// Token: 0x04000DDE RID: 3550
		public AnimationCurve VisionFalloff;

		// Token: 0x04000DDF RID: 3551
		public LayerMask VisibilityBlockingLayers;

		// Token: 0x04000DE0 RID: 3552
		[Range(0f, 2f)]
		public float RangeMultiplier = 1f;

		// Token: 0x04000DE1 RID: 3553
		[Header("Interest settings")]
		public List<VisionCone.StateContainer> StatesOfInterest = new List<VisionCone.StateContainer>();

		// Token: 0x04000DE2 RID: 3554
		[Header("Notice Settings")]
		public float MinVisionDelta = 0.1f;

		// Token: 0x04000DE3 RID: 3555
		public float Attentiveness = 1f;

		// Token: 0x04000DE4 RID: 3556
		public float Memory = 1f;

		// Token: 0x04000DE5 RID: 3557
		[Header("Worldspace Icons")]
		public bool WorldspaceIconsEnabled = true;

		// Token: 0x04000DE6 RID: 3558
		public WorldspacePopup QuestionMarkPopup;

		// Token: 0x04000DE7 RID: 3559
		public WorldspacePopup ExclamationPointPopup;

		// Token: 0x04000DE8 RID: 3560
		public AudioSourceController ExclamationSound;

		// Token: 0x04000DE9 RID: 3561
		public VisionCone.EventStateChange onVisionEventStarted;

		// Token: 0x04000DEA RID: 3562
		public VisionCone.EventStateChange onVisionEventHalf;

		// Token: 0x04000DEB RID: 3563
		public VisionCone.EventStateChange onVisionEventFull;

		// Token: 0x04000DEC RID: 3564
		public VisionCone.EventStateChange onVisionEventExpired;

		// Token: 0x04000DED RID: 3565
		public Dictionary<Player, Dictionary<PlayerVisualState.EVisualState, VisionCone.StateContainer>> StateSettings = new Dictionary<Player, Dictionary<PlayerVisualState.EVisualState, VisionCone.StateContainer>>();

		// Token: 0x04000DEE RID: 3566
		protected List<VisionEvent> activeVisionEvents = new List<VisionEvent>();

		// Token: 0x04000DEF RID: 3567
		protected Dictionary<Player, VisionCone.PlayerSightData> playerSightDatas = new Dictionary<Player, VisionCone.PlayerSightData>();

		// Token: 0x04000DF0 RID: 3568
		protected NPC npc;

		// Token: 0x04000DF1 RID: 3569
		private bool generalCrimeResponseActive;

		// Token: 0x04000DF2 RID: 3570
		private List<Player> sightedPlayers = new List<Player>();

		// Token: 0x04000DF3 RID: 3571
		private bool dll_Excuted;

		// Token: 0x04000DF4 RID: 3572
		private bool dll_Excuted;

		// Token: 0x0200027D RID: 637
		public enum EEventLevel
		{
			// Token: 0x04000DF6 RID: 3574
			Start,
			// Token: 0x04000DF7 RID: 3575
			Half,
			// Token: 0x04000DF8 RID: 3576
			Full,
			// Token: 0x04000DF9 RID: 3577
			Zero
		}

		// Token: 0x0200027E RID: 638
		[Serializable]
		public class StateContainer
		{
			// Token: 0x04000DFA RID: 3578
			public PlayerVisualState.EVisualState state;

			// Token: 0x04000DFB RID: 3579
			public bool Enabled;

			// Token: 0x04000DFC RID: 3580
			public float RequiredNoticeTime = 0.5f;
		}

		// Token: 0x0200027F RID: 639
		public class PlayerSightData
		{
			// Token: 0x04000DFD RID: 3581
			public Player Player;

			// Token: 0x04000DFE RID: 3582
			public float VisionDelta;

			// Token: 0x04000DFF RID: 3583
			public float TimeVisible;
		}

		// Token: 0x02000280 RID: 640
		// (Invoke) Token: 0x06000D63 RID: 3427
		public delegate void EventStateChange(VisionEventReceipt _event);
	}
}
