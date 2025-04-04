using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Input;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A30 RID: 2608
	public class TrashBagCanvas : Singleton<TrashBagCanvas>
	{
		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x0600464A RID: 17994 RVA: 0x00126495 File Offset: 0x00124695
		// (set) Token: 0x0600464B RID: 17995 RVA: 0x0012649D File Offset: 0x0012469D
		public bool IsOpen { get; private set; }

		// Token: 0x0600464C RID: 17996 RVA: 0x001264A6 File Offset: 0x001246A6
		public void Open()
		{
			this.IsOpen = true;
			this.Canvas.enabled = true;
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x001264BB File Offset: 0x001246BB
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
		}

		// Token: 0x0400340E RID: 13326
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400340F RID: 13327
		public InputPrompt InputPrompt;
	}
}
