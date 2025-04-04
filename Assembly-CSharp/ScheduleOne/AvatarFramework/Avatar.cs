using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.AvatarFramework.Animation;
using ScheduleOne.AvatarFramework.Emotions;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.AvatarFramework.Impostors;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x0200093E RID: 2366
	public class Avatar : MonoBehaviour
	{
		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06004031 RID: 16433 RVA: 0x0010D769 File Offset: 0x0010B969
		// (set) Token: 0x06004032 RID: 16434 RVA: 0x0010D771 File Offset: 0x0010B971
		public bool Ragdolled { get; protected set; }

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06004033 RID: 16435 RVA: 0x0010D77A File Offset: 0x0010B97A
		// (set) Token: 0x06004034 RID: 16436 RVA: 0x0010D782 File Offset: 0x0010B982
		public AvatarEquippable CurrentEquippable { get; protected set; }

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06004035 RID: 16437 RVA: 0x0010D78B File Offset: 0x0010B98B
		// (set) Token: 0x06004036 RID: 16438 RVA: 0x0010D793 File Offset: 0x0010B993
		public AvatarSettings CurrentSettings { get; protected set; }

		// Token: 0x06004037 RID: 16439 RVA: 0x0010D79C File Offset: 0x0010B99C
		[Button]
		public void Load()
		{
			this.LoadAvatarSettings(this.SettingsToLoad);
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x0010D7AA File Offset: 0x0010B9AA
		[Button]
		public void LoadNaked()
		{
			this.LoadNakedSettings(this.SettingsToLoad, 19);
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06004039 RID: 16441 RVA: 0x0010D7BA File Offset: 0x0010B9BA
		public Vector3 CenterPoint
		{
			get
			{
				return this.MiddleSpine.transform.position;
			}
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x0010D7CC File Offset: 0x0010B9CC
		protected virtual void Awake()
		{
			this.SetRagdollPhysicsEnabled(false, false);
			this.originalHipPos = this.HipBone.localPosition;
			if (this.InitialAvatarSettings != null)
			{
				this.LoadAvatarSettings(this.InitialAvatarSettings);
			}
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x0010D801 File Offset: 0x0010BA01
		protected virtual void Update()
		{
			if (!this.Ragdolled && this.Anim != null && !this.Anim.StandUpAnimationPlaying)
			{
				this.HipBone.localPosition = this.originalHipPos;
			}
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x0010D838 File Offset: 0x0010BA38
		protected virtual void LateUpdate()
		{
			if (!this.BodyContainer.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.CurrentSettings != null && !this.Anim.IsAvatarCulled)
			{
				Vector3 centerPoint = this.CenterPoint;
				if (PlayerSingleton<PlayerCamera>.InstanceExists && Vector3.SqrMagnitude(PlayerSingleton<PlayerCamera>.Instance.transform.position - this.CenterPoint) < 1600f * QualitySettings.lodBias)
				{
					this.ApplyShapeKeys(Mathf.Clamp01(this.appliedGender + this.additionalGender) * 100f, Mathf.Clamp01(this.appliedWeight + this.additionalWeight) * 100f, false);
				}
			}
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x0010D8E4 File Offset: 0x0010BAE4
		public void SetVisible(bool vis)
		{
			this.Eyes.SetEyesOpen(true);
			this.BodyContainer.gameObject.SetActive(vis);
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x0010D903 File Offset: 0x0010BB03
		public void GetMugshot(Action<Texture2D> callback)
		{
			Singleton<MugshotGenerator>.Instance.GenerateMugshot(this.CurrentSettings, false, callback);
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x0010D918 File Offset: 0x0010BB18
		public void SetEmission(Color color)
		{
			if (this.usingCombinedLayer)
			{
				this.BodyMeshes[0].sharedMaterial.SetColor("_EmissionColor", color);
				return;
			}
			SkinnedMeshRenderer[] bodyMeshes = this.BodyMeshes;
			for (int i = 0; i < bodyMeshes.Length; i++)
			{
				bodyMeshes[i].material.SetColor("_EmissionColor", color);
			}
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x0010D96E File Offset: 0x0010BB6E
		public bool IsMale()
		{
			return this.CurrentSettings == null || this.CurrentSettings.Gender < 0.5f;
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x0010D994 File Offset: 0x0010BB94
		public bool IsWhite()
		{
			return this.CurrentSettings == null || this.CurrentSettings.SkinColor.r + this.CurrentSettings.SkinColor.g + this.CurrentSettings.SkinColor.b > 1.5f;
		}

		// Token: 0x06004042 RID: 16450 RVA: 0x0010D9EC File Offset: 0x0010BBEC
		private void ApplyShapeKeys(float gender, float weight, bool bodyOnly = false)
		{
			bool enabled = true;
			if (this.Anim.animator != null)
			{
				enabled = this.Anim.animator.enabled;
				this.Anim.animator.enabled = false;
			}
			for (int i = 0; i < this.ShapeKeyMeshes.Length; i++)
			{
				if (this.ShapeKeyMeshes[i].sharedMesh.blendShapeCount >= 2)
				{
					this.ShapeKeyMeshes[i].SetBlendShapeWeight(0, gender);
					this.ShapeKeyMeshes[i].SetBlendShapeWeight(1, weight);
				}
			}
			float num = Mathf.Lerp(Avatar.maleShoulderScale, Avatar.femaleShoulderScale, gender / 100f);
			this.LeftShoulder.localScale = new Vector3(num, num, num);
			this.RightShoulder.localScale = new Vector3(num, num, num);
			if (this.Anim.animator != null)
			{
				this.Anim.animator.enabled = enabled;
			}
			if (bodyOnly)
			{
				return;
			}
			for (int j = 0; j < this.appliedAccessories.Length; j++)
			{
				if (this.appliedAccessories[j] != null)
				{
					this.appliedAccessories[j].ApplyShapeKeys(gender, weight);
				}
			}
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x0010DB10 File Offset: 0x0010BD10
		private void SetFeetShrunk(bool shrink, float reduction)
		{
			if (shrink)
			{
				for (int i = 0; i < this.BodyMeshes.Length; i++)
				{
					this.BodyMeshes[i].SetBlendShapeWeight(2, reduction * 100f);
				}
				return;
			}
			for (int j = 0; j < this.BodyMeshes.Length; j++)
			{
				this.BodyMeshes[j].SetBlendShapeWeight(2, 0f);
			}
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x0010DB6F File Offset: 0x0010BD6F
		private void SetWearingHairBlockingAccessory(bool blocked)
		{
			this.wearingHairBlockingAccessory = blocked;
			if (this.appliedHair != null)
			{
				this.appliedHair.SetBlockedByHat(blocked);
			}
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x0010DB94 File Offset: 0x0010BD94
		public void LoadAvatarSettings(AvatarSettings settings)
		{
			if (settings == null)
			{
				Console.LogWarning("LoadAvatarSettings: given settings are null", null);
				return;
			}
			this.CurrentSettings = settings;
			this.ApplyBodySettings(this.CurrentSettings);
			this.ApplyHairSettings(this.CurrentSettings);
			this.ApplyHairColorSettings(this.CurrentSettings);
			this.ApplyEyeLidSettings(this.CurrentSettings);
			this.ApplyEyeLidColorSettings(this.CurrentSettings);
			this.ApplyEyebrowSettings(this.CurrentSettings);
			this.ApplyEyeBallSettings(this.CurrentSettings);
			this.ApplyFaceLayerSettings(this.CurrentSettings);
			this.ApplyBodyLayerSettings(this.CurrentSettings, -1);
			this.ApplyAccessorySettings(this.CurrentSettings);
			FaceLayer faceLayer = Resources.Load(this.CurrentSettings.FaceLayer1Path) as FaceLayer;
			Texture2D faceTex = (faceLayer != null) ? faceLayer.Texture : null;
			this.EmotionManager.ConfigureNeutralFace(faceTex, this.CurrentSettings.EyebrowRestingHeight, this.CurrentSettings.EyebrowRestingAngle, this.CurrentSettings.LeftEyeRestingState, this.CurrentSettings.RightEyeRestingState);
			if (this.UseImpostor)
			{
				this.Impostor.SetAvatarSettings(this.CurrentSettings);
			}
			if (this.onSettingsLoaded != null)
			{
				this.onSettingsLoaded.Invoke();
			}
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x0010DCC4 File Offset: 0x0010BEC4
		public void LoadNakedSettings(AvatarSettings settings, int maxLayerOrder = 19)
		{
			if (settings == null)
			{
				Console.LogWarning("LoadAvatarSettings: given settings are null", null);
				return;
			}
			AvatarSettings currentSettings = this.CurrentSettings;
			this.CurrentSettings = settings;
			if (this.CurrentSettings == null)
			{
				this.CurrentSettings = new AvatarSettings();
			}
			this.CurrentSettings = UnityEngine.Object.Instantiate<AvatarSettings>(this.CurrentSettings);
			if (currentSettings != null)
			{
				this.CurrentSettings.BodyLayerSettings.AddRange(currentSettings.BodyLayerSettings);
			}
			this.ApplyBodySettings(this.CurrentSettings);
			this.ApplyHairSettings(this.CurrentSettings);
			this.ApplyHairColorSettings(this.CurrentSettings);
			this.ApplyEyeLidSettings(this.CurrentSettings);
			this.ApplyEyeLidColorSettings(this.CurrentSettings);
			this.ApplyEyebrowSettings(this.CurrentSettings);
			this.ApplyEyeBallSettings(this.CurrentSettings);
			this.ApplyFaceLayerSettings(this.CurrentSettings);
			this.ApplyBodyLayerSettings(this.CurrentSettings, maxLayerOrder);
			FaceLayer faceLayer = Resources.Load(this.CurrentSettings.FaceLayer1Path) as FaceLayer;
			Texture2D faceTex = (faceLayer != null) ? faceLayer.Texture : null;
			this.EmotionManager.ConfigureNeutralFace(faceTex, this.CurrentSettings.EyebrowRestingHeight, this.CurrentSettings.EyebrowRestingAngle, this.CurrentSettings.LeftEyeRestingState, this.CurrentSettings.RightEyeRestingState);
			if (this.onSettingsLoaded != null)
			{
				this.onSettingsLoaded.Invoke();
			}
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x0010DE20 File Offset: 0x0010C020
		public void ApplyBodySettings(AvatarSettings settings)
		{
			this.appliedGender = settings.Gender;
			this.appliedWeight = settings.Weight;
			this.CurrentSettings.SkinColor = settings.SkinColor;
			this.ApplyShapeKeys(settings.Gender * 100f, settings.Weight * 100f, false);
			base.transform.localScale = new Vector3(settings.Height, settings.Height, settings.Height);
			if (this.onSettingsLoaded != null)
			{
				this.onSettingsLoaded.Invoke();
			}
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x0010DEAA File Offset: 0x0010C0AA
		public void SetAdditionalWeight(float weight)
		{
			this.additionalWeight = weight;
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0010DEB3 File Offset: 0x0010C0B3
		public void SetAdditionalGender(float gender)
		{
			this.additionalGender = gender;
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x0010DEBC File Offset: 0x0010C0BC
		public void SetSkinColor(Color color)
		{
			if (this.usingCombinedLayer)
			{
				if (this.BodyMeshes[0].sharedMaterial.GetColor("_SkinColor") == color)
				{
					return;
				}
				this.BodyMeshes[0].sharedMaterial.SetColor("_SkinColor", color);
			}
			else
			{
				if (this.BodyMeshes[0].material.GetColor("_SkinColor") == color)
				{
					return;
				}
				SkinnedMeshRenderer[] bodyMeshes = this.BodyMeshes;
				for (int i = 0; i < bodyMeshes.Length; i++)
				{
					bodyMeshes[i].material.SetColor("_SkinColor", color);
				}
			}
			this.Eyes.leftEye.SetLidColor(color);
			this.Eyes.rightEye.SetLidColor(color);
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x0010DF78 File Offset: 0x0010C178
		public void ApplyHairSettings(AvatarSettings settings)
		{
			if (this.appliedHair != null)
			{
				UnityEngine.Object.Destroy(this.appliedHair.gameObject);
			}
			UnityEngine.Object @object = (settings.HairPath != null) ? Resources.Load(settings.HairPath) : null;
			if (@object != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(@object, this.HeadBone) as GameObject;
				this.appliedHair = gameObject.GetComponent<Hair>();
			}
			this.ApplyHairColorSettings(settings);
			if (this.appliedHair != null)
			{
				this.appliedHair.SetBlockedByHat(this.wearingHairBlockingAccessory);
			}
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x0010E007 File Offset: 0x0010C207
		public void SetHairVisible(bool visible)
		{
			if (this.appliedHair != null)
			{
				this.appliedHair.gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x0010E028 File Offset: 0x0010C228
		public void ApplyHairColorSettings(AvatarSettings settings)
		{
			this.appliedHairColor = settings.HairColor;
			if (this.appliedHair != null)
			{
				this.appliedHair.ApplyColor(this.appliedHairColor);
			}
			this.EyeBrows.ApplySettings(settings);
			this.SetFaceLayer(2, settings.FaceLayer2Path, settings.HairColor);
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x0010E080 File Offset: 0x0010C280
		public void OverrideHairColor(Color color)
		{
			if (this.appliedHair != null)
			{
				this.appliedHair.ApplyColor(color);
			}
			this.EyeBrows.leftBrow.SetColor(color);
			this.EyeBrows.rightBrow.SetColor(color);
			if (this.CurrentSettings != null)
			{
				this.SetFaceLayer(2, this.CurrentSettings.FaceLayer2Path, color);
			}
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x0010E0EC File Offset: 0x0010C2EC
		public void ResetHairColor()
		{
			if (this.CurrentSettings == null)
			{
				return;
			}
			if (this.appliedHair != null)
			{
				this.appliedHair.ApplyColor(this.CurrentSettings.HairColor);
			}
			this.EyeBrows.leftBrow.SetColor(this.CurrentSettings.HairColor);
			this.EyeBrows.rightBrow.SetColor(this.CurrentSettings.HairColor);
			this.SetFaceLayer(2, this.CurrentSettings.FaceLayer2Path, this.CurrentSettings.HairColor);
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0010E17F File Offset: 0x0010C37F
		public void ApplyEyeBallSettings(AvatarSettings settings)
		{
			this.Eyes.SetEyeballTint(settings.EyeBallTint);
			this.Eyes.SetPupilDilation(settings.PupilDilation, true);
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x0010E1A4 File Offset: 0x0010C3A4
		public void ApplyEyeLidSettings(AvatarSettings settings)
		{
			this.Eyes.SetLeftEyeRestingLidState(settings.LeftEyeRestingState);
			this.Eyes.SetRightEyeRestingLidState(settings.RightEyeRestingState);
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x0010E1C8 File Offset: 0x0010C3C8
		public void ApplyEyeLidColorSettings(AvatarSettings settings)
		{
			this.Eyes.leftEye.SetLidColor(settings.LeftEyeLidColor);
			this.Eyes.rightEye.SetLidColor(settings.RightEyeLidColor);
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x0010E1F6 File Offset: 0x0010C3F6
		public void ApplyEyebrowSettings(AvatarSettings settings)
		{
			this.EyeBrows.ApplySettings(settings);
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x0010E204 File Offset: 0x0010C404
		public void SetBlockEyeFaceLayers(bool block)
		{
			this.blockEyeFaceLayers = block;
			if (this.CurrentSettings != null)
			{
				this.ApplyFaceLayerSettings(this.CurrentSettings);
			}
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x0010E228 File Offset: 0x0010C428
		public void ApplyFaceLayerSettings(AvatarSettings settings)
		{
			for (int i = 1; i <= 6; i++)
			{
				this.SetFaceLayer(i, string.Empty, Color.white);
			}
			this.SetFaceLayer(1, settings.FaceLayer1Path, settings.FaceLayer1Color);
			this.SetFaceLayer(6, settings.FaceLayer2Path, settings.HairColor);
			List<Tuple<FaceLayer, Color>> list = new List<Tuple<FaceLayer, Color>>();
			for (int j = 2; j < settings.FaceLayerSettings.Count; j++)
			{
				if (!string.IsNullOrEmpty(settings.FaceLayerSettings[j].layerPath))
				{
					FaceLayer faceLayer = Resources.Load(settings.FaceLayerSettings[j].layerPath) as FaceLayer;
					if (!this.blockEyeFaceLayers || !faceLayer.Name.ToLower().Contains("eye"))
					{
						if (faceLayer != null)
						{
							list.Add(new Tuple<FaceLayer, Color>(faceLayer, settings.FaceLayerSettings[j].layerTint));
						}
						else
						{
							Console.LogWarning("Face layer not found at path " + settings.FaceLayerSettings[j].layerPath, null);
						}
					}
				}
			}
			list.Sort((Tuple<FaceLayer, Color> x, Tuple<FaceLayer, Color> y) => x.Item1.Order.CompareTo(y.Item1.Order));
			for (int k = 0; k < list.Count; k++)
			{
				this.SetFaceLayer(3 + k, list[k].Item1.AssetPath, list[k].Item2);
			}
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x0010E39C File Offset: 0x0010C59C
		private void SetFaceLayer(int index, string assetPath, Color color)
		{
			FaceLayer faceLayer = Resources.Load(assetPath) as FaceLayer;
			Texture2D texture2D = (faceLayer != null) ? faceLayer.Texture : null;
			if (texture2D == null)
			{
				color.a = 0f;
			}
			this.FaceMesh.material.SetTexture("_Layer_" + index.ToString() + "_Texture", texture2D);
			this.FaceMesh.material.SetColor("_Layer_" + index.ToString() + "_Color", color);
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x0010E42C File Offset: 0x0010C62C
		public void SetFaceTexture(Texture2D tex, Color color)
		{
			this.FaceMesh.material.SetTexture("_Layer_" + 1.ToString() + "_Texture", tex);
			this.FaceMesh.material.SetColor("_Layer_" + 1.ToString() + "_Color", color);
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x0010E48C File Offset: 0x0010C68C
		public void ApplyBodyLayerSettings(AvatarSettings settings, int maxOrder = -1)
		{
			for (int i = 1; i <= 6; i++)
			{
				this.SetBodyLayer(i, string.Empty, Color.white);
			}
			AvatarLayer avatarLayer = null;
			if (settings.UseCombinedLayer && settings.CombinedLayerPath != string.Empty)
			{
				avatarLayer = (Resources.Load(settings.CombinedLayerPath) as AvatarLayer);
			}
			if (avatarLayer != null)
			{
				this.usingCombinedLayer = true;
				SkinnedMeshRenderer[] bodyMeshes = this.BodyMeshes;
				for (int j = 0; j < bodyMeshes.Length; j++)
				{
					bodyMeshes[j].material = avatarLayer.CombinedMaterial;
				}
				return;
			}
			this.usingCombinedLayer = false;
			List<Tuple<AvatarLayer, Color>> list = new List<Tuple<AvatarLayer, Color>>();
			for (int k = 0; k < settings.BodyLayerSettings.Count; k++)
			{
				if (!string.IsNullOrEmpty(settings.BodyLayerSettings[k].layerPath))
				{
					AvatarLayer avatarLayer2 = Resources.Load(settings.BodyLayerSettings[k].layerPath) as AvatarLayer;
					if (maxOrder <= -1 || avatarLayer2.Order <= maxOrder)
					{
						if (avatarLayer2 != null)
						{
							list.Add(new Tuple<AvatarLayer, Color>(avatarLayer2, settings.BodyLayerSettings[k].layerTint));
						}
						else
						{
							Console.LogWarning("Body layer not found at path " + settings.BodyLayerSettings[k].layerPath, null);
						}
					}
				}
			}
			list.Sort((Tuple<AvatarLayer, Color> x, Tuple<AvatarLayer, Color> y) => x.Item1.Order.CompareTo(y.Item1.Order));
			for (int l = 0; l < list.Count; l++)
			{
				this.SetBodyLayer(l + 1, list[l].Item1.AssetPath, list[l].Item2);
			}
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x0010E640 File Offset: 0x0010C840
		private void SetBodyLayer(int index, string assetPath, Color color)
		{
			AvatarLayer avatarLayer = Resources.Load(assetPath) as AvatarLayer;
			Texture2D texture2D = (avatarLayer != null) ? avatarLayer.Texture : null;
			if (texture2D == null)
			{
				color.a = 0f;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.BodyMeshes)
			{
				if (skinnedMeshRenderer.material.shader != this.DefaultAvatarMaterial.shader)
				{
					skinnedMeshRenderer.material = new Material(this.DefaultAvatarMaterial);
				}
				skinnedMeshRenderer.material.SetTexture("_Layer_" + index.ToString() + "_Texture", texture2D);
				skinnedMeshRenderer.material.SetColor("_Layer_" + index.ToString() + "_Color", color);
				if (avatarLayer != null)
				{
					skinnedMeshRenderer.material.SetTexture("_Layer_" + index.ToString() + "_Normal", avatarLayer.Normal);
				}
			}
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x0010E748 File Offset: 0x0010C948
		public void ApplyAccessorySettings(AvatarSettings settings)
		{
			if (this.appliedAccessories.Length != 9)
			{
				this.DestroyAccessories();
				this.appliedAccessories = new Accessory[9];
			}
			bool shrink = false;
			float num = 0f;
			bool flag = false;
			for (int i = 0; i < 9; i++)
			{
				if (settings.AccessorySettings.Count > i && settings.AccessorySettings[i].path != string.Empty)
				{
					if (this.appliedAccessories[i] != null && this.appliedAccessories[i].AssetPath != settings.AccessorySettings[i].path)
					{
						UnityEngine.Object.Destroy(this.appliedAccessories[i].gameObject);
						this.appliedAccessories[i] = null;
					}
					if (this.appliedAccessories[i] == null)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(settings.AccessorySettings[i].path), this.BodyContainer) as GameObject;
						this.appliedAccessories[i] = gameObject.GetComponent<Accessory>();
						this.appliedAccessories[i].BindBones(this.BodyMeshes[0].bones);
						this.appliedAccessories[i].ApplyShapeKeys(this.appliedGender * 100f, this.appliedWeight * 100f);
					}
					if (this.appliedAccessories[i].ReduceFootSize)
					{
						shrink = true;
						num = Mathf.Max(num, this.appliedAccessories[i].FootSizeReduction);
					}
					if (this.appliedAccessories[i].ShouldBlockHair)
					{
						flag = true;
					}
				}
				else if (this.appliedAccessories[i] != null)
				{
					UnityEngine.Object.Destroy(this.appliedAccessories[i].gameObject);
					this.appliedAccessories[i] = null;
				}
			}
			this.SetFeetShrunk(shrink, num);
			this.SetWearingHairBlockingAccessory(flag);
			for (int j = 0; j < this.appliedAccessories.Length; j++)
			{
				if (this.appliedAccessories[j] != null)
				{
					this.appliedAccessories[j].ApplyColor(settings.AccessorySettings[j].color);
				}
			}
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x0010E954 File Offset: 0x0010CB54
		private void DestroyAccessories()
		{
			for (int i = 0; i < this.appliedAccessories.Length; i++)
			{
				if (this.appliedAccessories[i] != null)
				{
					UnityEngine.Object.Destroy(this.appliedAccessories[i].gameObject);
				}
			}
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0010E998 File Offset: 0x0010CB98
		public virtual void SetRagdollPhysicsEnabled(bool ragdollEnabled, bool playStandUpAnim = true)
		{
			bool ragdolled = this.Ragdolled;
			this.Ragdolled = ragdollEnabled;
			if (this.onRagdollChange != null)
			{
				this.onRagdollChange.Invoke(ragdolled, ragdollEnabled, playStandUpAnim);
			}
			foreach (Rigidbody rigidbody in this.RagdollRBs)
			{
				if (!(rigidbody == null))
				{
					rigidbody.isKinematic = !ragdollEnabled;
					if (!rigidbody.isKinematic)
					{
						rigidbody.velocity = Vector3.zero;
						rigidbody.angularVelocity = Vector3.zero;
					}
				}
			}
			foreach (Collider collider in this.RagdollColliders)
			{
				if (!(collider == null))
				{
					collider.enabled = ragdollEnabled;
				}
			}
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x0010EA44 File Offset: 0x0010CC44
		public virtual AvatarEquippable SetEquippable(string assetPath)
		{
			if (this.CurrentEquippable != null)
			{
				this.CurrentEquippable.Unequip();
			}
			if (!(assetPath != string.Empty))
			{
				return null;
			}
			GameObject gameObject = Resources.Load(assetPath) as GameObject;
			if (gameObject == null)
			{
				Console.LogError("Couldn't find equippable at path " + assetPath, null);
				return null;
			}
			this.CurrentEquippable = UnityEngine.Object.Instantiate<GameObject>(gameObject, null).GetComponent<AvatarEquippable>();
			this.CurrentEquippable.Equip(this);
			return this.CurrentEquippable;
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x0010EAC5 File Offset: 0x0010CCC5
		public virtual void ReceiveEquippableMessage(string message, object data)
		{
			if (this.CurrentEquippable != null)
			{
				this.CurrentEquippable.ReceiveMessage(message, data);
				return;
			}
			Console.LogWarning("Received equippable message but no equippable is equipped!", null);
		}

		// Token: 0x04002E23 RID: 11811
		public const int MAX_ACCESSORIES = 9;

		// Token: 0x04002E24 RID: 11812
		public const bool USE_COMBINED_LAYERS = true;

		// Token: 0x04002E25 RID: 11813
		public const float DEFAULT_SMOOTHNESS = 0.25f;

		// Token: 0x04002E26 RID: 11814
		private static float maleShoulderScale = 0.93f;

		// Token: 0x04002E27 RID: 11815
		private static float femaleShoulderScale = 0.875f;

		// Token: 0x04002E28 RID: 11816
		[Header("References")]
		public AvatarAnimation Anim;

		// Token: 0x04002E29 RID: 11817
		public AvatarLookController LookController;

		// Token: 0x04002E2A RID: 11818
		public SkinnedMeshRenderer[] BodyMeshes;

		// Token: 0x04002E2B RID: 11819
		public SkinnedMeshRenderer[] ShapeKeyMeshes;

		// Token: 0x04002E2C RID: 11820
		public SkinnedMeshRenderer FaceMesh;

		// Token: 0x04002E2D RID: 11821
		public EyeController Eyes;

		// Token: 0x04002E2E RID: 11822
		public EyebrowController EyeBrows;

		// Token: 0x04002E2F RID: 11823
		public Transform BodyContainer;

		// Token: 0x04002E30 RID: 11824
		public Transform Armature;

		// Token: 0x04002E31 RID: 11825
		public Transform LeftShoulder;

		// Token: 0x04002E32 RID: 11826
		public Transform RightShoulder;

		// Token: 0x04002E33 RID: 11827
		public Transform HeadBone;

		// Token: 0x04002E34 RID: 11828
		public Transform HipBone;

		// Token: 0x04002E35 RID: 11829
		public Rigidbody[] RagdollRBs;

		// Token: 0x04002E36 RID: 11830
		public Collider[] RagdollColliders;

		// Token: 0x04002E37 RID: 11831
		public Rigidbody MiddleSpineRB;

		// Token: 0x04002E38 RID: 11832
		public AvatarEmotionManager EmotionManager;

		// Token: 0x04002E39 RID: 11833
		public AvatarEffects Effects;

		// Token: 0x04002E3A RID: 11834
		public Transform MiddleSpine;

		// Token: 0x04002E3B RID: 11835
		public Transform LowerSpine;

		// Token: 0x04002E3C RID: 11836
		public Transform LowestSpine;

		// Token: 0x04002E3D RID: 11837
		public AvatarImpostor Impostor;

		// Token: 0x04002E3E RID: 11838
		[Header("Settings")]
		public AvatarSettings InitialAvatarSettings;

		// Token: 0x04002E3F RID: 11839
		public Material DefaultAvatarMaterial;

		// Token: 0x04002E40 RID: 11840
		public bool UseImpostor;

		// Token: 0x04002E41 RID: 11841
		public UnityEvent<bool, bool, bool> onRagdollChange;

		// Token: 0x04002E44 RID: 11844
		[Header("Data - readonly")]
		[SerializeField]
		protected float appliedGender;

		// Token: 0x04002E45 RID: 11845
		[SerializeField]
		protected float appliedWeight;

		// Token: 0x04002E46 RID: 11846
		[SerializeField]
		protected Hair appliedHair;

		// Token: 0x04002E47 RID: 11847
		[SerializeField]
		protected Color appliedHairColor;

		// Token: 0x04002E48 RID: 11848
		[SerializeField]
		protected Accessory[] appliedAccessories = new Accessory[9];

		// Token: 0x04002E49 RID: 11849
		[SerializeField]
		protected bool wearingHairBlockingAccessory;

		// Token: 0x04002E4A RID: 11850
		private float additionalWeight;

		// Token: 0x04002E4B RID: 11851
		private float additionalGender;

		// Token: 0x04002E4D RID: 11853
		[Header("Runtime loading")]
		public AvatarSettings SettingsToLoad;

		// Token: 0x04002E4E RID: 11854
		public UnityEvent onSettingsLoaded;

		// Token: 0x04002E4F RID: 11855
		private Vector3 originalHipPos = Vector3.zero;

		// Token: 0x04002E50 RID: 11856
		private bool usingCombinedLayer;

		// Token: 0x04002E51 RID: 11857
		private bool blockEyeFaceLayers;
	}
}
