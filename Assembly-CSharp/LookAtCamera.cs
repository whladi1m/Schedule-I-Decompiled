using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class LookAtCamera : MonoBehaviour
{
	// Token: 0x0600015B RID: 347 RVA: 0x00007A32 File Offset: 0x00005C32
	public void Start()
	{
		if (this.lookAtCamera == null)
		{
			this.lookAtCamera = Camera.main;
		}
		if (this.lookOnlyOnAwake)
		{
			this.LookCam();
		}
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00007A5B File Offset: 0x00005C5B
	public void Update()
	{
		if (!this.lookOnlyOnAwake)
		{
			this.LookCam();
		}
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00007A6B File Offset: 0x00005C6B
	public void LookCam()
	{
		base.transform.LookAt(this.lookAtCamera.transform);
	}

	// Token: 0x04000131 RID: 305
	public Camera lookAtCamera;

	// Token: 0x04000132 RID: 306
	public bool lookOnlyOnAwake;
}
