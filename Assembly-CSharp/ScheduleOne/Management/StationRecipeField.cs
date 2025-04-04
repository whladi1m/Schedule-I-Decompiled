using System;
using System.Collections.Generic;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.StationFramework;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200056E RID: 1390
	public class StationRecipeField : ConfigField
	{
		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x060022BF RID: 8895 RVA: 0x0008EEEC File Offset: 0x0008D0EC
		// (set) Token: 0x060022C0 RID: 8896 RVA: 0x0008EEF4 File Offset: 0x0008D0F4
		public StationRecipe SelectedRecipe { get; protected set; }

		// Token: 0x060022C1 RID: 8897 RVA: 0x0008EEFD File Offset: 0x0008D0FD
		public StationRecipeField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x0008EF1C File Offset: 0x0008D11C
		public void SetRecipe(StationRecipe recipe, bool network)
		{
			this.SelectedRecipe = recipe;
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onRecipeChanged != null)
			{
				this.onRecipeChanged.Invoke(this.SelectedRecipe);
			}
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x0008EF4E File Offset: 0x0008D14E
		public override bool IsValueDefault()
		{
			return this.SelectedRecipe == null;
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x0008EF5C File Offset: 0x0008D15C
		public StationRecipeFieldData GetData()
		{
			return new StationRecipeFieldData((this.SelectedRecipe != null) ? this.SelectedRecipe.RecipeID.ToString() : "");
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x0008EF88 File Offset: 0x0008D188
		public void Load(StationRecipeFieldData data)
		{
			if (data != null && !string.IsNullOrEmpty(data.RecipeID))
			{
				this.SelectedRecipe = this.Options.Find((StationRecipe x) => x.RecipeID == data.RecipeID);
			}
		}

		// Token: 0x04001A2E RID: 6702
		public List<StationRecipe> Options = new List<StationRecipe>();

		// Token: 0x04001A2F RID: 6703
		public UnityEvent<StationRecipe> onRecipeChanged = new UnityEvent<StationRecipe>();
	}
}
