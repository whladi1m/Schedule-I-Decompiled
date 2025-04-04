using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000113 RID: 275
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[SelectionBase]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-hd/")]
	public class VolumetricLightBeamHD2D : VolumetricLightBeamHD
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00017CA8 File Offset: 0x00015EA8
		// (set) Token: 0x060004B3 RID: 1203 RVA: 0x00017CB0 File Offset: 0x00015EB0
		public int sortingLayerID
		{
			get
			{
				return this.m_SortingLayerID;
			}
			set
			{
				this.m_SortingLayerID = value;
				if (this.m_BeamGeom)
				{
					this.m_BeamGeom.sortingLayerID = value;
				}
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00017CD2 File Offset: 0x00015ED2
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x00017CDF File Offset: 0x00015EDF
		public string sortingLayerName
		{
			get
			{
				return SortingLayer.IDToName(this.sortingLayerID);
			}
			set
			{
				this.sortingLayerID = SortingLayer.NameToID(value);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00017CED File Offset: 0x00015EED
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x00017CF5 File Offset: 0x00015EF5
		public int sortingOrder
		{
			get
			{
				return this.m_SortingOrder;
			}
			set
			{
				this.m_SortingOrder = value;
				if (this.m_BeamGeom)
				{
					this.m_BeamGeom.sortingOrder = value;
				}
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000022C9 File Offset: 0x000004C9
		public override Dimensions GetDimensions()
		{
			return Dimensions.Dim2D;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x000022C9 File Offset: 0x000004C9
		public override bool DoesSupportSorting2D()
		{
			return true;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00017D17 File Offset: 0x00015F17
		public override int GetSortingLayerID()
		{
			return this.sortingLayerID;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00017D1F File Offset: 0x00015F1F
		public override int GetSortingOrder()
		{
			return this.sortingOrder;
		}

		// Token: 0x04000611 RID: 1553
		[SerializeField]
		private int m_SortingLayerID;

		// Token: 0x04000612 RID: 1554
		[SerializeField]
		private int m_SortingOrder;
	}
}
