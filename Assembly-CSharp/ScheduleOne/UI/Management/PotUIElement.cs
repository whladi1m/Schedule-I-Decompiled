using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B03 RID: 2819
	public class PotUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06004B56 RID: 19286 RVA: 0x0013C2B3 File Offset: 0x0013A4B3
		// (set) Token: 0x06004B57 RID: 19287 RVA: 0x0013C2BB File Offset: 0x0013A4BB
		public Pot AssignedPot { get; protected set; }

		// Token: 0x06004B58 RID: 19288 RVA: 0x0013C2C4 File Offset: 0x0013A4C4
		public void Initialize(Pot pot)
		{
			this.AssignedPot = pot;
			this.AssignedPot.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x0013C304 File Offset: 0x0013A504
		protected virtual void RefreshUI()
		{
			PotConfiguration potConfiguration = this.AssignedPot.Configuration as PotConfiguration;
			this.NoSeed.gameObject.SetActive(potConfiguration.Seed.SelectedItem == null);
			this.SeedIcon.gameObject.SetActive(potConfiguration.Seed.SelectedItem != null);
			if (potConfiguration.Seed.SelectedItem != null)
			{
				this.SeedIcon.sprite = potConfiguration.Seed.SelectedItem.Icon;
			}
			if (potConfiguration.Additive1.SelectedItem != null)
			{
				this.Additive1Icon.sprite = potConfiguration.Additive1.SelectedItem.Icon;
				this.Additive1Icon.gameObject.SetActive(true);
			}
			else
			{
				this.Additive1Icon.gameObject.SetActive(false);
			}
			if (potConfiguration.Additive2.SelectedItem != null)
			{
				this.Additive2Icon.sprite = potConfiguration.Additive2.SelectedItem.Icon;
				this.Additive2Icon.gameObject.SetActive(true);
			}
			else
			{
				this.Additive2Icon.gameObject.SetActive(false);
			}
			if (potConfiguration.Additive3.SelectedItem != null)
			{
				this.Additive3Icon.sprite = potConfiguration.Additive3.SelectedItem.Icon;
				this.Additive3Icon.gameObject.SetActive(true);
			}
			else
			{
				this.Additive3Icon.gameObject.SetActive(false);
			}
			base.SetAssignedNPC(potConfiguration.AssignedBotanist.SelectedNPC);
		}

		// Token: 0x040038A7 RID: 14503
		[Header("References")]
		public Image SeedIcon;

		// Token: 0x040038A8 RID: 14504
		public GameObject NoSeed;

		// Token: 0x040038A9 RID: 14505
		public Image Additive1Icon;

		// Token: 0x040038AA RID: 14506
		public Image Additive2Icon;

		// Token: 0x040038AB RID: 14507
		public Image Additive3Icon;
	}
}
