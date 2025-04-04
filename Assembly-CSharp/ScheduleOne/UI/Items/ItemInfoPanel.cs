using System;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000B47 RID: 2887
	public class ItemInfoPanel : MonoBehaviour
	{
		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06004CB5 RID: 19637 RVA: 0x00142E16 File Offset: 0x00141016
		// (set) Token: 0x06004CB6 RID: 19638 RVA: 0x00142E1E File Offset: 0x0014101E
		public bool IsOpen { get; protected set; }

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06004CB7 RID: 19639 RVA: 0x00142E27 File Offset: 0x00141027
		// (set) Token: 0x06004CB8 RID: 19640 RVA: 0x00142E2F File Offset: 0x0014102F
		public ItemInstance CurrentItem { get; protected set; }

		// Token: 0x06004CB9 RID: 19641 RVA: 0x00142E38 File Offset: 0x00141038
		private void Awake()
		{
			this.Close();
		}

		// Token: 0x06004CBA RID: 19642 RVA: 0x00142E40 File Offset: 0x00141040
		public void Open(ItemInstance item, RectTransform rect)
		{
			if (this.IsOpen)
			{
				this.Close();
			}
			if (item == null)
			{
				Console.LogWarning("Item is null!", null);
				return;
			}
			this.CurrentItem = item;
			if (item.Definition.CustomInfoContent != null)
			{
				this.content = UnityEngine.Object.Instantiate<ItemInfoContent>(item.Definition.CustomInfoContent, this.ContentContainer);
				this.content.Initialize(item);
			}
			else
			{
				this.content = UnityEngine.Object.Instantiate<ItemInfoContent>(this.DefaultContentPrefab, this.ContentContainer);
				this.content.Initialize(item);
			}
			this.Container.sizeDelta = new Vector2(this.Container.sizeDelta.x, this.content.Height);
			float num = (rect.sizeDelta.y + this.Container.sizeDelta.y) / 2f + this.Offset.y;
			num *= this.Canvas.scaleFactor;
			if (rect.position.y > 200f)
			{
				this.Container.position = rect.position - new Vector3(0f, num, 0f);
				this.TopArrow.SetActive(true);
				this.BottomArrow.SetActive(false);
			}
			else
			{
				this.Container.position = rect.position + new Vector3(0f, num, 0f);
				this.TopArrow.SetActive(false);
				this.BottomArrow.SetActive(true);
			}
			this.IsOpen = true;
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x00142FE4 File Offset: 0x001411E4
		public void Open(ItemDefinition def, RectTransform rect)
		{
			if (this.IsOpen)
			{
				this.Close();
			}
			if (def == null)
			{
				Console.LogWarning("Item is null!", null);
				return;
			}
			this.CurrentItem = null;
			this.content = UnityEngine.Object.Instantiate<ItemInfoContent>(this.DefaultContentPrefab, this.ContentContainer);
			this.content.Initialize(def);
			float num = (rect.sizeDelta.y + this.Container.sizeDelta.y) / 2f + this.Offset.y;
			num *= this.Canvas.scaleFactor;
			if (rect.position.y > 200f)
			{
				this.Container.position = rect.position - new Vector3(0f, num, 0f);
				this.TopArrow.SetActive(true);
				this.BottomArrow.SetActive(false);
			}
			else
			{
				this.Container.position = rect.position + new Vector3(0f, num, 0f);
				this.TopArrow.SetActive(false);
				this.BottomArrow.SetActive(true);
			}
			this.IsOpen = true;
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x00143123 File Offset: 0x00141323
		public void Close()
		{
			if (this.content != null)
			{
				UnityEngine.Object.Destroy(this.content.gameObject);
			}
			this.IsOpen = false;
			this.CurrentItem = null;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x04003A04 RID: 14852
		public const float VERTICAL_THRESHOLD = 200f;

		// Token: 0x04003A07 RID: 14855
		[Header("References")]
		public RectTransform Container;

		// Token: 0x04003A08 RID: 14856
		public RectTransform ContentContainer;

		// Token: 0x04003A09 RID: 14857
		public GameObject TopArrow;

		// Token: 0x04003A0A RID: 14858
		public GameObject BottomArrow;

		// Token: 0x04003A0B RID: 14859
		public Canvas Canvas;

		// Token: 0x04003A0C RID: 14860
		[Header("Settings")]
		public Vector2 Offset = new Vector2(0f, 125f);

		// Token: 0x04003A0D RID: 14861
		[Header("Prefabs")]
		public ItemInfoContent DefaultContentPrefab;

		// Token: 0x04003A0E RID: 14862
		private ItemInfoContent content;
	}
}
