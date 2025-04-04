using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000829 RID: 2089
	public class ActiveOnMeshVisible : MonoBehaviour
	{
		// Token: 0x0600398B RID: 14731 RVA: 0x000F3878 File Offset: 0x000F1A78
		private void LateUpdate()
		{
			if (this.Mesh.isVisible && !this.isVisible)
			{
				this.isVisible = true;
				GameObject[] objectsToActivate = this.ObjectsToActivate;
				for (int i = 0; i < objectsToActivate.Length; i++)
				{
					objectsToActivate[i].SetActive(!this.Reverse);
				}
				return;
			}
			if (!this.Mesh.isVisible && this.isVisible)
			{
				this.isVisible = false;
				GameObject[] objectsToActivate = this.ObjectsToActivate;
				for (int i = 0; i < objectsToActivate.Length; i++)
				{
					objectsToActivate[i].SetActive(this.Reverse);
				}
			}
		}

		// Token: 0x04002991 RID: 10641
		public MeshRenderer Mesh;

		// Token: 0x04002992 RID: 10642
		public GameObject[] ObjectsToActivate;

		// Token: 0x04002993 RID: 10643
		public bool Reverse;

		// Token: 0x04002994 RID: 10644
		private bool isVisible = true;
	}
}
