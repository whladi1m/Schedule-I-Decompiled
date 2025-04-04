using System;
using EasyButtons;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Equipping;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200083B RID: 2107
	public class EquipUtility : MonoBehaviour
	{
		// Token: 0x060039C6 RID: 14790 RVA: 0x000F410B File Offset: 0x000F230B
		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				this.Equip();
			}
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x000F411C File Offset: 0x000F231C
		[Button]
		public void Equip()
		{
			base.GetComponent<ScheduleOne.AvatarFramework.Avatar>().SetEquippable(this.Equippable.AssetPath);
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x000F4135 File Offset: 0x000F2335
		[Button]
		public void Unequip()
		{
			base.GetComponent<ScheduleOne.AvatarFramework.Avatar>().SetEquippable(string.Empty);
		}

		// Token: 0x040029BF RID: 10687
		public AvatarEquippable Equippable;
	}
}
