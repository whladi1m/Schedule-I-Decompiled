using System;
using ScheduleOne.Equipping;
using ScheduleOne.UI.Items;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200092E RID: 2350
	[CreateAssetMenu(fileName = "ItemDefinition", menuName = "ScriptableObjects/ItemDefinition", order = 1)]
	[Serializable]
	public class ItemDefinition : ScriptableObject
	{
		// Token: 0x06003F9B RID: 16283 RVA: 0x0010BACC File Offset: 0x00109CCC
		public virtual ItemInstance GetDefaultInstance(int quantity = 1)
		{
			Console.LogError("This should be overridden in the definition class!", null);
			return null;
		}

		// Token: 0x04002DCF RID: 11727
		public const int DEFAULT_STACK_LIMIT = 10;

		// Token: 0x04002DD0 RID: 11728
		public string Name;

		// Token: 0x04002DD1 RID: 11729
		[TextArea(3, 10)]
		public string Description;

		// Token: 0x04002DD2 RID: 11730
		public string ID;

		// Token: 0x04002DD3 RID: 11731
		public Sprite Icon;

		// Token: 0x04002DD4 RID: 11732
		public EItemCategory Category;

		// Token: 0x04002DD5 RID: 11733
		public string[] Keywords;

		// Token: 0x04002DD6 RID: 11734
		public bool AvailableInDemo = true;

		// Token: 0x04002DD7 RID: 11735
		public Color LabelDisplayColor = Color.white;

		// Token: 0x04002DD8 RID: 11736
		public int StackLimit = 10;

		// Token: 0x04002DD9 RID: 11737
		public Equippable Equippable;

		// Token: 0x04002DDA RID: 11738
		public ItemUI CustomItemUI;

		// Token: 0x04002DDB RID: 11739
		public ItemInfoContent CustomInfoContent;

		// Token: 0x04002DDC RID: 11740
		[Header("Legal Status")]
		public ELegalStatus legalStatus;
	}
}
