using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[AddComponentMenu("")]
public class AmplifyColorTriggerProxy : AmplifyColorTriggerProxyBase
{
	// Token: 0x06000035 RID: 53 RVA: 0x00003E5C File Offset: 0x0000205C
	private void Start()
	{
		this.sphereCollider = base.GetComponent<SphereCollider>();
		this.sphereCollider.radius = 0.01f;
		this.sphereCollider.isTrigger = true;
		this.rigidBody = base.GetComponent<Rigidbody>();
		this.rigidBody.useGravity = false;
		this.rigidBody.isKinematic = true;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003EB5 File Offset: 0x000020B5
	private void LateUpdate()
	{
		base.transform.position = this.Reference.position;
		base.transform.rotation = this.Reference.rotation;
	}

	// Token: 0x04000052 RID: 82
	private SphereCollider sphereCollider;

	// Token: 0x04000053 RID: 83
	private Rigidbody rigidBody;
}
