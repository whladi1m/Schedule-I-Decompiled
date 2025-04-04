using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x02000989 RID: 2441
	public abstract class ACSelection<T> : MonoBehaviour where T : UnityEngine.Object
	{
		// Token: 0x06004226 RID: 16934 RVA: 0x001157D0 File Offset: 0x001139D0
		protected virtual void Awake()
		{
			for (int i = 0; i < this.Options.Count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ButtonPrefab, base.transform);
				gameObject.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = this.GetOptionLabel(i);
				this.buttons.Add(gameObject);
				int index = i;
				gameObject.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.SelectOption(index, true);
				});
			}
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x00115868 File Offset: 0x00113A68
		public void SelectOption(int index, bool notify = true)
		{
			int selectedOptionIndex = this.SelectedOptionIndex;
			if (index != this.SelectedOptionIndex)
			{
				if (this.SelectedOptionIndex != -1)
				{
					this.SetButtonHighlighted(this.SelectedOptionIndex, false);
				}
				this.SelectedOptionIndex = index;
				this.SetButtonHighlighted(this.SelectedOptionIndex, true);
			}
			else if (this.Nullable)
			{
				this.SetButtonHighlighted(this.SelectedOptionIndex, false);
				this.SelectedOptionIndex = -1;
			}
			if (selectedOptionIndex != this.SelectedOptionIndex && notify)
			{
				this.CallValueChange();
			}
		}

		// Token: 0x06004228 RID: 16936
		public abstract void CallValueChange();

		// Token: 0x06004229 RID: 16937
		public abstract string GetOptionLabel(int index);

		// Token: 0x0600422A RID: 16938
		public abstract int GetAssetPathIndex(string path);

		// Token: 0x0600422B RID: 16939 RVA: 0x001158E1 File Offset: 0x00113AE1
		private void SetButtonHighlighted(int buttonIndex, bool h)
		{
			if (buttonIndex == -1)
			{
				return;
			}
			this.buttons[buttonIndex].transform.Find("Indicator").gameObject.SetActive(h);
		}

		// Token: 0x0400301A RID: 12314
		[Header("References")]
		public GameObject ButtonPrefab;

		// Token: 0x0400301B RID: 12315
		[Header("Settings")]
		public int PropertyIndex;

		// Token: 0x0400301C RID: 12316
		public List<T> Options = new List<T>();

		// Token: 0x0400301D RID: 12317
		public bool Nullable = true;

		// Token: 0x0400301E RID: 12318
		public int DefaultOptionIndex;

		// Token: 0x0400301F RID: 12319
		protected List<GameObject> buttons = new List<GameObject>();

		// Token: 0x04003020 RID: 12320
		protected int SelectedOptionIndex = -1;

		// Token: 0x04003021 RID: 12321
		public UnityEvent<T> onValueChange;

		// Token: 0x04003022 RID: 12322
		public UnityEvent<T, int> onValueChangeWithIndex;
	}
}
