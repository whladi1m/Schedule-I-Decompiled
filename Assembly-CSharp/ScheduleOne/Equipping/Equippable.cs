using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000902 RID: 2306
	public class Equippable : MonoBehaviour
	{
		// Token: 0x06003E7B RID: 15995 RVA: 0x00107BEA File Offset: 0x00105DEA
		public virtual void Equip(ItemInstance item)
		{
			this.itemInstance = item;
			PlayerSingleton<PlayerInventory>.Instance.SetEquippable(this);
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x00107BFE File Offset: 0x00105DFE
		public virtual void Unequip()
		{
			PlayerSingleton<PlayerInventory>.Instance.SetEquippable(null);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04002CFB RID: 11515
		protected ItemInstance itemInstance;

		// Token: 0x04002CFC RID: 11516
		public bool CanInteractWhenEquipped = true;

		// Token: 0x04002CFD RID: 11517
		public bool CanPickUpWhenEquipped = true;
	}
}
