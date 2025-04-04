using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Dragging
{
	// Token: 0x02000670 RID: 1648
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(InteractableObject))]
	public class Draggable : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002D96 RID: 11670 RVA: 0x000BF20B File Offset: 0x000BD40B
		public bool IsBeingDragged
		{
			get
			{
				return this.CurrentDragger != null;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002D97 RID: 11671 RVA: 0x000BF219 File Offset: 0x000BD419
		// (set) Token: 0x06002D98 RID: 11672 RVA: 0x000BF221 File Offset: 0x000BD421
		public Player CurrentDragger { get; protected set; }

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002D99 RID: 11673 RVA: 0x000BF22A File Offset: 0x000BD42A
		// (set) Token: 0x06002D9A RID: 11674 RVA: 0x000BF232 File Offset: 0x000BD432
		public Guid GUID { get; protected set; }

		// Token: 0x06002D9B RID: 11675 RVA: 0x000BF23C File Offset: 0x000BD43C
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002D9C RID: 11676 RVA: 0x000BF262 File Offset: 0x000BD462
		// (set) Token: 0x06002D9D RID: 11677 RVA: 0x000BF26A File Offset: 0x000BD46A
		public Vector3 initialPosition { get; private set; }

		// Token: 0x06002D9E RID: 11678 RVA: 0x000BF274 File Offset: 0x000BD474
		protected virtual void Awake()
		{
			this.IntObj.MaxInteractionRange = 2.5f;
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			this.IntObj.SetMessage("Pick up");
			this.initialPosition = base.transform.position;
			if (this.CreateCoM)
			{
				Transform transform = new GameObject("CenterOfMass").transform;
				transform.SetParent(base.transform);
				transform.localPosition = this.Rigidbody.centerOfMass;
				this.IntObj.displayLocationPoint = transform;
				this.DragOrigin = transform;
			}
			if (!string.IsNullOrEmpty(this.BakedGUID))
			{
				this.GUID = new Guid(this.BakedGUID);
				GUIDManager.RegisterObject(this);
			}
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000BF358 File Offset: 0x000BD558
		protected virtual void Start()
		{
			NetworkSingleton<DragManager>.Instance.RegisterDraggable(this);
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000BF365 File Offset: 0x000BD565
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000BF374 File Offset: 0x000BD574
		protected void OnValidate()
		{
			if (this.IntObj == null)
			{
				this.IntObj = base.GetComponent<InteractableObject>();
			}
			if (this.Rigidbody == null)
			{
				this.Rigidbody = base.GetComponent<Rigidbody>();
			}
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000BF3AA File Offset: 0x000BD5AA
		protected void OnDestroy()
		{
			if (NetworkSingleton<DragManager>.InstanceExists)
			{
				if (this.IsBeingDragged && NetworkSingleton<DragManager>.Instance.CurrentDraggable == this)
				{
					NetworkSingleton<DragManager>.Instance.StopDragging(Vector3.zero);
				}
				NetworkSingleton<DragManager>.Instance.Deregister(this);
			}
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000BF3E8 File Offset: 0x000BD5E8
		private void FixedUpdate()
		{
			if (this.IsBeingDragged)
			{
				this.timeSinceLastDrag = 0f;
			}
			else if (this.timeSinceLastDrag < 1f)
			{
				this.timeSinceLastDrag += Time.fixedDeltaTime;
			}
			if (this.IsBeingDragged && this.CurrentDragger != Player.Local)
			{
				Vector3 targetPosition = this.CurrentDragger.MimicCamera.position + this.CurrentDragger.MimicCamera.forward * 1.25f * this.HoldDistanceMultiplier;
				this.ApplyDragForces(targetPosition);
			}
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000BF488 File Offset: 0x000BD688
		public void ApplyDragForces(Vector3 targetPosition)
		{
			Vector3 vector = targetPosition - base.transform.position;
			if (this.DragOrigin != null)
			{
				vector = targetPosition - this.DragOrigin.position;
			}
			float magnitude = vector.magnitude;
			Vector3 a = vector.normalized * NetworkSingleton<DragManager>.Instance.DragForce * magnitude;
			a -= this.Rigidbody.velocity * NetworkSingleton<DragManager>.Instance.DampingFactor;
			this.Rigidbody.AddForce(a * this.DragForceMultiplier, ForceMode.Acceleration);
			Vector3 a2 = Vector3.Cross(base.transform.up, Vector3.up);
			a2 -= this.Rigidbody.angularVelocity * NetworkSingleton<DragManager>.Instance.TorqueDampingFactor;
			this.Rigidbody.AddTorque(a2 * NetworkSingleton<DragManager>.Instance.TorqueForce, ForceMode.Acceleration);
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000BF578 File Offset: 0x000BD778
		protected virtual void Hovered()
		{
			if (this.CanInteract() && base.enabled)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.IntObj.SetMessage("Pick up");
			}
			else
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			}
			if (this.onHovered != null)
			{
				this.onHovered.Invoke();
			}
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000BF5D2 File Offset: 0x000BD7D2
		protected virtual void Interacted()
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.onInteracted != null)
			{
				this.onInteracted.Invoke();
			}
			if (!this.CanInteract())
			{
				return;
			}
			NetworkSingleton<DragManager>.Instance.StartDragging(this);
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000BF604 File Offset: 0x000BD804
		private bool CanInteract()
		{
			return !this.IsBeingDragged && this.timeSinceLastDrag >= 0.1f && !NetworkSingleton<DragManager>.Instance.IsDragging && NetworkSingleton<DragManager>.Instance.IsDraggingAllowed();
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000BF63C File Offset: 0x000BD83C
		public void StartDragging(Player dragger)
		{
			if (this.IsBeingDragged)
			{
				return;
			}
			this.CurrentDragger = dragger;
			this.Rigidbody.useGravity = false;
			if (this.onDragStart != null)
			{
				this.onDragStart.Invoke();
			}
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000BF66D File Offset: 0x000BD86D
		public void StopDragging()
		{
			if (!this.IsBeingDragged)
			{
				return;
			}
			this.CurrentDragger = null;
			this.Rigidbody.useGravity = true;
			if (this.onDragEnd != null)
			{
				this.onDragEnd.Invoke();
			}
		}

		// Token: 0x04002077 RID: 8311
		public const float INITIAL_REPLICATION_DISTANCE = 1f;

		// Token: 0x04002078 RID: 8312
		public const float MAX_DRAG_START_RANGE = 2.5f;

		// Token: 0x04002079 RID: 8313
		public const float MAX_TARGET_OFFSET = 1.5f;

		// Token: 0x0400207C RID: 8316
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x0400207D RID: 8317
		[Header("References")]
		public Rigidbody Rigidbody;

		// Token: 0x0400207E RID: 8318
		public InteractableObject IntObj;

		// Token: 0x0400207F RID: 8319
		public Transform DragOrigin;

		// Token: 0x04002080 RID: 8320
		[Header("Settings")]
		public bool CreateCoM = true;

		// Token: 0x04002081 RID: 8321
		[Range(0.5f, 2f)]
		public float HoldDistanceMultiplier = 1f;

		// Token: 0x04002082 RID: 8322
		[Range(0f, 5f)]
		public float DragForceMultiplier = 1f;

		// Token: 0x04002083 RID: 8323
		public Draggable.EInitialReplicationMode InitialReplicationMode;

		// Token: 0x04002084 RID: 8324
		private float timeSinceLastDrag = 100f;

		// Token: 0x04002085 RID: 8325
		public UnityEvent onDragStart;

		// Token: 0x04002086 RID: 8326
		public UnityEvent onDragEnd;

		// Token: 0x04002087 RID: 8327
		public UnityEvent onHovered;

		// Token: 0x04002088 RID: 8328
		public UnityEvent onInteracted;

		// Token: 0x02000671 RID: 1649
		public enum EInitialReplicationMode
		{
			// Token: 0x0400208B RID: 8331
			Off,
			// Token: 0x0400208C RID: 8332
			OnlyIfMoved,
			// Token: 0x0400208D RID: 8333
			Full
		}
	}
}
