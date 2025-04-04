using System;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000916 RID: 2326
	[CreateAssetMenu(fileName = "CashDefinition", menuName = "ScriptableObjects/CashDefinition", order = 1)]
	[Serializable]
	public class CashDefinition : StorableItemDefinition
	{
		// Token: 0x06003F5B RID: 16219 RVA: 0x0010B500 File Offset: 0x00109700
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new CashInstance(this, quantity);
		}
	}
}
