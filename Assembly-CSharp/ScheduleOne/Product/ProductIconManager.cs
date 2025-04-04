using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Product.Packaging;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008D4 RID: 2260
	public class ProductIconManager : Singleton<ProductIconManager>
	{
		// Token: 0x06003D1F RID: 15647 RVA: 0x0010061C File Offset: 0x000FE81C
		public Sprite GetIcon(string productID, string packagingID, bool ignoreError = false)
		{
			ProductIconManager.ProductIcon productIcon = this.icons.Find((ProductIconManager.ProductIcon x) => x.ProductID == productID && x.PackagingID == packagingID);
			if (productIcon == null)
			{
				if (!ignoreError)
				{
					Console.LogError(string.Concat(new string[]
					{
						"Failed to find icon for packaging (",
						packagingID,
						") containing product (",
						productID,
						")"
					}), null);
				}
				return null;
			}
			return productIcon.Icon;
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x001006A0 File Offset: 0x000FE8A0
		public Sprite GenerateIcons(string productID)
		{
			if (Registry.GetItem(productID) == null)
			{
				Console.LogError("Failed to find product with ID: " + productID, null);
				return null;
			}
			if (this.icons.Any((ProductIconManager.ProductIcon x) => x.ProductID == productID) && Registry.GetItem(productID) != null)
			{
				return Registry.GetItem(productID).Icon;
			}
			for (int i = 0; i < this.Packaging.Length; i++)
			{
				Texture2D texture2D = this.GenerateProductTexture(productID, this.Packaging[i].ID);
				if (texture2D == null)
				{
					Console.LogError(string.Concat(new string[]
					{
						"Failed to generate icon for packaging (",
						this.Packaging[i].ID,
						") containing product (",
						productID,
						")"
					}), null);
				}
				else
				{
					ProductIconManager.ProductIcon productIcon = new ProductIconManager.ProductIcon();
					productIcon.ProductID = productID;
					productIcon.PackagingID = this.Packaging[i].ID;
					texture2D.Apply();
					productIcon.Icon = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
					this.icons.Add(productIcon);
				}
			}
			Texture2D texture2D2 = this.GenerateProductTexture(productID, "none");
			texture2D2.Apply();
			return Sprite.Create(texture2D2, new Rect(0f, 0f, (float)texture2D2.width, (float)texture2D2.height), new Vector2(0.5f, 0.5f));
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x00100860 File Offset: 0x000FEA60
		private Texture2D GenerateProductTexture(string productID, string packagingID)
		{
			return this.IconGenerator.GeneratePackagingIcon(packagingID, productID);
		}

		// Token: 0x04002C34 RID: 11316
		[SerializeField]
		private List<ProductIconManager.ProductIcon> icons = new List<ProductIconManager.ProductIcon>();

		// Token: 0x04002C35 RID: 11317
		[Header("Product and packaging")]
		public IconGenerator IconGenerator;

		// Token: 0x04002C36 RID: 11318
		public string IconContainerPath = "ProductIcons";

		// Token: 0x04002C37 RID: 11319
		public ProductDefinition[] Products;

		// Token: 0x04002C38 RID: 11320
		public PackagingDefinition[] Packaging;

		// Token: 0x020008D5 RID: 2261
		[Serializable]
		public class ProductIcon
		{
			// Token: 0x04002C39 RID: 11321
			public string ProductID;

			// Token: 0x04002C3A RID: 11322
			public string PackagingID;

			// Token: 0x04002C3B RID: 11323
			public Sprite Icon;
		}
	}
}
