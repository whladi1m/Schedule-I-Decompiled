using System;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008B3 RID: 2227
	public class PourableAngleLimit : MonoBehaviour
	{
		// Token: 0x06003C8D RID: 15501 RVA: 0x000FE59A File Offset: 0x000FC79A
		private void Awake()
		{
			this.Constraint.ClampUpDirection = true;
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x000FE5A8 File Offset: 0x000FC7A8
		public void FixedUpdate()
		{
			float upDirectionMaxDifference = Mathf.Lerp(this.AngleAtMinFill, this.AngleAtMaxFill, this.Pourable.NormalizedLiquidLevel);
			this.Constraint.UpDirectionMaxDifference = upDirectionMaxDifference;
			float angleFromUpToPour = Mathf.Lerp(this.PourAngleMinFill, this.PourAngleMaxFill, this.Pourable.NormalizedLiquidLevel);
			this.Pourable.AngleFromUpToPour = angleFromUpToPour;
		}

		// Token: 0x04002BAB RID: 11179
		public PourableModule Pourable;

		// Token: 0x04002BAC RID: 11180
		public DraggableConstraint Constraint;

		// Token: 0x04002BAD RID: 11181
		[Header("Settings")]
		public float AngleAtMaxFill = 15f;

		// Token: 0x04002BAE RID: 11182
		public float AngleAtMinFill = 90f;

		// Token: 0x04002BAF RID: 11183
		public float PourAngleMaxFill = 15f;

		// Token: 0x04002BB0 RID: 11184
		public float PourAngleMinFill = 90f;
	}
}
