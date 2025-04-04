using System;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet.Serializing.Helping;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Map;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Phone;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x020002DD RID: 733
	[Serializable]
	public class QuestEntry : MonoBehaviour
	{
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06001019 RID: 4121 RVA: 0x00047B41 File Offset: 0x00045D41
		// (set) Token: 0x0600101A RID: 4122 RVA: 0x00047B49 File Offset: 0x00045D49
		[CodegenExclude]
		public Quest ParentQuest { get; private set; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x00047B52 File Offset: 0x00045D52
		[CodegenExclude]
		public string Title
		{
			get
			{
				return this.EntryTitle;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x0600101C RID: 4124 RVA: 0x00047B5A File Offset: 0x00045D5A
		[CodegenExclude]
		public EQuestState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00047B64 File Offset: 0x00045D64
		protected virtual void Awake()
		{
			this.ParentQuest = base.GetComponentInParent<Quest>();
			this.ParentQuest.onQuestEnd.AddListener(delegate(EQuestState <p0>)
			{
				this.DestroyPoI();
			});
			this.ParentQuest.onTrackChange.AddListener(delegate(bool b)
			{
				this.UpdatePoI();
			});
			if (this.AutoComplete)
			{
				StateMachine.OnStateChange = (Action)Delegate.Combine(StateMachine.OnStateChange, new Action(this.EvaluateConditions));
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00047BE0 File Offset: 0x00045DE0
		protected virtual void Start()
		{
			if (this.AutoCreatePoI && this.PoI == null)
			{
				this.CreatePoI();
			}
			if (!this.ParentQuest.Entries.Contains(this))
			{
				Console.LogError(string.Concat(new string[]
				{
					"Parent quest '",
					this.ParentQuest.GetQuestTitle(),
					"' does not contain entry '",
					this.EntryTitle,
					"'."
				}), null);
			}
			if (this.ParentQuest.hudUIExists)
			{
				this.CreateEntryUI();
			}
			else
			{
				Quest parentQuest = this.ParentQuest;
				parentQuest.onHudUICreated = (Action)Delegate.Combine(parentQuest.onHudUICreated, new Action(this.CreateEntryUI));
			}
			this.CreateCompassElement();
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00047C9F File Offset: 0x00045E9F
		private void OnValidate()
		{
			this.UpdateName();
			if (this.EntryAddedIn == null || this.EntryAddedIn == string.Empty)
			{
				this.EntryAddedIn = Application.version;
			}
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00047CCC File Offset: 0x00045ECC
		public virtual void MinPass()
		{
			if (this.AutoUpdatePoILocation && this.PoI != null)
			{
				this.PoI.transform.position = this.PoILocation.position;
				this.PoI.UpdatePosition();
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00047D0A File Offset: 0x00045F0A
		public void SetData(QuestEntryData data)
		{
			this.EntryTitle = data.Name;
			this.SetState(data.State, false);
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00047D25 File Offset: 0x00045F25
		public void Begin()
		{
			this.SetState(EQuestState.Active, true);
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x00047D2F File Offset: 0x00045F2F
		public void Complete()
		{
			this.SetState(EQuestState.Completed, true);
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x00047D39 File Offset: 0x00045F39
		public void SetActive(bool network = true)
		{
			this.SetState(EQuestState.Active, network);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00047D44 File Offset: 0x00045F44
		public virtual void SetState(EQuestState newState, bool network = true)
		{
			EQuestState equestState = this.state;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			this.state = newState;
			if (newState == EQuestState.Active && equestState != EQuestState.Active)
			{
				if (this.onStart != null)
				{
					this.onStart.Invoke();
				}
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			}
			if (newState != EQuestState.Active && equestState == EQuestState.Active && this.onEnd != null)
			{
				this.onEnd.Invoke();
			}
			if (newState == EQuestState.Completed && equestState != EQuestState.Completed)
			{
				if (this.onComplete != null)
				{
					this.onComplete.Invoke();
				}
				if (equestState == EQuestState.Active)
				{
					if (this.onInitialComplete != null)
					{
						this.onInitialComplete.Invoke();
					}
					NetworkSingleton<QuestManager>.Instance.PlayCompleteQuestEntrySound();
				}
				if (this.CompleteParentQuest)
				{
					this.ParentQuest.Complete(network);
				}
			}
			if (this.PoI != null)
			{
				this.PoI.gameObject.SetActive(this.ShouldShowPoI());
			}
			this.ParentQuest.UpdateHUDUI();
			this.UpdateCompassElement();
			if (network)
			{
				int entryIndex = this.ParentQuest.Entries.ToList<QuestEntry>().IndexOf(this);
				NetworkSingleton<QuestManager>.Instance.SendQuestEntryState(this.ParentQuest.GUID.ToString(), entryIndex, newState);
			}
			this.UpdateName();
			StateMachine.ChangeState();
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00047EB2 File Offset: 0x000460B2
		protected virtual bool ShouldShowPoI()
		{
			return this.State == EQuestState.Active && this.ParentQuest.IsTracked;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00047ECA File Offset: 0x000460CA
		protected virtual void UpdatePoI()
		{
			if (this.PoI != null)
			{
				this.PoI.gameObject.SetActive(this.ShouldShowPoI());
			}
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00047EF0 File Offset: 0x000460F0
		public void SetPoILocation(Vector3 location)
		{
			this.PoILocation.position = location;
			if (this.PoI != null)
			{
				this.PoI.transform.position = location;
				this.PoI.UpdatePosition();
			}
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00047F28 File Offset: 0x00046128
		public void CreatePoI()
		{
			if (this.PoI != null)
			{
				Console.LogWarning("PoI already exists for quest entry " + this.EntryTitle, null);
				return;
			}
			if (this.ParentQuest == null)
			{
				Console.LogWarning("Parent quest is null for quest entry " + this.EntryTitle, null);
				return;
			}
			if (this.PoILocation == null)
			{
				Console.LogWarning("PoI location is null for quest entry " + this.EntryTitle, null);
				return;
			}
			this.PoI = UnityEngine.Object.Instantiate<GameObject>(this.ParentQuest.PoIPrefab, base.transform).GetComponent<POI>();
			this.PoI.transform.position = this.PoILocation.position;
			this.PoI.SetMainText(this.Title);
			this.PoI.UpdatePosition();
			this.PoI.gameObject.SetActive(this.ShouldShowPoI());
			if (this.PoI.IconContainer != null)
			{
				this.<CreatePoI>g__CreateUI|36_0();
				return;
			}
			this.PoI.onUICreated.AddListener(new UnityAction(this.<CreatePoI>g__CreateUI|36_0));
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00048048 File Offset: 0x00046248
		public void DestroyPoI()
		{
			if (this.PoI != null)
			{
				UnityEngine.Object.Destroy(this.PoI.gameObject);
				this.PoI = null;
			}
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00048070 File Offset: 0x00046270
		public void CreateCompassElement()
		{
			if (this.compassElement != null)
			{
				Console.LogWarning("Compass element already exists for quest: " + this.Title, null);
				return;
			}
			this.compassElement = Singleton<CompassManager>.Instance.AddElement(this.PoILocation, this.ParentQuest.IconPrefab, this.state == EQuestState.Active);
			this.UpdateCompassElement();
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x000480CC File Offset: 0x000462CC
		public void UpdateCompassElement()
		{
			if (this.compassElement == null)
			{
				return;
			}
			this.compassElement.Transform = this.PoILocation;
			this.compassElement.Visible = (this.ParentQuest.QuestState == EQuestState.Active && this.ParentQuest.IsTracked && this.state == EQuestState.Active && this.PoILocation != null);
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x00048131 File Offset: 0x00046331
		public QuestEntryData GetSaveData()
		{
			return new QuestEntryData(this.EntryTitle, this.state);
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x00048144 File Offset: 0x00046344
		private void UpdateName()
		{
			base.name = this.EntryTitle + " (" + this.state.ToString() + ")";
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x00048172 File Offset: 0x00046372
		private void EvaluateConditions()
		{
			if (this.State != EQuestState.Active)
			{
				return;
			}
			if (this.AutoCompleteConditions.Evaluate())
			{
				this.SetState(EQuestState.Completed, true);
			}
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00048193 File Offset: 0x00046393
		public void SetEntryTitle(string newTitle)
		{
			this.EntryTitle = newTitle;
			this.ParentQuest.UpdateHUDUI();
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x000481A8 File Offset: 0x000463A8
		protected virtual void CreateEntryUI()
		{
			if (!this.ParentQuest.hudUIExists)
			{
				Console.LogWarning("Quest HUD UI does not exist for quest " + this.ParentQuest.GetQuestTitle(), null);
				return;
			}
			this.entryUI = UnityEngine.Object.Instantiate<QuestEntryHUDUI>(PlayerSingleton<JournalApp>.Instance.QuestEntryHUDUIPrefab, this.ParentQuest.hudUI.EntryContainer).GetComponent<QuestEntryHUDUI>();
			this.entryUI.Initialize(this);
			this.UpdateEntryUI();
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0004821A File Offset: 0x0004641A
		public virtual void UpdateEntryUI()
		{
			this.entryUI.UpdateUI();
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x00048294 File Offset: 0x00046494
		[CompilerGenerated]
		private void <CreatePoI>g__CreateUI|36_0()
		{
			if (this.PoI != null)
			{
				Console.LogWarning("PoI already exists for quest entry " + this.EntryTitle, null);
				return;
			}
			if (this.ParentQuest == null)
			{
				Console.LogWarning("Parent quest is null for quest entry " + this.EntryTitle, null);
				return;
			}
			UnityEngine.Object.Instantiate<GameObject>(this.ParentQuest.IconPrefab.gameObject, this.PoI.IconContainer).GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 20f);
		}

		// Token: 0x040010AA RID: 4266
		[Header("Naming")]
		[SerializeField]
		protected string EntryTitle = string.Empty;

		// Token: 0x040010AB RID: 4267
		[SerializeField]
		protected EQuestState state;

		// Token: 0x040010AC RID: 4268
		[Header("Settings")]
		public bool AutoComplete;

		// Token: 0x040010AD RID: 4269
		public Conditions AutoCompleteConditions;

		// Token: 0x040010AE RID: 4270
		public bool CompleteParentQuest;

		// Token: 0x040010AF RID: 4271
		public string EntryAddedIn = "0.0.1";

		// Token: 0x040010B0 RID: 4272
		[Header("PoI Settings")]
		public bool AutoCreatePoI = true;

		// Token: 0x040010B1 RID: 4273
		public Transform PoILocation;

		// Token: 0x040010B2 RID: 4274
		public bool AutoUpdatePoILocation;

		// Token: 0x040010B3 RID: 4275
		public POI PoI;

		// Token: 0x040010B4 RID: 4276
		public UnityEvent onStart = new UnityEvent();

		// Token: 0x040010B5 RID: 4277
		public UnityEvent onEnd = new UnityEvent();

		// Token: 0x040010B6 RID: 4278
		public UnityEvent onComplete = new UnityEvent();

		// Token: 0x040010B7 RID: 4279
		public UnityEvent onInitialComplete = new UnityEvent();

		// Token: 0x040010B8 RID: 4280
		private CompassManager.Element compassElement;

		// Token: 0x040010B9 RID: 4281
		private QuestEntryHUDUI entryUI;
	}
}
