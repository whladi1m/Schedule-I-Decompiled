using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Management
{
	// Token: 0x02000581 RID: 1409
	public class ManagementItemFilter
	{
		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x0600233C RID: 9020 RVA: 0x0008FFFD File Offset: 0x0008E1FD
		// (set) Token: 0x0600233D RID: 9021 RVA: 0x00090005 File Offset: 0x0008E205
		public ManagementItemFilter.EMode Mode { get; private set; } = ManagementItemFilter.EMode.Blacklist;

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x0600233E RID: 9022 RVA: 0x0009000E File Offset: 0x0008E20E
		// (set) Token: 0x0600233F RID: 9023 RVA: 0x00090016 File Offset: 0x0008E216
		public List<ItemDefinition> Items { get; private set; } = new List<ItemDefinition>();

		// Token: 0x06002340 RID: 9024 RVA: 0x0009001F File Offset: 0x0008E21F
		public ManagementItemFilter(ManagementItemFilter.EMode mode)
		{
			this.Mode = mode;
			this.Items = new List<ItemDefinition>();
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x0009004B File Offset: 0x0008E24B
		public void SetMode(ManagementItemFilter.EMode mode)
		{
			this.Mode = mode;
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x00090054 File Offset: 0x0008E254
		public void AddItem(ItemDefinition item)
		{
			this.Items.Add(item);
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x00090062 File Offset: 0x0008E262
		public void RemoveItem(ItemDefinition item)
		{
			this.Items.Remove(item);
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x00090071 File Offset: 0x0008E271
		public bool Contains(ItemDefinition item)
		{
			return this.Items.Contains(item);
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x0009007F File Offset: 0x0008E27F
		public bool DoesItemMeetFilter(ItemInstance item)
		{
			if (this.Mode != ManagementItemFilter.EMode.Whitelist)
			{
				return !this.Items.Contains(item.Definition);
			}
			return this.Items.Contains(item.Definition);
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000900B0 File Offset: 0x0008E2B0
		public string GetDescription()
		{
			if (this.Mode == ManagementItemFilter.EMode.Blacklist)
			{
				if (this.Items.Count == 0)
				{
					return "All";
				}
				return this.Items.Count.ToString() + " blacklisted";
			}
			else
			{
				if (this.Items.Count == 0)
				{
					return "None";
				}
				return this.Items.Count.ToString() + " whitelisted";
			}
		}

		// Token: 0x02000582 RID: 1410
		public enum EMode
		{
			// Token: 0x04001A68 RID: 6760
			Whitelist,
			// Token: 0x04001A69 RID: 6761
			Blacklist
		}
	}
}
