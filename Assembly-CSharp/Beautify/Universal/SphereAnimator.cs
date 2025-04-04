using System;
using UnityEngine;

namespace Beautify.Universal
{
	// Token: 0x020001F1 RID: 497
	public class SphereAnimator : MonoBehaviour
	{
		// Token: 0x06000AF4 RID: 2804 RVA: 0x000303E4 File Offset: 0x0002E5E4
		private void Start()
		{
			this.rb = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x000303F4 File Offset: 0x0002E5F4
		private void FixedUpdate()
		{
			if (base.transform.position.z < 2.5f)
			{
				this.rb.AddForce(Vector3.forward * 200f * Time.fixedDeltaTime);
				return;
			}
			if (base.transform.position.z > 8f)
			{
				this.rb.AddForce(Vector3.back * 200f * Time.fixedDeltaTime);
			}
		}

		// Token: 0x04000BC1 RID: 3009
		private Rigidbody rb;
	}
}
