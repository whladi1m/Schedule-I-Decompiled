using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x0200016B RID: 363
	public class CubeSpawn : MonoBehaviour
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x0001F87C File Offset: 0x0001DA7C
		private void Start()
		{
			for (int i = 1; i <= this.instances; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
				gameObject.GetComponent<CubeSpawn>().enabled = false;
				gameObject.name = "Cube" + i.ToString();
				float f = (float)i / (float)this.instances * 3.1415927f * 2f * this.laps;
				float num = (float)i * this.expansion;
				float x = Mathf.Cos(f) * (this.radius + num);
				float z = Mathf.Sin(f) * (this.radius + num);
				Vector3 b = UnityEngine.Random.insideUnitSphere * this.jitter;
				gameObject.transform.position = base.transform.position + new Vector3(x, 0f, z) + b;
				gameObject.transform.localScale *= 1f - UnityEngine.Random.value * this.jitter;
			}
		}

		// Token: 0x040007C9 RID: 1993
		public int instances = 150;

		// Token: 0x040007CA RID: 1994
		public float radius = 2f;

		// Token: 0x040007CB RID: 1995
		public float jitter = 0.5f;

		// Token: 0x040007CC RID: 1996
		public float expansion = 0.04f;

		// Token: 0x040007CD RID: 1997
		public float laps = 2f;
	}
}
