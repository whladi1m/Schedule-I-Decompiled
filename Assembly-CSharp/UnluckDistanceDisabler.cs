using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class UnluckDistanceDisabler : MonoBehaviour
{
	// Token: 0x06000156 RID: 342 RVA: 0x000078C0 File Offset: 0x00005AC0
	public void Start()
	{
		if (this._distanceFromMainCam)
		{
			this._distanceFrom = Camera.main.transform;
		}
		base.InvokeRepeating("CheckDisable", this._disableCheckInterval + UnityEngine.Random.value * this._disableCheckInterval, this._disableCheckInterval);
		base.InvokeRepeating("CheckEnable", this._enableCheckInterval + UnityEngine.Random.value * this._enableCheckInterval, this._enableCheckInterval);
		base.Invoke("DisableOnStart", 0.01f);
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000793D File Offset: 0x00005B3D
	public void DisableOnStart()
	{
		if (this._disableOnStart)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00007954 File Offset: 0x00005B54
	public void CheckDisable()
	{
		if (base.gameObject.activeInHierarchy && (base.transform.position - this._distanceFrom.position).sqrMagnitude > (float)(this._distanceDisable * this._distanceDisable))
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000159 RID: 345 RVA: 0x000079B0 File Offset: 0x00005BB0
	public void CheckEnable()
	{
		if (!base.gameObject.activeInHierarchy && (base.transform.position - this._distanceFrom.position).sqrMagnitude < (float)(this._distanceDisable * this._distanceDisable))
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x0400012B RID: 299
	public int _distanceDisable = 1000;

	// Token: 0x0400012C RID: 300
	public Transform _distanceFrom;

	// Token: 0x0400012D RID: 301
	public bool _distanceFromMainCam;

	// Token: 0x0400012E RID: 302
	public float _disableCheckInterval = 10f;

	// Token: 0x0400012F RID: 303
	public float _enableCheckInterval = 1f;

	// Token: 0x04000130 RID: 304
	public bool _disableOnStart;
}
