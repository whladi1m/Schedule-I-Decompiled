using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x02000871 RID: 2161
	public class FunctionalBaggie : FunctionalPackaging
	{
		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06003A92 RID: 14994 RVA: 0x000F6681 File Offset: 0x000F4881
		// (set) Token: 0x06003A93 RID: 14995 RVA: 0x000F6689 File Offset: 0x000F4889
		public override CursorManager.ECursorType HoveredCursor { get; protected set; } = CursorManager.ECursorType.Finger;

		// Token: 0x06003A94 RID: 14996 RVA: 0x000F6694 File Offset: 0x000F4894
		public void SetClosed(float closedDelta)
		{
			this.ClosedDelta = closedDelta;
			SkinnedMeshRenderer[] bagMeshes = this.BagMeshes;
			for (int i = 0; i < bagMeshes.Length; i++)
			{
				bagMeshes[i].SetBlendShapeWeight(0, closedDelta * 100f);
			}
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x000F66CD File Offset: 0x000F48CD
		public override void StartClick(RaycastHit hit)
		{
			if (base.IsFull && this.ClosedDelta == 0f)
			{
				this.ClickableEnabled = false;
				if (!base.IsSealed)
				{
					this.Seal();
				}
			}
			base.StartClick(hit);
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x000F6700 File Offset: 0x000F4900
		public override void Seal()
		{
			base.Seal();
			this.FunnelCollidersContainer.gameObject.SetActive(false);
			this.DynamicCollider.enabled = true;
			base.StartCoroutine(this.<Seal>g__Routine|11_0());
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x000F6732 File Offset: 0x000F4932
		protected override void FullyPacked()
		{
			base.FullyPacked();
			this.FullyPackedBlocker.SetActive(true);
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x000F6755 File Offset: 0x000F4955
		[CompilerGenerated]
		private IEnumerator <Seal>g__Routine|11_0()
		{
			float lerpTime = 0.25f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.SetClosed(i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.SetClosed(1f);
			yield break;
		}

		// Token: 0x04002A69 RID: 10857
		public SkinnedMeshRenderer[] BagMeshes;

		// Token: 0x04002A6A RID: 10858
		public GameObject FunnelCollidersContainer;

		// Token: 0x04002A6B RID: 10859
		public GameObject FullyPackedBlocker;

		// Token: 0x04002A6C RID: 10860
		public Collider DynamicCollider;

		// Token: 0x04002A6E RID: 10862
		private float ClosedDelta;
	}
}
