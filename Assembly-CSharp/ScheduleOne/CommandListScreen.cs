using System;
using System.Collections.Generic;
using ScheduleOne.UI.MainMenu;
using TMPro;
using UnityEngine;

namespace ScheduleOne
{
	// Token: 0x02000272 RID: 626
	public class CommandListScreen : MainMenuScreen
	{
		// Token: 0x06000D19 RID: 3353 RVA: 0x0003A33C File Offset: 0x0003853C
		private void Start()
		{
			if (this.commandEntries.Count == 0)
			{
				foreach (Console.ConsoleCommand consoleCommand in Console.Commands)
				{
					RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.CommandEntryPrefab, this.CommandEntryContainer);
					rectTransform.Find("Command").GetComponent<TextMeshProUGUI>().text = consoleCommand.CommandWord;
					rectTransform.Find("Description").GetComponent<TextMeshProUGUI>().text = consoleCommand.CommandDescription;
					rectTransform.Find("Example").GetComponent<TextMeshProUGUI>().text = consoleCommand.ExampleUsage;
					this.commandEntries.Add(rectTransform);
				}
			}
			this.CommandEntryContainer.offsetMin = new Vector2(this.CommandEntryContainer.offsetMin.x, 0f);
			this.CommandEntryContainer.offsetMax = new Vector2(this.CommandEntryContainer.offsetMax.x, 0f);
		}

		// Token: 0x04000D9C RID: 3484
		public RectTransform CommandEntryContainer;

		// Token: 0x04000D9D RID: 3485
		public RectTransform CommandEntryPrefab;

		// Token: 0x04000D9E RID: 3486
		private List<RectTransform> commandEntries = new List<RectTransform>();
	}
}
