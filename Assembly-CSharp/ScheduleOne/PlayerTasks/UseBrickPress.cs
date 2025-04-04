using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Packaging;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.UI;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000353 RID: 851
	public class UseBrickPress : Task
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x00054E73 File Offset: 0x00053073
		// (set) Token: 0x06001329 RID: 4905 RVA: 0x00054E7B File Offset: 0x0005307B
		public override string TaskName { get; protected set; } = "Use brick press";

		// Token: 0x0600132A RID: 4906 RVA: 0x00054E84 File Offset: 0x00053084
		public UseBrickPress(BrickPress _press, ProductItemInstance _product)
		{
			if (_press == null)
			{
				Console.LogError("Press is null!", null);
				return;
			}
			if (_press.GetState() != PackagingStation.EState.CanBegin)
			{
				Console.LogError("Press not ready to begin packaging!", null);
				return;
			}
			this.press = _press;
			this.product = _product;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.press.CameraPosition_Pouring.position, this.press.CameraPosition_Pouring.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.2f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("packaging");
			this.press.Container1.gameObject.SetActive(false);
			this.container = this.press.CreateFunctionalContainer(this.product, 0.75f, out this.products);
			base.CurrentInstruction = "Pour product into mould (0/20)";
			this.press.StartCoroutine(this.<.ctor>g__CheckMould|11_0());
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00054F97 File Offset: 0x00053197
		public override void Update()
		{
			base.Update();
			if (this.currentStep == UseBrickPress.EStep.Pressing && this.press.Handle.CurrentPosition >= 1f)
			{
				this.FinishPress();
			}
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00054FC8 File Offset: 0x000531C8
		public override void StopTask()
		{
			base.StopTask();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			if (this.container != null)
			{
				UnityEngine.Object.Destroy(this.container.gameObject);
			}
			for (int i = 0; i < this.products.Count; i++)
			{
				UnityEngine.Object.Destroy(this.products[i].gameObject);
			}
			this.press.Container1.gameObject.SetActive(true);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.press.CameraPosition.position, this.press.CameraPosition.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			Singleton<BrickPressCanvas>.Instance.SetIsOpen(this.press, true, true);
			this.press.Handle.Locked = false;
			this.press.Handle.SetInteractable(false);
			if (this.currentStep == UseBrickPress.EStep.Complete)
			{
				this.press.CompletePress(this.product);
			}
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x000550D8 File Offset: 0x000532D8
		private void CheckMould()
		{
			if (this.currentStep != UseBrickPress.EStep.Pouring)
			{
				return;
			}
			List<FunctionalProduct> productInMould = this.press.GetProductInMould();
			base.CurrentInstruction = "Pour product into mould (" + productInMould.Count.ToString() + "/20)";
			if (productInMould.Count >= 20)
			{
				this.BeginPress();
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x00055130 File Offset: 0x00053330
		private void BeginPress()
		{
			this.currentStep = UseBrickPress.EStep.Pressing;
			this.press.Handle.SetInteractable(true);
			this.container.ClickableEnabled = false;
			this.container.Rb.AddForce((this.press.transform.right + this.press.transform.up) * 2f, ForceMode.VelocityChange);
			base.CurrentInstruction = "Rotate handle quickly to press product";
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.3f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.press.CameraPosition_Raising.position, this.press.CameraPosition_Raising.rotation, 0.3f, false);
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x000551F0 File Offset: 0x000533F0
		private void FinishPress()
		{
			this.press.SlamSound.Play();
			this.currentStep = UseBrickPress.EStep.Complete;
			this.press.Handle.Locked = true;
			this.press.Handle.SetInteractable(false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.1f);
			PlayerSingleton<PlayerCamera>.Instance.StartCameraShake(0.25f, 0.2f, true);
			this.press.StartCoroutine(this.<FinishPress>g__Wait|16_0());
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x00055271 File Offset: 0x00053471
		[CompilerGenerated]
		private IEnumerator <.ctor>g__CheckMould|11_0()
		{
			while (base.TaskActive)
			{
				this.CheckMould();
				yield return new WaitForSeconds(0.2f);
			}
			yield break;
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00055280 File Offset: 0x00053480
		[CompilerGenerated]
		private IEnumerator <FinishPress>g__Wait|16_0()
		{
			yield return new WaitForSeconds(0.8f);
			this.StopTask();
			yield break;
		}

		// Token: 0x0400126D RID: 4717
		public const float PRODUCT_SCALE = 0.75f;

		// Token: 0x0400126F RID: 4719
		protected UseBrickPress.EStep currentStep;

		// Token: 0x04001270 RID: 4720
		protected BrickPress press;

		// Token: 0x04001271 RID: 4721
		protected ProductItemInstance product;

		// Token: 0x04001272 RID: 4722
		protected List<FunctionalProduct> products = new List<FunctionalProduct>();

		// Token: 0x04001273 RID: 4723
		protected Draggable container;

		// Token: 0x02000354 RID: 852
		public enum EStep
		{
			// Token: 0x04001275 RID: 4725
			Pouring,
			// Token: 0x04001276 RID: 4726
			Pressing,
			// Token: 0x04001277 RID: 4727
			Complete
		}
	}
}
