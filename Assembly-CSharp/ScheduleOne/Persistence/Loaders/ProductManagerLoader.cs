using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000391 RID: 913
	public class ProductManagerLoader : Loader
	{
		// Token: 0x0600148E RID: 5262 RVA: 0x0005B894 File Offset: 0x00059A94
		public override void Load(string mainPath)
		{
			string path = Path.Combine(mainPath, "CreatedProducts");
			if (Directory.Exists(path))
			{
				WeedProductLoader weedProductLoader = new WeedProductLoader();
				MethProductLoader methProductLoader = new MethProductLoader();
				CocaineProductLoader cocaineProductLoader = new CocaineProductLoader();
				string[] files = Directory.GetFiles(path);
				for (int i = 0; i < files.Length; i++)
				{
					string json;
					if (base.TryLoadFile(files[i], out json, false))
					{
						ProductData productData = null;
						try
						{
							productData = JsonUtility.FromJson<ProductData>(json);
						}
						catch (Exception ex)
						{
							Debug.LogError("Error loading product data: " + ex.Message);
						}
						if (productData != null)
						{
							bool flag = false;
							if (string.IsNullOrEmpty(productData.Name))
							{
								Console.LogWarning("Product name is empty; generating random name", null);
								if (Singleton<NewMixScreen>.InstanceExists)
								{
									productData.Name = Singleton<NewMixScreen>.Instance.GenerateUniqueName(null, EDrugType.Marijuana);
								}
								else
								{
									productData.Name = "Product " + UnityEngine.Random.Range(0, 1000).ToString();
								}
								flag = true;
							}
							if (string.IsNullOrEmpty(productData.ID))
							{
								Console.LogWarning("Product ID is empty; generating from name", null);
								productData.ID = ProductManager.MakeIDFileSafe(productData.Name);
								flag = true;
							}
							if (flag)
							{
								try
								{
									File.WriteAllText(files[i], productData.GetJson(true));
								}
								catch (Exception ex2)
								{
									Console.LogError("Error saving modified product data: " + ex2.Message, null);
								}
							}
							switch (productData.DrugType)
							{
							case EDrugType.Marijuana:
								weedProductLoader.Load(files[i]);
								break;
							case EDrugType.Methamphetamine:
								methProductLoader.Load(files[i]);
								break;
							case EDrugType.Cocaine:
								cocaineProductLoader.Load(files[i]);
								break;
							default:
								Console.LogError("Unknown drug type: " + productData.DrugType.ToString(), null);
								break;
							}
						}
					}
				}
			}
			string json2;
			if (base.TryLoadFile(Path.Combine(mainPath, "Products"), out json2, true))
			{
				ProductManagerData productManagerData = JsonUtility.FromJson<ProductManagerData>(json2);
				if (productManagerData != null)
				{
					for (int j = 0; j < productManagerData.DiscoveredProducts.Length; j++)
					{
						NetworkSingleton<ProductManager>.Instance.SetProductDiscovered(null, productManagerData.DiscoveredProducts[j], false);
					}
					for (int k = 0; k < productManagerData.ListedProducts.Length; k++)
					{
						NetworkSingleton<ProductManager>.Instance.SetProductListed(null, productManagerData.ListedProducts[k], true);
					}
					if (productManagerData.FavouritedProducts != null)
					{
						for (int l = 0; l < productManagerData.FavouritedProducts.Length; l++)
						{
							NetworkSingleton<ProductManager>.Instance.SetProductFavourited(null, productManagerData.FavouritedProducts[l], true);
						}
					}
					if (productManagerData.ActiveMixOperation != null && productManagerData.ActiveMixOperation.ProductID != string.Empty)
					{
						NetworkSingleton<ProductManager>.Instance.SendMixOperation(productManagerData.ActiveMixOperation, productManagerData.IsMixComplete);
					}
					for (int m = 0; m < productManagerData.MixRecipes.Length; m++)
					{
						MixRecipeData mixRecipeData = productManagerData.MixRecipes[m];
						NetworkSingleton<ProductManager>.Instance.CreateMixRecipe(null, mixRecipeData.Product, mixRecipeData.Mixer, mixRecipeData.Output);
					}
					if (productManagerData.ProductPrices != null)
					{
						for (int n = 0; n < productManagerData.ProductPrices.Length; n++)
						{
							StringIntPair stringIntPair = productManagerData.ProductPrices[n];
							ProductDefinition item = Registry.GetItem<ProductDefinition>(stringIntPair.String);
							if (item != null)
							{
								NetworkSingleton<ProductManager>.Instance.SetPrice(null, item.ID, (float)stringIntPair.Int);
							}
						}
					}
				}
			}
		}
	}
}
