using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E3 RID: 483
	public class WeatherEnclosureDetector : MonoBehaviour
	{
		// Token: 0x06000AC5 RID: 2757 RVA: 0x0002FC19 File Offset: 0x0002DE19
		private void Start()
		{
			this.ApplyEnclosure();
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0002FC19 File Offset: 0x0002DE19
		private void OnEnable()
		{
			this.ApplyEnclosure();
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0002FC24 File Offset: 0x0002DE24
		private void OnTriggerEnter(Collider other)
		{
			WeatherEnclosure componentInChildren = other.gameObject.GetComponentInChildren<WeatherEnclosure>();
			if (!componentInChildren)
			{
				return;
			}
			if (this.triggeredEnclosures.Contains(componentInChildren))
			{
				this.triggeredEnclosures.Remove(componentInChildren);
			}
			this.triggeredEnclosures.Add(componentInChildren);
			this.ApplyEnclosure();
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0002FC74 File Offset: 0x0002DE74
		private void OnTriggerExit(Collider other)
		{
			WeatherEnclosure componentInChildren = other.gameObject.GetComponentInChildren<WeatherEnclosure>();
			if (!componentInChildren)
			{
				return;
			}
			if (!this.triggeredEnclosures.Contains(componentInChildren))
			{
				return;
			}
			this.triggeredEnclosures.Remove(componentInChildren);
			this.ApplyEnclosure();
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0002FCB8 File Offset: 0x0002DEB8
		public void ApplyEnclosure()
		{
			WeatherEnclosure weatherEnclosure;
			if (this.triggeredEnclosures.Count > 0)
			{
				weatherEnclosure = this.triggeredEnclosures[this.triggeredEnclosures.Count - 1];
				if (!weatherEnclosure)
				{
					Debug.LogError("Failed to find mesh renderer on weather enclosure, using main enclosure instead.");
					weatherEnclosure = this.mainEnclosure;
				}
			}
			else
			{
				weatherEnclosure = this.mainEnclosure;
			}
			if (this.enclosureChangedCallback != null)
			{
				this.enclosureChangedCallback(weatherEnclosure);
			}
		}

		// Token: 0x04000BA1 RID: 2977
		[Tooltip("Default enclosure used when not inside the trigger of another enclosure area.")]
		public WeatherEnclosure mainEnclosure;

		// Token: 0x04000BA2 RID: 2978
		private List<WeatherEnclosure> triggeredEnclosures = new List<WeatherEnclosure>();

		// Token: 0x04000BA3 RID: 2979
		public RainDownfallController rainController;

		// Token: 0x04000BA4 RID: 2980
		public Action<WeatherEnclosure> enclosureChangedCallback;
	}
}
