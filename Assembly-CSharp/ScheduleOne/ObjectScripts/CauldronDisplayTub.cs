using System;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B95 RID: 2965
	public class CauldronDisplayTub : MonoBehaviour
	{
		// Token: 0x0600509E RID: 20638 RVA: 0x00153E7C File Offset: 0x0015207C
		public void Configure(CauldronDisplayTub.EContents contentsType, float fillLevel)
		{
			this.CocaLeafContainer.gameObject.SetActive(false);
			Transform transform = null;
			if (contentsType == CauldronDisplayTub.EContents.CocaLeaf)
			{
				transform = this.CocaLeafContainer;
			}
			if (transform != null)
			{
				transform.transform.localPosition = Vector3.Lerp(this.Container_Min.localPosition, this.Container_Max.localPosition, fillLevel);
				transform.gameObject.SetActive(fillLevel > 0f);
			}
		}

		// Token: 0x04003C87 RID: 15495
		public Transform CocaLeafContainer;

		// Token: 0x04003C88 RID: 15496
		public Transform Container_Min;

		// Token: 0x04003C89 RID: 15497
		public Transform Container_Max;

		// Token: 0x02000B96 RID: 2966
		public enum EContents
		{
			// Token: 0x04003C8B RID: 15499
			None,
			// Token: 0x04003C8C RID: 15500
			CocaLeaf
		}
	}
}
