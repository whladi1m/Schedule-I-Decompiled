using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006DA RID: 1754
	public class PackingAlgorithm : Singleton<PackingAlgorithm>
	{
		// Token: 0x06002FD6 RID: 12246 RVA: 0x000C7420 File Offset: 0x000C5620
		public List<PackingAlgorithm.StoredItemData> PackItems(List<ItemInstance> datas, int gridX, int gridY)
		{
			List<PackingAlgorithm.StoredItemData> list = new List<PackingAlgorithm.StoredItemData>();
			for (int i = 0; i < datas.Count; i++)
			{
				StorableItemDefinition storableItemDefinition = datas[i].Definition as StorableItemDefinition;
				if (!(storableItemDefinition == null))
				{
					PackingAlgorithm.StoredItemData item = new PackingAlgorithm.StoredItemData(storableItemDefinition.Name, storableItemDefinition.StoredItem.xSize, storableItemDefinition.StoredItem.ySize, datas[i]);
					list.Add(item);
				}
			}
			this.AttemptPack(list, gridX, gridY);
			return list;
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x000C749C File Offset: 0x000C569C
		public List<PackingAlgorithm.StoredItemData> AttemptPack(List<PackingAlgorithm.StoredItemData> rects, int gridX, int gridY)
		{
			List<PackingAlgorithm.StoredItemData> list = (from o in rects
			orderby o.sizeX * o.sizeY
			select o).ToList<PackingAlgorithm.StoredItemData>();
			list.Reverse();
			PackingAlgorithm.Coordinate[,] array = new PackingAlgorithm.Coordinate[gridX, gridY];
			for (int i = 0; i < gridX; i++)
			{
				for (int j = 0; j < gridY; j++)
				{
					array[i, j] = new PackingAlgorithm.Coordinate(i, j);
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				List<PackingAlgorithm.Coordinate> list2 = new List<PackingAlgorithm.Coordinate>();
				if (k == 0)
				{
					list2.Add(new PackingAlgorithm.Coordinate(0, 0));
				}
				for (int l = 0; l < gridX; l++)
				{
					for (int m = 0; m < gridY; m++)
					{
						if (array[l, m].occupant == null && this.DoesCoordinateHaveOccupiedAdjacent(array, new PackingAlgorithm.Coordinate(l, m), gridX, gridY))
						{
							list2.Add(new PackingAlgorithm.Coordinate(l, m));
						}
					}
				}
				int regionSize = this.GetRegionSize(array, gridX, gridY);
				int num = int.MaxValue;
				PackingAlgorithm.Coordinate coordinate = null;
				bool flipped = false;
				for (int n = 0; n < list2.Count; n++)
				{
					bool flag = true;
					for (int num2 = 0; num2 < list[k].actualSizeX; num2++)
					{
						for (int num3 = 0; num3 < list[k].actualSizeY; num3++)
						{
							PackingAlgorithm.Coordinate coordinate2 = this.TransformCoordinatePoint(array, list2[n], new PackingAlgorithm.Coordinate(num2, num3), gridX, gridY);
							if (coordinate2 == null)
							{
								flag = false;
							}
							else if (coordinate2.occupant != null)
							{
								flag = false;
							}
							if (!flag)
							{
								break;
							}
						}
						if (!flag)
						{
							break;
						}
					}
					if (flag)
					{
						for (int num4 = 0; num4 < list[k].actualSizeX; num4++)
						{
							for (int num5 = 0; num5 < list[k].actualSizeY; num5++)
							{
								this.TransformCoordinatePoint(array, list2[n], new PackingAlgorithm.Coordinate(num4, num5), gridX, gridY).occupant = list[k];
							}
						}
						int num6 = this.GetRegionSize(array, gridX, gridY) - regionSize;
						if (num6 < num)
						{
							num = num6;
							coordinate = list2[n];
							flipped = false;
						}
						for (int num7 = 0; num7 < list[k].actualSizeX; num7++)
						{
							for (int num8 = 0; num8 < list[k].actualSizeY; num8++)
							{
								this.TransformCoordinatePoint(array, list2[n], new PackingAlgorithm.Coordinate(num7, num8), gridX, gridY).occupant = null;
							}
						}
					}
				}
				for (int num9 = 0; num9 < list2.Count; num9++)
				{
					bool flag2 = true;
					list[k].flipped = true;
					for (int num10 = 0; num10 < list[k].actualSizeX; num10++)
					{
						for (int num11 = 0; num11 < list[k].actualSizeY; num11++)
						{
							PackingAlgorithm.Coordinate coordinate3 = this.TransformCoordinatePoint(array, list2[num9], new PackingAlgorithm.Coordinate(num10, num11), gridX, gridY);
							if (coordinate3 == null)
							{
								flag2 = false;
							}
							else if (coordinate3.occupant != null)
							{
								flag2 = false;
							}
							if (!flag2)
							{
								break;
							}
						}
						if (!flag2)
						{
							break;
						}
					}
					if (flag2)
					{
						for (int num12 = 0; num12 < list[k].actualSizeX; num12++)
						{
							for (int num13 = 0; num13 < list[k].actualSizeY; num13++)
							{
								this.TransformCoordinatePoint(array, list2[num9], new PackingAlgorithm.Coordinate(num12, num13), gridX, gridY).occupant = list[k];
							}
						}
						int num14 = this.GetRegionSize(array, gridX, gridY) - regionSize;
						if (num14 < num)
						{
							num = num14;
							coordinate = list2[num9];
							flipped = true;
						}
						for (int num15 = 0; num15 < list[k].actualSizeX; num15++)
						{
							for (int num16 = 0; num16 < list[k].actualSizeY; num16++)
							{
								this.TransformCoordinatePoint(array, list2[num9], new PackingAlgorithm.Coordinate(num15, num16), gridX, gridY).occupant = null;
							}
						}
					}
				}
				if (coordinate == null)
				{
					Console.LogWarning("Unable to resolve rectangle position.", null);
				}
				else
				{
					list[k].flipped = flipped;
					for (int num17 = 0; num17 < list[k].actualSizeX; num17++)
					{
						for (int num18 = 0; num18 < list[k].actualSizeY; num18++)
						{
							this.TransformCoordinatePoint(array, coordinate, new PackingAlgorithm.Coordinate(num17, num18), gridX, gridY).occupant = list[k];
						}
					}
					list[k].xPos = coordinate.x;
					list[k].yPos = coordinate.y;
				}
			}
			return rects;
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x000C794C File Offset: 0x000C5B4C
		private bool DoesCoordinateHaveOccupiedAdjacent(PackingAlgorithm.Coordinate[,] grid, PackingAlgorithm.Coordinate coord, int gridX, int gridY)
		{
			PackingAlgorithm.Coordinate coordinate = new PackingAlgorithm.Coordinate(coord.x - 1, coord.y);
			if (this.IsCoordinateInBounds(coordinate, gridX, gridY) && grid[coordinate.x, coordinate.y].occupant != null)
			{
				return true;
			}
			PackingAlgorithm.Coordinate coordinate2 = new PackingAlgorithm.Coordinate(coord.x + 1, coord.y);
			if (this.IsCoordinateInBounds(coordinate2, gridX, gridY) && grid[coordinate2.x, coordinate2.y].occupant != null)
			{
				return true;
			}
			PackingAlgorithm.Coordinate coordinate3 = new PackingAlgorithm.Coordinate(coord.x, coord.y - 1);
			if (this.IsCoordinateInBounds(coordinate3, gridX, gridY) && grid[coordinate3.x, coordinate3.y].occupant != null)
			{
				return true;
			}
			PackingAlgorithm.Coordinate coordinate4 = new PackingAlgorithm.Coordinate(coord.x, coord.y + 1);
			return this.IsCoordinateInBounds(coordinate4, gridX, gridY) && grid[coordinate4.x, coordinate4.y].occupant != null;
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x000C7A46 File Offset: 0x000C5C46
		private bool IsCoordinateInBounds(PackingAlgorithm.Coordinate coord, int gridX, int gridY)
		{
			return coord.x >= 0 && coord.x < gridX && coord.y >= 0 && coord.y < gridY;
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x000C7A74 File Offset: 0x000C5C74
		private void PrintGrid(PackingAlgorithm.Coordinate[,] grid, int gridX, int gridY)
		{
			string text = string.Empty;
			for (int i = 0; i < gridY; i++)
			{
				for (int j = 0; j < gridX; j++)
				{
					if (grid[j, gridY - i - 1].occupant == null)
					{
						text += "*, ";
					}
					else
					{
						text = text + grid[j, gridY - i - 1].occupant.name + ", ";
					}
				}
				text += "\n";
			}
			Console.Log(text, null);
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x000C7AF8 File Offset: 0x000C5CF8
		private int GetRegionSize(PackingAlgorithm.Coordinate[,] grid, int gridX, int gridY)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < gridX; i++)
			{
				for (int j = 0; j < gridY; j++)
				{
					if (grid[i, j].occupant != null)
					{
						if (i > num3)
						{
							num3 = i;
						}
						if (j > num4)
						{
							num4 = j;
						}
					}
				}
			}
			return (num3 - num) * (num4 - num2);
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x000C7B58 File Offset: 0x000C5D58
		private PackingAlgorithm.Coordinate TransformCoordinatePoint(PackingAlgorithm.Coordinate[,] grid, PackingAlgorithm.Coordinate baseCoordinate, PackingAlgorithm.Coordinate offset, int gridX, int gridY)
		{
			if (this.IsCoordinateInBounds(new PackingAlgorithm.Coordinate(baseCoordinate.x + offset.x, baseCoordinate.y + offset.y), gridX, gridY))
			{
				return grid[baseCoordinate.x + offset.x, baseCoordinate.y + offset.y];
			}
			return null;
		}

		// Token: 0x04002223 RID: 8739
		public List<PackingAlgorithm.Rectangle> rectsToPack = new List<PackingAlgorithm.Rectangle>();

		// Token: 0x020006DB RID: 1755
		[Serializable]
		public class Rectangle
		{
			// Token: 0x170006FA RID: 1786
			// (get) Token: 0x06002FDE RID: 12254 RVA: 0x000C7BC5 File Offset: 0x000C5DC5
			public int actualSizeX
			{
				get
				{
					if (this.flipped)
					{
						return this.sizeY;
					}
					return this.sizeX;
				}
			}

			// Token: 0x170006FB RID: 1787
			// (get) Token: 0x06002FDF RID: 12255 RVA: 0x000C7BDC File Offset: 0x000C5DDC
			public int actualSizeY
			{
				get
				{
					if (this.flipped)
					{
						return this.sizeX;
					}
					return this.sizeY;
				}
			}

			// Token: 0x06002FE0 RID: 12256 RVA: 0x000C7BF3 File Offset: 0x000C5DF3
			public Rectangle(string _name, int x, int y)
			{
				this.name = _name;
				this.sizeX = x;
				this.sizeY = y;
			}

			// Token: 0x04002224 RID: 8740
			public string name;

			// Token: 0x04002225 RID: 8741
			public int sizeX;

			// Token: 0x04002226 RID: 8742
			public int sizeY;

			// Token: 0x04002227 RID: 8743
			public bool flipped;
		}

		// Token: 0x020006DC RID: 1756
		public class StoredItemData : PackingAlgorithm.Rectangle
		{
			// Token: 0x170006FC RID: 1788
			// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x000C7C10 File Offset: 0x000C5E10
			public float rotation
			{
				get
				{
					if (!this.flipped)
					{
						return 0f;
					}
					return 90f;
				}
			}

			// Token: 0x06002FE2 RID: 12258 RVA: 0x000C7C25 File Offset: 0x000C5E25
			public StoredItemData(string _name, int x, int y, ItemInstance _item) : base(_name, x, y)
			{
				this.item = _item;
			}

			// Token: 0x04002228 RID: 8744
			public ItemInstance item;

			// Token: 0x04002229 RID: 8745
			public int xPos;

			// Token: 0x0400222A RID: 8746
			public int yPos;
		}

		// Token: 0x020006DD RID: 1757
		public class Coordinate
		{
			// Token: 0x06002FE3 RID: 12259 RVA: 0x000C7C38 File Offset: 0x000C5E38
			public Coordinate(int _x, int _y)
			{
				this.x = _x;
				this.y = _y;
			}

			// Token: 0x0400222B RID: 8747
			public int x;

			// Token: 0x0400222C RID: 8748
			public int y;

			// Token: 0x0400222D RID: 8749
			public PackingAlgorithm.Rectangle occupant;
		}
	}
}
