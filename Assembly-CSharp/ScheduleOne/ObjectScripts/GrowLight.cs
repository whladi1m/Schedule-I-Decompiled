using System;
using System.Collections.Generic;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Lighting;
using ScheduleOne.Misc;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B74 RID: 2932
	public class GrowLight : ProceduralGridItem
	{
		// Token: 0x06004E9A RID: 20122 RVA: 0x0014B60C File Offset: 0x0014980C
		public override void InitializeProceduralGridItem(ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			base.InitializeProceduralGridItem(instance, _rotation, _footprintTileMatches, GUID);
			if (!this.isGhost)
			{
				this.SetIsOn(true);
				foreach (CoordinateProceduralTilePair coordinateProceduralTilePair in base.SyncAccessor_footprintTileMatches)
				{
					if (coordinateProceduralTilePair.tile.MatchedFootprintTile != null)
					{
						coordinateProceduralTilePair.tile.MatchedFootprintTile.MatchedStandardTile.LightExposureNode.AddSource(this.usableLightSource, 1f);
					}
				}
			}
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x0014B6AC File Offset: 0x001498AC
		public void SetIsOn(bool isOn)
		{
			this.usableLightSource.isEmitting = isOn;
			this.Light.isOn = isOn;
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x0014B6C8 File Offset: 0x001498C8
		public override void DestroyItem(bool callOnServer = true)
		{
			foreach (CoordinateProceduralTilePair coordinateProceduralTilePair in base.SyncAccessor_footprintTileMatches)
			{
				if (coordinateProceduralTilePair.tile.MatchedFootprintTile != null)
				{
					coordinateProceduralTilePair.tile.MatchedFootprintTile.MatchedStandardTile.LightExposureNode.RemoveSource(this.usableLightSource);
				}
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x0014B758 File Offset: 0x00149958
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.GrowLightAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.GrowLightAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x0014B771 File Offset: 0x00149971
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.GrowLightAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.GrowLightAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x0014B78A File Offset: 0x0014998A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x0014B798 File Offset: 0x00149998
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003B71 RID: 15217
		[Header("References")]
		public ToggleableLight Light;

		// Token: 0x04003B72 RID: 15218
		public UsableLightSource usableLightSource;

		// Token: 0x04003B73 RID: 15219
		private bool dll_Excuted;

		// Token: 0x04003B74 RID: 15220
		private bool dll_Excuted;
	}
}
