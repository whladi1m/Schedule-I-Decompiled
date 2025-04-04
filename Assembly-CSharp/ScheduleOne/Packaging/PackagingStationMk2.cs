using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.UI;
using ScheduleOne.Variables;

namespace ScheduleOne.Packaging
{
	// Token: 0x02000876 RID: 2166
	public class PackagingStationMk2 : PackagingStation
	{
		// Token: 0x06003AAC RID: 15020 RVA: 0x000F6A3C File Offset: 0x000F4C3C
		public override void StartTask()
		{
			new PackageProductTaskMk2(this);
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("PackagingStationMk2TutorialDone"))
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("PackagingStationMk2TutorialDone", true.ToString(), true);
				Singleton<TaskManagerUI>.Instance.PackagingStationMK2TutorialDone.Open();
			}
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x000F6A91 File Offset: 0x000F4C91
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Packaging.PackagingStationMk2Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Packaging.PackagingStationMk2Assembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x000F6AAA File Offset: 0x000F4CAA
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Packaging.PackagingStationMk2Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Packaging.PackagingStationMk2Assembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x000F6AC3 File Offset: 0x000F4CC3
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x000F6AD1 File Offset: 0x000F4CD1
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002A7C RID: 10876
		public PackagingTool PackagingTool;

		// Token: 0x04002A7D RID: 10877
		private bool dll_Excuted;

		// Token: 0x04002A7E RID: 10878
		private bool dll_Excuted;
	}
}
