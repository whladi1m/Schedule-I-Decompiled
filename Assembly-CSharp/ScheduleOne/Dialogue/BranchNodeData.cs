using System;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006B1 RID: 1713
	[Serializable]
	public class BranchNodeData
	{
		// Token: 0x04002178 RID: 8568
		public string Guid;

		// Token: 0x04002179 RID: 8569
		public string BranchLabel;

		// Token: 0x0400217A RID: 8570
		public Vector2 Position;

		// Token: 0x0400217B RID: 8571
		public BranchOptionData[] options;
	}
}
