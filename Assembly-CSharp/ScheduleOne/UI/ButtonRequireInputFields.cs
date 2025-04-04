using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A2A RID: 2602
	public class ButtonRequireInputFields : MonoBehaviour
	{
		// Token: 0x0600463D RID: 17981 RVA: 0x00125FAC File Offset: 0x001241AC
		public void Update()
		{
			this.Button.interactable = true;
			if (this.Dropdown != null && this.Dropdown.value == 0)
			{
				this.Button.interactable = false;
			}
			foreach (ButtonRequireInputFields.Input input in this.Inputs)
			{
				if (string.IsNullOrEmpty(input.InputField.text))
				{
					input.ErrorMessage.gameObject.SetActive(true);
					this.Button.interactable = false;
				}
				else
				{
					input.ErrorMessage.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x040033F0 RID: 13296
		public List<ButtonRequireInputFields.Input> Inputs;

		// Token: 0x040033F1 RID: 13297
		public TMP_Dropdown Dropdown;

		// Token: 0x040033F2 RID: 13298
		public Button Button;

		// Token: 0x02000A2B RID: 2603
		[Serializable]
		public class Input
		{
			// Token: 0x040033F3 RID: 13299
			public TMP_InputField InputField;

			// Token: 0x040033F4 RID: 13300
			public RectTransform ErrorMessage;
		}
	}
}
