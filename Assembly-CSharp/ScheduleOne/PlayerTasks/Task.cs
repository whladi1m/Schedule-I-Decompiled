using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.UI.Compass;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200033F RID: 831
	public class Task
	{
		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600129B RID: 4763 RVA: 0x0005152D File Offset: 0x0004F72D
		// (set) Token: 0x0600129C RID: 4764 RVA: 0x00051535 File Offset: 0x0004F735
		public virtual string TaskName { get; protected set; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x0600129D RID: 4765 RVA: 0x0005153E File Offset: 0x0004F73E
		// (set) Token: 0x0600129E RID: 4766 RVA: 0x00051546 File Offset: 0x0004F746
		public string CurrentInstruction { get; protected set; } = string.Empty;

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x0005154F File Offset: 0x0004F74F
		// (set) Token: 0x060012A0 RID: 4768 RVA: 0x00051557 File Offset: 0x0004F757
		public bool TaskActive { get; private set; }

		// Token: 0x060012A1 RID: 4769 RVA: 0x00051560 File Offset: 0x0004F760
		public Task()
		{
			this.TaskActive = true;
			Singleton<TaskManager>.Instance.StartTask(this);
			Singleton<CompassManager>.Instance.SetVisible(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(this.TaskName);
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x000515E0 File Offset: 0x0004F7E0
		public virtual void StopTask()
		{
			Singleton<TaskManager>.Instance.currentTask = null;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(this.TaskName);
			Singleton<CompassManager>.Instance.SetVisible(true);
			Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
			this.TaskActive = false;
			if (this.clickable != null)
			{
				this.clickable.EndClick();
			}
			if (this.onTaskStop != null)
			{
				this.onTaskStop();
			}
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00051651 File Offset: 0x0004F851
		public virtual void Success()
		{
			this.Outcome = Task.EOutcome.Success;
			this.StopTask();
			Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
			if (this.onTaskSuccess != null)
			{
				this.onTaskSuccess();
			}
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0005167D File Offset: 0x0004F87D
		public virtual void Fail()
		{
			this.Outcome = Task.EOutcome.Fail;
			this.StopTask();
			if (this.onTaskFail != null)
			{
				this.onTaskFail();
			}
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x000516A0 File Offset: 0x0004F8A0
		public virtual void Update()
		{
			if (this.ClickDetectionEnabled && !this.isMultiDragging)
			{
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
				{
					RaycastHit hit;
					this.clickable = this.GetClickable(out hit);
					if (this.clickable != null)
					{
						this.clickable.StartClick(hit);
					}
					if (this.clickable is Draggable)
					{
						this.draggable = (this.clickable as Draggable);
						this.constraint = this.draggable.GetComponent<DraggableConstraint>();
					}
				}
				if (this.clickable != null && (!GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) || !this.clickable.ClickableEnabled) && !this.forcedClickables.Contains(this.clickable))
				{
					this.clickable.EndClick();
					this.clickable = null;
					this.draggable = null;
				}
			}
			else if (this.clickable != null)
			{
				this.clickable.EndClick();
				this.clickable = null;
			}
			this.UpdateCursor();
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0005179C File Offset: 0x0004F99C
		protected virtual void UpdateCursor()
		{
			if (this.draggable != null || this.isMultiDragging)
			{
				Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Grab);
				return;
			}
			RaycastHit raycastHit;
			Clickable clickable = this.GetClickable(out raycastHit);
			if (clickable != null)
			{
				Singleton<CursorManager>.Instance.SetCursorAppearance(clickable.HoveredCursor);
				return;
			}
			Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x000517FC File Offset: 0x0004F9FC
		public virtual void LateUpdate()
		{
			if (this.isMultiDragging)
			{
				Singleton<TaskManagerUI>.Instance.multiGrabIndicator.position = Input.mousePosition;
				Vector3 multiDragOrigin = this.GetMultiDragOrigin();
				Vector3 a = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(multiDragOrigin);
				Vector3 position = multiDragOrigin + PlayerSingleton<PlayerCamera>.Instance.transform.right * this.MultiGrabRadius;
				Vector3 b = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(position);
				float num = Vector3.Distance(a, b) / Singleton<TaskManagerUI>.Instance.canvas.scaleFactor;
				Singleton<TaskManagerUI>.Instance.multiGrabIndicator.sizeDelta = new Vector2(num * 2f, num * 2f);
				Singleton<TaskManagerUI>.Instance.multiGrabIndicator.gameObject.SetActive(true);
				return;
			}
			Singleton<TaskManagerUI>.Instance.multiGrabIndicator.gameObject.SetActive(false);
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x000518D8 File Offset: 0x0004FAD8
		private Vector3 GetMultiDragOrigin()
		{
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			Plane plane = new Plane(this.multiGrabProjectionPlane.forward, this.multiGrabProjectionPlane.position);
			float num;
			plane.Raycast(ray, out num);
			LayerMask layerMask = default(LayerMask) | 1 << LayerMask.NameToLayer("Default");
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(num, out raycastHit, layerMask, false, 0f))
			{
				return raycastHit.point;
			}
			return ray.GetPoint(num);
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0005196C File Offset: 0x0004FB6C
		public virtual void FixedUpdate()
		{
			this.UpdateDraggablePhysics();
			if (this.ClickDetectionEnabled && this.multiDraggingEnabled && this.multiGrabProjectionPlane != null && GameInput.GetButton(GameInput.ButtonCode.SecondaryClick) && this.draggable == null)
			{
				this.isMultiDragging = true;
				Vector3 multiDragOrigin = this.GetMultiDragOrigin();
				Collider[] array = Physics.OverlapSphere(multiDragOrigin, this.MultiGrabRadius, LayerMask.GetMask(new string[]
				{
					"Task"
				}));
				List<Draggable> list = new List<Draggable>();
				Collider[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					Draggable componentInParent = array2[i].GetComponentInParent<Draggable>();
					if (componentInParent != null && componentInParent.ClickableEnabled && componentInParent.CanBeMultiDragged)
					{
						list.Add(componentInParent);
					}
				}
				foreach (Draggable draggable in list)
				{
					if (!this.multiDragTargets.Contains(draggable))
					{
						this.multiDragTargets.Add(draggable);
						draggable.StartClick(default(RaycastHit));
						draggable.Rb.useGravity = false;
					}
					Vector3 force = (multiDragOrigin - draggable.transform.position) * 10f * draggable.DragForceMultiplier * 1.25f;
					draggable.Rb.AddForce(force, ForceMode.Acceleration);
				}
				foreach (Draggable draggable2 in this.multiDragTargets.ToArray())
				{
					if (!list.Contains(draggable2))
					{
						this.multiDragTargets.Remove(draggable2);
						draggable2.EndClick();
						draggable2.Rb.useGravity = true;
					}
				}
				return;
			}
			this.isMultiDragging = false;
			foreach (Draggable draggable3 in this.multiDragTargets.ToArray())
			{
				this.multiDragTargets.Remove(draggable3);
				draggable3.EndClick();
				draggable3.Rb.useGravity = true;
			}
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00051B8C File Offset: 0x0004FD8C
		public void ForceStartClick(Clickable _clickable)
		{
			if (!this.forcedClickables.Contains(_clickable))
			{
				this.forcedClickables.Add(_clickable);
			}
			_clickable.StartClick(default(RaycastHit));
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00051BC2 File Offset: 0x0004FDC2
		public void ForceEndClick(Clickable _clickable)
		{
			if (_clickable != null)
			{
				_clickable.EndClick();
				this.forcedClickables.Remove(_clickable);
			}
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00051BE0 File Offset: 0x0004FDE0
		private void UpdateDraggablePhysics()
		{
			if (this.draggable != null)
			{
				Vector3 normalized = Vector3.ProjectOnPlane(PlayerSingleton<PlayerCamera>.Instance.Camera.transform.forward, Vector3.up).normalized;
				Vector3 inNormal = (this.draggable.DragProjectionMode == Draggable.EDragProjectionMode.CameraForward) ? PlayerSingleton<PlayerCamera>.Instance.transform.forward : normalized;
				if (this.constraint != null && this.constraint.ProportionalZClamp)
				{
					inNormal = this.constraint.Container.forward;
				}
				Plane plane = new Plane(inNormal, this.draggable.originalHitPoint);
				Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
				float distance;
				plane.Raycast(ray, out distance);
				Vector3 force = (ray.GetPoint(distance) - this.draggable.transform.TransformPoint(this.relativeHitOffset)) * 10f * this.draggable.DragForceMultiplier;
				if (this.draggable.DragForceOrigin != null)
				{
					this.draggable.Rb.AddForceAtPosition(force, this.draggable.DragForceOrigin.position, ForceMode.Acceleration);
				}
				else
				{
					this.draggable.Rb.AddForce(force, ForceMode.Acceleration);
				}
				if (this.draggable.RotationEnabled)
				{
					float x = GameInput.MotionAxis.x;
					Vector3 a = normalized;
					this.draggable.Rb.AddTorque(a * -x * this.draggable.TorqueMultiplier, ForceMode.Acceleration);
				}
				this.draggable.PostFixedUpdate();
			}
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00051D84 File Offset: 0x0004FF84
		protected virtual Clickable GetClickable(out RaycastHit hit)
		{
			LayerMask layerMask = default(LayerMask) | 1 << LayerMask.NameToLayer("Task");
			layerMask |= 1 << LayerMask.NameToLayer("Temporary");
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(3f, out hit, layerMask, true, this.ClickDetectionRadius))
			{
				Clickable componentInParent = hit.collider.GetComponentInParent<Clickable>();
				if (componentInParent != null)
				{
					if (!componentInParent.enabled)
					{
						return null;
					}
					if (!componentInParent.ClickableEnabled)
					{
						return null;
					}
					if (componentInParent.IsHeld)
					{
						return null;
					}
					this.hitDistance = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, hit.point);
					componentInParent.SetOriginalHitPoint(hit.point);
					if (componentInParent.AutoCalculateOffset)
					{
						this.relativeHitOffset = componentInParent.transform.InverseTransformPoint(hit.point);
						if (componentInParent.FlattenZOffset)
						{
							this.relativeHitOffset.z = 0f;
						}
					}
					else
					{
						this.relativeHitOffset = Vector3.zero;
					}
				}
				return componentInParent;
			}
			return null;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00051E97 File Offset: 0x00050097
		protected void EnableMultiDragging(Transform projectionPlane, float radius = 0.08f)
		{
			this.multiDraggingEnabled = true;
			this.multiGrabProjectionPlane = projectionPlane;
			this.MultiGrabRadius = radius;
		}

		// Token: 0x040011FF RID: 4607
		public const float ClickDetectionRange = 3f;

		// Token: 0x04001200 RID: 4608
		public float ClickDetectionRadius;

		// Token: 0x04001201 RID: 4609
		protected float MultiGrabRadius = 0.08f;

		// Token: 0x04001202 RID: 4610
		public const float MultiGrabForceMultiplier = 1.25f;

		// Token: 0x04001206 RID: 4614
		public bool ClickDetectionEnabled = true;

		// Token: 0x04001207 RID: 4615
		public Task.EOutcome Outcome;

		// Token: 0x04001208 RID: 4616
		public Action onTaskSuccess;

		// Token: 0x04001209 RID: 4617
		public Action onTaskFail;

		// Token: 0x0400120A RID: 4618
		public Action onTaskStop;

		// Token: 0x0400120B RID: 4619
		protected Clickable clickable;

		// Token: 0x0400120C RID: 4620
		protected Draggable draggable;

		// Token: 0x0400120D RID: 4621
		protected DraggableConstraint constraint;

		// Token: 0x0400120E RID: 4622
		protected float hitDistance;

		// Token: 0x0400120F RID: 4623
		protected Vector3 relativeHitOffset = Vector3.zero;

		// Token: 0x04001210 RID: 4624
		private bool multiDraggingEnabled;

		// Token: 0x04001211 RID: 4625
		private Transform multiGrabProjectionPlane;

		// Token: 0x04001212 RID: 4626
		private List<Draggable> multiDragTargets = new List<Draggable>();

		// Token: 0x04001213 RID: 4627
		private bool isMultiDragging;

		// Token: 0x04001214 RID: 4628
		private List<Clickable> forcedClickables = new List<Clickable>();

		// Token: 0x02000340 RID: 832
		public enum EOutcome
		{
			// Token: 0x04001216 RID: 4630
			Cancelled,
			// Token: 0x04001217 RID: 4631
			Success,
			// Token: 0x04001218 RID: 4632
			Fail
		}
	}
}
