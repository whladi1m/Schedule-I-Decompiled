using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Law;
using ScheduleOne.NPCs.Schedules;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000461 RID: 1121
	public class NPCScheduleManager : MonoBehaviour
	{
		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060017D3 RID: 6099 RVA: 0x00069272 File Offset: 0x00067472
		// (set) Token: 0x060017D4 RID: 6100 RVA: 0x0006927A File Offset: 0x0006747A
		public bool ScheduleEnabled { get; protected set; }

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060017D5 RID: 6101 RVA: 0x00069283 File Offset: 0x00067483
		// (set) Token: 0x060017D6 RID: 6102 RVA: 0x0006928B File Offset: 0x0006748B
		public bool CurfewModeEnabled { get; protected set; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060017D7 RID: 6103 RVA: 0x00069294 File Offset: 0x00067494
		// (set) Token: 0x060017D8 RID: 6104 RVA: 0x0006929C File Offset: 0x0006749C
		public NPCAction ActiveAction { get; set; }

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060017D9 RID: 6105 RVA: 0x000692A5 File Offset: 0x000674A5
		// (set) Token: 0x060017DA RID: 6106 RVA: 0x000692AD File Offset: 0x000674AD
		public List<NPCAction> PendingActions { get; set; } = new List<NPCAction>();

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060017DB RID: 6107 RVA: 0x000692B6 File Offset: 0x000674B6
		// (set) Token: 0x060017DC RID: 6108 RVA: 0x000692BE File Offset: 0x000674BE
		public NPC Npc { get; protected set; }

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060017DD RID: 6109 RVA: 0x000692C7 File Offset: 0x000674C7
		// (set) Token: 0x060017DE RID: 6110 RVA: 0x000692CF File Offset: 0x000674CF
		protected List<NPCAction> ActionsAwaitingStart { get; set; } = new List<NPCAction>();

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060017DF RID: 6111 RVA: 0x000692D8 File Offset: 0x000674D8
		protected TimeManager Time
		{
			get
			{
				return NetworkSingleton<TimeManager>.Instance;
			}
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x000692DF File Offset: 0x000674DF
		protected virtual void Awake()
		{
			this.Npc = base.GetComponentInParent<NPC>();
			this.SetCurfewModeEnabled(false);
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x000692F4 File Offset: 0x000674F4
		protected virtual void Start()
		{
			this.InitializeActions();
			TimeManager time = this.Time;
			time.onTimeChanged = (Action)Delegate.Remove(time.onTimeChanged, new Action(this.EnforceState));
			TimeManager time2 = this.Time;
			time2.onTimeChanged = (Action)Delegate.Combine(time2.onTimeChanged, new Action(this.EnforceState));
			TimeManager time3 = this.Time;
			time3.onMinutePass = (Action)Delegate.Remove(time3.onMinutePass, new Action(this.MinPass));
			TimeManager time4 = this.Time;
			time4.onMinutePass = (Action)Delegate.Combine(time4.onMinutePass, new Action(this.MinPass));
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.LocalPlayerSpawned));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.LocalPlayerSpawned));
			NetworkSingleton<CurfewManager>.Instance.onCurfewEnabled.AddListener(new UnityAction(this.CurfewEnabled));
			NetworkSingleton<CurfewManager>.Instance.onCurfewDisabled.AddListener(new UnityAction(this.CurfewDisabled));
			if (this.DEBUG_MODE)
			{
				int min = 1250;
				int max = 930;
				this.GetActionsTotallyOccurringWithinRange(min, max, true);
			}
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0006943B File Offset: 0x0006763B
		private void LocalPlayerSpawned()
		{
			if (InstanceFinder.IsServer)
			{
				this.EnforceState(true);
			}
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00033870 File Offset: 0x00031A70
		private void OnValidate()
		{
			bool isPlaying = Application.isPlaying;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0006944B File Offset: 0x0006764B
		protected virtual void Update()
		{
			if (this.ActiveAction != null)
			{
				this.ActiveAction.ActiveUpdate();
			}
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00069466 File Offset: 0x00067666
		public void EnableSchedule()
		{
			this.ScheduleEnabled = true;
			this.MinPass();
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00069475 File Offset: 0x00067675
		public void DisableSchedule()
		{
			this.ScheduleEnabled = false;
			this.MinPass();
			if (this.Npc.Movement.IsMoving)
			{
				this.Npc.Movement.Stop();
			}
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x000694A8 File Offset: 0x000676A8
		[Button]
		public void InitializeActions()
		{
			List<NPCAction> list = base.gameObject.GetComponentsInChildren<NPCAction>(true).ToList<NPCAction>();
			list.Sort(delegate(NPCAction a, NPCAction b)
			{
				float num = (float)a.StartTime;
				float value = (float)b.StartTime;
				int num2 = num.CompareTo(value);
				if (num2 != 0)
				{
					return num2;
				}
				if (a.IsSignal)
				{
					return -1;
				}
				return 1;
			});
			if (!Application.isPlaying)
			{
				foreach (NPCAction npcaction in list)
				{
					npcaction.transform.name = npcaction.GetName() + " (" + npcaction.GetTimeDescription() + ")";
					npcaction.transform.SetAsLastSibling();
				}
			}
			this.ActionList = list;
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00069568 File Offset: 0x00067768
		protected virtual void MinPass()
		{
			if (!this.Npc.IsSpawned)
			{
				return;
			}
			if (!this.ScheduleEnabled)
			{
				if (this.ActiveAction != null)
				{
					this.ActiveAction.Interrupt();
				}
				return;
			}
			if (this.ActiveAction != null)
			{
				this.ActiveAction.ActiveMinPassed();
			}
			if (this.ActiveAction != null && !this.ActiveAction.gameObject.activeInHierarchy)
			{
				this.ActiveAction.End();
			}
			List<NPCAction> actionsOccurringAt = this.GetActionsOccurringAt(NetworkSingleton<TimeManager>.Instance.CurrentTime);
			bool debug_MODE = this.DEBUG_MODE;
			if (actionsOccurringAt.Count > 0)
			{
				NPCAction npcaction = actionsOccurringAt[0];
				if (this.ActiveAction != npcaction)
				{
					if (this.ActiveAction != null && npcaction.Priority > this.ActiveAction.Priority)
					{
						if (this.DEBUG_MODE)
						{
							Debug.Log("New active action: " + npcaction.GetName());
						}
						this.ActiveAction.Interrupt();
					}
					if (this.ActiveAction == null)
					{
						this.StartAction(npcaction);
					}
				}
			}
			foreach (NPCAction npcaction2 in actionsOccurringAt)
			{
				if (!npcaction2.HasStarted && !this.ActionsAwaitingStart.Contains(npcaction2))
				{
					this.ActionsAwaitingStart.Add(npcaction2);
				}
			}
			foreach (NPCAction npcaction3 in this.ActionsAwaitingStart.ToList<NPCAction>())
			{
				if (!NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(npcaction3.StartTime, npcaction3.GetEndTime()))
				{
					npcaction3.Skipped();
					this.ActionsAwaitingStart.Remove(npcaction3);
				}
			}
			this.lastProcessedTime = this.Time.CurrentTime;
			if (this.DEBUG_MODE)
			{
				Console.Log("Active action: " + ((this.ActiveAction != null) ? this.ActiveAction.GetName() : "None"), null);
			}
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x00069794 File Offset: 0x00067994
		private List<NPCAction> GetActionsOccurringAt(int time)
		{
			List<NPCAction> list = new List<NPCAction>();
			foreach (NPCAction npcaction in this.ActionList)
			{
				if (!(npcaction == null) && npcaction.ShouldStart() && TimeManager.IsGivenTimeWithinRange(time, npcaction.StartTime, TimeManager.AddMinutesTo24HourTime(npcaction.GetEndTime(), -1)))
				{
					list.Add(npcaction);
				}
			}
			list = (from x in list
			orderby x.Priority descending
			select x).ToList<NPCAction>();
			return list;
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00069844 File Offset: 0x00067A44
		private List<NPCAction> GetActionsTotallyOccurringWithinRange(int min, int max, bool checkShouldStart)
		{
			List<NPCAction> list = new List<NPCAction>();
			foreach (NPCAction npcaction in this.ActionList)
			{
				if ((!checkShouldStart || npcaction.ShouldStart()) && TimeManager.IsGivenTimeWithinRange(npcaction.StartTime, min, max) && TimeManager.IsGivenTimeWithinRange(npcaction.GetEndTime(), min, max))
				{
					list.Add(npcaction);
				}
			}
			list = (from x in list
			orderby x.Priority descending
			select x).ToList<NPCAction>();
			bool debug_MODE = this.DEBUG_MODE;
			return list;
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x000698FC File Offset: 0x00067AFC
		private void StartAction(NPCAction action)
		{
			if (this.ActiveAction != null)
			{
				Console.LogWarning("JumpToAction called but there is already an active action! Existing action should first be ended or interrupted!", null);
			}
			if (this.ActionsAwaitingStart.Contains(action))
			{
				this.ActionsAwaitingStart.Remove(action);
			}
			if (NetworkSingleton<TimeManager>.Instance.CurrentTime == action.StartTime)
			{
				action.Started();
				return;
			}
			if (action.HasStarted)
			{
				action.Resume();
				return;
			}
			action.LateStarted();
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0006996B File Offset: 0x00067B6B
		private void EnforceState()
		{
			this.EnforceState(Singleton<LoadManager>.Instance.IsLoading);
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x00069980 File Offset: 0x00067B80
		public void EnforceState(bool initial = false)
		{
			this.ActionsAwaitingStart.Clear();
			int currentTime = NetworkSingleton<TimeManager>.Instance.CurrentTime;
			int minSumFrom24HourTime = TimeManager.GetMinSumFrom24HourTime(currentTime);
			if (this.DEBUG_MODE)
			{
				Debug.Log("Enforcing state. Last processed time: " + this.lastProcessedTime.ToString() + ", Current time: " + NetworkSingleton<TimeManager>.Instance.CurrentTime.ToString());
			}
			List<NPCAction> list = this.GetActionsTotallyOccurringWithinRange(this.lastProcessedTime, NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
			List<NPCAction> actionsOccurringThisFrame = this.GetActionsOccurringAt(NetworkSingleton<TimeManager>.Instance.CurrentTime);
			list.RemoveAll((NPCAction x) => x.IsActive || actionsOccurringThisFrame.Contains(x));
			NPCAction npcaction = null;
			if (actionsOccurringThisFrame.Count > 0)
			{
				npcaction = actionsOccurringThisFrame[0];
			}
			if (this.ActiveAction != null && this.ActiveAction != npcaction)
			{
				this.ActiveAction.Interrupt();
			}
			Dictionary<NPCAction, float> skippedActionOrder = new Dictionary<NPCAction, float>();
			for (int i = 0; i < list.Count; i++)
			{
				float num;
				if (list[i].StartTime >= currentTime)
				{
					num = (float)(TimeManager.GetMinSumFrom24HourTime(list[i].StartTime) - minSumFrom24HourTime);
				}
				else
				{
					num = 1440f - (float)minSumFrom24HourTime + (float)TimeManager.GetMinSumFrom24HourTime(list[i].StartTime);
				}
				num -= 0.01f * (float)list[i].Priority;
				skippedActionOrder.Add(list[i], num);
			}
			list = (from x in list
			orderby skippedActionOrder[x]
			select x).ToList<NPCAction>();
			if (this.DEBUG_MODE)
			{
				Debug.Log("Ordered skipped actions: " + list.Count.ToString());
			}
			if (!initial)
			{
				for (int j = 0; j < list.Count; j++)
				{
					list[j].Skipped();
				}
			}
			if (npcaction != null)
			{
				npcaction.JumpTo();
			}
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00069B88 File Offset: 0x00067D88
		protected virtual void CurfewEnabled()
		{
			this.SetCurfewModeEnabled(true);
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00069B91 File Offset: 0x00067D91
		protected virtual void CurfewDisabled()
		{
			this.SetCurfewModeEnabled(false);
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00069B9C File Offset: 0x00067D9C
		public void SetCurfewModeEnabled(bool enabled)
		{
			for (int i = 0; i < this.EnabledDuringCurfew.Length; i++)
			{
				this.EnabledDuringCurfew[i].gameObject.SetActive(enabled);
			}
			for (int j = 0; j < this.EnabledDuringNoCurfew.Length; j++)
			{
				this.EnabledDuringNoCurfew[j].gameObject.SetActive(!enabled);
			}
		}

		// Token: 0x04001578 RID: 5496
		public bool DEBUG_MODE;

		// Token: 0x0400157C RID: 5500
		[Header("References")]
		public GameObject[] EnabledDuringCurfew;

		// Token: 0x0400157D RID: 5501
		public GameObject[] EnabledDuringNoCurfew;

		// Token: 0x0400157E RID: 5502
		public List<NPCAction> ActionList = new List<NPCAction>();

		// Token: 0x04001580 RID: 5504
		protected int lastProcessedTime;
	}
}
