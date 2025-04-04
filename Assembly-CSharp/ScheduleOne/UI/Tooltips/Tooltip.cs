using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Tooltips
{
	// Token: 0x02000A3E RID: 2622
	public class Tooltip : MonoBehaviour
	{
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x060046A3 RID: 18083 RVA: 0x001279C8 File Offset: 0x00125BC8
		public Vector3 labelPosition
		{
			get
			{
				if (this.isWorldspace)
				{
					return RectTransformUtility.WorldToScreenPoint(Singleton<GameplayMenu>.Instance.OverlayCamera, this.rect.position);
				}
				return this.rect.position + new Vector3(this.labelOffset.x, this.labelOffset.y, 0f);
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x060046A4 RID: 18084 RVA: 0x00127A2D File Offset: 0x00125C2D
		// (set) Token: 0x060046A5 RID: 18085 RVA: 0x00127A35 File Offset: 0x00125C35
		public bool isWorldspace { get; private set; }

		// Token: 0x060046A6 RID: 18086 RVA: 0x00127A40 File Offset: 0x00125C40
		protected virtual void Awake()
		{
			this.rect = base.GetComponent<RectTransform>();
			if (base.GetComponentInParent<GraphicRaycaster>() == null)
			{
				Console.LogWarning("Tooltip has not parent GraphicRaycaster! Tooltip won't ever be activated", null);
			}
			this.canvas = base.GetComponentInParent<Canvas>();
			if (this.canvas != null)
			{
				this.isWorldspace = (this.canvas.renderMode == RenderMode.WorldSpace);
			}
		}

		// Token: 0x04003460 RID: 13408
		[Header("Settings")]
		public string text;

		// Token: 0x04003461 RID: 13409
		public Vector2 labelOffset;

		// Token: 0x04003462 RID: 13410
		private RectTransform rect;

		// Token: 0x04003464 RID: 13412
		private Canvas canvas;
	}
}
