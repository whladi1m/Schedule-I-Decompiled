using System;
using ScheduleOne.Management.Presets.Options;
using UnityEngine;

namespace ScheduleOne.Management.Presets
{
	// Token: 0x0200058C RID: 1420
	public class PotPreset : Preset
	{
		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x00090964 File Offset: 0x0008EB64
		// (set) Token: 0x06002373 RID: 9075 RVA: 0x0009096B File Offset: 0x0008EB6B
		protected static PotPreset DefaultPreset { get; set; }

		// Token: 0x06002374 RID: 9076 RVA: 0x00090974 File Offset: 0x0008EB74
		public override Preset GetCopy()
		{
			PotPreset potPreset = new PotPreset();
			this.CopyTo(potPreset);
			return potPreset;
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x00090990 File Offset: 0x0008EB90
		public override void CopyTo(Preset other)
		{
			base.CopyTo(other);
			if (other is PotPreset)
			{
				PotPreset potPreset = other as PotPreset;
				this.Seeds.CopyTo(potPreset.Seeds);
				this.Additives.CopyTo(potPreset.Additives);
			}
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x000909D8 File Offset: 0x0008EBD8
		public override void InitializeOptions()
		{
			this.Seeds = new ItemList("Seed Types", ManagementUtilities.WeedSeedAssetPaths, true, true);
			this.Seeds.All = true;
			this.Additives = new ItemList("Additives", ManagementUtilities.AdditiveAssetPaths, true, true);
			this.Seeds.None = true;
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x00090A2C File Offset: 0x0008EC2C
		public static PotPreset GetDefaultPreset()
		{
			if (PotPreset.DefaultPreset == null)
			{
				PotPreset.DefaultPreset = new PotPreset
				{
					PresetName = "Default",
					ObjectType = ManageableObjectType.Pot,
					PresetColor = new Color32(180, 180, 180, byte.MaxValue)
				};
			}
			return PotPreset.DefaultPreset;
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x00090A80 File Offset: 0x0008EC80
		public static PotPreset GetNewBlankPreset()
		{
			PotPreset potPreset = PotPreset.GetDefaultPreset().GetCopy() as PotPreset;
			potPreset.PresetName = "New Preset";
			return potPreset;
		}

		// Token: 0x04001A7F RID: 6783
		public ItemList Seeds;

		// Token: 0x04001A80 RID: 6784
		public ItemList Additives;
	}
}
