using System;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006F6 RID: 1782
	[Serializable]
	public class InputSettings
	{
		// Token: 0x04002288 RID: 8840
		public float MouseSensitivity;

		// Token: 0x04002289 RID: 8841
		public bool InvertMouse;

		// Token: 0x0400228A RID: 8842
		public InputSettings.EActionMode SprintMode;

		// Token: 0x0400228B RID: 8843
		public string BindingOverrides;

		// Token: 0x020006F7 RID: 1783
		public enum EActionMode
		{
			// Token: 0x0400228D RID: 8845
			Press,
			// Token: 0x0400228E RID: 8846
			Hold
		}
	}
}
