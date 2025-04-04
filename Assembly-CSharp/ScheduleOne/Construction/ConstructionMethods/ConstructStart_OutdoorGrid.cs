using System;
using ScheduleOne.Building;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.DevUtilities;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Construction.ConstructionMethods
{
	// Token: 0x0200071B RID: 1819
	public class ConstructStart_OutdoorGrid : ConstructStart_Base
	{
		// Token: 0x06003149 RID: 12617 RVA: 0x000CC264 File Offset: 0x000CA464
		public override void StartConstruction(string constructableID, Constructable_GridBased movedConstructable = null)
		{
			base.StartConstruction(constructableID, movedConstructable);
			this.GenerateGhostModel(constructableID);
			for (int i = 0; i < this.constructable.CoordinateFootprintTilePairs.Count; i++)
			{
				this.constructable.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.tileDetectionMode = ETileDetectionMode.OutdoorTile;
			}
			base.GetComponent<ConstructUpdate_OutdoorGrid>().GhostModel = this.ghostModel;
			base.GetComponent<ConstructUpdate_OutdoorGrid>().ConstructableClass = this.constructable;
			if (movedConstructable != null)
			{
				base.GetComponent<ConstructUpdate_OutdoorGrid>().currentRotation = movedConstructable.SyncAccessor_Rotation;
			}
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x000CC2F8 File Offset: 0x000CA4F8
		private void GenerateGhostModel(string id)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Registry.GetConstructable(id).gameObject, base.transform);
			this.constructable = gameObject.GetComponent<Constructable_GridBased>();
			if (this.constructable == null)
			{
				Console.LogWarning("CreateGhostModel: asset path is not a Constructable!", null);
				return;
			}
			this.constructable.enabled = false;
			this.constructable.isGhost = true;
			Singleton<BuildManager>.Instance.DisableColliders(gameObject);
			Singleton<BuildManager>.Instance.ApplyMaterial(gameObject, Singleton<BuildManager>.Instance.ghostMaterial_White, true);
			Singleton<BuildManager>.Instance.DisableNavigation(gameObject);
			Singleton<BuildManager>.Instance.DisableNetworking(gameObject);
			this.constructable.SetFootprintTileVisiblity(false);
			this.ghostModel = this.constructable.gameObject.transform;
		}

		// Token: 0x04002338 RID: 9016
		private Constructable_GridBased constructable;

		// Token: 0x04002339 RID: 9017
		private Transform ghostModel;
	}
}
