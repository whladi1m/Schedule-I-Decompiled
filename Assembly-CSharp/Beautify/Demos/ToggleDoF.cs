using System;
using Beautify.Universal;
using UnityEngine;

namespace Beautify.Demos
{
	// Token: 0x020001ED RID: 493
	public class ToggleDoF : MonoBehaviour
	{
		// Token: 0x06000AE9 RID: 2793 RVA: 0x000301C4 File Offset: 0x0002E3C4
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				bool value = BeautifySettings.settings.depthOfField.value;
				BeautifySettings.settings.depthOfField.Override(!value);
			}
		}
	}
}
