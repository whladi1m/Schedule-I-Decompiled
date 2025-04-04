using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.GameTime
{
	// Token: 0x020002B1 RID: 689
	public class TutorialTimeController : MonoBehaviour
	{
		// Token: 0x06000EC8 RID: 3784 RVA: 0x0004161B File Offset: 0x0003F81B
		private void Awake()
		{
			TimeManager.onSleepStart = (Action)Delegate.Combine(TimeManager.onSleepStart, new Action(this.IncrementKeyframe));
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0004163D File Offset: 0x0003F83D
		private void OnDestroy()
		{
			TimeManager.onSleepStart = (Action)Delegate.Remove(TimeManager.onSleepStart, new Action(this.IncrementKeyframe));
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00041660 File Offset: 0x0003F860
		private void Update()
		{
			if (this.disabled)
			{
				return;
			}
			TutorialTimeController.KeyFrame keyFrame = this.KeyFrames[this.currentKeyFrameIndex];
			float time = Mathf.Clamp01(Mathf.InverseLerp((float)this.GetCurrentKeyFrameStart(), (float)keyFrame.Time, (float)NetworkSingleton<TimeManager>.Instance.CurrentTime));
			float timeProgressionMultiplier = this.TimeProgressionCurve.Evaluate(time) * keyFrame.SpeedMultiplier;
			NetworkSingleton<TimeManager>.Instance.TimeProgressionMultiplier = timeProgressionMultiplier;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x000416CB File Offset: 0x0003F8CB
		private int GetCurrentKeyFrameStart()
		{
			if (this.currentKeyFrameIndex > 0)
			{
				return this.KeyFrames[this.currentKeyFrameIndex - 1].Time;
			}
			return NetworkSingleton<TimeManager>.Instance.DefaultTime;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x000416FC File Offset: 0x0003F8FC
		[Button]
		public void IncrementKeyframe()
		{
			Console.Log("Incrementing keyframe to " + (this.currentKeyFrameIndex + 1).ToString(), null);
			this.currentKeyFrameIndex = Mathf.Clamp(this.currentKeyFrameIndex + 1, 0, this.KeyFrames.Length - 1);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00041747 File Offset: 0x0003F947
		public void Disable()
		{
			NetworkSingleton<TimeManager>.Instance.TimeProgressionMultiplier = 1f;
			base.enabled = false;
			this.disabled = true;
		}

		// Token: 0x04000F3A RID: 3898
		public AnimationCurve TimeProgressionCurve;

		// Token: 0x04000F3B RID: 3899
		public TutorialTimeController.KeyFrame[] KeyFrames;

		// Token: 0x04000F3C RID: 3900
		[SerializeField]
		private int currentKeyFrameIndex;

		// Token: 0x04000F3D RID: 3901
		private bool disabled;

		// Token: 0x020002B2 RID: 690
		[Serializable]
		public struct KeyFrame
		{
			// Token: 0x04000F3E RID: 3902
			public int Time;

			// Token: 0x04000F3F RID: 3903
			public float SpeedMultiplier;

			// Token: 0x04000F40 RID: 3904
			public string Note;
		}
	}
}
