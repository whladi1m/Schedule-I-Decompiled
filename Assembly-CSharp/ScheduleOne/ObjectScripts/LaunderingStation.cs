using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Property;
using ScheduleOne.Tiles;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BB1 RID: 2993
	public class LaunderingStation : GridItem
	{
		// Token: 0x060052BA RID: 21178 RVA: 0x0015CA73 File Offset: 0x0015AC73
		public override void InitializeGridItem(ItemInstance instance, Grid grid, Vector2 originCoordinate, int rotation, string GUID)
		{
			bool initialized = base.Initialized;
			base.InitializeGridItem(instance, grid, originCoordinate, rotation, GUID);
			if (!initialized)
			{
				this.Interface.Initialize(base.ParentProperty as Business);
			}
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x0015CAA0 File Offset: 0x0015ACA0
		private void Update()
		{
			if (this.Interface != null && this.Interface.business != null)
			{
				this.CashCounter.IsOn = (this.Interface.business.currentLaunderTotal > 0f);
			}
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x0015CAF0 File Offset: 0x0015ACF0
		public override bool CanBeDestroyed(out string reason)
		{
			reason = string.Empty;
			return false;
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x0015CAFA File Offset: 0x0015ACFA
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.LaunderingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.LaunderingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x0015CB13 File Offset: 0x0015AD13
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.LaunderingStationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.LaunderingStationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x0015CB2C File Offset: 0x0015AD2C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x0015CB3A File Offset: 0x0015AD3A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003DDA RID: 15834
		[Header("References")]
		public LaunderingInterface Interface;

		// Token: 0x04003DDB RID: 15835
		[SerializeField]
		protected CashCounter CashCounter;

		// Token: 0x04003DDC RID: 15836
		private bool dll_Excuted;

		// Token: 0x04003DDD RID: 15837
		private bool dll_Excuted;
	}
}
