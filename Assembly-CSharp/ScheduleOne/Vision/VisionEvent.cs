using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vision
{
	// Token: 0x02000283 RID: 643
	public class VisionEvent
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x0003BFC2 File Offset: 0x0003A1C2
		// (set) Token: 0x06000D6C RID: 3436 RVA: 0x0003BFCA File Offset: 0x0003A1CA
		public Player Target { get; protected set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x0003BFD3 File Offset: 0x0003A1D3
		// (set) Token: 0x06000D6E RID: 3438 RVA: 0x0003BFDB File Offset: 0x0003A1DB
		public PlayerVisualState.VisualState State { get; protected set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x0003BFE4 File Offset: 0x0003A1E4
		// (set) Token: 0x06000D70 RID: 3440 RVA: 0x0003BFEC File Offset: 0x0003A1EC
		public VisionCone Owner { get; protected set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x0003BFF5 File Offset: 0x0003A1F5
		// (set) Token: 0x06000D72 RID: 3442 RVA: 0x0003BFFD File Offset: 0x0003A1FD
		public float FullNoticeTime { get; protected set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x0003C006 File Offset: 0x0003A206
		public float NormalizedNoticeLevel
		{
			get
			{
				return this.currentNoticeTime / this.FullNoticeTime;
			}
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0003C018 File Offset: 0x0003A218
		public VisionEvent(VisionCone _owner, Player _target, PlayerVisualState.VisualState _state, float _noticeTime)
		{
			this.Owner = _owner;
			this.Target = _target;
			this.State = _state;
			this.FullNoticeTime = _noticeTime;
			PlayerVisualState.VisualState state = this.State;
			state.stateDestroyed = (Action)Delegate.Combine(state.stateDestroyed, new Action(this.EndEvent));
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0003C070 File Offset: 0x0003A270
		public void UpdateEvent(float visionDeltaThisFrame, float tickTime)
		{
			float normalizedNoticeLevel = this.NormalizedNoticeLevel;
			if (visionDeltaThisFrame > 0f)
			{
				this.timeSinceSighted = 0f;
			}
			else
			{
				this.timeSinceSighted += tickTime;
			}
			if (visionDeltaThisFrame > 0f)
			{
				this.currentNoticeTime += visionDeltaThisFrame * (this.Owner.Attentiveness * VisionCone.UniversalAttentivenessScale) * tickTime;
			}
			else if (this.timeSinceSighted > 1f * (this.Owner.Memory * VisionCone.UniversalMemoryScale))
			{
				this.currentNoticeTime -= tickTime / (this.Owner.Memory * VisionCone.UniversalMemoryScale);
			}
			this.currentNoticeTime = Mathf.Clamp(this.currentNoticeTime, 0f, this.FullNoticeTime);
			if (this.Target.Visibility.HighestVisionEvent == null || this.NormalizedNoticeLevel > this.Target.Visibility.HighestVisionEvent.NormalizedNoticeLevel)
			{
				this.Target.Visibility.HighestVisionEvent = this;
			}
			if (this.NormalizedNoticeLevel <= 0f && normalizedNoticeLevel > 0f)
			{
				this.EndEvent();
			}
			if (this.NormalizedNoticeLevel >= 0.5f && normalizedNoticeLevel < 0.5f)
			{
				if (this.Target.Visibility.HighestVisionEvent == this)
				{
					this.Target.Visibility.HighestVisionEvent = null;
				}
				this.Owner.EventHalfNoticed(this);
			}
			if (this.NormalizedNoticeLevel >= 1f && normalizedNoticeLevel < 1f)
			{
				if (this.Target.Visibility.HighestVisionEvent == this)
				{
					this.Target.Visibility.HighestVisionEvent = null;
				}
				this.Owner.EventFullyNoticed(this);
			}
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0003C213 File Offset: 0x0003A413
		public void EndEvent()
		{
			if (this.Target.Visibility.HighestVisionEvent == this)
			{
				this.Target.Visibility.HighestVisionEvent = null;
			}
			this.Owner.EventReachedZero(this);
		}

		// Token: 0x04000E04 RID: 3588
		private const float NOTICE_DROP_THRESHOLD = 1f;

		// Token: 0x04000E09 RID: 3593
		private float timeSinceSighted;

		// Token: 0x04000E0A RID: 3594
		private float currentNoticeTime;
	}
}
