using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B7F RID: 2943
	public class PotSoilCover : MonoBehaviour
	{
		// Token: 0x06004F5F RID: 20319 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Awake()
		{
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x0014ED17 File Offset: 0x0014CF17
		private void OnEnable()
		{
			base.StartCoroutine(this.CheckQueue());
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x0014ED26 File Offset: 0x0014CF26
		public void ConfigureAppearance(Color col, float transparency)
		{
			this.MeshRenderer.material.SetColor("_MainColor", col);
			this.MeshRenderer.material.SetFloat("_Transparency", transparency);
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x0014ED54 File Offset: 0x0014CF54
		public void Reset()
		{
			this.Blank();
			this.CurrentCoverage = 0.215f;
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x0014ED67 File Offset: 0x0014CF67
		public void QueuePour(Vector3 worldSpacePosition)
		{
			this.queued = true;
			this.queuedWorldPos = worldSpacePosition;
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x0014ED77 File Offset: 0x0014CF77
		public float GetNormalizedProgress()
		{
			return (this.CurrentCoverage - 0.215f) / 0.735f;
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x0014ED8B File Offset: 0x0014CF8B
		private IEnumerator CheckQueue()
		{
			while (base.gameObject != null)
			{
				if (this.queued)
				{
					this.queued = false;
					this.DelayedApplyPour(this.queuedWorldPos);
				}
				yield return new WaitForSeconds(0.041666668f);
			}
			yield break;
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x0014ED9C File Offset: 0x0014CF9C
		private void Blank()
		{
			Texture2D texture2D = new Texture2D(128, 128);
			Color[] array = new Color[16384];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Color.black;
			}
			texture2D.SetPixels(array);
			texture2D.Apply();
			this.MeshRenderer.material.mainTexture = texture2D;
			this.mainTex = texture2D;
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x0014EE04 File Offset: 0x0014D004
		private void DelayedApplyPour(Vector3 worldSpace)
		{
			PotSoilCover.<>c__DisplayClass27_0 CS$<>8__locals1 = new PotSoilCover.<>c__DisplayClass27_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.worldSpace = worldSpace;
			base.StartCoroutine(CS$<>8__locals1.<DelayedApplyPour>g__Routine|0());
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x0014EE34 File Offset: 0x0014D034
		private void ApplyPour(Vector3 worldSpace)
		{
			this.relative = base.transform.InverseTransformPoint(worldSpace);
			this.vector2 = new Vector2(this.relative.x, this.relative.z);
			if (this.vector2.magnitude > this.Radius)
			{
				return;
			}
			this.normalizedOffset = new Vector2(this.vector2.x / this.Radius, this.vector2.y / this.Radius);
			this.originPixel = new Vector2(64f * (1f + this.normalizedOffset.x), 64f * (1f + this.normalizedOffset.y));
			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					int num = (int)this.originPixel.x - 32 + i;
					int num2 = (int)this.originPixel.y - 32 + j;
					if (num >= 0 && num < 128 && num2 >= 0 && num2 < 128)
					{
						Color pixel = this.mainTex.GetPixel(num, num2);
						pixel.r += this.GetPourMaskValue(i, j);
						pixel.g = pixel.r;
						pixel.b = pixel.r;
						pixel.a = 1f;
						this.mainTex.SetPixel(num, num2, pixel);
					}
				}
			}
			this.mainTex.Apply();
			float currentCoverage = this.CurrentCoverage;
			float coverage = this.GetCoverage();
			this.CurrentCoverage = coverage;
			if (coverage >= 0.95f && currentCoverage < 0.95f && this.onSufficientCoverage != null)
			{
				this.onSufficientCoverage.Invoke();
			}
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x0014EFFC File Offset: 0x0014D1FC
		private float GetPourMaskValue(int x, int y)
		{
			return this.PourMask.GetPixel(x, y).grayscale;
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x0014F020 File Offset: 0x0014D220
		private float GetCoverage()
		{
			int num = 16384;
			int num2 = 0;
			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 128; j++)
				{
					if (this.mainTex.GetPixel(i, j).r > 0.5f)
					{
						num2++;
					}
				}
			}
			return Mathf.Clamp01((float)num2 / (float)num + 0.215f);
		}

		// Token: 0x04003BDE RID: 15326
		public const int TEXTURE_SIZE = 128;

		// Token: 0x04003BDF RID: 15327
		public const int POUR_RADIUS = 32;

		// Token: 0x04003BE0 RID: 15328
		public const int UPDATES_PER_SECOND = 24;

		// Token: 0x04003BE1 RID: 15329
		public const float COVERAGE_THRESHOLD = 0.5f;

		// Token: 0x04003BE2 RID: 15330
		public const float BASE_COVERAGE = 0.215f;

		// Token: 0x04003BE3 RID: 15331
		public const float SUCCESS_COVERAGE_THRESHOLD = 0.95f;

		// Token: 0x04003BE4 RID: 15332
		public const float DELAY = 0.35f;

		// Token: 0x04003BE5 RID: 15333
		public float CurrentCoverage;

		// Token: 0x04003BE6 RID: 15334
		[Header("Settings")]
		public float Radius;

		// Token: 0x04003BE7 RID: 15335
		[Header("References")]
		public MeshRenderer MeshRenderer;

		// Token: 0x04003BE8 RID: 15336
		public Texture2D PourMask;

		// Token: 0x04003BE9 RID: 15337
		public UnityEvent onSufficientCoverage;

		// Token: 0x04003BEA RID: 15338
		private bool queued;

		// Token: 0x04003BEB RID: 15339
		private Vector3 queuedWorldPos = Vector3.zero;

		// Token: 0x04003BEC RID: 15340
		private Texture2D mainTex;

		// Token: 0x04003BED RID: 15341
		private Vector3 relative;

		// Token: 0x04003BEE RID: 15342
		private Vector2 vector2;

		// Token: 0x04003BEF RID: 15343
		private Vector2 normalizedOffset;

		// Token: 0x04003BF0 RID: 15344
		private Vector2 originPixel;
	}
}
