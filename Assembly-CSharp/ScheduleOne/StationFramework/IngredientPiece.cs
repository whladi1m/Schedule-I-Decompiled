using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008AC RID: 2220
	[RequireComponent(typeof(Draggable))]
	public class IngredientPiece : MonoBehaviour
	{
		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06003C65 RID: 15461 RVA: 0x000FDF30 File Offset: 0x000FC130
		// (set) Token: 0x06003C66 RID: 15462 RVA: 0x000FDF38 File Offset: 0x000FC138
		public float CurrentDissolveAmount { get; private set; }

		// Token: 0x06003C67 RID: 15463 RVA: 0x000FDF41 File Offset: 0x000FC141
		private void Start()
		{
			base.InvokeRepeating("CheckLiquid", 0f, 0.05f);
			this.draggable = base.GetComponent<Draggable>();
			this.defaultDrag = this.draggable.NormalRBDrag;
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x000FDF75 File Offset: 0x000FC175
		private void Update()
		{
			if (this.DisableInteractionInLiquid && this.CurrentLiquidContainer != null)
			{
				this.draggable.ClickableEnabled = false;
			}
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x000FDF99 File Offset: 0x000FC199
		private void FixedUpdate()
		{
			this.UpdateDrag();
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x000FDFA4 File Offset: 0x000FC1A4
		private void UpdateDrag()
		{
			if (this.CurrentLiquidContainer != null)
			{
				Vector3 a = -this.draggable.Rb.velocity.normalized;
				float d = this.CurrentLiquidContainer.Viscosity * this.draggable.Rb.velocity.magnitude * 100f * this.LiquidFrictionMultiplier;
				this.draggable.Rb.AddForce(a * d, ForceMode.Acceleration);
			}
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x000FE028 File Offset: 0x000FC228
		private void CheckLiquid()
		{
			this.CurrentLiquidContainer = null;
			if (!this.DetectLiquid)
			{
				return;
			}
			Collider[] array = Physics.OverlapSphere(base.transform.position, 0.001f, 1 << LayerMask.NameToLayer("Task"), QueryTriggerInteraction.Collide);
			for (int i = 0; i < array.Length; i++)
			{
				LiquidVolumeCollider liquidVolumeCollider;
				if (array[i].isTrigger && array[i].TryGetComponent<LiquidVolumeCollider>(out liquidVolumeCollider))
				{
					this.CurrentLiquidContainer = liquidVolumeCollider.LiquidContainer;
					return;
				}
			}
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x000FE09C File Offset: 0x000FC29C
		public void DissolveAmount(float amount, bool showParticles = true)
		{
			if (this.CurrentDissolveAmount >= 1f)
			{
				return;
			}
			this.CurrentDissolveAmount = Mathf.Clamp01(this.CurrentDissolveAmount + amount);
			this.ModelContainer.transform.localScale = Vector3.one * (1f - this.CurrentDissolveAmount);
			if (showParticles)
			{
				if (!this.DissolveParticles.isPlaying)
				{
					this.DissolveParticles.Play();
				}
				if (this.dissolveParticleRoutine != null)
				{
					base.StopCoroutine(this.dissolveParticleRoutine);
				}
				this.dissolveParticleRoutine = base.StartCoroutine(this.<DissolveAmount>g__DissolveParticlesRoutine|19_0());
			}
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x000FE152 File Offset: 0x000FC352
		[CompilerGenerated]
		private IEnumerator <DissolveAmount>g__DissolveParticlesRoutine|19_0()
		{
			yield return new WaitForSeconds(0.2f);
			this.DissolveParticles.Stop();
			this.dissolveParticleRoutine = null;
			yield break;
		}

		// Token: 0x04002B8B RID: 11147
		public const float LIQUID_FRICTION = 100f;

		// Token: 0x04002B8D RID: 11149
		public LiquidContainer CurrentLiquidContainer;

		// Token: 0x04002B8E RID: 11150
		[Header("References")]
		public Transform ModelContainer;

		// Token: 0x04002B8F RID: 11151
		public ParticleSystem DissolveParticles;

		// Token: 0x04002B90 RID: 11152
		[Header("Settings")]
		public bool DetectLiquid = true;

		// Token: 0x04002B91 RID: 11153
		public bool DisableInteractionInLiquid = true;

		// Token: 0x04002B92 RID: 11154
		[Range(0f, 2f)]
		public float LiquidFrictionMultiplier = 1f;

		// Token: 0x04002B93 RID: 11155
		private Draggable draggable;

		// Token: 0x04002B94 RID: 11156
		private float defaultDrag;

		// Token: 0x04002B95 RID: 11157
		private Coroutine dissolveParticleRoutine;
	}
}
