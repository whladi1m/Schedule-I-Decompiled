using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x0200059D RID: 1437
	[RequireComponent(typeof(ReflectionProbe))]
	public class ReflectionProbeUpdater : MonoBehaviour
	{
		// Token: 0x060023C7 RID: 9159 RVA: 0x00091505 File Offset: 0x0008F705
		private void OnValidate()
		{
			if (this.Probe == null)
			{
				this.Probe = base.GetComponent<ReflectionProbe>();
			}
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x00091524 File Offset: 0x0008F724
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onHourPass = (Action)Delegate.Combine(instance.onHourPass, new Action(this.UpdateProbe));
			this.UpdateProbe();
			if (ReflectionProbeUpdater.RenderRoutine == null)
			{
				ReflectionProbeUpdater.RenderRoutine = base.StartCoroutine(this.ProcessQueue());
			}
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x00091575 File Offset: 0x0008F775
		private void UpdateProbe()
		{
			if (!ReflectionProbeUpdater.renderQueue.Contains(this.Probe))
			{
				ReflectionProbeUpdater.renderQueue.Add(this.Probe);
			}
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x00091599 File Offset: 0x0008F799
		private IEnumerator ProcessQueue()
		{
			int renderDuration_Frames = 14;
			for (;;)
			{
				if (ReflectionProbeUpdater.renderQueue.Count > 0)
				{
					ReflectionProbeUpdater.renderQueue[0].RenderProbe();
					ReflectionProbeUpdater.renderQueue.RemoveAt(0);
				}
				int num;
				for (int i = 0; i < renderDuration_Frames; i = num + 1)
				{
					yield return new WaitForEndOfFrame();
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x04001ABB RID: 6843
		public ReflectionProbe Probe;

		// Token: 0x04001ABC RID: 6844
		private static List<ReflectionProbe> renderQueue = new List<ReflectionProbe>();

		// Token: 0x04001ABD RID: 6845
		private static Coroutine RenderRoutine = null;
	}
}
