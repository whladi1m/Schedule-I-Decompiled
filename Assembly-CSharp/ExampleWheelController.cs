using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class ExampleWheelController : MonoBehaviour
{
	// Token: 0x06000231 RID: 561 RVA: 0x0000CF6E File Offset: 0x0000B16E
	private void Start()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Rigidbody.maxAngularVelocity = 100f;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000CF8C File Offset: 0x0000B18C
	private void Update()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(-1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		float value = -this.m_Rigidbody.angularVelocity.x / 100f;
		if (this.motionVectorRenderer)
		{
			this.motionVectorRenderer.material.SetFloat(ExampleWheelController.Uniforms._MotionAmount, Mathf.Clamp(value, -0.25f, 0.25f));
		}
	}

	// Token: 0x0400025D RID: 605
	public float acceleration;

	// Token: 0x0400025E RID: 606
	public Renderer motionVectorRenderer;

	// Token: 0x0400025F RID: 607
	private Rigidbody m_Rigidbody;

	// Token: 0x02000063 RID: 99
	private static class Uniforms
	{
		// Token: 0x04000260 RID: 608
		internal static readonly int _MotionAmount = Shader.PropertyToID("_MotionAmount");
	}
}
