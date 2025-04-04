using System;
using System.Collections;
using ScheduleOne.Audio;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x02000961 RID: 2401
	public class AvatarRangedWeapon : AvatarWeapon
	{
		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x0600414E RID: 16718 RVA: 0x00112128 File Offset: 0x00110328
		// (set) Token: 0x0600414F RID: 16719 RVA: 0x00112130 File Offset: 0x00110330
		public bool IsRaised { get; protected set; }

		// Token: 0x06004150 RID: 16720 RVA: 0x00112139 File Offset: 0x00110339
		public override void Equip(Avatar _avatar)
		{
			base.Equip(_avatar);
			if (this.MagazineSize != -1)
			{
				this.currentAmmo = this.MagazineSize;
			}
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x00112158 File Offset: 0x00110358
		public virtual void SetIsRaised(bool raised)
		{
			if (this.IsRaised == raised)
			{
				return;
			}
			this.IsRaised = raised;
			this.timeRaised = 0f;
			if (this.IsRaised)
			{
				base.ResetTrigger(this.LoweredAnimationTrigger);
				base.SetTrigger(this.RaisedAnimationTrigger);
				return;
			}
			base.ResetTrigger(this.RaisedAnimationTrigger);
			base.SetTrigger(this.LoweredAnimationTrigger);
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x001121BA File Offset: 0x001103BA
		private void Update()
		{
			this.timeEquipped += Time.deltaTime;
			this.timeSinceLastShot += Time.deltaTime;
			if (this.IsRaised)
			{
				this.timeRaised += Time.deltaTime;
			}
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x001121FC File Offset: 0x001103FC
		public override void ReceiveMessage(string message, object data)
		{
			base.ReceiveMessage(message, data);
			if (message == "Shoot")
			{
				this.Shoot((Vector3)data);
			}
			if (message == "Lower")
			{
				this.SetIsRaised(false);
			}
			if (message == "Raise")
			{
				this.SetIsRaised(true);
			}
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x00112254 File Offset: 0x00110454
		public bool CanShoot()
		{
			return (this.currentAmmo > 0 || this.MagazineSize == -1) && this.timeEquipped > this.EquipTime && !this.isReloading && this.timeSinceLastShot > this.MaxFireRate && this.timeRaised > this.RaiseTime;
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x001122A8 File Offset: 0x001104A8
		public virtual void Shoot(Vector3 endPoint)
		{
			this.timeSinceLastShot = 0f;
			if (this.RecoilAnimationTrigger != string.Empty)
			{
				base.ResetTrigger(this.RecoilAnimationTrigger);
				base.SetTrigger(this.RecoilAnimationTrigger);
			}
			Player componentInParent = base.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent.IsOwner)
			{
				return;
			}
			this.currentAmmo--;
			this.FireSound.PlayOneShot(true);
			if (this.currentAmmo <= 0 && this.MagazineSize != -1)
			{
				base.StartCoroutine(this.Reload());
			}
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x0011233C File Offset: 0x0011053C
		private IEnumerator Reload()
		{
			this.isReloading = true;
			yield return new WaitForSeconds(this.ReloadTime);
			this.currentAmmo = this.MagazineSize;
			this.isReloading = false;
			yield break;
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x0011234C File Offset: 0x0011054C
		public bool IsPlayerInLoS(Player target)
		{
			LayerMask mask = LayerMask.GetMask(AvatarRangedWeapon.RaycastLayers);
			RaycastHit raycastHit;
			return !Physics.Raycast(this.MuzzlePoint.position, (target.Avatar.CenterPoint - this.MuzzlePoint.position).normalized, out raycastHit, Vector3.Distance(this.MuzzlePoint.position, target.Avatar.CenterPoint), mask) || !raycastHit.collider.GetComponentInParent<Player>() || raycastHit.collider.GetComponentInParent<Player>() == target || (raycastHit.collider.GetComponentInParent<LandVehicle>() != null && raycastHit.collider.GetComponentInParent<LandVehicle>().DriverPlayer == target);
		}

		// Token: 0x04002F30 RID: 12080
		public static string[] RaycastLayers = new string[]
		{
			"Default",
			"Vehicle",
			"Door",
			"Terrain",
			"Player"
		};

		// Token: 0x04002F31 RID: 12081
		[Header("Weapon Settings")]
		public int MagazineSize = -1;

		// Token: 0x04002F32 RID: 12082
		public float ReloadTime = 2f;

		// Token: 0x04002F33 RID: 12083
		public float MaxFireRate = 0.5f;

		// Token: 0x04002F34 RID: 12084
		public bool CanShootWhileMoving;

		// Token: 0x04002F35 RID: 12085
		public float EquipTime = 1f;

		// Token: 0x04002F36 RID: 12086
		public float RaiseTime = 1f;

		// Token: 0x04002F37 RID: 12087
		public float Damage = 35f;

		// Token: 0x04002F38 RID: 12088
		[Header("Accuracy")]
		public float HitChange_MinRange = 0.6f;

		// Token: 0x04002F39 RID: 12089
		public float HitChange_MaxRange = 0.1f;

		// Token: 0x04002F3A RID: 12090
		[Header("References")]
		public Transform MuzzlePoint;

		// Token: 0x04002F3B RID: 12091
		public AudioSourceController FireSound;

		// Token: 0x04002F3C RID: 12092
		[Header("Settings")]
		public string LoweredAnimationTrigger;

		// Token: 0x04002F3D RID: 12093
		public string RaisedAnimationTrigger;

		// Token: 0x04002F3E RID: 12094
		public string RecoilAnimationTrigger;

		// Token: 0x04002F40 RID: 12096
		private bool isReloading;

		// Token: 0x04002F41 RID: 12097
		private float timeEquipped;

		// Token: 0x04002F42 RID: 12098
		private float timeRaised;

		// Token: 0x04002F43 RID: 12099
		private float timeSinceLastShot = 1000f;

		// Token: 0x04002F44 RID: 12100
		private int currentAmmo;
	}
}
