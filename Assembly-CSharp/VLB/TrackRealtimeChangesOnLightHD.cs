using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000110 RID: 272
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Light), typeof(VolumetricLightBeamHD))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-trackrealtimechanges-hd/")]
	public class TrackRealtimeChangesOnLightHD : MonoBehaviour
	{
		// Token: 0x06000436 RID: 1078 RVA: 0x00016E4D File Offset: 0x0001504D
		private void Awake()
		{
			this.m_Master = base.GetComponent<VolumetricLightBeamHD>();
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00016E5B File Offset: 0x0001505B
		private void Update()
		{
			if (this.m_Master.enabled)
			{
				this.m_Master.AssignPropertiesFromAttachedSpotLight();
			}
		}

		// Token: 0x040005E8 RID: 1512
		public const string ClassName = "TrackRealtimeChangesOnLightHD";

		// Token: 0x040005E9 RID: 1513
		private VolumetricLightBeamHD m_Master;
	}
}
