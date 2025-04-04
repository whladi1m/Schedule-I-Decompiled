using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.EntityFramework;
using ScheduleOne.Interaction;
using ScheduleOne.Tiles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BC1 RID: 3009
	public class Sprinkler : GridItem
	{
		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x06005467 RID: 21607 RVA: 0x00163BBA File Offset: 0x00161DBA
		// (set) Token: 0x06005468 RID: 21608 RVA: 0x00163BC2 File Offset: 0x00161DC2
		public bool IsSprinkling { get; private set; }

		// Token: 0x06005469 RID: 21609 RVA: 0x00163BCB File Offset: 0x00161DCB
		public void Hovered()
		{
			if (this.isGhost)
			{
				return;
			}
			if (this.CanWater())
			{
				this.IntObj.SetMessage("Activate sprinkler");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x0600546A RID: 21610 RVA: 0x00163C07 File Offset: 0x00161E07
		public void Interacted()
		{
			if (this.isGhost)
			{
				return;
			}
			if (!this.CanWater())
			{
				return;
			}
			this.SendWater();
		}

		// Token: 0x0600546B RID: 21611 RVA: 0x00163C21 File Offset: 0x00161E21
		private bool CanWater()
		{
			return !this.IsSprinkling;
		}

		// Token: 0x0600546C RID: 21612 RVA: 0x00163C2C File Offset: 0x00161E2C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendWater()
		{
			this.RpcWriter___Server_SendWater_2166136261();
			this.RpcLogic___SendWater_2166136261();
		}

		// Token: 0x0600546D RID: 21613 RVA: 0x00163C3A File Offset: 0x00161E3A
		[ObserversRpc(RunLocally = true)]
		private void Water()
		{
			this.RpcWriter___Observers_Water_2166136261();
			this.RpcLogic___Water_2166136261();
		}

		// Token: 0x0600546E RID: 21614 RVA: 0x00163C48 File Offset: 0x00161E48
		public void ApplyWater(float normalizedAmount)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			List<Pot> pots = this.GetPots();
			for (int i = 0; i < pots.Count; i++)
			{
				pots[i].ChangeWaterAmount(pots[i].WaterCapacity * normalizedAmount);
			}
		}

		// Token: 0x0600546F RID: 21615 RVA: 0x00163C90 File Offset: 0x00161E90
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

		// Token: 0x06005471 RID: 21617 RVA: 0x00163DB2 File Offset: 0x00161FB2
		[CompilerGenerated]
		private IEnumerator <Water>g__Routine|15_0()
		{
			if (this.onSprinklerStart != null)
			{
				this.onSprinklerStart.Invoke();
			}
			this.WaterSound.Play();
			for (int j = 0; j < this.WaterParticles.Length; j++)
			{
				this.WaterParticles[j].Play();
			}
			int segments = 5;
			int num;
			for (int i = 0; i < segments; i = num + 1)
			{
				yield return new WaitForSeconds(this.ApplyWaterDelay / (float)segments);
				if (InstanceFinder.IsServer)
				{
					this.ApplyWater(1f / (float)segments);
				}
				num = i;
			}
			yield return new WaitForSeconds(this.ParticleStopDelay);
			for (int k = 0; k < this.WaterParticles.Length; k++)
			{
				this.WaterParticles[k].Stop();
			}
			this.WaterSound.Stop();
			this.IsSprinkling = false;
			yield break;
		}

		// Token: 0x06005472 RID: 21618 RVA: 0x00163DC4 File Offset: 0x00161FC4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.SprinklerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.SprinklerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendWater_2166136261));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_Water_2166136261));
		}

		// Token: 0x06005473 RID: 21619 RVA: 0x00163E16 File Offset: 0x00162016
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.SprinklerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.SprinklerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06005474 RID: 21620 RVA: 0x00163E2F File Offset: 0x0016202F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005475 RID: 21621 RVA: 0x00163E40 File Offset: 0x00162040
		private void RpcWriter___Server_SendWater_2166136261()
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

		// Token: 0x06005476 RID: 21622 RVA: 0x00163EDA File Offset: 0x001620DA
		private void RpcLogic___SendWater_2166136261()
		{
			this.Water();
		}

		// Token: 0x06005477 RID: 21623 RVA: 0x00163EE4 File Offset: 0x001620E4
		private void RpcReader___Server_SendWater_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendWater_2166136261();
		}

		// Token: 0x06005478 RID: 21624 RVA: 0x00163F14 File Offset: 0x00162114
		private void RpcWriter___Observers_Water_2166136261()
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

		// Token: 0x06005479 RID: 21625 RVA: 0x00163FBD File Offset: 0x001621BD
		private void RpcLogic___Water_2166136261()
		{
			if (this.IsSprinkling)
			{
				return;
			}
			this.IsSprinkling = true;
			this.ClickSound.Play();
			base.StartCoroutine(this.<Water>g__Routine|15_0());
		}

		// Token: 0x0600547A RID: 21626 RVA: 0x00163FE8 File Offset: 0x001621E8
		private void RpcReader___Observers_Water_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Water_2166136261();
		}

		// Token: 0x0600547B RID: 21627 RVA: 0x00164012 File Offset: 0x00162212
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003EA0 RID: 16032
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x04003EA1 RID: 16033
		public ParticleSystem[] WaterParticles;

		// Token: 0x04003EA2 RID: 16034
		public AudioSourceController ClickSound;

		// Token: 0x04003EA3 RID: 16035
		public AudioSourceController WaterSound;

		// Token: 0x04003EA4 RID: 16036
		[Header("Settings")]
		public float ApplyWaterDelay = 6f;

		// Token: 0x04003EA5 RID: 16037
		public float ParticleStopDelay = 2.5f;

		// Token: 0x04003EA6 RID: 16038
		public UnityEvent onSprinklerStart;

		// Token: 0x04003EA7 RID: 16039
		private bool dll_Excuted;

		// Token: 0x04003EA8 RID: 16040
		private bool dll_Excuted;
	}
}
