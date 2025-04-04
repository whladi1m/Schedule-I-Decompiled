using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000838 RID: 2104
	public class DocumentOpener : MonoBehaviour
	{
		// Token: 0x060039C2 RID: 14786 RVA: 0x000F40E3 File Offset: 0x000F22E3
		public void Open()
		{
			Singleton<DocumentViewer>.Instance.Open(this.DocumentName);
		}

		// Token: 0x040029BA RID: 10682
		public string DocumentName;
	}
}
