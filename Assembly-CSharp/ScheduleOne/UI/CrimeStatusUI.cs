using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009FF RID: 2559
	public class CrimeStatusUI : MonoBehaviour
	{
		// Token: 0x0600451A RID: 17690 RVA: 0x00121B38 File Offset: 0x0011FD38
		public void UpdateStatus()
		{
			float b = 0f;
			this.animateText = false;
			PlayerCrimeData.EPursuitLevel currentPursuitLevel = Player.Local.CrimeData.CurrentPursuitLevel;
			this.InvestigatingMask.gameObject.SetActive(currentPursuitLevel == PlayerCrimeData.EPursuitLevel.Investigating);
			this.UnderArrestMask.gameObject.SetActive(currentPursuitLevel == PlayerCrimeData.EPursuitLevel.Arresting);
			this.WantedMask.gameObject.SetActive(currentPursuitLevel == PlayerCrimeData.EPursuitLevel.NonLethal);
			this.WantedDeadMask.gameObject.SetActive(currentPursuitLevel == PlayerCrimeData.EPursuitLevel.Lethal);
			this.BodysearchLabel.SetActive(currentPursuitLevel == PlayerCrimeData.EPursuitLevel.None && Player.Local.CrimeData.BodySearchPending);
			if (currentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				b = 0.6f;
				if (Player.Local.CrimeData.TimeSinceSighted < 3f)
				{
					b = 1f;
					this.animateText = true;
					if (this.routine == null)
					{
						this.routine = base.StartCoroutine(this.Routine());
					}
				}
			}
			else if (Player.Local.CrimeData.BodySearchPending)
			{
				b = 1f;
			}
			float fillAmount = 1f - Mathf.Clamp01((Player.Local.CrimeData.TimeSinceSighted - 3f) / Player.Local.CrimeData.GetSearchTime());
			this.InvestigatingMask.fillAmount = fillAmount;
			this.UnderArrestMask.fillAmount = fillAmount;
			this.WantedMask.fillAmount = fillAmount;
			this.WantedDeadMask.fillAmount = fillAmount;
			this.CrimeStatusGroup.alpha = Mathf.Lerp(this.CrimeStatusGroup.alpha, b, Time.deltaTime);
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x00121CB3 File Offset: 0x0011FEB3
		private void OnDestroy()
		{
			if (this.routine != null && Singleton<CoroutineService>.InstanceExists)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.Routine());
			}
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x00121CD4 File Offset: 0x0011FED4
		private IEnumerator Routine()
		{
			this.CrimeStatusContainer.localScale = Vector3.one * 0.75f;
			for (;;)
			{
				if (!this.animateText)
				{
					yield return new WaitForEndOfFrame();
				}
				else
				{
					float lerpTime = 1.5f;
					float t = 0f;
					while (t < lerpTime)
					{
						t += Time.deltaTime;
						this.CrimeStatusContainer.localScale = Vector3.one * Mathf.Lerp(0.75f, 1f, (Mathf.Sin(t / lerpTime * 2f * 3.1415927f) + 1f) / 2f);
						yield return new WaitForEndOfFrame();
					}
				}
			}
			yield break;
		}

		// Token: 0x040032F4 RID: 13044
		public const float SmallTextSize = 0.75f;

		// Token: 0x040032F5 RID: 13045
		public const float LargeTextSize = 1f;

		// Token: 0x040032F6 RID: 13046
		[Header("References")]
		public RectTransform CrimeStatusContainer;

		// Token: 0x040032F7 RID: 13047
		public CanvasGroup CrimeStatusGroup;

		// Token: 0x040032F8 RID: 13048
		public GameObject BodysearchLabel;

		// Token: 0x040032F9 RID: 13049
		public Image InvestigatingMask;

		// Token: 0x040032FA RID: 13050
		public Image UnderArrestMask;

		// Token: 0x040032FB RID: 13051
		public Image WantedMask;

		// Token: 0x040032FC RID: 13052
		public Image WantedDeadMask;

		// Token: 0x040032FD RID: 13053
		public GameObject ArrestProgressContainer;

		// Token: 0x040032FE RID: 13054
		private bool animateText;

		// Token: 0x040032FF RID: 13055
		private Coroutine routine;
	}
}
