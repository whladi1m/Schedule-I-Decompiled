using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000159 RID: 345
	public abstract class VolumetricLightBeamAbstractBase : MonoBehaviour
	{
		// Token: 0x060006AB RID: 1707
		public abstract BeamGeometryAbstractBase GetBeamGeometry();

		// Token: 0x060006AC RID: 1708
		protected abstract void SetBeamGeometryNull();

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001E1B8 File Offset: 0x0001C3B8
		public bool hasGeometry
		{
			get
			{
				return this.GetBeamGeometry() != null;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0001E1C6 File Offset: 0x0001C3C6
		public Bounds bounds
		{
			get
			{
				if (!(this.GetBeamGeometry() != null))
				{
					return new Bounds(Vector3.zero, Vector3.zero);
				}
				return this.GetBeamGeometry().meshRenderer.bounds;
			}
		}

		// Token: 0x060006AF RID: 1711
		public abstract bool IsScalable();

		// Token: 0x060006B0 RID: 1712
		public abstract Vector3 GetLossyScale();

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001E1F6 File Offset: 0x0001C3F6
		public int _INTERNAL_pluginVersion
		{
			get
			{
				return this.pluginVersion;
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0001E200 File Offset: 0x0001C400
		public Light GetLightSpotAttachedSlow(out VolumetricLightBeamAbstractBase.AttachedLightType lightType)
		{
			Light component = base.GetComponent<Light>();
			if (!component)
			{
				lightType = VolumetricLightBeamAbstractBase.AttachedLightType.NoLight;
				return null;
			}
			if (component.type == LightType.Spot)
			{
				lightType = VolumetricLightBeamAbstractBase.AttachedLightType.SpotLight;
				return component;
			}
			lightType = VolumetricLightBeamAbstractBase.AttachedLightType.OtherLight;
			return null;
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0001E232 File Offset: 0x0001C432
		public Light lightSpotAttached
		{
			get
			{
				return this.m_CachedLightSpot;
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001E23C File Offset: 0x0001C43C
		protected void InitLightSpotAttachedCached()
		{
			VolumetricLightBeamAbstractBase.AttachedLightType attachedLightType;
			this.m_CachedLightSpot = this.GetLightSpotAttachedSlow(out attachedLightType);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001E257 File Offset: 0x0001C457
		private void OnDestroy()
		{
			this.DestroyBeam();
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001E25F File Offset: 0x0001C45F
		protected void DestroyBeam()
		{
			if (Application.isPlaying)
			{
				BeamGeometryAbstractBase.DestroyBeamGeometryGameObject(this.GetBeamGeometry());
			}
			this.SetBeamGeometryNull();
		}

		// Token: 0x0400076E RID: 1902
		public const string ClassName = "VolumetricLightBeamAbstractBase";

		// Token: 0x0400076F RID: 1903
		[SerializeField]
		protected int pluginVersion = -1;

		// Token: 0x04000770 RID: 1904
		protected Light m_CachedLightSpot;

		// Token: 0x0200015A RID: 346
		public enum AttachedLightType
		{
			// Token: 0x04000772 RID: 1906
			NoLight,
			// Token: 0x04000773 RID: 1907
			OtherLight,
			// Token: 0x04000774 RID: 1908
			SpotLight
		}
	}
}
