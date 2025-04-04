using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class OscillateLightBrightness : MonoBehaviour
{
	// Token: 0x06000226 RID: 550 RVA: 0x0000CCD8 File Offset: 0x0000AED8
	private void Start()
	{
		this.lightComponent = base.GetComponent<Light>();
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000CCE6 File Offset: 0x0000AEE6
	private void Update()
	{
		this.lightComponent.intensity = UnityEngine.Random.Range(this.lower, this.upper);
	}

	// Token: 0x04000251 RID: 593
	private Light lightComponent;

	// Token: 0x04000252 RID: 594
	[SerializeField]
	[Range(0f, 10f)]
	private float lower;

	// Token: 0x04000253 RID: 595
	[SerializeField]
	[Range(0f, 10f)]
	private float upper;
}
