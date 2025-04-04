using System;
using System.Collections;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000169 RID: 361
	public class SpillController : MonoBehaviour
	{
		// Token: 0x060006E2 RID: 1762 RVA: 0x0001F60C File Offset: 0x0001D80C
		private void Start()
		{
			this.lv = base.GetComponent<LiquidVolume>();
			this.dropTemplates = new GameObject[10];
			for (int i = 0; i < 10; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.spill);
				gameObject.transform.localScale *= UnityEngine.Random.Range(0.45f, 0.65f);
				gameObject.GetComponent<Renderer>().material.color = Color.Lerp(this.lv.liquidColor1, this.lv.liquidColor2, UnityEngine.Random.value);
				gameObject.SetActive(false);
				this.dropTemplates[i] = gameObject;
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001F6B0 File Offset: 0x0001D8B0
		private void Update()
		{
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				base.transform.Rotate(Vector3.forward * Time.deltaTime * 10f);
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				base.transform.Rotate(-Vector3.forward * Time.deltaTime * 10f);
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001F724 File Offset: 0x0001D924
		private void FixedUpdate()
		{
			Vector3 a;
			float num;
			if (this.lv.GetSpillPoint(out a, out num, 1f, LEVEL_COMPENSATION.None))
			{
				for (int i = 0; i < 15; i++)
				{
					int num2 = UnityEngine.Random.Range(0, 10);
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dropTemplates[num2]);
					gameObject.SetActive(true);
					Rigidbody component = gameObject.GetComponent<Rigidbody>();
					component.transform.position = a + UnityEngine.Random.insideUnitSphere * 0.01f;
					component.AddForce(new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value * 0.1f - 0.2f, UnityEngine.Random.value - 0.5f));
					base.StartCoroutine(this.DestroySpill(gameObject));
				}
				this.lv.level -= num / 10f + 0.001f;
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001F804 File Offset: 0x0001DA04
		private IEnumerator DestroySpill(GameObject spill)
		{
			yield return new WaitForSeconds(1f);
			UnityEngine.Object.Destroy(spill);
			yield break;
		}

		// Token: 0x040007C2 RID: 1986
		public GameObject spill;

		// Token: 0x040007C3 RID: 1987
		private LiquidVolume lv;

		// Token: 0x040007C4 RID: 1988
		private GameObject[] dropTemplates;

		// Token: 0x040007C5 RID: 1989
		private const int DROP_TEMPLATES_COUNT = 10;
	}
}
