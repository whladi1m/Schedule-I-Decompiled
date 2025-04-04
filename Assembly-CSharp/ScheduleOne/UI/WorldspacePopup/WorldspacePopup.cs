using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ScheduleOne.UI.WorldspacePopup
{
	// Token: 0x02000A39 RID: 2617
	public class WorldspacePopup : MonoBehaviour
	{
		// Token: 0x06004688 RID: 18056 RVA: 0x001270B3 File Offset: 0x001252B3
		private void OnEnable()
		{
			if (!WorldspacePopup.ActivePopups.Contains(this))
			{
				WorldspacePopup.ActivePopups.Add(this);
			}
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x001270CD File Offset: 0x001252CD
		private void OnDisable()
		{
			WorldspacePopup.ActivePopups.Remove(this);
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x001270DC File Offset: 0x001252DC
		public WorldspacePopupUI CreateUI(RectTransform parent)
		{
			WorldspacePopupUI newUI = UnityEngine.Object.Instantiate<WorldspacePopupUI>(this.UIPrefab, parent);
			newUI.Popup = this;
			newUI.SetFill(this.CurrentFillLevel);
			this.UIs.Add(newUI);
			newUI.onDestroyed.AddListener(delegate()
			{
				this.UIs.Remove(newUI);
			});
			return newUI;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x00127158 File Offset: 0x00125358
		private void LateUpdate()
		{
			foreach (WorldspacePopupUI worldspacePopupUI in this.UIs)
			{
				worldspacePopupUI.SetFill(this.CurrentFillLevel);
			}
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x001271B0 File Offset: 0x001253B0
		public void Popup()
		{
			if (this.popupCoroutine != null)
			{
				base.StopCoroutine(this.popupCoroutine);
			}
			this.popupCoroutine = base.StartCoroutine(this.<Popup>g__PopupCoroutine|18_0());
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x00127231 File Offset: 0x00125431
		[CompilerGenerated]
		private IEnumerator <Popup>g__PopupCoroutine|18_0()
		{
			base.enabled = true;
			this.SizeMultiplier = 0f;
			float lerpTime = 0.25f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.SizeMultiplier = i / lerpTime;
				yield return new WaitForEndOfFrame();
			}
			this.SizeMultiplier = 1f;
			yield return new WaitForSeconds(0.6f);
			base.enabled = false;
			this.popupCoroutine = null;
			yield break;
		}

		// Token: 0x04003440 RID: 13376
		public static List<WorldspacePopup> ActivePopups = new List<WorldspacePopup>();

		// Token: 0x04003441 RID: 13377
		[Range(0f, 1f)]
		public float CurrentFillLevel = 1f;

		// Token: 0x04003442 RID: 13378
		[Header("Settings")]
		public WorldspacePopupUI UIPrefab;

		// Token: 0x04003443 RID: 13379
		public bool DisplayOnHUD = true;

		// Token: 0x04003444 RID: 13380
		public bool ScaleWithDistance = true;

		// Token: 0x04003445 RID: 13381
		public Vector3 WorldspaceOffset;

		// Token: 0x04003446 RID: 13382
		public float Range = 50f;

		// Token: 0x04003447 RID: 13383
		public float SizeMultiplier = 1f;

		// Token: 0x04003448 RID: 13384
		[HideInInspector]
		public WorldspacePopupUI WorldspaceUI;

		// Token: 0x04003449 RID: 13385
		[HideInInspector]
		public RectTransform HUDUI;

		// Token: 0x0400344A RID: 13386
		[HideInInspector]
		public WorldspacePopupUI HUDUIIcon;

		// Token: 0x0400344B RID: 13387
		[HideInInspector]
		public CanvasGroup HUDUICanvasGroup;

		// Token: 0x0400344C RID: 13388
		private List<WorldspacePopupUI> UIs = new List<WorldspacePopupUI>();

		// Token: 0x0400344D RID: 13389
		private Coroutine popupCoroutine;
	}
}
