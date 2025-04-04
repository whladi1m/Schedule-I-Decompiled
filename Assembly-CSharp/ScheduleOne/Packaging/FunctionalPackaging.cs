using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Audio;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x02000882 RID: 2178
	public class FunctionalPackaging : Draggable
	{
		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x000F79A8 File Offset: 0x000F5BA8
		// (set) Token: 0x06003ADC RID: 15068 RVA: 0x000F79B0 File Offset: 0x000F5BB0
		public bool IsSealed { get; protected set; }

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x000F79B9 File Offset: 0x000F5BB9
		// (set) Token: 0x06003ADE RID: 15070 RVA: 0x000F79C1 File Offset: 0x000F5BC1
		public bool IsFull { get; protected set; }

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x000F79CA File Offset: 0x000F5BCA
		// (set) Token: 0x06003AE0 RID: 15072 RVA: 0x000F79D2 File Offset: 0x000F5BD2
		public bool ReachedOutput { get; protected set; }

		// Token: 0x06003AE1 RID: 15073 RVA: 0x000F79DC File Offset: 0x000F5BDC
		public virtual void Initialize(PackagingStation _station, Transform alignment, bool align = true)
		{
			this.station = _station;
			if (align)
			{
				this.AlignTo(alignment);
			}
			this.ClickableEnabled = false;
			base.Rb.isKinematic = true;
			if (this.VelocityCalculator == null)
			{
				this.VelocityCalculator = base.gameObject.AddComponent<SmoothedVelocityCalculator>();
				this.VelocityCalculator.MaxReasonableVelocity = 2f;
			}
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x000F7A3C File Offset: 0x000F5C3C
		public void AlignTo(Transform alignment)
		{
			base.transform.rotation = alignment.rotation * (Quaternion.Inverse(this.AlignmentPoint.rotation) * base.transform.rotation);
			Vector3 b = base.transform.position - this.AlignmentPoint.position;
			base.transform.position = alignment.position + b;
			if (base.Rb == null)
			{
				base.Rb = base.GetComponent<Rigidbody>();
			}
			if (base.Rb != null)
			{
				base.Rb.position = base.transform.position;
				base.Rb.rotation = base.transform.rotation;
			}
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x000F7B06 File Offset: 0x000F5D06
		public virtual void Destroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x000F7B14 File Offset: 0x000F5D14
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.IsFull)
			{
				return;
			}
			foreach (FunctionalProduct functionalProduct in this.productContactTime.Keys.ToList<FunctionalProduct>())
			{
				if (!(functionalProduct.Rb == null) && this.productContactTime[functionalProduct] > this.ProductContactTime && !this.PackedProducts.Contains(functionalProduct) && !functionalProduct.IsHeld)
				{
					this.PackProduct(functionalProduct);
				}
			}
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x000F7BB8 File Offset: 0x000F5DB8
		protected virtual void PackProduct(FunctionalProduct product)
		{
			product.ClickableEnabled = false;
			product.ClampZ = false;
			UnityEngine.Object.Destroy(product.Rb);
			product.transform.SetParent(base.transform);
			if (this.ProductAlignmentPoints.Length > this.PackedProducts.Count)
			{
				product.transform.position = this.ProductAlignmentPoints[this.PackedProducts.Count].position;
				product.transform.rotation = this.ProductAlignmentPoints[this.PackedProducts.Count].rotation;
			}
			this.PackedProducts.Add(product);
			if (this.PackedProducts.Count >= this.Definition.Quantity && !this.IsFull)
			{
				this.FullyPacked();
			}
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x000F7C7C File Offset: 0x000F5E7C
		protected virtual void FullyPacked()
		{
			this.IsFull = true;
			if (this.onFullyPacked != null)
			{
				this.onFullyPacked();
			}
			foreach (FunctionalProduct functionalProduct in this.PackedProducts)
			{
				UnityEngine.Object.Destroy(functionalProduct.Rb);
			}
			if (this.AutoEnableSealing)
			{
				this.EnableSealing();
			}
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x000F7CFC File Offset: 0x000F5EFC
		protected virtual void OnTriggerStay(Collider other)
		{
			if (this.station == null)
			{
				return;
			}
			FunctionalProduct componentInParent = other.GetComponentInParent<FunctionalProduct>();
			if (componentInParent != null && componentInParent.IsHeld)
			{
				return;
			}
			if (componentInParent != null)
			{
				if (!this.productContactTime.ContainsKey(componentInParent))
				{
					this.productContactTime.Add(componentInParent, 0f);
				}
				Vector3 velocity = componentInParent.VelocityCalculator.Velocity;
				Vector3 velocity2 = this.VelocityCalculator.Velocity;
				Vector3 vector = velocity - velocity2;
				Debug.DrawRay(componentInParent.transform.position, velocity, Color.red);
				Debug.DrawRay(base.transform.position, velocity2, Color.blue);
				if (vector.magnitude < this.ProductContactMaxVelocity)
				{
					Dictionary<FunctionalProduct, float> dictionary = this.productContactTime;
					FunctionalProduct key = componentInParent;
					dictionary[key] += Time.fixedDeltaTime;
				}
			}
			if (other.gameObject.name == this.station.OutputCollider.name && !this.ReachedOutput && this.IsSealed && !base.IsHeld)
			{
				this.ReachedOutput = true;
				if (this.onReachOutput != null)
				{
					this.onReachOutput();
				}
			}
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x000F7E2E File Offset: 0x000F602E
		protected virtual void EnableSealing()
		{
			this.ClickableEnabled = true;
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x000F7E38 File Offset: 0x000F6038
		public virtual void Seal()
		{
			this.IsSealed = true;
			foreach (FunctionalProduct functionalProduct in this.PackedProducts)
			{
				Collider[] componentsInChildren = functionalProduct.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
			}
			if (this.SealSound != null)
			{
				this.SealSound.Play();
			}
			this.HoveredCursor = CursorManager.ECursorType.OpenHand;
			this.ClickableEnabled = true;
			base.Rb.isKinematic = false;
			if (this.onSealed != null)
			{
				this.onSealed();
			}
		}

		// Token: 0x04002ACA RID: 10954
		[Header("Settings")]
		public string SealInstruction = "Seal packaging";

		// Token: 0x04002ACB RID: 10955
		public bool AutoEnableSealing = true;

		// Token: 0x04002ACC RID: 10956
		public float ProductContactTime = 0.1f;

		// Token: 0x04002ACD RID: 10957
		public float ProductContactMaxVelocity = 0.3f;

		// Token: 0x04002ACE RID: 10958
		[Header("References")]
		public PackagingDefinition Definition;

		// Token: 0x04002ACF RID: 10959
		public Transform AlignmentPoint;

		// Token: 0x04002AD0 RID: 10960
		public Transform[] ProductAlignmentPoints;

		// Token: 0x04002AD1 RID: 10961
		public AudioSourceController SealSound;

		// Token: 0x04002AD2 RID: 10962
		protected List<FunctionalProduct> PackedProducts = new List<FunctionalProduct>();

		// Token: 0x04002AD3 RID: 10963
		public Action onFullyPacked;

		// Token: 0x04002AD4 RID: 10964
		public Action onSealed;

		// Token: 0x04002AD5 RID: 10965
		public Action onReachOutput;

		// Token: 0x04002AD6 RID: 10966
		private PackagingStation station;

		// Token: 0x04002AD7 RID: 10967
		private Dictionary<FunctionalProduct, float> productContactTime = new Dictionary<FunctionalProduct, float>();

		// Token: 0x04002AD8 RID: 10968
		private SmoothedVelocityCalculator VelocityCalculator;
	}
}
