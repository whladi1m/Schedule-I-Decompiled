using System;

namespace ScheduleOne.AvatarFramework.Emotions
{
	// Token: 0x02000969 RID: 2409
	public class EmotionOverride
	{
		// Token: 0x0600417A RID: 16762 RVA: 0x00112985 File Offset: 0x00110B85
		public EmotionOverride(string emotion, string label, int priority)
		{
			this.Emotion = emotion;
			this.Label = label;
			this.Priority = priority;
		}

		// Token: 0x04002F6F RID: 12143
		public string Emotion;

		// Token: 0x04002F70 RID: 12144
		public string Label;

		// Token: 0x04002F71 RID: 12145
		public int Priority;
	}
}
