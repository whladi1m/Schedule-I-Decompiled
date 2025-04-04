using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x0200097C RID: 2428
	public class AvatarIKController : MonoBehaviour
	{
		// Token: 0x060041EE RID: 16878 RVA: 0x0011481C File Offset: 0x00112A1C
		private void Awake()
		{
			this.BodyIK.InitiateBipedIK();
			this.defaultLeftLegBendTarget = this.BodyIK.solvers.leftFoot.bendGoal;
			this.defaultRightLegBendTarget = this.BodyIK.solvers.rightFoot.bendGoal;
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x0011486A File Offset: 0x00112A6A
		private void Start()
		{
			this.SetIKActive(false);
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x00114873 File Offset: 0x00112A73
		public void SetIKActive(bool active)
		{
			this.BodyIK.enabled = active;
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x00114881 File Offset: 0x00112A81
		public void OverrideLegBendTargets(Transform leftLegTarget, Transform rightLegTarget)
		{
			this.BodyIK.solvers.leftFoot.bendGoal = leftLegTarget;
			this.BodyIK.solvers.rightFoot.bendGoal = rightLegTarget;
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x001148AF File Offset: 0x00112AAF
		public void ResetLegBendTargets()
		{
			this.BodyIK.solvers.leftFoot.bendGoal = this.defaultLeftLegBendTarget;
			this.BodyIK.solvers.rightFoot.bendGoal = this.defaultRightLegBendTarget;
		}

		// Token: 0x04002FEB RID: 12267
		[Header("References")]
		public BipedIK BodyIK;

		// Token: 0x04002FEC RID: 12268
		private Transform defaultLeftLegBendTarget;

		// Token: 0x04002FED RID: 12269
		private Transform defaultRightLegBendTarget;
	}
}
