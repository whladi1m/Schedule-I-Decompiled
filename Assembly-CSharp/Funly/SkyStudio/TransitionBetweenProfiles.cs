using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E4 RID: 484
	public class TransitionBetweenProfiles : MonoBehaviour
	{
		// Token: 0x06000ACB RID: 2763 RVA: 0x0002FD35 File Offset: 0x0002DF35
		private void Start()
		{
			this.m_CurrentSkyProfile = this.daySkyProfile;
			if (this.timeOfDayController == null)
			{
				this.timeOfDayController = TimeOfDayController.instance;
			}
			this.timeOfDayController.skyProfile = this.m_CurrentSkyProfile;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002FD70 File Offset: 0x0002DF70
		public void ToggleSkyProfiles()
		{
			this.m_CurrentSkyProfile = ((this.m_CurrentSkyProfile == this.daySkyProfile) ? this.nightSkyProfile : this.daySkyProfile);
			this.timeOfDayController.StartSkyProfileTransition(this.m_CurrentSkyProfile, this.transitionDuration);
		}

		// Token: 0x04000BA5 RID: 2981
		public SkyProfile daySkyProfile;

		// Token: 0x04000BA6 RID: 2982
		public SkyProfile nightSkyProfile;

		// Token: 0x04000BA7 RID: 2983
		[Tooltip("How long the transition animation will last.")]
		[Range(0.1f, 30f)]
		public float transitionDuration = 2f;

		// Token: 0x04000BA8 RID: 2984
		public TimeOfDayController timeOfDayController;

		// Token: 0x04000BA9 RID: 2985
		private SkyProfile m_CurrentSkyProfile;
	}
}
