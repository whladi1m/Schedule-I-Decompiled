using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using ScheduleOne.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations.Drying_rack
{
	// Token: 0x02000A4E RID: 2638
	public class DryingOperationUI : MonoBehaviour
	{
		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06004734 RID: 18228 RVA: 0x0012B2D4 File Offset: 0x001294D4
		// (set) Token: 0x06004735 RID: 18229 RVA: 0x0012B2DC File Offset: 0x001294DC
		public DryingOperation AssignedOperation { get; protected set; }

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06004736 RID: 18230 RVA: 0x0012B2E5 File Offset: 0x001294E5
		// (set) Token: 0x06004737 RID: 18231 RVA: 0x0012B2ED File Offset: 0x001294ED
		public RectTransform Alignment { get; private set; }

		// Token: 0x06004738 RID: 18232 RVA: 0x0012B2F6 File Offset: 0x001294F6
		public void SetOperation(DryingOperation operation)
		{
			this.AssignedOperation = operation;
			this.Icon.sprite = Registry.GetItem(operation.ItemID).Icon;
			this.RefreshQuantity();
			this.UpdatePosition();
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x0012B326 File Offset: 0x00129526
		public void SetAlignment(RectTransform alignment)
		{
			this.Alignment = alignment;
			base.transform.SetParent(alignment);
			this.UpdatePosition();
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x0012B341 File Offset: 0x00129541
		public void RefreshQuantity()
		{
			this.QuantityLabel.text = this.AssignedOperation.Quantity.ToString() + "x";
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x0012B368 File Offset: 0x00129568
		public void Start()
		{
			this.Button.onClick.AddListener(delegate()
			{
				this.Clicked();
			});
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x0012B388 File Offset: 0x00129588
		public void UpdatePosition()
		{
			float t = Mathf.Clamp01((float)this.AssignedOperation.Time / 720f);
			int num = Mathf.Clamp(720 - this.AssignedOperation.Time, 0, 720);
			int num2 = num / 60;
			int num3 = num % 60;
			this.Tooltip.text = num2.ToString() + "h " + num3.ToString() + "m until next tier";
			float num4 = -62.5f;
			float b = -num4;
			this.Rect.anchoredPosition = new Vector2(Mathf.Lerp(num4, b, t), 0f);
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x0012B424 File Offset: 0x00129624
		private void Clicked()
		{
			Singleton<DryingRackCanvas>.Instance.Rack.TryEndOperation(Singleton<DryingRackCanvas>.Instance.Rack.DryingOperations.IndexOf(this.AssignedOperation), true, this.AssignedOperation.GetQuality(), UnityEngine.Random.Range(int.MinValue, int.MaxValue));
		}

		// Token: 0x040034EB RID: 13547
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x040034EC RID: 13548
		public Image Icon;

		// Token: 0x040034ED RID: 13549
		public TextMeshProUGUI QuantityLabel;

		// Token: 0x040034EE RID: 13550
		public Button Button;

		// Token: 0x040034EF RID: 13551
		public Tooltip Tooltip;
	}
}
