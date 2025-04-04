using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BED RID: 3053
	public class FoliageRustleSound : MonoBehaviour
	{
		// Token: 0x06005587 RID: 21895 RVA: 0x00167BB4 File Offset: 0x00165DB4
		private void Awake()
		{
			base.InvokeRepeating("UpdateActive", UnityEngine.Random.Range(0f, 3f), 3f);
			this.Container.SetActive(false);
		}

		// Token: 0x06005588 RID: 21896 RVA: 0x00167BE4 File Offset: 0x00165DE4
		public void OnTriggerEnter(Collider other)
		{
			if (Time.timeSinceLevelLoad - this.timeOnLastHit > 1f)
			{
				Player componentInParent = other.gameObject.GetComponentInParent<Player>();
				if (componentInParent != null)
				{
					if (componentInParent.IsOwner)
					{
						this.Sound.VolumeMultiplier = Mathf.Clamp01(PlayerSingleton<PlayerMovement>.Instance.Controller.velocity.magnitude / (PlayerMovement.WalkSpeed * PlayerMovement.SprintMultiplier));
					}
					else
					{
						this.Sound.VolumeMultiplier = 1f;
					}
					this.Sound.Play();
					this.timeOnLastHit = Time.timeSinceLevelLoad;
				}
			}
		}

		// Token: 0x06005589 RID: 21897 RVA: 0x00167C7C File Offset: 0x00165E7C
		private void UpdateActive()
		{
			if (Player.Local == null)
			{
				return;
			}
			float num = Vector3.SqrMagnitude(Player.Local.Avatar.CenterPoint - base.transform.position);
			this.Container.SetActive(num < 900f);
		}

		// Token: 0x04003F7F RID: 16255
		public const float ACTIVATION_RANGE_SQUARED = 900f;

		// Token: 0x04003F80 RID: 16256
		public const float COOLDOWN = 1f;

		// Token: 0x04003F81 RID: 16257
		public AudioSourceController Sound;

		// Token: 0x04003F82 RID: 16258
		public GameObject Container;

		// Token: 0x04003F83 RID: 16259
		private float timeOnLastHit;
	}
}
