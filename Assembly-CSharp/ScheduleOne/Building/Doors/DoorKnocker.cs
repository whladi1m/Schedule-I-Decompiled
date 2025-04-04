using System;
using UnityEngine;

namespace ScheduleOne.Building.Doors
{
	// Token: 0x02000780 RID: 1920
	public class DoorKnocker : MonoBehaviour
	{
		// Token: 0x06003449 RID: 13385 RVA: 0x000DBE97 File Offset: 0x000DA097
		public void Knock()
		{
			if (this.Anim.isPlaying)
			{
				this.Anim.Stop();
			}
			this.Anim.Play(this.KnockingSoundClipName);
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x000DBEC3 File Offset: 0x000DA0C3
		public void PlayKnockingSound()
		{
			this.KnockingSound.Play();
		}

		// Token: 0x04002580 RID: 9600
		[Header("References")]
		public Animation Anim;

		// Token: 0x04002581 RID: 9601
		public string KnockingSoundClipName;

		// Token: 0x04002582 RID: 9602
		public AudioSource KnockingSound;
	}
}
