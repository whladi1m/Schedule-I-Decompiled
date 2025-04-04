using System;
using System.Collections.Generic;
using System.IO;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007B6 RID: 1974
	public class Shitbox : LandVehicle
	{
		// Token: 0x06003614 RID: 13844 RVA: 0x000E3A50 File Offset: 0x000E1C50
		public override List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			if (this.LoanSharkVisuals != null && this.LoanSharkVisuals.BulletHoleDecals.activeSelf)
			{
				Shitbox.LoanSharkVisualsData loanSharkVisualsData = new Shitbox.LoanSharkVisualsData
				{
					Enabled = this.LoanSharkVisuals.BulletHoleDecals.activeSelf,
					NoteVisible = this.LoanSharkVisuals.Note.activeSelf
				};
				((ISaveable)this).WriteSubfile(parentFolderPath, "LoanSharkCarData", loanSharkVisualsData.GetJson(true));
				list.Add("LoanSharkCarData.json");
			}
			list.AddRange(base.WriteData(parentFolderPath));
			return list;
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x000E3AE4 File Offset: 0x000E1CE4
		public override void Load(VehicleData data, string containerPath)
		{
			base.Load(data, containerPath);
			string json;
			if (this.LoanSharkVisuals != null && File.Exists(Path.Combine(containerPath, "LoanSharkCarData.json")) && base.Loader.TryLoadFile(containerPath, "LoanSharkCarData", out json))
			{
				Shitbox.LoanSharkVisualsData loanSharkVisualsData = null;
				try
				{
					loanSharkVisualsData = JsonUtility.FromJson<Shitbox.LoanSharkVisualsData>(json);
				}
				catch (Exception ex)
				{
					Console.LogWarning("Failed to deserialize LoanSharkVisualsData: " + ex.Message, null);
					return;
				}
				this.LoanSharkVisuals.Configure(loanSharkVisualsData.Enabled, loanSharkVisualsData.NoteVisible);
			}
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x000E3B84 File Offset: 0x000E1D84
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.ShitboxAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.ShitboxAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x000E3B9D File Offset: 0x000E1D9D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.ShitboxAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.ShitboxAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x000E3BB6 File Offset: 0x000E1DB6
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x000E3BC4 File Offset: 0x000E1DC4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040026E3 RID: 9955
		public LoanSharkCarVisuals LoanSharkVisuals;

		// Token: 0x040026E4 RID: 9956
		private bool dll_Excuted;

		// Token: 0x040026E5 RID: 9957
		private bool dll_Excuted;

		// Token: 0x020007B7 RID: 1975
		[Serializable]
		public class LoanSharkVisualsData : SaveData
		{
			// Token: 0x040026E6 RID: 9958
			public bool Enabled;

			// Token: 0x040026E7 RID: 9959
			public bool NoteVisible;
		}
	}
}
