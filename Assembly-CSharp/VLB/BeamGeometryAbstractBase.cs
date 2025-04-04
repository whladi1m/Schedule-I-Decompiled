using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x020000DE RID: 222
	public abstract class BeamGeometryAbstractBase : MonoBehaviour
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600039F RID: 927 RVA: 0x000151F5 File Offset: 0x000133F5
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x000151FD File Offset: 0x000133FD
		public MeshRenderer meshRenderer { get; protected set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00015206 File Offset: 0x00013406
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x0001520E File Offset: 0x0001340E
		public MeshFilter meshFilter { get; protected set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00015217 File Offset: 0x00013417
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x0001521F File Offset: 0x0001341F
		public Mesh coneMesh { get; protected set; }

		// Token: 0x060003A5 RID: 933
		protected abstract VolumetricLightBeamAbstractBase GetMaster();

		// Token: 0x060003A6 RID: 934 RVA: 0x00015228 File Offset: 0x00013428
		private void Start()
		{
			this.DestroyInvalidOwner();
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00015230 File Offset: 0x00013430
		private void OnDestroy()
		{
			if (this.m_CustomMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.m_CustomMaterial);
				this.m_CustomMaterial = null;
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00015251 File Offset: 0x00013451
		private void DestroyInvalidOwner()
		{
			if (!this.GetMaster())
			{
				BeamGeometryAbstractBase.DestroyBeamGeometryGameObject(this);
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00015266 File Offset: 0x00013466
		public static void DestroyBeamGeometryGameObject(BeamGeometryAbstractBase beamGeom)
		{
			if (beamGeom)
			{
				UnityEngine.Object.DestroyImmediate(beamGeom.gameObject);
			}
		}

		// Token: 0x04000483 RID: 1155
		protected Matrix4x4 m_ColorGradientMatrix;

		// Token: 0x04000484 RID: 1156
		protected Material m_CustomMaterial;
	}
}
