using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.Soil;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks.Tasks
{
	// Token: 0x0200035A RID: 858
	public class PourSoilTask : PourIntoPotTask
	{
		// Token: 0x06001361 RID: 4961 RVA: 0x00056500 File Offset: 0x00054700
		public PourSoilTask(Pot _pot, ItemInstance _itemInstance, Pourable _pourablePrefab) : base(_pot, _itemInstance, _pourablePrefab)
		{
			base.CurrentInstruction = "Click and drag to cut soil bag";
			this.soil = (this.pourable as PourableSoil);
			this.soil.onOpened.AddListener(new UnityAction(base.RemoveItem));
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00056550 File Offset: 0x00054750
		public override void Update()
		{
			base.Update();
			if (this.soil.IsOpen)
			{
				base.CurrentInstruction = "Pour soil into pot (" + Mathf.FloorToInt(this.pot.SoilLevel / this.pot.SoilCapacity * 100f).ToString() + "%)";
			}
			this.UpdateHover();
			this.UpdateCursor();
			if (this.HoveredTopCollider != null && GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && this.soil.TopColliders.IndexOf(this.HoveredTopCollider) == this.soil.currentCut)
			{
				this.soil.Cut();
			}
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x000565FF File Offset: 0x000547FF
		public override void StopTask()
		{
			this.pot.PushSoilDataToServer();
			base.StopTask();
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00056614 File Offset: 0x00054814
		protected override void UpdateCursor()
		{
			if (this.soil.IsOpen)
			{
				base.UpdateCursor();
				return;
			}
			if (this.HoveredTopCollider != null && this.soil.TopColliders.IndexOf(this.HoveredTopCollider) == this.soil.currentCut)
			{
				Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Scissors);
				return;
			}
			Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0005667D File Offset: 0x0005487D
		private void UpdateHover()
		{
			this.HoveredTopCollider = this.GetHoveredTopCollider();
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x0005668C File Offset: 0x0005488C
		private Collider GetHoveredTopCollider()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(3f, out raycastHit, Singleton<InteractionManager>.Instance.Interaction_SearchMask, true, 0f) && this.soil.TopColliders.Contains(raycastHit.collider))
			{
				return raycastHit.collider;
			}
			return null;
		}

		// Token: 0x04001296 RID: 4758
		private PourableSoil soil;

		// Token: 0x04001297 RID: 4759
		private Collider HoveredTopCollider;
	}
}
