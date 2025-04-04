using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000A59 RID: 2649
	public class ShopAmountSelector : MonoBehaviour
	{
		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x0600478E RID: 18318 RVA: 0x0012C6B5 File Offset: 0x0012A8B5
		// (set) Token: 0x0600478F RID: 18319 RVA: 0x0012C6BD File Offset: 0x0012A8BD
		public bool IsOpen { get; private set; }

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06004790 RID: 18320 RVA: 0x0012C6C6 File Offset: 0x0012A8C6
		// (set) Token: 0x06004791 RID: 18321 RVA: 0x0012C6CE File Offset: 0x0012A8CE
		public int SelectedAmount { get; private set; } = 1;

		// Token: 0x06004792 RID: 18322 RVA: 0x0012C6D8 File Offset: 0x0012A8D8
		private void Awake()
		{
			this.Container.gameObject.SetActive(false);
			this.InputField.onSubmit.AddListener(new UnityAction<string>(this.OnSubmitted));
			this.InputField.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x0012C72E File Offset: 0x0012A92E
		public void Open()
		{
			this.Container.gameObject.SetActive(true);
			this.Container.SetAsLastSibling();
			this.InputField.text = string.Empty;
			this.InputField.Select();
			this.IsOpen = true;
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x0012C76E File Offset: 0x0012A96E
		public void Close()
		{
			this.Container.gameObject.SetActive(false);
			this.IsOpen = false;
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x0012C788 File Offset: 0x0012A988
		private void OnSubmitted(string value)
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.OnValueChanged(value);
			if (this.onSubmitted != null)
			{
				this.onSubmitted.Invoke(this.SelectedAmount);
			}
			this.Close();
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x0012C7BC File Offset: 0x0012A9BC
		private void OnValueChanged(string value)
		{
			int value2;
			if (int.TryParse(value, out value2))
			{
				this.SelectedAmount = Mathf.Clamp(value2, 1, 999);
				this.InputField.SetTextWithoutNotify(this.SelectedAmount.ToString());
				return;
			}
			this.SelectedAmount = 1;
			this.InputField.SetTextWithoutNotify(string.Empty);
		}

		// Token: 0x0400353C RID: 13628
		[Header("References")]
		public RectTransform Container;

		// Token: 0x0400353D RID: 13629
		public TMP_InputField InputField;

		// Token: 0x0400353E RID: 13630
		public UnityEvent<int> onSubmitted;
	}
}
