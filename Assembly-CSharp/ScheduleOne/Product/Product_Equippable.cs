using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Product
{
	// Token: 0x020008E4 RID: 2276
	public class Product_Equippable : Equippable_Viewmodel
	{
		// Token: 0x06003DF9 RID: 15865 RVA: 0x00105AE4 File Offset: 0x00103CE4
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			ProductItemInstance productItemInstance = item as ProductItemInstance;
			this.productAmount = productItemInstance.Amount;
			if (this.Consumable && this.productAmount == 1)
			{
				Singleton<InputPromptsCanvas>.Instance.LoadModule("consumable");
				Singleton<InputPromptsCanvas>.Instance.currentModule.gameObject.GetComponentsInChildren<Transform>().FirstOrDefault((Transform c) => c.gameObject.name == "Label").GetComponent<TextMeshProUGUI>().text = "(Hold) " + this.ConsumeDescription;
			}
			productItemInstance.SetupPackagingVisuals(this.Visuals);
			if (this.ModelContainer == null)
			{
				Console.LogWarning("Model container not set for equippable product: " + item.Name, null);
				this.ModelContainer = base.transform.GetChild(0);
			}
			this.defaultModelPosition = this.ModelContainer.localPosition;
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x00105BD0 File Offset: 0x00103DD0
		public override void Unequip()
		{
			if (this.Consumable)
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			if (base.transform.IsChildOf(Player.Local.transform))
			{
				if (!string.IsNullOrEmpty(this.ConsumeAnimationTrigger))
				{
					Player.Local.SendAnimationTrigger(this.ConsumeAnimationTrigger);
				}
				else if (!string.IsNullOrEmpty(this.ConsumeAnimationBool))
				{
					Player.Local.SendAnimationBool(this.ConsumeAnimationBool, false);
				}
			}
			if (this.consumingInProgress)
			{
				base.StopCoroutine(this.consumeRoutine);
			}
			base.Unequip();
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x00105C60 File Offset: 0x00103E60
		protected override void Update()
		{
			base.Update();
			Vector3 b = this.defaultModelPosition;
			if (this.Consumable && !this.consumingInProgress && GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && this.productAmount == 1 && !Singleton<PauseMenu>.Instance.IsPaused)
			{
				if (this.consumeTime == 0f && this.onConsumeInputStart != null)
				{
					this.onConsumeInputStart.Invoke();
				}
				this.consumeTime += Time.deltaTime;
				Singleton<HUD>.Instance.ShowRadialIndicator(this.consumeTime / this.ConsumeTime);
				if (this.consumeTime >= this.ConsumeTime)
				{
					this.Consume();
					if (this.onConsumeInputComplete != null)
					{
						this.onConsumeInputComplete.Invoke();
					}
				}
			}
			else
			{
				if (this.consumeTime > 0f && this.onConsumeInputCancel != null && !this.consumingInProgress)
				{
					this.onConsumeInputCancel.Invoke();
					if (base.transform.IsChildOf(Player.Local.transform) && !string.IsNullOrEmpty(this.ConsumeAnimationBool))
					{
						Player.Local.SendAnimationBool(this.ConsumeAnimationBool, false);
					}
				}
				this.consumeTime = 0f;
			}
			if (this.consumeTime > 0f || this.consumingInProgress)
			{
				b = this.defaultModelPosition - this.ModelContainer.transform.parent.InverseTransformDirection(PlayerSingleton<PlayerCamera>.Instance.transform.up) * 0.25f;
			}
			this.ModelContainer.transform.localPosition = Vector3.Lerp(this.ModelContainer.transform.localPosition, b, Time.deltaTime * 6f);
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x00105E10 File Offset: 0x00104010
		protected virtual void Consume()
		{
			this.consumingInProgress = true;
			if (base.transform.IsChildOf(Player.Local.transform))
			{
				if (!string.IsNullOrEmpty(this.ConsumeAnimationTrigger))
				{
					Player.Local.SendAnimationTrigger(this.ConsumeAnimationTrigger);
				}
				else if (!string.IsNullOrEmpty(this.ConsumeAnimationBool))
				{
					Player.Local.SendAnimationBool(this.ConsumeAnimationBool, true);
				}
				if (this.ConsumeEquippableAssetPath != string.Empty)
				{
					Player.Local.SendEquippable_Networked(this.ConsumeEquippableAssetPath);
				}
			}
			this.consumeRoutine = base.StartCoroutine(this.<Consume>g__ConsumeRoutine|20_0());
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x00105EAC File Offset: 0x001040AC
		protected virtual void ApplyEffects()
		{
			Player.Local.ConsumeProduct(this.itemInstance as ProductItemInstance);
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x00105F32 File Offset: 0x00104132
		[CompilerGenerated]
		private IEnumerator <Consume>g__ConsumeRoutine|20_0()
		{
			yield return new WaitForSeconds(this.EffectsApplyDelay);
			this.consumingInProgress = false;
			this.ApplyEffects();
			this.itemInstance.ChangeQuantity(-1);
			yield break;
		}

		// Token: 0x04002C7F RID: 11391
		[Header("References")]
		public FilledPackagingVisuals Visuals;

		// Token: 0x04002C80 RID: 11392
		public Transform ModelContainer;

		// Token: 0x04002C81 RID: 11393
		[Header("Settings")]
		public bool Consumable = true;

		// Token: 0x04002C82 RID: 11394
		public string ConsumeDescription = "Smoke";

		// Token: 0x04002C83 RID: 11395
		public float ConsumeTime = 1.5f;

		// Token: 0x04002C84 RID: 11396
		public float EffectsApplyDelay = 2f;

		// Token: 0x04002C85 RID: 11397
		public string ConsumeAnimationBool = string.Empty;

		// Token: 0x04002C86 RID: 11398
		public string ConsumeAnimationTrigger = string.Empty;

		// Token: 0x04002C87 RID: 11399
		public string ConsumeEquippableAssetPath = string.Empty;

		// Token: 0x04002C88 RID: 11400
		[Header("Events")]
		public UnityEvent onConsumeInputStart;

		// Token: 0x04002C89 RID: 11401
		public UnityEvent onConsumeInputComplete;

		// Token: 0x04002C8A RID: 11402
		public UnityEvent onConsumeInputCancel;

		// Token: 0x04002C8B RID: 11403
		private float consumeTime;

		// Token: 0x04002C8C RID: 11404
		private bool consumingInProgress;

		// Token: 0x04002C8D RID: 11405
		private Vector3 defaultModelPosition = Vector3.zero;

		// Token: 0x04002C8E RID: 11406
		private int productAmount = 1;

		// Token: 0x04002C8F RID: 11407
		private Coroutine consumeRoutine;
	}
}
