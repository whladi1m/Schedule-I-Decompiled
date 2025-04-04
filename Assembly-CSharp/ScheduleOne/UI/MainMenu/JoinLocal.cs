using System;
using FishNet.Managing;
using UnityEngine;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B0D RID: 2829
	public class JoinLocal : MonoBehaviour
	{
		// Token: 0x06004B86 RID: 19334 RVA: 0x0013CBF4 File Offset: 0x0013ADF4
		private void Awake()
		{
			base.gameObject.SetActive(Application.isEditor || Debug.isDebugBuild);
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x0013CC10 File Offset: 0x0013AE10
		public void Clicked()
		{
			UnityEngine.Object.FindObjectOfType<NetworkManager>().ClientManager.StartConnection();
		}
	}
}
