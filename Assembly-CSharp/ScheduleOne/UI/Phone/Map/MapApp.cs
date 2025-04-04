using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Map
{
	// Token: 0x02000AA2 RID: 2722
	public class MapApp : App<MapApp>
	{
		// Token: 0x06004946 RID: 18758 RVA: 0x0013281C File Offset: 0x00130A1C
		protected override void Start()
		{
			base.Start();
			this.BackgroundImage.sprite = (NetworkSingleton<GameManager>.Instance.IsTutorial ? this.TutorialMapSprite : this.MainMapSprite);
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x0013284C File Offset: 0x00130A4C
		public override void SetOpen(bool open)
		{
			base.SetOpen(open);
			if (NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("MapAppOpen", open.ToString(), false);
			}
			if (open)
			{
				if (!this.opened && !this.SkipFocusPlayer)
				{
					this.opened = true;
					Player.Local.PoI.UpdatePosition();
					this.FocusPosition(Player.Local.PoI.UI.anchoredPosition);
				}
				if (Player.Local != null && Player.Local.PoI.UI != null)
				{
					Player.Local.PoI.UI.GetComponentInChildren<Animation>().Play();
				}
			}
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x00132900 File Offset: 0x00130B00
		protected override void Update()
		{
			base.Update();
			if (base.isOpen)
			{
				GameInput.GetButton(GameInput.ButtonCode.Right);
				GameInput.GetButton(GameInput.ButtonCode.Left);
				GameInput.GetButton(GameInput.ButtonCode.Forward);
				GameInput.GetButton(GameInput.ButtonCode.Backward);
				float x = this.ContentRect.localScale.x;
				if (x >= this.LabelScrollMin)
				{
					this.LabelGroup.alpha = Mathf.Clamp01((x - this.LabelScrollMin) / (this.LabelScrollMax - this.LabelScrollMin));
					return;
				}
				this.LabelGroup.alpha = 0f;
			}
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x00132988 File Offset: 0x00130B88
		public void FocusPosition(Vector2 anchoredPosition)
		{
			this.ContentRect.pivot = new Vector2(0f, 1f);
			float num = 1.3f;
			Vector2 a = new Vector2(-this.ContentRect.sizeDelta.x / 2f, this.ContentRect.sizeDelta.y / 2f);
			a.x -= anchoredPosition.x;
			a.y -= anchoredPosition.y;
			this.ContentRect.localScale = new Vector3(num, num, num);
			this.ContentRect.anchoredPosition = a * num;
		}

		// Token: 0x040036A1 RID: 13985
		public const float KeyMoveSpeed = 1.25f;

		// Token: 0x040036A2 RID: 13986
		public RectTransform ContentRect;

		// Token: 0x040036A3 RID: 13987
		public RectTransform PoIContainer;

		// Token: 0x040036A4 RID: 13988
		public Scrollbar HorizontalScrollbar;

		// Token: 0x040036A5 RID: 13989
		public Scrollbar VerticalScrollbar;

		// Token: 0x040036A6 RID: 13990
		public Image BackgroundImage;

		// Token: 0x040036A7 RID: 13991
		public CanvasGroup LabelGroup;

		// Token: 0x040036A8 RID: 13992
		[Header("Settings")]
		public Sprite DemoMapSprite;

		// Token: 0x040036A9 RID: 13993
		public Sprite MainMapSprite;

		// Token: 0x040036AA RID: 13994
		public Sprite TutorialMapSprite;

		// Token: 0x040036AB RID: 13995
		public float LabelScrollMin = 1.2f;

		// Token: 0x040036AC RID: 13996
		public float LabelScrollMax = 1.5f;

		// Token: 0x040036AD RID: 13997
		[HideInInspector]
		public bool SkipFocusPlayer;

		// Token: 0x040036AE RID: 13998
		private Coroutine contentMoveRoutine;

		// Token: 0x040036AF RID: 13999
		private bool opened;
	}
}
