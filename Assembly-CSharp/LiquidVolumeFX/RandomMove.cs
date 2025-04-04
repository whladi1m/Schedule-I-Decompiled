using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000174 RID: 372
	public class RandomMove : MonoBehaviour
	{
		// Token: 0x06000704 RID: 1796 RVA: 0x000203B0 File Offset: 0x0001E5B0
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				this.flaskType++;
				if (this.flaskType >= 3)
				{
					this.flaskType = 0;
				}
				base.transform.Find("SphereFlask").gameObject.SetActive(this.flaskType == 0);
				base.transform.Find("CylinderFlask").gameObject.SetActive(this.flaskType == 1);
				base.transform.Find("CubeFlask").gameObject.SetActive(this.flaskType == 2);
			}
			Vector3 a = Vector3.zero;
			if (this.automatic)
			{
				if (UnityEngine.Random.value > 0.99f)
				{
					a = Vector3.right * (this.speed + (UnityEngine.Random.value - 0.5f) * this.randomSpeed);
				}
			}
			else
			{
				if (Input.GetKey(KeyCode.RightArrow))
				{
					a += Vector3.right * this.speed;
				}
				if (Input.GetKey(KeyCode.LeftArrow))
				{
					a += Vector3.left * this.speed;
				}
				if (Input.GetKey(KeyCode.UpArrow))
				{
					a += Vector3.forward * this.speed;
				}
				if (Input.GetKey(KeyCode.DownArrow))
				{
					a += Vector3.back * this.speed;
				}
			}
			float num = 60f * Time.deltaTime;
			this.velocity += a * num;
			float num2 = 0.005f * num;
			if (this.velocity.magnitude > num2)
			{
				this.velocity -= this.velocity.normalized * num2;
			}
			else
			{
				this.velocity = Vector3.zero;
			}
			base.transform.localPosition += this.velocity * num;
			if (Input.GetKey(KeyCode.W))
			{
				base.transform.Rotate(0f, 0f, this.rotationSpeed * num);
			}
			else if (Input.GetKey(KeyCode.S))
			{
				base.transform.Rotate(0f, 0f, -this.rotationSpeed * num);
			}
			if (base.transform.localPosition.x > this.right)
			{
				base.transform.localPosition = new Vector3(this.right, base.transform.localPosition.y, base.transform.localPosition.z);
				this.velocity.Set(0f, 0f, 0f);
			}
			if (base.transform.localPosition.x < this.left)
			{
				base.transform.localPosition = new Vector3(this.left, base.transform.localPosition.y, base.transform.localPosition.z);
				this.velocity.Set(0f, 0f, 0f);
			}
			if (base.transform.localPosition.z > this.back)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, this.back);
				this.velocity.Set(0f, 0f, 0f);
			}
			if (base.transform.localPosition.z < this.front)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, this.front);
				this.velocity.Set(0f, 0f, 0f);
			}
		}

		// Token: 0x04000804 RID: 2052
		[Range(-10f, 10f)]
		public float right = 2f;

		// Token: 0x04000805 RID: 2053
		[Range(-10f, 10f)]
		public float left = -2f;

		// Token: 0x04000806 RID: 2054
		[Range(-10f, 10f)]
		public float back = 2f;

		// Token: 0x04000807 RID: 2055
		[Range(-10f, 10f)]
		public float front = -1f;

		// Token: 0x04000808 RID: 2056
		[Range(0f, 0.2f)]
		public float speed = 0.5f;

		// Token: 0x04000809 RID: 2057
		[Range(0f, 2f)]
		public float rotationSpeed = 1f;

		// Token: 0x0400080A RID: 2058
		[Range(0.1f, 2f)]
		public float randomSpeed;

		// Token: 0x0400080B RID: 2059
		public bool automatic;

		// Token: 0x0400080C RID: 2060
		private Vector3 velocity = Vector3.zero;

		// Token: 0x0400080D RID: 2061
		private int flaskType;
	}
}
