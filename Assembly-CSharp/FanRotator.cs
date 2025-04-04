using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class FanRotator : MonoBehaviour
{
	// Token: 0x06000105 RID: 261 RVA: 0x0000659D File Offset: 0x0000479D
	private void Start()
	{
		this.thisTransform = base.GetComponent<Transform>();
	}

	// Token: 0x06000106 RID: 262 RVA: 0x000065AB File Offset: 0x000047AB
	private void Update()
	{
		this.thisTransform.Rotate(0f, this.speed * Time.deltaTime, 0f, Space.Self);
	}

	// Token: 0x040000EB RID: 235
	private Transform thisTransform;

	// Token: 0x040000EC RID: 236
	public float speed = 90f;
}
