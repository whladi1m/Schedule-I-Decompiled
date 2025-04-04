using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
[AddComponentMenu("Camera-Control/Smooth Mouse Orbit - Unluck Software")]
public class SmoothCameraOrbit : MonoBehaviour
{
	// Token: 0x0600015F RID: 351 RVA: 0x00007A83 File Offset: 0x00005C83
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00007A83 File Offset: 0x00005C83
	private void OnEnable()
	{
		this.Init();
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00007A8C File Offset: 0x00005C8C
	public void Init()
	{
		if (!this.target)
		{
			this.target = new GameObject("Cam Target")
			{
				transform = 
				{
					position = base.transform.position + base.transform.forward * this.distance
				}
			}.transform;
		}
		this.currentDistance = this.distance;
		this.desiredDistance = this.distance;
		this.position = base.transform.position;
		this.rotation = base.transform.rotation;
		this.currentRotation = base.transform.rotation;
		this.desiredRotation = base.transform.rotation;
		this.xDeg = Vector3.Angle(Vector3.right, base.transform.right);
		this.yDeg = Vector3.Angle(Vector3.up, base.transform.up);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00007BBC File Offset: 0x00005DBC
	private void LateUpdate()
	{
		if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
		{
			this.desiredDistance -= Input.GetAxis("Mouse Y") * 0.02f * (float)this.zoomRate * 0.125f * Mathf.Abs(this.desiredDistance);
		}
		else if (Input.GetMouseButton(0))
		{
			this.xDeg += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
			this.yDeg -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			this.yDeg = SmoothCameraOrbit.ClampAngle(this.yDeg, (float)this.yMinLimit, (float)this.yMaxLimit);
			this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.currentRotation = base.transform.rotation;
			this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, 0.02f * this.zoomDampening);
			base.transform.rotation = this.rotation;
			this.idleTimer = 0f;
			this.idleSmooth = 0f;
		}
		else
		{
			this.idleTimer += 0.02f;
			if (this.idleTimer > this.autoRotate && this.autoRotate > 0f)
			{
				this.idleSmooth += (0.02f + this.idleSmooth) * 0.005f;
				this.idleSmooth = Mathf.Clamp(this.idleSmooth, 0f, 1f);
				this.xDeg += this.xSpeed * Time.deltaTime * this.idleSmooth * this.autoRotateSpeed;
			}
			this.yDeg = SmoothCameraOrbit.ClampAngle(this.yDeg, (float)this.yMinLimit, (float)this.yMaxLimit);
			this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.currentRotation = base.transform.rotation;
			this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, 0.02f * this.zoomDampening * 2f);
			base.transform.rotation = this.rotation;
		}
		this.desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * 0.02f * (float)this.zoomRate * Mathf.Abs(this.desiredDistance);
		this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
		this.currentDistance = Mathf.Lerp(this.currentDistance, this.desiredDistance, 0.02f * this.zoomDampening);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
		base.transform.position = this.position;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00007EE5 File Offset: 0x000060E5
	private static float ClampAngle(float angle, float min, float max)
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

	// Token: 0x04000133 RID: 307
	public Transform target;

	// Token: 0x04000134 RID: 308
	public Vector3 targetOffset;

	// Token: 0x04000135 RID: 309
	public float distance = 5f;

	// Token: 0x04000136 RID: 310
	public float maxDistance = 20f;

	// Token: 0x04000137 RID: 311
	public float minDistance = 0.6f;

	// Token: 0x04000138 RID: 312
	public float xSpeed = 200f;

	// Token: 0x04000139 RID: 313
	public float ySpeed = 200f;

	// Token: 0x0400013A RID: 314
	public int yMinLimit = -80;

	// Token: 0x0400013B RID: 315
	public int yMaxLimit = 80;

	// Token: 0x0400013C RID: 316
	public int zoomRate = 40;

	// Token: 0x0400013D RID: 317
	public float panSpeed = 0.3f;

	// Token: 0x0400013E RID: 318
	public float zoomDampening = 5f;

	// Token: 0x0400013F RID: 319
	public float autoRotate = 1f;

	// Token: 0x04000140 RID: 320
	public float autoRotateSpeed = 0.1f;

	// Token: 0x04000141 RID: 321
	private float xDeg;

	// Token: 0x04000142 RID: 322
	private float yDeg;

	// Token: 0x04000143 RID: 323
	private float currentDistance;

	// Token: 0x04000144 RID: 324
	private float desiredDistance;

	// Token: 0x04000145 RID: 325
	private Quaternion currentRotation;

	// Token: 0x04000146 RID: 326
	private Quaternion desiredRotation;

	// Token: 0x04000147 RID: 327
	private Quaternion rotation;

	// Token: 0x04000148 RID: 328
	private Vector3 position;

	// Token: 0x04000149 RID: 329
	private float idleTimer;

	// Token: 0x0400014A RID: 330
	private float idleSmooth;
}
