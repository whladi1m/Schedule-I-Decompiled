using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace RadiantGI.Universal
{
	// Token: 0x02000167 RID: 359
	public class ToggleEffect : MonoBehaviour
	{
		// Token: 0x060006DD RID: 1757 RVA: 0x0001F4C4 File Offset: 0x0001D6C4
		private void Start()
		{
			this.profile.TryGet<RadiantGlobalIllumination>(out this.radiant);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001F4D8 File Offset: 0x0001D6D8
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.radiant.active = !this.radiant.active;
			}
		}

		// Token: 0x040007BD RID: 1981
		public VolumeProfile profile;

		// Token: 0x040007BE RID: 1982
		private RadiantGlobalIllumination radiant;
	}
}
