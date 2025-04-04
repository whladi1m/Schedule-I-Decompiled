using System;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x0200067B RID: 1659
	public class RollerDoor : MonoBehaviour
	{
		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002E08 RID: 11784 RVA: 0x000C1310 File Offset: 0x000BF510
		// (set) Token: 0x06002E09 RID: 11785 RVA: 0x000C1318 File Offset: 0x000BF518
		public bool IsOpen { get; protected set; } = true;

		// Token: 0x06002E0A RID: 11786 RVA: 0x000C1321 File Offset: 0x000BF521
		private void Awake()
		{
			this.Door.localPosition = (this.IsOpen ? this.LocalPos_Open : this.LocalPos_Closed);
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x000C1344 File Offset: 0x000BF544
		private void LateUpdate()
		{
			this.timeSinceValueChange += Time.deltaTime;
			if (this.timeSinceValueChange < this.LerpTime)
			{
				Vector3 b = this.IsOpen ? this.LocalPos_Open : this.LocalPos_Closed;
				this.Door.localPosition = Vector3.Lerp(this.startPos, b, this.timeSinceValueChange / this.LerpTime);
			}
			else
			{
				this.Door.localPosition = (this.IsOpen ? this.LocalPos_Open : this.LocalPos_Closed);
			}
			if (this.Blocker != null)
			{
				this.Blocker.gameObject.SetActive(!this.IsOpen);
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000C13F6 File Offset: 0x000BF5F6
		public void Open()
		{
			if (this.IsOpen)
			{
				return;
			}
			if (!this.CanOpen())
			{
				return;
			}
			this.IsOpen = true;
			this.timeSinceValueChange = 0f;
			this.startPos = this.Door.localPosition;
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000C142D File Offset: 0x000BF62D
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			this.timeSinceValueChange = 0f;
			this.startPos = this.Door.localPosition;
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x000022C9 File Offset: 0x000004C9
		protected virtual bool CanOpen()
		{
			return true;
		}

		// Token: 0x040020CD RID: 8397
		[Header("Settings")]
		public Transform Door;

		// Token: 0x040020CE RID: 8398
		public Vector3 LocalPos_Open;

		// Token: 0x040020CF RID: 8399
		public Vector3 LocalPos_Closed;

		// Token: 0x040020D0 RID: 8400
		public float LerpTime = 1f;

		// Token: 0x040020D1 RID: 8401
		public GameObject Blocker;

		// Token: 0x040020D2 RID: 8402
		private Vector3 startPos = Vector3.zero;

		// Token: 0x040020D3 RID: 8403
		private float timeSinceValueChange;
	}
}
