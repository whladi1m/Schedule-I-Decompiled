using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B9F RID: 2975
	public class StirringRod : MonoBehaviour
	{
		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x0600514F RID: 20815 RVA: 0x00156DAB File Offset: 0x00154FAB
		// (set) Token: 0x06005150 RID: 20816 RVA: 0x00156DB3 File Offset: 0x00154FB3
		public bool Interactable { get; private set; }

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06005151 RID: 20817 RVA: 0x00156DBC File Offset: 0x00154FBC
		// (set) Token: 0x06005152 RID: 20818 RVA: 0x00156DC4 File Offset: 0x00154FC4
		public float CurrentStirringSpeed { get; private set; }

		// Token: 0x06005153 RID: 20819 RVA: 0x00156DD0 File Offset: 0x00154FD0
		private void Start()
		{
			this.SetInteractable(true);
			this.Clickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ClickStart));
			this.Clickable.onClickEnd.AddListener(new UnityAction(this.ClickEnd));
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x00156E1C File Offset: 0x0015501C
		private void Update()
		{
			float volumeMultiplier = Mathf.MoveTowards(this.StirSound.VolumeMultiplier, this.CurrentStirringSpeed, Time.deltaTime * 4f);
			this.StirSound.VolumeMultiplier = volumeMultiplier;
			if (this.StirSound.VolumeMultiplier > 0f && !this.StirSound.AudioSource.isPlaying)
			{
				this.StirSound.AudioSource.Play();
				return;
			}
			if (this.StirSound.VolumeMultiplier == 0f)
			{
				this.StirSound.AudioSource.Stop();
			}
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x00156EB0 File Offset: 0x001550B0
		private void LateUpdate()
		{
			if (this.isMoving)
			{
				Vector3 forward = this.Container.forward;
				Vector3 planeHit = this.GetPlaneHit();
				float d = Vector3.SignedAngle(this.PlaneNormal.forward, planeHit - this.PlaneNormal.position, this.PlaneNormal.up);
				Quaternion b = this.PlaneNormal.rotation * Quaternion.Euler(Vector3.up * d);
				this.Container.rotation = Quaternion.Lerp(this.Container.rotation, b, Time.deltaTime * this.LerpSpeed);
				float f = Vector3.SignedAngle(forward, this.Container.forward, this.PlaneNormal.up);
				this.CurrentStirringSpeed = Mathf.Clamp01(Mathf.Abs(f) / 20f);
				this.RodPivot.localEulerAngles = new Vector3(7f * (1f - this.CurrentStirringSpeed), 0f, 0f);
				return;
			}
			this.CurrentStirringSpeed = 0f;
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x00156FBB File Offset: 0x001551BB
		public void SetInteractable(bool e)
		{
			this.Interactable = e;
			this.Clickable.ClickableEnabled = e;
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x00156FD0 File Offset: 0x001551D0
		public void ClickStart(RaycastHit hit)
		{
			this.isMoving = true;
			this.clickOffset = this.Clickable.transform.position - this.GetPlaneHit();
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x00156FFC File Offset: 0x001551FC
		private Vector3 GetPlaneHit()
		{
			Plane plane = new Plane(this.PlaneNormal.up, this.PlaneNormal.position);
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x0015704E File Offset: 0x0015524E
		public void ClickEnd()
		{
			this.isMoving = false;
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x000F7B06 File Offset: 0x000F5D06
		public void Destroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04003D04 RID: 15620
		public const float MAX_STIR_RATE = 20f;

		// Token: 0x04003D05 RID: 15621
		public const float MAX_PIVOT_ANGLE = 7f;

		// Token: 0x04003D08 RID: 15624
		public float LerpSpeed = 10f;

		// Token: 0x04003D09 RID: 15625
		[Header("References")]
		public Clickable Clickable;

		// Token: 0x04003D0A RID: 15626
		public Transform PlaneNormal;

		// Token: 0x04003D0B RID: 15627
		public Transform Container;

		// Token: 0x04003D0C RID: 15628
		public Transform RodPivot;

		// Token: 0x04003D0D RID: 15629
		public AudioSourceController StirSound;

		// Token: 0x04003D0E RID: 15630
		private Vector3 clickOffset = Vector3.zero;

		// Token: 0x04003D0F RID: 15631
		private bool isMoving;
	}
}
