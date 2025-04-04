using System;
using ScheduleOne.Audio;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008B4 RID: 2228
	public class PourableModule : ItemModule
	{
		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06003C90 RID: 15504 RVA: 0x000FE63B File Offset: 0x000FC83B
		// (set) Token: 0x06003C91 RID: 15505 RVA: 0x000FE643 File Offset: 0x000FC843
		public bool IsPouring { get; protected set; }

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06003C92 RID: 15506 RVA: 0x000FE64C File Offset: 0x000FC84C
		// (set) Token: 0x06003C93 RID: 15507 RVA: 0x000FE654 File Offset: 0x000FC854
		public float NormalizedPourRate { get; private set; }

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06003C94 RID: 15508 RVA: 0x000FE65D File Offset: 0x000FC85D
		// (set) Token: 0x06003C95 RID: 15509 RVA: 0x000FE665 File Offset: 0x000FC865
		public float LiquidLevel { get; protected set; } = 1f;

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06003C96 RID: 15510 RVA: 0x000FE66E File Offset: 0x000FC86E
		public float NormalizedLiquidLevel
		{
			get
			{
				return this.LiquidLevel / this.LiquidCapacity_L;
			}
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x000FE680 File Offset: 0x000FC880
		protected virtual void Start()
		{
			this.particleMinSizes = new float[this.PourParticles.Length];
			this.particleMaxSizes = new float[this.PourParticles.Length];
			for (int i = 0; i < this.PourParticles.Length; i++)
			{
				this.particleMinSizes[i] = this.PourParticles[i].main.startSize.constantMin;
				this.particleMaxSizes[i] = this.PourParticles[i].main.startSize.constantMax;
				ParticleSystem.CollisionModule collision = this.PourParticles[i].collision;
				LayerMask layerMask = collision.collidesWith;
				layerMask |= 1 << LayerMask.NameToLayer("Task");
				collision.collidesWith = layerMask;
				collision.sendCollisionMessages = true;
				this.PourParticles[i].gameObject.AddComponent<ParticleCollisionDetector>().onCollision.AddListener(new UnityAction<GameObject>(this.ParticleCollision));
			}
			if (this.LiquidContainer != null)
			{
				this.SetLiquidLevel(this.DefaultLiquid_L);
			}
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x000FE79C File Offset: 0x000FC99C
		public override void ActivateModule(StationItem item)
		{
			base.ActivateModule(item);
			if (this.DraggableConstraint != null)
			{
				this.DraggableConstraint.SetContainer(item.transform.parent);
			}
			if (this.Draggable != null)
			{
				this.Draggable.ClickableEnabled = true;
			}
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x000FE7EE File Offset: 0x000FC9EE
		protected virtual void FixedUpdate()
		{
			if (!base.IsModuleActive)
			{
				return;
			}
			this.UpdatePouring();
			this.UpdatePourSound();
			if (this.timeSinceFillableHit > 0.25f)
			{
				this.activeFillable = null;
			}
			this.timeSinceFillableHit += Time.fixedDeltaTime;
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x000FE82C File Offset: 0x000FCA2C
		protected virtual void UpdatePouring()
		{
			float num = Vector3.Angle(Vector3.up, this.PourPoint.forward);
			this.IsPouring = (num > this.AngleFromUpToPour && this.CanPour());
			this.NormalizedPourRate = 0f;
			if (this.IsPouring && this.NormalizedLiquidLevel > 0f)
			{
				float num2 = 0.3f + 0.7f * (num - this.AngleFromUpToPour) / (180f - this.AngleFromUpToPour);
				this.NormalizedPourRate = num2;
				this.PourAmount(num2 * this.PourRate * Time.deltaTime);
				for (int i = 0; i < this.PourParticles.Length; i++)
				{
					ParticleSystem.MainModule main = this.PourParticles[i].main;
					float num3 = 1f;
					if (this.LiquidContainer != null)
					{
						num3 = Mathf.Clamp(this.LiquidContainer.CurrentLiquidLevel, 0.3f, 1f);
					}
					float min = this.ParticleMinMultiplier * num2 * this.particleMinSizes[i] * num3;
					float max = this.ParticleMaxMultiplier * num2 * this.particleMaxSizes[i] * num3;
					main.startSize = new ParticleSystem.MinMaxCurve(min, max);
					main.startColor = this.PourParticlesColor;
				}
				if (!this.PourParticles[0].isEmitting && this.NormalizedLiquidLevel > 0f)
				{
					for (int j = 0; j < this.PourParticles.Length; j++)
					{
						this.PourParticles[j].Play();
					}
				}
			}
			else if (this.PourParticles[0].isEmitting)
			{
				for (int k = 0; k < this.PourParticles.Length; k++)
				{
					this.PourParticles[k].Stop(false, ParticleSystemStopBehavior.StopEmitting);
				}
			}
			if (this.NormalizedLiquidLevel == 0f && this.PourParticles[0].isEmitting)
			{
				for (int l = 0; l < this.PourParticles.Length; l++)
				{
					this.PourParticles[l].Stop(false, ParticleSystemStopBehavior.StopEmitting);
				}
			}
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x000FEA2C File Offset: 0x000FCC2C
		private void UpdatePourSound()
		{
			if (this.PourSound == null)
			{
				return;
			}
			if (this.NormalizedPourRate > 0f)
			{
				this.PourSound.VolumeMultiplier = this.NormalizedPourRate;
				if (!this.PourSound.isPlaying)
				{
					this.PourSound.Play();
					return;
				}
			}
			else if (this.PourSound.isPlaying)
			{
				this.PourSound.Stop();
			}
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x000FEA97 File Offset: 0x000FCC97
		public virtual void ChangeLiquidLevel(float change)
		{
			this.LiquidLevel = Mathf.Clamp(this.LiquidLevel + change, 0f, this.LiquidCapacity_L);
			if (this.LiquidContainer != null)
			{
				this.LiquidContainer.SetLiquidLevel(this.NormalizedLiquidLevel, false);
			}
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x000FEAD7 File Offset: 0x000FCCD7
		public virtual void SetLiquidLevel(float level)
		{
			this.LiquidLevel = Mathf.Clamp(level, 0f, this.LiquidCapacity_L);
			if (this.LiquidContainer != null)
			{
				this.LiquidContainer.SetLiquidLevel(this.NormalizedLiquidLevel, false);
			}
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x000FEB10 File Offset: 0x000FCD10
		protected virtual void PourAmount(float amount)
		{
			Physics.RaycastAll(this.PourPoint.position, Vector3.down, 1f, 1 << LayerMask.NameToLayer("Task"));
			if (!this.OnlyEmptyOverFillable || (this.activeFillable != null && this.activeFillable.FillableEnabled))
			{
				this.ChangeLiquidLevel(-amount);
				if (this.activeFillable != null)
				{
					this.activeFillable.AddLiquid(this.LiquidType, amount, this.LiquidColor);
				}
			}
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x000FEB98 File Offset: 0x000FCD98
		private void ParticleCollision(GameObject other)
		{
			Fillable componentInParent = other.GetComponentInParent<Fillable>();
			if (componentInParent != null && componentInParent.enabled)
			{
				this.timeSinceFillableHit = 0f;
				this.activeFillable = componentInParent;
			}
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x000022C9 File Offset: 0x000004C9
		protected virtual bool CanPour()
		{
			return true;
		}

		// Token: 0x04002BB4 RID: 11188
		[Header("Settings")]
		public string LiquidType = "Liquid";

		// Token: 0x04002BB5 RID: 11189
		public float PourRate = 0.2f;

		// Token: 0x04002BB6 RID: 11190
		public float AngleFromUpToPour = 90f;

		// Token: 0x04002BB7 RID: 11191
		public bool OnlyEmptyOverFillable = true;

		// Token: 0x04002BB8 RID: 11192
		public float LiquidCapacity_L = 0.25f;

		// Token: 0x04002BB9 RID: 11193
		public Color LiquidColor;

		// Token: 0x04002BBA RID: 11194
		public float DefaultLiquid_L = 1f;

		// Token: 0x04002BBB RID: 11195
		[Header("References")]
		public ParticleSystem[] PourParticles;

		// Token: 0x04002BBC RID: 11196
		public Transform PourPoint;

		// Token: 0x04002BBD RID: 11197
		public LiquidContainer LiquidContainer;

		// Token: 0x04002BBE RID: 11198
		public Draggable Draggable;

		// Token: 0x04002BBF RID: 11199
		public DraggableConstraint DraggableConstraint;

		// Token: 0x04002BC0 RID: 11200
		public AudioSourceController PourSound;

		// Token: 0x04002BC1 RID: 11201
		[Header("Particles")]
		public Color PourParticlesColor;

		// Token: 0x04002BC2 RID: 11202
		public float ParticleMinMultiplier = 0.8f;

		// Token: 0x04002BC3 RID: 11203
		public float ParticleMaxMultiplier = 1.5f;

		// Token: 0x04002BC4 RID: 11204
		private float[] particleMinSizes;

		// Token: 0x04002BC5 RID: 11205
		private float[] particleMaxSizes;

		// Token: 0x04002BC6 RID: 11206
		private Fillable activeFillable;

		// Token: 0x04002BC7 RID: 11207
		private float timeSinceFillableHit = 10f;
	}
}
