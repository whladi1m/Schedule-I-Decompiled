using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
	// Token: 0x06000045 RID: 69 RVA: 0x00004214 File Offset: 0x00002414
	private void Start()
	{
		this.animator = base.GetComponent<Animator>();
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00004224 File Offset: 0x00002424
	private void OnAnimatorIK()
	{
		if (this.animator)
		{
			if (this.ikActive)
			{
				if (this.lookObj != null)
				{
					this.animator.SetLookAtWeight(1f);
					this.animator.SetLookAtPosition(this.lookObj.position);
				}
				if (this.rightHandObj != null)
				{
					this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
					this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
					this.animator.SetIKPosition(AvatarIKGoal.RightHand, this.rightHandObj.position);
					this.animator.SetIKRotation(AvatarIKGoal.RightHand, this.rightHandObj.rotation);
					return;
				}
			}
			else
			{
				this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
				this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
				this.animator.SetLookAtWeight(0f);
			}
		}
	}

	// Token: 0x0400005E RID: 94
	protected Animator animator;

	// Token: 0x0400005F RID: 95
	public bool ikActive;

	// Token: 0x04000060 RID: 96
	public Transform rightHandObj;

	// Token: 0x04000061 RID: 97
	public Transform lookObj;
}
