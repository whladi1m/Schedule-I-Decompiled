using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004EF RID: 1263
	public class MoveItemBehaviour : Behaviour
	{
		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001D63 RID: 7523 RVA: 0x0007915D File Offset: 0x0007735D
		// (set) Token: 0x06001D64 RID: 7524 RVA: 0x00079165 File Offset: 0x00077365
		public bool Initialized { get; protected set; }

		// Token: 0x06001D65 RID: 7525 RVA: 0x00079170 File Offset: 0x00077370
		public void Initialize(TransitRoute route, ItemInstance _itemToRetrieveTemplate, int _maxMoveAmount = -1, bool _skipPickup = false)
		{
			string str;
			if (!this.IsTransitRouteValid(route, _itemToRetrieveTemplate, out str))
			{
				Console.LogError("Invalid transit route for move item behaviour! Reason: " + str, null);
				return;
			}
			this.assignedRoute = route;
			this.itemToRetrieveTemplate = _itemToRetrieveTemplate;
			this.maxMoveAmount = _maxMoveAmount;
			if (base.Npc.behaviour.DEBUG_MODE)
			{
				Console.Log(string.Concat(new string[]
				{
					"MoveItemBehaviour initialized with route: ",
					route.Source.Name,
					" -> ",
					route.Destination.Name,
					" for item: ",
					_itemToRetrieveTemplate.ID
				}), null);
			}
			this.skipPickup = _skipPickup;
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x00079218 File Offset: 0x00077418
		public void Resume(TransitRoute route, ItemInstance _itemToRetrieveTemplate, int _maxMoveAmount = -1)
		{
			this.assignedRoute = route;
			this.itemToRetrieveTemplate = _itemToRetrieveTemplate;
			this.maxMoveAmount = _maxMoveAmount;
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x0007922F File Offset: 0x0007742F
		protected override void Begin()
		{
			base.Begin();
			this.StartTransit();
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x0007923D File Offset: 0x0007743D
		protected override void Pause()
		{
			base.Pause();
			this.StopCurrentActivity();
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x0007924B File Offset: 0x0007744B
		protected override void Resume()
		{
			base.Resume();
			this.StartTransit();
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x00079259 File Offset: 0x00077459
		protected override void End()
		{
			base.End();
			this.skipPickup = false;
			this.EndTransit();
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x00077227 File Offset: 0x00075427
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x00079270 File Offset: 0x00077470
		private void StartTransit()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.Npc.Inventory.GetIdenticalItemAmount(this.itemToRetrieveTemplate) == 0)
			{
				string text;
				if (!this.IsTransitRouteValid(this.assignedRoute, this.itemToRetrieveTemplate, out text))
				{
					Console.LogWarning("Invalid transit route for move item behaviour!", null);
					base.Disable_Networked(null);
					return;
				}
			}
			else
			{
				ItemInstance firstIdenticalItem = base.Npc.Inventory.GetFirstIdenticalItem(this.itemToRetrieveTemplate, new NPCInventory.ItemFilter(this.IsNpcInventoryItemValid));
				if (base.Npc.behaviour.DEBUG_MODE)
				{
					string str = "Moving item: ";
					ItemInstance itemInstance = firstIdenticalItem;
					Console.Log(str + ((itemInstance != null) ? itemInstance.ToString() : null), null);
				}
				if (!this.IsDestinationValid(this.assignedRoute, firstIdenticalItem))
				{
					Console.LogWarning("Invalid transit route for move item behaviour!", null);
					base.Disable_Networked(null);
					return;
				}
			}
			this.currentState = MoveItemBehaviour.EState.Idle;
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x00079344 File Offset: 0x00077544
		private bool IsNpcInventoryItemValid(ItemInstance item)
		{
			return this.assignedRoute.Destination.GetInputCapacityForItem(item, base.Npc) != 0;
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x00079364 File Offset: 0x00077564
		private void EndTransit()
		{
			this.StopCurrentActivity();
			if (this.assignedRoute != null && base.Npc != null && this.assignedRoute.Destination != null)
			{
				this.assignedRoute.Destination.RemoveSlotLocks(base.Npc.NetworkObject);
			}
			this.Initialized = false;
			this.assignedRoute = null;
			this.itemToRetrieveTemplate = null;
			this.grabbedAmount = 0;
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000793D4 File Offset: 0x000775D4
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.assignedRoute.AreEntitiesNonNull())
			{
				Console.LogWarning("Transit route entities are null!", null);
				base.Disable_Networked(null);
				return;
			}
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("State: " + this.currentState.ToString(), null);
				Console.Log("Moving: " + base.Npc.Movement.IsMoving.ToString(), null);
			}
			if (this.currentState == MoveItemBehaviour.EState.Idle)
			{
				if (base.Npc.Inventory.GetIdenticalItemAmount(this.itemToRetrieveTemplate) > 0 && this.grabbedAmount > 0)
				{
					if (this.IsAtDestination())
					{
						this.PlaceItem();
						return;
					}
					this.WalkToDestination();
					return;
				}
				else
				{
					if (this.skipPickup)
					{
						this.TakeItem();
						this.skipPickup = false;
						return;
					}
					if (this.IsAtSource())
					{
						this.GrabItem();
						return;
					}
					this.WalkToSource();
				}
			}
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x000794D0 File Offset: 0x000776D0
		public void WalkToSource()
		{
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("MoveItemBehaviour.WalkToSource", null);
			}
			if (!base.Npc.Movement.CanGetTo(this.GetSourceAccessPoint(this.assignedRoute).position, 1f))
			{
				Console.LogWarning("MoveItemBehaviour.WalkToSource: Can't get to source", null);
				base.Disable_Networked(null);
				return;
			}
			this.currentState = MoveItemBehaviour.EState.WalkingToSource;
			this.walkToSourceRoutine = base.StartCoroutine(this.<WalkToSource>g__Routine|26_0());
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x00079549 File Offset: 0x00077749
		public void GrabItem()
		{
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("MoveItemBehaviour.GrabItem", null);
			}
			this.currentState = MoveItemBehaviour.EState.Grabbing;
			this.grabRoutine = base.StartCoroutine(this.<GrabItem>g__Routine|27_0());
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x0007957C File Offset: 0x0007777C
		private void TakeItem()
		{
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("MoveItemBehaviour.TakeItem", null);
			}
			int amountToGrab = this.GetAmountToGrab();
			if (amountToGrab == 0)
			{
				Console.LogWarning("Amount to grab is 0!", null);
				return;
			}
			ItemSlot firstSlotContainingTemplateItem = this.assignedRoute.Source.GetFirstSlotContainingTemplateItem(this.itemToRetrieveTemplate, ITransitEntity.ESlotType.Output);
			ItemInstance copy = ((firstSlotContainingTemplateItem != null) ? firstSlotContainingTemplateItem.ItemInstance : null).GetCopy(amountToGrab);
			this.grabbedAmount = amountToGrab;
			firstSlotContainingTemplateItem.ChangeQuantity(-amountToGrab, false);
			base.Npc.Inventory.InsertItem(copy, true);
			this.assignedRoute.Destination.ReserveInputSlotsForItem(copy, base.Npc.NetworkObject);
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x00079620 File Offset: 0x00077820
		public void WalkToDestination()
		{
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("MoveItemBehaviour.WalkToDestination", null);
			}
			if (!base.Npc.Movement.CanGetTo(this.GetDestinationAccessPoint(this.assignedRoute).position, 1f))
			{
				Console.LogWarning("MoveItemBehaviour.WalkToDestination: Can't get to destination", null);
				base.Disable_Networked(null);
				return;
			}
			this.currentState = MoveItemBehaviour.EState.WalkingToDestination;
			this.walkToDestinationRoutine = base.StartCoroutine(this.<WalkToDestination>g__Routine|29_0());
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x00079699 File Offset: 0x00077899
		public void PlaceItem()
		{
			if (base.beh.DEBUG_MODE)
			{
				Console.Log("MoveItemBehaviour.PlaceItem", null);
			}
			this.currentState = MoveItemBehaviour.EState.Placing;
			this.placingRoutine = base.StartCoroutine(this.<PlaceItem>g__Routine|30_0());
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x000796CC File Offset: 0x000778CC
		private int GetAmountToGrab()
		{
			ItemSlot firstSlotContainingTemplateItem = this.assignedRoute.Source.GetFirstSlotContainingTemplateItem(this.itemToRetrieveTemplate, ITransitEntity.ESlotType.Output);
			ItemInstance itemInstance = (firstSlotContainingTemplateItem != null) ? firstSlotContainingTemplateItem.ItemInstance : null;
			if (itemInstance == null)
			{
				return 0;
			}
			int num = itemInstance.Quantity;
			if (this.maxMoveAmount > 0)
			{
				num = Mathf.Min(this.maxMoveAmount, num);
			}
			int inputCapacityForItem = this.assignedRoute.Destination.GetInputCapacityForItem(itemInstance, base.Npc);
			return Mathf.Min(num, inputCapacityForItem);
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x00079740 File Offset: 0x00077940
		private void StopCurrentActivity()
		{
			switch (this.currentState)
			{
			case MoveItemBehaviour.EState.WalkingToSource:
				if (this.walkToSourceRoutine != null)
				{
					base.StopCoroutine(this.walkToSourceRoutine);
				}
				break;
			case MoveItemBehaviour.EState.Grabbing:
				if (this.grabRoutine != null)
				{
					base.StopCoroutine(this.grabRoutine);
				}
				break;
			case MoveItemBehaviour.EState.WalkingToDestination:
				if (this.walkToDestinationRoutine != null)
				{
					base.StopCoroutine(this.walkToDestinationRoutine);
				}
				break;
			case MoveItemBehaviour.EState.Placing:
				if (this.placingRoutine != null)
				{
					base.StopCoroutine(this.placingRoutine);
				}
				break;
			}
			this.currentState = MoveItemBehaviour.EState.Idle;
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x000797D0 File Offset: 0x000779D0
		public bool IsTransitRouteValid(TransitRoute route, string itemID, out string invalidReason)
		{
			invalidReason = string.Empty;
			if (route == null)
			{
				invalidReason = "Route is null!";
				return false;
			}
			if (!route.AreEntitiesNonNull())
			{
				invalidReason = "Entities are null!";
				return false;
			}
			ItemSlot firstSlotContainingItem = route.Source.GetFirstSlotContainingItem(itemID, ITransitEntity.ESlotType.Output);
			ItemInstance itemInstance = (firstSlotContainingItem != null) ? firstSlotContainingItem.ItemInstance : null;
			if (itemInstance == null || itemInstance.Quantity <= 0)
			{
				invalidReason = "Item is null or quantity is 0!";
				return false;
			}
			if (!this.IsDestinationValid(route, itemInstance))
			{
				invalidReason = "Can't access source, destination or destination is full!";
				return false;
			}
			return true;
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x00079844 File Offset: 0x00077A44
		public bool IsTransitRouteValid(TransitRoute route, ItemInstance templateItem, out string invalidReason)
		{
			invalidReason = string.Empty;
			if (route == null)
			{
				invalidReason = "Route is null!";
				return false;
			}
			if (!route.AreEntitiesNonNull())
			{
				invalidReason = "Entities are null!";
				return false;
			}
			ItemSlot firstSlotContainingTemplateItem = route.Source.GetFirstSlotContainingTemplateItem(templateItem, ITransitEntity.ESlotType.Output);
			ItemInstance itemInstance = (firstSlotContainingTemplateItem != null) ? firstSlotContainingTemplateItem.ItemInstance : null;
			if (itemInstance == null || itemInstance.Quantity <= 0)
			{
				invalidReason = "Item is null or quantity is 0!";
				return false;
			}
			if (!this.IsDestinationValid(route, itemInstance))
			{
				invalidReason = "Can't access source, destination or destination is full!";
				return false;
			}
			return true;
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x000798B8 File Offset: 0x00077AB8
		public bool IsTransitRouteValid(TransitRoute route, string itemID)
		{
			string text;
			return this.IsTransitRouteValid(route, itemID, out text);
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000798D0 File Offset: 0x00077AD0
		public bool IsDestinationValid(TransitRoute route, ItemInstance item)
		{
			if (route.Destination.GetInputCapacityForItem(item, base.Npc) == 0)
			{
				Console.LogWarning("Destination has no capacity for item!", null);
				return false;
			}
			if (!this.CanGetToDestination(route))
			{
				Console.LogWarning("Cannot get to destination!", null);
				return false;
			}
			if (!this.CanGetToSource(route))
			{
				Console.LogWarning("Cannot get to source!", null);
				return false;
			}
			return true;
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x0007992B File Offset: 0x00077B2B
		public bool CanGetToSource(TransitRoute route)
		{
			return this.GetSourceAccessPoint(route) != null;
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0007993A File Offset: 0x00077B3A
		private Transform GetSourceAccessPoint(TransitRoute route)
		{
			return NavMeshUtility.GetAccessPoint(route.Source, base.Npc);
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x0007994D File Offset: 0x00077B4D
		private bool IsAtSource()
		{
			return NavMeshUtility.IsAtTransitEntity(this.assignedRoute.Source, base.Npc, 0.4f);
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x0007996A File Offset: 0x00077B6A
		public bool CanGetToDestination(TransitRoute route)
		{
			return this.GetDestinationAccessPoint(route) != null;
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x00079979 File Offset: 0x00077B79
		private Transform GetDestinationAccessPoint(TransitRoute route)
		{
			if (route.Destination == null)
			{
				Console.LogWarning("Destination is null!", null);
				return null;
			}
			return NavMeshUtility.GetAccessPoint(route.Destination, base.Npc);
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x000799A4 File Offset: 0x00077BA4
		private bool IsAtDestination()
		{
			if (base.beh.DEBUG_MODE)
			{
				ITransitEntity destination = this.assignedRoute.Destination;
				Console.Log("Destination: " + destination.Name, null);
				foreach (Transform transform in destination.AccessPoints)
				{
					Debug.DrawLine(base.Npc.transform.position, transform.position, Color.red, 0.1f);
				}
			}
			return NavMeshUtility.IsAtTransitEntity(this.assignedRoute.Destination, base.Npc, 0.4f);
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x00079A3C File Offset: 0x00077C3C
		public MoveItemData GetSaveData()
		{
			if (!base.Active || this.grabbedAmount == 0)
			{
				return null;
			}
			string templateItemJson = string.Empty;
			if (this.itemToRetrieveTemplate != null)
			{
				templateItemJson = this.itemToRetrieveTemplate.GetItemData().GetJson(false);
			}
			return new MoveItemData(templateItemJson, this.grabbedAmount, (this.assignedRoute.Source as IGUIDRegisterable).GUID, (this.assignedRoute.Destination as IGUIDRegisterable).GUID);
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x00079AB4 File Offset: 0x00077CB4
		public void Load(MoveItemData moveItemData)
		{
			if (moveItemData == null)
			{
				return;
			}
			if (moveItemData.GrabbedItemQuantity == 0 || string.IsNullOrEmpty(moveItemData.TemplateItemJSON))
			{
				return;
			}
			ITransitEntity @object = GUIDManager.GetObject<ITransitEntity>(new Guid(moveItemData.SourceGUID));
			ITransitEntity object2 = GUIDManager.GetObject<ITransitEntity>(new Guid(moveItemData.DestinationGUID));
			if (@object == null)
			{
				Console.LogWarning("Failed to load source transit entity", null);
				return;
			}
			if (object2 == null)
			{
				Console.LogWarning("Failed to load destination transit entity", null);
				return;
			}
			TransitRoute route = new TransitRoute(@object, object2);
			this.grabbedAmount = moveItemData.GrabbedItemQuantity;
			Debug.Log("Resuming move item behaviour");
			ItemInstance itemInstance = ItemDeserializer.LoadItem(moveItemData.TemplateItemJSON);
			if (itemInstance != null)
			{
				this.Resume(route, itemInstance, -1);
				base.Enable_Networked(null);
			}
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x00079B67 File Offset: 0x00077D67
		[CompilerGenerated]
		private IEnumerator <WalkToSource>g__Routine|26_0()
		{
			base.SetDestination(this.GetSourceAccessPoint(this.assignedRoute).position, true);
			yield return new WaitForSeconds(0.5f);
			yield return new WaitUntil(() => !base.Npc.Movement.IsMoving);
			this.currentState = MoveItemBehaviour.EState.Idle;
			this.walkToSourceRoutine = null;
			yield break;
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x00079B8B File Offset: 0x00077D8B
		[CompilerGenerated]
		private IEnumerator <GrabItem>g__Routine|27_0()
		{
			Transform sourceAccessPoint = this.GetSourceAccessPoint(this.assignedRoute);
			if (sourceAccessPoint == null)
			{
				Console.LogWarning("Could not find source access point!", null);
				this.grabRoutine = null;
				base.Disable_Networked(null);
				yield break;
			}
			base.Npc.Movement.FaceDirection(sourceAccessPoint.forward, 0.5f);
			base.Npc.SetAnimationTrigger_Networked(null, "GrabItem");
			float seconds = 0.5f;
			yield return new WaitForSeconds(seconds);
			string str;
			if (!this.IsTransitRouteValid(this.assignedRoute, this.itemToRetrieveTemplate, out str))
			{
				Console.LogWarning("Transit route no longer valid! Reason: " + str, null);
				this.grabRoutine = null;
				base.Disable_Networked(null);
				yield break;
			}
			this.TakeItem();
			yield return new WaitForSeconds(0.5f);
			this.grabRoutine = null;
			this.currentState = MoveItemBehaviour.EState.Idle;
			yield break;
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x00079B9A File Offset: 0x00077D9A
		[CompilerGenerated]
		private IEnumerator <WalkToDestination>g__Routine|29_0()
		{
			base.SetDestination(this.GetDestinationAccessPoint(this.assignedRoute).position, true);
			yield return new WaitForSeconds(0.5f);
			yield return new WaitUntil(() => !base.Npc.Movement.IsMoving);
			this.currentState = MoveItemBehaviour.EState.Idle;
			this.walkToDestinationRoutine = null;
			yield break;
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x00079BA9 File Offset: 0x00077DA9
		[CompilerGenerated]
		private IEnumerator <PlaceItem>g__Routine|30_0()
		{
			if (this.GetDestinationAccessPoint(this.assignedRoute) != null)
			{
				base.Npc.Movement.FaceDirection(this.GetDestinationAccessPoint(this.assignedRoute).forward, 0.5f);
			}
			base.Npc.SetAnimationTrigger_Networked(null, "GrabItem");
			float seconds = 0.5f;
			yield return new WaitForSeconds(seconds);
			this.assignedRoute.Destination.RemoveSlotLocks(base.Npc.NetworkObject);
			ItemInstance firstIdenticalItem = base.Npc.Inventory.GetFirstIdenticalItem(this.itemToRetrieveTemplate, null);
			if (firstIdenticalItem != null && this.grabbedAmount > 0)
			{
				ItemInstance copy = firstIdenticalItem.GetCopy(this.grabbedAmount);
				if (this.assignedRoute.Destination.GetInputCapacityForItem(copy, base.Npc) >= this.grabbedAmount)
				{
					this.assignedRoute.Destination.InsertItemIntoInput(copy, base.Npc);
				}
				else
				{
					Console.LogWarning("Destination does not have enough capacity for item! Attempting to return item to source.", null);
					if (this.assignedRoute.Source.GetOutputCapacityForItem(copy, base.Npc) >= this.grabbedAmount)
					{
						this.assignedRoute.Source.InsertItemIntoOutput(copy, base.Npc);
					}
					else
					{
						Console.LogWarning("Source does not have enough capacity for item! Item will be lost.", null);
					}
				}
				firstIdenticalItem.ChangeQuantity(-this.grabbedAmount);
			}
			else
			{
				Console.LogWarning("Could not find carried item to place!", null);
			}
			yield return new WaitForSeconds(0.5f);
			this.placingRoutine = null;
			this.currentState = MoveItemBehaviour.EState.Idle;
			base.Disable_Networked(null);
			yield break;
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x00079BB8 File Offset: 0x00077DB8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.MoveItemBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.MoveItemBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x00079BD1 File Offset: 0x00077DD1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.MoveItemBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.MoveItemBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x00079BEA File Offset: 0x00077DEA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x00079BF8 File Offset: 0x00077DF8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017A5 RID: 6053
		private TransitRoute assignedRoute;

		// Token: 0x040017A6 RID: 6054
		private ItemInstance itemToRetrieveTemplate;

		// Token: 0x040017A7 RID: 6055
		private int grabbedAmount;

		// Token: 0x040017A8 RID: 6056
		private int maxMoveAmount = -1;

		// Token: 0x040017A9 RID: 6057
		private MoveItemBehaviour.EState currentState;

		// Token: 0x040017AA RID: 6058
		private Coroutine walkToSourceRoutine;

		// Token: 0x040017AB RID: 6059
		private Coroutine grabRoutine;

		// Token: 0x040017AC RID: 6060
		private Coroutine walkToDestinationRoutine;

		// Token: 0x040017AD RID: 6061
		private Coroutine placingRoutine;

		// Token: 0x040017AE RID: 6062
		private bool skipPickup;

		// Token: 0x040017AF RID: 6063
		private bool dll_Excuted;

		// Token: 0x040017B0 RID: 6064
		private bool dll_Excuted;

		// Token: 0x020004F0 RID: 1264
		public enum EState
		{
			// Token: 0x040017B2 RID: 6066
			Idle,
			// Token: 0x040017B3 RID: 6067
			WalkingToSource,
			// Token: 0x040017B4 RID: 6068
			Grabbing,
			// Token: 0x040017B5 RID: 6069
			WalkingToDestination,
			// Token: 0x040017B6 RID: 6070
			Placing
		}
	}
}
