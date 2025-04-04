using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x0200086C RID: 2156
	public class SoilChunk : Clickable
	{
		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06003A7D RID: 14973 RVA: 0x000F63CD File Offset: 0x000F45CD
		// (set) Token: 0x06003A7E RID: 14974 RVA: 0x000F63D5 File Offset: 0x000F45D5
		public float CurrentLerp { get; protected set; }

		// Token: 0x06003A7F RID: 14975 RVA: 0x000F63DE File Offset: 0x000F45DE
		protected virtual void Awake()
		{
			this.localPos_Start = base.transform.localPosition;
			this.localEulerAngles_Start = base.transform.localEulerAngles;
			this.localScale_Start = base.transform.localScale;
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x000F6414 File Offset: 0x000F4614
		public void SetLerpedTransform(float _lerp)
		{
			this.CurrentLerp = Mathf.Clamp(_lerp, 0f, 1f);
			base.transform.localPosition = Vector3.Lerp(this.localPos_Start, this.EndTransform.localPosition, this.CurrentLerp);
			base.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(this.localEulerAngles_Start), Quaternion.Euler(this.EndTransform.localEulerAngles), this.CurrentLerp);
			base.transform.localScale = Vector3.Lerp(this.localScale_Start, this.EndTransform.localScale, this.CurrentLerp);
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x000F64B6 File Offset: 0x000F46B6
		public override void StartClick(RaycastHit hit)
		{
			base.StartClick(hit);
			this.ClickableEnabled = false;
			this.StopLerp();
			this.lerpRoutine = base.StartCoroutine(this.<StartClick>g__Lerp|12_0());
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x000F64DE File Offset: 0x000F46DE
		public void StopLerp()
		{
			if (this.lerpRoutine != null)
			{
				base.StopCoroutine(this.lerpRoutine);
			}
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x000F6507 File Offset: 0x000F4707
		[CompilerGenerated]
		private IEnumerator <StartClick>g__Lerp|12_0()
		{
			for (float i = 0f; i < this.LerpTime; i += Time.deltaTime)
			{
				this.SetLerpedTransform(Mathf.Lerp(0f, 1f, i / this.LerpTime));
				yield return new WaitForEndOfFrame();
			}
			this.SetLerpedTransform(1f);
			this.lerpRoutine = null;
			yield break;
		}

		// Token: 0x04002A5C RID: 10844
		public Transform EndTransform;

		// Token: 0x04002A5D RID: 10845
		public float LerpTime = 0.4f;

		// Token: 0x04002A5E RID: 10846
		private Vector3 localPos_Start;

		// Token: 0x04002A5F RID: 10847
		private Vector3 localEulerAngles_Start;

		// Token: 0x04002A60 RID: 10848
		private Vector3 localScale_Start;

		// Token: 0x04002A61 RID: 10849
		private Coroutine lerpRoutine;
	}
}
