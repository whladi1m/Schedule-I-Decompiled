using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C04 RID: 3076
	public class ScheduledMaterialChange : MonoBehaviour
	{
		// Token: 0x06005605 RID: 22021 RVA: 0x0016927C File Offset: 0x0016747C
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.Tick));
			this.SetMaterial(false);
			this.appliedInsideTimeRange = false;
			this.randomShift = UnityEngine.Random.Range(-this.TimeRangeRandomization, this.TimeRangeRandomization);
			this.Tick();
		}

		// Token: 0x06005606 RID: 22022 RVA: 0x001692DC File Offset: 0x001674DC
		protected virtual void Tick()
		{
			if (!this.Enabled && this.appliedInsideTimeRange)
			{
				this.SetMaterial(false);
			}
			int min = TimeManager.AddMinutesTo24HourTime(this.TimeRangeMin, this.TimeRangeShift + this.randomShift);
			int max = TimeManager.AddMinutesTo24HourTime(this.TimeRangeMax, this.TimeRangeShift + this.randomShift);
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(min, max))
			{
				if (this.onState == ScheduledMaterialChange.EOnState.Undecided)
				{
					this.onState = ((UnityEngine.Random.Range(0f, 1f) > this.TurnOnChance) ? ScheduledMaterialChange.EOnState.Off : ScheduledMaterialChange.EOnState.On);
				}
			}
			else
			{
				this.onState = ScheduledMaterialChange.EOnState.Undecided;
			}
			if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(min, max) && this.onState == ScheduledMaterialChange.EOnState.On)
			{
				if (!this.appliedInsideTimeRange)
				{
					this.SetMaterial(true);
					return;
				}
			}
			else if (this.appliedInsideTimeRange)
			{
				this.SetMaterial(false);
			}
		}

		// Token: 0x06005607 RID: 22023 RVA: 0x001693A8 File Offset: 0x001675A8
		private void SetMaterial(bool insideTimeRange)
		{
			if (this.Renderers == null || this.Renderers.Length == 0)
			{
				return;
			}
			this.appliedInsideTimeRange = insideTimeRange;
			Material material = this.Renderers[0].materials[this.MaterialIndex];
			material = (insideTimeRange ? this.InsideTimeRangeMaterial : this.OutsideTimeRangeMaterial);
			foreach (MeshRenderer meshRenderer in this.Renderers)
			{
				Material[] materials = meshRenderer.materials;
				materials[this.MaterialIndex] = material;
				meshRenderer.materials = materials;
			}
		}

		// Token: 0x04003FE5 RID: 16357
		public MeshRenderer[] Renderers;

		// Token: 0x04003FE6 RID: 16358
		public int MaterialIndex;

		// Token: 0x04003FE7 RID: 16359
		[Header("Settings")]
		public bool Enabled = true;

		// Token: 0x04003FE8 RID: 16360
		public Material OutsideTimeRangeMaterial;

		// Token: 0x04003FE9 RID: 16361
		public Material InsideTimeRangeMaterial;

		// Token: 0x04003FEA RID: 16362
		public int TimeRangeMin;

		// Token: 0x04003FEB RID: 16363
		public int TimeRangeMax;

		// Token: 0x04003FEC RID: 16364
		public int TimeRangeShift;

		// Token: 0x04003FED RID: 16365
		public int TimeRangeRandomization;

		// Token: 0x04003FEE RID: 16366
		[Range(0f, 1f)]
		public float TurnOnChance = 1f;

		// Token: 0x04003FEF RID: 16367
		private bool appliedInsideTimeRange;

		// Token: 0x04003FF0 RID: 16368
		private ScheduledMaterialChange.EOnState onState;

		// Token: 0x04003FF1 RID: 16369
		private int randomShift;

		// Token: 0x02000C05 RID: 3077
		private enum EOnState
		{
			// Token: 0x04003FF3 RID: 16371
			Undecided,
			// Token: 0x04003FF4 RID: 16372
			On,
			// Token: 0x04003FF5 RID: 16373
			Off
		}
	}
}
