using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000379 RID: 889
	public class LoadRequest
	{
		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x00059FCC File Offset: 0x000581CC
		// (set) Token: 0x06001428 RID: 5160 RVA: 0x00059FD4 File Offset: 0x000581D4
		public bool IsDone { get; private set; }

		// Token: 0x06001429 RID: 5161 RVA: 0x00059FDD File Offset: 0x000581DD
		public LoadRequest(string filePath, Loader loader)
		{
			if (loader == null)
			{
				Debug.LogError("Loader is null for file path: " + filePath);
				return;
			}
			this.Path = filePath;
			this.Loader = loader;
			Singleton<LoadManager>.Instance.QueueLoadRequest(this);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0005A012 File Offset: 0x00058212
		public void Complete()
		{
			Singleton<LoadManager>.Instance.DequeueLoadRequest(this);
			this.Loader.Load(this.Path);
			this.IsDone = true;
		}

		// Token: 0x04001302 RID: 4866
		public string Path;

		// Token: 0x04001303 RID: 4867
		public Loader Loader;
	}
}
