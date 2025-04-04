using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000012 RID: 18
public class InvertMouseSetter : MonoBehaviour
{
	// Token: 0x06000064 RID: 100 RVA: 0x00004912 File Offset: 0x00002B12
	private void Awake()
	{
		base.GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool x)
		{
			Singleton<Settings>.Instance.InvertMouse = x;
		});
	}
}
