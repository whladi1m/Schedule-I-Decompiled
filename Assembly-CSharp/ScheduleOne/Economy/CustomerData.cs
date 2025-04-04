using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.Product;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x02000656 RID: 1622
	[CreateAssetMenu(fileName = "CustomerData", menuName = "ScriptableObjects/CustomerData", order = 1)]
	[Serializable]
	public class CustomerData : ScriptableObject
	{
		// Token: 0x06002C63 RID: 11363 RVA: 0x000B8BC0 File Offset: 0x000B6DC0
		public static float GetQualityScalar(EQuality quality)
		{
			switch (quality)
			{
			case EQuality.Trash:
				return 0f;
			case EQuality.Poor:
				return 0.25f;
			case EQuality.Standard:
				return 0.5f;
			case EQuality.Premium:
				return 0.75f;
			case EQuality.Heavenly:
				return 1f;
			default:
				return 0f;
			}
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x000B8C0C File Offset: 0x000B6E0C
		public List<EDay> GetOrderDays(float dependence, float normalizedRelationship)
		{
			float t = Mathf.Max(dependence, normalizedRelationship);
			int num = Mathf.RoundToInt(Mathf.Lerp((float)this.MinOrdersPerWeek, (float)this.MaxOrdersPerWeek, t));
			int preferredOrderDay = (int)this.PreferredOrderDay;
			int num2 = Mathf.RoundToInt(7f / (float)num);
			num2 = Mathf.Max(num2, 1);
			List<EDay> list = new List<EDay>();
			for (int i = 0; i < 7; i += num2)
			{
				list.Add((EDay)((i + preferredOrderDay) % 7));
			}
			return list;
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x000B8C7E File Offset: 0x000B6E7E
		public float GetAdjustedWeeklySpend(float normalizedRelationship)
		{
			return Mathf.Lerp(this.MinWeeklySpend, this.MaxWeeklySpend, normalizedRelationship) * LevelManager.GetOrderLimitMultiplier(NetworkSingleton<LevelManager>.Instance.GetFullRank());
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x000B8CA4 File Offset: 0x000B6EA4
		[Button]
		public void RandomizeAffinities()
		{
			this.DefaultAffinityData = new CustomerAffinityData();
			List<EDrugType> list = Enum.GetValues(typeof(EDrugType)).Cast<EDrugType>().ToList<EDrugType>();
			for (int i = 0; i < list.Count; i++)
			{
				this.DefaultAffinityData.ProductAffinities.Add(new ProductTypeAffinity
				{
					DrugType = list[i],
					Affinity = 0f
				});
			}
			for (int j = 0; j < this.DefaultAffinityData.ProductAffinities.Count; j++)
			{
				this.DefaultAffinityData.ProductAffinities[j].Affinity = UnityEngine.Random.Range(-1f, 1f);
			}
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x000B8D54 File Offset: 0x000B6F54
		[Button]
		public void RandomizeProperties()
		{
			string[] array = new string[]
			{
				"Properties/Tier1",
				"Properties/Tier2",
				"Properties/Tier3",
				"Properties/Tier4",
				"Properties/Tier5"
			};
			List<Property> list = new List<Property>();
			foreach (string path in array)
			{
				list.AddRange(Resources.LoadAll<Property>(path));
			}
			this.PreferredProperties.Clear();
			for (int j = 0; j < 3; j++)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				this.PreferredProperties.Add(list[index]);
				list.RemoveAt(index);
			}
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x000B8DF8 File Offset: 0x000B6FF8
		[Button]
		public void RandomizeTiming()
		{
			this.PreferredOrderDay = (EDay)UnityEngine.Random.Range(0, 7);
			int num = UnityEngine.Random.Range(420, 1440);
			num = Mathf.RoundToInt((float)num / 15f) * 15;
			this.OrderTime = TimeManager.Get24HourTimeFromMinSum(num);
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x000B8E3F File Offset: 0x000B703F
		[Button]
		public void ClearInvalid()
		{
			while (this.DefaultAffinityData.ProductAffinities.Count > 3)
			{
				this.DefaultAffinityData.ProductAffinities.RemoveAt(this.DefaultAffinityData.ProductAffinities.Count - 1);
			}
		}

		// Token: 0x04001FB7 RID: 8119
		public CustomerAffinityData DefaultAffinityData;

		// Token: 0x04001FB8 RID: 8120
		[Header("Preferred Properties - Properties the customer prefers in a product.")]
		public List<Property> PreferredProperties = new List<Property>();

		// Token: 0x04001FB9 RID: 8121
		[Header("Spending Behaviour")]
		public float MinWeeklySpend = 200f;

		// Token: 0x04001FBA RID: 8122
		public float MaxWeeklySpend = 500f;

		// Token: 0x04001FBB RID: 8123
		[Range(0f, 7f)]
		public int MinOrdersPerWeek = 1;

		// Token: 0x04001FBC RID: 8124
		[Range(0f, 7f)]
		public int MaxOrdersPerWeek = 5;

		// Token: 0x04001FBD RID: 8125
		[Header("Timing Settings")]
		public int OrderTime = 1200;

		// Token: 0x04001FBE RID: 8126
		public EDay PreferredOrderDay;

		// Token: 0x04001FBF RID: 8127
		[Header("Standards")]
		public ECustomerStandard Standards = ECustomerStandard.Moderate;

		// Token: 0x04001FC0 RID: 8128
		[Header("Direct approaching")]
		public bool CanBeDirectlyApproached = true;

		// Token: 0x04001FC1 RID: 8129
		public bool GuaranteeFirstSampleSuccess;

		// Token: 0x04001FC2 RID: 8130
		[Tooltip("The average relationship of mutual customers to provide a 50% chance of success")]
		[Range(0f, 5f)]
		public float MinMutualRelationRequirement = 3f;

		// Token: 0x04001FC3 RID: 8131
		[Tooltip("The average relationship of mutual customers to provide a 100% chance of success")]
		[Range(0f, 5f)]
		public float MaxMutualRelationRequirement = 5f;

		// Token: 0x04001FC4 RID: 8132
		[Tooltip("If direct approach fails, whats the chance the police will be called?")]
		[Range(0f, 1f)]
		public float CallPoliceChance = 0.5f;

		// Token: 0x04001FC5 RID: 8133
		[Header("Dependence")]
		[Tooltip("How quickly the customer builds dependence")]
		[Range(0f, 2f)]
		public float DependenceMultiplier = 1f;

		// Token: 0x04001FC6 RID: 8134
		[Tooltip("The customer's starting (and lowest possible) dependence level")]
		[Range(0f, 1f)]
		public float BaseAddiction;

		// Token: 0x04001FC7 RID: 8135
		public Action onChanged;
	}
}
