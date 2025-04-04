using System;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200093B RID: 2363
	[Serializable]
	public class QualityItemInstance : StorableItemInstance
	{
		// Token: 0x06004023 RID: 16419 RVA: 0x0010D4EE File Offset: 0x0010B6EE
		public QualityItemInstance()
		{
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x0010D4FD File Offset: 0x0010B6FD
		public QualityItemInstance(ItemDefinition definition, int quantity, EQuality quality) : base(definition, quantity)
		{
			this.definition = definition;
			this.Quantity = quantity;
			this.ID = definition.ID;
			this.Quality = quality;
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x0010D530 File Offset: 0x0010B730
		public override bool CanStackWith(ItemInstance other, bool checkQuantities = true)
		{
			QualityItemInstance qualityItemInstance = other as QualityItemInstance;
			return qualityItemInstance != null && qualityItemInstance.Quality == this.Quality && base.CanStackWith(other, checkQuantities);
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x0010D560 File Offset: 0x0010B760
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new QualityItemInstance(base.Definition, quantity, this.Quality);
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x0010D58C File Offset: 0x0010B78C
		public override ItemData GetItemData()
		{
			return new QualityItemData(this.ID, this.Quantity, this.Quality.ToString());
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x0010D5B0 File Offset: 0x0010B7B0
		public void SetQuality(EQuality quality)
		{
			this.Quality = quality;
			if (this.onDataChanged != null)
			{
				this.onDataChanged();
			}
		}

		// Token: 0x04002E12 RID: 11794
		public EQuality Quality = EQuality.Standard;
	}
}
