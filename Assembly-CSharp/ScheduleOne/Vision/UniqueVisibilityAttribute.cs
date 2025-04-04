using System;

namespace ScheduleOne.Vision
{
	// Token: 0x0200027A RID: 634
	[Serializable]
	public class UniqueVisibilityAttribute : VisibilityAttribute
	{
		// Token: 0x06000D39 RID: 3385 RVA: 0x0003AB7C File Offset: 0x00038D7C
		public UniqueVisibilityAttribute(string _name, float _pointsChange, string _uniquenessCode, float _multiplier = 1f, int attributeIndex = -1) : base(_name, _pointsChange, _multiplier, attributeIndex)
		{
			this.uniquenessCode = _uniquenessCode;
		}

		// Token: 0x04000DCE RID: 3534
		public string uniquenessCode;
	}
}
