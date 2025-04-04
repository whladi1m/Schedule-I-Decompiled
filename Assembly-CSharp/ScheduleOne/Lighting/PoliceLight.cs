using System;
using System.Collections;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x0200059B RID: 1435
	public class PoliceLight : MonoBehaviour
	{
		// Token: 0x060023BD RID: 9149 RVA: 0x000911DF File Offset: 0x0008F3DF
		public void SetIsOn(bool isOn)
		{
			this.IsOn = isOn;
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x000911E8 File Offset: 0x0008F3E8
		private void FixedUpdate()
		{
			if (this.IsOn)
			{
				if (!this.Siren.isPlaying)
				{
					this.Siren.Play();
				}
				if (this.cycleRoutine == null)
				{
					this.cycleRoutine = base.StartCoroutine(this.CycleCoroutine());
					return;
				}
			}
			else if (this.Siren.isPlaying)
			{
				this.Siren.Stop();
			}
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x00091248 File Offset: 0x0008F448
		protected IEnumerator CycleCoroutine()
		{
			foreach (OptimizedLight optimizedLight in this.RedLights)
			{
				optimizedLight._Light.intensity = 0f;
				optimizedLight.Enabled = true;
			}
			foreach (OptimizedLight optimizedLight2 in this.BlueLights)
			{
				optimizedLight2._Light.intensity = 0f;
				optimizedLight2.Enabled = true;
			}
			float time = 0f;
			MeshRenderer[] array2;
			while (this.IsOn)
			{
				time += Time.deltaTime;
				float time2 = time / this.CycleDuration % 1f;
				float num = this.RedBrightnessCurve.Evaluate(time2);
				float num2 = this.BlueBrightnessCurve.Evaluate(time2);
				OptimizedLight[] array = this.RedLights;
				for (int i = 0; i < array.Length; i++)
				{
					array[i]._Light.intensity = num * this.LightBrightness;
				}
				array2 = this.RedMeshes;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].material = ((num > 0f) ? this.RedOnMat : this.RedOffMat);
				}
				array = this.BlueLights;
				for (int i = 0; i < array.Length; i++)
				{
					array[i]._Light.intensity = num2 * this.LightBrightness;
				}
				array2 = this.BlueMeshes;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].material = ((num2 > 0f) ? this.BlueOnMat : this.BlueOffMat);
				}
				yield return new WaitForEndOfFrame();
			}
			foreach (OptimizedLight optimizedLight3 in this.RedLights)
			{
				optimizedLight3._Light.intensity = 0f;
				optimizedLight3.Enabled = false;
			}
			array2 = this.RedMeshes;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].material = this.RedOffMat;
			}
			foreach (OptimizedLight optimizedLight4 in this.BlueLights)
			{
				optimizedLight4._Light.intensity = 0f;
				optimizedLight4.Enabled = false;
			}
			array2 = this.BlueMeshes;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].material = this.BlueOffMat;
			}
			this.cycleRoutine = null;
			yield break;
		}

		// Token: 0x04001AA8 RID: 6824
		public bool IsOn;

		// Token: 0x04001AA9 RID: 6825
		[Header("References")]
		public MeshRenderer[] RedMeshes;

		// Token: 0x04001AAA RID: 6826
		public MeshRenderer[] BlueMeshes;

		// Token: 0x04001AAB RID: 6827
		public OptimizedLight[] RedLights;

		// Token: 0x04001AAC RID: 6828
		public OptimizedLight[] BlueLights;

		// Token: 0x04001AAD RID: 6829
		public AudioSourceController Siren;

		// Token: 0x04001AAE RID: 6830
		[Header("Settings")]
		public float CycleDuration = 0.5f;

		// Token: 0x04001AAF RID: 6831
		public Material RedOffMat;

		// Token: 0x04001AB0 RID: 6832
		public Material RedOnMat;

		// Token: 0x04001AB1 RID: 6833
		public Material BlueOffMat;

		// Token: 0x04001AB2 RID: 6834
		public Material BlueOnMat;

		// Token: 0x04001AB3 RID: 6835
		public AnimationCurve RedBrightnessCurve;

		// Token: 0x04001AB4 RID: 6836
		public AnimationCurve BlueBrightnessCurve;

		// Token: 0x04001AB5 RID: 6837
		public float LightBrightness = 5f;

		// Token: 0x04001AB6 RID: 6838
		private Coroutine cycleRoutine;
	}
}
