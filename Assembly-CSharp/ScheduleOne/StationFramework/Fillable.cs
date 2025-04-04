using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008A5 RID: 2213
	public class Fillable : MonoBehaviour
	{
		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06003C4F RID: 15439 RVA: 0x000FDCB0 File Offset: 0x000FBEB0
		// (set) Token: 0x06003C50 RID: 15440 RVA: 0x000FDCB8 File Offset: 0x000FBEB8
		public List<Fillable.Content> contents { get; protected set; } = new List<Fillable.Content>();

		// Token: 0x06003C51 RID: 15441 RVA: 0x000FDCC1 File Offset: 0x000FBEC1
		private void Awake()
		{
			this.LiquidContainer.SetLiquidLevel(0f, false);
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x000FDCD4 File Offset: 0x000FBED4
		public void AddLiquid(string label, float volume, Color color)
		{
			Fillable.Content content = this.contents.Find((Fillable.Content c) => c.Label == label);
			if (content == null)
			{
				content = new Fillable.Content();
				content.Label = label;
				content.Volume_L = 0f;
				content.Color = color;
				this.contents.Add(content);
			}
			content.Volume_L += volume;
			this.UpdateLiquid();
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x000FDD4D File Offset: 0x000FBF4D
		public void ResetContents()
		{
			this.contents.Clear();
			this.UpdateLiquid();
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x000FDD60 File Offset: 0x000FBF60
		private void UpdateLiquid()
		{
			float totalVolume = this.contents.Sum((Fillable.Content x) => x.Volume_L);
			this.LiquidContainer.SetLiquidLevel(totalVolume / this.LiquidCapacity_L, false);
			if (totalVolume > 0f)
			{
				Color color = this.contents.Aggregate(Color.clear, (Color acc, Fillable.Content c) => acc + c.Color * c.Volume_L / totalVolume);
				this.LiquidContainer.SetLiquidColor(color, true, true);
			}
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x000FDDF4 File Offset: 0x000FBFF4
		public float GetLiquidVolume(string label)
		{
			Fillable.Content content = this.contents.Find((Fillable.Content c) => c.Label == label);
			if (content == null)
			{
				return 0f;
			}
			return content.Volume_L;
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x000FDE35 File Offset: 0x000FC035
		public float GetTotalLiquidVolume()
		{
			return this.contents.Sum((Fillable.Content x) => x.Volume_L);
		}

		// Token: 0x04002B7E RID: 11134
		[Header("References")]
		public LiquidContainer LiquidContainer;

		// Token: 0x04002B7F RID: 11135
		[Header("Settings")]
		public bool FillableEnabled = true;

		// Token: 0x04002B80 RID: 11136
		public float LiquidCapacity_L = 1f;

		// Token: 0x020008A6 RID: 2214
		public class Content
		{
			// Token: 0x04002B81 RID: 11137
			public string Label;

			// Token: 0x04002B82 RID: 11138
			public float Volume_L;

			// Token: 0x04002B83 RID: 11139
			public Color Color;
		}
	}
}
