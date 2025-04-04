using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class SwooshTest : MonoBehaviour
{
	// Token: 0x060001BC RID: 444 RVA: 0x0000ABD0 File Offset: 0x00008DD0
	private void Start()
	{
		float num = this._animation.frameRate * this._animation.length;
		this._startN = (float)this._start / num;
		this._endN = (float)this._end / num;
		this._animationState = base.GetComponent<Animation>()[this._animation.name];
		this._trail.Emit = false;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000AC3C File Offset: 0x00008E3C
	private void Update()
	{
		this._time += this._animationState.normalizedTime - this._prevAnimTime;
		if (this._time > 1f || this._firstFrame)
		{
			if (!this._firstFrame)
			{
				this._time -= 1f;
			}
			this._firstFrame = false;
		}
		if (this._prevTime < this._startN && this._time >= this._startN)
		{
			this._trail.Emit = true;
		}
		else if (this._prevTime < this._endN && this._time >= this._endN)
		{
			this._trail.Emit = false;
		}
		this._prevTime = this._time;
		this._prevAnimTime = this._animationState.normalizedTime;
	}

	// Token: 0x040001D8 RID: 472
	[SerializeField]
	private AnimationClip _animation;

	// Token: 0x040001D9 RID: 473
	private AnimationState _animationState;

	// Token: 0x040001DA RID: 474
	[SerializeField]
	private int _start;

	// Token: 0x040001DB RID: 475
	[SerializeField]
	private int _end;

	// Token: 0x040001DC RID: 476
	private float _startN;

	// Token: 0x040001DD RID: 477
	private float _endN;

	// Token: 0x040001DE RID: 478
	private float _time;

	// Token: 0x040001DF RID: 479
	private float _prevTime;

	// Token: 0x040001E0 RID: 480
	private float _prevAnimTime;

	// Token: 0x040001E1 RID: 481
	[SerializeField]
	private MeleeWeaponTrail _trail;

	// Token: 0x040001E2 RID: 482
	private bool _firstFrame = true;
}
