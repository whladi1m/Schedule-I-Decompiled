using System;
using System.Collections;
using ScheduleOne.Clothing;
using ScheduleOne.ItemFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000A5A RID: 2650
	public class ShopColorPicker : MonoBehaviour
	{
		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06004798 RID: 18328 RVA: 0x0012C825 File Offset: 0x0012AA25
		public bool IsOpen
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x0012C834 File Offset: 0x0012AA34
		public void Start()
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(EClothingColor)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EClothingColor color = (EClothingColor)enumerator.Current;
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ColorButtonPrefab, this.ColorButtonParent);
					gameObject.transform.Find("Color").GetComponent<Image>().color = color.GetActualColor();
					gameObject.GetComponent<Button>().onClick.AddListener(delegate()
					{
						this.ColorPicked(color);
					});
					EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
					EventTrigger.Entry entry = new EventTrigger.Entry();
					entry.eventID = EventTriggerType.PointerEnter;
					entry.callback.AddListener(delegate(BaseEventData data)
					{
						this.ColorHovered(color);
					});
					eventTrigger.triggers.Add(entry);
				}
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x0012C93C File Offset: 0x0012AB3C
		private void ColorPicked(EClothingColor color)
		{
			if (this.onColorPicked != null)
			{
				this.onColorPicked.Invoke(color);
			}
			this.Close();
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x0012C958 File Offset: 0x0012AB58
		public void Open(ItemDefinition item)
		{
			this.AssetIconImage.sprite = item.Icon;
			this.ColorHovered(EClothingColor.White);
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x000BEE78 File Offset: 0x000BD078
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x0012C97E File Offset: 0x0012AB7E
		private void ColorHovered(EClothingColor color)
		{
			this.AssetIconImage.color = color.GetActualColor();
			this.ColorLabel.text = color.GetLabel();
		}

		// Token: 0x0400353F RID: 13631
		public Image AssetIconImage;

		// Token: 0x04003540 RID: 13632
		public TextMeshProUGUI ColorLabel;

		// Token: 0x04003541 RID: 13633
		public RectTransform ColorButtonParent;

		// Token: 0x04003542 RID: 13634
		public GameObject ColorButtonPrefab;

		// Token: 0x04003543 RID: 13635
		public UnityEvent<EClothingColor> onColorPicked = new UnityEvent<EClothingColor>();
	}
}
