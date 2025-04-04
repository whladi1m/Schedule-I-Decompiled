using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x02000184 RID: 388
	[ExecuteInEditMode]
	public class OrbitingBody : MonoBehaviour
	{
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x000256D2 File Offset: 0x000238D2
		public Transform positionTransform
		{
			get
			{
				if (this.m_PositionTransform == null)
				{
					this.m_PositionTransform = base.transform.Find("Position");
				}
				return this.m_PositionTransform;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x00025700 File Offset: 0x00023900
		public RotateBody rotateBody
		{
			get
			{
				if (this.m_RotateBody == null)
				{
					Transform positionTransform = this.positionTransform;
					if (!positionTransform)
					{
						Debug.LogError("Can't return rotation body without a position transform game object");
						return null;
					}
					this.m_RotateBody = positionTransform.GetComponent<RotateBody>();
				}
				return this.m_RotateBody;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00025748 File Offset: 0x00023948
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x00025750 File Offset: 0x00023950
		public SpherePoint Point
		{
			get
			{
				return this.m_SpherePoint;
			}
			set
			{
				if (this.m_SpherePoint == null)
				{
					this.m_SpherePoint = new SpherePoint(0f, 0f);
				}
				else
				{
					this.m_SpherePoint = value;
				}
				this.m_CachedWorldDirection = this.m_SpherePoint.GetWorldDirection();
				this.LayoutOribit();
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0002578F File Offset: 0x0002398F
		public Vector3 BodyGlobalDirection
		{
			get
			{
				return this.m_CachedWorldDirection;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x00025798 File Offset: 0x00023998
		public Light BodyLight
		{
			get
			{
				if (this.m_BodyLight == null)
				{
					this.m_BodyLight = base.transform.GetComponentInChildren<Light>();
					if (this.m_BodyLight != null)
					{
						this.m_BodyLight.transform.localRotation = Quaternion.identity;
					}
				}
				return this.m_BodyLight;
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x000257ED File Offset: 0x000239ED
		public void ResetOrbit()
		{
			this.LayoutOribit();
			this.m_PositionTransform = null;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x000257FC File Offset: 0x000239FC
		public void LayoutOribit()
		{
			base.transform.position = Vector3.zero;
			base.transform.rotation = Quaternion.identity;
			base.transform.forward = this.BodyGlobalDirection * -1f;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00025839 File Offset: 0x00023A39
		private void OnValidate()
		{
			this.LayoutOribit();
		}

		// Token: 0x0400090F RID: 2319
		private Transform m_PositionTransform;

		// Token: 0x04000910 RID: 2320
		private RotateBody m_RotateBody;

		// Token: 0x04000911 RID: 2321
		private SpherePoint m_SpherePoint = new SpherePoint(0f, 0f);

		// Token: 0x04000912 RID: 2322
		private Vector3 m_CachedWorldDirection = Vector3.right;

		// Token: 0x04000913 RID: 2323
		private Light m_BodyLight;
	}
}
