using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A29 RID: 2601
	public class TaskManagerUI : Singleton<TaskManagerUI>
	{
		// Token: 0x06004638 RID: 17976 RVA: 0x00125E6F File Offset: 0x0012406F
		protected virtual void Update()
		{
			this.UpdateInstructionLabel();
			this.canvas.enabled = (Singleton<TaskManager>.Instance.currentTask != null);
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x00125E8F File Offset: 0x0012408F
		protected override void Start()
		{
			base.Start();
			TaskManager instance = Singleton<TaskManager>.Instance;
			instance.OnTaskStarted = (Action<Task>)Delegate.Combine(instance.OnTaskStarted, new Action<Task>(this.TaskStarted));
			this.multiGrabIndicator.gameObject.SetActive(false);
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x00125ED0 File Offset: 0x001240D0
		protected virtual void UpdateInstructionLabel()
		{
			if (Singleton<TaskManager>.Instance.currentTask != null && Singleton<TaskManager>.Instance.currentTask.CurrentInstruction != string.Empty)
			{
				this.textShown = true;
				Singleton<HUD>.Instance.ShowTopScreenText(Singleton<TaskManager>.Instance.currentTask.CurrentInstruction);
				return;
			}
			if (this.textShown)
			{
				this.textShown = false;
				Singleton<HUD>.Instance.HideTopScreenText();
			}
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x00125F40 File Offset: 0x00124140
		private void TaskStarted(Task task)
		{
			bool value = NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("InputHintsTutorialDone");
			this.multiGrabIndicator.gameObject.SetActive(false);
			if (GameManager.IS_TUTORIAL && !value && !Application.isEditor)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("InputHintsTutorialDone", true.ToString(), true);
				this.inputPromptUI.Open();
			}
		}

		// Token: 0x040033EB RID: 13291
		private bool textShown;

		// Token: 0x040033EC RID: 13292
		public GenericUIScreen inputPromptUI;

		// Token: 0x040033ED RID: 13293
		public Canvas canvas;

		// Token: 0x040033EE RID: 13294
		public RectTransform multiGrabIndicator;

		// Token: 0x040033EF RID: 13295
		public GenericUIScreen PackagingStationMK2TutorialDone;
	}
}
