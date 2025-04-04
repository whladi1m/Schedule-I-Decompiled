using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x02000814 RID: 2068
	[RequireComponent(typeof(TrashContainer))]
	public class TrashContainerContents : MonoBehaviour
	{
		// Token: 0x060038BD RID: 14525 RVA: 0x000F0045 File Offset: 0x000EE245
		protected void Start()
		{
			this.TrashContainer.onTrashLevelChanged.AddListener(new UnityAction(this.UpdateVisuals));
			this.UpdateVisuals();
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x000F006C File Offset: 0x000EE26C
		private void UpdateVisuals()
		{
			float t = (float)this.TrashContainer.TrashLevel / (float)this.TrashContainer.TrashCapacity;
			this.ContentsTransform.transform.localPosition = Vector3.Lerp(this.VisualsMinTransform.localPosition, this.VisualsMaxTransform.localPosition, t);
			this.ContentsTransform.transform.localScale = Vector3.Lerp(this.VisualsMinTransform.localScale, this.VisualsMaxTransform.localScale, t);
			this.VisualsContainer.gameObject.SetActive(this.TrashContainer.TrashLevel > 0);
			this.Collider.enabled = (this.TrashContainer.TrashLevel >= this.TrashContainer.TrashCapacity);
		}

		// Token: 0x04002925 RID: 10533
		public TrashContainer TrashContainer;

		// Token: 0x04002926 RID: 10534
		[Header("References")]
		public Transform ContentsTransform;

		// Token: 0x04002927 RID: 10535
		public Transform VisualsContainer;

		// Token: 0x04002928 RID: 10536
		public Transform VisualsMinTransform;

		// Token: 0x04002929 RID: 10537
		public Transform VisualsMaxTransform;

		// Token: 0x0400292A RID: 10538
		public Collider Collider;
	}
}
