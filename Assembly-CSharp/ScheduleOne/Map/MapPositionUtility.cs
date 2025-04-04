using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BF3 RID: 3059
	public class MapPositionUtility : Singleton<MapPositionUtility>
	{
		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x060055A4 RID: 21924 RVA: 0x001682A0 File Offset: 0x001664A0
		// (set) Token: 0x060055A5 RID: 21925 RVA: 0x001682A8 File Offset: 0x001664A8
		private float conversionFactor { get; set; }

		// Token: 0x060055A6 RID: 21926 RVA: 0x001682B1 File Offset: 0x001664B1
		protected override void Awake()
		{
			base.Awake();
			this.Recalculate();
		}

		// Token: 0x060055A7 RID: 21927 RVA: 0x001682BF File Offset: 0x001664BF
		public Vector2 GetMapPosition(Vector3 worldPosition)
		{
			return new Vector2(worldPosition.x - this.OriginPoint.position.x, worldPosition.z - this.OriginPoint.position.z) * this.conversionFactor;
		}

		// Token: 0x060055A8 RID: 21928 RVA: 0x001682FF File Offset: 0x001664FF
		[Button]
		public void Recalculate()
		{
			this.conversionFactor = this.MapDimensions * 0.5f / Vector3.Distance(this.OriginPoint.position, this.EdgePoint.position);
		}

		// Token: 0x04003F9F RID: 16287
		public Transform OriginPoint;

		// Token: 0x04003FA0 RID: 16288
		public Transform EdgePoint;

		// Token: 0x04003FA1 RID: 16289
		public float MapDimensions = 2048f;
	}
}
