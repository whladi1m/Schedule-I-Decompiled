using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x020008FA RID: 2298
	public class Equippable_Cuke : Equippable_Viewmodel
	{
		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x00107131 File Offset: 0x00105331
		// (set) Token: 0x06003E4F RID: 15951 RVA: 0x00107139 File Offset: 0x00105339
		public bool IsDrinking { get; protected set; }

		// Token: 0x06003E50 RID: 15952 RVA: 0x00107142 File Offset: 0x00105342
		protected override void Update()
		{
			base.Update();
			if (this.IsDrinking)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && !GameInput.IsTyping && PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0)
			{
				this.Drink();
			}
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x00107174 File Offset: 0x00105374
		public void Drink()
		{
			this.IsDrinking = true;
			base.StartCoroutine(this.<Drink>g__DrinkRoutine|16_0());
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x0010718C File Offset: 0x0010538C
		public void ApplyEffects()
		{
			float num = Mathf.Pow(this.ConsecutiveReduction, (float)Player.Local.Energy.EnergyDrinksConsumed);
			float num2 = Mathf.Clamp(this.BaseEnergyGain * num, this.MinEnergyGain, this.BaseEnergyGain);
			Player.Local.Energy.SetEnergy(Player.Local.Energy.CurrentEnergy + num2);
			PlayerSingleton<PlayerMovement>.Instance.SetStamina(PlayerMovement.StaminaReserveMax, true);
			if (this.HealthGain > 0f)
			{
				Player.Local.Health.RecoverHealth(this.HealthGain);
			}
			Player.Local.Energy.IncrementEnergyDrinks();
			if (this.ClearDrugEffects && Player.Local.ConsumedProduct != null)
			{
				Player.Local.ClearProduct();
			}
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x00107282 File Offset: 0x00105482
		[CompilerGenerated]
		private IEnumerator <Drink>g__DrinkRoutine|16_0()
		{
			this.OpenAnim.Play();
			this.DrinkAnim.Play();
			this.OpenSound.Play();
			this.SlurpSound.Play();
			yield return new WaitForSeconds(this.AnimationDuration);
			this.ApplyEffects();
			this.TrashPrefab = NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.TrashPrefab.ID, PlayerSingleton<PlayerCamera>.Instance.transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.up * 0.3f, PlayerSingleton<PlayerCamera>.Instance.transform.rotation, PlayerSingleton<PlayerMovement>.Instance.Controller.velocity + (PlayerSingleton<PlayerCamera>.Instance.transform.forward + PlayerSingleton<PlayerCamera>.Instance.transform.up * 0.25f) * 4f, "", false);
			this.itemInstance.ChangeQuantity(-1);
			if (this.itemInstance.Quantity > 0)
			{
				PlayerSingleton<PlayerInventory>.Instance.Reequip();
			}
			yield break;
		}

		// Token: 0x04002CD3 RID: 11475
		[Header("Settings")]
		public float BaseEnergyGain = 100f;

		// Token: 0x04002CD4 RID: 11476
		public float MinEnergyGain = 2.5f;

		// Token: 0x04002CD5 RID: 11477
		public float ConsecutiveReduction = 0.5f;

		// Token: 0x04002CD6 RID: 11478
		public float HealthGain;

		// Token: 0x04002CD7 RID: 11479
		public float AnimationDuration = 2f;

		// Token: 0x04002CD8 RID: 11480
		public bool ClearDrugEffects;

		// Token: 0x04002CD9 RID: 11481
		[Header("References")]
		public Animation OpenAnim;

		// Token: 0x04002CDA RID: 11482
		public Animation DrinkAnim;

		// Token: 0x04002CDB RID: 11483
		public AudioSourceController OpenSound;

		// Token: 0x04002CDC RID: 11484
		public AudioSourceController SlurpSound;

		// Token: 0x04002CDD RID: 11485
		public TrashItem TrashPrefab;
	}
}
