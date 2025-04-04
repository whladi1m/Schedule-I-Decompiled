using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dragging;
using ScheduleOne.EntityFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Storage;
using ScheduleOne.UI;
using ScheduleOne.UI.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ScheduleOne.Interaction
{
	// Token: 0x02000609 RID: 1545
	public class InteractionManager : Singleton<InteractionManager>
	{
		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x0600289D RID: 10397 RVA: 0x000A7487 File Offset: 0x000A5687
		public LayerMask Interaction_SearchMask
		{
			get
			{
				return this.interaction_SearchMask;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x000A748F File Offset: 0x000A568F
		// (set) Token: 0x0600289F RID: 10399 RVA: 0x000A7497 File Offset: 0x000A5697
		public bool CanDestroy { get; set; } = true;

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x060028A0 RID: 10400 RVA: 0x000A74A0 File Offset: 0x000A56A0
		// (set) Token: 0x060028A1 RID: 10401 RVA: 0x000A74A8 File Offset: 0x000A56A8
		public InteractableObject hoveredInteractableObject { get; protected set; }

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x000A74B1 File Offset: 0x000A56B1
		// (set) Token: 0x060028A3 RID: 10403 RVA: 0x000A74B9 File Offset: 0x000A56B9
		public InteractableObject hoveredValidInteractableObject { get; protected set; }

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x060028A4 RID: 10404 RVA: 0x000A74C2 File Offset: 0x000A56C2
		// (set) Token: 0x060028A5 RID: 10405 RVA: 0x000A74CA File Offset: 0x000A56CA
		public InteractableObject interactedObject { get; protected set; }

		// Token: 0x060028A6 RID: 10406 RVA: 0x000A74D4 File Offset: 0x000A56D4
		protected override void Start()
		{
			base.Start();
			this.LoadInteractKey();
			Settings instance = Singleton<Settings>.Instance;
			instance.onInputsApplied = (Action)Delegate.Remove(instance.onInputsApplied, new Action(this.LoadInteractKey));
			Settings instance2 = Singleton<Settings>.Instance;
			instance2.onInputsApplied = (Action)Delegate.Combine(instance2.onInputsApplied, new Action(this.LoadInteractKey));
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000A7539 File Offset: 0x000A5739
		protected override void OnDestroy()
		{
			if (Singleton<Settings>.InstanceExists)
			{
				Settings instance = Singleton<Settings>.Instance;
				instance.onInputsApplied = (Action)Delegate.Remove(instance.onInputsApplied, new Action(this.LoadInteractKey));
			}
			base.OnDestroy();
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000A7570 File Offset: 0x000A5770
		private void LoadInteractKey()
		{
			string text;
			string controlPath;
			this.InteractInput.action.GetBindingDisplayString(0, out text, out controlPath, (InputBinding.DisplayStringOptions)0);
			this.InteractKey = Singleton<InputPromptsManager>.Instance.GetDisplayNameForControlPath(controlPath);
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000A75A5 File Offset: 0x000A57A5
		protected virtual void Update()
		{
			this.timeSinceLastInteractStart += Time.deltaTime;
			if (Singleton<GameInput>.InstanceExists)
			{
				this.CheckRightClick();
			}
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000A75C8 File Offset: 0x000A57C8
		protected virtual void LateUpdate()
		{
			if (!Singleton<GameInput>.InstanceExists)
			{
				return;
			}
			this.interactionDisplayEnabledThisFrame = false;
			this.CheckHover();
			if (this.hoveredInteractableObject != null)
			{
				this.hoveredInteractableObject.Hovered();
			}
			this.CheckInteraction();
			this.interaction_Canvas.enabled = (this.interactionDisplayEnabledThisFrame || this.activeWSlabels.Count > 0);
			this.interactionDisplay_Container.gameObject.SetActive(this.interactionDisplayEnabledThisFrame);
			if (!this.interactionDisplayEnabledThisFrame)
			{
				this.tempDisplayScale = 0.75f;
			}
			for (int i = 0; i < this.activeWSlabels.Count; i++)
			{
				this.activeWSlabels[i].RefreshDisplay();
			}
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000A7680 File Offset: 0x000A5880
		protected virtual void CheckHover()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (Singleton<TaskManager>.InstanceExists && Singleton<TaskManager>.Instance.currentTask != null)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (Singleton<ObjectSelector>.InstanceExists && Singleton<ObjectSelector>.Instance.isSelecting)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (Singleton<GameplayMenu>.InstanceExists && Singleton<GameplayMenu>.Instance.IsOpen)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (PlayerSingleton<PlayerMovement>.Instance.currentVehicle != null)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount > 0)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerInventory>.Instance.equippable != null && !PlayerSingleton<PlayerInventory>.Instance.equippable.CanInteractWhenEquipped)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (Player.Local.IsSkating)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			if (NetworkSingleton<DragManager>.Instance.IsDragging)
			{
				this.hoveredInteractableObject = null;
				return;
			}
			Ray ray = default(Ray);
			EInteractionSearchType einteractionSearchType = this.interactionSearchType;
			if (einteractionSearchType != EInteractionSearchType.CameraForward)
			{
				if (einteractionSearchType != EInteractionSearchType.Mouse)
				{
					Console.LogWarning("EInteractionSearchType type not accounted for", null);
					return;
				}
				ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			}
			else
			{
				ray.origin = PlayerSingleton<PlayerCamera>.Instance.transform.position;
				ray.direction = PlayerSingleton<PlayerCamera>.Instance.transform.forward;
			}
			InteractableObject hoveredInteractableObject = this.hoveredInteractableObject;
			this.hoveredInteractableObject = null;
			RaycastHit[] array = Physics.SphereCastAll(ray, 0.075f, 5f, this.interaction_SearchMask, QueryTriggerInteraction.Collide);
			RaycastHit[] array2 = Physics.RaycastAll(ray, 5f, this.interaction_SearchMask, QueryTriggerInteraction.Collide);
			if (array.Length != 0)
			{
				Array.Sort<RaycastHit>(array, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
				List<InteractableObject> list = new List<InteractableObject>();
				Dictionary<InteractableObject, RaycastHit> objectHits = new Dictionary<InteractableObject, RaycastHit>();
				foreach (RaycastHit value in array)
				{
					InteractableObject componentInParent = value.collider.GetComponentInParent<InteractableObject>();
					if (componentInParent == null)
					{
						bool flag = false;
						foreach (RaycastHit raycastHit in array2)
						{
							if (raycastHit.collider == value.collider)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					else if (!list.Contains(componentInParent) && componentInParent != null && Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, value.point) <= componentInParent.MaxInteractionRange)
					{
						list.Add(componentInParent);
						objectHits.Add(componentInParent, value);
					}
				}
				list.Sort(delegate(InteractableObject x, InteractableObject y)
				{
					int num = y.Priority.CompareTo(x.Priority);
					if (num == 0)
					{
						return objectHits[x].distance.CompareTo(objectHits[y].distance);
					}
					return num;
				});
				for (int k = 0; k < list.Count; k++)
				{
					RaycastHit raycastHit2 = objectHits[list[k]];
					InteractableObject interactableObject = list[k];
					if (interactableObject == null)
					{
						bool flag2 = false;
						foreach (RaycastHit raycastHit3 in array2)
						{
							if (raycastHit3.collider == raycastHit2.collider)
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							break;
						}
					}
					else
					{
						if (!interactableObject.CheckAngleLimit(ray.origin))
						{
							interactableObject = null;
						}
						if (interactableObject != null && !interactableObject.enabled)
						{
							interactableObject = null;
						}
						if (interactableObject != null && Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, raycastHit2.point) <= interactableObject.MaxInteractionRange)
						{
							this.hoveredInteractableObject = interactableObject;
							if (interactableObject != hoveredInteractableObject)
							{
								this.tempDisplayScale = 1f;
								break;
							}
							break;
						}
					}
				}
			}
			if (this.DEBUG)
			{
				string str = "Hovered interactable object: ";
				InteractableObject hoveredInteractableObject2 = this.hoveredInteractableObject;
				Debug.Log(str + ((hoveredInteractableObject2 != null) ? hoveredInteractableObject2.name : null));
			}
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000A7A98 File Offset: 0x000A5C98
		protected virtual void CheckInteraction()
		{
			this.hoveredValidInteractableObject = null;
			if (this.interactedObject != null && ((this.interactedObject._interactionType == InteractableObject.EInteractionType.Key_Press && !GameInput.GetButton(GameInput.ButtonCode.Interact)) || (this.interactedObject._interactionType == InteractableObject.EInteractionType.LeftMouse_Click && !GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))))
			{
				this.interactedObject.EndInteract();
				this.interactedObject = null;
			}
			if (this.hoveredInteractableObject == null)
			{
				return;
			}
			if (this.hoveredInteractableObject._interactionState == InteractableObject.EInteractableState.Disabled)
			{
				return;
			}
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				return;
			}
			this.hoveredValidInteractableObject = this.hoveredInteractableObject;
			if (GameInput.GetButton(GameInput.ButtonCode.Interact) && this.timeSinceLastInteractStart >= InteractionManager.interactCooldown && this.hoveredInteractableObject._interactionType == InteractableObject.EInteractionType.Key_Press && (!this.hoveredInteractableObject.RequiresUniqueClick || GameInput.GetButtonDown(GameInput.ButtonCode.Interact)))
			{
				this.timeSinceLastInteractStart = 0f;
				this.hoveredInteractableObject.StartInteract();
				this.interactedObject = this.hoveredInteractableObject;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && this.timeSinceLastInteractStart >= InteractionManager.interactCooldown && this.hoveredInteractableObject._interactionType == InteractableObject.EInteractionType.LeftMouse_Click && (!this.hoveredInteractableObject.RequiresUniqueClick || GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick)))
			{
				this.timeSinceLastInteractStart = 0f;
				this.hoveredInteractableObject.StartInteract();
				this.interactedObject = this.hoveredInteractableObject;
			}
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000A7BF0 File Offset: 0x000A5DF0
		protected virtual void CheckRightClick()
		{
			bool flag = false;
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (Singleton<TaskManager>.Instance.currentTask == null && (!PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped || (PlayerSingleton<PlayerInventory>.Instance.equippable != null && PlayerSingleton<PlayerInventory>.Instance.equippable.CanInteractWhenEquipped && PlayerSingleton<PlayerInventory>.Instance.equippable.CanPickUpWhenEquipped)) && PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0 && this.CanDestroy && GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
			{
				BuildableItem hoveredBuildableItem = this.GetHoveredBuildableItem();
				this.GetHoveredPallet();
				this.GetHoveredConstructable();
				if (hoveredBuildableItem != null)
				{
					string text;
					if (hoveredBuildableItem.CanBePickedUp(out text))
					{
						if (this.itemBeingDestroyed == hoveredBuildableItem)
						{
							this.destroyTime += Time.deltaTime;
						}
						this.itemBeingDestroyed = hoveredBuildableItem;
						if (this.destroyTime >= InteractionManager.timeToDestroy)
						{
							this.itemBeingDestroyed.PickupItem();
							this.destroyTime = 0f;
						}
						flag = true;
						Singleton<HUD>.Instance.ShowRadialIndicator(this.destroyTime / InteractionManager.timeToDestroy);
					}
					else
					{
						Singleton<HUD>.Instance.CrosshairText.Show(text, new Color32(byte.MaxValue, 100, 100, byte.MaxValue));
					}
				}
			}
			if (!flag)
			{
				this.destroyTime = 0f;
			}
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000A7D4C File Offset: 0x000A5F4C
		protected virtual BuildableItem GetHoveredBuildableItem()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.rightClickRange, out raycastHit, 1 << LayerMask.NameToLayer("Default"), true, 0f))
			{
				return raycastHit.collider.GetComponentInParent<BuildableItem>();
			}
			return null;
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000A7D98 File Offset: 0x000A5F98
		protected virtual Pallet GetHoveredPallet()
		{
			LayerMask layerMask = default(LayerMask) | 1 << LayerMask.NameToLayer("Default");
			layerMask |= 1 << LayerMask.NameToLayer("Pallet");
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.rightClickRange, out raycastHit, layerMask, true, 0f))
			{
				return raycastHit.collider.GetComponentInParent<Pallet>();
			}
			return null;
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000A7E10 File Offset: 0x000A6010
		protected virtual Constructable GetHoveredConstructable()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.rightClickRange, out raycastHit, 1 << LayerMask.NameToLayer("Default"), true, 0f))
			{
				return raycastHit.collider.GetComponentInParent<Constructable>();
			}
			return null;
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000A7E59 File Offset: 0x000A6059
		public void SetCanDestroy(bool canDestroy)
		{
			this.CanDestroy = canDestroy;
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x000A7E64 File Offset: 0x000A6064
		public void EnableInteractionDisplay(Vector3 pos, Sprite icon, string spriteText, string message, Color messageColor, Color iconColor)
		{
			this.interactionDisplayEnabledThisFrame = true;
			this.interactionDisplay_Container.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(pos);
			this.interactionDisplay_Icon.gameObject.SetActive(icon != null);
			this.interactionDisplay_Icon.sprite = icon;
			this.interactionDisplay_Icon.color = iconColor;
			this.interactionDisplay_IconText.enabled = (spriteText != string.Empty);
			this.interactionDisplay_IconText.text = spriteText.ToUpper();
			this.interactionDisplay_MessageText.text = message;
			this.interactionDisplay_MessageText.color = messageColor;
			this.interactionDisplay_Container.sizeDelta = new Vector2(60f + this.interactionDisplay_MessageText.preferredWidth, this.interactionDisplay_Container.sizeDelta.y);
			this.backgroundImage.sizeDelta = new Vector2(this.interactionDisplay_MessageText.preferredWidth + 180f, 140f);
			float num = Mathf.Clamp(1f / Vector3.Distance(pos, PlayerSingleton<PlayerCamera>.Instance.transform.position), 0f, 1f) * this.tempDisplayScale * this.displaySizeMultiplier;
			this.interactionDisplay_Container.localScale = new Vector3(num, num, 1f);
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x000A7FAB File Offset: 0x000A61AB
		public void LerpDisplayScale(float endScale)
		{
			if (this.ILerpDisplayScale_Coroutine != null)
			{
				base.StopCoroutine(this.ILerpDisplayScale_Coroutine);
			}
			this.ILerpDisplayScale_Coroutine = base.StartCoroutine(this.ILerpDisplayScale(this.tempDisplayScale, endScale));
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000A7FDA File Offset: 0x000A61DA
		protected IEnumerator ILerpDisplayScale(float startScale, float endScale)
		{
			float lerpTime = Mathf.Abs(startScale - endScale) * 0.75f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.tempDisplayScale = Mathf.Lerp(startScale, endScale, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.tempDisplayScale = endScale;
			this.ILerpDisplayScale_Coroutine = null;
			yield break;
		}

		// Token: 0x04001DC6 RID: 7622
		public const float RayRadius = 0.075f;

		// Token: 0x04001DC7 RID: 7623
		public const float MaxInteractionRange = 5f;

		// Token: 0x04001DC8 RID: 7624
		[SerializeField]
		protected LayerMask interaction_SearchMask;

		// Token: 0x04001DC9 RID: 7625
		[SerializeField]
		protected float rightClickRange = 5f;

		// Token: 0x04001DCA RID: 7626
		public EInteractionSearchType interactionSearchType;

		// Token: 0x04001DCB RID: 7627
		public bool DEBUG;

		// Token: 0x04001DCD RID: 7629
		[Header("Visuals Settings")]
		public Color messageColor_Default;

		// Token: 0x04001DCE RID: 7630
		public Color iconColor_Default;

		// Token: 0x04001DCF RID: 7631
		public Color iconColor_Default_Key;

		// Token: 0x04001DD0 RID: 7632
		public Color messageColor_Invalid;

		// Token: 0x04001DD1 RID: 7633
		public Color iconColor_Invalid;

		// Token: 0x04001DD2 RID: 7634
		public Sprite icon_Key;

		// Token: 0x04001DD3 RID: 7635
		public Sprite icon_LeftMouse;

		// Token: 0x04001DD4 RID: 7636
		public Sprite icon_Cross;

		// Token: 0x04001DD5 RID: 7637
		public float displaySizeMultiplier = 1f;

		// Token: 0x04001DD6 RID: 7638
		[Header("References")]
		[SerializeField]
		protected Canvas interaction_Canvas;

		// Token: 0x04001DD7 RID: 7639
		[SerializeField]
		protected RectTransform interactionDisplay_Container;

		// Token: 0x04001DD8 RID: 7640
		[SerializeField]
		protected Image interactionDisplay_Icon;

		// Token: 0x04001DD9 RID: 7641
		[SerializeField]
		protected Text interactionDisplay_IconText;

		// Token: 0x04001DDA RID: 7642
		[SerializeField]
		protected Text interactionDisplay_MessageText;

		// Token: 0x04001DDB RID: 7643
		public RectTransform wsLabelContainer;

		// Token: 0x04001DDC RID: 7644
		[SerializeField]
		protected InputActionReference InteractInput;

		// Token: 0x04001DDD RID: 7645
		[HideInInspector]
		public string InteractKey = string.Empty;

		// Token: 0x04001DDE RID: 7646
		[SerializeField]
		protected RectTransform backgroundImage;

		// Token: 0x04001DDF RID: 7647
		[Header("Prefabs")]
		public GameObject WSLabelPrefab;

		// Token: 0x04001DE3 RID: 7651
		private bool interactionDisplayEnabledThisFrame;

		// Token: 0x04001DE4 RID: 7652
		private BuildableItem itemBeingDestroyed;

		// Token: 0x04001DE5 RID: 7653
		private Pallet palletBeingDestroyed;

		// Token: 0x04001DE6 RID: 7654
		private Constructable constructableBeingDestroyed;

		// Token: 0x04001DE7 RID: 7655
		private float destroyTime;

		// Token: 0x04001DE8 RID: 7656
		private float tempDisplayScale = 0.75f;

		// Token: 0x04001DE9 RID: 7657
		public static float interactCooldown = 0.1f;

		// Token: 0x04001DEA RID: 7658
		private float timeSinceLastInteractStart;

		// Token: 0x04001DEB RID: 7659
		public List<WorldSpaceLabel> activeWSlabels = new List<WorldSpaceLabel>();

		// Token: 0x04001DEC RID: 7660
		private static float timeToDestroy = 0.5f;

		// Token: 0x04001DED RID: 7661
		private Coroutine ILerpDisplayScale_Coroutine;
	}
}
