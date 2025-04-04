using System;
using UnityEngine;

namespace StylizedGrassDemo
{
	// Token: 0x02000166 RID: 358
	public class PlayerController : MonoBehaviour
	{
		// Token: 0x060006D9 RID: 1753 RVA: 0x0001F330 File Offset: 0x0001D530
		private void Start()
		{
			this.rb = base.GetComponent<Rigidbody>();
			if (!this.cam)
			{
				this.cam = Camera.main;
			}
			this.isGrounded = true;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001F360 File Offset: 0x0001D560
		private void FixedUpdate()
		{
			Vector3 a = new Vector3(this.cam.transform.forward.x, 0f, this.cam.transform.forward.z);
			a *= Input.GetAxis("Vertical");
			a = a.normalized;
			this.rb.AddForce(a * this.speed);
			if (Input.GetKeyDown(KeyCode.Space) && this.isGrounded)
			{
				this.rb.AddForce(Vector3.up * this.jumpForce * this.rb.mass);
				this.isGrounded = false;
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001F418 File Offset: 0x0001D618
		private void Update()
		{
			if (!this.isGrounded)
			{
				Physics.Raycast(base.transform.position, -Vector3.up, out this.raycastHit, 0.5f);
				if (this.raycastHit.collider && this.raycastHit.collider.GetType() == typeof(TerrainCollider))
				{
					this.isGrounded = true;
					if (this.landBendEffect)
					{
						this.landBendEffect.Emit(1);
					}
				}
			}
		}

		// Token: 0x040007B6 RID: 1974
		public Camera cam;

		// Token: 0x040007B7 RID: 1975
		private float speed = 15f;

		// Token: 0x040007B8 RID: 1976
		private float jumpForce = 350f;

		// Token: 0x040007B9 RID: 1977
		private Rigidbody rb;

		// Token: 0x040007BA RID: 1978
		private bool isGrounded;

		// Token: 0x040007BB RID: 1979
		public ParticleSystem landBendEffect;

		// Token: 0x040007BC RID: 1980
		private RaycastHit raycastHit;
	}
}
