using System;
using UnityEngine;

namespace ScheduleOne.Levelling
{
	// Token: 0x020005A1 RID: 1441
	[Serializable]
	public struct FullRank
	{
		// Token: 0x060023D7 RID: 9175 RVA: 0x0009178E File Offset: 0x0008F98E
		public FullRank(ERank rank, int tier)
		{
			this.Rank = rank;
			this.Tier = tier;
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x0009179E File Offset: 0x0008F99E
		public override string ToString()
		{
			return FullRank.GetString(this);
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x000917AC File Offset: 0x0008F9AC
		public FullRank NextRank()
		{
			if (this.Rank == ERank.Kingpin)
			{
				return new FullRank(ERank.Kingpin, this.Tier + 1);
			}
			if (this.Tier < 5)
			{
				return new FullRank(this.Rank, this.Tier + 1);
			}
			return new FullRank(this.Rank + 1, 1);
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x00091800 File Offset: 0x0008FA00
		public static string GetString(FullRank rank)
		{
			string text = rank.Rank.ToString();
			text = text.Replace("_", " ");
			switch (rank.Tier)
			{
			case 1:
				text += " I";
				break;
			case 2:
				text += " II";
				break;
			case 3:
				text += " III";
				break;
			case 4:
				text += " IV";
				break;
			case 5:
				text += " V";
				break;
			default:
				text = text + " " + rank.Tier.ToString();
				break;
			}
			return text;
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x000918B5 File Offset: 0x0008FAB5
		public static bool operator >(FullRank a, FullRank b)
		{
			return a.Rank > b.Rank || (a.Rank == b.Rank && a.Tier > b.Tier);
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x000918E5 File Offset: 0x0008FAE5
		public static bool operator <(FullRank a, FullRank b)
		{
			return a.Rank < b.Rank || (a.Rank == b.Rank && a.Tier < b.Tier);
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x00091915 File Offset: 0x0008FB15
		public static bool operator <=(FullRank a, FullRank b)
		{
			return a < b || a == b;
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x00091929 File Offset: 0x0008FB29
		public static bool operator >=(FullRank a, FullRank b)
		{
			return a > b || a == b;
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x0009193D File Offset: 0x0008FB3D
		public static bool operator ==(FullRank a, FullRank b)
		{
			return a.Rank == b.Rank && a.Tier == b.Tier;
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x0009195D File Offset: 0x0008FB5D
		public static bool operator !=(FullRank a, FullRank b)
		{
			return a.Rank != b.Rank || a.Tier != b.Tier;
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x00091980 File Offset: 0x0008FB80
		public override bool Equals(object obj)
		{
			return obj is FullRank && this == (FullRank)obj;
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x0009199D File Offset: 0x0008FB9D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x000919AF File Offset: 0x0008FBAF
		public int CompareTo(FullRank other)
		{
			if (this > other)
			{
				return 1;
			}
			if (this < other)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x04001ACA RID: 6858
		public ERank Rank;

		// Token: 0x04001ACB RID: 6859
		[Range(1f, 5f)]
		public int Tier;
	}
}
