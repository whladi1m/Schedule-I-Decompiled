using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Impostors
{
	// Token: 0x02000958 RID: 2392
	public class ImpostorGenerator : MonoBehaviour
	{
		// Token: 0x04002F08 RID: 12040
		[Header("References")]
		public Camera ImpostorCamera;

		// Token: 0x04002F09 RID: 12041
		public Avatar Avatar;

		// Token: 0x04002F0A RID: 12042
		[Header("Settings")]
		public List<AvatarSettings> GenerationQueue = new List<AvatarSettings>();

		// Token: 0x04002F0B RID: 12043
		[SerializeField]
		private Texture2D output;
	}
}
