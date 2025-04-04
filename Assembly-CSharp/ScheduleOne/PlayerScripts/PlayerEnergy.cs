using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts.Health;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005E7 RID: 1511
	public class PlayerEnergy : MonoBehaviour
	{
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x0600276D RID: 10093 RVA: 0x000A1609 File Offset: 0x0009F809
		// (set) Token: 0x0600276E RID: 10094 RVA: 0x000A1611 File Offset: 0x0009F811
		public float CurrentEnergy { get; protected set; } = 100f;

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x0600276F RID: 10095 RVA: 0x000A161A File Offset: 0x0009F81A
		// (set) Token: 0x06002770 RID: 10096 RVA: 0x000A1622 File Offset: 0x0009F822
		public int EnergyDrinksConsumed { get; protected set; }

		// Token: 0x06002771 RID: 10097 RVA: 0x000A162C File Offset: 0x0009F82C
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			Singleton<SleepCanvas>.Instance.onSleepFullyFaded.AddListener(new UnityAction(this.SleepEnd));
			base.GetComponent<PlayerHealth>().onRevive.AddListener(new UnityAction(this.ResetEnergyDrinks));
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000A1698 File Offset: 0x0009F898
		private void MinPass()
		{
			if (this.DEBUG_DISABLE_ENERGY && (Debug.isDebugBuild || Application.isEditor))
			{
				return;
			}
			if (NetworkSingleton<TimeManager>.Instance.SleepInProgress)
			{
				return;
			}
			float num = -(1f / (this.EnergyDuration_Hours * 60f)) * 100f;
			if (PlayerSingleton<PlayerMovement>.Instance.isSprinting)
			{
				num *= 1.3f;
			}
			this.ChangeEnergy(num);
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000A16FE File Offset: 0x0009F8FE
		private void ChangeEnergy(float change)
		{
			this.SetEnergy(this.CurrentEnergy + change);
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000A1710 File Offset: 0x0009F910
		public void SetEnergy(float newEnergy)
		{
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000A171D File Offset: 0x0009F91D
		public void RestoreEnergy()
		{
			this.SetEnergy(100f);
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000A172A File Offset: 0x0009F92A
		private void SleepEnd()
		{
			this.ResetEnergyDrinks();
			this.RestoreEnergy();
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000A1738 File Offset: 0x0009F938
		public void IncrementEnergyDrinks()
		{
			int energyDrinksConsumed = this.EnergyDrinksConsumed;
			this.EnergyDrinksConsumed = energyDrinksConsumed + 1;
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000A1755 File Offset: 0x0009F955
		private void ResetEnergyDrinks()
		{
			this.EnergyDrinksConsumed = 0;
		}

		// Token: 0x04001CA0 RID: 7328
		public const float CRITICAL_THRESHOLD = 20f;

		// Token: 0x04001CA1 RID: 7329
		public const float MAX_ENERGY = 100f;

		// Token: 0x04001CA2 RID: 7330
		public const float SPRINT_DRAIN_MULTIPLIER = 1.3f;

		// Token: 0x04001CA5 RID: 7333
		public bool DEBUG_DISABLE_ENERGY;

		// Token: 0x04001CA6 RID: 7334
		[Header("Settings")]
		public float EnergyDuration_Hours = 22f;

		// Token: 0x04001CA7 RID: 7335
		public float EnergyRechargeTime_Hours = 6f;

		// Token: 0x04001CA8 RID: 7336
		public UnityEvent onEnergyChanged;

		// Token: 0x04001CA9 RID: 7337
		public UnityEvent onEnergyDepleted;
	}
}
