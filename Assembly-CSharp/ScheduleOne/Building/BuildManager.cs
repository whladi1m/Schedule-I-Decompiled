using System;
using System.Collections.Generic;
using FishNet.Object;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Building
{
	// Token: 0x0200076D RID: 1901
	public class BuildManager : Singleton<BuildManager>
	{
		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060033EC RID: 13292 RVA: 0x000D90D6 File Offset: 0x000D72D6
		public Transform _tempContainer
		{
			get
			{
				return this.tempContainer;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x000D90DE File Offset: 0x000D72DE
		// (set) Token: 0x060033EE RID: 13294 RVA: 0x000D90E6 File Offset: 0x000D72E6
		public bool isBuilding { get; protected set; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060033EF RID: 13295 RVA: 0x000D90EF File Offset: 0x000D72EF
		// (set) Token: 0x060033F0 RID: 13296 RVA: 0x000D90F7 File Offset: 0x000D72F7
		public GameObject currentBuildHandler { get; protected set; }

		// Token: 0x060033F1 RID: 13297 RVA: 0x000D9100 File Offset: 0x000D7300
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x000D9108 File Offset: 0x000D7308
		public void StartBuilding(ItemInstance item)
		{
			if (!(item.Definition is BuildableItemDefinition))
			{
				Console.LogError("StartBuilding called but not passed BuildableItemDefinition", null);
				return;
			}
			if (this.isBuilding)
			{
				Console.LogWarning("StartBuilding called but building is already happening!", null);
				this.StopBuilding();
			}
			BuildableItem builtItem = (item.Definition as BuildableItemDefinition).BuiltItem;
			if (builtItem == null)
			{
				Console.LogWarning("itemToBuild is null!", null);
				return;
			}
			this.isBuilding = true;
			this.currentBuildHandler = UnityEngine.Object.Instantiate<GameObject>(builtItem.BuildHandler, this.tempContainer);
			this.currentBuildHandler.GetComponent<BuildStart_Base>().StartBuilding(item);
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x000D919C File Offset: 0x000D739C
		public void StartBuildingStoredItem(ItemInstance item)
		{
			if (!(item.Definition is StorableItemDefinition))
			{
				Console.LogError("StartBuildingStoredItem called but not passed StorableItemDefinition", null);
				return;
			}
			if (this.isBuilding)
			{
				Console.LogWarning("StartBuildingStoredItem called but building is already happening!", null);
				this.StopBuilding();
			}
			this.isBuilding = true;
			this.currentBuildHandler = UnityEngine.Object.Instantiate<GameObject>(this.storedItemBuildHandler, this.tempContainer);
			this.currentBuildHandler.GetComponent<BuildStart_Base>().StartBuilding(item);
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x000D920C File Offset: 0x000D740C
		public void StartPlacingCash(ItemInstance item)
		{
			if (this.isBuilding)
			{
				Console.LogWarning("StartPlacingCash called but building is already happening!", null);
				this.StopBuilding();
			}
			this.isBuilding = true;
			this.currentBuildHandler = UnityEngine.Object.Instantiate<GameObject>(this.cashBuildHandler, this.tempContainer);
			this.currentBuildHandler.GetComponent<BuildStart_Cash>().StartBuilding(item);
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x000D9261 File Offset: 0x000D7461
		public void StopBuilding()
		{
			this.isBuilding = false;
			this.currentBuildHandler.GetComponent<BuildStop_Base>().Stop_Building();
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x000D927C File Offset: 0x000D747C
		public void PlayBuildSound(BuildableItemDefinition.EBuildSoundType type, Vector3 point)
		{
			BuildManager.BuildSound buildSound = this.PlaceSounds.Find((BuildManager.BuildSound s) => s.Type == type);
			if (buildSound != null)
			{
				buildSound.Sound.transform.position = point;
				buildSound.Sound.Play();
			}
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x000D92D0 File Offset: 0x000D74D0
		public void DisableColliders(GameObject obj)
		{
			Collider[] componentsInChildren = obj.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x000D92FC File Offset: 0x000D74FC
		public void DisableLights(GameObject obj)
		{
			Light[] componentsInChildren = obj.GetComponentsInChildren<Light>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x000D9328 File Offset: 0x000D7528
		public void DisableNetworking(GameObject obj)
		{
			NetworkObject[] componentsInChildren = obj.GetComponentsInChildren<NetworkObject>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i]);
			}
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x000D9354 File Offset: 0x000D7554
		public void DisableSpriteRenderers(GameObject obj)
		{
			SpriteRenderer[] componentsInChildren = obj.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x000D9380 File Offset: 0x000D7580
		public void ApplyMaterial(GameObject obj, Material mat, bool allMaterials = true)
		{
			MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!componentsInChildren[i].gameObject.GetComponentInParent<OverrideGhostMaterial>())
				{
					if (allMaterials)
					{
						Material[] materials = componentsInChildren[i].materials;
						for (int j = 0; j < materials.Length; j++)
						{
							materials[j] = mat;
						}
						componentsInChildren[i].materials = materials;
					}
					else
					{
						componentsInChildren[i].material = mat;
					}
				}
			}
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x000D93E8 File Offset: 0x000D75E8
		public void DisableNavigation(GameObject obj)
		{
			NavMeshObstacle[] componentsInChildren = obj.GetComponentsInChildren<NavMeshObstacle>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			NavMeshSurface[] componentsInChildren2 = obj.GetComponentsInChildren<NavMeshSurface>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].enabled = false;
			}
			NavMeshLink[] componentsInChildren3 = obj.GetComponentsInChildren<NavMeshLink>();
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].enabled = false;
			}
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x000D945C File Offset: 0x000D765C
		public void DisableCanvases(GameObject obj)
		{
			Canvas[] componentsInChildren = obj.GetComponentsInChildren<Canvas>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x000D9488 File Offset: 0x000D7688
		public GridItem CreateGridItem(ItemInstance item, Grid grid, Vector2 originCoordinate, int rotation, string guid = "")
		{
			BuildableItemDefinition buildableItemDefinition = item.Definition as BuildableItemDefinition;
			if (buildableItemDefinition == null)
			{
				Console.LogError("BuildGridItem called but could not find BuildableItemDefinition", null);
				return null;
			}
			if (grid == null)
			{
				Console.LogError("BuildGridItem called and passed null grid", null);
				return null;
			}
			string guid2 = string.IsNullOrEmpty(guid) ? GUIDManager.GenerateUniqueGUID().ToString() : guid;
			GridItem component = UnityEngine.Object.Instantiate<GameObject>(buildableItemDefinition.BuiltItem.gameObject, null).GetComponent<GridItem>();
			component.SetLocallyBuilt();
			component.InitializeGridItem(item, grid, originCoordinate, rotation, guid2);
			this.networkObject.Spawn(component.gameObject, null, default(Scene));
			return component;
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x000D9534 File Offset: 0x000D7734
		public ProceduralGridItem CreateProceduralGridItem(ItemInstance item, int rotationAngle, List<CoordinateProceduralTilePair> matches, string guid = "")
		{
			BuildableItemDefinition buildableItemDefinition = item.Definition as BuildableItemDefinition;
			if (buildableItemDefinition == null)
			{
				Console.LogError("BuildProceduralGridItem called but could not find BuildableItemDefinition", null);
				return null;
			}
			string guid2 = string.IsNullOrEmpty(guid) ? GUIDManager.GenerateUniqueGUID().ToString() : guid;
			ProceduralGridItem component = UnityEngine.Object.Instantiate<GameObject>(buildableItemDefinition.BuiltItem.gameObject, null).GetComponent<ProceduralGridItem>();
			component.SetLocallyBuilt();
			component.InitializeProceduralGridItem(item, rotationAngle, matches, guid2);
			this.networkObject.Spawn(component.gameObject, null, default(Scene));
			return component;
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x000D95C8 File Offset: 0x000D77C8
		public void CreateStoredItem(StorableItemInstance item, IStorageEntity parentStorageEntity, StorageGrid grid, Vector2 originCoord, float rotation)
		{
			if (parentStorageEntity == null)
			{
				Console.LogWarning("CreateStoredItem: parentStorageEntity is null", null);
				return;
			}
			if (item.Quantity != 1)
			{
				Console.LogWarning("CreateStoredItem: item quantity is '" + item.Quantity.ToString() + "'. It should be 1!", null);
			}
		}

		// Token: 0x04002545 RID: 9541
		public List<BuildManager.BuildSound> PlaceSounds = new List<BuildManager.BuildSound>();

		// Token: 0x04002546 RID: 9542
		[Header("References")]
		[SerializeField]
		protected Transform tempContainer;

		// Token: 0x04002547 RID: 9543
		public NetworkObject networkObject;

		// Token: 0x04002548 RID: 9544
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject storedItemBuildHandler;

		// Token: 0x04002549 RID: 9545
		[SerializeField]
		protected GameObject cashBuildHandler;

		// Token: 0x0400254A RID: 9546
		[Header("Materials")]
		public Material ghostMaterial_White;

		// Token: 0x0400254B RID: 9547
		public Material ghostMaterial_Red;

		// Token: 0x0200076E RID: 1902
		[Serializable]
		public class BuildSound
		{
			// Token: 0x0400254E RID: 9550
			public BuildableItemDefinition.EBuildSoundType Type;

			// Token: 0x0400254F RID: 9551
			public AudioSourceController Sound;
		}
	}
}
