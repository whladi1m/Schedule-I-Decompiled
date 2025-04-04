using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005C9 RID: 1481
	public class CursorManager : Singleton<CursorManager>
	{
		// Token: 0x060024D0 RID: 9424 RVA: 0x0009456A File Offset: 0x0009276A
		protected override void Awake()
		{
			base.Awake();
			this.SetCursorAppearance(CursorManager.ECursorType.Default);
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x0009457C File Offset: 0x0009277C
		public void SetCursorAppearance(CursorManager.ECursorType type)
		{
			CursorManager.CursorConfig cursorConfig = this.Cursors.Find((CursorManager.CursorConfig x) => x.CursorType == type);
			Cursor.SetCursor(cursorConfig.Texture, cursorConfig.HotSpot, CursorMode.Auto);
		}

		// Token: 0x04001B6C RID: 7020
		[Header("References")]
		public List<CursorManager.CursorConfig> Cursors = new List<CursorManager.CursorConfig>();

		// Token: 0x020005CA RID: 1482
		public enum ECursorType
		{
			// Token: 0x04001B6E RID: 7022
			Default,
			// Token: 0x04001B6F RID: 7023
			Finger,
			// Token: 0x04001B70 RID: 7024
			OpenHand,
			// Token: 0x04001B71 RID: 7025
			Grab,
			// Token: 0x04001B72 RID: 7026
			Scissors
		}

		// Token: 0x020005CB RID: 1483
		[Serializable]
		public class CursorConfig
		{
			// Token: 0x04001B73 RID: 7027
			public CursorManager.ECursorType CursorType;

			// Token: 0x04001B74 RID: 7028
			public Texture2D Texture;

			// Token: 0x04001B75 RID: 7029
			public Vector2 HotSpot;
		}
	}
}
