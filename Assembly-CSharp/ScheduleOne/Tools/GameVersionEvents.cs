using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x02000843 RID: 2115
	public class GameVersionEvents : MonoBehaviour
	{
		// Token: 0x060039E4 RID: 14820 RVA: 0x000F443C File Offset: 0x000F263C
		private void Start()
		{
			if (this.onFullGame != null)
			{
				this.onFullGame.Invoke();
			}
		}

		// Token: 0x040029CD RID: 10701
		public UnityEvent onFullGame;

		// Token: 0x040029CE RID: 10702
		public UnityEvent onDemoGame;
	}
}
