using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x02000940 RID: 2368
	public class AvatarEffects : MonoBehaviour
	{
		// Token: 0x06004065 RID: 16485 RVA: 0x0010EB6C File Offset: 0x0010CD6C
		private void Start()
		{
			this.AdditionalWeightController.Initialize();
			this.AdditionalWeightController.SetDefault(0f);
			this.AdditionalGenderController.Initialize();
			this.AdditionalGenderController.SetDefault(0f);
			this.HeadSizeBoost.Initialize();
			this.HeadSizeBoost.SetDefault(0f);
			this.NeckSizeBoost.Initialize();
			this.NeckSizeBoost.SetDefault(0f);
			this.SkinColorSmoother.Initialize();
			if (this.Avatar.CurrentSettings != null)
			{
				this.SetDefaultSkinColor(true);
			}
			this.ZapLoopSound.VolumeMultiplier = 0f;
			this.Avatar.onSettingsLoaded.AddListener(delegate()
			{
				this.SetDefaultSkinColor(true);
			});
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0010EC38 File Offset: 0x0010CE38
		public void FixedUpdate()
		{
			this.SetEffectsCulled(this.Avatar.Anim.IsAvatarCulled);
			if (!this.Avatar.Anim.enabled)
			{
				return;
			}
			if (this.Avatar.Anim.IsAvatarCulled)
			{
				return;
			}
			this.Avatar.SetAdditionalWeight(this.AdditionalWeightController.CurrentValue);
			this.Avatar.SetAdditionalGender(this.AdditionalGenderController.CurrentValue);
			this.Avatar.SetSkinColor(this.SkinColorSmoother.CurrentValue);
			this.currentEmission = Color.Lerp(this.currentEmission, this.targetEmission, Time.deltaTime * 0.5f);
			this.Avatar.SetEmission(this.currentEmission);
			if (this.DisableHead)
			{
				this.HeadBone.transform.localScale = Vector3.zero;
			}
			else
			{
				this.HeadBone.transform.localScale = Vector3.one * (1f + this.HeadSizeBoost.CurrentValue);
			}
			this.NeckBone.transform.localScale = Vector3.one * (1f + this.NeckSizeBoost.CurrentValue);
			if (this.FireParticles.isPlaying)
			{
				this.FireSound.VolumeMultiplier = Mathf.MoveTowards(this.FireSound.VolumeMultiplier, 1f, Time.deltaTime);
				if (!this.FireSound.isPlaying)
				{
					this.FireSound.Play();
				}
			}
			else
			{
				this.FireSound.VolumeMultiplier = Mathf.MoveTowards(this.FireSound.VolumeMultiplier, 0f, Time.deltaTime);
				if (this.FireSound.VolumeMultiplier <= 0f)
				{
					this.FireSound.Stop();
				}
			}
			if (this.ZapParticles.isPlaying)
			{
				this.ZapLoopSound.VolumeMultiplier = Mathf.MoveTowards(this.ZapLoopSound.VolumeMultiplier, 1f, Time.deltaTime * 2f);
				if (!this.ZapLoopSound.isPlaying)
				{
					this.ZapLoopSound.Play();
					return;
				}
			}
			else
			{
				this.ZapLoopSound.VolumeMultiplier = Mathf.MoveTowards(this.ZapLoopSound.VolumeMultiplier, 0f, Time.deltaTime * 2f);
				if (this.ZapLoopSound.VolumeMultiplier <= 0f)
				{
					this.ZapLoopSound.Stop();
				}
			}
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x0010EE94 File Offset: 0x0010D094
		private void SetEffectsCulled(bool culled)
		{
			if (this.isCulled == culled)
			{
				return;
			}
			this.isCulled = culled;
			GameObject[] objectsToCull = this.ObjectsToCull;
			for (int i = 0; i < objectsToCull.Length; i++)
			{
				objectsToCull[i].SetActive(!culled);
			}
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0010EED4 File Offset: 0x0010D0D4
		public void SetStinkParticlesActive(bool active, bool mirror = true)
		{
			foreach (ParticleSystem particleSystem in this.StinkParticles)
			{
				if (active)
				{
					particleSystem.Play();
				}
				else
				{
					particleSystem.Stop();
				}
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetStinkParticlesActive(active, false);
				}
			}
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0010EF30 File Offset: 0x0010D130
		public void TriggerSick(bool mirror = true)
		{
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].TriggerSick(false);
				}
			}
			base.StartCoroutine(this.<TriggerSick>g__Routine|36_0());
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x0010EF6C File Offset: 0x0010D16C
		public void SetAntiGrav(bool active, bool mirror = true)
		{
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetAntiGrav(active, false);
				}
			}
			if (active)
			{
				this.AntiGravParticles.Play();
				return;
			}
			this.AntiGravParticles.Stop();
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x0010EFB8 File Offset: 0x0010D1B8
		public void SetFoggy(bool active, bool mirror = true)
		{
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetFoggy(active, false);
				}
			}
			if (active)
			{
				this.FoggyEffects.Play();
				return;
			}
			this.FoggyEffects.Stop();
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x0010F004 File Offset: 0x0010D204
		public void VanishHair(bool mirror = true)
		{
			this.HeadPoofParticles.Play();
			this.PoofSound.Play();
			this.Avatar.SetHairVisible(false);
			this.Avatar.EyeBrows.leftBrow.gameObject.SetActive(false);
			this.Avatar.EyeBrows.rightBrow.gameObject.SetActive(false);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].VanishHair(false);
				}
			}
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0010F08C File Offset: 0x0010D28C
		public void SetZapped(bool zapped, bool mirror = true)
		{
			if (zapped)
			{
				LayerUtility.SetLayerRecursively(this.ZapParticles.gameObject, LayerMask.NameToLayer("Default"));
				this.ZapParticles.Play();
				this.ZapSound.Play();
			}
			else
			{
				this.ZapParticles.Stop();
				this.ZapSound.Stop();
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetZapped(zapped, false);
				}
			}
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x0010F108 File Offset: 0x0010D308
		public void ReturnHair(bool mirror = true)
		{
			this.HeadPoofParticles.Play();
			this.PoofSound.Play();
			this.Avatar.SetHairVisible(true);
			this.Avatar.EyeBrows.leftBrow.gameObject.SetActive(true);
			this.Avatar.EyeBrows.rightBrow.gameObject.SetActive(true);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].ReturnHair(false);
				}
			}
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x0010F190 File Offset: 0x0010D390
		public void OverrideHairColor(Color color, bool mirror = true)
		{
			this.HeadPoofParticles.Play();
			this.PoofSound.Play();
			this.Avatar.OverrideHairColor(color);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].OverrideHairColor(color, false);
				}
			}
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x0010F1E4 File Offset: 0x0010D3E4
		public void ResetHairColor(bool mirror = true)
		{
			this.HeadPoofParticles.Play();
			this.PoofSound.Play();
			this.Avatar.ResetHairColor();
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].ResetHairColor(false);
				}
			}
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x0010F234 File Offset: 0x0010D434
		public void OverrideEyeColor(Color color, float emission = 0.115f, bool mirror = true)
		{
			this.Avatar.Eyes.rightEye.SetEyeballColor(color, emission, false);
			this.Avatar.Eyes.leftEye.SetEyeballColor(color, emission, false);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].OverrideEyeColor(color, emission, false);
				}
			}
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x0010F294 File Offset: 0x0010D494
		public void ResetEyeColor(bool mirror = true)
		{
			this.Avatar.Eyes.rightEye.ResetEyeballColor();
			this.Avatar.Eyes.leftEye.ResetEyeballColor();
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].ResetEyeColor(false);
				}
			}
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x0010F2EC File Offset: 0x0010D4EC
		public void SetEyeLightEmission(float intensity, Color color, bool mirror = true)
		{
			this.Avatar.Eyes.rightEye.ConfigureEyeLight(color, intensity);
			this.Avatar.Eyes.leftEye.ConfigureEyeLight(color, intensity);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetEyeLightEmission(intensity, color, false);
				}
			}
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0010F34C File Offset: 0x0010D54C
		public void EnableLaxative(bool mirror = true)
		{
			this.laxativeEnabled = true;
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].EnableLaxative(false);
				}
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<EnableLaxative>g__Routine|47_0());
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x0010F394 File Offset: 0x0010D594
		public void DisableLaxative(bool mirror = true)
		{
			this.laxativeEnabled = false;
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].DisableLaxative(false);
				}
			}
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x0010F3CC File Offset: 0x0010D5CC
		public void SetFireActive(bool active, bool mirror = true)
		{
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetFireActive(active, false);
				}
			}
			this.FireLight.Enabled = active;
			if (active)
			{
				this.FireParticles.Play();
				return;
			}
			this.FireParticles.Stop();
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x0010F424 File Offset: 0x0010D624
		public void SetBigHeadActive(bool active, bool mirror = true)
		{
			if (active)
			{
				this.HeadSizeBoost.AddOverride(0.4f, 7, "big head");
			}
			else
			{
				this.HeadSizeBoost.RemoveOverride("big head");
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetBigHeadActive(active, false);
				}
			}
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x0010F480 File Offset: 0x0010D680
		public void SetGiraffeActive(bool active, bool mirror = true)
		{
			if (active)
			{
				this.HeadSizeBoost.AddOverride(-0.5f, 8, "giraffe");
				this.NeckSizeBoost.AddOverride(1f, 8, "giraffe");
			}
			else
			{
				this.HeadSizeBoost.RemoveOverride("giraffe");
				this.NeckSizeBoost.RemoveOverride("giraffe");
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetGiraffeActive(active, false);
				}
			}
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x0010F500 File Offset: 0x0010D700
		public void SetSkinColorInverted(bool inverted, bool mirror = true)
		{
			if (inverted)
			{
				if (this.Avatar.IsWhite())
				{
					this.SkinColorSmoother.AddOverride(new Color32(58, 49, 42, byte.MaxValue), 7, "inverted");
				}
				else
				{
					this.SkinColorSmoother.AddOverride(new Color32(223, 189, 161, byte.MaxValue), 7, "inverted");
				}
			}
			else
			{
				this.SkinColorSmoother.RemoveOverride("inverted");
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetSkinColorInverted(inverted, false);
				}
			}
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x0010F5A8 File Offset: 0x0010D7A8
		public void SetSicklySkinColor(bool mirror = true)
		{
			Color skinColor = this.Avatar.CurrentSettings.SkinColor;
			float num = 0.5f;
			float num2 = 0.3f * skinColor.r + 0.59f * skinColor.g + 0.11f * skinColor.b;
			Color color = Color.white;
			color.r = skinColor.r + (num2 - skinColor.r) * num;
			color.g = skinColor.g + (num2 - skinColor.g) * num;
			color.b = skinColor.b + (num2 - skinColor.b) * num;
			color *= 1.1f;
			string str = "Sickly Color: ";
			Color color2 = color;
			Console.Log(str + color2.ToString(), null);
			this.SkinColorSmoother.AddOverride(color, 6, "sickly");
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetSicklySkinColor(false);
				}
			}
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x0010F6A8 File Offset: 0x0010D8A8
		private void SetDefaultSkinColor(bool mirror = true)
		{
			if (this.Avatar.CurrentSettings == null)
			{
				return;
			}
			this.SkinColorSmoother.SetDefault(this.Avatar.CurrentSettings.SkinColor);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetDefaultSkinColor(false);
				}
			}
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x0010F708 File Offset: 0x0010D908
		public void SetGenderInverted(bool inverted, bool mirror = true)
		{
			if (inverted)
			{
				if (this.Avatar.IsMale())
				{
					this.AdditionalGenderController.AddOverride(1f, 7, "jennerising");
				}
				else
				{
					this.AdditionalGenderController.AddOverride(-1f, 7, "jennerising");
				}
			}
			else
			{
				this.AdditionalGenderController.RemoveOverride("jennerising");
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetGenderInverted(inverted, false);
				}
			}
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x0010F788 File Offset: 0x0010D988
		public void AddAdditionalWeightOverride(float value, int priority, string label, bool mirror = true)
		{
			this.AdditionalWeightController.AddOverride(value, priority, label);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].AddAdditionalWeightOverride(value, priority, label, false);
				}
			}
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x0010F7C8 File Offset: 0x0010D9C8
		public void RemoveAdditionalWeightOverride(string label, bool mirror = true)
		{
			this.AdditionalWeightController.RemoveOverride(label);
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].RemoveAdditionalWeightOverride(label, false);
				}
			}
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x0010F804 File Offset: 0x0010DA04
		public void SetGlowingOn(Color color, bool mirror = true)
		{
			this.targetEmission = color;
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetGlowingOn(color, false);
				}
			}
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x0010F83C File Offset: 0x0010DA3C
		public void SetGlowingOff(bool mirror = true)
		{
			this.targetEmission = Color.black;
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetGlowingOff(false);
				}
			}
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x0010F878 File Offset: 0x0010DA78
		public void TriggerCountdownExplosion(bool mirror = true)
		{
			this.CountdownExplosion.Trigger();
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].TriggerCountdownExplosion(false);
				}
			}
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x0010F8B4 File Offset: 0x0010DAB4
		public void StopCountdownExplosion(bool mirror = true)
		{
			this.CountdownExplosion.StopCountdown();
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].StopCountdownExplosion(false);
				}
			}
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x0010F8F0 File Offset: 0x0010DAF0
		public void SetCyclopean(bool enabled, bool mirror = true)
		{
			this.HeadPoofParticles.Play();
			this.PoofSound.Play();
			if (enabled)
			{
				this.Avatar.Eyes.rightEye.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
				this.Avatar.Eyes.rightEye.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
				this.Avatar.Eyes.leftEye.gameObject.SetActive(false);
				this.Avatar.SetBlockEyeFaceLayers(true);
			}
			else
			{
				this.Avatar.Eyes.rightEye.transform.localRotation = Quaternion.Euler(0f, 22f, 0f);
				this.Avatar.Eyes.rightEye.transform.localScale = new Vector3(1f, 1f, 1f);
				this.Avatar.Eyes.leftEye.gameObject.SetActive(true);
				this.Avatar.SetBlockEyeFaceLayers(false);
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetCyclopean(enabled, false);
				}
			}
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x0010FA48 File Offset: 0x0010DC48
		public void SetZombified(bool zombified, bool mirror = true)
		{
			if (zombified)
			{
				this.SkinColorSmoother.AddOverride(new Color32(117, 122, 92, byte.MaxValue), 10, "Zombified");
				this.Avatar.Eyes.leftEye.PupilContainer.gameObject.SetActive(!zombified);
				this.Avatar.Eyes.rightEye.PupilContainer.gameObject.SetActive(!zombified);
				this.OverrideEyeColor(new Color32(159, 129, 129, byte.MaxValue), 0.115f, false);
				this.Avatar.EmotionManager.AddEmotionOverride("Zombie", "Zombified", 0f, 10);
			}
			else
			{
				this.SkinColorSmoother.RemoveOverride("Zombified");
				this.Avatar.Eyes.leftEye.PupilContainer.gameObject.SetActive(true);
				this.Avatar.Eyes.rightEye.PupilContainer.gameObject.SetActive(true);
				this.ResetEyeColor(false);
				this.Avatar.EmotionManager.RemoveEmotionOverride("Zombified");
			}
			if (mirror)
			{
				AvatarEffects[] mirrorEffectsTo = this.MirrorEffectsTo;
				for (int i = 0; i < mirrorEffectsTo.Length; i++)
				{
					mirrorEffectsTo[i].SetZombified(zombified, false);
				}
			}
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x0010FBCA File Offset: 0x0010DDCA
		[CompilerGenerated]
		private IEnumerator <TriggerSick>g__Routine|36_0()
		{
			this.GurgleSound.Play();
			yield return new WaitForSeconds(4.5f);
			this.VomitSound.Play();
			this.VomitParticles.gameObject.layer = LayerMask.NameToLayer("Default");
			this.VomitParticles.Play();
			yield break;
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x0010FBD9 File Offset: 0x0010DDD9
		[CompilerGenerated]
		private IEnumerator <EnableLaxative>g__Routine|47_0()
		{
			do
			{
				this.FartParticles.Play();
				this.FartSound.Play();
				yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 20f));
			}
			while (this.laxativeEnabled);
			yield break;
		}

		// Token: 0x04002E55 RID: 11861
		[Header("References")]
		public Avatar Avatar;

		// Token: 0x04002E56 RID: 11862
		public ParticleSystem[] StinkParticles;

		// Token: 0x04002E57 RID: 11863
		public ParticleSystem VomitParticles;

		// Token: 0x04002E58 RID: 11864
		public ParticleSystem HeadPoofParticles;

		// Token: 0x04002E59 RID: 11865
		public ParticleSystem FartParticles;

		// Token: 0x04002E5A RID: 11866
		public ParticleSystem AntiGravParticles;

		// Token: 0x04002E5B RID: 11867
		public ParticleSystem FireParticles;

		// Token: 0x04002E5C RID: 11868
		public OptimizedLight FireLight;

		// Token: 0x04002E5D RID: 11869
		public ParticleSystem FoggyEffects;

		// Token: 0x04002E5E RID: 11870
		public Transform HeadBone;

		// Token: 0x04002E5F RID: 11871
		public Transform NeckBone;

		// Token: 0x04002E60 RID: 11872
		public AvatarEffects[] MirrorEffectsTo;

		// Token: 0x04002E61 RID: 11873
		public ParticleSystem ZapParticles;

		// Token: 0x04002E62 RID: 11874
		public CountdownExplosion CountdownExplosion;

		// Token: 0x04002E63 RID: 11875
		public GameObject[] ObjectsToCull;

		// Token: 0x04002E64 RID: 11876
		[Header("Settings")]
		public bool DisableHead;

		// Token: 0x04002E65 RID: 11877
		[Header("Sounds")]
		public AudioSourceController GurgleSound;

		// Token: 0x04002E66 RID: 11878
		public AudioSourceController VomitSound;

		// Token: 0x04002E67 RID: 11879
		public AudioSourceController PoofSound;

		// Token: 0x04002E68 RID: 11880
		public AudioSourceController FartSound;

		// Token: 0x04002E69 RID: 11881
		public AudioSourceController FireSound;

		// Token: 0x04002E6A RID: 11882
		public AudioSourceController ZapSound;

		// Token: 0x04002E6B RID: 11883
		public AudioSourceController ZapLoopSound;

		// Token: 0x04002E6C RID: 11884
		[Header("Smoothers")]
		[SerializeField]
		private FloatSmoother AdditionalWeightController;

		// Token: 0x04002E6D RID: 11885
		[SerializeField]
		private FloatSmoother AdditionalGenderController;

		// Token: 0x04002E6E RID: 11886
		[SerializeField]
		private FloatSmoother HeadSizeBoost;

		// Token: 0x04002E6F RID: 11887
		[SerializeField]
		private FloatSmoother NeckSizeBoost;

		// Token: 0x04002E70 RID: 11888
		[SerializeField]
		private ColorSmoother SkinColorSmoother;

		// Token: 0x04002E71 RID: 11889
		private bool laxativeEnabled;

		// Token: 0x04002E72 RID: 11890
		private Color currentEmission = Color.black;

		// Token: 0x04002E73 RID: 11891
		private Color targetEmission = Color.black;

		// Token: 0x04002E74 RID: 11892
		private bool isCulled;
	}
}
