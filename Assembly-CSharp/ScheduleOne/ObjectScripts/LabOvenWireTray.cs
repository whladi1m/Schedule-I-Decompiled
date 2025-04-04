using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BAF RID: 2991
	public class LabOvenWireTray : MonoBehaviour
	{
		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x060052A0 RID: 21152 RVA: 0x0015C6A6 File Offset: 0x0015A8A6
		// (set) Token: 0x060052A1 RID: 21153 RVA: 0x0015C6AE File Offset: 0x0015A8AE
		public bool Interactable { get; private set; }

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x060052A2 RID: 21154 RVA: 0x0015C6B7 File Offset: 0x0015A8B7
		// (set) Token: 0x060052A3 RID: 21155 RVA: 0x0015C6BF File Offset: 0x0015A8BF
		public float TargetPosition { get; private set; }

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x060052A4 RID: 21156 RVA: 0x0015C6C8 File Offset: 0x0015A8C8
		// (set) Token: 0x060052A5 RID: 21157 RVA: 0x0015C6D0 File Offset: 0x0015A8D0
		public float ActualPosition { get; private set; }

		// Token: 0x060052A6 RID: 21158 RVA: 0x0015C6D9 File Offset: 0x0015A8D9
		private void Start()
		{
			this.SetPosition(0f);
			this.SetInteractable(false);
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x0015C6F0 File Offset: 0x0015A8F0
		private void LateUpdate()
		{
			if (this.isMoving)
			{
				Vector3 position = this.GetPlaneHit() + this.clickOffset;
				float num = this.PlaneNormal.InverseTransformPoint(position).y;
				Debug.Log("Hit offset: " + num.ToString());
				num = Mathf.Clamp01(Mathf.InverseLerp(-0.25f, 0.24f, num));
				this.TargetPosition = num;
			}
			this.Move();
			this.ClampAngle();
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x0015C768 File Offset: 0x0015A968
		private void Move()
		{
			Vector3 b = Vector3.Lerp(this.ClosedPosition.localPosition, this.OpenPosition.localPosition, this.TargetPosition);
			this.Tray.localPosition = Vector3.Lerp(this.Tray.localPosition, b, Time.deltaTime * this.MoveSpeed);
			this.ActualPosition = Mathf.Lerp(this.ActualPosition, this.TargetPosition, Time.deltaTime * this.MoveSpeed);
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x0015C7E4 File Offset: 0x0015A9E4
		private void ClampAngle()
		{
			float max = this.DoorClampCurve.Evaluate(this.OvenDoor.ActualPosition);
			this.ActualPosition = Mathf.Clamp(this.ActualPosition, 0f, max);
			Vector3 localPosition = Vector3.Lerp(this.ClosedPosition.localPosition, this.OpenPosition.localPosition, this.ActualPosition);
			this.Tray.localPosition = localPosition;
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x0015C84D File Offset: 0x0015AA4D
		public void SetInteractable(bool interactable)
		{
			this.Interactable = interactable;
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x0015C856 File Offset: 0x0015AA56
		public void SetPosition(float position)
		{
			this.TargetPosition = position;
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x0015C85F File Offset: 0x0015AA5F
		public void ClickStart(RaycastHit hit)
		{
			this.isMoving = true;
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x0015C868 File Offset: 0x0015AA68
		private Vector3 GetPlaneHit()
		{
			Plane plane = new Plane(this.PlaneNormal.forward, this.PlaneNormal.position);
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x0015C8BA File Offset: 0x0015AABA
		public void ClickEnd()
		{
			this.isMoving = false;
		}

		// Token: 0x04003DC3 RID: 15811
		public const float HIT_OFFSET_MAX = 0.24f;

		// Token: 0x04003DC4 RID: 15812
		public const float HIT_OFFSET_MIN = -0.25f;

		// Token: 0x04003DC8 RID: 15816
		[Header("References")]
		public Transform Tray;

		// Token: 0x04003DC9 RID: 15817
		public Transform PlaneNormal;

		// Token: 0x04003DCA RID: 15818
		public Transform ClosedPosition;

		// Token: 0x04003DCB RID: 15819
		public Transform OpenPosition;

		// Token: 0x04003DCC RID: 15820
		public LabOvenDoor OvenDoor;

		// Token: 0x04003DCD RID: 15821
		[Header("Settings")]
		public float MoveSpeed = 2f;

		// Token: 0x04003DCE RID: 15822
		public AnimationCurve DoorClampCurve;

		// Token: 0x04003DCF RID: 15823
		private Vector3 clickOffset = Vector3.zero;

		// Token: 0x04003DD0 RID: 15824
		private bool isMoving;
	}
}
