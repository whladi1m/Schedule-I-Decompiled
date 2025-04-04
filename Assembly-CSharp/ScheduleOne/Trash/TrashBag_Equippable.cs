using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.Trash
{
	// Token: 0x02000810 RID: 2064
	public class TrashBag_Equippable : Equippable_Viewmodel
	{
		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06003881 RID: 14465 RVA: 0x000EEFE8 File Offset: 0x000ED1E8
		public static bool IsHoveringTrash
		{
			get
			{
				return Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.activeSelf;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06003882 RID: 14466 RVA: 0x000EEFFE File Offset: 0x000ED1FE
		// (set) Token: 0x06003883 RID: 14467 RVA: 0x000EF006 File Offset: 0x000ED206
		public bool IsBaggingTrash { get; private set; }

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06003884 RID: 14468 RVA: 0x000EF00F File Offset: 0x000ED20F
		// (set) Token: 0x06003885 RID: 14469 RVA: 0x000EF017 File Offset: 0x000ED217
		public bool IsPickingUpTrash { get; private set; }

		// Token: 0x06003886 RID: 14470 RVA: 0x000EF020 File Offset: 0x000ED220
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(false);
			Singleton<TrashBagCanvas>.Instance.Open();
			this.PickupAreaProjector.transform.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
			this.PickupAreaProjector.transform.localScale = Vector3.one;
			this.PickupAreaProjector.transform.forward = -Vector3.up;
			this.PickupAreaProjector.gameObject.SetActive(false);
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x000EF0AD File Offset: 0x000ED2AD
		public override void Unequip()
		{
			base.Unequip();
			Singleton<TrashBagCanvas>.Instance.Close();
			UnityEngine.Object.Destroy(this.PickupAreaProjector.gameObject);
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x000EF0D0 File Offset: 0x000ED2D0
		protected override void Update()
		{
			base.Update();
			Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(false);
			TrashContainer hoveredTrashContainer = this.GetHoveredTrashContainer();
			this.PickupAreaProjector.gameObject.SetActive(false);
			if (this.IsBaggingTrash)
			{
				if (!GameInput.GetButton(GameInput.ButtonCode.Interact) || hoveredTrashContainer != this._baggedContainer)
				{
					this.StopBagTrash(false);
					return;
				}
				this._bagTrashTime += Time.deltaTime;
				Singleton<TrashBagCanvas>.Instance.InputPrompt.SetLabel("Bag trash");
				Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(true);
				Singleton<HUD>.Instance.ShowRadialIndicator(this._bagTrashTime / 1f);
				if (this._bagTrashTime >= 1f)
				{
					this.StopBagTrash(true);
				}
				return;
			}
			else if (this.IsPickingUpTrash)
			{
				List<TrashItem> list = new List<TrashItem>();
				RaycastHit hit;
				if (this.RaycastLook(out hit) && this.IsPickupLocationValid(hit))
				{
					list = this.GetTrashItemsAtPoint(hit.point);
				}
				if (!GameInput.GetButton(GameInput.ButtonCode.Interact) || list.Count == 0)
				{
					this.StopPickup(false);
					return;
				}
				this._pickupTrashTime += Time.deltaTime;
				Singleton<TrashBagCanvas>.Instance.InputPrompt.SetLabel("Bag trash");
				Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(true);
				Singleton<HUD>.Instance.ShowRadialIndicator(this._pickupTrashTime / 1f);
				this.PickupAreaProjector.transform.position = hit.point + Vector3.up * 0.1f;
				this.PickupAreaProjector.gameObject.SetActive(true);
				if (this._pickupTrashTime >= 1f)
				{
					this.StopPickup(true);
				}
				return;
			}
			else
			{
				if (hoveredTrashContainer != null && hoveredTrashContainer.CanBeBagged())
				{
					this._baggedContainer = hoveredTrashContainer;
					Singleton<TrashBagCanvas>.Instance.InputPrompt.SetLabel("Bag trash");
					Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(true);
					if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact))
					{
						this.StartBagTrash(hoveredTrashContainer);
					}
					return;
				}
				RaycastHit hit2;
				if (hoveredTrashContainer == null && this.RaycastLook(out hit2) && this.IsPickupLocationValid(hit2))
				{
					this.PickupAreaProjector.transform.position = hit2.point + Vector3.up * 0.1f;
					this.PickupAreaProjector.gameObject.SetActive(true);
					if (this.GetTrashItemsAtPoint(hit2.point).Count > 0)
					{
						this.PickupAreaProjector.fadeFactor = 0.5f;
						Singleton<TrashBagCanvas>.Instance.InputPrompt.SetLabel("Bag trash");
						Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(true);
						if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact))
						{
							this.StartPickup();
							return;
						}
					}
					else
					{
						this.PickupAreaProjector.fadeFactor = 0.05f;
					}
				}
				return;
			}
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x000EF3B0 File Offset: 0x000ED5B0
		private TrashContainer GetHoveredTrashContainer()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(2.75f, out raycastHit, Singleton<InteractionManager>.Instance.Interaction_SearchMask, true, 0f))
			{
				TrashContainer componentInParent = raycastHit.collider.GetComponentInParent<TrashContainer>();
				if (componentInParent != null)
				{
					return componentInParent;
				}
			}
			return null;
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x000EF3F9 File Offset: 0x000ED5F9
		private bool RaycastLook(out RaycastHit hit)
		{
			return PlayerSingleton<PlayerCamera>.Instance.LookRaycast(3f, out hit, this.PickupLookMask, true, 0f);
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x000EF417 File Offset: 0x000ED617
		private bool IsPickupLocationValid(RaycastHit hit)
		{
			return Vector3.Angle(hit.normal, Vector3.up) <= 5f;
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x000EF434 File Offset: 0x000ED634
		private List<TrashItem> GetTrashItemsAtPoint(Vector3 pos)
		{
			Collider[] array = Physics.OverlapSphere(pos, 0.45f, Singleton<InteractionManager>.Instance.Interaction_SearchMask, QueryTriggerInteraction.Collide);
			List<TrashItem> list = new List<TrashItem>();
			Collider[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				TrashItem componentInParent = array2[i].GetComponentInParent<TrashItem>();
				if (componentInParent != null && componentInParent.CanGoInContainer)
				{
					list.Add(componentInParent);
				}
			}
			return list;
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x000EF493 File Offset: 0x000ED693
		private void StartBagTrash(TrashContainer container)
		{
			this.IsBaggingTrash = true;
			this._bagTrashTime = 0f;
			this._baggedContainer = container;
			this.RustleSound.Play();
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x000EF4BC File Offset: 0x000ED6BC
		private void StopBagTrash(bool complete)
		{
			this.IsBaggingTrash = false;
			this._bagTrashTime = 0f;
			this.RustleSound.Stop();
			if (complete)
			{
				this._baggedContainer.BagTrash();
				this.BagSound.PlayOneShot(true);
				this.itemInstance.ChangeQuantity(-1);
			}
			this._baggedContainer = null;
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x000EF513 File Offset: 0x000ED713
		private void StartPickup()
		{
			this.IsPickingUpTrash = true;
			this._pickupTrashTime = 0f;
			this.RustleSound.Play();
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x000EF534 File Offset: 0x000ED734
		private void StopPickup(bool complete)
		{
			this.IsPickingUpTrash = false;
			this._pickupTrashTime = 0f;
			this.PickupAreaProjector.gameObject.SetActive(false);
			this.RustleSound.Stop();
			if (complete)
			{
				List<TrashItem> trashItemsAtPoint = this.GetTrashItemsAtPoint(this.PickupAreaProjector.transform.position);
				foreach (TrashItem trashItem in trashItemsAtPoint)
				{
					trashItem.DestroyTrash();
				}
				this.itemInstance.ChangeQuantity(-1);
				TrashContentData content = new TrashContentData(trashItemsAtPoint);
				NetworkSingleton<TrashManager>.Instance.CreateTrashBag(NetworkSingleton<TrashManager>.Instance.TrashBagPrefab.ID, this.PickupAreaProjector.transform.position + Vector3.up * 0.4f, Quaternion.identity, content, default(Vector3), "", false);
				this.BagSound.PlayOneShot(true);
			}
		}

		// Token: 0x0400290F RID: 10511
		public const float TRASH_CONTAINER_INTERACT_DISTANCE = 2.75f;

		// Token: 0x04002910 RID: 10512
		public const float BAG_TRASH_TIME = 1f;

		// Token: 0x04002911 RID: 10513
		public const float PICKUP_RANGE = 3f;

		// Token: 0x04002912 RID: 10514
		public const float PICKUP_AREA_RADIUS = 0.5f;

		// Token: 0x04002915 RID: 10517
		public LayerMask PickupLookMask;

		// Token: 0x04002916 RID: 10518
		[Header("References")]
		public DecalProjector PickupAreaProjector;

		// Token: 0x04002917 RID: 10519
		public AudioSourceController RustleSound;

		// Token: 0x04002918 RID: 10520
		public AudioSourceController BagSound;

		// Token: 0x04002919 RID: 10521
		private float _bagTrashTime;

		// Token: 0x0400291A RID: 10522
		private TrashContainer _baggedContainer;

		// Token: 0x0400291B RID: 10523
		private float _pickupTrashTime;
	}
}
