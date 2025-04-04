using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000014 RID: 20
public class SensitivitySetter : MonoBehaviour
{
	// Token: 0x06000069 RID: 105 RVA: 0x00004964 File Offset: 0x00002B64
	private void Awake()
	{
		base.GetComponent<Slider>().onValueChanged.AddListener(delegate(float x)
		{
			Singleton<Settings>.Instance.LookSensitivity = x;
		});
	}
}
