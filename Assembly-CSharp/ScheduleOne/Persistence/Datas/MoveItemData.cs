using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000408 RID: 1032
	[Serializable]
	public class MoveItemData
	{
		// Token: 0x06001568 RID: 5480 RVA: 0x0005F4B4 File Offset: 0x0005D6B4
		public MoveItemData(string templateItemJson, int grabbedItemQuantity, Guid sourceGUID, Guid destinationGUID)
		{
			this.TemplateItemJSON = templateItemJson;
			this.GrabbedItemQuantity = grabbedItemQuantity;
			this.SourceGUID = sourceGUID.ToString();
			this.DestinationGUID = destinationGUID.ToString();
		}

		// Token: 0x040013E6 RID: 5094
		public string TemplateItemJSON = string.Empty;

		// Token: 0x040013E7 RID: 5095
		public int GrabbedItemQuantity;

		// Token: 0x040013E8 RID: 5096
		public string SourceGUID;

		// Token: 0x040013E9 RID: 5097
		public string DestinationGUID;
	}
}
