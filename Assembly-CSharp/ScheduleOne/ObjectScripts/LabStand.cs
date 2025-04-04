using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B9E RID: 2974
	public class LabStand : MonoBehaviour
	{
		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06005141 RID: 20801 RVA: 0x001569AE File Offset: 0x00154BAE
		// (set) Token: 0x06005142 RID: 20802 RVA: 0x001569B6 File Offset: 0x00154BB6
		public bool Interactable { get; private set; }

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06005143 RID: 20803 RVA: 0x001569BF File Offset: 0x00154BBF
		// (set) Token: 0x06005144 RID: 20804 RVA: 0x001569C7 File Offset: 0x00154BC7
		public float CurrentPosition { get; private set; } = 1f;

		// Token: 0x06005145 RID: 20805 RVA: 0x001569D0 File Offset: 0x00154BD0
		private void Start()
		{
			this.SetPosition(1f);
			this.SetInteractable(false);
			this.HandleClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ClickStart));
			this.HandleClickable.onClickEnd.AddListener(new UnityAction(this.ClickEnd));
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x00156A28 File Offset: 0x00154C28
		private void LateUpdate()
		{
			if (this.isMoving)
			{
				Vector3 vector = this.GetPlaneHit() + this.clickOffset;
				float position = Mathf.Clamp01(Mathf.InverseLerp(Mathf.Min(this.LoweredTransform.position.y, this.RaisedTransform.position.y), Mathf.Max(this.LoweredTransform.position.y, this.RaisedTransform.position.y), vector.y));
				this.SetPosition(position);
			}
			this.Highlight.gameObject.SetActive(this.Interactable && !this.isMoving);
			this.Move();
			this.Funnel.gameObject.SetActive(this.FunnelEnabled && this.CurrentPosition < this.FunnelThreshold);
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x00156B04 File Offset: 0x00154D04
		private void Move()
		{
			float y = this.GripTransform.localPosition.y;
			Vector3 b = Vector3.Lerp(this.LoweredTransform.localPosition, this.RaisedTransform.localPosition, this.CurrentPosition);
			Quaternion b2 = Quaternion.Lerp(this.LoweredTransform.localRotation, this.RaisedTransform.localRotation, this.CurrentPosition);
			this.GripTransform.localPosition = Vector3.Lerp(this.GripTransform.localPosition, b, Time.deltaTime * this.MoveSpeed);
			this.GripTransform.localRotation = Quaternion.Lerp(this.GripTransform.localRotation, b2, Time.deltaTime * this.MoveSpeed);
			float num = this.GripTransform.localPosition.y - y;
			this.SpinnyThingy.Rotate(Vector3.up, num * 1800f, Space.Self);
			this.UpdateSound(num);
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x00156BE8 File Offset: 0x00154DE8
		private void UpdateSound(float difference)
		{
			difference /= 0.05f;
			float num = 0f;
			if (difference < 0f)
			{
				num = Mathf.Abs(difference);
			}
			float num2 = 0f;
			if (difference > 0f)
			{
				num2 = Mathf.Abs(difference);
			}
			this.LowerSound.VolumeMultiplier = num;
			this.RaiseSound.VolumeMultiplier = num2;
			if (num > 0f && !this.LowerSound.AudioSource.isPlaying)
			{
				this.LowerSound.Play();
			}
			else if (num == 0f)
			{
				this.LowerSound.Stop();
			}
			if (num2 > 0f && !this.RaiseSound.AudioSource.isPlaying)
			{
				this.RaiseSound.Play();
				return;
			}
			if (num2 == 0f)
			{
				this.RaiseSound.Stop();
			}
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x00156CB3 File Offset: 0x00154EB3
		public void SetPosition(float position)
		{
			this.CurrentPosition = position;
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x00156CBC File Offset: 0x00154EBC
		public void SetInteractable(bool e)
		{
			this.Interactable = e;
			this.HandleClickable.ClickableEnabled = e;
			if (this.Interactable)
			{
				this.Anim.Play();
				return;
			}
			this.Anim.Stop();
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x00156CF1 File Offset: 0x00154EF1
		public void ClickStart(RaycastHit hit)
		{
			this.isMoving = true;
			this.clickOffset = this.HandleClickable.transform.position - this.GetPlaneHit();
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x00156D1C File Offset: 0x00154F1C
		private Vector3 GetPlaneHit()
		{
			Plane plane = new Plane(this.PlaneNormal.forward, this.PlaneNormal.position);
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x00156D6E File Offset: 0x00154F6E
		public void ClickEnd()
		{
			this.isMoving = false;
		}

		// Token: 0x04003CF4 RID: 15604
		[Header("Settings")]
		public float MoveSpeed = 1f;

		// Token: 0x04003CF5 RID: 15605
		public bool FunnelEnabled;

		// Token: 0x04003CF6 RID: 15606
		public float FunnelThreshold = 0.05f;

		// Token: 0x04003CF7 RID: 15607
		[Header("References")]
		public Animation Anim;

		// Token: 0x04003CF8 RID: 15608
		public Transform GripTransform;

		// Token: 0x04003CF9 RID: 15609
		public Transform SpinnyThingy;

		// Token: 0x04003CFA RID: 15610
		public Transform RaisedTransform;

		// Token: 0x04003CFB RID: 15611
		public Transform LoweredTransform;

		// Token: 0x04003CFC RID: 15612
		public Transform PlaneNormal;

		// Token: 0x04003CFD RID: 15613
		public Clickable HandleClickable;

		// Token: 0x04003CFE RID: 15614
		public Transform Funnel;

		// Token: 0x04003CFF RID: 15615
		public GameObject Highlight;

		// Token: 0x04003D00 RID: 15616
		public AudioSourceController LowerSound;

		// Token: 0x04003D01 RID: 15617
		public AudioSourceController RaiseSound;

		// Token: 0x04003D02 RID: 15618
		private Vector3 clickOffset = Vector3.zero;

		// Token: 0x04003D03 RID: 15619
		private bool isMoving;
	}
}
