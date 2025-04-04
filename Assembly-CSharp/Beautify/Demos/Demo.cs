using System;
using Beautify.Universal;
using UnityEngine;
using UnityEngine.UI;

namespace Beautify.Demos
{
	// Token: 0x020001EC RID: 492
	public class Demo : MonoBehaviour
	{
		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002FF37 File Offset: 0x0002E137
		private void Start()
		{
			this.UpdateText();
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0002FF40 File Offset: 0x0002E140
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.J))
			{
				BeautifySettings.settings.bloomIntensity.value += 0.1f;
			}
			if (Input.GetKeyDown(KeyCode.T) || Input.GetMouseButtonDown(0))
			{
				BeautifySettings.settings.disabled.value = !BeautifySettings.settings.disabled.value;
				this.UpdateText();
			}
			if (Input.GetKeyDown(KeyCode.B))
			{
				BeautifySettings.Blink(0.2f, 1f);
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				BeautifySettings.settings.compareMode.value = !BeautifySettings.settings.compareMode.value;
			}
			if (Input.GetKeyDown(KeyCode.N))
			{
				BeautifySettings.settings.nightVision.Override(!BeautifySettings.settings.nightVision.value);
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				if (BeautifySettings.settings.blurIntensity.overrideState)
				{
					BeautifySettings.settings.blurIntensity.overrideState = false;
				}
				else
				{
					BeautifySettings.settings.blurIntensity.Override(4f);
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				BeautifySettings.settings.brightness.Override(0.1f);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				BeautifySettings.settings.brightness.Override(0.5f);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				BeautifySettings.settings.brightness.overrideState = false;
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				BeautifySettings.settings.outline.Override(true);
				BeautifySettings.settings.outlineColor.Override(Color.cyan);
				BeautifySettings.settings.outlineCustomize.Override(true);
				BeautifySettings.settings.outlineSpread.Override(1.5f);
			}
			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				BeautifySettings.settings.outline.overrideState = false;
			}
			if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				BeautifySettings.settings.lut.Override(true);
				BeautifySettings.settings.lutIntensity.Override(1f);
				BeautifySettings.settings.lutTexture.Override(this.lutTexture);
			}
			if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				BeautifySettings.settings.lut.Override(false);
			}
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00030170 File Offset: 0x0002E370
		private void UpdateText()
		{
			if (BeautifySettings.settings.disabled.value)
			{
				GameObject.Find("Beautify").GetComponent<Text>().text = "Beautify OFF";
				return;
			}
			GameObject.Find("Beautify").GetComponent<Text>().text = "Beautify ON";
		}

		// Token: 0x04000BB5 RID: 2997
		public Texture lutTexture;
	}
}
