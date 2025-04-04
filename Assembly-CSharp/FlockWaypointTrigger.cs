using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class FlockWaypointTrigger : MonoBehaviour
{
	// Token: 0x06000190 RID: 400 RVA: 0x00009708 File Offset: 0x00007908
	public void Start()
	{
		if (this._flockChild == null)
		{
			this._flockChild = base.transform.parent.GetComponent<FlockChild>();
		}
		float num = UnityEngine.Random.Range(this._timer, this._timer * 3f);
		base.InvokeRepeating("Trigger", num, num);
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000975E File Offset: 0x0000795E
	public void Trigger()
	{
		this._flockChild.Wander(0f);
	}

	// Token: 0x040001A8 RID: 424
	public float _timer = 1f;

	// Token: 0x040001A9 RID: 425
	public FlockChild _flockChild;
}
