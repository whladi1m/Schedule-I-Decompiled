using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class RotateAround : MonoBehaviour
{
	// Token: 0x06000108 RID: 264 RVA: 0x000045B1 File Offset: 0x000027B1
	private void Start()
	{
	}

	// Token: 0x06000109 RID: 265 RVA: 0x000065E2 File Offset: 0x000047E2
	private void Update()
	{
		base.transform.RotateAround(this.rot_center.position, Vector3.up, 0.25f);
	}

	// Token: 0x040000ED RID: 237
	public Transform rot_center;
}
