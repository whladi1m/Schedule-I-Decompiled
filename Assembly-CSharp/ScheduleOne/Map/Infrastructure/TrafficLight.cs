using System;
using UnityEngine;

namespace ScheduleOne.Map.Infrastructure
{
	// Token: 0x02000C0D RID: 3085
	public class TrafficLight : MonoBehaviour
	{
		// Token: 0x06005633 RID: 22067 RVA: 0x00169F10 File Offset: 0x00168110
		protected virtual void Start()
		{
			this.ApplyState();
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x00169F18 File Offset: 0x00168118
		protected virtual void Update()
		{
			if (this.appliedState != this.state)
			{
				this.ApplyState();
			}
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x00169F30 File Offset: 0x00168130
		protected virtual void ApplyState()
		{
			this.appliedState = this.state;
			this.redMesh.material = this.redOff_Mat;
			this.orangeMesh.material = this.orangeOff_Mat;
			this.greenMesh.material = this.greenOff_Mat;
			switch (this.state)
			{
			case TrafficLight.State.Red:
				this.redMesh.material = this.redOn_Mat;
				return;
			case TrafficLight.State.Orange:
				this.orangeMesh.material = this.orangeOn_Mat;
				return;
			case TrafficLight.State.Green:
				this.greenMesh.material = this.greenOn_Mat;
				return;
			default:
				return;
			}
		}

		// Token: 0x0400401F RID: 16415
		public static float amberTime = 3f;

		// Token: 0x04004020 RID: 16416
		[Header("References")]
		[SerializeField]
		protected MeshRenderer redMesh;

		// Token: 0x04004021 RID: 16417
		[SerializeField]
		protected MeshRenderer orangeMesh;

		// Token: 0x04004022 RID: 16418
		[SerializeField]
		protected MeshRenderer greenMesh;

		// Token: 0x04004023 RID: 16419
		[Header("Materials")]
		[SerializeField]
		protected Material redOn_Mat;

		// Token: 0x04004024 RID: 16420
		[SerializeField]
		protected Material redOff_Mat;

		// Token: 0x04004025 RID: 16421
		[SerializeField]
		protected Material orangeOn_Mat;

		// Token: 0x04004026 RID: 16422
		[SerializeField]
		protected Material orangeOff_Mat;

		// Token: 0x04004027 RID: 16423
		[SerializeField]
		protected Material greenOn_Mat;

		// Token: 0x04004028 RID: 16424
		[SerializeField]
		protected Material greenOff_Mat;

		// Token: 0x04004029 RID: 16425
		[Header("Settings")]
		public TrafficLight.State state;

		// Token: 0x0400402A RID: 16426
		private TrafficLight.State appliedState;

		// Token: 0x02000C0E RID: 3086
		public enum State
		{
			// Token: 0x0400402C RID: 16428
			Red,
			// Token: 0x0400402D RID: 16429
			Orange,
			// Token: 0x0400402E RID: 16430
			Green
		}
	}
}
