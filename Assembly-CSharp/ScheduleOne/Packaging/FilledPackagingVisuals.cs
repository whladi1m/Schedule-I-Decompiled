using System;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x0200087C RID: 2172
	public class FilledPackagingVisuals : MonoBehaviour
	{
		// Token: 0x06003AD4 RID: 15060 RVA: 0x000F7918 File Offset: 0x000F5B18
		public void ResetVisuals()
		{
			if (this.weedVisuals.Container != null)
			{
				this.weedVisuals.Container.gameObject.SetActive(false);
			}
			if (this.methVisuals.Container != null)
			{
				this.methVisuals.Container.gameObject.SetActive(false);
			}
			if (this.cocaineVisuals.Container != null)
			{
				this.cocaineVisuals.Container.gameObject.SetActive(false);
			}
		}

		// Token: 0x04002ABB RID: 10939
		public FilledPackagingVisuals.WeedVisuals weedVisuals;

		// Token: 0x04002ABC RID: 10940
		public FilledPackagingVisuals.MethVisuals methVisuals;

		// Token: 0x04002ABD RID: 10941
		public FilledPackagingVisuals.CocaineVisuals cocaineVisuals;

		// Token: 0x0200087D RID: 2173
		[Serializable]
		public class MeshIndexPair
		{
			// Token: 0x04002ABE RID: 10942
			public MeshRenderer Mesh;

			// Token: 0x04002ABF RID: 10943
			public int MaterialIndex;
		}

		// Token: 0x0200087E RID: 2174
		[Serializable]
		public class BaseVisuals
		{
			// Token: 0x04002AC0 RID: 10944
			public Transform Container;
		}

		// Token: 0x0200087F RID: 2175
		[Serializable]
		public class WeedVisuals : FilledPackagingVisuals.BaseVisuals
		{
			// Token: 0x04002AC1 RID: 10945
			public FilledPackagingVisuals.MeshIndexPair[] MainMeshes;

			// Token: 0x04002AC2 RID: 10946
			public FilledPackagingVisuals.MeshIndexPair[] SecondaryMeshes;

			// Token: 0x04002AC3 RID: 10947
			public FilledPackagingVisuals.MeshIndexPair[] LeafMeshes;

			// Token: 0x04002AC4 RID: 10948
			public FilledPackagingVisuals.MeshIndexPair[] StemMeshes;
		}

		// Token: 0x02000880 RID: 2176
		[Serializable]
		public class MethVisuals : FilledPackagingVisuals.BaseVisuals
		{
			// Token: 0x04002AC5 RID: 10949
			public MeshRenderer[] CrystalMeshes;
		}

		// Token: 0x02000881 RID: 2177
		[Serializable]
		public class CocaineVisuals : FilledPackagingVisuals.BaseVisuals
		{
			// Token: 0x04002AC6 RID: 10950
			public MeshRenderer[] RockMeshes;
		}
	}
}
