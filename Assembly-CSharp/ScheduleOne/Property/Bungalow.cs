using System;
using System.Linq;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Growing;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Property
{
	// Token: 0x020007F2 RID: 2034
	public class Bungalow : Property
	{
		// Token: 0x0600373A RID: 14138 RVA: 0x000EA6F1 File Offset: 0x000E88F1
		protected override void Start()
		{
			base.Start();
			base.InvokeRepeating("UpdateVariables", 0f, 0.5f);
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x000EA710 File Offset: 0x000E8910
		private void UpdateVariables()
		{
			if (!NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			Pot[] array = (from x in this.BuildableItems
			where x is Pot
			select x as Pot).ToArray<Pot>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = (from x in this.BuildableItems
			where x is PackagingStation
			select x).Count<BuildableItem>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IsFilledWithSoil)
				{
					num++;
				}
				if (array[i].NormalizedWaterLevel > 0.2f)
				{
					num2++;
				}
				if (array[i].Plant != null)
				{
					num3++;
				}
				if (array[i].AppliedAdditives.Find((Additive x) => x.AdditiveName == "Speed Grow"))
				{
					num4++;
				}
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Bungalow_Pots", array.Length.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Bungalow_Soil_Pots", num.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Bungalow_Watered_Pots", num2.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Bungalow_Seed_Pots", num3.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Bungalow_PackagingStations", num5.ToString(), true);
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x000EA8BF File Offset: 0x000E8ABF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.BungalowAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.BungalowAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x000EA8D8 File Offset: 0x000E8AD8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.BungalowAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.BungalowAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x000EA8F1 File Offset: 0x000E8AF1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x000EA8FF File Offset: 0x000E8AFF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400286D RID: 10349
		public Transform ModelContainer;

		// Token: 0x0400286E RID: 10350
		private bool dll_Excuted;

		// Token: 0x0400286F RID: 10351
		private bool dll_Excuted;
	}
}
