using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A08 RID: 2568
	public class OffenceNoticeUI : Singleton<OffenceNoticeUI>
	{
		// Token: 0x06004551 RID: 17745 RVA: 0x00122788 File Offset: 0x00120988
		public void ShowOffenceNotice(Offense offence)
		{
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			for (int i = 0; i < this.charges.Count; i++)
			{
				if (i < offence.charges.Count)
				{
					string str = "- ";
					if (offence.charges[i].quantity > 1)
					{
						str = "- " + offence.charges[i].quantity.ToString() + "x ";
					}
					this.charges[i].text = str + offence.charges[i].chargeName;
					this.charges[i].enabled = true;
				}
				else
				{
					this.charges[i].enabled = false;
				}
			}
			for (int j = 0; j < this.penalties.Count; j++)
			{
				if (j < offence.penalties.Count)
				{
					string str2 = "- ";
					this.penalties[j].text = str2 + offence.penalties[j];
					this.penalties[j].enabled = true;
				}
				else
				{
					this.penalties[j].enabled = false;
				}
			}
			this.container.gameObject.SetActive(true);
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x001228E4 File Offset: 0x00120AE4
		protected void Update()
		{
			if (this.container.activeSelf && GameInput.GetButtonDown(GameInput.ButtonCode.Escape))
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				this.container.gameObject.SetActive(false);
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				Singleton<HUD>.Instance.SetCrosshairVisible(true);
				PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.2f);
			}
		}

		// Token: 0x0400332D RID: 13101
		[Header("References")]
		[SerializeField]
		protected GameObject container;

		// Token: 0x0400332E RID: 13102
		[SerializeField]
		protected List<Text> charges = new List<Text>();

		// Token: 0x0400332F RID: 13103
		[SerializeField]
		protected List<Text> penalties = new List<Text>();
	}
}
