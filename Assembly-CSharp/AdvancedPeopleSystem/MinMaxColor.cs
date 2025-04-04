using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200020B RID: 523
	[Serializable]
	public class MinMaxColor
	{
		// Token: 0x06000B70 RID: 2928 RVA: 0x00035508 File Offset: 0x00033708
		public Color GetRandom()
		{
			int index = UnityEngine.Random.Range(0, this.minColors.Count);
			return Color.Lerp(this.minColors[index], this.maxColors[index], UnityEngine.Random.Range(0f, 1f));
		}

		// Token: 0x04000C61 RID: 3169
		public List<Color> minColors = new List<Color>();

		// Token: 0x04000C62 RID: 3170
		public List<Color> maxColors = new List<Color>();
	}
}
