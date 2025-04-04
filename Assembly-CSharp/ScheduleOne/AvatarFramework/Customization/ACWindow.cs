using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x0200098B RID: 2443
	public class ACWindow : MonoBehaviour
	{
		// Token: 0x0600422F RID: 16943 RVA: 0x00115950 File Offset: 0x00113B50
		private void Start()
		{
			this.TitleText.text = this.WindowTitle;
			if (this.Predecessor != null)
			{
				this.BackButton.onClick.AddListener(new UnityAction(this.Close));
				this.BackButton.gameObject.SetActive(true);
			}
			else
			{
				this.BackButton.gameObject.SetActive(false);
			}
			if (this.Predecessor != null)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x000BEE6A File Offset: 0x000BD06A
		public void Open()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x001159D6 File Offset: 0x00113BD6
		public void Close()
		{
			base.gameObject.SetActive(false);
			if (this.Predecessor != null)
			{
				this.Predecessor.Open();
			}
		}

		// Token: 0x04003025 RID: 12325
		[Header("Settings")]
		public string WindowTitle;

		// Token: 0x04003026 RID: 12326
		public ACWindow Predecessor;

		// Token: 0x04003027 RID: 12327
		[Header("References")]
		public TextMeshProUGUI TitleText;

		// Token: 0x04003028 RID: 12328
		public Button BackButton;
	}
}
