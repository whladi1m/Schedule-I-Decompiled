using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000339 RID: 825
	public class Draggable : Clickable
	{
		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x000507C2 File Offset: 0x0004E9C2
		// (set) Token: 0x06001271 RID: 4721 RVA: 0x000507CA File Offset: 0x0004E9CA
		public Rigidbody Rb { get; protected set; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x000507D3 File Offset: 0x0004E9D3
		// (set) Token: 0x06001273 RID: 4723 RVA: 0x000507DB File Offset: 0x0004E9DB
		public override CursorManager.ECursorType HoveredCursor { get; protected set; } = CursorManager.ECursorType.OpenHand;

		// Token: 0x06001274 RID: 4724 RVA: 0x000507E4 File Offset: 0x0004E9E4
		protected virtual void Awake()
		{
			this.Rb = base.GetComponent<Rigidbody>();
			this.constraint = base.GetComponent<DraggableConstraint>();
			if (base.gameObject.isStatic)
			{
				Console.LogWarning("Draggable object is static, this will cause issues with dragging.", null);
			}
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00050818 File Offset: 0x0004EA18
		protected virtual void FixedUpdate()
		{
			if (this.Rb == null)
			{
				return;
			}
			this.Rb.drag = (base.IsHeld ? this.HeldRBDrag : this.NormalRBDrag);
			if (!base.IsHeld && !this.Rb.isKinematic)
			{
				this.Rb.angularVelocity = Vector3.ClampMagnitude(this.Rb.angularVelocity, this.Rb.angularVelocity.magnitude * 0.9f);
				this.Rb.velocity = Vector3.ClampMagnitude(this.Rb.velocity, this.Rb.velocity.magnitude * 0.95f);
				this.Rb.AddForce(Vector3.up * this.idleUpForce, ForceMode.Acceleration);
			}
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Update()
		{
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void PostFixedUpdate()
		{
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x000508F4 File Offset: 0x0004EAF4
		protected virtual void LateUpdate()
		{
			if (this.LocationRestrictionEnabled && Vector3.Distance(base.transform.position, this.Origin) > this.MaxDistanceFromOrigin)
			{
				base.transform.position = this.Origin + (base.transform.position - this.Origin).normalized * this.MaxDistanceFromOrigin;
			}
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00050966 File Offset: 0x0004EB66
		protected virtual void OnTriggerExit(Collider other)
		{
			if (this.onTriggerExit != null)
			{
				this.onTriggerExit.Invoke(other);
			}
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0005097C File Offset: 0x0004EB7C
		public override void StartClick(RaycastHit hit)
		{
			base.StartClick(hit);
			if (this.DisableGravityWhenDragged)
			{
				this.Rb.useGravity = false;
			}
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00050999 File Offset: 0x0004EB99
		public override void EndClick()
		{
			base.EndClick();
			if (this.DisableGravityWhenDragged && this.Rb != null)
			{
				this.Rb.useGravity = true;
			}
		}

		// Token: 0x040011C4 RID: 4548
		[Header("Drag Force")]
		public float DragForceMultiplier = 30f;

		// Token: 0x040011C5 RID: 4549
		public Transform DragForceOrigin;

		// Token: 0x040011C6 RID: 4550
		[Header("Rotation")]
		public bool RotationEnabled = true;

		// Token: 0x040011C7 RID: 4551
		public float TorqueMultiplier = 20f;

		// Token: 0x040011C8 RID: 4552
		[Header("Settings")]
		public Draggable.EDragProjectionMode DragProjectionMode;

		// Token: 0x040011C9 RID: 4553
		public bool DisableGravityWhenDragged;

		// Token: 0x040011CA RID: 4554
		public float NormalRBDrag = 3f;

		// Token: 0x040011CB RID: 4555
		public float HeldRBDrag = 15f;

		// Token: 0x040011CC RID: 4556
		public bool CanBeMultiDragged = true;

		// Token: 0x040011CF RID: 4559
		[Header("Additional force")]
		public float idleUpForce;

		// Token: 0x040011D0 RID: 4560
		[HideInInspector]
		public bool LocationRestrictionEnabled;

		// Token: 0x040011D1 RID: 4561
		[HideInInspector]
		public Vector3 Origin = Vector3.zero;

		// Token: 0x040011D2 RID: 4562
		[HideInInspector]
		public float MaxDistanceFromOrigin = 0.5f;

		// Token: 0x040011D3 RID: 4563
		public UnityEvent<Collider> onTriggerExit;

		// Token: 0x040011D4 RID: 4564
		protected DraggableConstraint constraint;

		// Token: 0x0200033A RID: 826
		public enum EDragProjectionMode
		{
			// Token: 0x040011D6 RID: 4566
			CameraForward,
			// Token: 0x040011D7 RID: 4567
			FlatCameraForward
		}
	}
}
