using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x02000698 RID: 1688
	[CreateAssetMenu(fileName = "New Dialogue Database", menuName = "Dialogue/Dialogue Database")]
	[Serializable]
	public class DialogueDatabase : ScriptableObject
	{
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002E97 RID: 11927 RVA: 0x000C3263 File Offset: 0x000C1463
		private List<DialogueModule> runtimeModules
		{
			get
			{
				return this.handler.runtimeModules;
			}
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000C3270 File Offset: 0x000C1470
		public void Initialize(DialogueHandler _handler)
		{
			this.handler = _handler;
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000C327C File Offset: 0x000C147C
		public DialogueModule GetModule(EDialogueModule moduleType)
		{
			if (this.runtimeModules == null)
			{
				Console.LogWarning("DialogueDatabase not initialized", null);
				return null;
			}
			DialogueModule dialogueModule = this.runtimeModules.Find((DialogueModule module) => module.ModuleType == moduleType);
			if (dialogueModule != null)
			{
				return dialogueModule;
			}
			return Singleton<DialogueManager>.Instance.Get(moduleType);
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x000C32E0 File Offset: 0x000C14E0
		public DialogueChain GetChain(EDialogueModule moduleType, string key)
		{
			DialogueModule module = this.GetModule(moduleType);
			if (module == null)
			{
				Console.LogWarning("Could not find module: " + moduleType.ToString(), null);
				return null;
			}
			return module.GetChain(key);
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x000C3324 File Offset: 0x000C1524
		public bool HasChain(EDialogueModule moduleType, string key)
		{
			DialogueModule module = this.GetModule(moduleType);
			if (module == null)
			{
				Console.LogWarning("Could not find module: " + moduleType.ToString(), null);
				return false;
			}
			return module.HasChain(key);
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x000C3368 File Offset: 0x000C1568
		public string GetLine(EDialogueModule moduleType, string key)
		{
			DialogueModule module = this.GetModule(moduleType);
			if (module == null)
			{
				Console.LogWarning("Could not find module: " + moduleType.ToString(), null);
				return string.Empty;
			}
			return module.GetLine(key);
		}

		// Token: 0x04002134 RID: 8500
		public List<DialogueModule> Modules;

		// Token: 0x04002135 RID: 8501
		public List<Entry> GenericEntries = new List<Entry>();

		// Token: 0x04002136 RID: 8502
		private DialogueHandler handler;
	}
}
