using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Clothing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B38 RID: 2872
	public class CharacterCreatorOptionList : CharacterCreatorField<string>
	{
		// Token: 0x06004C6F RID: 19567 RVA: 0x00141CDC File Offset: 0x0013FEDC
		protected override void Awake()
		{
			base.Awake();
			if (this.CanSelectNone)
			{
				this.Options.Insert(0, new CharacterCreatorOptionList.Option
				{
					AssetPath = "",
					Label = "None"
				});
			}
			for (int i = 0; i < this.Options.Count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab, this.OptionContainer);
				gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = this.Options[i].Label;
				string option = this.Options[i].AssetPath;
				gameObject.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.OptionClicked(option);
				});
				this.optionButtons.Add(gameObject.GetComponent<Button>());
			}
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x00141DC8 File Offset: 0x0013FFC8
		public override void ApplyValue()
		{
			base.ApplyValue();
			Button button = null;
			int i = 0;
			while (i < this.Options.Count)
			{
				if (base.value == this.Options[i].AssetPath)
				{
					this.selectedClothingDefinition = this.Options[i].ClothingItemEquivalent;
					if (this.optionButtons.Count > i)
					{
						button = this.optionButtons[i];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (this.selectedButton != null)
			{
				this.selectedButton.interactable = true;
			}
			this.selectedButton = button;
			if (this.selectedButton != null)
			{
				this.selectedButton.interactable = false;
			}
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x00141E80 File Offset: 0x00140080
		public void OptionClicked(string option)
		{
			base.value = option;
			CharacterCreatorOptionList.Option option2 = this.Options.FirstOrDefault((CharacterCreatorOptionList.Option o) => o.AssetPath == option);
			if (option2 != null)
			{
				this.selectedClothingDefinition = option2.ClothingItemEquivalent;
			}
			else
			{
				this.selectedClothingDefinition = null;
			}
			this.WriteValue(true);
		}

		// Token: 0x040039C5 RID: 14789
		[Header("References")]
		public RectTransform OptionContainer;

		// Token: 0x040039C6 RID: 14790
		[Header("Settings")]
		public bool CanSelectNone = true;

		// Token: 0x040039C7 RID: 14791
		public List<CharacterCreatorOptionList.Option> Options;

		// Token: 0x040039C8 RID: 14792
		public GameObject OptionPrefab;

		// Token: 0x040039C9 RID: 14793
		private List<Button> optionButtons = new List<Button>();

		// Token: 0x040039CA RID: 14794
		private Button selectedButton;

		// Token: 0x02000B39 RID: 2873
		[Serializable]
		public class Option
		{
			// Token: 0x040039CB RID: 14795
			public string Label;

			// Token: 0x040039CC RID: 14796
			public string AssetPath;

			// Token: 0x040039CD RID: 14797
			public ClothingDefinition ClothingItemEquivalent;
		}
	}
}
