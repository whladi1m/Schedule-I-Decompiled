using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Phone.ProductManagerApp;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.Product
{
	// Token: 0x020008F7 RID: 2295
	public class ProductEntry : MonoBehaviour
	{
		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06003E3F RID: 15935 RVA: 0x00106C7B File Offset: 0x00104E7B
		// (set) Token: 0x06003E40 RID: 15936 RVA: 0x00106C83 File Offset: 0x00104E83
		public ProductDefinition Definition { get; private set; }

		// Token: 0x06003E41 RID: 15937 RVA: 0x00106C8C File Offset: 0x00104E8C
		public void Initialize(ProductDefinition definition)
		{
			this.Definition = definition;
			this.Icon.sprite = definition.Icon;
			this.Button.onClick.AddListener(new UnityAction(this.Clicked));
			this.FavouriteButton.onClick.AddListener(new UnityAction(this.FavouriteClicked));
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.onHovered.Invoke();
			});
			this.Trigger.triggers.Add(entry);
			this.UpdateListed();
			this.UpdateFavourited();
			this.UpdateDiscovered(this.Definition);
			ProductManager instance = NetworkSingleton<ProductManager>.Instance;
			instance.onProductDiscovered = (Action<ProductDefinition>)Delegate.Combine(instance.onProductDiscovered, new Action<ProductDefinition>(this.UpdateDiscovered));
			ProductManager instance2 = NetworkSingleton<ProductManager>.Instance;
			instance2.onProductListed = (Action<ProductDefinition>)Delegate.Combine(instance2.onProductListed, new Action<ProductDefinition>(this.ProductListedOrDelisted));
			ProductManager instance3 = NetworkSingleton<ProductManager>.Instance;
			instance3.onProductDelisted = (Action<ProductDefinition>)Delegate.Combine(instance3.onProductDelisted, new Action<ProductDefinition>(this.ProductListedOrDelisted));
			ProductManager instance4 = NetworkSingleton<ProductManager>.Instance;
			instance4.onProductFavourited = (Action<ProductDefinition>)Delegate.Combine(instance4.onProductFavourited, new Action<ProductDefinition>(this.ProductFavouritedOrUnFavourited));
			ProductManager instance5 = NetworkSingleton<ProductManager>.Instance;
			instance5.onProductUnfavourited = (Action<ProductDefinition>)Delegate.Combine(instance5.onProductUnfavourited, new Action<ProductDefinition>(this.ProductFavouritedOrUnFavourited));
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x00106DF4 File Offset: 0x00104FF4
		public void Destroy()
		{
			this.destroyed = true;
			base.gameObject.SetActive(false);
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x00106E14 File Offset: 0x00105014
		private void OnDestroy()
		{
			ProductManager instance = NetworkSingleton<ProductManager>.Instance;
			instance.onProductDiscovered = (Action<ProductDefinition>)Delegate.Remove(instance.onProductDiscovered, new Action<ProductDefinition>(this.UpdateDiscovered));
			ProductManager instance2 = NetworkSingleton<ProductManager>.Instance;
			instance2.onProductListed = (Action<ProductDefinition>)Delegate.Remove(instance2.onProductListed, new Action<ProductDefinition>(this.ProductListedOrDelisted));
			ProductManager instance3 = NetworkSingleton<ProductManager>.Instance;
			instance3.onProductDelisted = (Action<ProductDefinition>)Delegate.Remove(instance3.onProductDelisted, new Action<ProductDefinition>(this.ProductListedOrDelisted));
			ProductManager instance4 = NetworkSingleton<ProductManager>.Instance;
			instance4.onProductFavourited = (Action<ProductDefinition>)Delegate.Remove(instance4.onProductFavourited, new Action<ProductDefinition>(this.ProductFavouritedOrUnFavourited));
			ProductManager instance5 = NetworkSingleton<ProductManager>.Instance;
			instance5.onProductUnfavourited = (Action<ProductDefinition>)Delegate.Remove(instance5.onProductUnfavourited, new Action<ProductDefinition>(this.ProductFavouritedOrUnFavourited));
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x00106EDF File Offset: 0x001050DF
		private void Clicked()
		{
			PlayerSingleton<ProductManagerApp>.Instance.SelectProduct(this);
			this.UpdateListed();
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x00106EF4 File Offset: 0x001050F4
		private void FavouriteClicked()
		{
			if (!ProductManager.DiscoveredProducts.Contains(this.Definition))
			{
				return;
			}
			if (ProductManager.FavouritedProducts.Contains(this.Definition))
			{
				NetworkSingleton<ProductManager>.Instance.SetProductFavourited(this.Definition.ID, false);
				return;
			}
			NetworkSingleton<ProductManager>.Instance.SetProductFavourited(this.Definition.ID, true);
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x00106F53 File Offset: 0x00105153
		private void ProductListedOrDelisted(ProductDefinition def)
		{
			if (def == this.Definition)
			{
				this.UpdateListed();
			}
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x00106F6C File Offset: 0x0010516C
		public void UpdateListed()
		{
			if (this.destroyed)
			{
				return;
			}
			if (this == null)
			{
				return;
			}
			if (base.gameObject == null)
			{
				return;
			}
			if (ProductManager.ListedProducts.Contains(this.Definition))
			{
				this.Frame.color = this.SelectedColor;
				this.Tick.gameObject.SetActive(true);
				this.Cross.gameObject.SetActive(false);
				return;
			}
			this.Frame.color = this.DeselectedColor;
			this.Tick.gameObject.SetActive(false);
			this.Cross.gameObject.SetActive(true);
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x00107014 File Offset: 0x00105214
		private void ProductFavouritedOrUnFavourited(ProductDefinition def)
		{
			if (def == this.Definition)
			{
				this.UpdateFavourited();
			}
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x0010702C File Offset: 0x0010522C
		public void UpdateFavourited()
		{
			if (this.destroyed)
			{
				return;
			}
			if (this == null)
			{
				return;
			}
			if (base.gameObject == null)
			{
				return;
			}
			if (ProductManager.FavouritedProducts.Contains(this.Definition))
			{
				this.FavouriteIcon.color = this.FavouritedColor;
				return;
			}
			this.FavouriteIcon.color = this.UnfavouritedColor;
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x00107090 File Offset: 0x00105290
		public void UpdateDiscovered(ProductDefinition def)
		{
			if (def == null)
			{
				Console.LogWarning(((def != null) ? def.ToString() : null) + " productDefinition is null", null);
			}
			if (def.ID == this.Definition.ID)
			{
				if (ProductManager.DiscoveredProducts.Contains(this.Definition))
				{
					this.Icon.color = Color.white;
				}
				else
				{
					this.Icon.color = Color.black;
				}
				this.UpdateListed();
			}
		}

		// Token: 0x04002CBB RID: 11451
		public Color SelectedColor;

		// Token: 0x04002CBC RID: 11452
		public Color DeselectedColor;

		// Token: 0x04002CBD RID: 11453
		public Color FavouritedColor;

		// Token: 0x04002CBE RID: 11454
		public Color UnfavouritedColor;

		// Token: 0x04002CBF RID: 11455
		[Header("References")]
		public Button Button;

		// Token: 0x04002CC0 RID: 11456
		public Image Frame;

		// Token: 0x04002CC1 RID: 11457
		public Image Icon;

		// Token: 0x04002CC2 RID: 11458
		public RectTransform Tick;

		// Token: 0x04002CC3 RID: 11459
		public RectTransform Cross;

		// Token: 0x04002CC4 RID: 11460
		public EventTrigger Trigger;

		// Token: 0x04002CC5 RID: 11461
		public Button FavouriteButton;

		// Token: 0x04002CC6 RID: 11462
		public Image FavouriteIcon;

		// Token: 0x04002CC7 RID: 11463
		public UnityEvent onHovered;

		// Token: 0x04002CC8 RID: 11464
		private bool destroyed;
	}
}
