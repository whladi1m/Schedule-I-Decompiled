using System;
using UnityEngine;

namespace ScheduleOne.Noise
{
	// Token: 0x0200052D RID: 1325
	public class NoiseEvent
	{
		// Token: 0x06002080 RID: 8320 RVA: 0x000859BB File Offset: 0x00083BBB
		public NoiseEvent(Vector3 _origin, float _range, ENoiseType _type, GameObject _source = null)
		{
			this.origin = _origin;
			this.range = _range;
			this.type = _type;
			this.source = _source;
		}

		// Token: 0x0400191F RID: 6431
		public Vector3 origin;

		// Token: 0x04001920 RID: 6432
		public float range;

		// Token: 0x04001921 RID: 6433
		public ENoiseType type;

		// Token: 0x04001922 RID: 6434
		public GameObject source;
	}
}
