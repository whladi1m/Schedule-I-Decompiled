using System;
using ScheduleOne.Management.Presets;
using UnityEngine;

namespace ScheduleOne.Management.Objects
{
	// Token: 0x02000594 RID: 1428
	public abstract class ManageableObject : MonoBehaviour
	{
		// Token: 0x06002399 RID: 9113
		public abstract ManageableObjectType GetObjectType();

		// Token: 0x0600239A RID: 9114
		public abstract Preset GetCurrentPreset();

		// Token: 0x0600239B RID: 9115 RVA: 0x00090CF6 File Offset: 0x0008EEF6
		public void SetPreset(Preset newPreset)
		{
			if (this.GetCurrentPreset() != null)
			{
				Preset currentPreset = this.GetCurrentPreset();
				currentPreset.onDeleted = (Preset.PresetDeletion)Delegate.Remove(currentPreset.onDeleted, new Preset.PresetDeletion(this.ExistingPresetDeleted));
			}
			this.SetPreset_Internal(newPreset);
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x00090D2E File Offset: 0x0008EF2E
		protected virtual void SetPreset_Internal(Preset preset)
		{
			preset.onDeleted = (Preset.PresetDeletion)Delegate.Combine(preset.onDeleted, new Preset.PresetDeletion(this.ExistingPresetDeleted));
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x00090D52 File Offset: 0x0008EF52
		public void ExistingPresetDeleted(Preset replacement)
		{
			this.SetPreset(replacement);
		}
	}
}
