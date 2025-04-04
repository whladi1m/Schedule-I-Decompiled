using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006E4 RID: 1764
	public class PositionHistoryTracker : MonoBehaviour
	{
		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x000C7FFB File Offset: 0x000C61FB
		public float RecordedTime
		{
			get
			{
				return (float)this.positionHistory.Count * this.recordingFrequency;
			}
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x000C8010 File Offset: 0x000C6210
		private void Start()
		{
			this.lastRecordTime = Time.time;
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x000C801D File Offset: 0x000C621D
		private void Update()
		{
			if (Time.time - this.lastRecordTime >= this.recordingFrequency)
			{
				this.RecordPosition();
				this.lastRecordTime = Time.time;
			}
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000C8044 File Offset: 0x000C6244
		private void RecordPosition()
		{
			this.positionHistory.Add(base.transform.position);
			if ((float)this.positionHistory.Count * this.recordingFrequency > this.historyDuration)
			{
				this.positionHistory.RemoveAt(0);
			}
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x000C8084 File Offset: 0x000C6284
		public Vector3 GetPositionXSecondsAgo(float secondsAgo)
		{
			int num = (int)(secondsAgo / this.recordingFrequency);
			num = Mathf.Clamp(num, 0, this.positionHistory.Count - 1);
			return this.positionHistory[num];
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000C80BC File Offset: 0x000C62BC
		public void ClearHistory()
		{
			this.positionHistory.Clear();
		}

		// Token: 0x0400223C RID: 8764
		[Tooltip("Frequency (in seconds) to record the position.")]
		public float recordingFrequency = 1f;

		// Token: 0x0400223D RID: 8765
		[Tooltip("Duration (in seconds) to store the position history.")]
		public float historyDuration = 10f;

		// Token: 0x0400223E RID: 8766
		public List<Vector3> positionHistory = new List<Vector3>();

		// Token: 0x0400223F RID: 8767
		private float lastRecordTime;
	}
}
