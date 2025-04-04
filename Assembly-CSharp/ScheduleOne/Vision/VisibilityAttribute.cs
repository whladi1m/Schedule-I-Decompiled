using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vision
{
	// Token: 0x0200027B RID: 635
	[Serializable]
	public class VisibilityAttribute
	{
		// Token: 0x06000D3A RID: 3386 RVA: 0x0003AB94 File Offset: 0x00038D94
		public VisibilityAttribute(string _name, float _pointsChange, float _multiplier = 1f, int attributeIndex = -1)
		{
			this.name = _name;
			this.pointsChange = _pointsChange;
			this.multiplier = _multiplier;
			if (attributeIndex == -1)
			{
				Player.Local.Visibility.activeAttributes.Add(this);
				return;
			}
			Player.Local.Visibility.activeAttributes.Insert(attributeIndex, this);
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0003AC04 File Offset: 0x00038E04
		public void Delete()
		{
			Player.Local.Visibility.activeAttributes.Remove(this);
		}

		// Token: 0x04000DCF RID: 3535
		public string name = "Attribute Name";

		// Token: 0x04000DD0 RID: 3536
		public float pointsChange;

		// Token: 0x04000DD1 RID: 3537
		[Range(0f, 5f)]
		public float multiplier = 1f;
	}
}
