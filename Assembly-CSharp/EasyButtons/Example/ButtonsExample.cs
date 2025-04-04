using System;
using UnityEngine;

namespace EasyButtons.Example
{
	// Token: 0x020001E9 RID: 489
	public class ButtonsExample : MonoBehaviour
	{
		// Token: 0x06000AD4 RID: 2772 RVA: 0x0002FE9E File Offset: 0x0002E09E
		[Button]
		public void SayMyName()
		{
			Debug.Log(base.name);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0002FEAB File Offset: 0x0002E0AB
		[Button(Mode = ButtonMode.DisabledInPlayMode)]
		protected void SayHelloEditor()
		{
			Debug.Log("Hello from edit mode");
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0002FEB7 File Offset: 0x0002E0B7
		[Button(Mode = ButtonMode.EnabledInPlayMode)]
		private void SayHelloInRuntime()
		{
			Debug.Log("Hello from play mode");
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002FEC3 File Offset: 0x0002E0C3
		[Button("Special Name", Spacing = ButtonSpacing.Before)]
		private void TestButtonName()
		{
			Debug.Log("Hello from special name button");
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002FECF File Offset: 0x0002E0CF
		[Button("Special Name Editor Only", Mode = ButtonMode.DisabledInPlayMode)]
		private void TestButtonNameEditorOnly()
		{
			Debug.Log("Hello from special name button for editor only");
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0002FEDB File Offset: 0x0002E0DB
		[Button]
		private static void TestStaticMethod()
		{
			Debug.Log("Hello from static method");
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0002FEE7 File Offset: 0x0002E0E7
		[Button("Space Before and After", Spacing = (ButtonSpacing.Before | ButtonSpacing.After))]
		private void TestButtonSpaceBoth()
		{
			Debug.Log("Hello from a button surround by spaces");
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0002FEF3 File Offset: 0x0002E0F3
		[Button("Button With Parameters")]
		private void TestButtonWithParams(string message, int number)
		{
			Debug.Log(string.Format("Your message #{0}: \"{1}\"", number, message));
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0002FF0B File Offset: 0x0002E10B
		[Button("Expanded Button Example", Expanded = true)]
		private void TestExpandedButton(string message)
		{
			Debug.Log(message);
		}
	}
}
