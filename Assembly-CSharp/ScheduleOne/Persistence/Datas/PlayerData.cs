using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200041C RID: 1052
	[Serializable]
	public class PlayerData : SaveData
	{
		// Token: 0x0600157C RID: 5500 RVA: 0x0005F86D File Offset: 0x0005DA6D
		public PlayerData(string playerCode, Vector3 playerPos, float playerRot, bool introCompleted)
		{
			this.PlayerCode = playerCode;
			this.Position = playerPos;
			this.Rotation = playerRot;
			this.IntroCompleted = introCompleted;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0005F89D File Offset: 0x0005DA9D
		public PlayerData()
		{
		}

		// Token: 0x04001427 RID: 5159
		public string PlayerCode;

		// Token: 0x04001428 RID: 5160
		public Vector3 Position = Vector3.zero;

		// Token: 0x04001429 RID: 5161
		public float Rotation;

		// Token: 0x0400142A RID: 5162
		public bool IntroCompleted;
	}
}
