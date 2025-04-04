using System;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009EB RID: 2539
	public class MouseTooltip : Singleton<MouseTooltip>
	{
		// Token: 0x06004487 RID: 17543 RVA: 0x0011F0FF File Offset: 0x0011D2FF
		public void ShowTooltip(string text, Color col)
		{
			this.TooltipLabel.text = text;
			this.TooltipLabel.color = col;
			this.tooltipShownThisFrame = true;
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0011F120 File Offset: 0x0011D320
		public void ShowIcon(Sprite sprite, Color col)
		{
			this.IconImg.sprite = sprite;
			this.IconImg.color = col;
			this.iconShownThisFrame = true;
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0011F144 File Offset: 0x0011D344
		private void LateUpdate()
		{
			this.TooltipLabel.gameObject.SetActive(this.tooltipShownThisFrame);
			this.IconRect.gameObject.SetActive(this.iconShownThisFrame);
			this.IconRect.position = Input.mousePosition + this.IconOffset;
			if (this.iconShownThisFrame)
			{
				this.TooltipRect.position = Input.mousePosition + this.TooltipOffset_WithIcon;
			}
			else
			{
				this.TooltipRect.position = Input.mousePosition + this.TooltipOffset_NoIcon;
			}
			this.tooltipShownThisFrame = false;
			this.iconShownThisFrame = false;
		}

		// Token: 0x0400325D RID: 12893
		[Header("References")]
		public RectTransform IconRect;

		// Token: 0x0400325E RID: 12894
		public Image IconImg;

		// Token: 0x0400325F RID: 12895
		public RectTransform TooltipRect;

		// Token: 0x04003260 RID: 12896
		public TextMeshProUGUI TooltipLabel;

		// Token: 0x04003261 RID: 12897
		[Header("Settings")]
		public Vector3 TooltipOffset_NoIcon;

		// Token: 0x04003262 RID: 12898
		public Vector3 TooltipOffset_WithIcon;

		// Token: 0x04003263 RID: 12899
		public Vector3 IconOffset;

		// Token: 0x04003264 RID: 12900
		[Header("Colors")]
		public Color Color_Invalid;

		// Token: 0x04003265 RID: 12901
		[Header("Sprites")]
		public Sprite Sprite_Cross;

		// Token: 0x04003266 RID: 12902
		private bool tooltipShownThisFrame;

		// Token: 0x04003267 RID: 12903
		private bool iconShownThisFrame;
	}
}
