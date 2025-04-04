using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class TestSetup : MonoBehaviour
{
	// Token: 0x0600009B RID: 155 RVA: 0x00005329 File Offset: 0x00003529
	private void Start()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 3000;
		Screen.SetResolution(1920, 1080, true);
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0000534C File Offset: 0x0000354C
	private void Update()
	{
		Debug.Log(1f / Time.deltaTime);
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 3000;
		if (Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log("A");
			Screen.SetResolution(1920, 1080, true);
		}
	}
}
