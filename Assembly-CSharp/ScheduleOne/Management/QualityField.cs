using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200056C RID: 1388
	public class QualityField : ConfigField
	{
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x060022B0 RID: 8880 RVA: 0x0008EB0E File Offset: 0x0008CD0E
		// (set) Token: 0x060022B1 RID: 8881 RVA: 0x0008EB16 File Offset: 0x0008CD16
		public EQuality Value { get; protected set; } = EQuality.Standard;

		// Token: 0x060022B2 RID: 8882 RVA: 0x0008EB1F File Offset: 0x0008CD1F
		public QualityField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x0008EB3A File Offset: 0x0008CD3A
		public void SetValue(EQuality value, bool network)
		{
			this.Value = value;
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onValueChanged != null)
			{
				this.onValueChanged.Invoke(this.Value);
			}
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x0008EB6C File Offset: 0x0008CD6C
		public override bool IsValueDefault()
		{
			return this.Value == EQuality.Standard;
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x0008EB77 File Offset: 0x0008CD77
		public QualityFieldData GetData()
		{
			return new QualityFieldData(this.Value);
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x0008EB84 File Offset: 0x0008CD84
		public void Load(QualityFieldData data)
		{
			if (data != null)
			{
				this.SetValue(data.Value, true);
			}
		}

		// Token: 0x04001A29 RID: 6697
		public UnityEvent<EQuality> onValueChanged = new UnityEvent<EQuality>();
	}
}
