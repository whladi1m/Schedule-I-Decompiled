using System;
using System.Collections;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000141 RID: 321
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-skewinghandle-sd/")]
	public class SkewingHandleSD : MonoBehaviour
	{
		// Token: 0x060005C4 RID: 1476 RVA: 0x0001B51E File Offset: 0x0001971E
		public bool IsAttachedToSelf()
		{
			return this.volumetricLightBeam != null && this.volumetricLightBeam.gameObject == base.gameObject;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001B546 File Offset: 0x00019746
		public bool CanSetSkewingVector()
		{
			return this.volumetricLightBeam != null && this.volumetricLightBeam.canHaveMeshSkewing;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001B563 File Offset: 0x00019763
		public bool CanUpdateEachFrame()
		{
			return this.CanSetSkewingVector() && this.volumetricLightBeam.trackChangesDuringPlaytime;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001B57A File Offset: 0x0001977A
		private bool ShouldUpdateEachFrame()
		{
			return this.shouldUpdateEachFrame && this.CanUpdateEachFrame();
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001B58C File Offset: 0x0001978C
		private void OnEnable()
		{
			if (this.CanSetSkewingVector())
			{
				this.SetSkewingVector();
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001B59C File Offset: 0x0001979C
		private void Start()
		{
			if (Application.isPlaying && this.ShouldUpdateEachFrame())
			{
				base.StartCoroutine(this.CoUpdate());
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001B5BA File Offset: 0x000197BA
		private IEnumerator CoUpdate()
		{
			while (this.ShouldUpdateEachFrame())
			{
				this.SetSkewingVector();
				yield return null;
			}
			yield break;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001B5CC File Offset: 0x000197CC
		private void SetSkewingVector()
		{
			Vector3 skewingLocalForwardDirection = this.volumetricLightBeam.transform.InverseTransformPoint(base.transform.position);
			this.volumetricLightBeam.skewingLocalForwardDirection = skewingLocalForwardDirection;
		}

		// Token: 0x040006BE RID: 1726
		public const string ClassName = "SkewingHandleSD";

		// Token: 0x040006BF RID: 1727
		public VolumetricLightBeamSD volumetricLightBeam;

		// Token: 0x040006C0 RID: 1728
		public bool shouldUpdateEachFrame;
	}
}
