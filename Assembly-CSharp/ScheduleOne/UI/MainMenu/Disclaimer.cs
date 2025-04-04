using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B0B RID: 2827
	public class Disclaimer : MonoBehaviour
	{
		// Token: 0x06004B7C RID: 19324 RVA: 0x0013CA30 File Offset: 0x0013AC30
		private void Awake()
		{
			if (Application.isEditor || Disclaimer.Shown)
			{
				base.gameObject.SetActive(false);
				return;
			}
			Disclaimer.Shown = true;
			this.Group.alpha = 1f;
			this.TextGroup.alpha = 0f;
			this.Fade();
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x0013CA84 File Offset: 0x0013AC84
		private void Fade()
		{
			base.StartCoroutine(this.<Fade>g__Fade|5_0());
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x0013CAA6 File Offset: 0x0013ACA6
		[CompilerGenerated]
		private IEnumerator <Fade>g__Fade|5_0()
		{
			while (this.TextGroup.alpha < 1f)
			{
				this.TextGroup.alpha += Time.deltaTime * 2f;
				yield return null;
			}
			for (float i = 0f; i < this.Duration; i += Time.deltaTime)
			{
				if (Input.GetKey(KeyCode.Space))
				{
					IL_FC:
					while (this.Group.alpha > 0f)
					{
						this.Group.alpha -= Time.deltaTime * 2f;
						yield return null;
					}
					base.gameObject.SetActive(false);
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}
			goto IL_FC;
		}

		// Token: 0x040038C3 RID: 14531
		public static bool Shown;

		// Token: 0x040038C4 RID: 14532
		public CanvasGroup Group;

		// Token: 0x040038C5 RID: 14533
		public CanvasGroup TextGroup;

		// Token: 0x040038C6 RID: 14534
		public float Duration = 3.8f;
	}
}
