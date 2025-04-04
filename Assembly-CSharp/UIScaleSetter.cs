using System;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000016 RID: 22
public class UIScaleSetter : MonoBehaviour
{
	// Token: 0x0600006E RID: 110 RVA: 0x000049AE File Offset: 0x00002BAE
	private void Awake()
	{
		base.GetComponent<Slider>().onValueChanged.AddListener(delegate(float x)
		{
			ScheduleOne.UI.CanvasScaler.SetScaleFactor(x);
		});
	}
}
