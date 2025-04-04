using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dragging;
using ScheduleOne.FX;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Combat
{
	// Token: 0x02000729 RID: 1833
	public class PunchController : MonoBehaviour
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06003193 RID: 12691 RVA: 0x000CE025 File Offset: 0x000CC225
		// (set) Token: 0x06003194 RID: 12692 RVA: 0x000CE02D File Offset: 0x000CC22D
		public bool PunchingEnabled { get; set; } = true;

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06003195 RID: 12693 RVA: 0x000CE036 File Offset: 0x000CC236
		public bool IsLoading
		{
			get
			{
				return this.punchLoad > 0f;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06003196 RID: 12694 RVA: 0x000CE045 File Offset: 0x000CC245
		// (set) Token: 0x06003197 RID: 12695 RVA: 0x000CE04D File Offset: 0x000CC24D
		public bool IsPunching { get; private set; }

		// Token: 0x06003198 RID: 12696 RVA: 0x000CE056 File Offset: 0x000CC256
		private void Awake()
		{
			this.player = base.GetComponentInParent<Player>();
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x000CE064 File Offset: 0x000CC264
		private void Start()
		{
			PlayerSingleton<PlayerInventory>.Instance.onPreItemEquipped.AddListener(delegate()
			{
				this.SetPunchingEnabled(false);
			});
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x000CE081 File Offset: 0x000CC281
		private void Update()
		{
			this.SetPunchingEnabled(this.ShouldBeEnabled());
			if (!this.PunchingEnabled || this.timeSincePunchingEnabled < 0.1f)
			{
				return;
			}
			this.UpdateInput();
			this.UpdateCooldown();
			this.itemEquippedLastFrame = PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped;
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x000CE0C1 File Offset: 0x000CC2C1
		private void LateUpdate()
		{
			if (this.PunchingEnabled)
			{
				this.timeSincePunchingEnabled += Time.deltaTime;
				return;
			}
			this.timeSincePunchingEnabled = 0f;
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x000CE0EC File Offset: 0x000CC2EC
		private void UpdateCooldown()
		{
			if (this.remainingCooldown > 0f && !this.IsLoading && !this.IsPunching)
			{
				this.remainingCooldown -= Time.deltaTime;
				this.remainingCooldown = Mathf.Clamp(this.remainingCooldown, 0f, 0.2f);
			}
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x000CE144 File Offset: 0x000CC344
		private void UpdateInput()
		{
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				if (this.punchLoad == 0f)
				{
					if (!this.CanStartLoading() || !GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
					{
						return;
					}
					this.StartLoad();
				}
				this.punchLoad += Time.deltaTime;
				Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Load", this.punchLoad / 1f);
				PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", this.punchLoad / 1f);
				if (this.punchLoad < 1f)
				{
					PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-(this.MaxStaminaCost - this.MinStaminaCost) * Time.deltaTime / 1f, true);
				}
				else
				{
					PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-1E-07f, true);
				}
				if (this.IsLoading && PlayerSingleton<PlayerMovement>.Instance.CurrentStaminaReserve <= 0f)
				{
					this.Release();
					return;
				}
			}
			else if (this.punchLoad > 0f)
			{
				this.Release();
			}
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x000CE24C File Offset: 0x000CC44C
		private bool CanStartLoading()
		{
			return this.remainingCooldown <= 0f && !this.IsPunching && PlayerSingleton<PlayerMovement>.Instance.CurrentStaminaReserve >= this.MinStaminaCost && !this.itemEquippedLastFrame && !GameManager.IS_TUTORIAL;
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x000CE29C File Offset: 0x000CC49C
		private void StartLoad()
		{
			PlayerSingleton<PlayerMovement>.Instance.ChangeStamina(-this.MinStaminaCost, true);
			Singleton<ViewmodelAvatar>.Instance.SetVisibility(true);
			Singleton<ViewmodelAvatar>.Instance.SetOffset(this.ViewmodelAvatarOffset);
			Singleton<ViewmodelAvatar>.Instance.SetAnimatorController(this.PunchAnimator);
			Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Load", 0f);
			PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", 0f);
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x000CE318 File Offset: 0x000CC518
		private void Release()
		{
			float num = Mathf.Clamp01(this.punchLoad / 1f);
			this.Punch(num);
			PlayerSingleton<PlayerMovement>.Instance.SetResidualVelocity(this.player.transform.forward, Mathf.Lerp(0f, 300f, num), Mathf.Lerp(0.05f, 0.15f, num));
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
			this.punchLoad = 0f;
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x000CE3E0 File Offset: 0x000CC5E0
		private void Punch(float power)
		{
			PunchController.<>c__DisplayClass39_0 CS$<>8__locals1 = new PunchController.<>c__DisplayClass39_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.power = power;
			this.IsPunching = true;
			this.PunchSound.VolumeMultiplier = Mathf.Lerp(0.4f, 1f, CS$<>8__locals1.power);
			this.PunchSound.PitchMultiplier = Mathf.Lerp(1f, 0.8f, CS$<>8__locals1.power);
			this.PunchSound.Play();
			this.player.SendPunch();
			this.punchRoutine = base.StartCoroutine(CS$<>8__locals1.<Punch>g__PunchRoutine|0());
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x000CE470 File Offset: 0x000CC670
		private void ExecuteHit(float power)
		{
			RaycastHit hit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(1.25f, out hit, NetworkSingleton<CombatManager>.Instance.MeleeLayerMask, true, 0.3f))
			{
				IDamageable componentInParent = hit.collider.GetComponentInParent<IDamageable>();
				if (componentInParent != null)
				{
					float impactDamage = Mathf.Lerp(this.MinPunchDamage, this.MaxPunchDamage, power);
					float impactForce = Mathf.Lerp(this.MinPunchForce, this.MaxPunchForce, power);
					Impact impact = new Impact(hit, hit.point, PlayerSingleton<PlayerCamera>.Instance.transform.forward, impactForce, impactDamage, EImpactType.Punch, this.player, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
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
					PlayerSingleton<PlayerCamera>.Instance.StartCameraShake(Mathf.Lerp(0.1f, 0.4f, power), 0.2f, true);
				}
			}
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x000CE59C File Offset: 0x000CC79C
		private void SetPunchingEnabled(bool enabled)
		{
			if (this.PunchingEnabled == enabled)
			{
				return;
			}
			this.PunchingEnabled = enabled;
			if (!this.PunchingEnabled)
			{
				this.punchLoad = 0f;
				Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Load", 0f);
				Singleton<ViewmodelAvatar>.Instance.SetVisibility(false);
				PlayerSingleton<PlayerCamera>.Instance.Animator.SetFloat("Load", 0f);
				if (this.punchRoutine != null)
				{
					base.StopCoroutine(this.punchRoutine);
					this.remainingCooldown = 0.1f;
					this.IsPunching = false;
					this.punchRoutine = null;
				}
			}
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x000CE638 File Offset: 0x000CC838
		private bool ShouldBeEnabled()
		{
			return PlayerSingleton<PlayerInventory>.InstanceExists && PlayerSingleton<PlayerCamera>.InstanceExists && !(Player.Local == null) && Singleton<PauseMenu>.InstanceExists && !PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount <= 0 && !Singleton<PauseMenu>.Instance.IsPaused && !(Player.Local.CurrentVehicle != null) && Player.Local.Health.IsAlive && !NetworkSingleton<DragManager>.Instance.IsDragging;
		}

		// Token: 0x04002376 RID: 9078
		public const float MAX_PUNCH_LOAD = 1f;

		// Token: 0x04002377 RID: 9079
		public const float MIN_COOLDOWN = 0.1f;

		// Token: 0x04002378 RID: 9080
		public const float MAX_COOLDOWN = 0.2f;

		// Token: 0x04002379 RID: 9081
		public const float PUNCH_RANGE = 1.25f;

		// Token: 0x0400237A RID: 9082
		public const float PUNCH_DEBOUNCE = 0.1f;

		// Token: 0x0400237D RID: 9085
		[Header("Settings")]
		public Vector3 ViewmodelAvatarOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x0400237E RID: 9086
		public float MinPunchDamage = 20f;

		// Token: 0x0400237F RID: 9087
		public float MaxPunchDamage = 60f;

		// Token: 0x04002380 RID: 9088
		public float MinPunchForce = 100f;

		// Token: 0x04002381 RID: 9089
		public float MaxPunchForce = 300f;

		// Token: 0x04002382 RID: 9090
		[Header("Stamina Settings")]
		public float MinStaminaCost = 10f;

		// Token: 0x04002383 RID: 9091
		public float MaxStaminaCost = 40f;

		// Token: 0x04002384 RID: 9092
		[Header("References")]
		public AudioSourceController PunchSound;

		// Token: 0x04002385 RID: 9093
		public RuntimeAnimatorController PunchAnimator;

		// Token: 0x04002386 RID: 9094
		private float punchLoad;

		// Token: 0x04002387 RID: 9095
		private float remainingCooldown;

		// Token: 0x04002388 RID: 9096
		private Player player;

		// Token: 0x04002389 RID: 9097
		private Coroutine punchRoutine;

		// Token: 0x0400238A RID: 9098
		private bool itemEquippedLastFrame;

		// Token: 0x0400238B RID: 9099
		private float timeSincePunchingEnabled;
	}
}
