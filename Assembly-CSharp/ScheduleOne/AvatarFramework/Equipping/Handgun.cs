using System;
using System.Collections;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x02000964 RID: 2404
	public class Handgun : AvatarRangedWeapon
	{
		// Token: 0x06004166 RID: 16742 RVA: 0x001125F0 File Offset: 0x001107F0
		public override void Shoot(Vector3 endPoint)
		{
			base.Shoot(endPoint);
			this.Anim.Play();
			this.ShellParticles.Play();
			this.SmokeParticles.Play();
			Player componentInParent = base.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent.IsOwner)
			{
				return;
			}
			if (this.flashRoutine != null)
			{
				base.StopCoroutine(this.flashRoutine);
			}
			this.flashRoutine = base.StartCoroutine(this.Flash(endPoint));
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x00112666 File Offset: 0x00110866
		private IEnumerator Flash(Vector3 endPoint)
		{
			float num = 0.06f;
			this.FlashObject.localEulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(0f, 360f));
			this.FlashObject.gameObject.SetActive(true);
			Transform transform = UnityEngine.Object.Instantiate<GameObject>(this.RayPrefab, GameObject.Find("_Temp").transform).transform;
			UnityEngine.Object.Destroy(transform.gameObject, num);
			transform.transform.position = (this.MuzzlePoint.position + endPoint) / 2f;
			transform.transform.LookAt(endPoint);
			transform.transform.localScale = new Vector3(1f, 1f, Vector3.Distance(this.MuzzlePoint.position, endPoint));
			yield return new WaitForSeconds(num);
			this.FlashObject.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04002F4F RID: 12111
		[Header("References")]
		public Animation Anim;

		// Token: 0x04002F50 RID: 12112
		public ParticleSystem ShellParticles;

		// Token: 0x04002F51 RID: 12113
		public ParticleSystem SmokeParticles;

		// Token: 0x04002F52 RID: 12114
		public Transform FlashObject;

		// Token: 0x04002F53 RID: 12115
		[Header("Prefabs")]
		public GameObject RayPrefab;

		// Token: 0x04002F54 RID: 12116
		private Coroutine flashRoutine;
	}
}
