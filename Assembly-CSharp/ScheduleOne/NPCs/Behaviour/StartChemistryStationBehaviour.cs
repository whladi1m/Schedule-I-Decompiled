using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Employees;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000503 RID: 1283
	public class StartChemistryStationBehaviour : Behaviour
	{
		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001E5A RID: 7770 RVA: 0x0007CBCB File Offset: 0x0007ADCB
		// (set) Token: 0x06001E5B RID: 7771 RVA: 0x0007CBD3 File Offset: 0x0007ADD3
		public ChemistryStation targetStation { get; private set; }

		// Token: 0x06001E5C RID: 7772 RVA: 0x0007CBDC File Offset: 0x0007ADDC
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.StartChemistryStationBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0007CBF0 File Offset: 0x0007ADF0
		public void SetTargetStation(ChemistryStation station)
		{
			this.targetStation = station;
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0007CBFC File Offset: 0x0007ADFC
		protected override void End()
		{
			base.End();
			if (this.beaker != null)
			{
				this.beaker.Destroy();
				this.beaker = null;
			}
			if (this.targetStation != null)
			{
				this.targetStation.StaticBeaker.gameObject.SetActive(true);
			}
			if (this.cookRoutine != null)
			{
				this.StopCook();
			}
			this.Disable();
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x0007CC68 File Offset: 0x0007AE68
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.cookRoutine != null)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!base.Npc.Movement.IsMoving)
			{
				if (this.IsAtStation())
				{
					this.StartCook();
					return;
				}
				base.SetDestination(this.GetStationAccessPoint(), true);
			}
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x0007CCBA File Offset: 0x0007AEBA
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			if (this.cookRoutine != null)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.targetStation.UIPoint.position, 5, false);
			}
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x0007CCF1 File Offset: 0x0007AEF1
		[ObserversRpc(RunLocally = true)]
		private void StartCook()
		{
			this.RpcWriter___Observers_StartCook_2166136261();
			this.RpcLogic___StartCook_2166136261();
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x0007CD00 File Offset: 0x0007AF00
		private void SetupBeaker()
		{
			if (this.beaker != null)
			{
				Console.LogWarning("Beaker already exists!", null);
				return;
			}
			this.beaker = this.targetStation.CreateBeaker();
			this.targetStation.StaticBeaker.gameObject.SetActive(false);
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x0007CD50 File Offset: 0x0007AF50
		private void FillBeaker(StationRecipe recipe, Beaker beaker)
		{
			for (int i = 0; i < recipe.Ingredients.Count; i++)
			{
				StorableItemDefinition storableItemDefinition = null;
				foreach (ItemDefinition itemDefinition in recipe.Ingredients[i].Items)
				{
					StorableItemDefinition storableItemDefinition2 = itemDefinition as StorableItemDefinition;
					for (int j = 0; j < this.targetStation.IngredientSlots.Length; j++)
					{
						if (this.targetStation.IngredientSlots[j].ItemInstance != null && this.targetStation.IngredientSlots[j].ItemInstance.Definition.ID == storableItemDefinition2.ID)
						{
							storableItemDefinition = storableItemDefinition2;
							break;
						}
					}
				}
				if (storableItemDefinition.StationItem == null)
				{
					Console.LogError("Ingredient '" + storableItemDefinition.Name + "' does not have a station item", null);
				}
				else
				{
					StationItem stationItem = storableItemDefinition.StationItem;
					if (!stationItem.HasModule<IngredientModule>())
					{
						if (stationItem.HasModule<PourableModule>())
						{
							PourableModule module = stationItem.GetModule<PourableModule>();
							beaker.Fillable.AddLiquid(module.LiquidType, module.LiquidCapacity_L, module.LiquidColor);
						}
						else
						{
							Console.LogError("Ingredient '" + storableItemDefinition.Name + "' does not have an ingredient or pourable module", null);
						}
					}
				}
			}
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x0007CEB4 File Offset: 0x0007B0B4
		private bool CanCookStart()
		{
			if (this.targetStation == null)
			{
				return false;
			}
			if (((IUsable)this.targetStation).IsInUse && ((IUsable)this.targetStation).NPCUserObject != base.Npc.NetworkObject)
			{
				return false;
			}
			ChemistryStationConfiguration chemistryStationConfiguration = this.targetStation.Configuration as ChemistryStationConfiguration;
			return !(chemistryStationConfiguration.Recipe.SelectedRecipe == null) && this.targetStation.HasIngredientsForRecipe(chemistryStationConfiguration.Recipe.SelectedRecipe);
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x0007CF3E File Offset: 0x0007B13E
		private void StopCook()
		{
			this.targetStation.SetNPCUser(null);
			base.Npc.SetAnimationBool_Networked(null, "UseChemistryStation", false);
			if (this.cookRoutine != null)
			{
				base.StopCoroutine(this.cookRoutine);
				this.cookRoutine = null;
			}
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x0007CF79 File Offset: 0x0007B179
		private Vector3 GetStationAccessPoint()
		{
			if (this.targetStation == null)
			{
				return base.Npc.transform.position;
			}
			return ((ITransitEntity)this.targetStation).AccessPoints[0].position;
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x0007CFAC File Offset: 0x0007B1AC
		private bool IsAtStation()
		{
			return !(this.targetStation == null) && Vector3.Distance(base.Npc.transform.position, this.GetStationAccessPoint()) < 1f;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x0007CFE0 File Offset: 0x0007B1E0
		[CompilerGenerated]
		private IEnumerator <StartCook>g__CookRoutine|15_0()
		{
			base.Npc.Movement.FacePoint(this.targetStation.transform.position, 0.5f);
			yield return new WaitForSeconds(0.5f);
			base.Npc.SetAnimationBool_Networked(null, "UseChemistryStation", true);
			if (!this.CanCookStart())
			{
				this.StopCook();
				base.End_Networked(null);
				yield break;
			}
			this.targetStation.SetNPCUser(base.Npc.NetworkObject);
			StationRecipe recipe = (this.targetStation.Configuration as ChemistryStationConfiguration).Recipe.SelectedRecipe;
			this.SetupBeaker();
			yield return new WaitForSeconds(1f);
			this.FillBeaker(recipe, this.beaker);
			yield return new WaitForSeconds(8f);
			yield return new WaitForSeconds(6f);
			yield return new WaitForSeconds(6f);
			List<ItemInstance> list = new List<ItemInstance>();
			for (int i = 0; i < recipe.Ingredients.Count; i++)
			{
				foreach (ItemDefinition itemDefinition in recipe.Ingredients[i].Items)
				{
					StorableItemDefinition storableItemDefinition = itemDefinition as StorableItemDefinition;
					for (int j = 0; j < this.targetStation.IngredientSlots.Length; j++)
					{
						if (this.targetStation.IngredientSlots[j].ItemInstance != null && this.targetStation.IngredientSlots[j].ItemInstance.Definition.ID == storableItemDefinition.ID)
						{
							list.Add(this.targetStation.IngredientSlots[j].ItemInstance.GetCopy(recipe.Ingredients[i].Quantity));
							this.targetStation.IngredientSlots[j].ChangeQuantity(-recipe.Ingredients[i].Quantity, false);
							break;
						}
					}
				}
			}
			EQuality productQuality = recipe.CalculateQuality(list);
			this.targetStation.SendCookOperation(new ChemistryCookOperation(recipe, productQuality, this.beaker.Container.LiquidColor, this.beaker.Fillable.LiquidContainer.CurrentLiquidLevel, 0));
			this.beaker.Destroy();
			this.beaker = null;
			this.StopCook();
			base.End_Networked(null);
			yield break;
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x0007CFEF File Offset: 0x0007B1EF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartChemistryStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartChemistryStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_StartCook_2166136261));
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x0007D01F File Offset: 0x0007B21F
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartChemistryStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartChemistryStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x0007D038 File Offset: 0x0007B238
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x0007D048 File Offset: 0x0007B248
		private void RpcWriter___Observers_StartCook_2166136261()
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
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x0007D0F1 File Offset: 0x0007B2F1
		private void RpcLogic___StartCook_2166136261()
		{
			if (this.cookRoutine != null)
			{
				return;
			}
			if (this.targetStation == null)
			{
				return;
			}
			this.cookRoutine = base.StartCoroutine(this.<StartCook>g__CookRoutine|15_0());
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x0007D120 File Offset: 0x0007B320
		private void RpcReader___Observers_StartCook_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartCook_2166136261();
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x0007D14A File Offset: 0x0007B34A
		protected virtual void dll()
		{
			base.Awake();
			this.chemist = (base.Npc as Chemist);
		}

		// Token: 0x04001811 RID: 6161
		public const float PLACE_INGREDIENTS_TIME = 8f;

		// Token: 0x04001812 RID: 6162
		public const float STIR_TIME = 6f;

		// Token: 0x04001813 RID: 6163
		public const float BURNER_TIME = 6f;

		// Token: 0x04001815 RID: 6165
		private Chemist chemist;

		// Token: 0x04001816 RID: 6166
		private Coroutine cookRoutine;

		// Token: 0x04001817 RID: 6167
		private Beaker beaker;

		// Token: 0x04001818 RID: 6168
		private bool dll_Excuted;

		// Token: 0x04001819 RID: 6169
		private bool dll_Excuted;
	}
}
