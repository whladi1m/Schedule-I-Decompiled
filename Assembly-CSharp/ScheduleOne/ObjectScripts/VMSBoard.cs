using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B68 RID: 2920
	public class VMSBoard : MonoBehaviour
	{
		// Token: 0x06004DCF RID: 19919 RVA: 0x0014843C File Offset: 0x0014663C
		public void SetText(string text, Color col)
		{
			this.Label.text = text;
			this.Label.color = col;
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x00148456 File Offset: 0x00146656
		public void SetText(string text)
		{
			this.SetText(text, new Color32(byte.MaxValue, 215, 50, byte.MaxValue));
		}

		// Token: 0x04003AE6 RID: 15078
		[Header("References")]
		public TextMeshProUGUI Label;
	}
}
