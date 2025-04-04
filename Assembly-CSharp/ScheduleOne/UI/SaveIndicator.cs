using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x02000A26 RID: 2598
	public class SaveIndicator : MonoBehaviour
	{
		// Token: 0x06004620 RID: 17952 RVA: 0x00125978 File Offset: 0x00123B78
		public void Awake()
		{
			this.Canvas.enabled = false;
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x00125986 File Offset: 0x00123B86
		public void Start()
		{
			Singleton<SaveManager>.Instance.onSaveStart.AddListener(new UnityAction(this.Display));
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x001259A3 File Offset: 0x00123BA3
		public void OnDestroy()
		{
			if (Singleton<SaveManager>.InstanceExists)
			{
				Singleton<SaveManager>.Instance.onSaveStart.RemoveListener(new UnityAction(this.Display));
			}
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x001259C7 File Offset: 0x00123BC7
		public void Display()
		{
			base.StartCoroutine(this.<Display>g__Routine|6_0());
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x001259D6 File Offset: 0x00123BD6
		[CompilerGenerated]
		private IEnumerator <Display>g__Routine|6_0()
		{
			this.Canvas.enabled = true;
			this.Icon.gameObject.SetActive(true);
			while (Singleton<SaveManager>.Instance.IsSaving)
			{
				this.Icon.Rotate(Vector3.forward, 360f * Time.unscaledDeltaTime);
				yield return new WaitForEndOfFrame();
			}
			this.Icon.gameObject.SetActive(false);
			this.Anim.Play();
			yield return new WaitForSecondsRealtime(5f);
			this.Canvas.enabled = false;
			yield break;
		}

		// Token: 0x040033DA RID: 13274
		public Canvas Canvas;

		// Token: 0x040033DB RID: 13275
		public RectTransform Icon;

		// Token: 0x040033DC RID: 13276
		public Animation Anim;
	}
}
