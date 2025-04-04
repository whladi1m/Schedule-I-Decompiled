using System;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x02000569 RID: 1385
	public class NumberField : ConfigField
	{
		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06002294 RID: 8852 RVA: 0x0008E5EB File Offset: 0x0008C7EB
		// (set) Token: 0x06002295 RID: 8853 RVA: 0x0008E5F3 File Offset: 0x0008C7F3
		public float Value { get; protected set; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06002296 RID: 8854 RVA: 0x0008E5FC File Offset: 0x0008C7FC
		// (set) Token: 0x06002297 RID: 8855 RVA: 0x0008E604 File Offset: 0x0008C804
		public float MinValue { get; protected set; }

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06002298 RID: 8856 RVA: 0x0008E60D File Offset: 0x0008C80D
		// (set) Token: 0x06002299 RID: 8857 RVA: 0x0008E615 File Offset: 0x0008C815
		public float MaxValue { get; protected set; } = 100f;

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x0600229A RID: 8858 RVA: 0x0008E61E File Offset: 0x0008C81E
		// (set) Token: 0x0600229B RID: 8859 RVA: 0x0008E626 File Offset: 0x0008C826
		public bool WholeNumbers { get; protected set; }

		// Token: 0x0600229C RID: 8860 RVA: 0x0008E62F File Offset: 0x0008C82F
		public NumberField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x0008E64E File Offset: 0x0008C84E
		public void SetValue(float value, bool network)
		{
			this.Value = value;
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onItemChanged != null)
			{
				this.onItemChanged.Invoke(this.Value);
			}
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x0008E680 File Offset: 0x0008C880
		public void Configure(float minValue, float maxValue, bool wholeNumbers)
		{
			this.MinValue = minValue;
			this.MaxValue = maxValue;
			this.WholeNumbers = wholeNumbers;
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x0008E697 File Offset: 0x0008C897
		public override bool IsValueDefault()
		{
			return this.Value == 0f;
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x0008E6A6 File Offset: 0x0008C8A6
		public NumberFieldData GetData()
		{
			return new NumberFieldData(this.Value);
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x0008E6B3 File Offset: 0x0008C8B3
		public void Load(NumberFieldData data)
		{
			if (data != null)
			{
				this.SetValue(data.Value, true);
			}
		}

		// Token: 0x04001A1D RID: 6685
		public UnityEvent<float> onItemChanged = new UnityEvent<float>();
	}
}
