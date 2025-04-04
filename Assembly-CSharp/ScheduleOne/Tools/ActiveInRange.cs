using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000828 RID: 2088
	public class ActiveInRange : MonoBehaviour
	{
		// Token: 0x06003989 RID: 14729 RVA: 0x000F378C File Offset: 0x000F198C
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			bool flag = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, base.transform.position) < this.Distance * (this.ScaleByLODBias ? QualitySettings.lodBias : 1f);
			if (flag && !this.isVisible)
			{
				this.isVisible = true;
				GameObject[] objectsToActivate = this.ObjectsToActivate;
				for (int i = 0; i < objectsToActivate.Length; i++)
				{
					objectsToActivate[i].SetActive(!this.Reverse);
				}
				return;
			}
			if (!flag && this.isVisible)
			{
				this.isVisible = false;
				GameObject[] objectsToActivate = this.ObjectsToActivate;
				for (int i = 0; i < objectsToActivate.Length; i++)
				{
					objectsToActivate[i].SetActive(this.Reverse);
				}
			}
		}

		// Token: 0x0400298C RID: 10636
		public float Distance = 10f;

		// Token: 0x0400298D RID: 10637
		public bool ScaleByLODBias = true;

		// Token: 0x0400298E RID: 10638
		public GameObject[] ObjectsToActivate;

		// Token: 0x0400298F RID: 10639
		public bool Reverse;

		// Token: 0x04002990 RID: 10640
		private bool isVisible = true;
	}
}
