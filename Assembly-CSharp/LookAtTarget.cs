using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class LookAtTarget : MonoBehaviour
{
	// Token: 0x060001B7 RID: 439 RVA: 0x0000AAD6 File Offset: 0x00008CD6
	private void Update()
	{
		this._lookAtTarget = Vector3.Lerp(this._lookAtTarget, this._target.position, Time.deltaTime * this._speed);
		base.transform.LookAt(this._lookAtTarget);
	}

	// Token: 0x040001D3 RID: 467
	[SerializeField]
	private Transform _target;

	// Token: 0x040001D4 RID: 468
	[SerializeField]
	private float _speed = 0.5f;

	// Token: 0x040001D5 RID: 469
	private Vector3 _lookAtTarget;
}
