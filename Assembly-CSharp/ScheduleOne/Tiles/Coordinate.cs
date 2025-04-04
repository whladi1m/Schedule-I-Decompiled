using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002B3 RID: 691
	[Serializable]
	public class Coordinate
	{
		// Token: 0x06000ECF RID: 3791 RVA: 0x00041766 File Offset: 0x0003F966
		public static implicit operator Vector2(Coordinate c)
		{
			return new Vector2((float)c.x, (float)c.y);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0004177B File Offset: 0x0003F97B
		public Coordinate()
		{
			this.x = 0;
			this.y = 0;
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00041791 File Offset: 0x0003F991
		public Coordinate(int _x, int _y)
		{
			this.x = _x;
			this.y = _y;
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x000417A7 File Offset: 0x0003F9A7
		public Coordinate(Vector2 vector)
		{
			this.x = (int)vector.x;
			this.y = (int)vector.y;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x000417C9 File Offset: 0x0003F9C9
		public override int GetHashCode()
		{
			return this.SignedCantorPair(this.x, this.y);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x000417E0 File Offset: 0x0003F9E0
		public override bool Equals(object obj)
		{
			Coordinate coordinate = obj as Coordinate;
			return coordinate != null && coordinate.x == this.x && coordinate.y == this.y;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00041815 File Offset: 0x0003FA15
		public static Coordinate operator +(Coordinate a, Coordinate b)
		{
			return new Coordinate(a.x + b.x, a.y + b.y);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00041836 File Offset: 0x0003FA36
		public static Coordinate operator -(Coordinate a, Coordinate b)
		{
			return new Coordinate(a.x - b.x, a.y - b.y);
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00041857 File Offset: 0x0003FA57
		private int CantorPair(int x, int y)
		{
			return (int)(0.5f * (float)(x + y) * ((float)(x + y) + 1f) + (float)y);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00041874 File Offset: 0x0003FA74
		private int SignedCantorPair(int x, int y)
		{
			int num = (int)(((float)x >= 0f) ? (2f * (float)x) : (-2f * (float)x - 1f));
			int num2 = (int)(((float)y >= 0f) ? (2f * (float)y) : (-2f * (float)y - 1f));
			return this.CantorPair(num, num2);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x000418D0 File Offset: 0x0003FAD0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.x.ToString(),
				",",
				this.y.ToString(),
				"]"
			});
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0004191C File Offset: 0x0003FB1C
		public static List<CoordinatePair> BuildCoordinateMatches(Coordinate originCoord, int sizeX, int sizeY, float rot)
		{
			List<CoordinatePair> list = new List<CoordinatePair>();
			rot = (float)Coordinate.MathMod(Mathf.RoundToInt(rot), 360);
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					Coordinate coordinate = new Coordinate(originCoord.x, originCoord.y);
					if ((double)rot == 0.0)
					{
						coordinate.x += i;
						coordinate.y += j;
					}
					else if (rot == 90f)
					{
						coordinate.x += j;
						coordinate.y -= i;
					}
					else if (rot == 180f)
					{
						coordinate.x -= i;
						coordinate.y -= j;
					}
					else if (rot == 270f)
					{
						coordinate.x -= j;
						coordinate.y += i;
					}
					else
					{
						Console.LogWarning("Cock!!!!!! " + rot.ToString(), null);
					}
					list.Add(new CoordinatePair(new Coordinate(i, j), coordinate));
				}
			}
			return list;
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00041A44 File Offset: 0x0003FC44
		public static Coordinate RotateCoordinates(Coordinate coord, float angle)
		{
			angle = (float)Coordinate.MathMod(Mathf.RoundToInt(angle), 360);
			if (Mathf.Abs(angle - 90f) < 0.01f)
			{
				return new Coordinate(coord.y, -coord.x);
			}
			if (Mathf.Abs(angle - 180f) < 0.01f)
			{
				return new Coordinate(-coord.x, -coord.y);
			}
			if (Mathf.Abs(angle - 270f) < 0.01f)
			{
				return new Coordinate(-coord.y, coord.x);
			}
			return coord;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00041AD8 File Offset: 0x0003FCD8
		private static int MathMod(int a, int b)
		{
			return (Mathf.Abs(a * b) + a) % b;
		}

		// Token: 0x04000F41 RID: 3905
		public int x;

		// Token: 0x04000F42 RID: 3906
		public int y;
	}
}
