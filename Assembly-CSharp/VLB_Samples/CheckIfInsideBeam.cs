using System;
using UnityEngine;
using VLB;

namespace VLB_Samples
{
	// Token: 0x0200015C RID: 348
	[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(MeshRenderer))]
	public class CheckIfInsideBeam : MonoBehaviour
	{
		// Token: 0x060006BA RID: 1722 RVA: 0x0001E2F4 File Offset: 0x0001C4F4
		private void Start()
		{
			this.m_Collider = base.GetComponent<Collider>();
			MeshRenderer component = base.GetComponent<MeshRenderer>();
			if (component)
			{
				this.m_Material = component.material;
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0001E328 File Offset: 0x0001C528
		private void Update()
		{
			if (this.m_Material)
			{
				this.m_Material.SetColor("_Color", this.isInsideBeam ? Color.green : Color.red);
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001E35B File Offset: 0x0001C55B
		private void FixedUpdate()
		{
			this.isInsideBeam = false;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001E364 File Offset: 0x0001C564
		private void OnTriggerStay(Collider trigger)
		{
			DynamicOcclusionRaycasting component = trigger.GetComponent<DynamicOcclusionRaycasting>();
			if (component)
			{
				this.isInsideBeam = !component.IsColliderHiddenByDynamicOccluder(this.m_Collider);
				return;
			}
			this.isInsideBeam = true;
		}

		// Token: 0x04000776 RID: 1910
		private bool isInsideBeam;

		// Token: 0x04000777 RID: 1911
		private Material m_Material;

		// Token: 0x04000778 RID: 1912
		private Collider m_Collider;
	}
}
