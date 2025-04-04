using System;

namespace ScheduleOne.Product
{
	// Token: 0x020008CE RID: 2254
	[Serializable]
	public class MixRecipeData
	{
		// Token: 0x06003CF6 RID: 15606 RVA: 0x000FFF15 File Offset: 0x000FE115
		public MixRecipeData(string product, string mixer, string output)
		{
			this.Product = product;
			this.Mixer = mixer;
			this.Output = output;
		}

		// Token: 0x04002C11 RID: 11281
		public string Product;

		// Token: 0x04002C12 RID: 11282
		public string Mixer;

		// Token: 0x04002C13 RID: 11283
		public string Output;
	}
}
