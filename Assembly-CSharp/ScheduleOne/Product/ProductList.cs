using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008DA RID: 2266
	[Serializable]
	public class ProductList
	{
		// Token: 0x06003D46 RID: 15686 RVA: 0x00100EEC File Offset: 0x000FF0EC
		public string GetCommaSeperatedString()
		{
			string text = string.Empty;
			foreach (ProductList.Entry entry in this.entries)
			{
				text = text + entry.Quantity.ToString() + "x ";
				text += Registry.GetItem(entry.ProductID).Name;
				if (entry != this.entries[this.entries.Count - 1])
				{
					text += ", ";
				}
			}
			return text;
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x00100F94 File Offset: 0x000FF194
		public string GetLineSeperatedString()
		{
			string text = "\n";
			foreach (ProductList.Entry entry in this.entries)
			{
				text = text + entry.Quantity.ToString() + "x ";
				text += Registry.GetItem(entry.ProductID).Name;
				if (entry != this.entries[this.entries.Count - 1])
				{
					text += "\n";
				}
			}
			return text;
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x0010103C File Offset: 0x000FF23C
		public string GetQualityString()
		{
			ProductList.Entry entry = this.entries[0];
			return string.Concat(new string[]
			{
				"<color=#",
				ColorUtility.ToHtmlStringRGBA(ItemQuality.GetColor(entry.Quality)),
				">",
				entry.Quality.ToString(),
				"</color> "
			});
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x001010A0 File Offset: 0x000FF2A0
		public int GetTotalQuantity()
		{
			int num = 0;
			foreach (ProductList.Entry entry in this.entries)
			{
				num += entry.Quantity;
			}
			return num;
		}

		// Token: 0x04002C46 RID: 11334
		public List<ProductList.Entry> entries = new List<ProductList.Entry>();

		// Token: 0x020008DB RID: 2267
		[Serializable]
		public class Entry
		{
			// Token: 0x04002C47 RID: 11335
			public string ProductID;

			// Token: 0x04002C48 RID: 11336
			public EQuality Quality;

			// Token: 0x04002C49 RID: 11337
			public int Quantity;
		}
	}
}
