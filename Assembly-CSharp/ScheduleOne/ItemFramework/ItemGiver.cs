using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200092F RID: 2351
	public class ItemGiver : MonoBehaviour
	{
		// Token: 0x06003F9D RID: 16285 RVA: 0x0010BAFC File Offset: 0x00109CFC
		public void Give()
		{
			PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.Item.GetDefaultInstance(this.Quantity));
		}

		// Token: 0x04002DDD RID: 11741
		public ItemDefinition Item;

		// Token: 0x04002DDE RID: 11742
		public int Quantity;
	}
}
