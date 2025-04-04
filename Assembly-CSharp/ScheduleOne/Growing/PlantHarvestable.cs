using System;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x02000869 RID: 2153
	public class PlantHarvestable : MonoBehaviour
	{
		// Token: 0x06003A78 RID: 14968 RVA: 0x000F6238 File Offset: 0x000F4438
		public virtual void Harvest(bool giveProduct = true)
		{
			Plant componentInParent = base.GetComponentInParent<Plant>();
			if (giveProduct)
			{
				ItemInstance harvestedProduct = componentInParent.GetHarvestedProduct(this.ProductQuantity);
				ProductDefinition productDefinition = this.Product as ProductDefinition;
				if (productDefinition != null && !ProductManager.DiscoveredProducts.Contains(productDefinition))
				{
					NetworkSingleton<ProductManager>.Instance.DiscoverProduct(productDefinition.ID);
				}
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(harvestedProduct);
			}
			base.GetComponentInParent<Pot>().SendHarvestableActive(componentInParent.FinalGrowthStage.GrowthSites.IndexOf(base.transform.parent), false);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject, GameObject.Find("_Temp").transform);
			gameObject.transform.position = base.transform.position;
			gameObject.transform.rotation = base.transform.rotation;
			gameObject.transform.localScale = base.transform.lossyScale;
			UnityEngine.Object.Destroy(gameObject.GetComponent<PlantHarvestable>());
			UnityEngine.Object.Destroy(gameObject.GetComponentInChildren<Collider>());
			gameObject.AddComponent(typeof(Rigidbody));
			Rigidbody component = gameObject.GetComponent<Rigidbody>();
			component.AddForce(Vector3.up * 1.5f, ForceMode.VelocityChange);
			component.AddTorque(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * 4f, ForceMode.VelocityChange);
			UnityEngine.Object.Destroy(gameObject, 2f);
		}

		// Token: 0x04002A53 RID: 10835
		public StorableItemDefinition Product;

		// Token: 0x04002A54 RID: 10836
		public int ProductQuantity = 1;
	}
}
