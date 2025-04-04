using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200082E RID: 2094
	[Serializable]
	public class ColorSmoother
	{
		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06003996 RID: 14742 RVA: 0x000F3A84 File Offset: 0x000F1C84
		// (set) Token: 0x06003997 RID: 14743 RVA: 0x000F3A8C File Offset: 0x000F1C8C
		public Color CurrentValue { get; private set; } = Color.white;

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06003998 RID: 14744 RVA: 0x000F3A95 File Offset: 0x000F1C95
		// (set) Token: 0x06003999 RID: 14745 RVA: 0x000F3A9D File Offset: 0x000F1C9D
		public float Multiplier { get; private set; } = 1f;

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x0600399A RID: 14746 RVA: 0x000F3AA6 File Offset: 0x000F1CA6
		public Color Default
		{
			get
			{
				return this.DefaultValue;
			}
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x000F3AAE File Offset: 0x000F1CAE
		public void Initialize()
		{
			this.SetDefault(this.DefaultValue);
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Combine(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x000F3AE9 File Offset: 0x000F1CE9
		public void Destroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onFixedUpdate = (Action)Delegate.Remove(instance.onFixedUpdate, new Action(this.Update));
			}
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x000F3B18 File Offset: 0x000F1D18
		public void SetDefault(Color value)
		{
			this.AddOverride(value, 0, "Default");
			this.CurrentValue = value;
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x000F3B2E File Offset: 0x000F1D2E
		public void SetMultiplier(float value)
		{
			this.Multiplier = value;
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x000F3B38 File Offset: 0x000F1D38
		public void AddOverride(Color value, int priority, string label)
		{
			ColorSmoother.Override @override = this.overrides.Find((ColorSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override == null)
			{
				@override = new ColorSmoother.Override();
				@override.Label = label;
				this.overrides.Add(@override);
			}
			@override.Value = value;
			@override.Priority = priority;
			this.overrides.Sort((ColorSmoother.Override x, ColorSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x000F3BC4 File Offset: 0x000F1DC4
		public void RemoveOverride(string label)
		{
			ColorSmoother.Override @override = this.overrides.Find((ColorSmoother.Override x) => x.Label.ToLower() == label.ToLower());
			if (@override != null)
			{
				this.overrides.Remove(@override);
			}
			this.overrides.Sort((ColorSmoother.Override x, ColorSmoother.Override y) => y.Priority.CompareTo(x.Priority));
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x000F3C30 File Offset: 0x000F1E30
		public void Update()
		{
			if (this.overrides.Count == 0)
			{
				return;
			}
			ColorSmoother.Override @override = this.overrides[0];
			this.CurrentValue = Color.Lerp(this.CurrentValue, @override.Value, this.SmoothingSpeed * Time.fixedDeltaTime) * this.Multiplier;
		}

		// Token: 0x0400299B RID: 10651
		[SerializeField]
		private Color DefaultValue = Color.white;

		// Token: 0x0400299C RID: 10652
		[SerializeField]
		private float SmoothingSpeed = 1f;

		// Token: 0x0400299D RID: 10653
		[SerializeField]
		private List<ColorSmoother.Override> overrides = new List<ColorSmoother.Override>();

		// Token: 0x0200082F RID: 2095
		[Serializable]
		public class Override
		{
			// Token: 0x0400299E RID: 10654
			public Color Value;

			// Token: 0x0400299F RID: 10655
			public int Priority;

			// Token: 0x040029A0 RID: 10656
			public string Label;
		}
	}
}
