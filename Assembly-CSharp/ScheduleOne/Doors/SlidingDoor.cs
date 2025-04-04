using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x0200067D RID: 1661
	public class SlidingDoor : MonoBehaviour
	{
		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002E12 RID: 11794 RVA: 0x000C1535 File Offset: 0x000BF735
		// (set) Token: 0x06002E13 RID: 11795 RVA: 0x000C153D File Offset: 0x000BF73D
		public bool IsOpen { get; protected set; }

		// Token: 0x06002E14 RID: 11796 RVA: 0x000C1546 File Offset: 0x000BF746
		public virtual void Opened(EDoorSide openSide)
		{
			this.IsOpen = true;
			this.Move();
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000C1555 File Offset: 0x000BF755
		public virtual void Closed()
		{
			this.IsOpen = false;
			this.Move();
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x000C1564 File Offset: 0x000BF764
		private void Move()
		{
			if (this.MoveRoutine != null)
			{
				base.StopCoroutine(this.MoveRoutine);
			}
			this.MoveRoutine = base.StartCoroutine(this.<Move>g__Move|12_0());
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x000C159F File Offset: 0x000BF79F
		[CompilerGenerated]
		private IEnumerator <Move>g__Move|12_0()
		{
			Vector3 start = this.DoorTransform.position;
			Vector3 end = this.IsOpen ? this.OpenPosition.position : this.ClosedPosition.position;
			for (float i = 0f; i < this.SlideDuration; i += Time.deltaTime)
			{
				this.DoorTransform.position = Vector3.Lerp(start, end, this.SlideCurve.Evaluate(i / this.SlideDuration));
				yield return new WaitForEndOfFrame();
			}
			this.DoorTransform.position = end;
			this.MoveRoutine = null;
			yield break;
		}

		// Token: 0x040020D8 RID: 8408
		[Header("Settings")]
		public Transform DoorTransform;

		// Token: 0x040020D9 RID: 8409
		public Transform ClosedPosition;

		// Token: 0x040020DA RID: 8410
		public Transform OpenPosition;

		// Token: 0x040020DB RID: 8411
		public float SlideDuration = 3f;

		// Token: 0x040020DC RID: 8412
		public AnimationCurve SlideCurve;

		// Token: 0x040020DD RID: 8413
		private Coroutine MoveRoutine;
	}
}
