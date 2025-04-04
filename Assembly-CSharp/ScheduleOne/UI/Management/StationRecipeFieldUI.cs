using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AE7 RID: 2791
	public class StationRecipeFieldUI : MonoBehaviour
	{
		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06004AAA RID: 19114 RVA: 0x00139314 File Offset: 0x00137514
		// (set) Token: 0x06004AAB RID: 19115 RVA: 0x0013931C File Offset: 0x0013751C
		public List<StationRecipeField> Fields { get; protected set; } = new List<StationRecipeField>();

		// Token: 0x06004AAC RID: 19116 RVA: 0x00139328 File Offset: 0x00137528
		public void Bind(List<StationRecipeField> field)
		{
			this.Fields = new List<StationRecipeField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onRecipeChanged.AddListener(new UnityAction<StationRecipe>(this.Refresh));
			this.Refresh(this.Fields[0].SelectedRecipe);
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x00139394 File Offset: 0x00137594
		private void Refresh(StationRecipe newVal)
		{
			this.None.gameObject.SetActive(false);
			this.Mixed.gameObject.SetActive(false);
			this.ClearButton.gameObject.SetActive(false);
			this.RecipeEntry.gameObject.SetActive(false);
			if (this.AreFieldsUniform())
			{
				if (newVal != null)
				{
					this.ClearButton.gameObject.SetActive(true);
					this.RecipeEntry.AssignRecipe(newVal);
					this.RecipeEntry.gameObject.SetActive(true);
				}
				else
				{
					this.None.SetActive(true);
				}
			}
			else
			{
				this.Mixed.gameObject.SetActive(true);
				this.ClearButton.gameObject.SetActive(true);
			}
			this.ClearButton.gameObject.SetActive(false);
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x00139468 File Offset: 0x00137668
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (this.Fields[i].SelectedRecipe != this.Fields[i + 1].SelectedRecipe)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x001394BC File Offset: 0x001376BC
		public void Clicked()
		{
			bool flag = this.AreFieldsUniform();
			StationRecipe selectedOption = null;
			if (flag)
			{
				selectedOption = this.Fields[0].SelectedRecipe;
			}
			List<StationRecipe> options = (from x in this.Fields[0].Options
			where x.Unlocked
			select x).ToList<StationRecipe>();
			Singleton<ManagementInterface>.Instance.RecipeSelectorScreen.Initialize("Select Recipe", options, selectedOption, new Action<StationRecipe>(this.OptionSelected));
			Singleton<ManagementInterface>.Instance.RecipeSelectorScreen.Open();
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x00139554 File Offset: 0x00137754
		private void OptionSelected(StationRecipe option)
		{
			foreach (StationRecipeField stationRecipeField in this.Fields)
			{
				stationRecipeField.SetRecipe(option, true);
			}
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x001395A8 File Offset: 0x001377A8
		public void ClearClicked()
		{
			foreach (StationRecipeField stationRecipeField in this.Fields)
			{
				stationRecipeField.SetRecipe(null, true);
			}
		}

		// Token: 0x04003830 RID: 14384
		[Header("References")]
		public StationRecipeEntry RecipeEntry;

		// Token: 0x04003831 RID: 14385
		public GameObject None;

		// Token: 0x04003832 RID: 14386
		public GameObject Mixed;

		// Token: 0x04003833 RID: 14387
		public GameObject ClearButton;
	}
}
