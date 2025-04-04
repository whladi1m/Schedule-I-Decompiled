using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Properties;
using ScheduleOne.Properties.MixMaps;
using ScheduleOne.StationFramework;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Product
{
	// Token: 0x020008DC RID: 2268
	public class ProductManager : NetworkSingleton<ProductManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06003D4B RID: 15691 RVA: 0x001010F8 File Offset: 0x000FF2F8
		public static bool MethDiscovered
		{
			get
			{
				return ProductManager.DiscoveredProducts.Any((ProductDefinition p) => p.ID == "meth");
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06003D4C RID: 15692 RVA: 0x00101123 File Offset: 0x000FF323
		public static bool CocaineDiscovered
		{
			get
			{
				return ProductManager.DiscoveredProducts.Any((ProductDefinition p) => p.ID == "cocaine");
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06003D4D RID: 15693 RVA: 0x0010114E File Offset: 0x000FF34E
		// (set) Token: 0x06003D4E RID: 15694 RVA: 0x00101155 File Offset: 0x000FF355
		public static bool IsAcceptingOrders { get; private set; } = true;

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06003D4F RID: 15695 RVA: 0x0010115D File Offset: 0x000FF35D
		// (set) Token: 0x06003D50 RID: 15696 RVA: 0x00101165 File Offset: 0x000FF365
		public NewMixOperation CurrentMixOperation { get; private set; }

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06003D51 RID: 15697 RVA: 0x0010116E File Offset: 0x000FF36E
		public bool IsMixingInProgress
		{
			get
			{
				return this.CurrentMixOperation != null;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06003D52 RID: 15698 RVA: 0x00101179 File Offset: 0x000FF379
		// (set) Token: 0x06003D53 RID: 15699 RVA: 0x00101181 File Offset: 0x000FF381
		public bool IsMixComplete { get; private set; }

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06003D54 RID: 15700 RVA: 0x0010118A File Offset: 0x000FF38A
		// (set) Token: 0x06003D55 RID: 15701 RVA: 0x00101192 File Offset: 0x000FF392
		public float TimeSinceProductListingChanged { get; private set; }

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06003D56 RID: 15702 RVA: 0x0010119B File Offset: 0x000FF39B
		public string SaveFolderName
		{
			get
			{
				return "Products";
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06003D57 RID: 15703 RVA: 0x0010119B File Offset: 0x000FF39B
		public string SaveFileName
		{
			get
			{
				return "Products";
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06003D58 RID: 15704 RVA: 0x001011A2 File Offset: 0x000FF3A2
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06003D59 RID: 15705 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06003D5A RID: 15706 RVA: 0x001011AA File Offset: 0x000FF3AA
		// (set) Token: 0x06003D5B RID: 15707 RVA: 0x001011B2 File Offset: 0x000FF3B2
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06003D5C RID: 15708 RVA: 0x001011BB File Offset: 0x000FF3BB
		// (set) Token: 0x06003D5D RID: 15709 RVA: 0x001011C3 File Offset: 0x000FF3C3
		public List<string> LocalExtraFolders { get; set; } = new List<string>
		{
			"CreatedProducts"
		};

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06003D5E RID: 15710 RVA: 0x001011CC File Offset: 0x000FF3CC
		// (set) Token: 0x06003D5F RID: 15711 RVA: 0x001011D4 File Offset: 0x000FF3D4
		public bool HasChanged { get; set; }

		// Token: 0x06003D60 RID: 15712 RVA: 0x001011DD File Offset: 0x000FF3DD
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Product.ProductManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x001011F4 File Offset: 0x000FF3F4
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(new UnityAction(this.Clean));
			foreach (ProductDefinition productDefinition in this.DefaultKnownProducts)
			{
				productDefinition.OnValidate();
				if (this.highestValueProduct == null || productDefinition.MarketValue > this.highestValueProduct.MarketValue)
				{
					this.highestValueProduct = productDefinition;
				}
			}
			foreach (ProductDefinition productDefinition2 in this.AllProducts)
			{
				if (!this.ProductNames.Contains(productDefinition2.Name))
				{
					this.ProductNames.Add(productDefinition2.Name);
				}
				if (!this.ProductPrices.ContainsKey(productDefinition2))
				{
					this.ProductPrices.Add(productDefinition2, productDefinition2.MarketValue);
				}
			}
			NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepEnd.AddListener(new UnityAction(this.OnNewDay));
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.OnMinPass));
			foreach (PropertyItemDefinition propertyItemDefinition in this.ValidMixIngredients)
			{
				for (int i = 0; i < propertyItemDefinition.Properties.Count; i++)
				{
					if (!Singleton<PropertyUtility>.Instance.AllProperties.Contains(propertyItemDefinition.Properties[i]))
					{
						string[] array = new string[5];
						array[0] = "Mixer ";
						array[1] = propertyItemDefinition.Name;
						array[2] = " has property ";
						int num = 3;
						Property property = propertyItemDefinition.Properties[i];
						array[num] = ((property != null) ? property.ToString() : null);
						array[4] = " that is not in the valid properties list";
						Console.LogError(string.Concat(array), null);
					}
				}
			}
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x00101420 File Offset: 0x000FF620
		public override void OnStartServer()
		{
			base.OnStartServer();
			for (int i = 0; i < this.DefaultKnownProducts.Count; i++)
			{
				this.SetProductDiscovered(null, this.DefaultKnownProducts[i].ID, false);
			}
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x00101462 File Offset: 0x000FF662
		public override void OnStartClient()
		{
			base.OnStartClient();
			this.RefreshHighestValueProduct();
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x00101470 File Offset: 0x000FF670
		private void Update()
		{
			this.TimeSinceProductListingChanged += Time.deltaTime;
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x00101484 File Offset: 0x000FF684
		private void Clean()
		{
			ProductManager.DiscoveredProducts.Clear();
			ProductManager.ListedProducts.Clear();
			ProductManager.FavouritedProducts.Clear();
			ProductManager.IsAcceptingOrders = true;
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x001014AA File Offset: 0x000FF6AA
		[ServerRpc(RequireOwnership = false)]
		public void SetMethDiscovered()
		{
			this.RpcWriter___Server_SetMethDiscovered_2166136261();
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x001014B2 File Offset: 0x000FF6B2
		[ServerRpc(RequireOwnership = false)]
		public void SetCocaineDiscovered()
		{
			this.RpcWriter___Server_SetCocaineDiscovered_2166136261();
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x001014BC File Offset: 0x000FF6BC
		public MixerMap GetMixerMap(EDrugType type)
		{
			switch (type)
			{
			case EDrugType.Marijuana:
				return this.WeedMixMap;
			case EDrugType.Methamphetamine:
				return this.MethMixMap;
			case EDrugType.Cocaine:
				return this.CokeMixMap;
			default:
				Console.LogError("No mixer map found for " + type.ToString(), null);
				return null;
			}
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x00101510 File Offset: 0x000FF710
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			foreach (ProductDefinition productDefinition in this.createdProducts)
			{
				if (productDefinition is WeedDefinition)
				{
					WeedDefinition weedDefinition = productDefinition as WeedDefinition;
					WeedAppearanceSettings appearance = new WeedAppearanceSettings(weedDefinition.MainMat.color, weedDefinition.SecondaryMat.color, weedDefinition.LeafMat.color, weedDefinition.StemMat.color);
					List<string> list = new List<string>();
					foreach (Property property in weedDefinition.Properties)
					{
						list.Add(property.ID);
					}
					this.CreateWeed(connection, productDefinition.Name, productDefinition.ID, EDrugType.Marijuana, list, appearance);
				}
				else if (productDefinition is MethDefinition)
				{
					MethDefinition methDefinition = productDefinition as MethDefinition;
					MethAppearanceSettings appearanceSettings = methDefinition.AppearanceSettings;
					List<string> list2 = new List<string>();
					foreach (Property property2 in methDefinition.Properties)
					{
						list2.Add(property2.ID);
					}
					this.CreateMeth(connection, productDefinition.Name, productDefinition.ID, EDrugType.Methamphetamine, list2, appearanceSettings);
				}
				else if (productDefinition is CocaineDefinition)
				{
					CocaineDefinition cocaineDefinition = productDefinition as CocaineDefinition;
					CocaineAppearanceSettings appearanceSettings2 = cocaineDefinition.AppearanceSettings;
					List<string> list3 = new List<string>();
					foreach (Property property3 in cocaineDefinition.Properties)
					{
						list3.Add(property3.ID);
					}
					this.CreateCocaine(connection, productDefinition.Name, productDefinition.ID, EDrugType.Cocaine, list3, appearanceSettings2);
				}
			}
			for (int i = 0; i < this.mixRecipes.Count; i++)
			{
				this.CreateMixRecipe(null, this.mixRecipes[i].Ingredients[1].Items[0].ID, this.mixRecipes[i].Ingredients[0].Items[0].ID, this.mixRecipes[i].Product.Item.ID);
			}
			for (int j = 0; j < ProductManager.DiscoveredProducts.Count; j++)
			{
				this.SetProductDiscovered(connection, ProductManager.DiscoveredProducts[j].ID, false);
			}
			for (int k = 0; k < ProductManager.ListedProducts.Count; k++)
			{
				this.SetProductListed(connection, ProductManager.ListedProducts[k].ID, true);
			}
			for (int l = 0; l < ProductManager.FavouritedProducts.Count; l++)
			{
				this.SetProductFavourited(connection, ProductManager.FavouritedProducts[l].ID, true);
			}
			foreach (KeyValuePair<ProductDefinition, float> keyValuePair in this.ProductPrices)
			{
				this.SetPrice(connection, keyValuePair.Key.ID, keyValuePair.Value);
			}
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x001018E8 File Offset: 0x000FFAE8
		private void OnMinPass()
		{
			if (!NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				return;
			}
			if (GameManager.IS_TUTORIAL)
			{
				return;
			}
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("SecondUniqueProductDiscovered"))
			{
				float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Inventory_OGKush");
				if (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Inventory_Weed_Count") > value)
				{
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("SecondUniqueProductDiscovered", true.ToString(), true);
					if (this.onSecondUniqueProductCreated != null)
					{
						this.onSecondUniqueProductCreated.Invoke();
					}
				}
			}
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x00101964 File Offset: 0x000FFB64
		private void OnNewDay()
		{
			if (InstanceFinder.IsServer && this.CurrentMixOperation != null && !this.IsMixComplete)
			{
				this.SetMixOperation(this.CurrentMixOperation, true);
			}
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x0010198A File Offset: 0x000FFB8A
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetProductListed(string productID, bool listed)
		{
			this.RpcWriter___Server_SetProductListed_310431262(productID, listed);
			this.RpcLogic___SetProductListed_310431262(productID, listed);
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x001019A8 File Offset: 0x000FFBA8
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetProductListed(NetworkConnection conn, string productID, bool listed)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetProductListed_619441887(conn, productID, listed);
				this.RpcLogic___SetProductListed_619441887(conn, productID, listed);
			}
			else
			{
				this.RpcWriter___Target_SetProductListed_619441887(conn, productID, listed);
			}
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x001019F5 File Offset: 0x000FFBF5
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetProductFavourited(string productID, bool listed)
		{
			this.RpcWriter___Server_SetProductFavourited_310431262(productID, listed);
			this.RpcLogic___SetProductFavourited_310431262(productID, listed);
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x00101A14 File Offset: 0x000FFC14
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetProductFavourited(NetworkConnection conn, string productID, bool fav)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetProductFavourited_619441887(conn, productID, fav);
				this.RpcLogic___SetProductFavourited_619441887(conn, productID, fav);
			}
			else
			{
				this.RpcWriter___Target_SetProductFavourited_619441887(conn, productID, fav);
			}
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x00101A61 File Offset: 0x000FFC61
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void DiscoverProduct(string productID)
		{
			this.RpcWriter___Server_DiscoverProduct_3615296227(productID);
			this.RpcLogic___DiscoverProduct_3615296227(productID);
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x00101A78 File Offset: 0x000FFC78
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetProductDiscovered(NetworkConnection conn, string productID, bool autoList)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetProductDiscovered_619441887(conn, productID, autoList);
				this.RpcLogic___SetProductDiscovered_619441887(conn, productID, autoList);
			}
			else
			{
				this.RpcWriter___Target_SetProductDiscovered_619441887(conn, productID, autoList);
			}
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x00101AC5 File Offset: 0x000FFCC5
		public void SetIsAcceptingOrder(bool accepting)
		{
			ProductManager.IsAcceptingOrders = accepting;
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x00101ACD File Offset: 0x000FFCCD
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void CreateWeed_Server(string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			this.RpcWriter___Server_CreateWeed_Server_2331775230(name, id, type, properties, appearance);
			this.RpcLogic___CreateWeed_Server_2331775230(name, id, type, properties, appearance);
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x00101B04 File Offset: 0x000FFD04
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		private void CreateWeed(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateWeed_1777266891(conn, name, id, type, properties, appearance);
				this.RpcLogic___CreateWeed_1777266891(conn, name, id, type, properties, appearance);
			}
			else
			{
				this.RpcWriter___Target_CreateWeed_1777266891(conn, name, id, type, properties, appearance);
			}
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x00101B75 File Offset: 0x000FFD75
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void CreateCocaine_Server(string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			this.RpcWriter___Server_CreateCocaine_Server_891166717(name, id, type, properties, appearance);
			this.RpcLogic___CreateCocaine_Server_891166717(name, id, type, properties, appearance);
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x00101BAC File Offset: 0x000FFDAC
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		private void CreateCocaine(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateCocaine_1327282946(conn, name, id, type, properties, appearance);
				this.RpcLogic___CreateCocaine_1327282946(conn, name, id, type, properties, appearance);
			}
			else
			{
				this.RpcWriter___Target_CreateCocaine_1327282946(conn, name, id, type, properties, appearance);
			}
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x00101C1D File Offset: 0x000FFE1D
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void CreateMeth_Server(string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			this.RpcWriter___Server_CreateMeth_Server_4251728555(name, id, type, properties, appearance);
			this.RpcLogic___CreateMeth_Server_4251728555(name, id, type, properties, appearance);
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x00101C54 File Offset: 0x000FFE54
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		private void CreateMeth(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateMeth_1869045686(conn, name, id, type, properties, appearance);
				this.RpcLogic___CreateMeth_1869045686(conn, name, id, type, properties, appearance);
			}
			else
			{
				this.RpcWriter___Target_CreateMeth_1869045686(conn, name, id, type, properties, appearance);
			}
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x00101CC8 File Offset: 0x000FFEC8
		private void RefreshHighestValueProduct()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			for (int i = 0; i < ProductManager.DiscoveredProducts.Count; i++)
			{
				if (this.highestValueProduct == null || ProductManager.DiscoveredProducts[i].MarketValue > this.highestValueProduct.MarketValue)
				{
					this.highestValueProduct = ProductManager.DiscoveredProducts[i];
				}
			}
			float marketValue = this.highestValueProduct.MarketValue;
			if (marketValue >= 100f)
			{
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.MASTER_CHEF);
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("HighestValueProduct", marketValue.ToString(), true);
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x00101D64 File Offset: 0x000FFF64
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendMixRecipe(string product, string mixer, string output)
		{
			this.RpcWriter___Server_SendMixRecipe_852232071(product, mixer, output);
			this.RpcLogic___SendMixRecipe_852232071(product, mixer, output);
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x00101D8C File Offset: 0x000FFF8C
		[TargetRpc]
		[ObserversRpc(RunLocally = true)]
		public void CreateMixRecipe(NetworkConnection conn, string product, string mixer, string output)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateMixRecipe_1410895574(conn, product, mixer, output);
				this.RpcLogic___CreateMixRecipe_1410895574(conn, product, mixer, output);
			}
			else
			{
				this.RpcWriter___Target_CreateMixRecipe_1410895574(conn, product, mixer, output);
			}
		}

		// Token: 0x06003D7D RID: 15741 RVA: 0x00101DE8 File Offset: 0x000FFFE8
		public StationRecipe GetRecipe(string product, string mixer)
		{
			return this.mixRecipes.Find((StationRecipe r) => r.Product.Item.ID == product && r.Ingredients[0].Items[0].ID == mixer);
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x00101E20 File Offset: 0x00100020
		public StationRecipe GetRecipe(List<Property> productProperties, Property mixerProperty)
		{
			Console.Log("Trying to find recipe with product properties: " + string.Join<Property>(", ", productProperties) + " and mixer property: " + mixerProperty.Name, null);
			foreach (StationRecipe stationRecipe in this.mixRecipes)
			{
				if (!(stationRecipe == null) && stationRecipe.Ingredients.Count >= 2)
				{
					ItemDefinition item = stationRecipe.Ingredients[0].Item;
					ItemDefinition item2 = stationRecipe.Ingredients[1].Item;
					if (!(item == null) && !(item2 == null))
					{
						PropertyItemDefinition propertyItemDefinition = item as PropertyItemDefinition;
						List<Property> list = (propertyItemDefinition != null) ? propertyItemDefinition.Properties : null;
						PropertyItemDefinition propertyItemDefinition2 = item2 as PropertyItemDefinition;
						List<Property> list2 = (propertyItemDefinition2 != null) ? propertyItemDefinition2.Properties : null;
						if (item2 is ProductDefinition)
						{
							PropertyItemDefinition propertyItemDefinition3 = item2 as PropertyItemDefinition;
							list = ((propertyItemDefinition3 != null) ? propertyItemDefinition3.Properties : null);
							PropertyItemDefinition propertyItemDefinition4 = item as PropertyItemDefinition;
							list2 = ((propertyItemDefinition4 != null) ? propertyItemDefinition4.Properties : null);
						}
						if (list.Count == productProperties.Count && list2.Count == 1)
						{
							bool flag = true;
							for (int i = 0; i < productProperties.Count; i++)
							{
								if (!list.Contains(productProperties[i]))
								{
									flag = false;
									break;
								}
							}
							if (flag && !(list2[0] != mixerProperty))
							{
								return stationRecipe;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x00101FBC File Offset: 0x001001BC
		[TargetRpc]
		private void GiveItem(NetworkConnection conn, string id)
		{
			this.RpcWriter___Target_GiveItem_2971853958(conn, id);
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x00101FCC File Offset: 0x001001CC
		public ProductDefinition GetKnownProduct(EDrugType type, List<Property> properties)
		{
			foreach (ProductDefinition productDefinition in this.AllProducts)
			{
				if (productDefinition.DrugTypes[0].DrugType == type && productDefinition.Properties.Count == properties.Count)
				{
					int num = 0;
					while (num < properties.Count && productDefinition.Properties.Contains(properties[num]))
					{
						if (num == properties.Count - 1)
						{
							return productDefinition;
						}
						num++;
					}
				}
			}
			return null;
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x00102078 File Offset: 0x00100278
		public float GetPrice(ProductDefinition product)
		{
			if (product == null)
			{
				Console.LogError("Product is null", null);
				return 1f;
			}
			if (this.ProductPrices.ContainsKey(product))
			{
				return Mathf.Clamp(this.ProductPrices[product], 1f, 999f);
			}
			Console.LogError("Price not found for product: " + product.ID + ". Returning market value", null);
			return Mathf.Clamp(product.MarketValue, 1f, 999f);
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x001020F9 File Offset: 0x001002F9
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPrice(string productID, float value)
		{
			this.RpcWriter___Server_SendPrice_606697822(productID, value);
			this.RpcLogic___SendPrice_606697822(productID, value);
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x00102118 File Offset: 0x00100318
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetPrice(NetworkConnection conn, string productID, float value)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetPrice_4077118173(conn, productID, value);
				this.RpcLogic___SetPrice_4077118173(conn, productID, value);
			}
			else
			{
				this.RpcWriter___Target_SetPrice_4077118173(conn, productID, value);
			}
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x00102165 File Offset: 0x00100365
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendMixOperation(NewMixOperation operation, bool complete)
		{
			this.RpcWriter___Server_SendMixOperation_3670976965(operation, complete);
			this.RpcLogic___SendMixOperation_3670976965(operation, complete);
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x00102183 File Offset: 0x00100383
		[ObserversRpc(RunLocally = true)]
		private void SetMixOperation(NewMixOperation operation, bool complete)
		{
			this.RpcWriter___Observers_SetMixOperation_3670976965(operation, complete);
			this.RpcLogic___SetMixOperation_3670976965(operation, complete);
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x001021A4 File Offset: 0x001003A4
		public string FinishAndNameMix(string productID, string ingredientID, string mixName)
		{
			if (!ProductManager.IsMixNameValid(mixName))
			{
				Console.LogError("Invalid mix name. Using random name", null);
				mixName = Singleton<NewMixScreen>.Instance.GenerateUniqueName(null, EDrugType.Marijuana);
			}
			string text = mixName.ToLower().Replace(" ", string.Empty);
			text = ProductManager.MakeIDFileSafe(text);
			text = text.Replace(" ", string.Empty);
			text = text.Replace("(", string.Empty);
			text = text.Replace(")", string.Empty);
			text = text.Replace("'", string.Empty);
			text = text.Replace("\"", string.Empty);
			text = text.Replace(":", string.Empty);
			text = text.Replace(";", string.Empty);
			text = text.Replace(",", string.Empty);
			text = text.Replace(".", string.Empty);
			text = text.Replace("!", string.Empty);
			text = text.Replace("?", string.Empty);
			this.FinishAndNameMix(productID, ingredientID, mixName, text);
			if (!InstanceFinder.IsServer)
			{
				this.SendFinishAndNameMix(productID, ingredientID, mixName, text);
			}
			return text;
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x001022C8 File Offset: 0x001004C8
		public static string MakeIDFileSafe(string id)
		{
			id = id.ToLower();
			id = id.Replace(" ", string.Empty);
			id = id.Replace("(", string.Empty);
			id = id.Replace(")", string.Empty);
			id = id.Replace("'", string.Empty);
			id = id.Replace("\"", string.Empty);
			id = id.Replace(":", string.Empty);
			id = id.Replace(";", string.Empty);
			id = id.Replace(",", string.Empty);
			id = id.Replace(".", string.Empty);
			id = id.Replace("!", string.Empty);
			id = id.Replace("?", string.Empty);
			return id;
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x001023A4 File Offset: 0x001005A4
		public static bool IsMixNameValid(string mixName)
		{
			return !string.IsNullOrEmpty(mixName);
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x001023B4 File Offset: 0x001005B4
		[ObserversRpc(RunLocally = true)]
		private void FinishAndNameMix(string productID, string ingredientID, string mixName, string mixID)
		{
			this.RpcWriter___Observers_FinishAndNameMix_4237212381(productID, ingredientID, mixName, mixID);
			this.RpcLogic___FinishAndNameMix_4237212381(productID, ingredientID, mixName, mixID);
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x001023ED File Offset: 0x001005ED
		[ServerRpc(RequireOwnership = false)]
		private void SendFinishAndNameMix(string productID, string ingredientID, string mixName, string mixID)
		{
			this.RpcWriter___Server_SendFinishAndNameMix_4237212381(productID, ingredientID, mixName, mixID);
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x00102405 File Offset: 0x00100605
		public static float CalculateProductValue(ProductDefinition product, float baseValue)
		{
			return ProductManager.CalculateProductValue(baseValue, product.Properties);
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x00102414 File Offset: 0x00100614
		public static float CalculateProductValue(float baseValue, List<Property> properties)
		{
			float num = baseValue;
			float num2 = 1f;
			for (int i = 0; i < properties.Count; i++)
			{
				num += (float)properties[i].ValueChange;
				num += baseValue * properties[i].AddBaseValueMultiple;
				num2 *= properties[i].ValueMultiplier;
			}
			num *= num2;
			return (float)Mathf.RoundToInt(num);
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x00102478 File Offset: 0x00100678
		public virtual string GetSaveString()
		{
			string[] array = new string[ProductManager.DiscoveredProducts.Count];
			for (int i = 0; i < ProductManager.DiscoveredProducts.Count; i++)
			{
				array[i] = ProductManager.DiscoveredProducts[i].ID;
			}
			string[] array2 = new string[ProductManager.ListedProducts.Count];
			for (int j = 0; j < ProductManager.ListedProducts.Count; j++)
			{
				array2[j] = ProductManager.ListedProducts[j].ID;
			}
			string[] array3 = new string[ProductManager.FavouritedProducts.Count];
			for (int k = 0; k < ProductManager.FavouritedProducts.Count; k++)
			{
				array3[k] = ProductManager.FavouritedProducts[k].ID;
			}
			MixRecipeData[] array4 = new MixRecipeData[this.mixRecipes.Count];
			for (int l = 0; l < this.mixRecipes.Count; l++)
			{
				array4[l] = new MixRecipeData(this.mixRecipes[l].Ingredients[1].Items[0].ID, this.mixRecipes[l].Ingredients[0].Items[0].ID, this.mixRecipes[l].Product.Item.ID);
			}
			StringIntPair[] array5 = new StringIntPair[this.ProductPrices.Count];
			for (int m = 0; m < this.AllProducts.Count; m++)
			{
				array5[m] = new StringIntPair(this.AllProducts[m].ID, Mathf.RoundToInt(this.ProductPrices[this.AllProducts[m]]));
			}
			return new ProductManagerData(array, array2, this.CurrentMixOperation, this.IsMixComplete, array4, array5, array3).GetJson(true);
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x00102664 File Offset: 0x00100864
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> result = new List<string>();
			string parentFolderPath2 = ((ISaveable)this).WriteFolder(parentFolderPath, "CreatedProducts");
			for (int i = 0; i < this.createdProducts.Count; i++)
			{
				new SaveRequest(this.createdProducts[i], parentFolderPath2);
			}
			return result;
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x00102764 File Offset: 0x00100964
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Product.ProductManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Product.ProductManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetMethDiscovered_2166136261));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_SetCocaineDiscovered_2166136261));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SetProductListed_310431262));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetProductListed_619441887));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetProductListed_619441887));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SetProductFavourited_310431262));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_SetProductFavourited_619441887));
			base.RegisterTargetRpc(7U, new ClientRpcDelegate(this.RpcReader___Target_SetProductFavourited_619441887));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_DiscoverProduct_3615296227));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_SetProductDiscovered_619441887));
			base.RegisterTargetRpc(10U, new ClientRpcDelegate(this.RpcReader___Target_SetProductDiscovered_619441887));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_CreateWeed_Server_2331775230));
			base.RegisterTargetRpc(12U, new ClientRpcDelegate(this.RpcReader___Target_CreateWeed_1777266891));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_CreateWeed_1777266891));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_CreateCocaine_Server_891166717));
			base.RegisterTargetRpc(15U, new ClientRpcDelegate(this.RpcReader___Target_CreateCocaine_1327282946));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_CreateCocaine_1327282946));
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_CreateMeth_Server_4251728555));
			base.RegisterTargetRpc(18U, new ClientRpcDelegate(this.RpcReader___Target_CreateMeth_1869045686));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_CreateMeth_1869045686));
			base.RegisterServerRpc(20U, new ServerRpcDelegate(this.RpcReader___Server_SendMixRecipe_852232071));
			base.RegisterTargetRpc(21U, new ClientRpcDelegate(this.RpcReader___Target_CreateMixRecipe_1410895574));
			base.RegisterObserversRpc(22U, new ClientRpcDelegate(this.RpcReader___Observers_CreateMixRecipe_1410895574));
			base.RegisterTargetRpc(23U, new ClientRpcDelegate(this.RpcReader___Target_GiveItem_2971853958));
			base.RegisterServerRpc(24U, new ServerRpcDelegate(this.RpcReader___Server_SendPrice_606697822));
			base.RegisterObserversRpc(25U, new ClientRpcDelegate(this.RpcReader___Observers_SetPrice_4077118173));
			base.RegisterTargetRpc(26U, new ClientRpcDelegate(this.RpcReader___Target_SetPrice_4077118173));
			base.RegisterServerRpc(27U, new ServerRpcDelegate(this.RpcReader___Server_SendMixOperation_3670976965));
			base.RegisterObserversRpc(28U, new ClientRpcDelegate(this.RpcReader___Observers_SetMixOperation_3670976965));
			base.RegisterObserversRpc(29U, new ClientRpcDelegate(this.RpcReader___Observers_FinishAndNameMix_4237212381));
			base.RegisterServerRpc(30U, new ServerRpcDelegate(this.RpcReader___Server_SendFinishAndNameMix_4237212381));
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x00102A51 File Offset: 0x00100C51
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Product.ProductManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Product.ProductManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x00102A6A File Offset: 0x00100C6A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x00102A78 File Offset: 0x00100C78
		private void RpcWriter___Server_SetMethDiscovered_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x00102B12 File Offset: 0x00100D12
		public void RpcLogic___SetMethDiscovered_2166136261()
		{
			this.SetProductDiscovered(null, "meth", false);
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x00102B24 File Offset: 0x00100D24
		private void RpcReader___Server_SetMethDiscovered_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetMethDiscovered_2166136261();
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x00102B44 File Offset: 0x00100D44
		private void RpcWriter___Server_SetCocaineDiscovered_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x00102BDE File Offset: 0x00100DDE
		public void RpcLogic___SetCocaineDiscovered_2166136261()
		{
			this.SetProductDiscovered(null, "cocaine", false);
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x00102BF0 File Offset: 0x00100DF0
		private void RpcReader___Server_SetCocaineDiscovered_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetCocaineDiscovered_2166136261();
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x00102C10 File Offset: 0x00100E10
		private void RpcWriter___Server_SetProductListed_310431262(string productID, bool listed)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteBoolean(listed);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x00102CC4 File Offset: 0x00100EC4
		public void RpcLogic___SetProductListed_310431262(string productID, bool listed)
		{
			this.SetProductListed(null, productID, listed);
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x00102CD0 File Offset: 0x00100ED0
		private void RpcReader___Server_SetProductListed_310431262(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string productID = PooledReader0.ReadString();
			bool listed = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetProductListed_310431262(productID, listed);
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x00102D20 File Offset: 0x00100F20
		private void RpcWriter___Observers_SetProductListed_619441887(NetworkConnection conn, string productID, bool listed)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteBoolean(listed);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x00102DE4 File Offset: 0x00100FE4
		public void RpcLogic___SetProductListed_619441887(NetworkConnection conn, string productID, bool listed)
		{
			ProductDefinition productDefinition = this.AllProducts.Find((ProductDefinition p) => p.ID == productID);
			if (productDefinition == null)
			{
				Console.LogWarning("SetProductListed: product is not found (" + productID + ")", null);
				return;
			}
			if (!ProductManager.DiscoveredProducts.Contains(productDefinition))
			{
				Console.LogWarning("SetProductListed: product is not yet discovered", null);
			}
			if (listed)
			{
				if (!ProductManager.ListedProducts.Contains(productDefinition))
				{
					ProductManager.ListedProducts.Add(productDefinition);
				}
			}
			else if (ProductManager.ListedProducts.Contains(productDefinition))
			{
				ProductManager.ListedProducts.Remove(productDefinition);
			}
			if (NetworkSingleton<VariableDatabase>.InstanceExists && InstanceFinder.IsServer)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("ListedProductsCount", ProductManager.ListedProducts.Count.ToString(), true);
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("OGKushListed", (ProductManager.ListedProducts.Find((ProductDefinition x) => x.ID == "ogkush") != null).ToString(), true);
			}
			this.HasChanged = true;
			this.TimeSinceProductListingChanged = 0f;
			if (listed)
			{
				if (this.onProductListed != null)
				{
					this.onProductListed(productDefinition);
					return;
				}
			}
			else if (this.onProductDelisted != null)
			{
				this.onProductDelisted(productDefinition);
			}
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x00102F40 File Offset: 0x00101140
		private void RpcReader___Observers_SetProductListed_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool listed = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetProductListed_619441887(null, productID, listed);
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x00102F90 File Offset: 0x00101190
		private void RpcWriter___Target_SetProductListed_619441887(NetworkConnection conn, string productID, bool listed)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteBoolean(listed);
			base.SendTargetRpc(4U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x00103054 File Offset: 0x00101254
		private void RpcReader___Target_SetProductListed_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool listed = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetProductListed_619441887(base.LocalConnection, productID, listed);
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x0010309C File Offset: 0x0010129C
		private void RpcWriter___Server_SetProductFavourited_310431262(string productID, bool listed)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteBoolean(listed);
			base.SendServerRpc(5U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x00103150 File Offset: 0x00101350
		public void RpcLogic___SetProductFavourited_310431262(string productID, bool listed)
		{
			this.SetProductFavourited(null, productID, listed);
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x0010315C File Offset: 0x0010135C
		private void RpcReader___Server_SetProductFavourited_310431262(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string productID = PooledReader0.ReadString();
			bool listed = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetProductFavourited_310431262(productID, listed);
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x001031AC File Offset: 0x001013AC
		private void RpcWriter___Observers_SetProductFavourited_619441887(NetworkConnection conn, string productID, bool fav)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteBoolean(fav);
			base.SendObserversRpc(6U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x00103270 File Offset: 0x00101470
		public void RpcLogic___SetProductFavourited_619441887(NetworkConnection conn, string productID, bool fav)
		{
			ProductDefinition productDefinition = this.AllProducts.Find((ProductDefinition p) => p.ID == productID);
			if (productDefinition == null)
			{
				Console.LogWarning("SetProductFavourited: product is not found (" + productID + ")", null);
				return;
			}
			if (!ProductManager.DiscoveredProducts.Contains(productDefinition))
			{
				Console.LogWarning("SetProductFavourited: product is not yet discovered", null);
			}
			if (fav)
			{
				if (!ProductManager.FavouritedProducts.Contains(productDefinition))
				{
					ProductManager.FavouritedProducts.Add(productDefinition);
				}
			}
			else if (ProductManager.FavouritedProducts.Contains(productDefinition))
			{
				ProductManager.FavouritedProducts.Remove(productDefinition);
			}
			this.HasChanged = true;
			if (fav)
			{
				if (this.onProductFavourited != null)
				{
					this.onProductFavourited(productDefinition);
					return;
				}
			}
			else if (this.onProductUnfavourited != null)
			{
				this.onProductUnfavourited(productDefinition);
			}
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x00103348 File Offset: 0x00101548
		private void RpcReader___Observers_SetProductFavourited_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool fav = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetProductFavourited_619441887(null, productID, fav);
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x00103398 File Offset: 0x00101598
		private void RpcWriter___Target_SetProductFavourited_619441887(NetworkConnection conn, string productID, bool fav)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteBoolean(fav);
			base.SendTargetRpc(7U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x0010345C File Offset: 0x0010165C
		private void RpcReader___Target_SetProductFavourited_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool fav = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetProductFavourited_619441887(base.LocalConnection, productID, fav);
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x001034A4 File Offset: 0x001016A4
		private void RpcWriter___Server_DiscoverProduct_3615296227(string productID)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			base.SendServerRpc(8U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x0010354B File Offset: 0x0010174B
		public void RpcLogic___DiscoverProduct_3615296227(string productID)
		{
			this.SetProductDiscovered(null, productID, true);
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x00103558 File Offset: 0x00101758
		private void RpcReader___Server_DiscoverProduct_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string productID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___DiscoverProduct_3615296227(productID);
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x00103598 File Offset: 0x00101798
		private void RpcWriter___Observers_SetProductDiscovered_619441887(NetworkConnection conn, string productID, bool autoList)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteBoolean(autoList);
			base.SendObserversRpc(9U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x0010365C File Offset: 0x0010185C
		public void RpcLogic___SetProductDiscovered_619441887(NetworkConnection conn, string productID, bool autoList)
		{
			ProductDefinition productDefinition = this.AllProducts.Find((ProductDefinition p) => p.ID == productID);
			if (productDefinition == null)
			{
				Console.LogWarning("SetProductDiscovered: product is not found", null);
				return;
			}
			if (!ProductManager.DiscoveredProducts.Contains(productDefinition))
			{
				ProductManager.DiscoveredProducts.Add(productDefinition);
				if (autoList)
				{
					this.SetProductListed(productID, true);
				}
				if (this.onProductDiscovered != null)
				{
					this.onProductDiscovered(productDefinition);
				}
			}
			this.HasChanged = true;
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x001036E8 File Offset: 0x001018E8
		private void RpcReader___Observers_SetProductDiscovered_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool autoList = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetProductDiscovered_619441887(null, productID, autoList);
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x00103738 File Offset: 0x00101938
		private void RpcWriter___Target_SetProductDiscovered_619441887(NetworkConnection conn, string productID, bool autoList)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteBoolean(autoList);
			base.SendTargetRpc(10U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x001037FC File Offset: 0x001019FC
		private void RpcReader___Target_SetProductDiscovered_619441887(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			bool autoList = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetProductDiscovered_619441887(base.LocalConnection, productID, autoList);
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x00103844 File Offset: 0x00101A44
		private void RpcWriter___Server_CreateWeed_Server_2331775230(string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendServerRpc(11U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003DB3 RID: 15795 RVA: 0x0010391F File Offset: 0x00101B1F
		public void RpcLogic___CreateWeed_Server_2331775230(string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			this.CreateWeed(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x00103930 File Offset: 0x00101B30
		private void RpcReader___Server_CreateWeed_Server_2331775230(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			WeedAppearanceSettings appearance = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CreateWeed_Server_2331775230(name, id, type, properties, appearance);
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x001039B4 File Offset: 0x00101BB4
		private void RpcWriter___Target_CreateWeed_1777266891(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendTargetRpc(12U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x00103AA0 File Offset: 0x00101CA0
		private void RpcLogic___CreateWeed_1777266891(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			if (Registry.GetItem(id) != null)
			{
				Console.LogError("Product with ID " + id + " already exists", null);
				return;
			}
			WeedDefinition weedDefinition = UnityEngine.Object.Instantiate<WeedDefinition>(this.DefaultWeed);
			weedDefinition.name = name;
			weedDefinition.Name = name;
			weedDefinition.Description = string.Empty;
			weedDefinition.ID = id;
			weedDefinition.Initialize(Singleton<PropertyUtility>.Instance.GetProperties(properties), new List<EDrugType>
			{
				type
			}, appearance);
			this.AllProducts.Add(weedDefinition);
			this.ProductPrices.Add(weedDefinition, weedDefinition.MarketValue);
			this.ProductNames.Add(name);
			this.createdProducts.Add(weedDefinition);
			Singleton<Registry>.Instance.AddToRegistry(weedDefinition);
			this.RefreshHighestValueProduct();
			weedDefinition.Icon = Singleton<ProductIconManager>.Instance.GenerateIcons(id);
			if (weedDefinition.Icon == null)
			{
				Console.LogError("Failed to generate icons for " + name, null);
			}
			this.SetProductDiscovered(null, id, false);
			if (this.onNewProductCreated != null)
			{
				this.onNewProductCreated(weedDefinition);
			}
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x00103BB4 File Offset: 0x00101DB4
		private void RpcReader___Target_CreateWeed_1777266891(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			WeedAppearanceSettings appearance = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateWeed_1777266891(base.LocalConnection, name, id, type, properties, appearance);
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x00103C30 File Offset: 0x00101E30
		private void RpcWriter___Observers_CreateWeed_1777266891(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, WeedAppearanceSettings appearance)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendObserversRpc(13U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x00103D1C File Offset: 0x00101F1C
		private void RpcReader___Observers_CreateWeed_1777266891(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			WeedAppearanceSettings appearance = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.WeedAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateWeed_1777266891(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x00103D9C File Offset: 0x00101F9C
		private void RpcWriter___Server_CreateCocaine_Server_891166717(string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendServerRpc(14U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x00103E77 File Offset: 0x00102077
		public void RpcLogic___CreateCocaine_Server_891166717(string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			this.CreateCocaine(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x00103E88 File Offset: 0x00102088
		private void RpcReader___Server_CreateCocaine_Server_891166717(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			CocaineAppearanceSettings appearance = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CreateCocaine_Server_891166717(name, id, type, properties, appearance);
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x00103F0C File Offset: 0x0010210C
		private void RpcWriter___Target_CreateCocaine_1327282946(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendTargetRpc(15U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x00103FF8 File Offset: 0x001021F8
		private void RpcLogic___CreateCocaine_1327282946(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			if (Registry.GetItem(id) != null)
			{
				Console.LogError("Product with ID " + id + " already exists", null);
				return;
			}
			CocaineDefinition cocaineDefinition = UnityEngine.Object.Instantiate<CocaineDefinition>(this.DefaultCocaine);
			cocaineDefinition.name = name;
			cocaineDefinition.Name = name;
			cocaineDefinition.Description = string.Empty;
			cocaineDefinition.ID = id;
			cocaineDefinition.Initialize(Singleton<PropertyUtility>.Instance.GetProperties(properties), new List<EDrugType>
			{
				type
			}, appearance);
			this.AllProducts.Add(cocaineDefinition);
			this.ProductPrices.Add(cocaineDefinition, cocaineDefinition.MarketValue);
			this.ProductNames.Add(name);
			this.createdProducts.Add(cocaineDefinition);
			Singleton<Registry>.Instance.AddToRegistry(cocaineDefinition);
			this.RefreshHighestValueProduct();
			cocaineDefinition.Icon = Singleton<ProductIconManager>.Instance.GenerateIcons(id);
			if (cocaineDefinition.Icon == null)
			{
				Console.LogError("Failed to generate icons for " + name, null);
			}
			this.SetProductDiscovered(null, id, false);
			if (this.onNewProductCreated != null)
			{
				this.onNewProductCreated(cocaineDefinition);
			}
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x0010410C File Offset: 0x0010230C
		private void RpcReader___Target_CreateCocaine_1327282946(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			CocaineAppearanceSettings appearance = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateCocaine_1327282946(base.LocalConnection, name, id, type, properties, appearance);
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x00104188 File Offset: 0x00102388
		private void RpcWriter___Observers_CreateCocaine_1327282946(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, CocaineAppearanceSettings appearance)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendObserversRpc(16U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x00104274 File Offset: 0x00102474
		private void RpcReader___Observers_CreateCocaine_1327282946(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			CocaineAppearanceSettings appearance = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.CocaineAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateCocaine_1327282946(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x001042F4 File Offset: 0x001024F4
		private void RpcWriter___Server_CreateMeth_Server_4251728555(string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendServerRpc(17U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x001043CF File Offset: 0x001025CF
		public void RpcLogic___CreateMeth_Server_4251728555(string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			this.CreateMeth(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x001043E0 File Offset: 0x001025E0
		private void RpcReader___Server_CreateMeth_Server_4251728555(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			MethAppearanceSettings appearance = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CreateMeth_Server_4251728555(name, id, type, properties, appearance);
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x00104464 File Offset: 0x00102664
		private void RpcWriter___Target_CreateMeth_1869045686(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendTargetRpc(18U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x00104550 File Offset: 0x00102750
		private void RpcLogic___CreateMeth_1869045686(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			if (Registry.GetItem(id) != null)
			{
				Console.LogError("Product with ID " + id + " already exists", null);
				return;
			}
			MethDefinition methDefinition = UnityEngine.Object.Instantiate<MethDefinition>(this.DefaultMeth);
			methDefinition.name = name;
			methDefinition.Name = name;
			methDefinition.Description = string.Empty;
			methDefinition.ID = id;
			methDefinition.Initialize(Singleton<PropertyUtility>.Instance.GetProperties(properties), new List<EDrugType>
			{
				type
			}, appearance);
			this.AllProducts.Add(methDefinition);
			this.ProductPrices.Add(methDefinition, methDefinition.MarketValue);
			this.ProductNames.Add(name);
			this.createdProducts.Add(methDefinition);
			Singleton<Registry>.Instance.AddToRegistry(methDefinition);
			this.RefreshHighestValueProduct();
			methDefinition.Icon = Singleton<ProductIconManager>.Instance.GenerateIcons(id);
			if (methDefinition.Icon == null)
			{
				Console.LogError("Failed to generate icons for " + name, null);
			}
			this.SetProductDiscovered(null, id, false);
			if (this.onNewProductCreated != null)
			{
				this.onNewProductCreated(methDefinition);
			}
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x00104664 File Offset: 0x00102864
		private void RpcReader___Target_CreateMeth_1869045686(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			MethAppearanceSettings appearance = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateMeth_1869045686(base.LocalConnection, name, id, type, properties, appearance);
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x001046E0 File Offset: 0x001028E0
		private void RpcWriter___Observers_CreateMeth_1869045686(NetworkConnection conn, string name, string id, EDrugType type, List<string> properties, MethAppearanceSettings appearance)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(name);
			writer.WriteString(id);
			writer.Write___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generated(type);
			writer.Write___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generated(properties);
			writer.Write___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generated(appearance);
			base.SendObserversRpc(19U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x001047CC File Offset: 0x001029CC
		private void RpcReader___Observers_CreateMeth_1869045686(PooledReader PooledReader0, Channel channel)
		{
			string name = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			EDrugType type = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.EDrugTypeFishNet.Serializing.Generateds(PooledReader0);
			List<string> properties = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
			MethAppearanceSettings appearance = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.MethAppearanceSettingsFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateMeth_1869045686(null, name, id, type, properties, appearance);
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x0010484C File Offset: 0x00102A4C
		private void RpcWriter___Server_SendMixRecipe_852232071(string product, string mixer, string output)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(product);
			writer.WriteString(mixer);
			writer.WriteString(output);
			base.SendServerRpc(20U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x0010490D File Offset: 0x00102B0D
		public void RpcLogic___SendMixRecipe_852232071(string product, string mixer, string output)
		{
			this.CreateMixRecipe(null, product, mixer, output);
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x0010491C File Offset: 0x00102B1C
		private void RpcReader___Server_SendMixRecipe_852232071(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string product = PooledReader0.ReadString();
			string mixer = PooledReader0.ReadString();
			string output = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMixRecipe_852232071(product, mixer, output);
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x0010497C File Offset: 0x00102B7C
		private void RpcWriter___Target_CreateMixRecipe_1410895574(NetworkConnection conn, string product, string mixer, string output)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(product);
			writer.WriteString(mixer);
			writer.WriteString(output);
			base.SendTargetRpc(21U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x00104A4C File Offset: 0x00102C4C
		public void RpcLogic___CreateMixRecipe_1410895574(NetworkConnection conn, string product, string mixer, string output)
		{
			if (string.IsNullOrEmpty(product) || string.IsNullOrEmpty(mixer) || string.IsNullOrEmpty(output))
			{
				Console.LogError(string.Concat(new string[]
				{
					"Invalid mix recipe: Product:",
					product,
					" Mixer:",
					mixer,
					" Output:",
					output
				}), null);
				return;
			}
			StationRecipe x = null;
			for (int i = 0; i < this.mixRecipes.Count; i++)
			{
				if (!(this.mixRecipes[i] == null) && this.mixRecipes[i].Product != null && this.mixRecipes[i].Ingredients.Count >= 2)
				{
					string id = this.mixRecipes[i].Ingredients[0].Items[0].ID;
					string id2 = this.mixRecipes[i].Ingredients[1].Items[0].ID;
					string id3 = this.mixRecipes[i].Product.Item.ID;
					if (id == product && id2 == mixer && id3 == output)
					{
						x = this.mixRecipes[i];
						break;
					}
					if (id2 == product && id == mixer && id3 == output)
					{
						x = this.mixRecipes[i];
						break;
					}
				}
			}
			if (x != null)
			{
				Console.LogWarning("Mix recipe already exists", null);
				return;
			}
			StationRecipe stationRecipe = ScriptableObject.CreateInstance<StationRecipe>();
			ItemDefinition item = Registry.GetItem(product);
			ItemDefinition item2 = Registry.GetItem(mixer);
			if (item == null)
			{
				Console.LogError("Product not found: " + product, null);
				return;
			}
			if (item2 == null)
			{
				Console.LogError("Mixer not found: " + mixer, null);
				return;
			}
			stationRecipe.Ingredients = new List<StationRecipe.IngredientQuantity>();
			stationRecipe.Ingredients.Add(new StationRecipe.IngredientQuantity
			{
				Items = new List<ItemDefinition>
				{
					item
				},
				Quantity = 1
			});
			stationRecipe.Ingredients.Add(new StationRecipe.IngredientQuantity
			{
				Items = new List<ItemDefinition>
				{
					item2
				},
				Quantity = 1
			});
			ItemDefinition item3 = Registry.GetItem(output);
			if (item3 == null)
			{
				Console.LogError("Output item not found: " + output, null);
				return;
			}
			stationRecipe.Product = new StationRecipe.ItemQuantity
			{
				Item = item3,
				Quantity = 1
			};
			stationRecipe.RecipeTitle = stationRecipe.Product.Item.Name;
			stationRecipe.Unlocked = true;
			this.mixRecipes.Add(stationRecipe);
			if (this.onMixRecipeAdded != null)
			{
				this.onMixRecipeAdded(stationRecipe);
			}
			ProductDefinition productDefinition = stationRecipe.Product.Item as ProductDefinition;
			if (productDefinition != null)
			{
				productDefinition.AddRecipe(stationRecipe);
			}
			else
			{
				Console.LogError("Product is not a product definition: " + product, null);
			}
			this.HasChanged = true;
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x00104D60 File Offset: 0x00102F60
		private void RpcReader___Target_CreateMixRecipe_1410895574(PooledReader PooledReader0, Channel channel)
		{
			string product = PooledReader0.ReadString();
			string mixer = PooledReader0.ReadString();
			string output = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateMixRecipe_1410895574(base.LocalConnection, product, mixer, output);
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x00104DBC File Offset: 0x00102FBC
		private void RpcWriter___Observers_CreateMixRecipe_1410895574(NetworkConnection conn, string product, string mixer, string output)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(product);
			writer.WriteString(mixer);
			writer.WriteString(output);
			base.SendObserversRpc(22U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x00104E8C File Offset: 0x0010308C
		private void RpcReader___Observers_CreateMixRecipe_1410895574(PooledReader PooledReader0, Channel channel)
		{
			string product = PooledReader0.ReadString();
			string mixer = PooledReader0.ReadString();
			string output = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateMixRecipe_1410895574(null, product, mixer, output);
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x00104EEC File Offset: 0x001030EC
		private void RpcWriter___Target_GiveItem_2971853958(NetworkConnection conn, string id)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(id);
			base.SendTargetRpc(23U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x00104FA1 File Offset: 0x001031A1
		private void RpcLogic___GiveItem_2971853958(NetworkConnection conn, string id)
		{
			PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(Registry.GetItem(id).GetDefaultInstance(1));
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x00104FBC File Offset: 0x001031BC
		private void RpcReader___Target_GiveItem_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___GiveItem_2971853958(base.LocalConnection, id);
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x00104FF4 File Offset: 0x001031F4
		private void RpcWriter___Server_SendPrice_606697822(string productID, float value)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteSingle(value, AutoPackType.Unpacked);
			base.SendServerRpc(24U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x001050AD File Offset: 0x001032AD
		public void RpcLogic___SendPrice_606697822(string productID, float value)
		{
			this.SetPrice(null, productID, value);
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x001050B8 File Offset: 0x001032B8
		private void RpcReader___Server_SendPrice_606697822(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string productID = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPrice_606697822(productID, value);
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x0010510C File Offset: 0x0010330C
		private void RpcWriter___Observers_SetPrice_4077118173(NetworkConnection conn, string productID, float value)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteSingle(value, AutoPackType.Unpacked);
			base.SendObserversRpc(25U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x001051D4 File Offset: 0x001033D4
		public void RpcLogic___SetPrice_4077118173(NetworkConnection conn, string productID, float value)
		{
			ProductDefinition item = Registry.GetItem<ProductDefinition>(productID);
			if (item == null)
			{
				Console.LogError("Product not found: " + productID, null);
				return;
			}
			value = (float)Mathf.RoundToInt(Mathf.Clamp(value, 1f, 999f));
			if (!this.ProductPrices.ContainsKey(item))
			{
				this.ProductPrices.Add(item, value);
				return;
			}
			this.ProductPrices[item] = value;
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x00105244 File Offset: 0x00103444
		private void RpcReader___Observers_SetPrice_4077118173(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetPrice_4077118173(null, productID, value);
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x00105298 File Offset: 0x00103498
		private void RpcWriter___Target_SetPrice_4077118173(NetworkConnection conn, string productID, float value)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteSingle(value, AutoPackType.Unpacked);
			base.SendTargetRpc(26U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x00105360 File Offset: 0x00103560
		private void RpcReader___Target_SetPrice_4077118173(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetPrice_4077118173(base.LocalConnection, productID, value);
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x001053B0 File Offset: 0x001035B0
		private void RpcWriter___Server_SendMixOperation_3670976965(NewMixOperation operation, bool complete)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generated(operation);
			writer.WriteBoolean(complete);
			base.SendServerRpc(27U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x00105464 File Offset: 0x00103664
		public void RpcLogic___SendMixOperation_3670976965(NewMixOperation operation, bool complete)
		{
			this.SetMixOperation(operation, complete);
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x00105470 File Offset: 0x00103670
		private void RpcReader___Server_SendMixOperation_3670976965(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NewMixOperation operation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generateds(PooledReader0);
			bool complete = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMixOperation_3670976965(operation, complete);
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x001054C0 File Offset: 0x001036C0
		private void RpcWriter___Observers_SetMixOperation_3670976965(NewMixOperation operation, bool complete)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generated(operation);
			writer.WriteBoolean(complete);
			base.SendObserversRpc(28U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x00105583 File Offset: 0x00103783
		private void RpcLogic___SetMixOperation_3670976965(NewMixOperation operation, bool complete)
		{
			this.CurrentMixOperation = operation;
			this.IsMixComplete = complete;
			if (this.CurrentMixOperation != null && this.IsMixComplete && this.onMixCompleted != null)
			{
				this.onMixCompleted(this.CurrentMixOperation);
			}
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x001055BC File Offset: 0x001037BC
		private void RpcReader___Observers_SetMixOperation_3670976965(PooledReader PooledReader0, Channel channel)
		{
			NewMixOperation operation = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Product.NewMixOperationFishNet.Serializing.Generateds(PooledReader0);
			bool complete = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetMixOperation_3670976965(operation, complete);
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x00105608 File Offset: 0x00103808
		private void RpcWriter___Observers_FinishAndNameMix_4237212381(string productID, string ingredientID, string mixName, string mixID)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteString(ingredientID);
			writer.WriteString(mixName);
			writer.WriteString(mixID);
			base.SendObserversRpc(29U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x001056E8 File Offset: 0x001038E8
		private void RpcLogic___FinishAndNameMix_4237212381(string productID, string ingredientID, string mixName, string mixID)
		{
			if (this.AllProducts.Find((ProductDefinition p) => p.ID == mixID) != null)
			{
				return;
			}
			ProductDefinition productDefinition = Registry.GetItem(productID) as ProductDefinition;
			PropertyItemDefinition propertyItemDefinition = Registry.GetItem(ingredientID) as PropertyItemDefinition;
			if (productDefinition == null || propertyItemDefinition == null)
			{
				Debug.LogError("Product or mixer not found");
				return;
			}
			List<Property> list = PropertyMixCalculator.MixProperties(productDefinition.Properties, propertyItemDefinition.Properties[0], productDefinition.DrugType);
			List<string> list2 = new List<string>();
			foreach (Property property in list)
			{
				list2.Add(property.ID);
			}
			switch (productDefinition.DrugType)
			{
			case EDrugType.Marijuana:
				this.CreateWeed(null, mixName, mixID, EDrugType.Marijuana, list2, WeedDefinition.GetAppearanceSettings(list));
				return;
			case EDrugType.Methamphetamine:
				this.CreateMeth(null, mixName, mixID, EDrugType.Methamphetamine, list2, MethDefinition.GetAppearanceSettings(list));
				return;
			case EDrugType.Cocaine:
				this.CreateCocaine(null, mixName, mixID, EDrugType.Cocaine, list2, CocaineDefinition.GetAppearanceSettings(list));
				return;
			default:
				Console.LogError("Drug type not supported", null);
				return;
			}
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x00105838 File Offset: 0x00103A38
		private void RpcReader___Observers_FinishAndNameMix_4237212381(PooledReader PooledReader0, Channel channel)
		{
			string productID = PooledReader0.ReadString();
			string ingredientID = PooledReader0.ReadString();
			string mixName = PooledReader0.ReadString();
			string mixID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___FinishAndNameMix_4237212381(productID, ingredientID, mixName, mixID);
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x001058A8 File Offset: 0x00103AA8
		private void RpcWriter___Server_SendFinishAndNameMix_4237212381(string productID, string ingredientID, string mixName, string mixID)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(productID);
			writer.WriteString(ingredientID);
			writer.WriteString(mixName);
			writer.WriteString(mixID);
			base.SendServerRpc(30U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x00105976 File Offset: 0x00103B76
		private void RpcLogic___SendFinishAndNameMix_4237212381(string productID, string ingredientID, string mixName, string mixID)
		{
			this.FinishAndNameMix(productID, ingredientID, mixName, mixID);
			this.CreateMixRecipe(null, productID, ingredientID, mixID);
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x00105990 File Offset: 0x00103B90
		private void RpcReader___Server_SendFinishAndNameMix_4237212381(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string productID = PooledReader0.ReadString();
			string ingredientID = PooledReader0.ReadString();
			string mixName = PooledReader0.ReadString();
			string mixID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendFinishAndNameMix_4237212381(productID, ingredientID, mixName, mixID);
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x001059F4 File Offset: 0x00103BF4
		protected virtual void dll()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x04002C4A RID: 11338
		public const int MIN_PRICE = 1;

		// Token: 0x04002C4B RID: 11339
		public const int MAX_PRICE = 999;

		// Token: 0x04002C4C RID: 11340
		public Action<ProductDefinition> onProductDiscovered;

		// Token: 0x04002C4D RID: 11341
		public static List<ProductDefinition> DiscoveredProducts = new List<ProductDefinition>();

		// Token: 0x04002C4E RID: 11342
		public static List<ProductDefinition> ListedProducts = new List<ProductDefinition>();

		// Token: 0x04002C4F RID: 11343
		public static List<ProductDefinition> FavouritedProducts = new List<ProductDefinition>();

		// Token: 0x04002C51 RID: 11345
		public List<ProductDefinition> AllProducts = new List<ProductDefinition>();

		// Token: 0x04002C52 RID: 11346
		public List<ProductDefinition> DefaultKnownProducts = new List<ProductDefinition>();

		// Token: 0x04002C53 RID: 11347
		public List<PropertyItemDefinition> ValidMixIngredients = new List<PropertyItemDefinition>();

		// Token: 0x04002C54 RID: 11348
		public AnimationCurve SampleSuccessCurve;

		// Token: 0x04002C55 RID: 11349
		[Header("Default Products")]
		public WeedDefinition DefaultWeed;

		// Token: 0x04002C56 RID: 11350
		public CocaineDefinition DefaultCocaine;

		// Token: 0x04002C57 RID: 11351
		public MethDefinition DefaultMeth;

		// Token: 0x04002C58 RID: 11352
		[Header("Mix Maps")]
		public MixerMap WeedMixMap;

		// Token: 0x04002C59 RID: 11353
		public MixerMap MethMixMap;

		// Token: 0x04002C5A RID: 11354
		public MixerMap CokeMixMap;

		// Token: 0x04002C5B RID: 11355
		private List<ProductDefinition> createdProducts = new List<ProductDefinition>();

		// Token: 0x04002C5F RID: 11359
		public Action<NewMixOperation> onMixCompleted;

		// Token: 0x04002C60 RID: 11360
		public Action<ProductDefinition> onNewProductCreated;

		// Token: 0x04002C61 RID: 11361
		public Action<ProductDefinition> onProductListed;

		// Token: 0x04002C62 RID: 11362
		public Action<ProductDefinition> onProductDelisted;

		// Token: 0x04002C63 RID: 11363
		public Action<ProductDefinition> onProductFavourited;

		// Token: 0x04002C64 RID: 11364
		public Action<ProductDefinition> onProductUnfavourited;

		// Token: 0x04002C65 RID: 11365
		public UnityEvent onFirstSampleRejection;

		// Token: 0x04002C66 RID: 11366
		public UnityEvent onSecondUniqueProductCreated;

		// Token: 0x04002C67 RID: 11367
		public List<string> ProductNames = new List<string>();

		// Token: 0x04002C68 RID: 11368
		private List<StationRecipe> mixRecipes = new List<StationRecipe>();

		// Token: 0x04002C69 RID: 11369
		public Action<StationRecipe> onMixRecipeAdded;

		// Token: 0x04002C6A RID: 11370
		private Dictionary<ProductDefinition, float> ProductPrices = new Dictionary<ProductDefinition, float>();

		// Token: 0x04002C6B RID: 11371
		private ProductDefinition highestValueProduct;

		// Token: 0x04002C6C RID: 11372
		private ProductManagerLoader loader = new ProductManagerLoader();

		// Token: 0x04002C70 RID: 11376
		private bool dll_Excuted;

		// Token: 0x04002C71 RID: 11377
		private bool dll_Excuted;
	}
}
