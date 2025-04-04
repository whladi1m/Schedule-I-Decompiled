using System;
using FishNet.Object;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.Construction.ConstructionMethods;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.Tiles;
using ScheduleOne.UI.Construction;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Construction
{
	// Token: 0x0200070E RID: 1806
	public class ConstructionManager : Singleton<ConstructionManager>
	{
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060030EA RID: 12522 RVA: 0x000CB3E6 File Offset: 0x000C95E6
		// (set) Token: 0x060030EB RID: 12523 RVA: 0x000CB3EE File Offset: 0x000C95EE
		public bool constructionModeEnabled { get; protected set; }

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060030EC RID: 12524 RVA: 0x000CB3F7 File Offset: 0x000C95F7
		// (set) Token: 0x060030ED RID: 12525 RVA: 0x000CB3FF File Offset: 0x000C95FF
		public bool isDeployingConstructable { get; protected set; }

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060030EE RID: 12526 RVA: 0x000CB408 File Offset: 0x000C9608
		// (set) Token: 0x060030EF RID: 12527 RVA: 0x000CB410 File Offset: 0x000C9610
		public bool isMovingConstructable { get; protected set; }

		// Token: 0x060030F0 RID: 12528 RVA: 0x000CB419 File Offset: 0x000C9619
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000CB434 File Offset: 0x000C9634
		public void EnterConstructionMode(Property prop)
		{
			this.currentProperty = prop;
			this.constructionModeEnabled = true;
			prop.SetBoundsVisible(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			if (this.onConstructionModeEnabled != null)
			{
				this.onConstructionModeEnabled();
			}
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x000CB48C File Offset: 0x000C968C
		public void ExitConstructionMode()
		{
			this.currentProperty.SetBoundsVisible(false);
			this.constructionModeEnabled = false;
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<BirdsEyeView>.Instance.Disable(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			this.currentProperty = null;
			if (this.onConstructionModeDisabled != null)
			{
				this.onConstructionModeDisabled();
			}
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x000CB4F4 File Offset: 0x000C96F4
		public void DeployConstructable(ConstructionMenu.ConstructionMenuListing listing)
		{
			this.isDeployingConstructable = true;
			if (Registry.GetConstructable(listing.ID)._constructionHandler_Asset != null)
			{
				this.constructHandler = UnityEngine.Object.Instantiate<GameObject>(Registry.GetConstructable(listing.ID)._constructionHandler_Asset, base.transform);
				this.constructHandler.GetComponent<ConstructStart_Base>().StartConstruction(listing.ID, null);
				return;
			}
			Console.LogWarning("Constructable doesn't have a construction handler!", null);
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x000CB564 File Offset: 0x000C9764
		public void StopConstructableDeploy()
		{
			this.isDeployingConstructable = false;
			this.constructHandler.GetComponent<ConstructStop_Base>().StopConstruction();
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x000CB580 File Offset: 0x000C9780
		public void MoveConstructable(Constructable_GridBased c)
		{
			this.isMovingConstructable = true;
			if (c._constructionHandler_Asset != null)
			{
				this.constructHandler = UnityEngine.Object.Instantiate<GameObject>(c._constructionHandler_Asset, base.transform);
				this.constructHandler.GetComponent<ConstructStart_Base>().StartConstruction(c.PrefabID, c);
				return;
			}
			Console.LogWarning("Constructable doesn't have a construction handler!", null);
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x000CB5DC File Offset: 0x000C97DC
		public void StopMovingConstructable()
		{
			this.isMovingConstructable = false;
			this.constructHandler.GetComponent<ConstructStop_Base>().StopConstruction();
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x000CB5F8 File Offset: 0x000C97F8
		private void Exit(ExitAction exit)
		{
			if (exit.used)
			{
				return;
			}
			if (this.constructionModeEnabled)
			{
				if (this.isDeployingConstructable)
				{
					exit.used = true;
					Singleton<ConstructionMenu>.Instance.ClearSelectedListing();
					return;
				}
				if (this.isMovingConstructable)
				{
					exit.used = true;
					this.StopMovingConstructable();
					return;
				}
				if (exit.exitType == ExitType.Escape)
				{
					exit.used = true;
					this.ExitConstructionMode();
				}
			}
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x000CB65C File Offset: 0x000C985C
		public Constructable_GridBased CreateConstructable_GridBased(string ID, Grid grid, Vector2 originCoordinate, float rotation)
		{
			Constructable_GridBased component = UnityEngine.Object.Instantiate<GameObject>(Registry.GetPrefab(ID), null).GetComponent<Constructable_GridBased>();
			component.InitializeConstructable_GridBased(grid, originCoordinate, rotation);
			this.networkObject.Spawn(component.gameObject, null, default(Scene));
			return component;
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x000CB6A1 File Offset: 0x000C98A1
		public Constructable CreateConstructable(string prefabID)
		{
			return UnityEngine.Object.Instantiate<GameObject>(Registry.GetPrefab(prefabID), null).GetComponent<Constructable>();
		}

		// Token: 0x040022FE RID: 8958
		public NetworkObject networkObject;

		// Token: 0x04002300 RID: 8960
		public Action onConstructionModeEnabled;

		// Token: 0x04002301 RID: 8961
		public Action onConstructionModeDisabled;

		// Token: 0x04002303 RID: 8963
		public GameObject constructHandler;

		// Token: 0x04002305 RID: 8965
		public ConstructionManager.ConstructableNotification onNewConstructableBuilt;

		// Token: 0x04002306 RID: 8966
		public ConstructionManager.ConstructableNotification onConstructableMoved;

		// Token: 0x04002307 RID: 8967
		public Property currentProperty;

		// Token: 0x0200070F RID: 1807
		public class WorldIntersection
		{
			// Token: 0x04002308 RID: 8968
			public FootprintTile footprint;

			// Token: 0x04002309 RID: 8969
			public Tile tile;
		}

		// Token: 0x02000710 RID: 1808
		// (Invoke) Token: 0x060030FD RID: 12541
		public delegate void ConstructableNotification(Constructable c);
	}
}
