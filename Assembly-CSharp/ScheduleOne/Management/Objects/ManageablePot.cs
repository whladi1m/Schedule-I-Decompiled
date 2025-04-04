using System;
using ScheduleOne.Management.Presets;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.Management.Objects
{
	// Token: 0x02000595 RID: 1429
	[RequireComponent(typeof(Pot))]
	public class ManageablePot : ManageableObject
	{
		// Token: 0x0600239F RID: 9119 RVA: 0x00090D5B File Offset: 0x0008EF5B
		protected virtual void Awake()
		{
			this.CurrentPreset = PotPreset.GetDefaultPreset();
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x00014002 File Offset: 0x00012202
		public override ManageableObjectType GetObjectType()
		{
			return ManageableObjectType.Pot;
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x00090D68 File Offset: 0x0008EF68
		public override Preset GetCurrentPreset()
		{
			return this.CurrentPreset;
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x00090D70 File Offset: 0x0008EF70
		protected override void SetPreset_Internal(Preset newPreset)
		{
			base.SetPreset_Internal(newPreset);
			PotPreset potPreset = (PotPreset)newPreset;
			if (potPreset == null)
			{
				Console.LogWarning("SetPreset_Internal: preset is not the right type", null);
				return;
			}
			this.CurrentPreset = potPreset;
		}

		// Token: 0x04001A90 RID: 6800
		public PotPreset CurrentPreset;
	}
}
