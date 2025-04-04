using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000380 RID: 896
	public class SaveRequest
	{
		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001460 RID: 5216 RVA: 0x0005B051 File Offset: 0x00059251
		// (set) Token: 0x06001461 RID: 5217 RVA: 0x0005B059 File Offset: 0x00059259
		public string SaveString { get; private set; }

		// Token: 0x06001462 RID: 5218 RVA: 0x0005B064 File Offset: 0x00059264
		public SaveRequest(ISaveable saveable, string parentFolderPath)
		{
			this.Saveable = saveable;
			this.ParentFolderPath = parentFolderPath;
			this.SaveString = saveable.GetSaveString();
			if (this.SaveString != string.Empty)
			{
				Singleton<SaveManager>.Instance.QueueSaveRequest(this);
				return;
			}
			saveable.CompleteSave(parentFolderPath, false);
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0005B0B7 File Offset: 0x000592B7
		public void Complete()
		{
			Singleton<SaveManager>.Instance.DequeueSaveRequest(this);
			this.Saveable.WriteBaseData(this.ParentFolderPath, this.SaveString);
		}

		// Token: 0x04001331 RID: 4913
		public ISaveable Saveable;

		// Token: 0x04001332 RID: 4914
		public string ParentFolderPath;
	}
}
