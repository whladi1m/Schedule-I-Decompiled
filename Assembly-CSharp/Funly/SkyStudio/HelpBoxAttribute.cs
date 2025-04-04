using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E6 RID: 486
	public class HelpBoxAttribute : PropertyAttribute
	{
		// Token: 0x06000ACE RID: 2766 RVA: 0x0002FDCF File Offset: 0x0002DFCF
		public HelpBoxAttribute(string text, HelpBoxMessageType messageType = HelpBoxMessageType.None)
		{
			this.text = text;
			this.messageType = messageType;
		}

		// Token: 0x04000BAF RID: 2991
		public string text;

		// Token: 0x04000BB0 RID: 2992
		public HelpBoxMessageType messageType;
	}
}
