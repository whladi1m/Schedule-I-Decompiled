using System;
using UnityEngine;

namespace ScheduleOne.Construction.ConstructionMethods
{
	// Token: 0x0200071D RID: 1821
	public class ConstructStop_Base : MonoBehaviour
	{
		// Token: 0x0600314D RID: 12621 RVA: 0x000CC3BB File Offset: 0x000CA5BB
		public virtual void StopConstruction()
		{
			base.GetComponent<ConstructUpdate_Base>().ConstructionStop();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
