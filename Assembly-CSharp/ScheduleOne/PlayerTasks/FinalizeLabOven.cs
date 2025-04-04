using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Stations;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000346 RID: 838
	public class FinalizeLabOven : Task
	{
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0005296E File Offset: 0x00050B6E
		// (set) Token: 0x060012D2 RID: 4818 RVA: 0x00052976 File Offset: 0x00050B76
		public LabOven Oven { get; private set; }

		// Token: 0x060012D3 RID: 4819 RVA: 0x00052980 File Offset: 0x00050B80
		public FinalizeLabOven(LabOven oven)
		{
			this.Oven = oven;
			this.hammer = oven.CreateHammer();
			this.hammer.onCollision.AddListener(new UnityAction<Collision>(this.Collision));
			this.startSequence = this.Oven.StartCoroutine(this.StartSequence());
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x000529EF File Offset: 0x00050BEF
		public override void Update()
		{
			base.Update();
			this.timeSinceLastImpact += Time.deltaTime;
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00052A0C File Offset: 0x00050C0C
		public override void StopTask()
		{
			if (this.startSequence != null)
			{
				this.Oven.StopCoroutine(this.startSequence);
			}
			UnityEngine.Object.Destroy(this.hammer.gameObject);
			this.Oven.RemoveTrayAnimation.Stop();
			this.Oven.ResetSquareTray();
			this.Oven.ClearDecals();
			this.Oven.OutputVisuals.BlockRefreshes = false;
			this.Oven.OutputVisuals.RefreshVisuals();
			this.Oven.Door.SetPosition(0f);
			this.Oven.Door.SetInteractable(false);
			this.Oven.WireTray.SetPosition(0f);
			this.Oven.Button.SetInteractable(false);
			this.Oven.ClearShards();
			Singleton<LabOvenCanvas>.Instance.SetIsOpen(this.Oven, true, true);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Oven.CameraPosition_Default.position, this.Oven.CameraPosition_Default.rotation, 0.2f, false);
			base.StopTask();
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00052B3C File Offset: 0x00050D3C
		private IEnumerator StartSequence()
		{
			this.Oven.Door.SetPosition(1f);
			this.Oven.WireTray.SetPosition(1f);
			yield return new WaitForSeconds(0.5f);
			this.Oven.SquareTray.SetParent(this.Oven.transform);
			this.Oven.RemoveTrayAnimation.Play();
			yield return new WaitForSeconds(0.1f);
			this.Oven.Door.SetPosition(0f);
			yield return new WaitForSeconds(0.4f);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.Oven.CameraPosition_Breaking.position, this.Oven.CameraPosition_Breaking.rotation, 0.25f, false);
			base.CurrentInstruction = "Use hammer to break up the product";
			yield break;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00052B4C File Offset: 0x00050D4C
		public void Collision(Collision col)
		{
			if (col.collider != this.Oven.CookedLiquidCollider)
			{
				return;
			}
			if (this.hammer.VelocityCalculator.Velocity.magnitude < this.SMASH_VELOCITY_THRESHOLD)
			{
				return;
			}
			if (!this.hammer.Draggable.IsHeld)
			{
				return;
			}
			if (this.timeSinceLastImpact < 0.1f)
			{
				return;
			}
			ContactPoint[] array = new ContactPoint[col.contactCount];
			col.GetContacts(array);
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < array.Length; i++)
			{
				if (Vector3.Distance(array[i].point, this.hammer.ImpactPoint.position) < 0.1f)
				{
					vector = array[i].point;
					break;
				}
			}
			if (vector == Vector3.zero)
			{
				return;
			}
			this.timeSinceLastImpact = 0f;
			this.impactCount++;
			this.Oven.CreateImpactEffects(vector, true);
			if (this.impactCount == 3)
			{
				this.Shatter();
			}
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00052C54 File Offset: 0x00050E54
		private void Shatter()
		{
			int num = this.Oven.CurrentOperation.Cookable.ProductQuantity * this.Oven.CurrentOperation.IngredientQuantity;
			this.Oven.Shatter(num, this.Oven.CurrentOperation.Cookable.ProductShardPrefab.gameObject);
			this.Oven.OutputVisuals.BlockRefreshes = true;
			ItemInstance productItem = this.Oven.CurrentOperation.GetProductItem(num);
			this.Oven.OutputSlot.AddItem(productItem, false);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Oven_Cooks_Completed", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Oven_Cooks_Completed") + 1f).ToString(), true);
			this.Oven.SendCookOperation(null);
			this.Oven.StartCoroutine(this.<Shatter>g__Routine|16_0());
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00052D2E File Offset: 0x00050F2E
		[CompilerGenerated]
		private IEnumerator <Shatter>g__Routine|16_0()
		{
			yield return new WaitForSeconds(1.4f);
			if (base.TaskActive)
			{
				this.Success();
			}
			yield break;
		}

		// Token: 0x0400122A RID: 4650
		public const float MAX_DISTANCE_FROM_IMPACT_POINT = 0.1f;

		// Token: 0x0400122B RID: 4651
		public float SMASH_VELOCITY_THRESHOLD = 1.2f;

		// Token: 0x0400122C RID: 4652
		public const int REQUIRED_IMPACTS = 3;

		// Token: 0x0400122E RID: 4654
		private Coroutine startSequence;

		// Token: 0x0400122F RID: 4655
		private LabOvenHammer hammer;

		// Token: 0x04001230 RID: 4656
		private int impactCount;

		// Token: 0x04001231 RID: 4657
		private float timeSinceLastImpact = 100f;
	}
}
