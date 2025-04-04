using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B36 RID: 2870
	public class CharacterCreatorMenu : MonoBehaviour
	{
		// Token: 0x06004C67 RID: 19559 RVA: 0x00141BE4 File Offset: 0x0013FDE4
		public void Start()
		{
			CharacterCreatorMenu.Window[] windows = this.Windows;
			for (int i = 0; i < windows.Length; i++)
			{
				windows[i].Close();
			}
			this.OpenWindow(0);
		}

		// Token: 0x06004C68 RID: 19560 RVA: 0x00141C18 File Offset: 0x0013FE18
		public void OpenWindow(int index)
		{
			if (this.openWindow != null)
			{
				this.openWindow.Close();
			}
			this.openWindowIndex = index;
			this.openWindow = this.Windows[index];
			this.openWindow.Open();
			this.CategoryLabel.text = this.openWindow.Name;
			this.BackButton.interactable = (index > 0);
			this.NextButton.interactable = (index < this.Windows.Length - 1);
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x00141C95 File Offset: 0x0013FE95
		public void Back()
		{
			this.OpenWindow(this.openWindowIndex - 1);
		}

		// Token: 0x06004C6A RID: 19562 RVA: 0x00141CA5 File Offset: 0x0013FEA5
		public void Next()
		{
			this.OpenWindow(this.openWindowIndex + 1);
		}

		// Token: 0x040039BD RID: 14781
		public CharacterCreatorMenu.Window[] Windows;

		// Token: 0x040039BE RID: 14782
		[Header("References")]
		public TextMeshProUGUI CategoryLabel;

		// Token: 0x040039BF RID: 14783
		public Button BackButton;

		// Token: 0x040039C0 RID: 14784
		public Button NextButton;

		// Token: 0x040039C1 RID: 14785
		private int openWindowIndex;

		// Token: 0x040039C2 RID: 14786
		private CharacterCreatorMenu.Window openWindow;

		// Token: 0x02000B37 RID: 2871
		[Serializable]
		public class Window
		{
			// Token: 0x06004C6C RID: 19564 RVA: 0x00141CB5 File Offset: 0x0013FEB5
			public void Open()
			{
				this.Container.gameObject.SetActive(true);
			}

			// Token: 0x06004C6D RID: 19565 RVA: 0x00141CC8 File Offset: 0x0013FEC8
			public void Close()
			{
				this.Container.gameObject.SetActive(false);
			}

			// Token: 0x040039C3 RID: 14787
			public string Name;

			// Token: 0x040039C4 RID: 14788
			public RectTransform Container;
		}
	}
}
