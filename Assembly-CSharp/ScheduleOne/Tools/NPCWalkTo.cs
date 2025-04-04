using System;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200084C RID: 2124
	[RequireComponent(typeof(NPCMovement))]
	public class NPCWalkTo : MonoBehaviour
	{
		// Token: 0x06003A04 RID: 14852 RVA: 0x000F4788 File Offset: 0x000F2988
		private void Update()
		{
			this.timeSinceLastPath += Time.deltaTime;
			if (this.timeSinceLastPath >= this.RepathRate)
			{
				this.timeSinceLastPath = 0f;
				base.GetComponent<NPCMovement>().SetDestination(this.Target.position);
			}
		}

		// Token: 0x040029E3 RID: 10723
		public Transform Target;

		// Token: 0x040029E4 RID: 10724
		public float RepathRate = 0.5f;

		// Token: 0x040029E5 RID: 10725
		private float timeSinceLastPath;
	}
}
