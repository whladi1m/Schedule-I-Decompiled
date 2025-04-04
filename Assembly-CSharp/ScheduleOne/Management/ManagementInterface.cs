using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management.UI;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using ScheduleOne.UI.Management;
using TMPro;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x0200057D RID: 1405
	public class ManagementInterface : Singleton<ManagementInterface>
	{
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x0600232C RID: 9004 RVA: 0x0008FD61 File Offset: 0x0008DF61
		// (set) Token: 0x0600232D RID: 9005 RVA: 0x0008FD69 File Offset: 0x0008DF69
		public ManagementClipboard_Equippable EquippedClipboard { get; protected set; }

		// Token: 0x0600232E RID: 9006 RVA: 0x0008FD72 File Offset: 0x0008DF72
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x0008FD7C File Offset: 0x0008DF7C
		public void Open(List<IConfigurable> configurables, ManagementClipboard_Equippable _equippedClipboard)
		{
			this.Configurables = new List<IConfigurable>();
			this.Configurables.AddRange(configurables);
			this.EquippedClipboard = _equippedClipboard;
			this.areConfigurablesUniform = true;
			if (this.Configurables.Count > 1)
			{
				for (int i = 0; i < this.Configurables.Count - 1; i++)
				{
					if (this.Configurables[i].ConfigurableType != this.Configurables[i + 1].ConfigurableType)
					{
						this.areConfigurablesUniform = false;
						break;
					}
				}
			}
			this.UpdateMainLabels();
			this.InitializeConfigPanel();
			Singleton<InputPromptsCanvas>.Instance.LoadModule("backonly_rightclick");
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x0008FE20 File Offset: 0x0008E020
		public void Close(bool preserveState = false)
		{
			if (this.ItemSelectorScreen.IsOpen)
			{
				this.ItemSelectorScreen.Close();
			}
			if (this.RecipeSelectorScreen.IsOpen)
			{
				this.RecipeSelectorScreen.Close();
			}
			if (Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "exitonly")
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			this.DestroyConfigPanel();
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x0008FE83 File Offset: 0x0008E083
		private void UpdateMainLabels()
		{
			this.NothingSelectedLabel.gameObject.SetActive(this.Configurables.Count == 0);
			this.DifferentTypesSelectedLabel.gameObject.SetActive(!this.areConfigurablesUniform);
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0008FEBC File Offset: 0x0008E0BC
		private void InitializeConfigPanel()
		{
			if (this.loadedPanel != null)
			{
				Console.LogWarning("InitializeConfigPanel called when there is an existing config panel. Destroying existing.", null);
				this.DestroyConfigPanel();
			}
			if (!this.areConfigurablesUniform || this.Configurables.Count == 0)
			{
				return;
			}
			ConfigPanel configPanelPrefab = this.GetConfigPanelPrefab(this.Configurables[0].ConfigurableType);
			this.loadedPanel = UnityEngine.Object.Instantiate<ConfigPanel>(configPanelPrefab, this.PanelContainer).GetComponent<ConfigPanel>();
			this.loadedPanel.Bind((from x in this.Configurables
			select x.Configuration).ToList<EntityConfiguration>());
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0008FF67 File Offset: 0x0008E167
		private void DestroyConfigPanel()
		{
			if (this.loadedPanel != null)
			{
				UnityEngine.Object.Destroy(this.loadedPanel.gameObject);
				this.loadedPanel = null;
			}
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0008FF90 File Offset: 0x0008E190
		public ConfigPanel GetConfigPanelPrefab(EConfigurableType type)
		{
			return this.ConfigPanelPrefabs.FirstOrDefault((ManagementInterface.ConfigurableTypePanel x) => x.Type == type).Panel;
		}

		// Token: 0x04001A51 RID: 6737
		public const float PANEL_SLIDE_TIME = 0.1f;

		// Token: 0x04001A53 RID: 6739
		[Header("References")]
		public TextMeshProUGUI NothingSelectedLabel;

		// Token: 0x04001A54 RID: 6740
		public TextMeshProUGUI DifferentTypesSelectedLabel;

		// Token: 0x04001A55 RID: 6741
		public RectTransform PanelContainer;

		// Token: 0x04001A56 RID: 6742
		public ClipboardScreen MainScreen;

		// Token: 0x04001A57 RID: 6743
		public ScheduleOne.UI.Management.ItemSelector ItemSelectorScreen;

		// Token: 0x04001A58 RID: 6744
		public NPCSelector NPCSelector;

		// Token: 0x04001A59 RID: 6745
		public ScheduleOne.UI.Management.ObjectSelector ObjectSelector;

		// Token: 0x04001A5A RID: 6746
		public RecipeSelector RecipeSelectorScreen;

		// Token: 0x04001A5B RID: 6747
		public TransitEntitySelector TransitEntitySelector;

		// Token: 0x04001A5C RID: 6748
		[SerializeField]
		protected ManagementInterface.ConfigurableTypePanel[] ConfigPanelPrefabs;

		// Token: 0x04001A5D RID: 6749
		public List<IConfigurable> Configurables = new List<IConfigurable>();

		// Token: 0x04001A5E RID: 6750
		private bool areConfigurablesUniform;

		// Token: 0x04001A5F RID: 6751
		private ConfigPanel loadedPanel;

		// Token: 0x0200057E RID: 1406
		[Serializable]
		public class ConfigurableTypePanel
		{
			// Token: 0x04001A60 RID: 6752
			public EConfigurableType Type;

			// Token: 0x04001A61 RID: 6753
			public ConfigPanel Panel;
		}
	}
}
