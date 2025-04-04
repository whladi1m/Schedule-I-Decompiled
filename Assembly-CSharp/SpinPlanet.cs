using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class SpinPlanet : MonoBehaviour
{
	// Token: 0x06000100 RID: 256 RVA: 0x000064B3 File Offset: 0x000046B3
	private void Update()
	{
		base.transform.Rotate(Vector3.up, this.speed * Time.deltaTime);
	}

	// Token: 0x040000E5 RID: 229
	public float speed = 4f;
}
