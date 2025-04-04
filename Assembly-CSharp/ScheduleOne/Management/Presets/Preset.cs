using System;
using UnityEngine;

namespace ScheduleOne.Management.Presets
{
	// Token: 0x0200058D RID: 1421
	public abstract class Preset
	{
		// Token: 0x0600237A RID: 9082 RVA: 0x00090AA4 File Offset: 0x0008ECA4
		public Preset()
		{
			this.InitializeOptions();
		}

		// Token: 0x0600237B RID: 9083
		public abstract Preset GetCopy();

		// Token: 0x0600237C RID: 9084 RVA: 0x00090ADC File Offset: 0x0008ECDC
		public virtual void CopyTo(Preset other)
		{
			other.PresetName = this.PresetName;
			other.PresetColor = this.PresetColor;
		}

		// Token: 0x0600237D RID: 9085
		public abstract void InitializeOptions();

		// Token: 0x0600237E RID: 9086 RVA: 0x00090AF6 File Offset: 0x0008ECF6
		public void SetName(string newName)
		{
			if (this.PresetName == newName)
			{
				return;
			}
			this.PresetName = newName;
			if (this.onNameChanged != null)
			{
				this.onNameChanged(newName);
			}
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x00090B22 File Offset: 0x0008ED22
		public void DeletePreset(Preset replacement)
		{
			if (this.onDeleted != null)
			{
				this.onDeleted(replacement);
			}
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x00090B38 File Offset: 0x0008ED38
		public static Preset GetDefault(ManageableObjectType type)
		{
			if (type == ManageableObjectType.Pot)
			{
				return PotPreset.GetDefaultPreset();
			}
			Console.LogWarning("GetDefault: type not accounted for", null);
			return null;
		}

		// Token: 0x04001A81 RID: 6785
		public string PresetName = "Default";

		// Token: 0x04001A82 RID: 6786
		public Color32 PresetColor = new Color32(180, 180, 180, byte.MaxValue);

		// Token: 0x04001A83 RID: 6787
		public ManageableObjectType ObjectType;

		// Token: 0x04001A84 RID: 6788
		public Preset.NameChange onNameChanged;

		// Token: 0x04001A85 RID: 6789
		public Preset.PresetDeletion onDeleted;

		// Token: 0x0200058E RID: 1422
		// (Invoke) Token: 0x06002382 RID: 9090
		public delegate void NameChange(string name);

		// Token: 0x0200058F RID: 1423
		// (Invoke) Token: 0x06002386 RID: 9094
		public delegate void PresetDeletion(Preset replacement);
	}
}
