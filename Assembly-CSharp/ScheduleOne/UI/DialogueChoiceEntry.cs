using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009BD RID: 2493
	[Serializable]
	public class DialogueChoiceEntry
	{
		// Token: 0x0400313F RID: 12607
		public GameObject gameObject;

		// Token: 0x04003140 RID: 12608
		public TextMeshProUGUI text;

		// Token: 0x04003141 RID: 12609
		public Button button;

		// Token: 0x04003142 RID: 12610
		public GameObject notPossibleGameObject;

		// Token: 0x04003143 RID: 12611
		public TextMeshProUGUI notPossibleText;

		// Token: 0x04003144 RID: 12612
		public CanvasGroup canvasGroup;
	}
}
