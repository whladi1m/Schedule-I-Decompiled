using System;
using HSVPicker;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000019 RID: 25
public class SColorPicker : HSVPicker.ColorPicker
{
	// Token: 0x0600007B RID: 123 RVA: 0x00004EF9 File Offset: 0x000030F9
	private void Start()
	{
		this.onValueChanged.AddListener(new UnityAction<Color>(this.ValueChanged));
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00004F12 File Offset: 0x00003112
	private void ValueChanged(Color col)
	{
		this.onValueChangeWithIndex.Invoke(col, this.PropertyIndex);
	}

	// Token: 0x0400007A RID: 122
	public int PropertyIndex;

	// Token: 0x0400007B RID: 123
	public UnityEvent<Color, int> onValueChangeWithIndex;
}
