using System;
using ScheduleOne.Packaging;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B8D RID: 2957
	public class BrickPressContainer : MonoBehaviour
	{
		// Token: 0x06005004 RID: 20484 RVA: 0x00151394 File Offset: 0x0014F594
		public void SetContents(ProductItemInstance product, float fillLevel)
		{
			fillLevel = Mathf.Clamp01(fillLevel);
			if (product == null || fillLevel == 0f)
			{
				this.ContentsContainer.gameObject.SetActive(false);
				return;
			}
			product.SetupPackagingVisuals(this.Visuals);
			this.ContentsContainer.localPosition = Vector3.Lerp(this.Contents_Min.localPosition, this.Contents_Max.localPosition, fillLevel);
			this.ContentsContainer.gameObject.SetActive(true);
		}

		// Token: 0x04003C32 RID: 15410
		public FilledPackagingVisuals Visuals;

		// Token: 0x04003C33 RID: 15411
		public Transform ContentsContainer;

		// Token: 0x04003C34 RID: 15412
		public Transform Contents_Min;

		// Token: 0x04003C35 RID: 15413
		public Transform Contents_Max;
	}
}
