using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000AB2 RID: 2738
	public class MessageBubble : MonoBehaviour
	{
		// Token: 0x060049BE RID: 18878 RVA: 0x00134958 File Offset: 0x00132B58
		public void SetupBubble(string _text, MessageBubble.Alignment _alignment, bool alignCenter = false)
		{
			this.alignment = _alignment;
			this.text = _text;
			this.alignTextCenter = alignCenter;
			ColorBlock colors = this.button.colors;
			if (this.alignment == MessageBubble.Alignment.Left)
			{
				this.container.anchorMin = new Vector2(0f, 1f);
				this.container.anchorMax = new Vector2(0f, 1f);
				colors.normalColor = MessageBubble.backgroundColor_Left;
				colors.disabledColor = MessageBubble.backgroundColor_Left;
				this.content.color = MessageBubble.textColor_Left;
			}
			else if (this.alignment == MessageBubble.Alignment.Right)
			{
				this.container.anchorMin = new Vector2(1f, 1f);
				this.container.anchorMax = new Vector2(1f, 1f);
				colors.normalColor = MessageBubble.backgroundColor_Right;
				colors.disabledColor = MessageBubble.backgroundColor_Right;
				this.content.color = MessageBubble.textColor_Right;
			}
			else
			{
				this.container.anchorMin = new Vector2(0.5f, 1f);
				this.container.anchorMax = new Vector2(0.5f, 1f);
				colors.normalColor = MessageBubble.backgroundColor_Right;
				colors.disabledColor = MessageBubble.backgroundColor_Right;
				this.content.color = MessageBubble.textColor_Right;
			}
			this.button.colors = colors;
			this.RefreshDisplayedText();
			this.RefreshTriangle();
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x00134AF8 File Offset: 0x00132CF8
		protected virtual void Update()
		{
			if (this.text != this.displayedText)
			{
				this.RefreshDisplayedText();
			}
			if (this.showTriangle != this.triangleShown)
			{
				this.RefreshTriangle();
			}
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x00134B28 File Offset: 0x00132D28
		public virtual void RefreshDisplayedText()
		{
			this.displayedText = this.text;
			this.content.text = this.displayedText;
			if (this.alignTextCenter)
			{
				this.content.alignment = TextAnchor.UpperCenter;
			}
			else
			{
				this.content.alignment = TextAnchor.UpperLeft;
			}
			RectTransform component = base.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(Mathf.Clamp(this.content.preferredWidth + 50f, this.bubble_MinWidth, this.bubble_MaxWidth), 75f);
			this.height = Mathf.Clamp(this.content.preferredHeight + 25f, 75f, float.MaxValue);
			component.sizeDelta = new Vector2(component.sizeDelta.x, this.height);
			float num = 1f;
			if (this.alignment == MessageBubble.Alignment.Right)
			{
				num = -1f;
			}
			else if (this.alignment == MessageBubble.Alignment.Center)
			{
				num = 0f;
			}
			if (this.autosetPosition)
			{
				component.anchoredPosition = new Vector2((component.sizeDelta.x / 2f + 25f) * num, -this.height / 2f);
			}
		}

		// Token: 0x060049C1 RID: 18881 RVA: 0x00134C50 File Offset: 0x00132E50
		protected virtual void RefreshTriangle()
		{
			this.triangleShown = this.showTriangle;
			this.triangle_Left.gameObject.SetActive(false);
			this.triangle_Right.gameObject.SetActive(false);
			if (this.showTriangle)
			{
				this.triangle_Left.color = this.button.colors.normalColor;
				this.triangle_Right.color = this.button.colors.normalColor;
				if (this.alignment == MessageBubble.Alignment.Left)
				{
					this.triangle_Left.gameObject.SetActive(true);
					return;
				}
				this.triangle_Right.gameObject.SetActive(true);
			}
		}

		// Token: 0x0400371C RID: 14108
		[Header("Settings")]
		public string text = string.Empty;

		// Token: 0x0400371D RID: 14109
		public MessageBubble.Alignment alignment = MessageBubble.Alignment.Left;

		// Token: 0x0400371E RID: 14110
		public bool showTriangle;

		// Token: 0x0400371F RID: 14111
		public float bubble_MinWidth = 75f;

		// Token: 0x04003720 RID: 14112
		public float bubble_MaxWidth = 500f;

		// Token: 0x04003721 RID: 14113
		public bool alignTextCenter;

		// Token: 0x04003722 RID: 14114
		public bool autosetPosition = true;

		// Token: 0x04003723 RID: 14115
		private string displayedText = string.Empty;

		// Token: 0x04003724 RID: 14116
		private bool triangleShown;

		// Token: 0x04003725 RID: 14117
		[Header("References")]
		public RectTransform container;

		// Token: 0x04003726 RID: 14118
		[SerializeField]
		protected Image bubble;

		// Token: 0x04003727 RID: 14119
		[SerializeField]
		protected Text content;

		// Token: 0x04003728 RID: 14120
		[SerializeField]
		protected Image triangle_Left;

		// Token: 0x04003729 RID: 14121
		[SerializeField]
		protected Image triangle_Right;

		// Token: 0x0400372A RID: 14122
		public Button button;

		// Token: 0x0400372B RID: 14123
		public float height;

		// Token: 0x0400372C RID: 14124
		public float spacingAbove;

		// Token: 0x0400372D RID: 14125
		public static Color32 backgroundColor_Left = new Color32(225, 225, 225, byte.MaxValue);

		// Token: 0x0400372E RID: 14126
		public static Color32 textColor_Left = new Color32(50, 50, 50, byte.MaxValue);

		// Token: 0x0400372F RID: 14127
		public static Color32 backgroundColor_Right = new Color32(75, 175, 225, byte.MaxValue);

		// Token: 0x04003730 RID: 14128
		public static Color32 textColor_Right = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04003731 RID: 14129
		public static float baseBubbleSpacing = 5f;

		// Token: 0x02000AB3 RID: 2739
		public enum Alignment
		{
			// Token: 0x04003733 RID: 14131
			Center,
			// Token: 0x04003734 RID: 14132
			Left,
			// Token: 0x04003735 RID: 14133
			Right
		}
	}
}
