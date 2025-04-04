using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008A3 RID: 2211
	public class CookableModule : ItemModule
	{
		// Token: 0x04002B73 RID: 11123
		[Header("Cook Settings")]
		public int CookTime = 360;

		// Token: 0x04002B74 RID: 11124
		public CookableModule.ECookableType CookType;

		// Token: 0x04002B75 RID: 11125
		[Header("Product Settings")]
		public StorableItemDefinition Product;

		// Token: 0x04002B76 RID: 11126
		public int ProductQuantity = 1;

		// Token: 0x04002B77 RID: 11127
		public Rigidbody ProductShardPrefab;

		// Token: 0x04002B78 RID: 11128
		[Header("Appearance")]
		public Color LiquidColor;

		// Token: 0x04002B79 RID: 11129
		public Color SolidColor;

		// Token: 0x020008A4 RID: 2212
		public enum ECookableType
		{
			// Token: 0x04002B7B RID: 11131
			Liquid,
			// Token: 0x04002B7C RID: 11132
			Solid
		}
	}
}
