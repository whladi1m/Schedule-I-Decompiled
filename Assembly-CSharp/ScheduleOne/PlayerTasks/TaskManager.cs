using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000341 RID: 833
	public class TaskManager : Singleton<TaskManager>
	{
		// Token: 0x060012AF RID: 4783 RVA: 0x00051EAE File Offset: 0x000500AE
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x00051EC8 File Offset: 0x000500C8
		protected virtual void Update()
		{
			if (this.currentTask != null)
			{
				this.currentTask.Update();
			}
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00051EDD File Offset: 0x000500DD
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (action.exitType == ExitType.Escape && this.currentTask != null)
			{
				action.used = true;
				this.currentTask.Outcome = Task.EOutcome.Cancelled;
				this.currentTask.StopTask();
			}
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00051F17 File Offset: 0x00050117
		protected virtual void LateUpdate()
		{
			if (this.currentTask != null)
			{
				this.currentTask.LateUpdate();
			}
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00051F2C File Offset: 0x0005012C
		protected virtual void FixedUpdate()
		{
			if (this.currentTask != null)
			{
				this.currentTask.FixedUpdate();
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00051F41 File Offset: 0x00050141
		public void PlayTaskCompleteSound()
		{
			this.TaskCompleteSound.Play();
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00051F4E File Offset: 0x0005014E
		public void StartTask(Task task)
		{
			this.currentTask = task;
			if (this.OnTaskStarted != null)
			{
				this.OnTaskStarted(task);
			}
		}

		// Token: 0x04001219 RID: 4633
		public Task currentTask;

		// Token: 0x0400121A RID: 4634
		public AudioSourceController TaskCompleteSound;

		// Token: 0x0400121B RID: 4635
		public Action<Task> OnTaskStarted;
	}
}
