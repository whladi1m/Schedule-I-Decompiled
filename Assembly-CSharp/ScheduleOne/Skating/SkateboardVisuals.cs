using System;
using UnityEngine;

namespace ScheduleOne.Skating
{
	// Token: 0x020002D1 RID: 721
	[RequireComponent(typeof(Skateboard))]
	public class SkateboardVisuals : MonoBehaviour
	{
		// Token: 0x06000F95 RID: 3989 RVA: 0x000455F8 File Offset: 0x000437F8
		private void Awake()
		{
			this.skateboard = base.GetComponent<Skateboard>();
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00045608 File Offset: 0x00043808
		private void LateUpdate()
		{
			Vector3 euler = new Vector3(0f, 0f, this.skateboard.CurrentSteerInput * -this.MaxBoardLean);
			this.Board.localRotation = Quaternion.Lerp(this.Board.localRotation, Quaternion.Euler(euler), Time.deltaTime * this.BoardLeanRate);
		}

		// Token: 0x04001041 RID: 4161
		[Header("Settings")]
		public float MaxBoardLean = 8f;

		// Token: 0x04001042 RID: 4162
		public float BoardLeanRate = 2f;

		// Token: 0x04001043 RID: 4163
		[Header("References")]
		public Transform Board;

		// Token: 0x04001044 RID: 4164
		private Skateboard skateboard;
	}
}
