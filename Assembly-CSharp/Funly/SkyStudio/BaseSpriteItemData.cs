using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001D7 RID: 471
	public class BaseSpriteItemData
	{
		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0002E45F File Offset: 0x0002C65F
		// (set) Token: 0x06000A69 RID: 2665 RVA: 0x0002E467 File Offset: 0x0002C667
		public Matrix4x4 modelMatrix { get; protected set; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0002E470 File Offset: 0x0002C670
		// (set) Token: 0x06000A6B RID: 2667 RVA: 0x0002E478 File Offset: 0x0002C678
		public BaseSpriteItemData.SpriteState state { get; protected set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0002E481 File Offset: 0x0002C681
		// (set) Token: 0x06000A6D RID: 2669 RVA: 0x0002E489 File Offset: 0x0002C689
		public Vector3 spritePosition { get; set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0002E492 File Offset: 0x0002C692
		// (set) Token: 0x06000A6F RID: 2671 RVA: 0x0002E49A File Offset: 0x0002C69A
		public float startTime { get; protected set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0002E4A3 File Offset: 0x0002C6A3
		// (set) Token: 0x06000A71 RID: 2673 RVA: 0x0002E4AB File Offset: 0x0002C6AB
		public float endTime { get; protected set; }

		// Token: 0x06000A72 RID: 2674 RVA: 0x0002E4B4 File Offset: 0x0002C6B4
		public BaseSpriteItemData()
		{
			this.state = BaseSpriteItemData.SpriteState.NotStarted;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0002E4C3 File Offset: 0x0002C6C3
		public void SetTRSMatrix(Vector3 worldPosition, Quaternion rotation, Vector3 scale)
		{
			this.spritePosition = worldPosition;
			this.modelMatrix = Matrix4x4.TRS(worldPosition, rotation, scale);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0002E4DC File Offset: 0x0002C6DC
		public void Start()
		{
			this.state = BaseSpriteItemData.SpriteState.Animating;
			this.startTime = BaseSpriteItemData.CalculateStartTimeWithDelay(this.delay);
			this.endTime = BaseSpriteItemData.CalculateEndTime(this.startTime, this.spriteSheetData.frameCount, this.spriteSheetData.frameRate);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0002E528 File Offset: 0x0002C728
		public void Continue()
		{
			if (this.state != BaseSpriteItemData.SpriteState.Animating)
			{
				return;
			}
			if (Time.time > this.endTime)
			{
				this.state = BaseSpriteItemData.SpriteState.Complete;
				return;
			}
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0002E549 File Offset: 0x0002C749
		public void Reset()
		{
			this.state = BaseSpriteItemData.SpriteState.NotStarted;
			this.startTime = -1f;
			this.endTime = -1f;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0002E568 File Offset: 0x0002C768
		public static float CalculateStartTimeWithDelay(float delay)
		{
			return Time.time + delay;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0002E574 File Offset: 0x0002C774
		public static float CalculateEndTime(float startTime, int itemCount, int animationSpeed)
		{
			float num = 1f / (float)animationSpeed;
			float num2 = (float)itemCount * num;
			return startTime + num2;
		}

		// Token: 0x04000B5E RID: 2910
		public SpriteSheetData spriteSheetData;

		// Token: 0x04000B64 RID: 2916
		public float delay;

		// Token: 0x020001D8 RID: 472
		public enum SpriteState
		{
			// Token: 0x04000B66 RID: 2918
			Unknown,
			// Token: 0x04000B67 RID: 2919
			NotStarted,
			// Token: 0x04000B68 RID: 2920
			Animating,
			// Token: 0x04000B69 RID: 2921
			Complete
		}
	}
}
