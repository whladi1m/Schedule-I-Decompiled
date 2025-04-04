using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class Rotate : MonoBehaviour
{
	// Token: 0x06000096 RID: 150 RVA: 0x000052AB File Offset: 0x000034AB
	private void Update()
	{
		base.transform.Rotate(this.Axis, this.Speed * Time.deltaTime, Space.Self);
	}

	// Token: 0x04000089 RID: 137
	public float Speed = 5f;

	// Token: 0x0400008A RID: 138
	public Vector3 Axis = Vector3.up;
}
