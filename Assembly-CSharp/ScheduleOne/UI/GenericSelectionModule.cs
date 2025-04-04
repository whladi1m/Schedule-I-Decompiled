using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009D0 RID: 2512
	public class GenericSelectionModule : Singleton<GenericSelectionModule>
	{
		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x060043DB RID: 17371 RVA: 0x0011C913 File Offset: 0x0011AB13
		// (set) Token: 0x060043DC RID: 17372 RVA: 0x0011C91B File Offset: 0x0011AB1B
		public bool isOpen { get; protected set; }

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x060043DD RID: 17373 RVA: 0x0011C924 File Offset: 0x0011AB24
		// (set) Token: 0x060043DE RID: 17374 RVA: 0x0011C92C File Offset: 0x0011AB2C
		[HideInInspector]
		public int ChosenOptionIndex { get; protected set; } = -1;

		// Token: 0x060043DF RID: 17375 RVA: 0x0011C935 File Offset: 0x0011AB35
		protected override void Awake()
		{
			base.Awake();
			this.Close();
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x0011C943 File Offset: 0x0011AB43
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 50);
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x0011C95E File Offset: 0x0011AB5E
		private void Exit(ExitAction action)
		{
			if (!this.isOpen)
			{
				return;
			}
			if (action.used)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.Cancel();
			}
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x0011C988 File Offset: 0x0011AB88
		public void Open(string title, List<string> options)
		{
			this.isOpen = true;
			this.OptionChosen = false;
			this.ChosenOptionIndex = -1;
			this.ClearOptions();
			this.TitleText.text = title;
			for (int i = 0; i < options.Count; i++)
			{
				RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.ListOptionPrefab, this.OptionContainer).GetComponent<RectTransform>();
				component.Find("Label").GetComponent<TextMeshProUGUI>().text = options[i];
				component.anchoredPosition = new Vector2(0f, -((float)i + 0.5f) * component.sizeDelta.y);
				int index = i;
				component.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.ListOptionClicked(index);
				});
			}
			this.canvas.enabled = true;
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x0011CA67 File Offset: 0x0011AC67
		public void Close()
		{
			this.isOpen = false;
			this.canvas.enabled = false;
			this.ClearOptions();
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x0011CA82 File Offset: 0x0011AC82
		public void Cancel()
		{
			this.ChosenOptionIndex = -1;
			this.OptionChosen = true;
			this.Close();
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x0011CA98 File Offset: 0x0011AC98
		private void ClearOptions()
		{
			int childCount = this.OptionContainer.childCount;
			for (int i = 0; i < childCount; i++)
			{
				UnityEngine.Object.Destroy(this.OptionContainer.GetChild(0).gameObject);
			}
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x0011CAD3 File Offset: 0x0011ACD3
		private void ListOptionClicked(int index)
		{
			this.ChosenOptionIndex = index;
			this.OptionChosen = true;
			this.Close();
		}

		// Token: 0x040031AC RID: 12716
		[Header("References")]
		public Canvas canvas;

		// Token: 0x040031AD RID: 12717
		public TextMeshProUGUI TitleText;

		// Token: 0x040031AE RID: 12718
		public RectTransform OptionContainer;

		// Token: 0x040031AF RID: 12719
		public Button CloseButton;

		// Token: 0x040031B0 RID: 12720
		[Header("Prefabs")]
		public GameObject ListOptionPrefab;

		// Token: 0x040031B1 RID: 12721
		[HideInInspector]
		public bool OptionChosen;
	}
}
