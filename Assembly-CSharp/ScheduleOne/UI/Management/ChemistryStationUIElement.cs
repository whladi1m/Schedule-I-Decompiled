using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using ScheduleOne.UI.Stations;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AFB RID: 2811
	public class ChemistryStationUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06004B2E RID: 19246 RVA: 0x0013BC9F File Offset: 0x00139E9F
		// (set) Token: 0x06004B2F RID: 19247 RVA: 0x0013BCA7 File Offset: 0x00139EA7
		public ChemistryStation AssignedStation { get; protected set; }

		// Token: 0x06004B30 RID: 19248 RVA: 0x0013BCB0 File Offset: 0x00139EB0
		public void Initialize(ChemistryStation oven)
		{
			this.AssignedStation = oven;
			this.AssignedStation.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x0013BCF0 File Offset: 0x00139EF0
		protected virtual void RefreshUI()
		{
			ChemistryStationConfiguration chemistryStationConfiguration = this.AssignedStation.Configuration as ChemistryStationConfiguration;
			base.SetAssignedNPC(chemistryStationConfiguration.AssignedChemist.SelectedNPC);
			if (chemistryStationConfiguration.Recipe.SelectedRecipe != null)
			{
				this.RecipeEntry.AssignRecipe(chemistryStationConfiguration.Recipe.SelectedRecipe);
				this.RecipeEntry.gameObject.SetActive(true);
				this.NoRecipe.SetActive(false);
				return;
			}
			this.RecipeEntry.gameObject.SetActive(false);
			this.NoRecipe.SetActive(true);
		}

		// Token: 0x0400389A RID: 14490
		[Header("References")]
		public StationRecipeEntry RecipeEntry;

		// Token: 0x0400389B RID: 14491
		public GameObject NoRecipe;
	}
}
