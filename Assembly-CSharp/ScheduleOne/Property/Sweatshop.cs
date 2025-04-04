using System;
using System.Linq;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Growing;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Variables;

namespace ScheduleOne.Property
{
	// Token: 0x02000807 RID: 2055
	public class Sweatshop : Property
	{
		// Token: 0x06003813 RID: 14355 RVA: 0x000EA6F1 File Offset: 0x000E88F1
		protected override void Start()
		{
			base.Start();
			base.InvokeRepeating("UpdateVariables", 0f, 0.5f);
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x000ED218 File Offset: 0x000EB418
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
			int num5 = this.Container.GetComponentsInChildren<PackagingStation>().Length;
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
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Sweatshop_Pots", array.Length.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Sweatshop_PackagingStations", num5.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Sweatshop_MixingStations", this.Container.GetComponentsInChildren<MixingStation>().Length.ToString(), true);
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x000ED37E File Offset: 0x000EB57E
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.SweatshopAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.SweatshopAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x000ED397 File Offset: 0x000EB597
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.SweatshopAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.SweatshopAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x000ED3B0 File Offset: 0x000EB5B0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x000ED3BE File Offset: 0x000EB5BE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040028D6 RID: 10454
		private bool dll_Excuted;

		// Token: 0x040028D7 RID: 10455
		private bool dll_Excuted;
	}
}
