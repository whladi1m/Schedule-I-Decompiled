using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.ProductManagerApp
{
	// Token: 0x02000A9A RID: 2714
	public class ProductManagerApp : App<ProductManagerApp>
	{
		// Token: 0x06004923 RID: 18723 RVA: 0x0013210B File Offset: 0x0013030B
		protected override void Awake()
		{
			base.Awake();
			this.DetailPanel.SetActiveProduct(null);
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x00132120 File Offset: 0x00130320
		protected override void Start()
		{
			base.Start();
			ProductManager instance = NetworkSingleton<ProductManager>.Instance;
			instance.onProductDiscovered = (Action<ProductDefinition>)Delegate.Combine(instance.onProductDiscovered, new Action<ProductDefinition>(this.CreateEntry));
			ProductManager instance2 = NetworkSingleton<ProductManager>.Instance;
			instance2.onProductFavourited = (Action<ProductDefinition>)Delegate.Combine(instance2.onProductFavourited, new Action<ProductDefinition>(this.ProductFavourited));
			ProductManager instance3 = NetworkSingleton<ProductManager>.Instance;
			instance3.onProductUnfavourited = (Action<ProductDefinition>)Delegate.Combine(instance3.onProductUnfavourited, new Action<ProductDefinition>(this.ProductUnfavourited));
			foreach (ProductDefinition definition in ProductManager.FavouritedProducts)
			{
				this.CreateFavouriteEntry(definition);
			}
			foreach (ProductDefinition definition2 in ProductManager.DiscoveredProducts)
			{
				this.CreateEntry(definition2);
			}
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x0013222C File Offset: 0x0013042C
		private void LateUpdate()
		{
			if (!base.isOpen)
			{
				return;
			}
			if (this.selectedEntry != null)
			{
				this.SelectionIndicator.position = this.selectedEntry.transform.position;
			}
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x00132260 File Offset: 0x00130460
		public virtual void CreateEntry(ProductDefinition definition)
		{
			ProductManagerApp.ProductTypeContainer productTypeContainer = this.ProductTypeContainers.Find((ProductManagerApp.ProductTypeContainer x) => x.DrugType == definition.DrugTypes[0].DrugType);
			ProductEntry component = UnityEngine.Object.Instantiate<GameObject>(this.EntryPrefab, productTypeContainer.Container).GetComponent<ProductEntry>();
			component.Initialize(definition);
			this.entries.Add(component);
			productTypeContainer.RefreshNoneDisplay();
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x001322D2 File Offset: 0x001304D2
		private void ProductFavourited(ProductDefinition product)
		{
			this.CreateFavouriteEntry(product);
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x001322DB File Offset: 0x001304DB
		private void ProductUnfavourited(ProductDefinition product)
		{
			this.RemoveFavouriteEntry(product);
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x001322E4 File Offset: 0x001304E4
		private void CreateFavouriteEntry(ProductDefinition definition)
		{
			if (this.favouriteEntries.Find((ProductEntry x) => x.Definition == definition) != null)
			{
				return;
			}
			ProductEntry component = UnityEngine.Object.Instantiate<GameObject>(this.EntryPrefab, this.FavouritesContainer.Container).GetComponent<ProductEntry>();
			component.Initialize(definition);
			this.favouriteEntries.Add(component);
			this.FavouritesContainer.RefreshNoneDisplay();
			this.DelayedRebuildLayout();
		}

		// Token: 0x0600492A RID: 18730 RVA: 0x00132364 File Offset: 0x00130564
		private void RemoveFavouriteEntry(ProductDefinition definition)
		{
			ProductEntry productEntry = this.favouriteEntries.Find((ProductEntry x) => x.Definition == definition);
			if (this.selectedEntry == productEntry)
			{
				this.selectedEntry = null;
				this.SelectionIndicator.gameObject.SetActive(false);
				this.DetailPanel.SetActiveProduct(null);
			}
			if (productEntry != null)
			{
				this.favouriteEntries.Remove(productEntry);
				productEntry.Destroy();
			}
			this.FavouritesContainer.RefreshNoneDisplay();
			this.DelayedRebuildLayout();
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x001323F5 File Offset: 0x001305F5
		private void DelayedRebuildLayout()
		{
			base.StartCoroutine(this.<DelayedRebuildLayout>g__Delay|17_0());
		}

		// Token: 0x0600492C RID: 18732 RVA: 0x00132404 File Offset: 0x00130604
		public void SelectProduct(ProductEntry entry)
		{
			this.selectedEntry = entry;
			this.DetailPanel.SetActiveProduct(entry.Definition);
			this.SelectionIndicator.position = entry.transform.position;
			this.SelectionIndicator.gameObject.SetActive(true);
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x00132450 File Offset: 0x00130650
		public override void SetOpen(bool open)
		{
			ProductManagerApp.<>c__DisplayClass19_0 CS$<>8__locals1 = new ProductManagerApp.<>c__DisplayClass19_0();
			CS$<>8__locals1.<>4__this = this;
			base.SetOpen(open);
			if (open)
			{
				for (int i = 0; i < this.entries.Count; i++)
				{
					this.entries[i].UpdateDiscovered(this.entries[i].Definition);
					this.entries[i].UpdateListed();
				}
				for (int j = 0; j < this.favouriteEntries.Count; j++)
				{
					this.favouriteEntries[j].UpdateDiscovered(this.favouriteEntries[j].Definition);
					this.favouriteEntries[j].UpdateListed();
				}
				LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
				base.gameObject.SetActive(false);
				base.gameObject.SetActive(true);
				CS$<>8__locals1.layoutGroups = base.GetComponentsInChildren<VerticalLayoutGroup>();
				for (int k = 0; k < CS$<>8__locals1.layoutGroups.Length; k++)
				{
					CS$<>8__locals1.layoutGroups[k].enabled = false;
					CS$<>8__locals1.layoutGroups[k].enabled = true;
				}
				if (this.selectedEntry != null)
				{
					this.DetailPanel.SetActiveProduct(this.selectedEntry.Definition);
				}
				base.StartCoroutine(CS$<>8__locals1.<SetOpen>g__Delay|0());
			}
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x001325B6 File Offset: 0x001307B6
		[CompilerGenerated]
		private IEnumerator <DelayedRebuildLayout>g__Delay|17_0()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
			yield return new WaitForEndOfFrame();
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
			ContentSizeFitter[] componentsInChildren = base.GetComponentsInChildren<ContentSizeFitter>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
				componentsInChildren[i].enabled = true;
			}
			yield break;
		}

		// Token: 0x0400368B RID: 13963
		[Header("References")]
		public ProductManagerApp.ProductTypeContainer FavouritesContainer;

		// Token: 0x0400368C RID: 13964
		public List<ProductManagerApp.ProductTypeContainer> ProductTypeContainers;

		// Token: 0x0400368D RID: 13965
		public ProductAppDetailPanel DetailPanel;

		// Token: 0x0400368E RID: 13966
		public RectTransform SelectionIndicator;

		// Token: 0x0400368F RID: 13967
		public GameObject EntryPrefab;

		// Token: 0x04003690 RID: 13968
		private List<ProductEntry> favouriteEntries = new List<ProductEntry>();

		// Token: 0x04003691 RID: 13969
		private List<ProductEntry> entries = new List<ProductEntry>();

		// Token: 0x04003692 RID: 13970
		private ProductEntry selectedEntry;

		// Token: 0x02000A9B RID: 2715
		[Serializable]
		public class ProductTypeContainer
		{
			// Token: 0x06004930 RID: 18736 RVA: 0x001325C5 File Offset: 0x001307C5
			public void RefreshNoneDisplay()
			{
				this.NoneDisplay.gameObject.SetActive(this.Container.childCount == 0);
			}

			// Token: 0x04003693 RID: 13971
			public EDrugType DrugType;

			// Token: 0x04003694 RID: 13972
			public RectTransform Container;

			// Token: 0x04003695 RID: 13973
			public RectTransform NoneDisplay;
		}
	}
}
