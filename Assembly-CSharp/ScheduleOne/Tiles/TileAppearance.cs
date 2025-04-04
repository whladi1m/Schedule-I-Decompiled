using System;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002C1 RID: 705
	public class TileAppearance : MonoBehaviour
	{
		// Token: 0x06000F0B RID: 3851 RVA: 0x000423E0 File Offset: 0x000405E0
		public void Awake()
		{
			this.SetVisible(false);
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x000423E9 File Offset: 0x000405E9
		public void SetVisible(bool visible)
		{
			this.tileMesh.enabled = visible;
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x000423F8 File Offset: 0x000405F8
		public void SetColor(ETileColor col)
		{
			Material material = this.mat_White;
			switch (col)
			{
			case ETileColor.White:
				material = this.mat_White;
				break;
			case ETileColor.Blue:
				material = this.mat_Blue;
				break;
			case ETileColor.Red:
				material = this.mat_Red;
				break;
			default:
				Console.LogWarning("GridUnitAppearance: enum type not accounted for.", null);
				break;
			}
			this.tileMesh.material = material;
		}

		// Token: 0x04000F71 RID: 3953
		[Header("References")]
		[SerializeField]
		protected MeshRenderer tileMesh;

		// Token: 0x04000F72 RID: 3954
		[Header("Settings")]
		[SerializeField]
		protected Material mat_White;

		// Token: 0x04000F73 RID: 3955
		[SerializeField]
		protected Material mat_Blue;

		// Token: 0x04000F74 RID: 3956
		[SerializeField]
		protected Material mat_Red;
	}
}
