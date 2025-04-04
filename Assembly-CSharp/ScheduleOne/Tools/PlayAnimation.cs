using System;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000851 RID: 2129
	public class PlayAnimation : MonoBehaviour
	{
		// Token: 0x06003A1C RID: 14876 RVA: 0x000F4AB4 File Offset: 0x000F2CB4
		[Button]
		public void Play()
		{
			base.GetComponent<Animation>().Play();
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x000F4AC2 File Offset: 0x000F2CC2
		public void Play(string animationName)
		{
			base.GetComponent<Animation>().Play(animationName);
		}
	}
}
