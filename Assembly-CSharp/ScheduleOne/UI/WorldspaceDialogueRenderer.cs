using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A37 RID: 2615
	public class WorldspaceDialogueRenderer : MonoBehaviour
	{
		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06004677 RID: 18039 RVA: 0x00126CA8 File Offset: 0x00124EA8
		// (set) Token: 0x06004678 RID: 18040 RVA: 0x00126CB0 File Offset: 0x00124EB0
		public string ShownText { get; protected set; } = string.Empty;

		// Token: 0x06004679 RID: 18041 RVA: 0x00126CB9 File Offset: 0x00124EB9
		private void Awake()
		{
			this.localOffset = base.transform.localPosition;
			this.SetOpacity(0f);
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x00126CD8 File Offset: 0x00124ED8
		private void FixedUpdate()
		{
			if (this.ShownText == string.Empty)
			{
				if (this.CurrentOpacity != 0f)
				{
					this.SetOpacity(0f);
				}
				return;
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				if (this.CurrentOpacity != 0f)
				{
					this.SetOpacity(0f);
				}
				return;
			}
			if (Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) > this.MaxRange)
			{
				if (this.CurrentOpacity != 0f)
				{
					this.SetOpacity(0f);
				}
				return;
			}
			float num = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			if (num < this.MaxRange - 2f)
			{
				this.SetOpacity(1f);
			}
			else
			{
				this.SetOpacity(1f - (num - (this.MaxRange - 2f)) / 2f);
			}
			this.Text.text = this.ShownText;
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x00126DDD File Offset: 0x00124FDD
		private void LateUpdate()
		{
			if (this.CurrentOpacity > 0f)
			{
				this.UpdatePosition();
			}
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x00126DF4 File Offset: 0x00124FF4
		private void UpdatePosition()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			float num = this.BaseScale * this.Scale.Evaluate(Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) / this.MaxRange);
			this.Canvas.transform.localScale = new Vector3(num, num, num);
			this.Background.sizeDelta = new Vector2(this.Text.renderedWidth + this.Padding.x, this.Text.renderedHeight + this.Padding.y);
			this.Canvas.transform.LookAt(PlayerSingleton<PlayerCamera>.Instance.transform.position);
			base.transform.localPosition = this.localOffset;
			base.transform.position = base.transform.position + this.WorldSpaceOffset;
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x00126EEC File Offset: 0x001250EC
		public void ShowText(string text, float duration = 0f)
		{
			if (this.hideCoroutine != null)
			{
				base.StopCoroutine(this.hideCoroutine);
				this.hideCoroutine = null;
			}
			this.ShownText = text;
			if (this.ShownText != string.Empty)
			{
				this.Text.text = this.ShownText;
				this.Text.ForceMeshUpdate(false, false);
				this.UpdatePosition();
			}
			if (!this.Canvas.enabled && this.Anim != null)
			{
				this.Anim.Play();
			}
			if (duration > 0f)
			{
				this.hideCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<ShowText>g__Wait|22_0(duration));
			}
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x00126F97 File Offset: 0x00125197
		public void HideText()
		{
			if (this.hideCoroutine != null)
			{
				base.StopCoroutine(this.hideCoroutine);
				this.hideCoroutine = null;
			}
			this.ShownText = string.Empty;
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x00126FBF File Offset: 0x001251BF
		private void SetOpacity(float op)
		{
			this.CurrentOpacity = op;
			this.CanvasGroup.alpha = op;
			this.Canvas.enabled = (op > 0f);
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x00127026 File Offset: 0x00125226
		[CompilerGenerated]
		private IEnumerator <ShowText>g__Wait|22_0(float dur)
		{
			yield return new WaitForSeconds(dur);
			this.ShownText = string.Empty;
			this.hideCoroutine = null;
			yield break;
		}

		// Token: 0x0400342D RID: 13357
		private const float FadeDist = 2f;

		// Token: 0x0400342F RID: 13359
		[Header("Settings")]
		public float MaxRange = 10f;

		// Token: 0x04003430 RID: 13360
		public float BaseScale = 0.01f;

		// Token: 0x04003431 RID: 13361
		public AnimationCurve Scale;

		// Token: 0x04003432 RID: 13362
		public Vector2 Padding;

		// Token: 0x04003433 RID: 13363
		public Vector3 WorldSpaceOffset = Vector3.zero;

		// Token: 0x04003434 RID: 13364
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003435 RID: 13365
		public CanvasGroup CanvasGroup;

		// Token: 0x04003436 RID: 13366
		public RectTransform Background;

		// Token: 0x04003437 RID: 13367
		public TextMeshProUGUI Text;

		// Token: 0x04003438 RID: 13368
		public Animation Anim;

		// Token: 0x04003439 RID: 13369
		private Vector3 localOffset = Vector3.zero;

		// Token: 0x0400343A RID: 13370
		private float CurrentOpacity;

		// Token: 0x0400343B RID: 13371
		private Coroutine hideCoroutine;
	}
}
