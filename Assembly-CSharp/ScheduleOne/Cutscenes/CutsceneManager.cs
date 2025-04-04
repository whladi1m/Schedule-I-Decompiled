using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Cutscenes
{
	// Token: 0x0200070B RID: 1803
	public class CutsceneManager : Singleton<CutsceneManager>
	{
		// Token: 0x060030DE RID: 12510 RVA: 0x000CB274 File Offset: 0x000C9474
		[Button]
		private void RunCutscene()
		{
			this.Play(this.cutsceneName);
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x000CB284 File Offset: 0x000C9484
		public void Play(string name)
		{
			Cutscene cutscene = this.Cutscenes.Find((Cutscene c) => c.Name == name);
			if (cutscene != null)
			{
				cutscene.Play();
				this.playingCutscene = cutscene;
				this.playingCutscene.onEnd.AddListener(new UnityAction(this.Ended));
			}
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x000CB2E8 File Offset: 0x000C94E8
		private void Ended()
		{
			this.playingCutscene.onEnd.RemoveListener(new UnityAction(this.Ended));
			this.playingCutscene = null;
		}

		// Token: 0x040022F6 RID: 8950
		public List<Cutscene> Cutscenes;

		// Token: 0x040022F7 RID: 8951
		[Header("Run cutscene by name")]
		[SerializeField]
		private string cutsceneName = "Wake up morning";

		// Token: 0x040022F8 RID: 8952
		private Cutscene playingCutscene;
	}
}
