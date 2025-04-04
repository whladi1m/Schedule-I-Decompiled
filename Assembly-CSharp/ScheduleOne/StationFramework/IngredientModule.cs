using System;
using ScheduleOne.PlayerTasks;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008AB RID: 2219
	public class IngredientModule : ItemModule
	{
		// Token: 0x06003C63 RID: 15459 RVA: 0x000FDEE4 File Offset: 0x000FC0E4
		public override void ActivateModule(StationItem item)
		{
			base.ActivateModule(item);
			for (int i = 0; i < this.Pieces.Length; i++)
			{
				this.Pieces[i].GetComponent<DraggableConstraint>().SetContainer(item.transform.parent);
			}
		}

		// Token: 0x04002B8A RID: 11146
		public IngredientPiece[] Pieces;
	}
}
