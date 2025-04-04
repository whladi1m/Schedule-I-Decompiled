using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000338 RID: 824
	public class Clickable : MonoBehaviour
	{
		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x0005070E File Offset: 0x0004E90E
		// (set) Token: 0x06001266 RID: 4710 RVA: 0x00050716 File Offset: 0x0004E916
		public virtual CursorManager.ECursorType HoveredCursor { get; protected set; } = CursorManager.ECursorType.Finger;

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x0005071F File Offset: 0x0004E91F
		// (set) Token: 0x06001268 RID: 4712 RVA: 0x00050727 File Offset: 0x0004E927
		public Vector3 originalHitPoint { get; protected set; } = Vector3.zero;

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x00050730 File Offset: 0x0004E930
		// (set) Token: 0x0600126A RID: 4714 RVA: 0x00050738 File Offset: 0x0004E938
		public bool IsHeld { get; protected set; }

		// Token: 0x0600126B RID: 4715 RVA: 0x00050741 File Offset: 0x0004E941
		private void Awake()
		{
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Task"));
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00050758 File Offset: 0x0004E958
		public virtual void StartClick(RaycastHit hit)
		{
			if (this.onClickStart != null)
			{
				this.onClickStart.Invoke(hit);
			}
			this.IsHeld = true;
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00050775 File Offset: 0x0004E975
		public virtual void EndClick()
		{
			if (this.onClickEnd != null)
			{
				this.onClickEnd.Invoke();
			}
			this.IsHeld = false;
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00050791 File Offset: 0x0004E991
		public void SetOriginalHitPoint(Vector3 hitPoint)
		{
			this.originalHitPoint = hitPoint;
		}

		// Token: 0x040011BC RID: 4540
		public bool ClickableEnabled = true;

		// Token: 0x040011BD RID: 4541
		public bool AutoCalculateOffset = true;

		// Token: 0x040011BE RID: 4542
		public bool FlattenZOffset;

		// Token: 0x040011C1 RID: 4545
		public UnityEvent<RaycastHit> onClickStart;

		// Token: 0x040011C2 RID: 4546
		public UnityEvent onClickEnd;
	}
}
