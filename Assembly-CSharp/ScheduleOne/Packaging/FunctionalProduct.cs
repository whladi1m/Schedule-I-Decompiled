using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x02000883 RID: 2179
	public class FunctionalProduct : Draggable
	{
		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06003AEB RID: 15083 RVA: 0x000F7F41 File Offset: 0x000F6141
		// (set) Token: 0x06003AEC RID: 15084 RVA: 0x000F7F49 File Offset: 0x000F6149
		public SmoothedVelocityCalculator VelocityCalculator { get; private set; }

		// Token: 0x06003AED RID: 15085 RVA: 0x000F7F54 File Offset: 0x000F6154
		public virtual void Initialize(PackagingStation station, ItemInstance item, Transform alignment, bool align = true)
		{
			if (align)
			{
				this.AlignTo(alignment);
			}
			this.startLocalPos = base.transform.localPosition;
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Task"));
			this.InitializeVisuals(item);
			base.Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			if (this.VelocityCalculator == null)
			{
				this.VelocityCalculator = base.gameObject.AddComponent<SmoothedVelocityCalculator>();
				this.VelocityCalculator.MaxReasonableVelocity = 2f;
			}
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x000F7FD4 File Offset: 0x000F61D4
		public virtual void Initialize(ItemInstance item)
		{
			this.startLocalPos = base.transform.localPosition;
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Task"));
			this.InitializeVisuals(item);
			base.Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			if (this.VelocityCalculator == null)
			{
				this.VelocityCalculator = base.gameObject.AddComponent<SmoothedVelocityCalculator>();
				this.VelocityCalculator.MaxReasonableVelocity = 2f;
			}
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x000F804C File Offset: 0x000F624C
		public virtual void InitializeVisuals(ItemInstance item)
		{
			ProductItemInstance productItemInstance = item as ProductItemInstance;
			if (productItemInstance == null)
			{
				Console.LogError("Item instance is not a product instance!", null);
				return;
			}
			productItemInstance.SetupPackagingVisuals(this.Visuals);
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x000F807C File Offset: 0x000F627C
		public void AlignTo(Transform alignment)
		{
			base.transform.rotation = alignment.rotation * (Quaternion.Inverse(this.AlignmentPoint.rotation) * base.transform.rotation);
			base.transform.position = alignment.position + (base.transform.position - this.AlignmentPoint.position);
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x000F80F0 File Offset: 0x000F62F0
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x000F80F8 File Offset: 0x000F62F8
		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (this.ClampZ)
			{
				this.Clamp();
			}
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x000F8110 File Offset: 0x000F6310
		private void Clamp()
		{
			float num = Mathf.Clamp(Mathf.Abs(base.transform.localPosition.x / this.startLocalPos.x), 0f, 1f);
			float num2 = Mathf.Min(Mathf.Abs(this.startLocalPos.z) * num, this.lowestMaxZ);
			this.lowestMaxZ = num2;
			Vector3 vector = base.transform.parent.InverseTransformPoint(base.originalHitPoint);
			vector.z = Mathf.Clamp(vector.z, -num2, num2);
			Vector3 originalHitPoint = base.transform.parent.TransformPoint(vector);
			base.SetOriginalHitPoint(originalHitPoint);
		}

		// Token: 0x04002AD9 RID: 10969
		public bool ClampZ = true;

		// Token: 0x04002ADA RID: 10970
		[Header("References")]
		public Transform AlignmentPoint;

		// Token: 0x04002ADB RID: 10971
		public FilledPackagingVisuals Visuals;

		// Token: 0x04002ADC RID: 10972
		private Vector3 startLocalPos;

		// Token: 0x04002ADD RID: 10973
		private float lowestMaxZ = 500f;
	}
}
