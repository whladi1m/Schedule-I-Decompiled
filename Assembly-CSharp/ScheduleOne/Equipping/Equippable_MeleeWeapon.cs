using System;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000906 RID: 2310
	public class Equippable_MeleeWeapon : Equippable_AvatarViewmodel
	{
		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06003E8B RID: 16011 RVA: 0x00107EDB File Offset: 0x001060DB
		public bool IsLoading
		{
			get
			{
				return this.load > 0f;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06003E8C RID: 16012 RVA: 0x00107EEA File Offset: 0x001060EA
		// (set) Token: 0x06003E8D RID: 16013 RVA: 0x00107EF2 File Offset: 0x001060F2
		public bool IsAttacking { get; private set; }

		// Token: 0x06003E8E RID: 16014 RVA: 0x00107EFB File Offset: 0x001060FB
		protected override void Update()
		{
			base.Update();
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				return;
			}
			this.UpdateInput();
			this.UpdateCooldown();
		}

		// Token: 0x06003E8F RID: 16015 RVA: 0x00107F1C File Offset: 0x0010611C
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x00107F25 File Offset: 0x00106125
		public override void Unequip()
		{
			base.Unequip();
			PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", 0f);
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x00107F48 File Offset: 0x00106148
		private void UpdateCooldown()
		{
			if (this.remainingCooldown > 0f && !this.IsLoading && !this.IsAttacking)
			{
				this.remainingCooldown -= Time.deltaTime;
				this.remainingCooldown = Mathf.Clamp(this.remainingCooldown, 0f, this.MaxCooldown);
			}
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x00107FA0 File Offset: 0x001061A0
		private void UpdateInput()
		{
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				if (this.load == 0f)
				{
					if (!GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && (!this.loadQueued || !GameInput.GetButton(GameInput.ButtonCode.PrimaryClick)))
					{
						return;
					}
					if (this.CanStartLoading())
					{
						this.StartLoad();
					}
					else if (this.clickReleased)
					{
						this.loadQueued = true;
					}
				}
				if (this.load >= 0.0001f)
				{
					this.load += Time.deltaTime;
					if (this.load < this.MaxLoadTime)
					{
						PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-(this.MaxStaminaCost - this.MinStaminaCost) * Time.deltaTime / this.MaxLoadTime, true);
					}
					else
					{
						PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-1E-07f, true);
					}
				}
				this.clickReleased = false;
				Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Load", Mathf.Clamp01(this.load / this.MaxLoadTime));
				PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", Mathf.Clamp01(this.load / this.MaxLoadTime));
				if (this.IsLoading && PlayerSingleton<PlayerMovement>.Instance.CurrentStaminaReserve <= 0f)
				{
					this.Release();
					return;
				}
			}
			else
			{
				this.clickReleased = true;
				this.loadQueued = false;
				if (this.load > 0f)
				{
					this.Release();
				}
			}
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x001080F8 File Offset: 0x001062F8
		private bool CanStartLoading()
		{
			return this.remainingCooldown <= 0f && !this.IsAttacking && base.equipAnimDone && PlayerSingleton<PlayerMovement>.Instance.CurrentStaminaReserve >= this.MinStaminaCost && !GameManager.IS_TUTORIAL;
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x00108148 File Offset: 0x00106348
		private void StartLoad()
		{
			this.loadQueued = false;
			this.load = 0.001f;
			PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-this.MinStaminaCost, true);
			Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Load", 0f);
			PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", 0f);
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x001081AC File Offset: 0x001063AC
		private void Release()
		{
			this.loadQueued = false;
			float num = Mathf.Clamp01(this.load / this.MaxLoadTime);
			this.remainingCooldown = Mathf.Lerp(this.MinCooldown, this.MaxCooldown, num);
			this.Hit(num);
			PlayerSingleton<PlayerMovement>.Instance.SetResidualVelocity(Player.Local.transform.forward, Mathf.Lerp(0f, 300f, num), Mathf.Lerp(0.05f, 0.15f, num));
			if (num >= 1f)
			{
				Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger("Release_Heavy");
				PlayerSingleton<PlayerCamera>.Instance.Animator.SetTrigger("Release_Heavy");
			}
			else
			{
				Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger("Release_Light");
				PlayerSingleton<PlayerCamera>.Instance.Animator.SetTrigger("Release_Light");
			}
			if (this.SwingAnimationTrigger != string.Empty)
			{
				Player.Local.SendAnimationTrigger(this.SwingAnimationTrigger);
			}
			this.load = 0f;
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x001082B4 File Offset: 0x001064B4
		private void Hit(float power)
		{
			Equippable_MeleeWeapon.<>c__DisplayClass37_0 CS$<>8__locals1 = new Equippable_MeleeWeapon.<>c__DisplayClass37_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.power = power;
			this.IsAttacking = true;
			this.WhooshSound.VolumeMultiplier = Mathf.Lerp(0.4f, 1f, CS$<>8__locals1.power);
			this.WhooshSound.PitchMultiplier = Mathf.Lerp(1f, 0.8f, CS$<>8__locals1.power) * this.WhooshSoundPitch;
			this.WhooshSound.Play();
			this.hitRoutine = base.StartCoroutine(CS$<>8__locals1.<Hit>g__HitRoutine|0());
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x00108340 File Offset: 0x00106540
		private void ExecuteHit(float power)
		{
			RaycastHit hit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.Range, out hit, NetworkSingleton<CombatManager>.Instance.MeleeLayerMask, true, this.HitRadius))
			{
				IDamageable componentInParent = hit.collider.GetComponentInParent<IDamageable>();
				if (componentInParent != null)
				{
					float impactDamage = Mathf.Lerp(this.MinDamage, this.MaxDamage, power);
					float impactForce = Mathf.Lerp(this.MinForce, this.MaxForce, power);
					Impact impact = new Impact(hit, hit.point, PlayerSingleton<PlayerCamera>.Instance.transform.forward, impactForce, impactDamage, this.ImpactType, Player.Local, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
					string[] array = new string[7];
					array[0] = "Hit ";
					int num = 1;
					IDamageable damageable = componentInParent;
					array[num] = ((damageable != null) ? damageable.ToString() : null);
					array[2] = " with ";
					array[3] = impactDamage.ToString();
					array[4] = " damage and ";
					array[5] = impactForce.ToString();
					array[6] = " force.";
					Console.Log(string.Concat(array), null);
					componentInParent.SendImpact(impact);
					Singleton<FXManager>.Instance.CreateImpactFX(impact);
					this.ImpactSound.Play();
					PlayerSingleton<PlayerCamera>.Instance.StartCameraShake(Mathf.Lerp(0.1f, 0.4f, power), 0.2f, true);
					if (componentInParent is NPC)
					{
						Player.Local.VisualState.ApplyState("melee_attack", PlayerVisualState.EVisualState.Brandishing, 2.5f);
					}
				}
			}
		}

		// Token: 0x04002D06 RID: 11526
		[Header("Basic Settings")]
		public EImpactType ImpactType;

		// Token: 0x04002D07 RID: 11527
		public float Range = 1.25f;

		// Token: 0x04002D08 RID: 11528
		public float HitRadius = 0.2f;

		// Token: 0x04002D09 RID: 11529
		[Header("Timing")]
		public float MaxLoadTime = 1f;

		// Token: 0x04002D0A RID: 11530
		public float MinCooldown = 0.1f;

		// Token: 0x04002D0B RID: 11531
		public float MaxCooldown = 0.2f;

		// Token: 0x04002D0C RID: 11532
		public float MinHitDelay = 0.1f;

		// Token: 0x04002D0D RID: 11533
		public float MaxHitDelay = 0.2f;

		// Token: 0x04002D0E RID: 11534
		[Header("Damage")]
		public float MinDamage = 20f;

		// Token: 0x04002D0F RID: 11535
		public float MaxDamage = 60f;

		// Token: 0x04002D10 RID: 11536
		public float MinForce = 100f;

		// Token: 0x04002D11 RID: 11537
		public float MaxForce = 300f;

		// Token: 0x04002D12 RID: 11538
		[Header("Stamina Settings")]
		public float MinStaminaCost = 10f;

		// Token: 0x04002D13 RID: 11539
		public float MaxStaminaCost = 40f;

		// Token: 0x04002D14 RID: 11540
		[Header("Sound")]
		public AudioSourceController WhooshSound;

		// Token: 0x04002D15 RID: 11541
		public float WhooshSoundPitch = 1f;

		// Token: 0x04002D16 RID: 11542
		public AudioSourceController ImpactSound;

		// Token: 0x04002D17 RID: 11543
		[Header("Animation")]
		public string SwingAnimationTrigger;

		// Token: 0x04002D18 RID: 11544
		private float load;

		// Token: 0x04002D19 RID: 11545
		private float remainingCooldown;

		// Token: 0x04002D1A RID: 11546
		private Coroutine hitRoutine;

		// Token: 0x04002D1B RID: 11547
		private bool loadQueued;

		// Token: 0x04002D1C RID: 11548
		private bool clickReleased;
	}
}
