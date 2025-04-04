using System;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class ResetPosition : MonoBehaviour
{
	// Token: 0x06000238 RID: 568 RVA: 0x0000D064 File Offset: 0x0000B264
	private void Start()
	{
		this.startPosition = base.transform.position;
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000D077 File Offset: 0x0000B277
	private void Update()
	{
		if (Vector3.Distance(this.startPosition, base.transform.position) >= this.distanceToReset)
		{
			base.transform.position = this.startPosition;
		}
	}

	// Token: 0x0400026A RID: 618
	public float distanceToReset = 5f;

	// Token: 0x0400026B RID: 619
	private Vector3 startPosition;
}
