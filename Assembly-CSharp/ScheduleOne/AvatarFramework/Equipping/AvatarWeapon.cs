using System;
using ScheduleOne.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x02000963 RID: 2403
	public class AvatarWeapon : AvatarEquippable
	{
		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06004160 RID: 16736 RVA: 0x00112543 File Offset: 0x00110743
		// (set) Token: 0x06004161 RID: 16737 RVA: 0x0011254B File Offset: 0x0011074B
		public float LastUseTime { get; private set; }

		// Token: 0x06004162 RID: 16738 RVA: 0x00112554 File Offset: 0x00110754
		public override void Equip(Avatar _avatar)
		{
			base.Equip(_avatar);
			if (this.EquipClips.Length != 0 && this.EquipSound != null)
			{
				this.EquipSound.AudioSource.clip = this.EquipClips[UnityEngine.Random.Range(0, this.EquipClips.Length)];
				this.EquipSound.Play();
			}
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x001125AF File Offset: 0x001107AF
		public virtual void Attack()
		{
			this.LastUseTime = Time.time;
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x001125BC File Offset: 0x001107BC
		public virtual bool IsReadyToAttack()
		{
			return Time.time - this.LastUseTime > this.CooldownDuration;
		}

		// Token: 0x04002F48 RID: 12104
		[Header("Range settings")]
		public float MinUseRange;

		// Token: 0x04002F49 RID: 12105
		public float MaxUseRange = 1f;

		// Token: 0x04002F4A RID: 12106
		[Header("Cooldown settings")]
		public float CooldownDuration = 1f;

		// Token: 0x04002F4B RID: 12107
		[Header("Equipping")]
		public AudioClip[] EquipClips;

		// Token: 0x04002F4C RID: 12108
		public AudioSourceController EquipSound;

		// Token: 0x04002F4E RID: 12110
		public UnityEvent onSuccessfulHit;
	}
}
