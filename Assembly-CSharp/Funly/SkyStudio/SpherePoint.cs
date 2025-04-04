using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001D3 RID: 467
	[Serializable]
	public class SpherePoint
	{
		// Token: 0x06000A3A RID: 2618 RVA: 0x0002DB11 File Offset: 0x0002BD11
		public SpherePoint(float horizontalRotation, float verticalRotation)
		{
			this.horizontalRotation = horizontalRotation;
			this.verticalRotation = verticalRotation;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0002DB28 File Offset: 0x0002BD28
		public SpherePoint(Vector3 worldDirection)
		{
			Vector2 vector = SphereUtility.DirectionToSphericalCoordinate(worldDirection);
			this.horizontalRotation = vector.x;
			this.verticalRotation = vector.y;
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0002DB5C File Offset: 0x0002BD5C
		public void SetFromWorldDirection(Vector3 worldDirection)
		{
			Vector2 vector = SphereUtility.DirectionToSphericalCoordinate(worldDirection);
			this.horizontalRotation = vector.x;
			this.verticalRotation = vector.y;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0002DB88 File Offset: 0x0002BD88
		public Vector3 GetWorldDirection()
		{
			return SphereUtility.SphericalCoordinateToDirection(new Vector2(this.horizontalRotation, this.verticalRotation));
		}

		// Token: 0x04000B44 RID: 2884
		public float horizontalRotation;

		// Token: 0x04000B45 RID: 2885
		public float verticalRotation;

		// Token: 0x04000B46 RID: 2886
		public const float MinHorizontalRotation = -3.1415927f;

		// Token: 0x04000B47 RID: 2887
		public const float MaxHorizontalRotation = 3.1415927f;

		// Token: 0x04000B48 RID: 2888
		public const float MinVerticalRotation = -1.5707964f;

		// Token: 0x04000B49 RID: 2889
		public const float MaxVerticalRotation = 1.5707964f;
	}
}
