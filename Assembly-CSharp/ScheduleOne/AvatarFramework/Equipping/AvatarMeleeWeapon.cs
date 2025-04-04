using System;
using ScheduleOne.Audio;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x0200095D RID: 2397
	public class AvatarMeleeWeapon : AvatarWeapon
	{
		// Token: 0x06004142 RID: 16706 RVA: 0x00111DE7 File Offset: 0x0010FFE7
		public override void Unequip()
		{
			if (this.attackRoutine != null)
			{
				base.StopCoroutine(this.attackRoutine);
				this.attackRoutine = null;
			}
			base.Unequip();
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x00111E0C File Offset: 0x0011000C
		public override void Attack()
		{
			AvatarMeleeWeapon.<>c__DisplayClass10_0 CS$<>8__locals1 = new AvatarMeleeWeapon.<>c__DisplayClass10_0();
			CS$<>8__locals1.<>4__this = this;
			base.Attack();
			CS$<>8__locals1.attack = this.Attacks[UnityEngine.Random.Range(0, this.Attacks.Length)];
			CS$<>8__locals1.npc = this.avatar.GetComponentInParent<NPC>();
			this.avatar.Anim.ResetTrigger(CS$<>8__locals1.attack.AnimationTrigger);
			this.avatar.Anim.SetTrigger(CS$<>8__locals1.attack.AnimationTrigger);
			this.attackRoutine = base.StartCoroutine(CS$<>8__locals1.<Attack>g__AttackRoutine|0());
		}

		// Token: 0x04002F1B RID: 12059
		public const float GruntChance = 0.4f;

		// Token: 0x04002F1C RID: 12060
		[Header("References")]
		public AudioSourceController AttackSound;

		// Token: 0x04002F1D RID: 12061
		public AudioSourceController HitSound;

		// Token: 0x04002F1E RID: 12062
		[Header("Melee Weapon settings")]
		public float AttackRange = 1.5f;

		// Token: 0x04002F1F RID: 12063
		public float AttackRadius = 0.25f;

		// Token: 0x04002F20 RID: 12064
		public float Damage = 25f;

		// Token: 0x04002F21 RID: 12065
		public AvatarMeleeWeapon.MeleeAttack[] Attacks;

		// Token: 0x04002F22 RID: 12066
		private Coroutine attackRoutine;

		// Token: 0x0200095E RID: 2398
		[Serializable]
		public class MeleeAttack
		{
			// Token: 0x04002F23 RID: 12067
			public float RangeMultiplier = 1f;

			// Token: 0x04002F24 RID: 12068
			public float DamageMultiplier = 1f;

			// Token: 0x04002F25 RID: 12069
			public string AnimationTrigger = string.Empty;

			// Token: 0x04002F26 RID: 12070
			public float DamageDelay = 0.4f;

			// Token: 0x04002F27 RID: 12071
			public float AttackSoundDelay;

			// Token: 0x04002F28 RID: 12072
			public AudioClip[] AttackClips;

			// Token: 0x04002F29 RID: 12073
			public AudioClip[] HitClips;
		}
	}
}
