using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x0200013D RID: 317
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-dynocclusion-sd-raycasting/")]
	public class DynamicOcclusionRaycasting : DynamicOcclusionAbstractBase
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0001AC79 File Offset: 0x00018E79
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x0001AC81 File Offset: 0x00018E81
		[Obsolete("Use 'fadeDistanceToSurface' instead")]
		public float fadeDistanceToPlane
		{
			get
			{
				return this.fadeDistanceToSurface;
			}
			set
			{
				this.fadeDistanceToSurface = value;
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001AC8A File Offset: 0x00018E8A
		public bool IsColliderHiddenByDynamicOccluder(Collider collider)
		{
			return this.planeEquationWS.IsValid() && !GeometryUtility.TestPlanesAABB(new Plane[]
			{
				this.planeEquationWS
			}, collider.bounds);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001ACBC File Offset: 0x00018EBC
		protected override string GetShaderKeyword()
		{
			return "VLB_OCCLUSION_CLIPPING_PLANE";
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000022C9 File Offset: 0x000004C9
		protected override MaterialManager.SD.DynamicOcclusion GetDynamicOcclusionMode()
		{
			return MaterialManager.SD.DynamicOcclusion.ClippingPlane;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0001ACC3 File Offset: 0x00018EC3
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x0001ACCB File Offset: 0x00018ECB
		public Plane planeEquationWS { get; private set; }

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001ACD4 File Offset: 0x00018ED4
		protected override void OnValidateProperties()
		{
			base.OnValidateProperties();
			this.minOccluderArea = Mathf.Max(this.minOccluderArea, 0f);
			this.fadeDistanceToSurface = Mathf.Max(this.fadeDistanceToSurface, 0f);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001AD08 File Offset: 0x00018F08
		protected override void OnEnablePostValidate()
		{
			this.m_CurrentHit.SetNull();
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001AD15 File Offset: 0x00018F15
		protected override void OnDisable()
		{
			base.OnDisable();
			this.SetHitNull();
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001AD24 File Offset: 0x00018F24
		private void Start()
		{
			if (Application.isPlaying)
			{
				TriggerZone component = base.GetComponent<TriggerZone>();
				if (component)
				{
					this.m_RangeMultiplier = Mathf.Max(1f, component.rangeMultiplier);
				}
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001AD60 File Offset: 0x00018F60
		private Vector3 GetRandomVectorAround(Vector3 direction, float angleDiff)
		{
			float num = angleDiff * 0.5f;
			return Quaternion.Euler(UnityEngine.Random.Range(-num, num), UnityEngine.Random.Range(-num, num), UnityEngine.Random.Range(-num, num)) * direction;
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x0001AD98 File Offset: 0x00018F98
		private QueryTriggerInteraction queryTriggerInteraction
		{
			get
			{
				if (!this.considerTriggers)
				{
					return QueryTriggerInteraction.Ignore;
				}
				return QueryTriggerInteraction.Collide;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001ADA5 File Offset: 0x00018FA5
		private float raycastMaxDistance
		{
			get
			{
				return this.m_Master.raycastDistance * this.m_RangeMultiplier * this.m_Master.GetLossyScale().z;
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001ADCA File Offset: 0x00018FCA
		private DynamicOcclusionRaycasting.HitResult GetBestHit(Vector3 rayPos, Vector3 rayDir)
		{
			if (this.dimensions != Dimensions.Dim2D)
			{
				return this.GetBestHit3D(rayPos, rayDir);
			}
			return this.GetBestHit2D(rayPos, rayDir);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001ADE8 File Offset: 0x00018FE8
		private DynamicOcclusionRaycasting.HitResult GetBestHit3D(Vector3 rayPos, Vector3 rayDir)
		{
			RaycastHit[] array = Physics.RaycastAll(rayPos, rayDir, this.raycastMaxDistance, this.layerMask.value, this.queryTriggerInteraction);
			int num = -1;
			float num2 = float.MaxValue;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].collider.gameObject != this.m_Master.gameObject && array[i].collider.bounds.GetMaxArea2D() >= this.minOccluderArea && array[i].distance < num2)
				{
					num2 = array[i].distance;
					num = i;
				}
			}
			if (num != -1)
			{
				return new DynamicOcclusionRaycasting.HitResult(ref array[num]);
			}
			return default(DynamicOcclusionRaycasting.HitResult);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001AEA4 File Offset: 0x000190A4
		private DynamicOcclusionRaycasting.HitResult GetBestHit2D(Vector3 rayPos, Vector3 rayDir)
		{
			RaycastHit2D[] array = Physics2D.RaycastAll(new Vector2(rayPos.x, rayPos.y), new Vector2(rayDir.x, rayDir.y), this.raycastMaxDistance, this.layerMask.value);
			int num = -1;
			float num2 = float.MaxValue;
			for (int i = 0; i < array.Length; i++)
			{
				if ((this.considerTriggers || !array[i].collider.isTrigger) && array[i].collider.gameObject != this.m_Master.gameObject && array[i].collider.bounds.GetMaxArea2D() >= this.minOccluderArea && array[i].distance < num2)
				{
					num2 = array[i].distance;
					num = i;
				}
			}
			if (num != -1)
			{
				return new DynamicOcclusionRaycasting.HitResult(ref array[num]);
			}
			return default(DynamicOcclusionRaycasting.HitResult);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001AF98 File Offset: 0x00019198
		private uint GetDirectionCount()
		{
			if (this.dimensions != Dimensions.Dim2D)
			{
				return 4U;
			}
			return 2U;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001AFA8 File Offset: 0x000191A8
		private Vector3 GetDirection(uint dirInt)
		{
			dirInt %= this.GetDirectionCount();
			switch (dirInt)
			{
			case 0U:
				return this.m_Master.raycastGlobalUp;
			case 1U:
				return -this.m_Master.raycastGlobalUp;
			case 2U:
				return -this.m_Master.raycastGlobalRight;
			case 3U:
				return this.m_Master.raycastGlobalRight;
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001B016 File Offset: 0x00019216
		private bool IsHitValid(ref DynamicOcclusionRaycasting.HitResult hit, Vector3 forwardVec)
		{
			return hit.hasCollider && Vector3.Dot(hit.normal, -forwardVec) >= this.maxSurfaceDot;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001B040 File Offset: 0x00019240
		protected override bool OnProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource source)
		{
			Vector3 raycastGlobalForward = this.m_Master.raycastGlobalForward;
			DynamicOcclusionRaycasting.HitResult hitResult = this.GetBestHit(base.transform.position, raycastGlobalForward);
			if (this.IsHitValid(ref hitResult, raycastGlobalForward))
			{
				if (this.minSurfaceRatio > 0.5f)
				{
					float raycastDistance = this.m_Master.raycastDistance;
					for (uint num = 0U; num < this.GetDirectionCount(); num += 1U)
					{
						Vector3 a = this.GetDirection(num + this.m_PrevNonSubHitDirectionId) * (this.minSurfaceRatio * 2f - 1f);
						a.Scale(base.transform.localScale);
						Vector3 vector = base.transform.position + a * this.m_Master.coneRadiusStart;
						Vector3 a2 = base.transform.position + a * this.m_Master.coneRadiusEnd + raycastGlobalForward * raycastDistance;
						DynamicOcclusionRaycasting.HitResult bestHit = this.GetBestHit(vector, (a2 - vector).normalized);
						if (!this.IsHitValid(ref bestHit, raycastGlobalForward))
						{
							this.m_PrevNonSubHitDirectionId = num;
							hitResult.SetNull();
							break;
						}
						if (bestHit.distance > hitResult.distance)
						{
							hitResult = bestHit;
						}
					}
				}
			}
			else
			{
				hitResult.SetNull();
			}
			this.SetHit(ref hitResult);
			return hitResult.hasCollider;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001B1A0 File Offset: 0x000193A0
		private void SetHit(ref DynamicOcclusionRaycasting.HitResult hit)
		{
			if (!hit.hasCollider)
			{
				this.SetHitNull();
				return;
			}
			PlaneAlignment planeAlignment = this.planeAlignment;
			if (planeAlignment != PlaneAlignment.Surface && planeAlignment == PlaneAlignment.Beam)
			{
				this.SetClippingPlane(new Plane(-this.m_Master.raycastGlobalForward, hit.point));
			}
			else
			{
				this.SetClippingPlane(new Plane(hit.normal, hit.point));
			}
			this.m_CurrentHit = hit;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001B210 File Offset: 0x00019410
		private void SetHitNull()
		{
			this.SetClippingPlaneOff();
			this.m_CurrentHit.SetNull();
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001B224 File Offset: 0x00019424
		protected override void OnModifyMaterialCallback(MaterialModifier.Interface owner)
		{
			Plane planeEquationWS = this.planeEquationWS;
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionClippingPlaneWS, new Vector4(planeEquationWS.normal.x, planeEquationWS.normal.y, planeEquationWS.normal.z, planeEquationWS.distance));
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionClippingPlaneProps, this.fadeDistanceToSurface);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001B284 File Offset: 0x00019484
		private void SetClippingPlane(Plane planeWS)
		{
			planeWS = planeWS.TranslateCustom(planeWS.normal * this.planeOffset);
			this.SetPlaneWS(planeWS);
			this.m_Master._INTERNAL_SetDynamicOcclusionCallback(this.GetShaderKeyword(), this.m_MaterialModifierCallbackCached);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001B2C0 File Offset: 0x000194C0
		private void SetClippingPlaneOff()
		{
			this.SetPlaneWS(default(Plane));
			this.m_Master._INTERNAL_SetDynamicOcclusionCallback(this.GetShaderKeyword(), null);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001B2EE File Offset: 0x000194EE
		private void SetPlaneWS(Plane planeWS)
		{
			this.planeEquationWS = planeWS;
		}

		// Token: 0x040006A2 RID: 1698
		public new const string ClassName = "DynamicOcclusionRaycasting";

		// Token: 0x040006A3 RID: 1699
		public Dimensions dimensions;

		// Token: 0x040006A4 RID: 1700
		public LayerMask layerMask = Consts.DynOcclusion.LayerMaskDefault;

		// Token: 0x040006A5 RID: 1701
		public bool considerTriggers;

		// Token: 0x040006A6 RID: 1702
		public float minOccluderArea;

		// Token: 0x040006A7 RID: 1703
		public float minSurfaceRatio = 0.5f;

		// Token: 0x040006A8 RID: 1704
		public float maxSurfaceDot = 0.25f;

		// Token: 0x040006A9 RID: 1705
		public PlaneAlignment planeAlignment;

		// Token: 0x040006AA RID: 1706
		public float planeOffset = 0.1f;

		// Token: 0x040006AB RID: 1707
		[FormerlySerializedAs("fadeDistanceToPlane")]
		public float fadeDistanceToSurface = 0.25f;

		// Token: 0x040006AC RID: 1708
		private DynamicOcclusionRaycasting.HitResult m_CurrentHit;

		// Token: 0x040006AD RID: 1709
		private float m_RangeMultiplier = 1f;

		// Token: 0x040006AF RID: 1711
		private uint m_PrevNonSubHitDirectionId;

		// Token: 0x0200013E RID: 318
		public struct HitResult
		{
			// Token: 0x060005BC RID: 1468 RVA: 0x0001B34D File Offset: 0x0001954D
			public HitResult(ref RaycastHit hit3D)
			{
				this.point = hit3D.point;
				this.normal = hit3D.normal;
				this.distance = hit3D.distance;
				this.collider3D = hit3D.collider;
				this.collider2D = null;
			}

			// Token: 0x060005BD RID: 1469 RVA: 0x0001B388 File Offset: 0x00019588
			public HitResult(ref RaycastHit2D hit2D)
			{
				this.point = hit2D.point;
				this.normal = hit2D.normal;
				this.distance = hit2D.distance;
				this.collider2D = hit2D.collider;
				this.collider3D = null;
			}

			// Token: 0x1700010A RID: 266
			// (get) Token: 0x060005BE RID: 1470 RVA: 0x0001B3D6 File Offset: 0x000195D6
			public bool hasCollider
			{
				get
				{
					return this.collider2D || this.collider3D;
				}
			}

			// Token: 0x1700010B RID: 267
			// (get) Token: 0x060005BF RID: 1471 RVA: 0x0001B3F2 File Offset: 0x000195F2
			public string name
			{
				get
				{
					if (this.collider3D)
					{
						return this.collider3D.name;
					}
					if (this.collider2D)
					{
						return this.collider2D.name;
					}
					return "null collider";
				}
			}

			// Token: 0x1700010C RID: 268
			// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0001B42C File Offset: 0x0001962C
			public Bounds bounds
			{
				get
				{
					if (this.collider3D)
					{
						return this.collider3D.bounds;
					}
					if (this.collider2D)
					{
						return this.collider2D.bounds;
					}
					return default(Bounds);
				}
			}

			// Token: 0x060005C1 RID: 1473 RVA: 0x0001B474 File Offset: 0x00019674
			public void SetNull()
			{
				this.collider2D = null;
				this.collider3D = null;
			}

			// Token: 0x040006B0 RID: 1712
			public Vector3 point;

			// Token: 0x040006B1 RID: 1713
			public Vector3 normal;

			// Token: 0x040006B2 RID: 1714
			public float distance;

			// Token: 0x040006B3 RID: 1715
			private Collider2D collider2D;

			// Token: 0x040006B4 RID: 1716
			private Collider collider3D;
		}

		// Token: 0x0200013F RID: 319
		private enum Direction
		{
			// Token: 0x040006B6 RID: 1718
			Up,
			// Token: 0x040006B7 RID: 1719
			Down,
			// Token: 0x040006B8 RID: 1720
			Left,
			// Token: 0x040006B9 RID: 1721
			Right,
			// Token: 0x040006BA RID: 1722
			Max2D = 1,
			// Token: 0x040006BB RID: 1723
			Max3D = 3
		}
	}
}
