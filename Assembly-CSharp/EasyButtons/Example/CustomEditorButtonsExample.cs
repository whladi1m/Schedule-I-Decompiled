using System;
using UnityEngine;

namespace EasyButtons.Example
{
	// Token: 0x020001EA RID: 490
	public class CustomEditorButtonsExample : MonoBehaviour
	{
		// Token: 0x06000ADE RID: 2782 RVA: 0x0002FF13 File Offset: 0x0002E113
		[Button("Custom Editor Example")]
		private void SayHello()
		{
			Debug.Log("Hello from custom editor");
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002FF1F File Offset: 0x0002E11F
		[Button]
		private void SecondButton()
		{
			Debug.Log("Second button of the custom editor.");
		}
	}
}
