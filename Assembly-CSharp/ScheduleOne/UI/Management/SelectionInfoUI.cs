using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AF5 RID: 2805
	public class SelectionInfoUI : MonoBehaviour
	{
		// Token: 0x06004B08 RID: 19208 RVA: 0x0013B0E4 File Offset: 0x001392E4
		private void Update()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.SelfUpdate)
			{
				List<IConfigurable> list = new List<IConfigurable>();
				list.AddRange(Singleton<ManagementWorldspaceCanvas>.Instance.SelectedConfigurables);
				if (Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable != null && !list.Contains(Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable))
				{
					list.Add(Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable);
				}
				this.Set(list);
			}
		}

		// Token: 0x06004B09 RID: 19209 RVA: 0x0013B154 File Offset: 0x00139354
		public void Set(List<IConfigurable> Configurables)
		{
			if (Configurables.Count == 0)
			{
				this.Icon.sprite = this.CrossSprite;
				this.Title.text = "Nothing selected";
				return;
			}
			bool flag = true;
			if (Configurables.Count > 1)
			{
				for (int i = 0; i < Configurables.Count - 1; i++)
				{
					if (Configurables[i].ConfigurableType != Configurables[i + 1].ConfigurableType)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				this.Icon.sprite = Configurables[0].TypeIcon;
				this.Title.text = Configurables.Count.ToString() + "x " + ConfigurableType.GetTypeName(Configurables[0].ConfigurableType);
				return;
			}
			this.Icon.sprite = this.NonUniformTypeSprite;
			this.Title.text = Configurables.Count.ToString() + "x Mixed types";
		}

		// Token: 0x0400387C RID: 14460
		[Header("References")]
		public Image Icon;

		// Token: 0x0400387D RID: 14461
		public TextMeshProUGUI Title;

		// Token: 0x0400387E RID: 14462
		[Header("Settings")]
		public bool SelfUpdate = true;

		// Token: 0x0400387F RID: 14463
		public Sprite NonUniformTypeSprite;

		// Token: 0x04003880 RID: 14464
		public Sprite CrossSprite;
	}
}
