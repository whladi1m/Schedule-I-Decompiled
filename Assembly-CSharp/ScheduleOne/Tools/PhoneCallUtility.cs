using System;
using ScheduleOne.Calling;
using ScheduleOne.DevUtilities;
using ScheduleOne.ScriptableObjects;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000850 RID: 2128
	public class PhoneCallUtility : MonoBehaviour
	{
		// Token: 0x06003A16 RID: 14870 RVA: 0x000F4A9B File Offset: 0x000F2C9B
		public void PromptCall(PhoneCallData callData)
		{
			Singleton<CallManager>.Instance.QueueCall(callData);
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x000F4A9B File Offset: 0x000F2C9B
		public void StartCall(PhoneCallData callData)
		{
			Singleton<CallManager>.Instance.QueueCall(callData);
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x000F4A9B File Offset: 0x000F2C9B
		public void SetQueuedCall(PhoneCallData callData)
		{
			Singleton<CallManager>.Instance.QueueCall(callData);
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x000F4AA8 File Offset: 0x000F2CA8
		public void ClearCall()
		{
			Singleton<CallManager>.Instance.ClearQueuedCall();
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x000045B1 File Offset: 0x000027B1
		public void SetPhoneOpenable(bool openable)
		{
		}
	}
}
