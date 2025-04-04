using System;
using System.Collections;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x02000966 RID: 2406
	public class Taser : AvatarRangedWeapon
	{
		// Token: 0x0600416F RID: 16751 RVA: 0x001127CE File Offset: 0x001109CE
		public override void Equip(Avatar _avatar)
		{
			base.Equip(_avatar);
			this.FlashObject.gameObject.SetActive(false);
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x001127E8 File Offset: 0x001109E8
		public override void Shoot(Vector3 endPoint)
		{
			base.Shoot(endPoint);
			if (this.flashRoutine != null)
			{
				base.StopCoroutine(this.flashRoutine);
			}
			this.ChargeSound.Stop();
			this.flashRoutine = base.StartCoroutine(this.Flash(endPoint));
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x00112823 File Offset: 0x00110A23
		public override void SetIsRaised(bool raised)
		{
			base.SetIsRaised(raised);
			if (base.IsRaised)
			{
				this.ChargeSound.Play();
				return;
			}
			this.ChargeSound.Stop();
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x0011284B File Offset: 0x00110A4B
		private IEnumerator Flash(Vector3 endPoint)
		{
			float t = 0.2f;
			this.FlashObject.gameObject.SetActive(true);
			Transform transform = UnityEngine.Object.Instantiate<GameObject>(this.RayPrefab, GameObject.Find("_Temp").transform).transform;
			UnityEngine.Object.Destroy(transform.gameObject, t);
			transform.transform.position = (this.MuzzlePoint.position + endPoint) / 2f;
			transform.transform.LookAt(endPoint);
			transform.transform.localScale = new Vector3(1f, 1f, Vector3.Distance(this.MuzzlePoint.position, endPoint));
			yield return new WaitForSeconds(0.2f);
			this.FlashObject.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04002F59 RID: 12121
		public const float TaseDuration = 2f;

		// Token: 0x04002F5A RID: 12122
		public const float TaseMoveSpeedMultiplier = 0.5f;

		// Token: 0x04002F5B RID: 12123
		[Header("References")]
		public GameObject FlashObject;

		// Token: 0x04002F5C RID: 12124
		public AudioSourceController ChargeSound;

		// Token: 0x04002F5D RID: 12125
		[Header("Prefabs")]
		public GameObject RayPrefab;

		// Token: 0x04002F5E RID: 12126
		private Coroutine flashRoutine;
	}
}
