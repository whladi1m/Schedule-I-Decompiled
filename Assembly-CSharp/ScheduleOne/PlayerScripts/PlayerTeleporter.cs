using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005F5 RID: 1525
	public class PlayerTeleporter : MonoBehaviour
	{
		// Token: 0x06002808 RID: 10248 RVA: 0x000A4954 File Offset: 0x000A2B54
		public void Teleport(Transform destination)
		{
			PlayerSingleton<PlayerMovement>.Instance.Teleport(destination.position);
			Player.Local.transform.rotation = destination.rotation;
			Player.Local.transform.eulerAngles = new Vector3(0f, Player.Local.transform.eulerAngles.y, 0f);
		}
	}
}
