using System;
using Beautify.Universal;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace ScheduleOne.FX
{
	// Token: 0x02000616 RID: 1558
	public class PlayerHealthVisuals : MonoBehaviour
	{
		// Token: 0x06002900 RID: 10496 RVA: 0x000A911C File Offset: 0x000A731C
		private void Awake()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.Spawned));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.Spawned));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			this.GlobalVolume.sharedProfile.TryGet<Beautify>(out this._beautifySettings);
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x000A91CC File Offset: 0x000A73CC
		private void Spawned()
		{
			if (!Player.Local.Owner.IsLocalClient)
			{
				return;
			}
			this.UpdateEffects(Player.Local.Health.CurrentHealth);
			Player.Local.Health.onHealthChanged.AddListener(new UnityAction<float>(this.UpdateEffects));
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x000A9220 File Offset: 0x000A7420
		private void MinPass()
		{
			this._beautifySettings.vignettingOuterRing.value = this.OuterRingCurve.Evaluate(NetworkSingleton<TimeManager>.Instance.NormalizedTime);
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x000A9248 File Offset: 0x000A7448
		private void UpdateEffects(float newHealth)
		{
			this._beautifySettings.vignettingColor.value = new Color(this._beautifySettings.vignettingColor.value.r, this._beautifySettings.vignettingColor.value.g, this._beautifySettings.vignettingColor.value.b, Mathf.Lerp(this.VignetteAlpha_MinHealth, this.VignetteAlpha_MaxHealth, newHealth / 100f));
			this._beautifySettings.saturate.value = Mathf.Lerp(this.Saturation_MinHealth, this.Saturation_MaxHealth, newHealth / 100f);
			this._beautifySettings.chromaticAberrationIntensity.value = Mathf.Lerp(this.ChromAb_MinHealth, this.ChromAb_MaxHealth, newHealth / 100f);
			this._beautifySettings.lensDirtIntensity.value = Mathf.Lerp(this.LensDirt_MinHealth, this.LensDirt_MaxHealth, newHealth / 100f);
		}

		// Token: 0x04001E3E RID: 7742
		[Header("References")]
		public Volume GlobalVolume;

		// Token: 0x04001E3F RID: 7743
		[Header("Vignette")]
		public float VignetteAlpha_MaxHealth;

		// Token: 0x04001E40 RID: 7744
		public float VignetteAlpha_MinHealth;

		// Token: 0x04001E41 RID: 7745
		public AnimationCurve OuterRingCurve;

		// Token: 0x04001E42 RID: 7746
		[Header("Saturation")]
		public float Saturation_MaxHealth = 0.5f;

		// Token: 0x04001E43 RID: 7747
		public float Saturation_MinHealth = -2f;

		// Token: 0x04001E44 RID: 7748
		[Header("Chromatic Abberation")]
		public float ChromAb_MaxHealth;

		// Token: 0x04001E45 RID: 7749
		public float ChromAb_MinHealth = 0.02f;

		// Token: 0x04001E46 RID: 7750
		[Header("Lens Dirt")]
		public float LensDirt_MaxHealth;

		// Token: 0x04001E47 RID: 7751
		public float LensDirt_MinHealth = 1f;

		// Token: 0x04001E48 RID: 7752
		private Beautify _beautifySettings;
	}
}
