using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.TV
{
	// Token: 0x0200029F RID: 671
	public class SnakeTile : MonoBehaviour
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E07 RID: 3591 RVA: 0x0003EBB5 File Offset: 0x0003CDB5
		// (set) Token: 0x06000E08 RID: 3592 RVA: 0x0003EBBD File Offset: 0x0003CDBD
		public SnakeTile.TileType Type { get; private set; }

		// Token: 0x06000E09 RID: 3593 RVA: 0x0003EBC8 File Offset: 0x0003CDC8
		public void SetType(SnakeTile.TileType type, int index = 0)
		{
			this.Type = type;
			switch (this.Type)
			{
			case SnakeTile.TileType.Empty:
				base.gameObject.SetActive(false);
				return;
			case SnakeTile.TileType.Snake:
				this.Image.color = this.SnakeColor;
				if (index > 0)
				{
					float a = 1f - 0.8f * Mathf.Sqrt((float)index / 240f);
					this.Image.color = new Color(this.SnakeColor.r, this.SnakeColor.g, this.SnakeColor.b, a);
				}
				base.gameObject.SetActive(true);
				return;
			case SnakeTile.TileType.Food:
				this.Image.color = this.FoodColor;
				base.gameObject.SetActive(true);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0003EC90 File Offset: 0x0003CE90
		public void SetPosition(Vector2 position, float tileSize)
		{
			this.Position = position;
			this.RectTransform.anchoredPosition = new Vector2((0.5f + position.x) * tileSize, (0.5f + position.y) * tileSize);
			base.gameObject.name = string.Format("Tile {0}, {1}", position.x, position.y);
			this.RectTransform.sizeDelta = new Vector2(tileSize, tileSize);
		}

		// Token: 0x04000EAF RID: 3759
		public Vector2 Position = Vector2.zero;

		// Token: 0x04000EB0 RID: 3760
		public Color SnakeColor;

		// Token: 0x04000EB1 RID: 3761
		public Color FoodColor;

		// Token: 0x04000EB2 RID: 3762
		public RectTransform RectTransform;

		// Token: 0x04000EB3 RID: 3763
		public Image Image;

		// Token: 0x020002A0 RID: 672
		public enum TileType
		{
			// Token: 0x04000EB5 RID: 3765
			Empty,
			// Token: 0x04000EB6 RID: 3766
			Snake,
			// Token: 0x04000EB7 RID: 3767
			Food
		}
	}
}
