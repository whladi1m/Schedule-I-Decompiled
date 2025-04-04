using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.StationFramework;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000343 RID: 835
	public class CauldronTask : Task
	{
		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060012BC RID: 4796 RVA: 0x0005209F File Offset: 0x0005029F
		// (set) Token: 0x060012BD RID: 4797 RVA: 0x000520A7 File Offset: 0x000502A7
		public Cauldron Cauldron { get; private set; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060012BE RID: 4798 RVA: 0x000520B0 File Offset: 0x000502B0
		// (set) Token: 0x060012BF RID: 4799 RVA: 0x000520B8 File Offset: 0x000502B8
		public CauldronTask.EStep CurrentStep { get; private set; }

		// Token: 0x060012C0 RID: 4800 RVA: 0x000520C1 File Offset: 0x000502C1
		public static string GetStepDescription(CauldronTask.EStep step)
		{
			if (step == CauldronTask.EStep.CombineIngredients)
			{
				return "Combine leaves and gasoline in cauldron";
			}
			if (step != CauldronTask.EStep.StartMixing)
			{
				return "Unknown step";
			}
			return "Start cauldron";
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x000520E0 File Offset: 0x000502E0
		public CauldronTask(Cauldron caudron)
		{
			this.Cauldron = caudron;
			this.Cauldron.onStartButtonClicked.AddListener(new UnityAction(this.StartButtonPressed));
			this.Cauldron.OverheadLight.enabled = true;
			this.ClickDetectionRadius = 0.012f;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Cauldron.CameraPosition_CombineIngredients.position, this.Cauldron.CameraPosition_CombineIngredients.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(this.TaskName);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			base.EnableMultiDragging(this.Cauldron.ItemContainer, 0.15f);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("packaging");
			this.Gasoline = UnityEngine.Object.Instantiate<StationItem>(this.Cauldron.GasolinePrefab, caudron.ItemContainer);
			this.Gasoline.transform.rotation = caudron.GasolineSpawnPoint.rotation;
			this.Gasoline.transform.position = caudron.GasolineSpawnPoint.position;
			this.Gasoline.transform.localScale = Vector3.one * 1.5f;
			this.Gasoline.ActivateModule<PourableModule>();
			this.Gasoline.GetComponentInChildren<Rigidbody>().rotation = caudron.GasolineSpawnPoint.rotation;
			this.CocaLeaves = new StationItem[20];
			for (int i = 0; i < this.CocaLeaves.Length; i++)
			{
				this.CocaLeaves[i] = UnityEngine.Object.Instantiate<StationItem>(this.Cauldron.CocaLeafPrefab, caudron.ItemContainer);
				this.CocaLeaves[i].transform.rotation = caudron.LeafSpawns[i].rotation;
				this.CocaLeaves[i].transform.position = caudron.LeafSpawns[i].position;
				this.CocaLeaves[i].ActivateModule<IngredientModule>();
				this.CocaLeaves[i].transform.localScale = Vector3.one * 0.85f;
				this.CocaLeaves[i].GetModule<IngredientModule>().Pieces[0].transform.SetParent(caudron.ItemContainer);
			}
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00052310 File Offset: 0x00050510
		public override void Success()
		{
			EQuality quality = this.Cauldron.RemoveIngredients();
			this.Cauldron.SendCookOperation(this.Cauldron.CookTime, quality);
			this.Cauldron.CreateTrash(new List<StationItem>
			{
				this.Gasoline
			});
			base.Success();
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00052364 File Offset: 0x00050564
		public override void StopTask()
		{
			this.Cauldron.OverheadLight.enabled = false;
			this.Cauldron.onStartButtonClicked.RemoveListener(new UnityAction(this.StartButtonPressed));
			this.Cauldron.StartButtonClickable.ClickableEnabled = false;
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(this.TaskName);
			this.Cauldron.Open();
			foreach (StationItem stationItem in this.CocaLeaves)
			{
				UnityEngine.Object.Destroy(stationItem.GetModule<IngredientModule>().Pieces[0].gameObject);
				stationItem.Destroy();
			}
			this.Gasoline.Destroy();
			if (this.Outcome != Task.EOutcome.Success)
			{
				this.Cauldron.CauldronFillable.ResetContents();
			}
			base.StopTask();
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00052432 File Offset: 0x00050632
		public override void Update()
		{
			base.Update();
			this.CheckProgress();
			this.UpdateInstruction();
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00052446 File Offset: 0x00050646
		private void CheckProgress()
		{
			if (this.CurrentStep == CauldronTask.EStep.CombineIngredients)
			{
				this.CheckStep_CombineIngredients();
			}
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00052458 File Offset: 0x00050658
		private void CheckStep_CombineIngredients()
		{
			if (this.Gasoline.GetModule<PourableModule>().LiquidLevel > 0.01f)
			{
				return;
			}
			StationItem[] cocaLeaves = this.CocaLeaves;
			for (int i = 0; i < cocaLeaves.Length; i++)
			{
				if (cocaLeaves[i].GetModule<IngredientModule>().Pieces[0].CurrentLiquidContainer == null)
				{
					return;
				}
			}
			this.StartMixing();
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x000524B8 File Offset: 0x000506B8
		private void StartMixing()
		{
			this.CurrentStep = CauldronTask.EStep.StartMixing;
			bool isHeld = this.Gasoline.GetModule<PourableModule>().Draggable.IsHeld;
			this.Gasoline.GetModule<PourableModule>().Draggable.ClickableEnabled = false;
			if (isHeld)
			{
				this.Gasoline.GetModule<PourableModule>().Draggable.Rb.AddForce(this.Cauldron.transform.right * 10f, ForceMode.VelocityChange);
			}
			StationItem[] cocaLeaves = this.CocaLeaves;
			for (int i = 0; i < cocaLeaves.Length; i++)
			{
				cocaLeaves[i].GetModule<IngredientModule>().Pieces[0].GetComponent<Draggable>().ClickableEnabled = false;
			}
			this.Cauldron.StartButtonClickable.ClickableEnabled = true;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Cauldron.CameraPosition_StartMachine.position, this.Cauldron.CameraPosition_StartMachine.rotation, 0.2f, false);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0005259E File Offset: 0x0005079E
		private void UpdateInstruction()
		{
			base.CurrentInstruction = CauldronTask.GetStepDescription(this.CurrentStep);
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x000525B1 File Offset: 0x000507B1
		private void StartButtonPressed()
		{
			if (this.CurrentStep == CauldronTask.EStep.StartMixing)
			{
				this.Success();
			}
		}

		// Token: 0x0400121F RID: 4639
		private StationItem[] CocaLeaves;

		// Token: 0x04001220 RID: 4640
		private StationItem Gasoline;

		// Token: 0x04001221 RID: 4641
		private Draggable Tub;

		// Token: 0x02000344 RID: 836
		public enum EStep
		{
			// Token: 0x04001223 RID: 4643
			CombineIngredients,
			// Token: 0x04001224 RID: 4644
			StartMixing
		}
	}
}
