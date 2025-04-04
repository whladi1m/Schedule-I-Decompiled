using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Trash;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x0200033D RID: 829
	[RequireComponent(typeof(Accelerometer))]
	public class Pourable : Draggable
	{
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x00050FCD File Offset: 0x0004F1CD
		// (set) Token: 0x0600128D RID: 4749 RVA: 0x00050FD5 File Offset: 0x0004F1D5
		public bool IsPouring { get; protected set; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x00050FDE File Offset: 0x0004F1DE
		// (set) Token: 0x0600128F RID: 4751 RVA: 0x00050FE6 File Offset: 0x0004F1E6
		public float NormalizedPourRate { get; private set; }

		// Token: 0x06001290 RID: 4752 RVA: 0x00050FF0 File Offset: 0x0004F1F0
		protected virtual void Start()
		{
			if (this.autoSetCurrentQuantity)
			{
				this.currentQuantity = this.StartQuantity;
			}
			this.accelerometer = base.GetComponent<AverageAcceleration>();
			if (this.accelerometer == null)
			{
				this.accelerometer = base.gameObject.AddComponent<AverageAcceleration>();
			}
			this.particleMinSizes = new float[this.PourParticles.Length];
			this.particleMaxSizes = new float[this.PourParticles.Length];
			for (int i = 0; i < this.PourParticles.Length; i++)
			{
				this.particleMinSizes[i] = this.PourParticles[i].main.startSize.constantMin;
				this.particleMaxSizes[i] = this.PourParticles[i].main.startSize.constantMax;
			}
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x000510BF File Offset: 0x0004F2BF
		protected override void Update()
		{
			base.Update();
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x000510C7 File Offset: 0x0004F2C7
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			this.UpdatePouring();
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x000510D8 File Offset: 0x0004F2D8
		protected virtual void UpdatePouring()
		{
			float num = Vector3.Angle(Vector3.up, this.PourPoint.forward);
			this.IsPouring = (num > this.AngleFromUpToPour && this.CanPour());
			this.NormalizedPourRate = 0f;
			if (this.IsPouring && this.currentQuantity > 0f)
			{
				float num2 = (0.3f + 0.7f * (num - this.AngleFromUpToPour) / (180f - this.AngleFromUpToPour)) * this.GetShakeBoost();
				this.NormalizedPourRate = num2;
				if (this.PourLoop != null)
				{
					this.PourLoop.VolumeMultiplier = num2 - 0.3f;
					if (!this.PourLoop.isPlaying)
					{
						this.PourLoop.Play();
					}
				}
				this.PourAmount(this.PourRate_L * num2 * Time.deltaTime);
				for (int i = 0; i < this.PourParticles.Length; i++)
				{
					ParticleSystem.MainModule main = this.PourParticles[i].main;
					float min = this.ParticleMinMultiplier * num2 * this.particleMinSizes[i];
					float max = this.ParticleMaxMultiplier * num2 * this.particleMaxSizes[i];
					main.startSize = new ParticleSystem.MinMaxCurve(min, max);
				}
				if (!this.PourParticles[0].isEmitting && this.currentQuantity > 0f)
				{
					for (int j = 0; j < this.PourParticles.Length; j++)
					{
						this.PourParticles[j].Play();
					}
				}
			}
			else
			{
				if (this.PourLoop != null && this.PourLoop.isPlaying)
				{
					this.PourLoop.Stop();
				}
				if (this.PourParticles[0].isEmitting)
				{
					for (int k = 0; k < this.PourParticles.Length; k++)
					{
						this.PourParticles[k].Stop(false, ParticleSystemStopBehavior.StopEmitting);
					}
				}
			}
			if (this.currentQuantity == 0f && this.PourParticles[0].isEmitting)
			{
				for (int l = 0; l < this.PourParticles.Length; l++)
				{
					this.PourParticles[l].Stop(false, ParticleSystemStopBehavior.StopEmitting);
				}
			}
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x000512F4 File Offset: 0x0004F4F4
		private float GetShakeBoost()
		{
			return Mathf.Lerp(1f, this.ShakeBoostRate, Mathf.Clamp(this.accelerometer.Acceleration.y / 0.75f, 0f, 1f));
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0005132C File Offset: 0x0004F52C
		protected virtual void PourAmount(float amount)
		{
			if (!this.Unlimited)
			{
				this.currentQuantity = Mathf.Clamp(this.currentQuantity - amount, 0f, this.StartQuantity);
			}
			if (this.AffectsCoverage && this.IsPourPointOverPot())
			{
				this.TargetPot.SoilCover.QueuePour(this.PourPoint.position + this.PourPoint.forward * 0.05f);
			}
			if (!this.hasPoured)
			{
				if (this.onInitialPour != null)
				{
					this.onInitialPour();
				}
				this.hasPoured = true;
			}
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x000513C8 File Offset: 0x0004F5C8
		protected bool IsPourPointOverPot()
		{
			Vector3 position = this.PourPoint.position;
			position.y = this.TargetPot.transform.position.y;
			return Vector3.Distance(position, this.TargetPot.transform.position) < this.TargetPot.PotRadius;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x000022C9 File Offset: 0x000004C9
		protected virtual bool CanPour()
		{
			return true;
		}

		// Token: 0x040011E6 RID: 4582
		public Action onInitialPour;

		// Token: 0x040011E7 RID: 4583
		[Header("Pourable settings")]
		public bool Unlimited;

		// Token: 0x040011E8 RID: 4584
		public float StartQuantity = 10f;

		// Token: 0x040011E9 RID: 4585
		public float PourRate_L = 0.25f;

		// Token: 0x040011EA RID: 4586
		public float AngleFromUpToPour = 90f;

		// Token: 0x040011EB RID: 4587
		[Tooltip("Multiplier for pour rate when pourable is shaken up and down")]
		public float ShakeBoostRate = 1.35f;

		// Token: 0x040011EC RID: 4588
		public bool AffectsCoverage;

		// Token: 0x040011ED RID: 4589
		[Header("Particles")]
		public float ParticleMinMultiplier = 0.8f;

		// Token: 0x040011EE RID: 4590
		public float ParticleMaxMultiplier = 1.5f;

		// Token: 0x040011EF RID: 4591
		[Header("Pourable References")]
		public ParticleSystem[] PourParticles;

		// Token: 0x040011F0 RID: 4592
		public Transform PourPoint;

		// Token: 0x040011F1 RID: 4593
		public AudioSourceController PourLoop;

		// Token: 0x040011F2 RID: 4594
		[Header("Trash")]
		public TrashItem TrashItem;

		// Token: 0x040011F3 RID: 4595
		[HideInInspector]
		public Pot TargetPot;

		// Token: 0x040011F5 RID: 4597
		public float currentQuantity;

		// Token: 0x040011F6 RID: 4598
		protected bool hasPoured;

		// Token: 0x040011F7 RID: 4599
		protected bool autoSetCurrentQuantity = true;

		// Token: 0x040011F8 RID: 4600
		private float[] particleMinSizes;

		// Token: 0x040011F9 RID: 4601
		private float[] particleMaxSizes;

		// Token: 0x040011FA RID: 4602
		private AverageAcceleration accelerometer;
	}
}
