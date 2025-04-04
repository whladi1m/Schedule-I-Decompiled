using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B04 RID: 2820
	public class WorldspaceUIElement : MonoBehaviour
	{
		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06004B5B RID: 19291 RVA: 0x0013C499 File Offset: 0x0013A699
		// (set) Token: 0x06004B5C RID: 19292 RVA: 0x0013C4A1 File Offset: 0x0013A6A1
		public bool IsEnabled { get; protected set; }

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06004B5D RID: 19293 RVA: 0x0012C825 File Offset: 0x0012AA25
		public bool IsVisible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		// Token: 0x06004B5E RID: 19294 RVA: 0x0013C4AC File Offset: 0x0013A6AC
		public virtual void Show()
		{
			if (this == null || this.Container == null)
			{
				return;
			}
			if (base.gameObject == null)
			{
				return;
			}
			this.IsEnabled = true;
			base.gameObject.SetActive(true);
			this.SetScale(1f, null);
		}

		// Token: 0x06004B5F RID: 19295 RVA: 0x0013C500 File Offset: 0x0013A700
		public virtual void Hide(Action callback = null)
		{
			if (this == null || this.Container == null)
			{
				return;
			}
			this.IsEnabled = false;
			this.SetScale(0f, delegate
			{
				base.<Hide>g__Done|1();
			});
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x000F7B06 File Offset: 0x000F5D06
		public virtual void Destroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x0013C558 File Offset: 0x0013A758
		public void UpdatePosition(Vector3 worldSpacePosition)
		{
			if (this == null || this.Container == null)
			{
				return;
			}
			if (PlayerSingleton<PlayerCamera>.Instance.transform.InverseTransformPoint(worldSpacePosition).z > 0f)
			{
				this.RectTransform.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(worldSpacePosition);
				this.Container.gameObject.SetActive(true);
				return;
			}
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x0013C5D7 File Offset: 0x0013A7D7
		public virtual void SetInternalScale(float scale)
		{
			if (this == null || this.Container == null)
			{
				return;
			}
			this.Container.localScale = new Vector3(scale, scale, 1f);
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x0013C608 File Offset: 0x0013A808
		private void SetScale(float scale, Action callback)
		{
			WorldspaceUIElement.<>c__DisplayClass17_0 CS$<>8__locals1 = new WorldspaceUIElement.<>c__DisplayClass17_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.scale = scale;
			CS$<>8__locals1.callback = callback;
			if (this == null || this.Container == null)
			{
				return;
			}
			if (this.scaleRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.scaleRoutine);
			}
			if (!base.gameObject.activeInHierarchy)
			{
				this.RectTransform.localScale = new Vector3(CS$<>8__locals1.scale, CS$<>8__locals1.scale, 1f);
				if (CS$<>8__locals1.callback != null)
				{
					CS$<>8__locals1.callback();
				}
				return;
			}
			CS$<>8__locals1.startScale = this.RectTransform.localScale.x;
			CS$<>8__locals1.lerpTime = 0.1f / Mathf.Abs(CS$<>8__locals1.startScale - CS$<>8__locals1.scale);
			this.scaleRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetScale>g__Routine|0());
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void HoverStart()
		{
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void HoverEnd()
		{
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x0013C6EC File Offset: 0x0013A8EC
		public void SetAssignedNPC(NPC npc)
		{
			if (this == null || this.Container == null)
			{
				return;
			}
			this.AssignedWorkerDisplay.Set(npc);
		}

		// Token: 0x040038AD RID: 14509
		public const float TRANSITION_TIME = 0.1f;

		// Token: 0x040038AF RID: 14511
		[Header("References")]
		public RectTransform RectTransform;

		// Token: 0x040038B0 RID: 14512
		public RectTransform Container;

		// Token: 0x040038B1 RID: 14513
		public TextMeshProUGUI TitleLabel;

		// Token: 0x040038B2 RID: 14514
		public AssignedWorkerDisplay AssignedWorkerDisplay;

		// Token: 0x040038B3 RID: 14515
		private Coroutine scaleRoutine;
	}
}
