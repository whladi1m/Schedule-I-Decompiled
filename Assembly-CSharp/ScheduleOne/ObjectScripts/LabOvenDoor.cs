using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BAD RID: 2989
	public class LabOvenDoor : MonoBehaviour
	{
		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x0600528D RID: 21133 RVA: 0x0015C336 File Offset: 0x0015A536
		// (set) Token: 0x0600528E RID: 21134 RVA: 0x0015C33E File Offset: 0x0015A53E
		public bool Interactable { get; private set; }

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x0600528F RID: 21135 RVA: 0x0015C347 File Offset: 0x0015A547
		// (set) Token: 0x06005290 RID: 21136 RVA: 0x0015C34F File Offset: 0x0015A54F
		public float TargetPosition { get; private set; }

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06005291 RID: 21137 RVA: 0x0015C358 File Offset: 0x0015A558
		// (set) Token: 0x06005292 RID: 21138 RVA: 0x0015C360 File Offset: 0x0015A560
		public float ActualPosition { get; private set; }

		// Token: 0x06005293 RID: 21139 RVA: 0x0015C36C File Offset: 0x0015A56C
		private void Start()
		{
			this.SetPosition(0f);
			this.SetInteractable(false);
			this.HandleClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ClickStart));
			this.HandleClickable.onClickEnd.AddListener(new UnityAction(this.ClickEnd));
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x0015C3C4 File Offset: 0x0015A5C4
		private void LateUpdate()
		{
			if (this.isMoving)
			{
				Vector3 position = this.GetPlaneHit() + this.clickOffset;
				float num = this.PlaneNormal.InverseTransformPoint(position).y;
				num = Mathf.Clamp01(Mathf.InverseLerp(-0.25f, 0.24f, num));
				this.SetPosition(this.HitMapCurve.Evaluate(num));
			}
			this.Move();
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x0015C42C File Offset: 0x0015A62C
		private void Move()
		{
			float y = Mathf.Lerp(90f, 10f, this.TargetPosition);
			Quaternion b = Quaternion.Euler(0f, y, 0f);
			this.Door.localRotation = Quaternion.Lerp(this.Door.localRotation, b, Time.deltaTime * this.DoorMoveSpeed);
			this.ActualPosition = Mathf.Lerp(this.ActualPosition, this.TargetPosition, Time.deltaTime * this.DoorMoveSpeed);
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x0015C4AB File Offset: 0x0015A6AB
		public void SetInteractable(bool interactable)
		{
			this.Interactable = interactable;
			this.HandleClickable.ClickableEnabled = interactable;
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x0015C4C0 File Offset: 0x0015A6C0
		public void SetPosition(float newPosition)
		{
			float targetPosition = this.TargetPosition;
			this.TargetPosition = newPosition;
			if (targetPosition == 0f && newPosition > 0.02f)
			{
				this.OpenSound.Play();
				return;
			}
			if (targetPosition >= 0.98f && newPosition < 0.98f)
			{
				this.CloseSound.Play();
				return;
			}
			if (targetPosition > 0.01f && newPosition <= 0.001f)
			{
				this.ShutSound.Play();
			}
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x0015C52E File Offset: 0x0015A72E
		public void ClickStart(RaycastHit hit)
		{
			this.isMoving = true;
			this.clickOffset = this.HandleClickable.transform.position - this.GetPlaneHit();
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x0015C558 File Offset: 0x0015A758
		private Vector3 GetPlaneHit()
		{
			Plane plane = new Plane(this.PlaneNormal.forward, this.PlaneNormal.position);
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x0015C5AA File Offset: 0x0015A7AA
		public void ClickEnd()
		{
			this.isMoving = false;
		}

		// Token: 0x04003DA7 RID: 15783
		public const float HIT_OFFSET_MAX = 0.24f;

		// Token: 0x04003DA8 RID: 15784
		public const float HIT_OFFSET_MIN = -0.25f;

		// Token: 0x04003DA9 RID: 15785
		public const float DOOR_ANGLE_CLOSED = 90f;

		// Token: 0x04003DAA RID: 15786
		public const float DOOR_ANGLE_OPEN = 10f;

		// Token: 0x04003DAE RID: 15790
		[Header("References")]
		public Clickable HandleClickable;

		// Token: 0x04003DAF RID: 15791
		public Transform Door;

		// Token: 0x04003DB0 RID: 15792
		public Transform PlaneNormal;

		// Token: 0x04003DB1 RID: 15793
		public AnimationCurve HitMapCurve;

		// Token: 0x04003DB2 RID: 15794
		[Header("Sounds")]
		public AudioSourceController OpenSound;

		// Token: 0x04003DB3 RID: 15795
		public AudioSourceController CloseSound;

		// Token: 0x04003DB4 RID: 15796
		public AudioSourceController ShutSound;

		// Token: 0x04003DB5 RID: 15797
		[Header("Settings")]
		public float DoorMoveSpeed = 2f;

		// Token: 0x04003DB6 RID: 15798
		private Vector3 clickOffset = Vector3.zero;

		// Token: 0x04003DB7 RID: 15799
		private bool isMoving;
	}
}
