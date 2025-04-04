using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.Tools
{
	// Token: 0x02000861 RID: 2145
	public class VerticalLayoutGroupSetter : MonoBehaviour
	{
		// Token: 0x06003A54 RID: 14932 RVA: 0x000F59EA File Offset: 0x000F3BEA
		private void Awake()
		{
			this.layoutGroup = base.GetComponent<VerticalLayoutGroup>();
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x000F59F8 File Offset: 0x000F3BF8
		public void Update()
		{
			if (this.layoutGroup.padding.left != (int)this.LeftSpacing)
			{
				this.layoutGroup.padding.left = (int)this.LeftSpacing;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.layoutGroup.GetComponent<RectTransform>());
			}
		}

		// Token: 0x04002A26 RID: 10790
		public float LeftSpacing;

		// Token: 0x04002A27 RID: 10791
		private VerticalLayoutGroup layoutGroup;
	}
}
