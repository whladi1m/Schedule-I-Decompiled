using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A13 RID: 2579
	public class StaminaBar : MonoBehaviour
	{
		// Token: 0x0600459D RID: 17821 RVA: 0x00123FCA File Offset: 0x001221CA
		private void Awake()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			this.Group.alpha = 0f;
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x00123FFC File Offset: 0x001221FC
		private void PlayerSpawned()
		{
			PlayerMovement instance = PlayerSingleton<PlayerMovement>.Instance;
			instance.onStaminaReserveChanged = (Action<float>)Delegate.Combine(instance.onStaminaReserveChanged, new Action<float>(this.UpdateStaminaBar));
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x00124024 File Offset: 0x00122224
		private void UpdateStaminaBar(float change)
		{
			if (!PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				return;
			}
			Slider[] sliders = this.Sliders;
			for (int i = 0; i < sliders.Length; i++)
			{
				sliders[i].value = PlayerSingleton<PlayerMovement>.Instance.CurrentStaminaReserve / PlayerMovement.StaminaReserveMax;
			}
			this.Group.alpha = 1f;
			if (this.routine != null)
			{
				base.StopCoroutine(this.routine);
			}
			this.routine = base.StartCoroutine(this.<UpdateStaminaBar>g__Routine|7_0());
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x0012409C File Offset: 0x0012229C
		[CompilerGenerated]
		private IEnumerator <UpdateStaminaBar>g__Routine|7_0()
		{
			yield return new WaitForSeconds(1.5f);
			for (float i = 0f; i < 0.5f; i += Time.deltaTime)
			{
				this.Group.alpha = Mathf.Lerp(1f, 0f, i / 0.5f);
				yield return new WaitForEndOfFrame();
			}
			this.Group.alpha = 0f;
			this.routine = null;
			yield break;
		}

		// Token: 0x0400337B RID: 13179
		public const float StaminaShowTime = 1.5f;

		// Token: 0x0400337C RID: 13180
		public const float StaminaFadeTime = 0.5f;

		// Token: 0x0400337D RID: 13181
		[Header("References")]
		public Slider[] Sliders;

		// Token: 0x0400337E RID: 13182
		public CanvasGroup Group;

		// Token: 0x0400337F RID: 13183
		private Coroutine routine;
	}
}
