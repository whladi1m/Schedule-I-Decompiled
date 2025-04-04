using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Law
{
	// Token: 0x020005BF RID: 1471
	public class LawController : Singleton<LawController>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06002487 RID: 9351 RVA: 0x00093684 File Offset: 0x00091884
		// (set) Token: 0x06002488 RID: 9352 RVA: 0x0009368C File Offset: 0x0009188C
		public bool OverrideSettings { get; protected set; }

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06002489 RID: 9353 RVA: 0x00093695 File Offset: 0x00091895
		// (set) Token: 0x0600248A RID: 9354 RVA: 0x0009369D File Offset: 0x0009189D
		public LawActivitySettings OverriddenSettings { get; protected set; }

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x0600248B RID: 9355 RVA: 0x000936A6 File Offset: 0x000918A6
		// (set) Token: 0x0600248C RID: 9356 RVA: 0x000936AE File Offset: 0x000918AE
		public LawActivitySettings CurrentSettings { get; protected set; }

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x0600248D RID: 9357 RVA: 0x000936B7 File Offset: 0x000918B7
		public string SaveFolderName
		{
			get
			{
				return "Law";
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x0600248E RID: 9358 RVA: 0x000936B7 File Offset: 0x000918B7
		public string SaveFileName
		{
			get
			{
				return "Law";
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x0600248F RID: 9359 RVA: 0x000936BE File Offset: 0x000918BE
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06002490 RID: 9360 RVA: 0x00014002 File Offset: 0x00012202
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x000936C6 File Offset: 0x000918C6
		// (set) Token: 0x06002492 RID: 9362 RVA: 0x000936CE File Offset: 0x000918CE
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06002493 RID: 9363 RVA: 0x000936D7 File Offset: 0x000918D7
		// (set) Token: 0x06002494 RID: 9364 RVA: 0x000936DF File Offset: 0x000918DF
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06002495 RID: 9365 RVA: 0x000936E8 File Offset: 0x000918E8
		// (set) Token: 0x06002496 RID: 9366 RVA: 0x000936F0 File Offset: 0x000918F0
		public bool HasChanged { get; set; }

		// Token: 0x06002497 RID: 9367 RVA: 0x000936F9 File Offset: 0x000918F9
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x00093708 File Offset: 0x00091908
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onHourPass = (Action)Delegate.Combine(instance2.onHourPass, new Action(this.HourPass));
			TimeManager instance3 = NetworkSingleton<TimeManager>.Instance;
			instance3.onDayPass = (Action)Delegate.Combine(instance3.onDayPass, new Action(this.DayPass));
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.OnLoadComplete));
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x000937A8 File Offset: 0x000919A8
		protected override void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onHourPass = (Action)Delegate.Remove(instance2.onHourPass, new Action(this.HourPass));
				TimeManager instance3 = NetworkSingleton<TimeManager>.Instance;
				instance3.onDayPass = (Action)Delegate.Remove(instance3.onDayPass, new Action(this.DayPass));
			}
			base.OnDestroy();
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x00093834 File Offset: 0x00091A34
		private void OnLoadComplete()
		{
			this.GetSettings().OnLoaded();
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x00093844 File Offset: 0x00091A44
		private void MinPass()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			LawActivitySettings settings = this.GetSettings();
			if (settings != this.CurrentSettings)
			{
				if (this.CurrentSettings != null)
				{
					this.CurrentSettings.End();
				}
				this.CurrentSettings = settings;
			}
			this.CurrentSettings.Evaluate();
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x0009388E File Offset: 0x00091A8E
		private void HourPass()
		{
			bool isServer = InstanceFinder.IsServer;
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x00093896 File Offset: 0x00091A96
		private void DayPass()
		{
			if (InstanceFinder.IsServer)
			{
				this.ChangeInternalIntensity(this.IntensityIncreasePerDay / 10f);
			}
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000938B1 File Offset: 0x00091AB1
		public LawActivitySettings GetSettings()
		{
			if (this.OverrideSettings && this.OverriddenSettings != null)
			{
				return this.OverriddenSettings;
			}
			return this.GetSettings(NetworkSingleton<TimeManager>.Instance.CurrentDay);
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x000938DC File Offset: 0x00091ADC
		public LawActivitySettings GetSettings(EDay day)
		{
			switch (day)
			{
			case EDay.Monday:
				return this.MondaySettings;
			case EDay.Tuesday:
				return this.TuesdaySettings;
			case EDay.Wednesday:
				return this.WednesdaySettings;
			case EDay.Thursday:
				return this.ThursdaySettings;
			case EDay.Friday:
				return this.FridaySettings;
			case EDay.Saturday:
				return this.SaturdaySettings;
			case EDay.Sunday:
				return this.SundaySettings;
			default:
				return null;
			}
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x0009393F File Offset: 0x00091B3F
		public void OverrideSetings(LawActivitySettings settings)
		{
			this.OverrideSettings = true;
			this.OverriddenSettings = settings;
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x0009394F File Offset: 0x00091B4F
		public void EndOverride()
		{
			this.OverrideSettings = false;
			this.OverriddenSettings = null;
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x0009395F File Offset: 0x00091B5F
		public void ChangeInternalIntensity(float change)
		{
			this.internalLawIntensity = Mathf.Clamp01(this.internalLawIntensity + change);
			this.LE_Intensity = Mathf.RoundToInt(Mathf.Lerp(1f, 10f, this.internalLawIntensity));
			this.HasChanged = true;
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x0009399B File Offset: 0x00091B9B
		public void SetInternalIntensity(float intensity)
		{
			this.internalLawIntensity = Mathf.Clamp01(intensity);
			this.LE_Intensity = Mathf.RoundToInt(Mathf.Lerp(1f, 10f, this.internalLawIntensity));
			this.HasChanged = true;
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000939D0 File Offset: 0x00091BD0
		public virtual string GetSaveString()
		{
			return new LawData(this.internalLawIntensity).GetJson(true);
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000939E3 File Offset: 0x00091BE3
		public void Load(LawData data)
		{
			this.SetInternalIntensity(data.InternalLawIntensity);
		}

		// Token: 0x04001B2E RID: 6958
		public const float DAILY_INTENSITY_DRAIN = 0.05f;

		// Token: 0x04001B2F RID: 6959
		[Range(1f, 10f)]
		public int LE_Intensity = 1;

		// Token: 0x04001B30 RID: 6960
		private float internalLawIntensity;

		// Token: 0x04001B31 RID: 6961
		[Header("Settings")]
		public LawActivitySettings MondaySettings;

		// Token: 0x04001B32 RID: 6962
		public LawActivitySettings TuesdaySettings;

		// Token: 0x04001B33 RID: 6963
		public LawActivitySettings WednesdaySettings;

		// Token: 0x04001B34 RID: 6964
		public LawActivitySettings ThursdaySettings;

		// Token: 0x04001B35 RID: 6965
		public LawActivitySettings FridaySettings;

		// Token: 0x04001B36 RID: 6966
		public LawActivitySettings SaturdaySettings;

		// Token: 0x04001B37 RID: 6967
		public LawActivitySettings SundaySettings;

		// Token: 0x04001B38 RID: 6968
		[Header("Demo Settings")]
		public float IntensityIncreasePerDay = 1.5f;

		// Token: 0x04001B3C RID: 6972
		private LawLoader loader = new LawLoader();
	}
}
