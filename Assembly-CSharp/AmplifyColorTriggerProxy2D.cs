using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[AddComponentMenu("")]
public class AmplifyColorTriggerProxy2D : AmplifyColorTriggerProxyBase
{
	// Token: 0x06000038 RID: 56 RVA: 0x00003EEC File Offset: 0x000020EC
	private void Start()
	{
		this.circleCollider = base.GetComponent<CircleCollider2D>();
		this.circleCollider.radius = 0.01f;
		this.circleCollider.isTrigger = true;
		this.rigidBody = base.GetComponent<Rigidbody2D>();
		this.rigidBody.gravityScale = 0f;
		this.rigidBody.bodyType = RigidbodyType2D.Kinematic;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00003EB5 File Offset: 0x000020B5
	private void LateUpdate()
	{
		base.transform.position = this.Reference.position;
		base.transform.rotation = this.Reference.rotation;
	}

	// Token: 0x04000054 RID: 84
	private CircleCollider2D circleCollider;

	// Token: 0x04000055 RID: 85
	private Rigidbody2D rigidBody;
}
