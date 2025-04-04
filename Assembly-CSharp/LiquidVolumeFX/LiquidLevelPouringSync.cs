using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000175 RID: 373
	public class LiquidLevelPouringSync : MonoBehaviour
	{
		// Token: 0x06000706 RID: 1798 RVA: 0x000207FC File Offset: 0x0001E9FC
		private void Start()
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.lv = base.transform.parent.GetComponent<LiquidVolume>();
			this.UpdateColliderPos();
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00020826 File Offset: 0x0001EA26
		private void OnParticleCollision(GameObject other)
		{
			if (this.lv.level < 1f)
			{
				this.lv.level += this.fillSpeed;
			}
			this.UpdateColliderPos();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00020858 File Offset: 0x0001EA58
		private void UpdateColliderPos()
		{
			Vector3 position = new Vector3(base.transform.position.x, this.lv.liquidSurfaceYPosition - base.transform.localScale.y * 0.5f - this.sinkFactor, base.transform.position.z);
			this.rb.position = position;
			if (this.lv.level >= 1f)
			{
				base.transform.localRotation = Quaternion.Euler(UnityEngine.Random.value * 30f - 15f, UnityEngine.Random.value * 30f - 15f, UnityEngine.Random.value * 30f - 15f);
				return;
			}
			base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		}

		// Token: 0x0400080E RID: 2062
		public float fillSpeed = 0.01f;

		// Token: 0x0400080F RID: 2063
		public float sinkFactor = 0.1f;

		// Token: 0x04000810 RID: 2064
		private LiquidVolume lv;

		// Token: 0x04000811 RID: 2065
		private Rigidbody rb;
	}
}
