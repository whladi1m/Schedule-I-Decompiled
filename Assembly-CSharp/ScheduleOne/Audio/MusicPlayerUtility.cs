using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x0200079C RID: 1948
	public class MusicPlayerUtility : MonoBehaviour
	{
		// Token: 0x060034D5 RID: 13525 RVA: 0x000DE163 File Offset: 0x000DC363
		public void PlayTrack(string trackName)
		{
			Singleton<MusicPlayer>.Instance.SetTrackEnabled(trackName, true);
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x000DE171 File Offset: 0x000DC371
		public void StopTracks()
		{
			Singleton<MusicPlayer>.Instance.StopAndDisableTracks();
		}
	}
}
