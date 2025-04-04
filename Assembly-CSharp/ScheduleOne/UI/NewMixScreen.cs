using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.Properties;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009EF RID: 2543
	public class NewMixScreen : Singleton<NewMixScreen>
	{
		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x0600449C RID: 17564 RVA: 0x0011F508 File Offset: 0x0011D708
		public bool IsOpen
		{
			get
			{
				return this.canvas.enabled;
			}
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x0011F518 File Offset: 0x0011D718
		protected override void Awake()
		{
			base.Awake();
			this.nameInputField.onValueChanged.AddListener(new UnityAction<string>(this.OnNameValueChanged));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Exit(ExitAction action)
		{
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x0011F576 File Offset: 0x0011D776
		protected virtual void Update()
		{
			if (this.IsOpen && this.confirmButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
			{
				this.ConfirmButtonClicked();
			}
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x0011F59C File Offset: 0x0011D79C
		public void Open(List<Property> properties, EDrugType drugType, float productMarketValue)
		{
			this.canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.nameInputField.text = this.GenerateUniqueName(properties.ToArray(), drugType);
			Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
			this.PropertiesLabel.text = string.Empty;
			for (int i = 0; i < properties.Count; i++)
			{
				Property property = properties[i];
				if (this.PropertiesLabel.text.Length > 0)
				{
					TextMeshProUGUI propertiesLabel = this.PropertiesLabel;
					propertiesLabel.text += "\n";
				}
				if (i == 4 && properties.Count > 5)
				{
					int num = properties.Count - 5 + 1;
					TextMeshProUGUI propertiesLabel2 = this.PropertiesLabel;
					propertiesLabel2.text = propertiesLabel2.text + "+ " + num.ToString() + " more...";
					break;
				}
				TextMeshProUGUI propertiesLabel3 = this.PropertiesLabel;
				propertiesLabel3.text = string.Concat(new string[]
				{
					propertiesLabel3.text,
					"<color=#",
					ColorUtility.ToHtmlStringRGBA(property.LabelColor),
					">• ",
					property.Name,
					"</color>"
				});
			}
			this.MarketValueLabel.text = "Market Value: <color=#54E717>" + MoneyManager.FormatAmount(productMarketValue, false, false) + "</color>";
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x0011F708 File Offset: 0x0011D908
		public void Close()
		{
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x0011F737 File Offset: 0x0011D937
		public void RandomizeButtonClicked()
		{
			this.nameInputField.text = this.GenerateUniqueName(null, EDrugType.Marijuana);
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x0011F74C File Offset: 0x0011D94C
		public void ConfirmButtonClicked()
		{
			if (this.onMixNamed != null)
			{
				this.onMixNamed(this.nameInputField.text);
			}
			this.Sound.Play();
			this.RandomizeButtonClicked();
			this.Close();
		}

		// Token: 0x060044A4 RID: 17572 RVA: 0x0011F784 File Offset: 0x0011D984
		public string GenerateUniqueName(Property[] properties = null, EDrugType drugType = EDrugType.Marijuana)
		{
			UnityEngine.Random.InitState((int)(Time.timeSinceLevelLoad * 10f));
			string str = this.name1Library[UnityEngine.Random.Range(0, this.name1Library.Count)];
			string str2 = this.name2Library[UnityEngine.Random.Range(0, this.name2Library.Count)];
			if (properties != null)
			{
				int num = 0;
				foreach (Property property in properties)
				{
					num += property.Name.GetHashCode() / 2000;
				}
				num += drugType.GetHashCode() / 1000;
				int value = num % this.name1Library.Count;
				int value2 = num / 2 % this.name2Library.Count;
				str = this.name1Library[Mathf.Clamp(value, 0, this.name1Library.Count)];
				str2 = this.name2Library[Mathf.Clamp(value2, 0, this.name2Library.Count)];
			}
			while (NetworkSingleton<ProductManager>.Instance.ProductNames.Contains(str + " " + str2))
			{
				str = this.name1Library[UnityEngine.Random.Range(0, this.name1Library.Count)];
				str2 = this.name2Library[UnityEngine.Random.Range(0, this.name2Library.Count)];
			}
			return str + " " + str2;
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x0011F8F0 File Offset: 0x0011DAF0
		protected void RefreshNameButtons()
		{
			float num = this.nameInputField.textComponent.preferredWidth / 2f;
			float num2 = 20f;
			this.editIcon.anchoredPosition = new Vector2(num + num2, this.editIcon.anchoredPosition.y);
			this.randomizeNameButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-num - num2, this.randomizeNameButton.GetComponent<RectTransform>().anchoredPosition.y);
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x0011F96C File Offset: 0x0011DB6C
		public void OnNameValueChanged(string newVal)
		{
			if (NetworkSingleton<ProductManager>.Instance.ProductNames.Contains(this.nameInputField.text) || !ProductManager.IsMixNameValid(this.nameInputField.text))
			{
				this.mixAlreadyExistsText.gameObject.SetActive(true);
				this.confirmButton.interactable = false;
			}
			else
			{
				this.mixAlreadyExistsText.gameObject.SetActive(false);
				this.confirmButton.interactable = true;
			}
			this.RefreshNameButtons();
			base.Invoke("RefreshNameButtons", 0.016666668f);
		}

		// Token: 0x04003276 RID: 12918
		public const int MAX_PROPERTIES_DISPLAYED = 5;

		// Token: 0x04003277 RID: 12919
		[Header("References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x04003278 RID: 12920
		public RectTransform Container;

		// Token: 0x04003279 RID: 12921
		[SerializeField]
		protected TMP_InputField nameInputField;

		// Token: 0x0400327A RID: 12922
		[SerializeField]
		protected GameObject mixAlreadyExistsText;

		// Token: 0x0400327B RID: 12923
		[SerializeField]
		protected RectTransform editIcon;

		// Token: 0x0400327C RID: 12924
		[SerializeField]
		protected Button randomizeNameButton;

		// Token: 0x0400327D RID: 12925
		[SerializeField]
		protected Button confirmButton;

		// Token: 0x0400327E RID: 12926
		[SerializeField]
		protected TextMeshProUGUI PropertiesLabel;

		// Token: 0x0400327F RID: 12927
		[SerializeField]
		protected TextMeshProUGUI MarketValueLabel;

		// Token: 0x04003280 RID: 12928
		public AudioSourceController Sound;

		// Token: 0x04003281 RID: 12929
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject attributeEntryPrefab;

		// Token: 0x04003282 RID: 12930
		[Header("Name Library")]
		[SerializeField]
		protected List<string> name1Library = new List<string>();

		// Token: 0x04003283 RID: 12931
		[SerializeField]
		protected List<string> name2Library = new List<string>();

		// Token: 0x04003284 RID: 12932
		public Action<string> onMixNamed;
	}
}
