using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x020002FE RID: 766
	public class StateMachine : MonoBehaviour
	{
		// Token: 0x060010FE RID: 4350 RVA: 0x0004BCE5 File Offset: 0x00049EE5
		private void Start()
		{
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(new UnityAction(this.Clean));
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0004BD02 File Offset: 0x00049F02
		private void Update()
		{
			if (StateMachine.stateChanged)
			{
				Action onStateChange = StateMachine.OnStateChange;
				if (onStateChange != null)
				{
					onStateChange();
				}
				StateMachine.stateChanged = false;
			}
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0004BD21 File Offset: 0x00049F21
		private void Clean()
		{
			Debug.Log("Clearing state change...");
			StateMachine.OnStateChange = null;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0004BD33 File Offset: 0x00049F33
		public static void ChangeState()
		{
			StateMachine.stateChanged = true;
		}

		// Token: 0x04001124 RID: 4388
		public static Action OnStateChange;

		// Token: 0x04001125 RID: 4389
		private static bool stateChanged;
	}
}
