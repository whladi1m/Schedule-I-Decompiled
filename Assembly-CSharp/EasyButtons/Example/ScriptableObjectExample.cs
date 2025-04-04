using System;
using UnityEngine;

namespace EasyButtons.Example
{
	// Token: 0x020001EB RID: 491
	[CreateAssetMenu(fileName = "ScriptableObjectExample.asset", menuName = "EasyButtons/ScriptableObjectExample")]
	public class ScriptableObjectExample : ScriptableObject
	{
		// Token: 0x06000AE1 RID: 2785 RVA: 0x0002FF2B File Offset: 0x0002E12B
		[Button]
		public void SayHello()
		{
			Debug.Log("Hello");
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002FEAB File Offset: 0x0002E0AB
		[Button(Mode = ButtonMode.DisabledInPlayMode)]
		public void SayHelloEditor()
		{
			Debug.Log("Hello from edit mode");
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0002FEB7 File Offset: 0x0002E0B7
		[Button(Mode = ButtonMode.EnabledInPlayMode)]
		public void SayHelloPlayMode()
		{
			Debug.Log("Hello from play mode");
		}
	}
}
