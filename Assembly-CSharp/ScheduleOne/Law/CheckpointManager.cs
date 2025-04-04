using System;
using FishNet.Connection;
using ScheduleOne.DevUtilities;
using ScheduleOne.Map;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x020005A8 RID: 1448
	public class CheckpointManager : NetworkSingleton<CheckpointManager>
	{
		// Token: 0x06002427 RID: 9255 RVA: 0x000929F4 File Offset: 0x00090BF4
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.WesternCheckpoint.ActivationState == RoadCheckpoint.ECheckpointState.Enabled)
			{
				this.WesternCheckpoint.Enable(connection);
			}
			if (this.DocksCheckpoint.ActivationState == RoadCheckpoint.ECheckpointState.Enabled)
			{
				this.DocksCheckpoint.Enable(connection);
			}
			if (this.NorthResidentialCheckpoint.ActivationState == RoadCheckpoint.ECheckpointState.Enabled)
			{
				this.NorthResidentialCheckpoint.Enable(connection);
			}
			if (this.WestResidentialCheckpoint.ActivationState == RoadCheckpoint.ECheckpointState.Enabled)
			{
				this.WestResidentialCheckpoint.Enable(connection);
			}
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x00092A70 File Offset: 0x00090C70
		public void SetCheckpointEnabled(CheckpointManager.ECheckpointLocation checkpoint, bool enabled, int requestedOfficers)
		{
			if (enabled)
			{
				this.GetCheckpoint(checkpoint).Enable(null);
				for (int i = 0; i < requestedOfficers; i++)
				{
					if (Singleton<Map>.Instance.PoliceStation.OfficerPool.Count <= 0)
					{
						return;
					}
					Singleton<Map>.Instance.PoliceStation.PullOfficer().AssignToCheckpoint(checkpoint);
				}
				return;
			}
			this.GetCheckpoint(checkpoint).Disable();
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x00092AD2 File Offset: 0x00090CD2
		public RoadCheckpoint GetCheckpoint(CheckpointManager.ECheckpointLocation loc)
		{
			switch (loc)
			{
			case CheckpointManager.ECheckpointLocation.Western:
				return this.WesternCheckpoint;
			case CheckpointManager.ECheckpointLocation.Docks:
				return this.DocksCheckpoint;
			case CheckpointManager.ECheckpointLocation.NorthResidential:
				return this.NorthResidentialCheckpoint;
			case CheckpointManager.ECheckpointLocation.WestResidential:
				return this.WestResidentialCheckpoint;
			default:
				return null;
			}
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x00092B11 File Offset: 0x00090D11
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Law.CheckpointManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Law.CheckpointManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x00092B2A File Offset: 0x00090D2A
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Law.CheckpointManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Law.CheckpointManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x00092B43 File Offset: 0x00090D43
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x00092B51 File Offset: 0x00090D51
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001AF9 RID: 6905
		[Header("References")]
		public RoadCheckpoint WesternCheckpoint;

		// Token: 0x04001AFA RID: 6906
		public RoadCheckpoint DocksCheckpoint;

		// Token: 0x04001AFB RID: 6907
		public RoadCheckpoint NorthResidentialCheckpoint;

		// Token: 0x04001AFC RID: 6908
		public RoadCheckpoint WestResidentialCheckpoint;

		// Token: 0x04001AFD RID: 6909
		private bool dll_Excuted;

		// Token: 0x04001AFE RID: 6910
		private bool dll_Excuted;

		// Token: 0x020005A9 RID: 1449
		public enum ECheckpointLocation
		{
			// Token: 0x04001B00 RID: 6912
			Western,
			// Token: 0x04001B01 RID: 6913
			Docks,
			// Token: 0x04001B02 RID: 6914
			NorthResidential,
			// Token: 0x04001B03 RID: 6915
			WestResidential
		}
	}
}
