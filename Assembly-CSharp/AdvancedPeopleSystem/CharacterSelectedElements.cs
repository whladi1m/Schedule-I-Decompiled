using System;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000219 RID: 537
	[Serializable]
	public class CharacterSelectedElements : ICloneable
	{
		// Token: 0x06000B81 RID: 2945 RVA: 0x00035748 File Offset: 0x00033948
		public object Clone()
		{
			return new CharacterSelectedElements
			{
				Hair = this.Hair,
				Beard = this.Beard,
				Hat = this.Hat,
				Shirt = this.Shirt,
				Pants = this.Pants,
				Shoes = this.Shoes,
				Accessory = this.Accessory,
				Item1 = this.Item1
			};
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x000357BC File Offset: 0x000339BC
		public int GetSelectedIndex(CharacterElementType type)
		{
			switch (type)
			{
			case CharacterElementType.Hat:
				return this.Hat;
			case CharacterElementType.Shirt:
				return this.Shirt;
			case CharacterElementType.Pants:
				return this.Pants;
			case CharacterElementType.Shoes:
				return this.Shoes;
			case CharacterElementType.Accessory:
				return this.Accessory;
			case CharacterElementType.Hair:
				return this.Hair;
			case CharacterElementType.Beard:
				return this.Beard;
			case CharacterElementType.Item1:
				return this.Item1;
			default:
				return -1;
			}
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0003582C File Offset: 0x00033A2C
		public void SetSelectedIndex(CharacterElementType type, int newIndex)
		{
			switch (type)
			{
			case CharacterElementType.Hat:
				this.Hat = newIndex;
				return;
			case CharacterElementType.Shirt:
				this.Shirt = newIndex;
				return;
			case CharacterElementType.Pants:
				this.Pants = newIndex;
				return;
			case CharacterElementType.Shoes:
				this.Shoes = newIndex;
				return;
			case CharacterElementType.Accessory:
				this.Accessory = newIndex;
				return;
			case CharacterElementType.Hair:
				this.Hair = newIndex;
				return;
			case CharacterElementType.Beard:
				this.Beard = newIndex;
				return;
			case CharacterElementType.Item1:
				this.Item1 = newIndex;
				return;
			default:
				return;
			}
		}

		// Token: 0x04000CA1 RID: 3233
		public int Hair = -1;

		// Token: 0x04000CA2 RID: 3234
		public int Beard = -1;

		// Token: 0x04000CA3 RID: 3235
		public int Hat = -1;

		// Token: 0x04000CA4 RID: 3236
		public int Shirt = -1;

		// Token: 0x04000CA5 RID: 3237
		public int Pants = -1;

		// Token: 0x04000CA6 RID: 3238
		public int Shoes = -1;

		// Token: 0x04000CA7 RID: 3239
		public int Accessory = -1;

		// Token: 0x04000CA8 RID: 3240
		public int Item1 = -1;
	}
}
