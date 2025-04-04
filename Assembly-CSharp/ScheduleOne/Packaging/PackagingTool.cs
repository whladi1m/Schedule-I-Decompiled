using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using TMPro;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x02000877 RID: 2167
	public class PackagingTool : MonoBehaviour
	{
		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06003AB2 RID: 15026 RVA: 0x000F6AE5 File Offset: 0x000F4CE5
		// (set) Token: 0x06003AB3 RID: 15027 RVA: 0x000F6AED File Offset: 0x000F4CED
		public bool ReceiveInput { get; private set; }

		// Token: 0x06003AB4 RID: 15028 RVA: 0x000F6AF8 File Offset: 0x000F4CF8
		public void Initialize(Task _task, FunctionalPackaging packaging, int packagingQuantity, ProductItemInstance product, int productQuantity)
		{
			this.task = _task;
			this.ReceiveInput = true;
			this.LeftButton.ClickableEnabled = true;
			this.RightButton.ClickableEnabled = true;
			this.DropButton.ClickableEnabled = true;
			this.LoadPackaging(packaging, packagingQuantity);
			this.LoadProduct(product, productQuantity);
			int num = Mathf.RoundToInt(180f / this.DeployAngle);
			for (int i = 0; i < num; i++)
			{
				this.CheckDeployPackaging();
				this.Rotate(this.DeployAngle);
			}
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x000F6B7C File Offset: 0x000F4D7C
		public void Deinitialize()
		{
			this.ReceiveInput = false;
			if (this.LeftButton.IsHeld)
			{
				this.task.ForceEndClick(this.LeftButton);
			}
			if (this.RightButton.IsHeld)
			{
				this.task.ForceEndClick(this.RightButton);
			}
			if (this.DropButton.IsHeld)
			{
				this.task.ForceEndClick(this.DropButton);
			}
			this.LeftButton.ClickableEnabled = false;
			this.RightButton.ClickableEnabled = false;
			this.DropButton.ClickableEnabled = false;
			for (int i = 0; i < this.ProductInstances.Count; i++)
			{
				UnityEngine.Object.Destroy(this.ProductInstances[i].gameObject);
			}
			this.ProductInstances.Clear();
			for (int j = 0; j < this.PackagingInstances.Count; j++)
			{
				UnityEngine.Object.Destroy(this.PackagingInstances[j].Container.gameObject);
			}
			this.PackagingInstances.Clear();
			for (int k = 0; k < this.FinalizedPackaging.Count; k++)
			{
				UnityEngine.Object.Destroy(this.FinalizedPackaging[k].gameObject);
			}
			this.FinalizedPackaging.Clear();
			if (this.finalizeCoroutine != null)
			{
				base.StopCoroutine(this.finalizeCoroutine);
				this.finalizeCoroutine = null;
			}
			this.UnloadPackaging();
			this.UnloadProduct();
			this.task = null;
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x000F6CE6 File Offset: 0x000F4EE6
		private void LoadPackaging(FunctionalPackaging prefab, int quantity)
		{
			this.PackagingPrefab = prefab;
			this.ConcealedPackaging = quantity;
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x000F6CF6 File Offset: 0x000F4EF6
		private void UnloadPackaging()
		{
			this.PackagingPrefab = null;
			this.ConcealedPackaging = 0;
		}

		// Token: 0x06003AB8 RID: 15032 RVA: 0x000F6D06 File Offset: 0x000F4F06
		private void LoadProduct(ProductItemInstance product, int quantity)
		{
			this.ProductItem = product;
			this.ProductPrefab = (product.Definition as ProductDefinition).FunctionalProduct;
			this.ProductInHopper = quantity;
			this.UpdateScreen();
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x000F6D32 File Offset: 0x000F4F32
		private void UnloadProduct()
		{
			this.ProductPrefab = null;
			this.ProductInHopper = 0;
			this.UpdateScreen();
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x000F6D48 File Offset: 0x000F4F48
		public void Update()
		{
			this.timeSinceLastDrop += Time.deltaTime;
			this.UpdateInput();
			this.UpdateConveyor();
			if (this.ConcealedPackaging > 0)
			{
				this.CheckDeployPackaging();
			}
			if (this.DropButton.IsHeld && this.ProductInHopper > 0 && this.timeSinceLastDrop > this.DropCooldown)
			{
				this.DropProduct();
			}
			if (Mathf.Abs(this.conveyorVelocity) > 0f && !this.MotorSound.isPlaying)
			{
				this.MotorSound.Play();
			}
			this.MotorSound.VolumeMultiplier = Mathf.Abs(this.conveyorVelocity);
			this.MotorSound.PitchMultiplier = Mathf.Lerp(0.7f, 1f, Mathf.Abs(this.conveyorVelocity));
			if (this.MotorSound.VolumeMultiplier <= 0f)
			{
				this.MotorSound.Stop();
			}
			else if (this.MotorSound.VolumeMultiplier > 0f && !this.MotorSound.isPlaying)
			{
				this.MotorSound.Play();
			}
			this.CheckFinalize();
			this.CheckInsertions();
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x000F6E68 File Offset: 0x000F5068
		private void UpdateInput()
		{
			this.directionInput = 0;
			if (!this.ReceiveInput)
			{
				return;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.Left))
			{
				if (!this.LeftButton.IsHeld)
				{
					this.leftDown = true;
					this.task.ForceStartClick(this.LeftButton);
				}
			}
			else if (this.leftDown)
			{
				this.leftDown = false;
				this.task.ForceEndClick(this.LeftButton);
			}
			if (GameInput.GetButton(GameInput.ButtonCode.Right))
			{
				if (!this.RightButton.IsHeld)
				{
					this.rightDown = true;
					this.task.ForceStartClick(this.RightButton);
				}
			}
			else if (this.rightDown)
			{
				this.rightDown = false;
				this.task.ForceEndClick(this.RightButton);
			}
			if (GameInput.GetButton(GameInput.ButtonCode.Jump))
			{
				if (!this.DropButton.IsHeld)
				{
					this.dropDown = true;
					this.task.ForceStartClick(this.DropButton);
				}
			}
			else if (this.dropDown)
			{
				this.dropDown = false;
				this.task.ForceEndClick(this.DropButton);
			}
			if (this.LeftButton.IsHeld)
			{
				this.directionInput--;
			}
			if (this.RightButton.IsHeld)
			{
				this.directionInput++;
			}
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x000F6FA8 File Offset: 0x000F51A8
		private void UpdateScreen()
		{
			this.ProductCountText.text = this.ProductInHopper.ToString();
			this.ProductCountText.gameObject.SetActive(this.ProductInHopper > 0);
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x000F6FDC File Offset: 0x000F51DC
		private void UpdateConveyor()
		{
			float num = Mathf.MoveTowards(this.conveyorVelocity, (float)this.directionInput, this.ConveyorAcceleration * Time.deltaTime);
			this.conveyorVelocity = num;
			this.Rotate(this.conveyorVelocity * this.ConveyorSpeed * Time.deltaTime);
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x000F7028 File Offset: 0x000F5228
		private void Rotate(float angle)
		{
			this.ConveyorModel.Rotate(Vector3.forward, -angle);
			for (int i = 0; i < this.PackagingInstances.Count; i++)
			{
				this.PackagingInstances[i].ChangePosition(angle);
			}
			this.PackagingInstances.Sort((PackagingTool.PackagingInstance a, PackagingTool.PackagingInstance b) => a.AnglePosition.CompareTo(b.AnglePosition));
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x000F709C File Offset: 0x000F529C
		private void CheckDeployPackaging()
		{
			if (this.PackagingInstances.Count > 0 && (this.PackagingInstances[0].AnglePosition < this.DeployAngle || this.PackagingInstances[this.PackagingInstances.Count - 1].AnglePosition > 360f - this.DeployAngle))
			{
				return;
			}
			this.DeployPackaging();
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x000F7104 File Offset: 0x000F5304
		private void CheckFinalize()
		{
			if (this.finalizeCoroutine != null)
			{
				return;
			}
			for (int i = 0; i < this.PackagingInstances.Count; i++)
			{
				if (this.PackagingInstances[i].Packaging.IsFull && this.PackagingInstances[i].AnglePosition > 255f && this.PackagingInstances[i].AnglePosition < 270f)
				{
					this.Finalize(this.PackagingInstances[i]);
					return;
				}
			}
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x000F718C File Offset: 0x000F538C
		private void Finalize(PackagingTool.PackagingInstance instance)
		{
			PackagingTool.<>c__DisplayClass66_0 CS$<>8__locals1 = new PackagingTool.<>c__DisplayClass66_0();
			CS$<>8__locals1.instance = instance;
			CS$<>8__locals1.<>4__this = this;
			this.finalizeInstance = CS$<>8__locals1.instance;
			this.finalizeCoroutine = base.StartCoroutine(CS$<>8__locals1.<Finalize>g__FinalizeRoutine|0());
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x000F71CC File Offset: 0x000F53CC
		private void DropProduct()
		{
			if (this.ProductInHopper <= 0)
			{
				return;
			}
			this.timeSinceLastDrop = 0f;
			this.ProductInHopper--;
			this.UpdateScreen();
			this.DropSound.Play();
			FunctionalProduct functionalProduct = UnityEngine.Object.Instantiate<FunctionalProduct>(this.ProductPrefab, this.HopperDropPoint.position, this.HopperDropPoint.rotation);
			functionalProduct.Initialize(this.ProductItem);
			functionalProduct.transform.SetParent(this.ProductContainer);
			functionalProduct.ClampZ = true;
			functionalProduct.DragProjectionMode = Draggable.EDragProjectionMode.FlatCameraForward;
			functionalProduct.Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			functionalProduct.Rb.AddForce(Vector3.down * this.ProductInitialForce, ForceMode.VelocityChange);
			functionalProduct.Rb.AddTorque(UnityEngine.Random.insideUnitSphere * this.ProductRandomTorque, ForceMode.VelocityChange);
			this.ProductInstances.Add(functionalProduct);
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x000F72AC File Offset: 0x000F54AC
		private void CheckInsertions()
		{
			for (int i = 0; i < this.ProductInstances.Count; i++)
			{
				if (!(this.ProductInstances[i].Rb == null) && !this.ProductInstances[i].Rb.isKinematic && this.HopperInputCollider.bounds.Contains(this.ProductInstances[i].transform.position))
				{
					this.InsertIntoHopper(this.ProductInstances[i]);
					i--;
				}
			}
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x000F7344 File Offset: 0x000F5544
		private void InsertIntoHopper(FunctionalProduct product)
		{
			this.ProductInHopper++;
			this.UpdateScreen();
			if (product.IsHeld)
			{
				this.task.ForceEndClick(product);
			}
			UnityEngine.Object.Destroy(product.gameObject);
			this.ProductInstances.Remove(product);
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x000F7394 File Offset: 0x000F5594
		private void DeployPackaging()
		{
			if (this.ConcealedPackaging <= 0)
			{
				return;
			}
			this.ConcealedPackaging--;
			GameObject gameObject = new GameObject("Packaging Container");
			gameObject.transform.SetParent(this.PackagingContainer);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			FunctionalPackaging functionalPackaging = UnityEngine.Object.Instantiate<FunctionalPackaging>(this.PackagingPrefab, gameObject.transform);
			functionalPackaging.AutoEnableSealing = false;
			functionalPackaging.Initialize(this.Station, null, false);
			functionalPackaging.Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			if (functionalPackaging is FunctionalBaggie)
			{
				functionalPackaging.transform.position = this.BaggieStartPoint.position;
				functionalPackaging.Rb.position = this.BaggieStartPoint.position;
				functionalPackaging.transform.rotation = this.BaggieStartPoint.rotation;
				functionalPackaging.Rb.rotation = this.BaggieStartPoint.rotation;
			}
			else if (functionalPackaging is FunctionalJar)
			{
				functionalPackaging.transform.position = this.JarStartPoint.position;
				functionalPackaging.Rb.position = this.JarStartPoint.position;
				functionalPackaging.transform.rotation = this.JarStartPoint.rotation;
				functionalPackaging.Rb.rotation = this.JarStartPoint.rotation;
			}
			else
			{
				Console.LogError("Unknown packaging type!", null);
			}
			PackagingTool.PackagingInstance packagingInstance = new PackagingTool.PackagingInstance();
			packagingInstance.Container = gameObject.transform;
			packagingInstance.ContainerRb = gameObject.AddComponent<Rigidbody>();
			packagingInstance.ContainerRb.isKinematic = true;
			packagingInstance.ContainerRb.useGravity = false;
			packagingInstance.ContainerRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			packagingInstance.Packaging = functionalPackaging;
			Console.Log("Deployed packaging", null);
			this.PackagingInstances.Insert(0, packagingInstance);
		}

		// Token: 0x04002A80 RID: 10880
		private const float FinalizeRange_Min = 255f;

		// Token: 0x04002A81 RID: 10881
		private const float FinalizeRange_Max = 270f;

		// Token: 0x04002A82 RID: 10882
		[Header("Settings")]
		public float ConveyorSpeed = 1f;

		// Token: 0x04002A83 RID: 10883
		public float ConveyorAcceleration = 1f;

		// Token: 0x04002A84 RID: 10884
		public float BaggieRadius = 0.3f;

		// Token: 0x04002A85 RID: 10885
		public float JarRadius = 0.35f;

		// Token: 0x04002A86 RID: 10886
		public float DeployAngle = 60f;

		// Token: 0x04002A87 RID: 10887
		public float ProductInitialForce = 0.2f;

		// Token: 0x04002A88 RID: 10888
		public float ProductRandomTorque = 0.5f;

		// Token: 0x04002A89 RID: 10889
		public float KickForce = 1f;

		// Token: 0x04002A8A RID: 10890
		public float DropCooldown = 0.25f;

		// Token: 0x04002A8B RID: 10891
		[Header("References")]
		public PackagingStation Station;

		// Token: 0x04002A8C RID: 10892
		public Transform ConveyorModel;

		// Token: 0x04002A8D RID: 10893
		public Animation DoorAnim;

		// Token: 0x04002A8E RID: 10894
		public Animation CapAnim;

		// Token: 0x04002A8F RID: 10895
		public Animation SealAnim;

		// Token: 0x04002A90 RID: 10896
		public Animation KickAnim;

		// Token: 0x04002A91 RID: 10897
		public Clickable LeftButton;

		// Token: 0x04002A92 RID: 10898
		public Clickable RightButton;

		// Token: 0x04002A93 RID: 10899
		public Clickable DropButton;

		// Token: 0x04002A94 RID: 10900
		public Transform PackagingContainer;

		// Token: 0x04002A95 RID: 10901
		public TextMeshPro ProductCountText;

		// Token: 0x04002A96 RID: 10902
		public Transform HopperDropPoint;

		// Token: 0x04002A97 RID: 10903
		public Transform BaggieStartPoint;

		// Token: 0x04002A98 RID: 10904
		public Transform JarStartPoint;

		// Token: 0x04002A99 RID: 10905
		public Transform ProductContainer;

		// Token: 0x04002A9A RID: 10906
		public Transform KickOrigin;

		// Token: 0x04002A9B RID: 10907
		public SphereCollider HopperInputCollider;

		// Token: 0x04002A9C RID: 10908
		public AudioSourceController KickSound;

		// Token: 0x04002A9D RID: 10909
		public AudioSourceController MotorSound;

		// Token: 0x04002A9E RID: 10910
		public AudioSourceController DropSound;

		// Token: 0x04002A9F RID: 10911
		private FunctionalPackaging PackagingPrefab;

		// Token: 0x04002AA0 RID: 10912
		private int ConcealedPackaging;

		// Token: 0x04002AA1 RID: 10913
		private ProductItemInstance ProductItem;

		// Token: 0x04002AA2 RID: 10914
		private FunctionalProduct ProductPrefab;

		// Token: 0x04002AA3 RID: 10915
		private int ProductInHopper;

		// Token: 0x04002AA4 RID: 10916
		private List<PackagingTool.PackagingInstance> PackagingInstances = new List<PackagingTool.PackagingInstance>();

		// Token: 0x04002AA5 RID: 10917
		private List<FunctionalProduct> ProductInstances = new List<FunctionalProduct>();

		// Token: 0x04002AA6 RID: 10918
		private List<FunctionalPackaging> FinalizedPackaging = new List<FunctionalPackaging>();

		// Token: 0x04002AA7 RID: 10919
		private float conveyorVelocity;

		// Token: 0x04002AA8 RID: 10920
		private int directionInput;

		// Token: 0x04002AA9 RID: 10921
		private Task task;

		// Token: 0x04002AAA RID: 10922
		private PackagingTool.PackagingInstance finalizeInstance;

		// Token: 0x04002AAB RID: 10923
		private Coroutine finalizeCoroutine;

		// Token: 0x04002AAC RID: 10924
		private bool leftDown;

		// Token: 0x04002AAD RID: 10925
		private bool rightDown;

		// Token: 0x04002AAE RID: 10926
		private bool dropDown;

		// Token: 0x04002AAF RID: 10927
		private float timeSinceLastDrop = 10f;

		// Token: 0x02000878 RID: 2168
		public class PackagingInstance
		{
			// Token: 0x06003AC7 RID: 15047 RVA: 0x000F75FC File Offset: 0x000F57FC
			public void ChangePosition(float angleDelta)
			{
				this.AnglePosition += angleDelta;
				this.AnglePosition = Mathf.Repeat(this.AnglePosition, 360f);
				Quaternion rhs = Quaternion.Euler(0f, -this.AnglePosition, 0f);
				Quaternion rot = this.Container.parent.rotation * rhs;
				this.ContainerRb.MoveRotation(rot);
			}

			// Token: 0x04002AB0 RID: 10928
			public Transform Container;

			// Token: 0x04002AB1 RID: 10929
			public Rigidbody ContainerRb;

			// Token: 0x04002AB2 RID: 10930
			public FunctionalPackaging Packaging;

			// Token: 0x04002AB3 RID: 10931
			public float AnglePosition;
		}
	}
}
