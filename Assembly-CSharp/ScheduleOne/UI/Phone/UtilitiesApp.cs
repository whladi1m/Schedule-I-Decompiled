using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Money;
using ScheduleOne.Property;
using ScheduleOne.Property.Utilities.Power;
using ScheduleOne.Property.Utilities.Water;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000A8A RID: 2698
	public class UtilitiesApp : App<UtilitiesApp>
	{
		// Token: 0x0600489D RID: 18589 RVA: 0x00130104 File Offset: 0x0012E304
		protected override void Awake()
		{
			base.Awake();
			this.water_Cost.text = "Cost per litre: $" + WaterManager.pricePerL.ToString();
			this.electricity_Cost.text = "Cost per kWh $" + PowerManager.pricePerkWh.ToString();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.RefreshShownValues));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onDayPass = (Action)Delegate.Combine(instance2.onDayPass, new Action(this.OnDayPass));
			PropertyDropdown propertyDropdown = this.propertySelector;
			propertyDropdown.onSelectionChanged = (Action)Delegate.Combine(propertyDropdown.onSelectionChanged, new Action(this.RefreshShownValues));
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x001301CC File Offset: 0x0012E3CC
		protected override void OnDestroy()
		{
			base.OnDestroy();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.RefreshShownValues));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onDayPass = (Action)Delegate.Remove(instance2.onDayPass, new Action(this.OnDayPass));
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x0013022D File Offset: 0x0012E42D
		protected override void Update()
		{
			base.Update();
			if (base.isOpen)
			{
				this.selectedProperty = this.propertySelector.selectedProperty;
			}
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x00130250 File Offset: 0x0012E450
		protected virtual void RefreshShownValues()
		{
			if (!base.isOpen)
			{
				return;
			}
			this.selectedProperty = this.propertySelector.selectedProperty;
			this.water_Usage.text = "Water usage: " + this.Round(Singleton<WaterManager>.Instance.GetTotalUsage(), 1f).ToString() + " litres";
			this.water_Total.text = "Total cost: " + MoneyManager.FormatAmount(this.Round(Singleton<WaterManager>.Instance.GetTotalUsage() * WaterManager.pricePerL, 2f), true, false);
			this.electricity_Usage.text = "Electricity usage: " + this.Round(Singleton<PowerManager>.Instance.GetTotalUsage(), 2f).ToString() + " kWh";
			this.electricity_Total.text = "Total cost: " + MoneyManager.FormatAmount(this.Round(Singleton<PowerManager>.Instance.GetTotalUsage() * PowerManager.pricePerkWh, 2f), true, false);
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void OnDayPass()
		{
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x00130353 File Offset: 0x0012E553
		private float Round(float n, float decimals)
		{
			return Mathf.Round(n * Mathf.Pow(10f, decimals)) / Mathf.Pow(10f, decimals);
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x00130373 File Offset: 0x0012E573
		public override void SetOpen(bool open)
		{
			base.SetOpen(open);
			if (open)
			{
				this.RefreshShownValues();
			}
		}

		// Token: 0x040035FF RID: 13823
		[Header("References")]
		[SerializeField]
		protected Text water_Usage;

		// Token: 0x04003600 RID: 13824
		[SerializeField]
		protected Text water_Cost;

		// Token: 0x04003601 RID: 13825
		[SerializeField]
		protected Text water_Total;

		// Token: 0x04003602 RID: 13826
		[SerializeField]
		protected Text electricity_Usage;

		// Token: 0x04003603 RID: 13827
		[SerializeField]
		protected Text electricity_Cost;

		// Token: 0x04003604 RID: 13828
		[SerializeField]
		protected Text electricity_Total;

		// Token: 0x04003605 RID: 13829
		[SerializeField]
		protected Text dumpster_Count;

		// Token: 0x04003606 RID: 13830
		[SerializeField]
		protected Text dumpster_EmptyCost;

		// Token: 0x04003607 RID: 13831
		[SerializeField]
		protected Text dumpster_Total;

		// Token: 0x04003608 RID: 13832
		[SerializeField]
		protected Button dumpsterButton;

		// Token: 0x04003609 RID: 13833
		[SerializeField]
		protected PropertyDropdown propertySelector;

		// Token: 0x0400360A RID: 13834
		private Property selectedProperty;
	}
}
