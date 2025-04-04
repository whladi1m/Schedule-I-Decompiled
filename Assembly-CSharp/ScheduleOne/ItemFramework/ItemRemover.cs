using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000936 RID: 2358
	public class ItemRemover : MonoBehaviour
	{
		// Token: 0x06003FCF RID: 16335 RVA: 0x0010C2A8 File Offset: 0x0010A4A8
		public void Remove()
		{
			PlayerSingleton<PlayerInventory>.Instance.RemoveAmountOfItem(this.Item.ID, (uint)this.Quantity);
		}

		// Token: 0x04002E01 RID: 11777
		public ItemDefinition Item;

		// Token: 0x04002E02 RID: 11778
		public int Quantity;
	}
}
