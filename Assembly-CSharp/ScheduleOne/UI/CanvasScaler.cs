using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009AA RID: 2474
	[RequireComponent(typeof(CanvasScaler))]
	public class CanvasScaler : MonoBehaviour
	{
		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x060042D1 RID: 17105 RVA: 0x001180BA File Offset: 0x001162BA
		public static float NormalizedCanvasScaleFactor
		{
			get
			{
				return Mathf.InverseLerp(0.7f, 1.4f, CanvasScaler.CanvasScaleFactor);
			}
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x001180D0 File Offset: 0x001162D0
		public void Awake()
		{
			this.canvasScaler = base.GetComponent<CanvasScaler>();
			this.referenceResolution = this.canvasScaler.referenceResolution;
			CanvasScaler.OnCanvasScaleFactorChanged = (Action)Delegate.Combine(CanvasScaler.OnCanvasScaleFactorChanged, new Action(this.RefreshScale));
			this.RefreshScale();
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x00118120 File Offset: 0x00116320
		private void OnDestroy()
		{
			CanvasScaler.OnCanvasScaleFactorChanged = (Action)Delegate.Remove(CanvasScaler.OnCanvasScaleFactorChanged, new Action(this.RefreshScale));
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x00118142 File Offset: 0x00116342
		private void RefreshScale()
		{
			this.canvasScaler.referenceResolution = this.referenceResolution / CanvasScaler.CanvasScaleFactor / this.ScaleMultiplier;
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x0011816A File Offset: 0x0011636A
		public static void SetScaleFactor(float scaleFactor)
		{
			scaleFactor = Mathf.Clamp(scaleFactor, 0.7f, 1.4f);
			CanvasScaler.CanvasScaleFactor = scaleFactor;
			Action onCanvasScaleFactorChanged = CanvasScaler.OnCanvasScaleFactorChanged;
			if (onCanvasScaleFactorChanged == null)
			{
				return;
			}
			onCanvasScaleFactorChanged();
		}

		// Token: 0x040030C7 RID: 12487
		public static float CanvasScaleFactor = 1f;

		// Token: 0x040030C8 RID: 12488
		public static Action OnCanvasScaleFactorChanged;

		// Token: 0x040030C9 RID: 12489
		public float ScaleMultiplier = 1f;

		// Token: 0x040030CA RID: 12490
		private Vector2 referenceResolution = new Vector2(1920f, 1080f);

		// Token: 0x040030CB RID: 12491
		private CanvasScaler canvasScaler;
	}
}
