using System;
using ScheduleOne.GameTime;
using ScheduleOne.Quests;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003CB RID: 971
	public class CustomerData : SaveData
	{
		// Token: 0x06001519 RID: 5401 RVA: 0x0005EC5C File Offset: 0x0005CE5C
		public CustomerData(float dependence, string[] purchaseableProducts, float[] productAffinities, int timeSinceLastDealCompleted, int timeSinceLastDealOffered, int offeredDeals, int completedDeals, bool isContractOffered, ContractInfo offeredContract, GameDateTime offeredTime, int timeSincePlayerApproached, int timeSinceInstantDealOffered, bool hasBeenRecommended)
		{
			this.Dependence = dependence;
			this.PurchaseableProducts = purchaseableProducts;
			this.ProductAffinities = productAffinities;
			this.TimeSinceLastDealCompleted = timeSinceLastDealCompleted;
			this.TimeSinceLastDealOffered = timeSinceLastDealOffered;
			this.OfferedDeals = offeredDeals;
			this.CompletedDeals = completedDeals;
			this.IsContractOffered = isContractOffered;
			this.OfferedContract = offeredContract;
			this.OfferedContractTime = offeredTime;
			this.TimeSincePlayerApproached = timeSincePlayerApproached;
			this.TimeSinceInstantDealOffered = timeSinceInstantDealOffered;
			this.HasBeenRecommended = hasBeenRecommended;
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0005ECD4 File Offset: 0x0005CED4
		public CustomerData()
		{
		}

		// Token: 0x04001361 RID: 4961
		public float Dependence;

		// Token: 0x04001362 RID: 4962
		public string[] PurchaseableProducts;

		// Token: 0x04001363 RID: 4963
		public float[] ProductAffinities;

		// Token: 0x04001364 RID: 4964
		public int TimeSinceLastDealCompleted;

		// Token: 0x04001365 RID: 4965
		public int TimeSinceLastDealOffered;

		// Token: 0x04001366 RID: 4966
		public int OfferedDeals;

		// Token: 0x04001367 RID: 4967
		public int CompletedDeals;

		// Token: 0x04001368 RID: 4968
		public bool IsContractOffered;

		// Token: 0x04001369 RID: 4969
		public ContractInfo OfferedContract;

		// Token: 0x0400136A RID: 4970
		public GameDateTime OfferedContractTime;

		// Token: 0x0400136B RID: 4971
		public int TimeSincePlayerApproached;

		// Token: 0x0400136C RID: 4972
		public int TimeSinceInstantDealOffered;

		// Token: 0x0400136D RID: 4973
		public bool HasBeenRecommended;
	}
}
