using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B8E RID: 2958
	public class BrickPressHandle : MonoBehaviour
	{
		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06005006 RID: 20486 RVA: 0x0015140A File Offset: 0x0014F60A
		// (set) Token: 0x06005007 RID: 20487 RVA: 0x00151412 File Offset: 0x0014F612
		public bool Interactable { get; private set; }

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06005008 RID: 20488 RVA: 0x0015141B File Offset: 0x0014F61B
		// (set) Token: 0x06005009 RID: 20489 RVA: 0x00151423 File Offset: 0x0014F623
		public float CurrentPosition { get; private set; }

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x0600500A RID: 20490 RVA: 0x0015142C File Offset: 0x0014F62C
		// (set) Token: 0x0600500B RID: 20491 RVA: 0x00151434 File Offset: 0x0014F634
		public float TargetPosition { get; private set; }

		// Token: 0x0600500C RID: 20492 RVA: 0x00151440 File Offset: 0x0014F640
		private void Start()
		{
			this.SetPosition(0f);
			this.SetInteractable(false);
			this.HandleClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ClickStart));
			this.HandleClickable.onClickEnd.AddListener(new UnityAction(this.ClickEnd));
		}

		// Token: 0x0600500D RID: 20493 RVA: 0x00151498 File Offset: 0x0014F698
		private void LateUpdate()
		{
			if (!this.Locked)
			{
				if (this.isMoving)
				{
					Vector3 vector = this.GetPlaneHit() + this.clickOffset;
					float position = 1f - Mathf.Clamp01(Mathf.InverseLerp(Mathf.Min(this.LoweredTransform.position.y, this.RaisedTransform.position.y), Mathf.Max(this.LoweredTransform.position.y, this.RaisedTransform.position.y), vector.y));
					this.SetPosition(position);
				}
				else
				{
					this.SetPosition(Mathf.MoveTowards(this.TargetPosition, 0f, Time.deltaTime));
				}
			}
			this.Move();
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x00151558 File Offset: 0x0014F758
		private void Move()
		{
			this.CurrentPosition = Mathf.MoveTowards(this.CurrentPosition, this.TargetPosition, this.MoveSpeed * Time.deltaTime);
			base.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, 360f, this.CurrentPosition));
			if (Mathf.Abs(this.CurrentPosition - this.lastClickPosition) > 0.1666f)
			{
				this.lastClickPosition = this.CurrentPosition;
				this.ClickSound.AudioSource.pitch = Mathf.Lerp(0.7f, 1.1f, this.CurrentPosition);
				this.ClickSound.Play();
			}
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x0015160C File Offset: 0x0014F80C
		private void UpdateSound(float difference)
		{
			difference /= 0.05f;
			if (difference < 0f)
			{
				Mathf.Abs(difference);
			}
			if (difference > 0f)
			{
				Mathf.Abs(difference);
			}
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x00151635 File Offset: 0x0014F835
		public void SetPosition(float position)
		{
			this.TargetPosition = position;
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x0015163E File Offset: 0x0014F83E
		public void SetInteractable(bool e)
		{
			this.Interactable = e;
			this.HandleClickable.ClickableEnabled = e;
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x00151653 File Offset: 0x0014F853
		public void ClickStart(RaycastHit hit)
		{
			this.isMoving = true;
			this.clickOffset = this.HandleClickable.transform.position - this.GetPlaneHit();
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x0015167D File Offset: 0x0014F87D
		public void ClickEnd()
		{
			this.isMoving = false;
		}

		// Token: 0x06005014 RID: 20500 RVA: 0x00151688 File Offset: 0x0014F888
		private Vector3 GetPlaneHit()
		{
			Plane plane = new Plane(this.PlaneNormal.forward, this.PlaneNormal.position);
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x04003C39 RID: 15417
		private float lastClickPosition;

		// Token: 0x04003C3A RID: 15418
		[Header("Settings")]
		public float MoveSpeed = 1f;

		// Token: 0x04003C3B RID: 15419
		public bool Locked;

		// Token: 0x04003C3C RID: 15420
		[Header("References")]
		public Transform PlaneNormal;

		// Token: 0x04003C3D RID: 15421
		public Transform RaisedTransform;

		// Token: 0x04003C3E RID: 15422
		public Transform LoweredTransform;

		// Token: 0x04003C3F RID: 15423
		public Clickable HandleClickable;

		// Token: 0x04003C40 RID: 15424
		public AudioSourceController ClickSound;

		// Token: 0x04003C41 RID: 15425
		private Vector3 clickOffset = Vector3.zero;

		// Token: 0x04003C42 RID: 15426
		private bool isMoving;
	}
}
