using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200083D RID: 2109
	[Serializable]
	public class FloatSmoother
	{
		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x060039CC RID: 14796 RVA: 0x000F4157 File Offset: 0x000F2357
		// (set) Token: 0x060039CD RID: 14797 RVA: 0x000F415F File Offset: 0x000F235F
		public float CurrentValue { get; private set; }

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x060039CE RID: 14798 RVA: 0x000F4168 File Offset: 0x000F2368
		// (set) Token: 0x060039CF RID: 14799 RVA: 0x000F4170 File Offset: 0x000F2370
		public float Multiplier { get; private set; } = 1f;

		// Token: 0x060039D0 RID: 14800 RVA: 0x000F4179 File Offset: 0x000F2379
		public void Initialize()
		{
			this.SetDefault(this.DefaultValue);
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Combine(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x000F41B4 File Offset: 0x000F23B4
		public void Destroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Remove(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x000F41E3 File Offset: 0x000F23E3
		public void SetDefault(float value)
		{
			this.AddOverride(value, 0, "Default");
			this.CurrentValue = value;
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x000F41F9 File Offset: 0x000F23F9
		public void SetMultiplier(float value)
		{
			this.Multiplier = value;
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x000F4202 File Offset: 0x000F2402
		public void SetSmoothingSpeed(float value)
		{
			this.SmoothingSpeed = value;
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x000F420C File Offset: 0x000F240C
		public void AddOverride(float value, int priority, string label)
		{
			FloatSmoother.Override @override = this.overrides.Find((FloatSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override == null)
			{
				@override = new FloatSmoother.Override();
				@override.Label = label;
				this.overrides.Add(@override);
			}
			@override.Value = value;
			@override.Priority = priority;
			this.overrides.Sort((FloatSmoother.Override x, FloatSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x000F4298 File Offset: 0x000F2498
		public void RemoveOverride(string label)
		{
			FloatSmoother.Override @override = this.overrides.Find((FloatSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override != null)
			{
				this.overrides.Remove(@override);
			}
			this.overrides.Sort((FloatSmoother.Override x, FloatSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x000F4304 File Offset: 0x000F2504
		public void Update()
		{
			if (this.overrides.Count == 0)
			{
				return;
			}
			FloatSmoother.Override @override = this.overrides[0];
			this.CurrentValue = Mathf.Lerp(this.CurrentValue, @override.Value, this.SmoothingSpeed * Time.fixedDeltaTime) * this.Multiplier;
		}

		// Token: 0x040029C2 RID: 10690
		[SerializeField]
		private float DefaultValue = 1f;

		// Token: 0x040029C3 RID: 10691
		[SerializeField]
		private float SmoothingSpeed = 1f;

		// Token: 0x040029C4 RID: 10692
		private List<FloatSmoother.Override> overrides = new List<FloatSmoother.Override>();

		// Token: 0x0200083E RID: 2110
		public class Override
		{
			// Token: 0x040029C5 RID: 10693
			public float Value;

			// Token: 0x040029C6 RID: 10694
			public int Priority;

			// Token: 0x040029C7 RID: 10695
			public string Label;
		}
	}
}
