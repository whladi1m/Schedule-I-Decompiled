using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000844 RID: 2116
	[Serializable]
	public class HDRColorSmoother
	{
		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x060039E6 RID: 14822 RVA: 0x000F4451 File Offset: 0x000F2651
		// (set) Token: 0x060039E7 RID: 14823 RVA: 0x000F4459 File Offset: 0x000F2659
		public Color CurrentValue { get; private set; } = Color.white;

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x060039E8 RID: 14824 RVA: 0x000F4462 File Offset: 0x000F2662
		// (set) Token: 0x060039E9 RID: 14825 RVA: 0x000F446A File Offset: 0x000F266A
		public float Multiplier { get; private set; } = 1f;

		// Token: 0x060039EA RID: 14826 RVA: 0x000F4473 File Offset: 0x000F2673
		public void Initialize()
		{
			this.SetDefault(this.DefaultValue);
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Combine(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x000F44AE File Offset: 0x000F26AE
		public void Destroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Remove(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x000F44DD File Offset: 0x000F26DD
		public void SetDefault(Color value)
		{
			this.AddOverride(value, 0, "Default");
			this.CurrentValue = value;
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x000F44F3 File Offset: 0x000F26F3
		public void SetMultiplier(float value)
		{
			this.Multiplier = value;
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x000F44FC File Offset: 0x000F26FC
		public void AddOverride(Color value, int priority, string label)
		{
			HDRColorSmoother.Override @override = this.overrides.Find((HDRColorSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override == null)
			{
				@override = new HDRColorSmoother.Override();
				@override.Label = label;
				this.overrides.Add(@override);
			}
			@override.Value = value;
			@override.Priority = priority;
			this.overrides.Sort((HDRColorSmoother.Override x, HDRColorSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x000F4588 File Offset: 0x000F2788
		public void RemoveOverride(string label)
		{
			HDRColorSmoother.Override @override = this.overrides.Find((HDRColorSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override != null)
			{
				this.overrides.Remove(@override);
			}
			this.overrides.Sort((HDRColorSmoother.Override x, HDRColorSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x000F45F4 File Offset: 0x000F27F4
		public void Update()
		{
			if (this.overrides.Count == 0)
			{
				return;
			}
			HDRColorSmoother.Override @override = this.overrides[0];
			this.CurrentValue = Color.Lerp(this.CurrentValue, @override.Value, this.SmoothingSpeed * Time.fixedDeltaTime) * this.Multiplier;
		}

		// Token: 0x040029D1 RID: 10705
		[ColorUsage(true, true)]
		[SerializeField]
		private Color DefaultValue = Color.white;

		// Token: 0x040029D2 RID: 10706
		[SerializeField]
		private float SmoothingSpeed = 1f;

		// Token: 0x040029D3 RID: 10707
		[SerializeField]
		private List<HDRColorSmoother.Override> overrides = new List<HDRColorSmoother.Override>();

		// Token: 0x02000845 RID: 2117
		[Serializable]
		public class Override
		{
			// Token: 0x040029D4 RID: 10708
			public Color Value;

			// Token: 0x040029D5 RID: 10709
			public int Priority;

			// Token: 0x040029D6 RID: 10710
			public string Label;
		}
	}
}
