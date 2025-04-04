using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x02000773 RID: 1907
	public class BuildStart_ProceduralGrid : BuildStart_Base
	{
		// Token: 0x0600340C RID: 13324 RVA: 0x000D9874 File Offset: 0x000D7A74
		public override void StartBuilding(ItemInstance itemInstance)
		{
			ProceduralGridItem proceduralGridItem = this.CreateGhostModel(itemInstance.Definition as BuildableItemDefinition);
			if (proceduralGridItem == null)
			{
				return;
			}
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			ProceduralGridItem component = proceduralGridItem.GetComponent<ProceduralGridItem>();
			base.gameObject.GetComponent<BuildUpdate_ProceduralGrid>().GhostModel = proceduralGridItem.gameObject;
			base.gameObject.GetComponent<BuildUpdate_ProceduralGrid>().ItemClass = component;
			base.gameObject.GetComponent<BuildUpdate_ProceduralGrid>().ItemInstance = itemInstance;
			Singleton<InputPromptsCanvas>.Instance.LoadModule("building");
			for (int i = 0; i < component.CoordinateFootprintTilePairs.Count; i++)
			{
				component.CoordinateFootprintTilePairs[i].footprintTile.tileDetector.tileDetectionMode = ETileDetectionMode.ProceduralTile;
			}
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x000D9928 File Offset: 0x000D7B28
		protected virtual ProceduralGridItem CreateGhostModel(BuildableItemDefinition itemDefinition)
		{
			itemDefinition.BuiltItem.isGhost = true;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(itemDefinition.BuiltItem.gameObject, base.transform);
			itemDefinition.BuiltItem.isGhost = false;
			ProceduralGridItem component = gameObject.GetComponent<ProceduralGridItem>();
			if (component == null)
			{
				Console.LogWarning("CreateGhostModel: asset path is not a BuildableItem!", null);
				return null;
			}
			component.enabled = false;
			component.isGhost = true;
			Singleton<BuildManager>.Instance.DisableColliders(gameObject);
			Singleton<BuildManager>.Instance.DisableNavigation(gameObject);
			Singleton<BuildManager>.Instance.DisableNetworking(gameObject);
			component.SetFootprintTileVisiblity(false);
			return component;
		}
	}
}
