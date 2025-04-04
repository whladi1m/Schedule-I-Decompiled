using System;
using System.Collections.Generic;
using ScheduleOne.AvatarFramework.Animation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006C7 RID: 1735
	public class CameraOrbit : MonoBehaviour
	{
		// Token: 0x06002F4E RID: 12110 RVA: 0x000C55B0 File Offset: 0x000C37B0
		private void Start()
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			this.x = eulerAngles.y;
			this.y = eulerAngles.x;
			this.rb = base.GetComponent<Rigidbody>();
			if (this.rb != null)
			{
				this.rb.freezeRotation = true;
			}
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x000C5608 File Offset: 0x000C3808
		private void Update()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = Input.mousePosition;
			List<RaycastResult> list = new List<RaycastResult>();
			this.raycaster.Raycast(pointerEventData, list);
			this.hoveringUI = (list.Count > 0);
			this.LookAt.OverrideLookTarget(this.cam.transform.position, 100, false);
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x000C5670 File Offset: 0x000C3870
		private void LateUpdate()
		{
			if (this.target)
			{
				if (Input.GetMouseButton(0) && !this.hoveringUI)
				{
					this.targetx += Input.GetAxis("Mouse X") * this.xSpeed * this.distance * 0.02f * (5f / (this.distance + 2f));
					this.targety -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
				}
				this.targety = CameraOrbit.ClampAngle(this.targety, this.yMinLimit, this.yMaxLimit);
				this.x = Mathf.LerpAngle(this.x, this.targetx, 0.1f);
				this.y = Mathf.LerpAngle(this.y, this.targety, 1f);
				Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
				if (!this.hoveringUI)
				{
					this.targetdistance = Mathf.Clamp(this.targetdistance - Input.GetAxis("Mouse ScrollWheel") * this.ScrollSensativity, this.distanceMin, this.distanceMax);
				}
				this.distance = Mathf.Lerp(this.distance, this.targetdistance, 0.1f);
				RaycastHit raycastHit;
				if (Physics.Linecast(this.target.position, base.transform.position, out raycastHit))
				{
					this.targetdistance -= raycastHit.distance;
				}
				Vector3 point = new Vector3(0f, 0f, -this.distance);
				Vector3 position = rotation * point + this.target.position;
				base.transform.rotation = rotation;
				base.transform.position = position;
			}
			this.cam.position = base.transform.position;
			this.cam.rotation = base.transform.rotation;
			this.cam.position = this.cam.position - base.transform.right * this.sideOffset * Vector3.Distance(this.cam.position, this.target.position);
			if (Input.GetKey(KeyCode.KeypadPlus))
			{
				base.GetComponent<Camera>().fieldOfView += 0.3f;
			}
			if (Input.GetKey(KeyCode.KeypadMinus))
			{
				base.GetComponent<Camera>().fieldOfView -= 0.3f;
			}
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x00007EE5 File Offset: 0x000060E5
		public static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}

		// Token: 0x040021B8 RID: 8632
		[Header("Required")]
		public Transform target;

		// Token: 0x040021B9 RID: 8633
		public Transform cam;

		// Token: 0x040021BA RID: 8634
		public GraphicRaycaster raycaster;

		// Token: 0x040021BB RID: 8635
		public AvatarLookController LookAt;

		// Token: 0x040021BC RID: 8636
		[Header("Config")]
		public float targetdistance = 5f;

		// Token: 0x040021BD RID: 8637
		public float xSpeed = 120f;

		// Token: 0x040021BE RID: 8638
		public float ySpeed = 120f;

		// Token: 0x040021BF RID: 8639
		public float sideOffset = 1f;

		// Token: 0x040021C0 RID: 8640
		public float yMinLimit = -20f;

		// Token: 0x040021C1 RID: 8641
		public float yMaxLimit = 80f;

		// Token: 0x040021C2 RID: 8642
		public float distanceMin = 0.5f;

		// Token: 0x040021C3 RID: 8643
		public float distanceMax = 15f;

		// Token: 0x040021C4 RID: 8644
		public float ScrollSensativity = 4f;

		// Token: 0x040021C5 RID: 8645
		private Rigidbody rb;

		// Token: 0x040021C6 RID: 8646
		private float x;

		// Token: 0x040021C7 RID: 8647
		private float y;

		// Token: 0x040021C8 RID: 8648
		private float targetx;

		// Token: 0x040021C9 RID: 8649
		private float targety;

		// Token: 0x040021CA RID: 8650
		private float distance = 5f;

		// Token: 0x040021CB RID: 8651
		private bool hoveringUI;
	}
}
