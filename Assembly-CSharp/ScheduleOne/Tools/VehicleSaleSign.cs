using System;
using ScheduleOne.Money;
using ScheduleOne.Vehicles;
using TMPro;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000860 RID: 2144
	public class VehicleSaleSign : MonoBehaviour
	{
		// Token: 0x06003A52 RID: 14930 RVA: 0x000F59A4 File Offset: 0x000F3BA4
		private void Awake()
		{
			LandVehicle componentInParent = base.GetComponentInParent<LandVehicle>();
			if (componentInParent != null)
			{
				this.NameLabel.text = componentInParent.VehicleName;
				this.PriceLabel.text = MoneyManager.FormatAmount(componentInParent.VehiclePrice, false, false);
			}
		}

		// Token: 0x04002A24 RID: 10788
		public TextMeshPro NameLabel;

		// Token: 0x04002A25 RID: 10789
		public TextMeshPro PriceLabel;
	}
}
