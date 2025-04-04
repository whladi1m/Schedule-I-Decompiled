using System;
using ScheduleOne.ConstructableScripts;
using UnityEngine;

namespace ScheduleOne.Construction.ConstructionMethods
{
	// Token: 0x0200071A RID: 1818
	public abstract class ConstructStart_Base : MonoBehaviour
	{
		// Token: 0x06003147 RID: 12615 RVA: 0x000CC23F File Offset: 0x000CA43F
		public virtual void StartConstruction(string constructableID, Constructable_GridBased movedConstructable = null)
		{
			if (movedConstructable != null)
			{
				base.gameObject.GetComponent<ConstructUpdate_Base>().MovedConstructable = movedConstructable;
				movedConstructable.SetInvisible();
			}
		}
	}
}
