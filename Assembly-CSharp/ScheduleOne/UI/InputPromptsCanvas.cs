using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009DB RID: 2523
	public class InputPromptsCanvas : Singleton<InputPromptsCanvas>
	{
		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06004427 RID: 17447 RVA: 0x0011D78A File Offset: 0x0011B98A
		// (set) Token: 0x06004428 RID: 17448 RVA: 0x0011D792 File Offset: 0x0011B992
		public string currentModuleLabel { get; protected set; } = string.Empty;

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06004429 RID: 17449 RVA: 0x0011D79B File Offset: 0x0011B99B
		// (set) Token: 0x0600442A RID: 17450 RVA: 0x0011D7A3 File Offset: 0x0011B9A3
		public RectTransform currentModule { get; private set; }

		// Token: 0x0600442B RID: 17451 RVA: 0x0011D7AC File Offset: 0x0011B9AC
		public void LoadModule(string key)
		{
			GameObject module = this.Modules.Find((InputPromptsCanvas.Module x) => x.key.ToLower() == key.ToLower()).module;
			if (module == null)
			{
				Console.LogError("Input prompt module with key '" + key + "' not found!", null);
				return;
			}
			if (this.currentModule != null)
			{
				this.UnloadModule();
			}
			this.currentModuleLabel = key;
			this.currentModule = UnityEngine.Object.Instantiate<GameObject>(module, this.InputPromptsContainer).GetComponent<RectTransform>();
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x0011D83F File Offset: 0x0011BA3F
		public void UnloadModule()
		{
			this.currentModuleLabel = string.Empty;
			if (this.currentModule != null)
			{
				UnityEngine.Object.Destroy(this.currentModule.gameObject);
			}
		}

		// Token: 0x04003206 RID: 12806
		public RectTransform InputPromptsContainer;

		// Token: 0x04003207 RID: 12807
		[Header("Input prompt modules")]
		public List<InputPromptsCanvas.Module> Modules = new List<InputPromptsCanvas.Module>();

		// Token: 0x020009DC RID: 2524
		[Serializable]
		public class Module
		{
			// Token: 0x0400320A RID: 12810
			public string key;

			// Token: 0x0400320B RID: 12811
			public GameObject module;
		}
	}
}
