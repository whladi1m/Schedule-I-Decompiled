using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A36 RID: 2614
	public class VersionText : MonoBehaviour
	{
		// Token: 0x06004675 RID: 18037 RVA: 0x00126C8C File Offset: 0x00124E8C
		private void Awake()
		{
			base.GetComponent<TextMeshProUGUI>().text = "v" + Application.version;
		}
	}
}
