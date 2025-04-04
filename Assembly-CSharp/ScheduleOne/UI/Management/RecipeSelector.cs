using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AF2 RID: 2802
	public class RecipeSelector : ClipboardScreen
	{
		// Token: 0x06004AFC RID: 19196 RVA: 0x0013AE68 File Offset: 0x00139068
		public void Initialize(string selectionTitle, List<StationRecipe> _options, StationRecipe _selectedOption = null, Action<StationRecipe> _optionCallback = null)
		{
			this.TitleLabel.text = selectionTitle;
			this.options = new List<StationRecipe>();
			this.options.AddRange(_options);
			this.selectedOption = _selectedOption;
			this.optionCallback = _optionCallback;
			this.DeleteOptions();
			this.CreateOptions(this.options);
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x0013AEBC File Offset: 0x001390BC
		public override void Open()
		{
			base.Open();
			Debug.Log(this.Container.gameObject.name + " is active: " + this.Container.gameObject.activeSelf.ToString());
			Singleton<ManagementInterface>.Instance.MainScreen.Close();
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x0013AF15 File Offset: 0x00139115
		public override void Close()
		{
			base.Close();
			Debug.Log("Closed");
			Singleton<ManagementInterface>.Instance.MainScreen.Open();
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x0013AF36 File Offset: 0x00139136
		private void ButtonClicked(StationRecipe option)
		{
			if (this.optionCallback != null)
			{
				this.optionCallback(option);
			}
			this.Close();
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x0013AF54 File Offset: 0x00139154
		private void CreateOptions(List<StationRecipe> options)
		{
			options.Sort((StationRecipe a, StationRecipe b) => a.RecipeTitle.CompareTo(b.RecipeTitle));
			for (int i = 0; i < options.Count; i++)
			{
				StationRecipeEntry component = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab, this.OptionContainer).GetComponent<StationRecipeEntry>();
				component.AssignRecipe(options[i]);
				if (options[i] == this.selectedOption)
				{
					component.transform.Find("Selected").gameObject.GetComponent<Image>().color = new Color32(90, 90, 90, byte.MaxValue);
				}
				StationRecipe opt = options[i];
				component.Button.onClick.AddListener(delegate()
				{
					this.ButtonClicked(opt);
				});
				this.optionButtons.Add(component.GetComponent<RectTransform>());
			}
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x0013B050 File Offset: 0x00139250
		private void DeleteOptions()
		{
			for (int i = 0; i < this.optionButtons.Count; i++)
			{
				UnityEngine.Object.Destroy(this.optionButtons[i].gameObject);
			}
			this.optionButtons.Clear();
		}

		// Token: 0x0400386F RID: 14447
		[Header("References")]
		public RectTransform OptionContainer;

		// Token: 0x04003870 RID: 14448
		public TextMeshProUGUI TitleLabel;

		// Token: 0x04003871 RID: 14449
		public GameObject OptionPrefab;

		// Token: 0x04003872 RID: 14450
		[Header("Settings")]
		public Sprite EmptyOptionSprite;

		// Token: 0x04003873 RID: 14451
		private Coroutine lerpRoutine;

		// Token: 0x04003874 RID: 14452
		private List<StationRecipe> options = new List<StationRecipe>();

		// Token: 0x04003875 RID: 14453
		private StationRecipe selectedOption;

		// Token: 0x04003876 RID: 14454
		private List<RectTransform> optionButtons = new List<RectTransform>();

		// Token: 0x04003877 RID: 14455
		private Action<StationRecipe> optionCallback;
	}
}
