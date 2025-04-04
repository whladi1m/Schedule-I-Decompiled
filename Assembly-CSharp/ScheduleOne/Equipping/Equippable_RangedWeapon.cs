using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.ItemFramework;
using ScheduleOne.Noise;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.Trash;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Equipping
{
	// Token: 0x0200090A RID: 2314
	public class Equippable_RangedWeapon : Equippable_AvatarViewmodel
	{
		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06003EA7 RID: 16039 RVA: 0x00108799 File Offset: 0x00106999
		// (set) Token: 0x06003EA8 RID: 16040 RVA: 0x001087A1 File Offset: 0x001069A1
		public float Aim { get; private set; }

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06003EA9 RID: 16041 RVA: 0x001087AA File Offset: 0x001069AA
		// (set) Token: 0x06003EAA RID: 16042 RVA: 0x001087B2 File Offset: 0x001069B2
		public float Accuracy { get; private set; }

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06003EAB RID: 16043 RVA: 0x001087BB File Offset: 0x001069BB
		// (set) Token: 0x06003EAC RID: 16044 RVA: 0x001087C3 File Offset: 0x001069C3
		public float TimeSinceFire { get; set; } = 1000f;

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06003EAD RID: 16045 RVA: 0x001087CC File Offset: 0x001069CC
		// (set) Token: 0x06003EAE RID: 16046 RVA: 0x001087D4 File Offset: 0x001069D4
		public bool IsReloading { get; private set; }

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06003EAF RID: 16047 RVA: 0x001087DD File Offset: 0x001069DD
		// (set) Token: 0x06003EB0 RID: 16048 RVA: 0x001087E5 File Offset: 0x001069E5
		public bool IsCocked { get; private set; }

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06003EB1 RID: 16049 RVA: 0x001087EE File Offset: 0x001069EE
		// (set) Token: 0x06003EB2 RID: 16050 RVA: 0x001087F6 File Offset: 0x001069F6
		public bool IsCocking { get; private set; }

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06003EB3 RID: 16051 RVA: 0x001087FF File Offset: 0x001069FF
		public int Ammo
		{
			get
			{
				if (this.weaponItem == null)
				{
					return 0;
				}
				return this.weaponItem.Value;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06003EB4 RID: 16052 RVA: 0x00108816 File Offset: 0x00106A16
		private float aimFov
		{
			get
			{
				return Singleton<Settings>.Instance.CameraFOV - this.AimFOVReduction;
			}
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x0010882C File Offset: 0x00106A2C
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("gun");
			this.weaponItem = (item as IntegerItemInstance);
			base.InvokeRepeating("CheckAimingAtNPC", 0f, 0.5f);
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x0010887C File Offset: 0x00106A7C
		public override void Unequip()
		{
			base.Unequip();
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			if (this.fovOverridden)
			{
				PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(this.FOVChangeDuration);
				PlayerSingleton<PlayerMovement>.Instance.RemoveSprintBlocker("Aiming");
				this.fovOverridden = false;
			}
			if (this.reloadRoutine != null)
			{
				base.StopCoroutine(this.reloadRoutine);
			}
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x001088E6 File Offset: 0x00106AE6
		protected override void Update()
		{
			base.Update();
			this.UpdateInput();
			this.UpdateAnim();
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			this.TimeSinceFire += Time.deltaTime;
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x00108918 File Offset: 0x00106B18
		private void UpdateInput()
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			if ((GameInput.GetButton(GameInput.ButtonCode.SecondaryClick) || this.timeSincePrimaryClick < 0.5f || this.IsCocking) && this.CanAim())
			{
				this.Aim = Mathf.SmoothDamp(this.Aim, 1f, ref this.aimVelocity, this.AimDuration);
				this.Accuracy = Mathf.MoveTowards(this.Accuracy, 1f, Time.deltaTime / this.AccuracyChangeDuration);
				if (!this.fovOverridden)
				{
					PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(this.aimFov, this.FOVChangeDuration);
					PlayerSingleton<PlayerMovement>.Instance.AddSprintBlocker("Aiming");
					this.fovOverridden = true;
					Player.Local.SendEquippableMessage_Networked("Raise", UnityEngine.Random.Range(int.MinValue, int.MaxValue));
				}
			}
			else
			{
				if (this.TimeSinceFire > this.FireCooldown)
				{
					this.Aim = Mathf.SmoothDamp(this.Aim, 0f, ref this.aimVelocity, this.AimDuration);
				}
				this.Accuracy = Mathf.MoveTowards(this.Accuracy, 0f, Time.deltaTime / this.AccuracyChangeDuration * 2f);
				if (this.fovOverridden)
				{
					PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(this.FOVChangeDuration);
					PlayerSingleton<PlayerMovement>.Instance.RemoveSprintBlocker("Aiming");
					this.fovOverridden = false;
					Player.Local.SendEquippableMessage_Networked("Lower", UnityEngine.Random.Range(int.MinValue, int.MaxValue));
				}
			}
			float t = Mathf.Clamp01(PlayerSingleton<PlayerMovement>.Instance.Controller.velocity.magnitude / PlayerMovement.WalkSpeed);
			float num = Mathf.Lerp(1f, 0f, t);
			if (this.Accuracy > num)
			{
				this.Accuracy = Mathf.MoveTowards(this.Accuracy, num, Time.deltaTime / this.AccuracyChangeDuration * 2f);
			}
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				return;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				this.timeSincePrimaryClick = 0f;
			}
			else
			{
				this.timeSincePrimaryClick += Time.deltaTime;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) || this.shotQueued)
			{
				if (this.CanFire(false))
				{
					if (this.Ammo > 0)
					{
						if (!this.MustBeCocked || this.IsCocked)
						{
							this.Fire();
						}
						else
						{
							this.Cock();
						}
					}
					else if (this.EmptySound != null)
					{
						this.EmptySound.Play();
						this.shotQueued = false;
						if (this.IsReloadReady(false))
						{
							this.Reload();
						}
					}
				}
				else if (this.TimeSinceFire < this.FireCooldown || this.IsCocking)
				{
					this.shotQueued = true;
				}
			}
			if (this.reloadQueued || GameInput.GetButtonDown(GameInput.ButtonCode.Reload))
			{
				if (this.IsReloadReady(false))
				{
					this.Reload();
					return;
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.Reload) && this.IsReloadReady(true) && this.TimeSinceFire > this.FireCooldown * 0.5f)
				{
					Console.Log("Reload qeueued", null);
					this.reloadQueued = true;
				}
			}
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x00108C20 File Offset: 0x00106E20
		private void UpdateAnim()
		{
			Singleton<ViewmodelAvatar>.Instance.Animator.SetFloat("Aim", this.Aim);
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x000022C9 File Offset: 0x000004C9
		private bool CanAim()
		{
			return true;
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x00108C3C File Offset: 0x00106E3C
		public virtual void Fire()
		{
			this.IsCocked = false;
			this.shotQueued = false;
			this.TimeSinceFire = 0f;
			Vector3 data = PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * 50f;
			Player.Local.SendEquippableMessage_Networked_Vector("Shoot", UnityEngine.Random.Range(int.MinValue, int.MaxValue), data);
			Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.FireAnimTriggers[UnityEngine.Random.Range(0, this.FireAnimTriggers.Length)]);
			PlayerSingleton<PlayerCamera>.Instance.JoltCamera();
			this.FireSound.Play();
			this.weaponItem.ChangeValue(-1);
			float spread = this.GetSpread();
			Vector3 vector = PlayerSingleton<PlayerCamera>.Instance.transform.forward;
			vector = Quaternion.Euler(UnityEngine.Random.insideUnitCircle * spread) * vector;
			Vector3 vector2 = PlayerSingleton<PlayerCamera>.Instance.transform.position;
			vector2 += PlayerSingleton<PlayerCamera>.Instance.transform.forward * 0.4f;
			vector2 += PlayerSingleton<PlayerCamera>.Instance.transform.right * 0.1f;
			vector2 += PlayerSingleton<PlayerCamera>.Instance.transform.up * -0.03f;
			Singleton<FXManager>.Instance.CreateBulletTrail(vector2, vector, this.TracerSpeed, this.Range, NetworkSingleton<CombatManager>.Instance.RangedWeaponLayerMask);
			NoiseUtility.EmitNoise(base.transform.position, ENoiseType.Gunshot, 25f, Player.Local.gameObject);
			if (Player.Local.CurrentProperty == null)
			{
				Player.Local.VisualState.ApplyState("shooting", PlayerVisualState.EVisualState.DischargingWeapon, 4f);
			}
			RaycastHit[] array = Physics.SphereCastAll(vector2, this.RayRadius, vector, this.Range, NetworkSingleton<CombatManager>.Instance.RangedWeaponLayerMask);
			Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			RaycastHit[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				RaycastHit hit = array2[i];
				IDamageable componentInParent = hit.collider.GetComponentInParent<IDamageable>();
				if (componentInParent == null || componentInParent != Player.Local)
				{
					if (componentInParent != null)
					{
						Impact impact = new Impact(hit, hit.point, PlayerSingleton<PlayerCamera>.Instance.transform.forward, this.ImpactForce, this.Damage, EImpactType.Bullet, Player.Local, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
						componentInParent.SendImpact(impact);
						Singleton<FXManager>.Instance.CreateImpactFX(impact);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			this.Accuracy = 0f;
			if (this.onFire != null)
			{
				this.onFire.Invoke();
			}
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x00108F07 File Offset: 0x00107107
		public virtual void Reload()
		{
			this.reloadQueued = false;
			this.IsReloading = true;
			Console.Log("Reloading...", null);
			this.reloadRoutine = base.StartCoroutine(this.<Reload>g__ReloadRoutine|77_0());
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void NotifyIncrementalReload()
		{
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x00108F34 File Offset: 0x00107134
		private bool IsReloadReady(bool ignoreTiming)
		{
			StorableItemInstance storableItemInstance;
			return this.CanReload && !this.IsReloading && this.GetMagazine(out storableItemInstance) && this.weaponItem.Value < this.MagazineSize && (this.TimeSinceFire >= this.FireCooldown || ignoreTiming) && (base.equipAnimDone || ignoreTiming) && !this.IsCocking;
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x00108FA4 File Offset: 0x001071A4
		protected virtual bool GetMagazine(out StorableItemInstance mag)
		{
			mag = null;
			for (int i = 0; i < PlayerSingleton<PlayerInventory>.Instance.hotbarSlots.Count; i++)
			{
				if (PlayerSingleton<PlayerInventory>.Instance.hotbarSlots[i].Quantity != 0 && PlayerSingleton<PlayerInventory>.Instance.hotbarSlots[i].ItemInstance.ID == this.Magazine.ID)
				{
					mag = (PlayerSingleton<PlayerInventory>.Instance.hotbarSlots[i].ItemInstance as StorableItemInstance);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x00109030 File Offset: 0x00107230
		private bool CanFire(bool checkAmmo = true)
		{
			return this.TimeSinceFire >= this.FireCooldown && this.Aim >= 0.1f && base.equipAnimDone && (!checkAmmo || this.Ammo > 0) && !this.IsReloading && !this.IsCocking;
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x0010908C File Offset: 0x0010728C
		private bool CanCock()
		{
			return !this.IsCocked && !this.IsCocking && this.weaponItem.Value > 0 && base.equipAnimDone && !this.IsReloading && this.TimeSinceFire >= this.FireCooldown;
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x001090E2 File Offset: 0x001072E2
		private void Cock()
		{
			Console.Log("Cocking", null);
			this.shotQueued = false;
			this.IsCocking = true;
			base.StartCoroutine(this.<Cock>g__CockRoutine|83_0());
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0010910A File Offset: 0x0010730A
		private float GetSpread()
		{
			return Mathf.Lerp(this.MaxSpread, this.MinSpread, this.Accuracy);
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x00109124 File Offset: 0x00107324
		private void CheckAimingAtNPC()
		{
			if (this.Aim < 0.5f)
			{
				return;
			}
			RaycastHit[] array = Physics.SphereCastAll(new Ray(PlayerSingleton<PlayerCamera>.Instance.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.forward), 0.5f, 10f, NetworkSingleton<CombatManager>.Instance.RangedWeaponLayerMask);
			List<NPC> list = new List<NPC>();
			foreach (RaycastHit raycastHit in array)
			{
				NPC componentInParent = raycastHit.collider.GetComponentInParent<NPC>();
				if (componentInParent != null && !list.Contains(componentInParent))
				{
					list.Add(componentInParent);
					if (componentInParent.awareness.VisionCone.IsPlayerVisible(Player.Local))
					{
						componentInParent.responses.RespondToAimedAt(Player.Local);
					}
				}
			}
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x001092ED File Offset: 0x001074ED
		[CompilerGenerated]
		private IEnumerator <Reload>g__ReloadRoutine|77_0()
		{
			if (this.onReloadStart != null)
			{
				this.onReloadStart.Invoke();
			}
			Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.ReloadStartAnimTrigger);
			yield return new WaitForSeconds(this.ReloadStartTime);
			StorableItemInstance storableItemInstance;
			if (this.IncrementalReload)
			{
				StorableItemInstance mag;
				while (this.weaponItem.Value < this.MagazineSize && this.GetMagazine(out mag))
				{
					if (this.onReloadIndividual != null)
					{
						this.onReloadIndividual.Invoke();
					}
					Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.ReloadIndividualAnimTrigger);
					yield return new WaitForSeconds(this.ReloadIndividalTime);
					this.weaponItem.ChangeValue(1);
					IntegerItemInstance integerItemInstance = mag as IntegerItemInstance;
					integerItemInstance.ChangeValue(-1);
					this.NotifyIncrementalReload();
					if (integerItemInstance.Value <= 0)
					{
						mag.ChangeQuantity(-1);
						if (this.ReloadTrash != null)
						{
							Vector3 posiiton = PlayerSingleton<PlayerCamera>.Instance.transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.up * 0.4f;
							NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.ReloadTrash.ID, posiiton, UnityEngine.Random.rotation, default(Vector3), "", false);
						}
					}
				}
				yield return new WaitForSeconds(0.05f);
				if (this.onReloadEnd != null)
				{
					this.onReloadEnd.Invoke();
				}
				Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.ReloadEndAnimTrigger);
				yield return new WaitForSeconds(this.ReloadEndTime);
			}
			else if (this.GetMagazine(out storableItemInstance))
			{
				IntegerItemInstance integerItemInstance2 = storableItemInstance as IntegerItemInstance;
				integerItemInstance2.ChangeValue(-(this.MagazineSize - this.weaponItem.Value));
				if (integerItemInstance2.Value <= 0)
				{
					storableItemInstance.ChangeQuantity(-1);
					if (this.ReloadTrash != null)
					{
						Vector3 posiiton2 = PlayerSingleton<PlayerCamera>.Instance.transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.up * 0.4f;
						NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.ReloadTrash.ID, posiiton2, UnityEngine.Random.rotation, default(Vector3), "", false);
					}
				}
				this.weaponItem.SetValue(this.MagazineSize);
			}
			Console.Log("Reloading done!", null);
			this.IsReloading = false;
			this.reloadRoutine = null;
			yield break;
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x001092FC File Offset: 0x001074FC
		[CompilerGenerated]
		private IEnumerator <Cock>g__CockRoutine|83_0()
		{
			if (this.onCockStart != null)
			{
				this.onCockStart.Invoke();
			}
			Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.CockAnimTrigger);
			yield return new WaitForSeconds(this.CockTime);
			this.IsCocked = true;
			this.IsCocking = false;
			yield break;
		}

		// Token: 0x04002D25 RID: 11557
		public const float NPC_AIM_DETECTION_RANGE = 10f;

		// Token: 0x04002D2C RID: 11564
		public int MagazineSize = 7;

		// Token: 0x04002D2D RID: 11565
		[Header("Aim Settings")]
		public float AimDuration = 0.2f;

		// Token: 0x04002D2E RID: 11566
		public float AimFOVReduction = 10f;

		// Token: 0x04002D2F RID: 11567
		public float FOVChangeDuration = 0.3f;

		// Token: 0x04002D30 RID: 11568
		[Header("Firing")]
		public AudioSourceController FireSound;

		// Token: 0x04002D31 RID: 11569
		public AudioSourceController EmptySound;

		// Token: 0x04002D32 RID: 11570
		public float FireCooldown = 0.3f;

		// Token: 0x04002D33 RID: 11571
		public string[] FireAnimTriggers;

		// Token: 0x04002D34 RID: 11572
		public float AccuracyChangeDuration = 0.6f;

		// Token: 0x04002D35 RID: 11573
		[Header("Raycasting")]
		public float Range = 40f;

		// Token: 0x04002D36 RID: 11574
		public float RayRadius = 0.05f;

		// Token: 0x04002D37 RID: 11575
		[Header("Spread")]
		public float MinSpread = 5f;

		// Token: 0x04002D38 RID: 11576
		public float MaxSpread = 15f;

		// Token: 0x04002D39 RID: 11577
		[Header("Damage")]
		public float Damage = 60f;

		// Token: 0x04002D3A RID: 11578
		public float ImpactForce = 300f;

		// Token: 0x04002D3B RID: 11579
		[Header("Reloading")]
		public bool CanReload = true;

		// Token: 0x04002D3C RID: 11580
		public bool IncrementalReload;

		// Token: 0x04002D3D RID: 11581
		public StorableItemDefinition Magazine;

		// Token: 0x04002D3E RID: 11582
		public float ReloadStartTime = 1.5f;

		// Token: 0x04002D3F RID: 11583
		public float ReloadIndividalTime;

		// Token: 0x04002D40 RID: 11584
		public float ReloadEndTime;

		// Token: 0x04002D41 RID: 11585
		public string ReloadStartAnimTrigger = "MagazineReload";

		// Token: 0x04002D42 RID: 11586
		public string ReloadIndividualAnimTrigger = string.Empty;

		// Token: 0x04002D43 RID: 11587
		public string ReloadEndAnimTrigger = string.Empty;

		// Token: 0x04002D44 RID: 11588
		public TrashItem ReloadTrash;

		// Token: 0x04002D45 RID: 11589
		[Header("Cocking")]
		public bool MustBeCocked;

		// Token: 0x04002D46 RID: 11590
		public float CockTime = 0.5f;

		// Token: 0x04002D47 RID: 11591
		public string CockAnimTrigger = "MagazineReload";

		// Token: 0x04002D48 RID: 11592
		[Header("Effects")]
		public float TracerSpeed = 50f;

		// Token: 0x04002D49 RID: 11593
		public UnityEvent onFire;

		// Token: 0x04002D4A RID: 11594
		public UnityEvent onReloadStart;

		// Token: 0x04002D4B RID: 11595
		public UnityEvent onReloadIndividual;

		// Token: 0x04002D4C RID: 11596
		public UnityEvent onReloadEnd;

		// Token: 0x04002D4D RID: 11597
		public UnityEvent onCockStart;

		// Token: 0x04002D4E RID: 11598
		protected IntegerItemInstance weaponItem;

		// Token: 0x04002D4F RID: 11599
		private bool fovOverridden;

		// Token: 0x04002D50 RID: 11600
		private float aimVelocity;

		// Token: 0x04002D51 RID: 11601
		private Coroutine reloadRoutine;

		// Token: 0x04002D52 RID: 11602
		private bool shotQueued;

		// Token: 0x04002D53 RID: 11603
		private bool reloadQueued;

		// Token: 0x04002D54 RID: 11604
		private float timeSincePrimaryClick = 100f;
	}
}
