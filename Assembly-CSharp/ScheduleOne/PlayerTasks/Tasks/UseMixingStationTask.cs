using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.StationFramework;
using ScheduleOne.Trash;
using ScheduleOne.UI;
using ScheduleOne.UI.Stations;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks.Tasks
{
	// Token: 0x0200035C RID: 860
	public class UseMixingStationTask : Task
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x00056819 File Offset: 0x00054A19
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x00056821 File Offset: 0x00054A21
		public MixingStation Station { get; private set; }

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x0005682A File Offset: 0x00054A2A
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x00056832 File Offset: 0x00054A32
		public UseMixingStationTask.EStep CurrentStep { get; private set; }

		// Token: 0x06001371 RID: 4977 RVA: 0x0005683B File Offset: 0x00054A3B
		public static string GetStepDescription(UseMixingStationTask.EStep step)
		{
			if (step == UseMixingStationTask.EStep.CombineIngredients)
			{
				return "Combine ingredients in bowl";
			}
			if (step != UseMixingStationTask.EStep.StartMixing)
			{
				return "Unknown step";
			}
			return "Start mixing machine";
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x00056858 File Offset: 0x00054A58
		public UseMixingStationTask(MixingStation station)
		{
			UseMixingStationTask.<>c__DisplayClass15_0 CS$<>8__locals1;
			CS$<>8__locals1.station = station;
			base..ctor();
			CS$<>8__locals1.<>4__this = this;
			this.Station = CS$<>8__locals1.station;
			this.Station.onStartButtonClicked.AddListener(new UnityAction(this.StartButtonPressed));
			this.ClickDetectionRadius = 0.012f;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Station.CameraPosition_CombineIngredients.position, this.Station.CameraPosition_CombineIngredients.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(this.TaskName);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			this.removedIngredients = new ItemInstance[2];
			int mixQuantity = CS$<>8__locals1.station.GetMixQuantity();
			this.removedIngredients[0] = CS$<>8__locals1.station.ProductSlot.ItemInstance.GetCopy(mixQuantity);
			this.removedIngredients[1] = CS$<>8__locals1.station.MixerSlot.ItemInstance.GetCopy(mixQuantity);
			CS$<>8__locals1.station.ProductSlot.ChangeQuantity(-mixQuantity, false);
			CS$<>8__locals1.station.MixerSlot.ChangeQuantity(-mixQuantity, false);
			base.EnableMultiDragging(CS$<>8__locals1.station.ItemContainer, 0.12f);
			int num = 0;
			Singleton<InputPromptsCanvas>.Instance.LoadModule("packaging");
			for (int i = 0; i < mixQuantity; i++)
			{
				this.<.ctor>g__SetupIngredient|15_0(this.removedIngredients[1].Definition as StorableItemDefinition, num, true, ref CS$<>8__locals1);
				num++;
			}
			for (int j = 0; j < mixQuantity; j++)
			{
				this.<.ctor>g__SetupIngredient|15_0(this.removedIngredients[0].Definition as StorableItemDefinition, num, false, ref CS$<>8__locals1);
				num++;
			}
			if (this.Jug != null)
			{
				this.Jug.Pourable.LiquidCapacity_L = this.Jug.Fillable.LiquidCapacity_L;
				this.Jug.Pourable.DefaultLiquid_L = this.Jug.Fillable.GetTotalLiquidVolume();
				this.Jug.Pourable.SetLiquidLevel(this.Jug.Pourable.DefaultLiquid_L);
				this.Jug.Pourable.PourParticlesColor = this.Jug.Fillable.LiquidContainer.LiquidColor;
				this.Jug.Pourable.LiquidColor = this.Jug.Fillable.LiquidContainer.LiquidColor;
				this.Jug.Pourable.PourParticles[0].trigger.AddCollider(this.Station.BowlFillable.LiquidContainer.Collider);
				this.Jug.Fillable.FillableEnabled = false;
			}
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00056B18 File Offset: 0x00054D18
		private Beaker CreateJug()
		{
			Beaker component = UnityEngine.Object.Instantiate<GameObject>(this.Station.JugPrefab, this.Station.ItemContainer).GetComponent<Beaker>();
			component.transform.position = this.Station.JugAlignment.position;
			component.transform.rotation = this.Station.JugAlignment.rotation;
			component.GetComponent<DraggableConstraint>().Container = this.Station.ItemContainer;
			component.ActivateModule<PourableModule>();
			return component;
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00056B97 File Offset: 0x00054D97
		public override void Update()
		{
			base.Update();
			this.CheckProgress();
			this.UpdateInstruction();
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x00056BAC File Offset: 0x00054DAC
		private void UpdateInstruction()
		{
			base.CurrentInstruction = UseMixingStationTask.GetStepDescription(this.CurrentStep);
			if (this.CurrentStep == UseMixingStationTask.EStep.CombineIngredients)
			{
				int num = this.items.Count;
				if (this.Jug != null)
				{
					num++;
				}
				int combinedIngredients = this.GetCombinedIngredients();
				base.CurrentInstruction = string.Concat(new string[]
				{
					base.CurrentInstruction,
					" (",
					combinedIngredients.ToString(),
					"/",
					num.ToString(),
					")"
				});
			}
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x00056C3D File Offset: 0x00054E3D
		private void CheckProgress()
		{
			if (this.CurrentStep == UseMixingStationTask.EStep.CombineIngredients)
			{
				this.CheckStep_CombineIngredients();
			}
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00056C4D File Offset: 0x00054E4D
		private void CheckStep_CombineIngredients()
		{
			if (this.GetCombinedIngredients() >= this.items.Count + ((this.Jug != null) ? 1 : 0))
			{
				this.ProgressStep();
			}
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00056C7C File Offset: 0x00054E7C
		private int GetCombinedIngredients()
		{
			int num = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].HasModule<IngredientModule>())
				{
					IngredientModule module = this.items[i].GetModule<IngredientModule>();
					bool flag = true;
					IngredientPiece[] pieces = module.Pieces;
					for (int j = 0; j < pieces.Length; j++)
					{
						if (pieces[j].CurrentLiquidContainer != this.Station.BowlFillable.LiquidContainer)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						num++;
					}
				}
				else if (this.items[i].HasModule<PourableModule>() && this.items[i].GetModule<PourableModule>().NormalizedLiquidLevel <= 0.02f)
				{
					num++;
				}
			}
			if (this.Jug != null && this.Jug.Pourable.NormalizedLiquidLevel <= 0.02f)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x00056D70 File Offset: 0x00054F70
		private void ProgressStep()
		{
			UseMixingStationTask.EStep currentStep = this.CurrentStep;
			this.CurrentStep = currentStep + 1;
			if (this.CurrentStep == UseMixingStationTask.EStep.StartMixing)
			{
				this.Station.SetStartButtonClickable(true);
			}
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00056DA2 File Offset: 0x00054FA2
		private void StartButtonPressed()
		{
			if (this.CurrentStep == UseMixingStationTask.EStep.StartMixing)
			{
				this.Success();
			}
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00056DB4 File Offset: 0x00054FB4
		public override void Success()
		{
			ProductItemInstance productItemInstance = this.removedIngredients[0] as ProductItemInstance;
			string id = this.removedIngredients[1].Definition.ID;
			this.CreateTrash();
			Singleton<MixingStationCanvas>.Instance.StartMixOperation(new MixOperation(productItemInstance.ID, productItemInstance.Quality, id, productItemInstance.Quantity));
			base.Success();
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00056E10 File Offset: 0x00055010
		private void CreateTrash()
		{
			BoxCollider trashSpawnVolume = this.Station.TrashSpawnVolume;
			for (int i = 0; i < Mathf.CeilToInt((float)this.mixerItems.Count / 2f); i++)
			{
				if (!(this.mixerItems[0].TrashPrefab == null))
				{
					Vector3 posiiton = trashSpawnVolume.transform.TransformPoint(new Vector3(UnityEngine.Random.Range(-trashSpawnVolume.size.x / 2f, trashSpawnVolume.size.x / 2f), 0f, UnityEngine.Random.Range(-trashSpawnVolume.size.z / 2f, trashSpawnVolume.size.z / 2f)));
					Vector3 vector = trashSpawnVolume.transform.forward;
					vector = Quaternion.Euler(0f, UnityEngine.Random.Range(-45f, 45f), 0f) * vector;
					float d = UnityEngine.Random.Range(0.25f, 0.4f);
					NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.mixerItems[0].TrashPrefab.ID, posiiton, UnityEngine.Random.rotation, vector * d, "", false);
				}
			}
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00056F4C File Offset: 0x0005514C
		public override void StopTask()
		{
			this.Station.onStartButtonClicked.RemoveListener(new UnityAction(this.StartButtonPressed));
			this.Station.BowlFillable.ResetContents();
			if (this.Outcome != Task.EOutcome.Success)
			{
				this.Station.ProductSlot.AddItem(this.removedIngredients[0], false);
				this.Station.MixerSlot.AddItem(this.removedIngredients[1], false);
			}
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			foreach (StationItem stationItem in this.items)
			{
				stationItem.Destroy();
			}
			this.items.Clear();
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(this.TaskName);
			this.Station.Open();
			if (this.Jug != null)
			{
				UnityEngine.Object.Destroy(this.Jug.gameObject);
			}
			base.StopTask();
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x00057058 File Offset: 0x00055258
		[CompilerGenerated]
		private void <.ctor>g__SetupIngredient|15_0(StorableItemDefinition def, int index, bool mixer, ref UseMixingStationTask.<>c__DisplayClass15_0 A_4)
		{
			if (def.StationItem == null)
			{
				Console.LogError("Ingredient '" + def.Name + "' does not have a station item", null);
				return;
			}
			if (mixer)
			{
				this.mixerItems.Add(def.StationItem);
			}
			if (def.StationItem.HasModule<PourableModule>())
			{
				if (this.Jug == null)
				{
					this.Jug = this.CreateJug();
				}
				PourableModule module = def.StationItem.GetModule<PourableModule>();
				this.Jug.Fillable.AddLiquid(module.LiquidType, module.LiquidCapacity_L, module.LiquidColor);
				return;
			}
			StationItem stationItem = UnityEngine.Object.Instantiate<StationItem>(def.StationItem, A_4.station.ItemContainer);
			stationItem.transform.rotation = A_4.station.IngredientTransforms[this.items.Count].rotation;
			Vector3 eulerAngles = stationItem.transform.eulerAngles;
			eulerAngles.y = UnityEngine.Random.Range(0f, 360f);
			stationItem.transform.eulerAngles = eulerAngles;
			stationItem.transform.position = A_4.station.IngredientTransforms[this.items.Count].position;
			stationItem.Initialize(def);
			if (stationItem.HasModule<IngredientModule>())
			{
				stationItem.ActivateModule<IngredientModule>();
				foreach (IngredientPiece ingredientPiece in stationItem.GetModule<IngredientModule>().Pieces)
				{
					this.ingredientPieces.Add(ingredientPiece);
					ingredientPiece.DisableInteractionInLiquid = false;
				}
			}
			else
			{
				Console.LogError("Ingredient '" + def.Name + "' does not have an ingredient or pourable module", null);
			}
			foreach (Draggable draggable in stationItem.GetComponentsInChildren<Draggable>())
			{
				draggable.DragProjectionMode = Draggable.EDragProjectionMode.FlatCameraForward;
				DraggableConstraint component = draggable.gameObject.GetComponent<DraggableConstraint>();
				if (component != null)
				{
					component.ProportionalZClamp = true;
				}
			}
			this.items.Add(stationItem);
		}

		// Token: 0x0400129C RID: 4764
		private List<StationItem> items = new List<StationItem>();

		// Token: 0x0400129D RID: 4765
		private List<StationItem> mixerItems = new List<StationItem>();

		// Token: 0x0400129E RID: 4766
		private List<IngredientPiece> ingredientPieces = new List<IngredientPiece>();

		// Token: 0x0400129F RID: 4767
		private ItemInstance[] removedIngredients;

		// Token: 0x040012A0 RID: 4768
		private Beaker Jug;

		// Token: 0x0200035D RID: 861
		public enum EStep
		{
			// Token: 0x040012A2 RID: 4770
			CombineIngredients,
			// Token: 0x040012A3 RID: 4771
			StartMixing
		}
	}
}
