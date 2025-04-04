using System;
using UnityEngine;

namespace RadiantGI.Demos
{
	// Token: 0x02000168 RID: 360
	public class ShootGlowingBalls : MonoBehaviour
	{
		// Token: 0x060006E0 RID: 1760 RVA: 0x0001F4FC File Offset: 0x0001D6FC
		private void Start()
		{
			for (int i = 0; i < this.count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.glowingBall, this.center.position + Vector3.right * (float)UnityEngine.Random.Range(-4, 4) + Vector3.up * (5f + (float)i), Quaternion.identity);
				Color color = UnityEngine.Random.ColorHSV();
				float value = UnityEngine.Random.value;
				if (value < 0.33f)
				{
					color.r *= 0.2f;
				}
				else if (value < 0.66f)
				{
					color.g *= 0.2f;
				}
				else
				{
					color.b *= 0.2f;
				}
				Renderer component = gameObject.GetComponent<Renderer>();
				component.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.65f, 1f);
				component.material.color = color;
				component.material.SetColor("_EmissionColor", color * 2f);
			}
		}

		// Token: 0x040007BF RID: 1983
		public int count;

		// Token: 0x040007C0 RID: 1984
		public Transform center;

		// Token: 0x040007C1 RID: 1985
		public GameObject glowingBall;
	}
}
