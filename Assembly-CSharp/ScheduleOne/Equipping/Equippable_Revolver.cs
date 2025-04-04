using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x0200090E RID: 2318
	public class Equippable_Revolver : Equippable_RangedWeapon
	{
		// Token: 0x06003ED7 RID: 16087 RVA: 0x001096DB File Offset: 0x001078DB
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			this.SetDisplayedBullets(this.weaponItem.Value);
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x001096F5 File Offset: 0x001078F5
		public override void Fire()
		{
			base.Fire();
			this.SetDisplayedBullets(this.weaponItem.Value);
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x0010970E File Offset: 0x0010790E
		public override void Reload()
		{
			base.Reload();
			this.SetDisplayedBullets(this.weaponItem.Value);
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x00109727 File Offset: 0x00107927
		protected override void NotifyIncrementalReload()
		{
			base.NotifyIncrementalReload();
			this.SetDisplayedBullets(this.weaponItem.Value);
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x00109740 File Offset: 0x00107940
		private void SetDisplayedBullets(int count)
		{
			for (int i = 0; i < this.Bullets.Length; i++)
			{
				this.Bullets[i].gameObject.SetActive(i < count);
			}
		}

		// Token: 0x04002D5E RID: 11614
		public Transform[] Bullets;
	}
}
