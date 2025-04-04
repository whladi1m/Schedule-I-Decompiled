using System;
using UnityEngine;

namespace VolumetricFogAndMist2.Demos
{
	// Token: 0x02000163 RID: 355
	public class FPS_Controller : MonoBehaviour
	{
		// Token: 0x060006D0 RID: 1744 RVA: 0x0001EC34 File Offset: 0x0001CE34
		private void Start()
		{
			this.characterController = base.gameObject.AddComponent<CharacterController>();
			this.mainCamera = Camera.main.transform;
			this.characterController.height = this.characterHeight;
			this.characterController.center = Vector3.up * this.characterHeight / 2f;
			this.mainCamera.position = base.transform.position + Vector3.up * this.characterHeight;
			this.mainCamera.rotation = Quaternion.identity;
			this.mainCamera.parent = base.transform;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001ECF0 File Offset: 0x0001CEF0
		private void Update()
		{
			Vector3 mousePosition = Input.mousePosition;
			if (mousePosition.x < 0f || mousePosition.x >= (float)Screen.width || mousePosition.y < 0f || mousePosition.y >= (float)Screen.height)
			{
				return;
			}
			this.isGrounded = this.characterController.isGrounded;
			this.inputHor = Input.GetAxis("Horizontal");
			this.inputVert = Input.GetAxis("Vertical");
			this.mouseHor = Input.GetAxis("Mouse X");
			this.mouseVert = Input.GetAxis("Mouse Y");
			base.transform.Rotate(0f, this.mouseHor * this.rotationSpeed * this.mouseSensitivity * this.mouseInvertX, 0f);
			Vector3 a = base.transform.forward * this.inputVert + base.transform.right * this.inputHor;
			a *= this.speed;
			if (this.isGrounded)
			{
				if (Input.GetKey(KeyCode.LeftShift))
				{
					if (this.sprint < this.sprintMax)
					{
						this.sprint += 10f * Time.deltaTime;
					}
				}
				else if (this.sprint > 1f)
				{
					this.sprint -= 10f * Time.deltaTime;
				}
				if (Input.GetKeyDown(KeyCode.Space))
				{
					this.jumpDirection.y = this.jumpHeight;
				}
				else
				{
					this.jumpDirection.y = -1f;
				}
			}
			else
			{
				a *= this.airControl;
			}
			this.jumpDirection.y = this.jumpDirection.y - this.gravity * Time.deltaTime;
			this.characterController.Move(a * this.sprint * Time.deltaTime);
			this.characterController.Move(this.jumpDirection * Time.deltaTime);
			this.camVertAngle += this.mouseVert * this.rotationSpeed * this.mouseSensitivity * this.mouseInvertY;
			this.camVertAngle = Mathf.Clamp(this.camVertAngle, -85f, 85f);
			this.mainCamera.localEulerAngles = new Vector3(this.camVertAngle, 0f, 0f);
		}

		// Token: 0x04000792 RID: 1938
		private CharacterController characterController;

		// Token: 0x04000793 RID: 1939
		private Transform mainCamera;

		// Token: 0x04000794 RID: 1940
		private float inputHor;

		// Token: 0x04000795 RID: 1941
		private float inputVert;

		// Token: 0x04000796 RID: 1942
		private float mouseHor;

		// Token: 0x04000797 RID: 1943
		private float mouseVert;

		// Token: 0x04000798 RID: 1944
		private float mouseInvertX = 1f;

		// Token: 0x04000799 RID: 1945
		private float mouseInvertY = -1f;

		// Token: 0x0400079A RID: 1946
		private float camVertAngle;

		// Token: 0x0400079B RID: 1947
		private bool isGrounded;

		// Token: 0x0400079C RID: 1948
		private Vector3 jumpDirection = Vector3.zero;

		// Token: 0x0400079D RID: 1949
		private float sprint = 1f;

		// Token: 0x0400079E RID: 1950
		public float sprintMax = 2f;

		// Token: 0x0400079F RID: 1951
		public float airControl = 1.5f;

		// Token: 0x040007A0 RID: 1952
		public float jumpHeight = 10f;

		// Token: 0x040007A1 RID: 1953
		public float gravity = 20f;

		// Token: 0x040007A2 RID: 1954
		public float characterHeight = 1.8f;

		// Token: 0x040007A3 RID: 1955
		public float cameraHeight = 1.7f;

		// Token: 0x040007A4 RID: 1956
		public float speed = 15f;

		// Token: 0x040007A5 RID: 1957
		public float rotationSpeed = 2f;

		// Token: 0x040007A6 RID: 1958
		public float mouseSensitivity = 1f;
	}
}
