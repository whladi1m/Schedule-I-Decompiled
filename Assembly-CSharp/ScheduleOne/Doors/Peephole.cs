using System;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x02000679 RID: 1657
	public class Peephole : MonoBehaviour
	{
		// Token: 0x06002E00 RID: 11776 RVA: 0x000C1211 File Offset: 0x000BF411
		public void Open()
		{
			this.DoorAnim.Play("Peephole open");
			this.OpenSound.Play();
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x000C122F File Offset: 0x000BF42F
		public void Close()
		{
			this.DoorAnim.Play("Peephole close");
			this.CloseSound.Play();
		}

		// Token: 0x040020C3 RID: 8387
		public Animation DoorAnim;

		// Token: 0x040020C4 RID: 8388
		public AudioSourceController OpenSound;

		// Token: 0x040020C5 RID: 8389
		public AudioSourceController CloseSound;
	}
}
