using System;
using ScheduleOne.Audio;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B98 RID: 2968
	public class BunsenBurner : MonoBehaviour
	{
		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x060050A4 RID: 20644 RVA: 0x0015401C File Offset: 0x0015221C
		// (set) Token: 0x060050A5 RID: 20645 RVA: 0x00154024 File Offset: 0x00152224
		public bool Interactable { get; private set; }

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x060050A6 RID: 20646 RVA: 0x0015402D File Offset: 0x0015222D
		// (set) Token: 0x060050A7 RID: 20647 RVA: 0x00154035 File Offset: 0x00152235
		public bool IsDialHeld { get; private set; }

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x060050A8 RID: 20648 RVA: 0x0015403E File Offset: 0x0015223E
		// (set) Token: 0x060050A9 RID: 20649 RVA: 0x00154046 File Offset: 0x00152246
		public float CurrentDialValue { get; private set; }

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x060050AA RID: 20650 RVA: 0x0015404F File Offset: 0x0015224F
		// (set) Token: 0x060050AB RID: 20651 RVA: 0x00154057 File Offset: 0x00152257
		public float CurrentHeat { get; private set; }

		// Token: 0x060050AC RID: 20652 RVA: 0x00154060 File Offset: 0x00152260
		private void Start()
		{
			this.SetInteractable(false);
			this.HandleClickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.ClickStart));
			this.HandleClickable.onClickEnd.AddListener(new UnityAction(this.ClickEnd));
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x001540AC File Offset: 0x001522AC
		private void Update()
		{
			if (!this.LockDial)
			{
				if (this.IsDialHeld)
				{
					this.CurrentDialValue = Mathf.Clamp01(this.CurrentDialValue + this.HandleRotationSpeed * Time.deltaTime);
				}
				else
				{
					this.CurrentDialValue = Mathf.Clamp01(this.CurrentDialValue - this.HandleRotationSpeed * Time.deltaTime);
				}
				this.Handle.localRotation = Quaternion.Lerp(this.Handle_Min.localRotation, this.Handle_Max.localRotation, this.CurrentDialValue);
			}
			this.CurrentHeat = this.CurrentDialValue;
			this.Highlight.gameObject.SetActive(this.Interactable && !this.IsDialHeld);
			if (this.CurrentHeat > 0f)
			{
				this.FlameSound.VolumeMultiplier = this.CurrentHeat;
				this.FlameSound.AudioSource.pitch = this.FlamePitch.Evaluate(this.CurrentHeat);
				if (!this.FlameSound.AudioSource.isPlaying)
				{
					this.FlameSound.Play();
				}
			}
			else if (this.FlameSound.AudioSource.isPlaying)
			{
				this.FlameSound.Stop();
			}
			this.UpdateEffects();
		}

		// Token: 0x060050AE RID: 20654 RVA: 0x001541E4 File Offset: 0x001523E4
		private void UpdateEffects()
		{
			if (this.CurrentHeat > 0f)
			{
				if (!this.Flame.isPlaying)
				{
					this.Flame.Play();
				}
				this.Light.gameObject.SetActive(true);
				this.Flame.startColor = this.FlameColor.Evaluate(this.CurrentHeat);
				this.Light.color = this.Flame.startColor;
				this.Light.intensity = this.LightIntensity.Evaluate(this.CurrentHeat);
				return;
			}
			if (this.Flame.isPlaying)
			{
				this.Flame.Stop();
			}
			this.Light.gameObject.SetActive(false);
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x0015429F File Offset: 0x0015249F
		public void SetDialPosition(float pos)
		{
			this.CurrentDialValue = Mathf.Clamp01(pos);
			this.Handle.localRotation = Quaternion.Lerp(this.Handle_Min.localRotation, this.Handle_Max.localRotation, this.CurrentDialValue);
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x001542DC File Offset: 0x001524DC
		public void SetInteractable(bool e)
		{
			this.Interactable = e;
			this.HandleClickable.ClickableEnabled = e;
			if (!this.Interactable)
			{
				this.IsDialHeld = false;
			}
			if (this.Interactable)
			{
				this.Anim.Play();
				return;
			}
			this.Anim.Stop();
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x0015432B File Offset: 0x0015252B
		public void ClickStart(RaycastHit hit)
		{
			this.IsDialHeld = true;
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x00154334 File Offset: 0x00152534
		public void ClickEnd()
		{
			this.IsDialHeld = false;
		}

		// Token: 0x04003CA0 RID: 15520
		public bool LockDial;

		// Token: 0x04003CA1 RID: 15521
		[Header("Settings")]
		public Gradient FlameColor;

		// Token: 0x04003CA2 RID: 15522
		public AnimationCurve LightIntensity;

		// Token: 0x04003CA3 RID: 15523
		public float HandleRotationSpeed = 1f;

		// Token: 0x04003CA4 RID: 15524
		public AnimationCurve FlamePitch;

		// Token: 0x04003CA5 RID: 15525
		[Header("References")]
		public ParticleSystem Flame;

		// Token: 0x04003CA6 RID: 15526
		public Light Light;

		// Token: 0x04003CA7 RID: 15527
		public Transform Handle;

		// Token: 0x04003CA8 RID: 15528
		public Clickable HandleClickable;

		// Token: 0x04003CA9 RID: 15529
		public Transform Handle_Min;

		// Token: 0x04003CAA RID: 15530
		public Transform Handle_Max;

		// Token: 0x04003CAB RID: 15531
		public Transform Highlight;

		// Token: 0x04003CAC RID: 15532
		public Animation Anim;

		// Token: 0x04003CAD RID: 15533
		public AudioSourceController FlameSound;
	}
}
