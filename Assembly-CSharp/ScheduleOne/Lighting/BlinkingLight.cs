using System;
using System.Collections;
using ScheduleOne.Misc;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x02000596 RID: 1430
	[RequireComponent(typeof(ToggleableLight))]
	public class BlinkingLight : MonoBehaviour
	{
		// Token: 0x060023A4 RID: 9124 RVA: 0x00090DA9 File Offset: 0x0008EFA9
		private void Awake()
		{
			this.light = base.GetComponent<ToggleableLight>();
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x00090DB7 File Offset: 0x0008EFB7
		private void Update()
		{
			if (this.IsOn && this.blinkRoutine == null)
			{
				this.blinkRoutine = base.StartCoroutine(this.Blink());
			}
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x00090DDB File Offset: 0x0008EFDB
		private IEnumerator Blink()
		{
			while (this.IsOn)
			{
				this.light.isOn = true;
				yield return new WaitForSeconds(this.OnTime);
				this.light.isOn = false;
				yield return new WaitForSeconds(this.OffTime);
			}
			this.blinkRoutine = null;
			yield break;
		}

		// Token: 0x04001A91 RID: 6801
		public bool IsOn;

		// Token: 0x04001A92 RID: 6802
		public float OnTime = 0.5f;

		// Token: 0x04001A93 RID: 6803
		public float OffTime = 0.5f;

		// Token: 0x04001A94 RID: 6804
		private ToggleableLight light;

		// Token: 0x04001A95 RID: 6805
		private Coroutine blinkRoutine;
	}
}
