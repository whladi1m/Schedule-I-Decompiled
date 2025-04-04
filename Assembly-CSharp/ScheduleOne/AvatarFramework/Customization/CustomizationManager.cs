using System;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x02000991 RID: 2449
	public class CustomizationManager : Singleton<CustomizationManager>
	{
		// Token: 0x06004252 RID: 16978 RVA: 0x001164AA File Offset: 0x001146AA
		protected override void Start()
		{
			base.Start();
			this.LoadSettings(UnityEngine.Object.Instantiate<AvatarSettings>(this.DefaultSettings));
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x000045B1 File Offset: 0x000027B1
		public void CreateSettings(string name)
		{
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x001164C3 File Offset: 0x001146C3
		public void CreateSettings()
		{
			if (this.SaveInputField.text == "")
			{
				Console.LogWarning("No name entered for settings file.", null);
				return;
			}
			this.CreateSettings(this.SaveInputField.text);
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x001164FC File Offset: 0x001146FC
		public void LoadSettings(AvatarSettings loadedSettings)
		{
			if (loadedSettings == null)
			{
				Console.LogWarning("Settings are null!", null);
				return;
			}
			this.ActiveSettings = loadedSettings;
			Debug.Log("Settings loaded: " + this.ActiveSettings.name);
			this.TemplateAvatar.LoadAvatarSettings(this.ActiveSettings);
			if (this.OnAvatarSettingsChanged != null)
			{
				this.OnAvatarSettingsChanged(this.ActiveSettings);
			}
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x0011656C File Offset: 0x0011476C
		public void LoadSettings(string settingsName, bool editOriginal = false)
		{
			this.isEditingOriginal = editOriginal;
			AvatarSettings loadedSettings;
			if (editOriginal)
			{
				loadedSettings = Resources.Load<AvatarSettings>("CharacterSettings/" + settingsName);
				this.SaveInputField.SetTextWithoutNotify(settingsName);
			}
			else
			{
				loadedSettings = UnityEngine.Object.Instantiate<AvatarSettings>(Resources.Load<AvatarSettings>("CharacterSettings/" + settingsName));
			}
			this.LoadSettings(loadedSettings);
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x001165C4 File Offset: 0x001147C4
		private void ApplyDefaultSettings(AvatarSettings settings)
		{
			settings.SkinColor = new Color32(150, 120, 95, byte.MaxValue);
			settings.Height = 0.98f;
			settings.Gender = 0f;
			settings.Weight = 0.4f;
			settings.EyebrowScale = 1f;
			settings.EyebrowThickness = 1f;
			settings.EyebrowRestingHeight = 0f;
			settings.EyebrowRestingAngle = 0f;
			settings.LeftEyeLidColor = new Color32(150, 120, 95, byte.MaxValue);
			settings.RightEyeLidColor = new Color32(150, 120, 95, byte.MaxValue);
			settings.LeftEyeRestingState = new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.5f,
				topLidOpen = 0.5f
			};
			settings.RightEyeRestingState = new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.5f,
				topLidOpen = 0.5f
			};
			settings.EyeballMaterialIdentifier = "Default";
			settings.EyeBallTint = Color.white;
			settings.PupilDilation = 1f;
			settings.HairPath = string.Empty;
			settings.HairColor = Color.black;
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x00116700 File Offset: 0x00114900
		public void LoadSettings()
		{
			this.isEditingOriginal = true;
			Debug.Log("Loading!: " + this.LoadInputField.text);
			this.LoadSettings(this.LoadInputField.text, this.LoadInputField.text != "Default");
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x00116754 File Offset: 0x00114954
		public void GenderChanged(float genderScale)
		{
			this.ActiveSettings.Gender = genderScale;
			this.TemplateAvatar.ApplyBodySettings(this.ActiveSettings);
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x00116773 File Offset: 0x00114973
		public void WeightChanged(float weightScale)
		{
			this.ActiveSettings.Weight = weightScale;
			this.TemplateAvatar.ApplyBodySettings(this.ActiveSettings);
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x00116792 File Offset: 0x00114992
		public void HeightChanged(float height)
		{
			this.ActiveSettings.Height = height;
			this.TemplateAvatar.ApplyBodySettings(this.ActiveSettings);
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x001167B4 File Offset: 0x001149B4
		public void SkinColorChanged(Color col)
		{
			this.ActiveSettings.SkinColor = col;
			this.TemplateAvatar.ApplyBodySettings(this.ActiveSettings);
			if (Input.GetKey(KeyCode.LeftControl))
			{
				this.ActiveSettings.LeftEyeLidColor = col;
				this.ActiveSettings.RightEyeLidColor = col;
			}
			this.TemplateAvatar.ApplyEyeLidColorSettings(this.ActiveSettings);
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x00116813 File Offset: 0x00114A13
		public void HairChanged(Accessory newHair)
		{
			this.ActiveSettings.HairPath = ((newHair != null) ? newHair.AssetPath : string.Empty);
			this.TemplateAvatar.ApplyHairSettings(this.ActiveSettings);
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x00116847 File Offset: 0x00114A47
		public void HairColorChanged(Color newCol)
		{
			this.ActiveSettings.HairColor = newCol;
			this.TemplateAvatar.ApplyHairColorSettings(this.ActiveSettings);
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x00116866 File Offset: 0x00114A66
		public void EyeBallTintChanged(Color col)
		{
			this.ActiveSettings.EyeBallTint = col;
			this.TemplateAvatar.ApplyEyeBallSettings(this.ActiveSettings);
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x00116885 File Offset: 0x00114A85
		public void UpperEyeLidRestingPositionChanged(float newVal)
		{
			this.ActiveSettings.LeftEyeRestingState.topLidOpen = newVal;
			this.ActiveSettings.RightEyeRestingState.topLidOpen = newVal;
			this.TemplateAvatar.ApplyEyeLidSettings(this.ActiveSettings);
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x001168BA File Offset: 0x00114ABA
		public void LowerEyeLidRestingPositionChanged(float newVal)
		{
			this.ActiveSettings.LeftEyeRestingState.bottomLidOpen = newVal;
			this.ActiveSettings.RightEyeRestingState.bottomLidOpen = newVal;
			this.TemplateAvatar.ApplyEyeLidSettings(this.ActiveSettings);
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x001168EF File Offset: 0x00114AEF
		public void EyebrowScaleChanged(float newVal)
		{
			this.ActiveSettings.EyebrowScale = newVal;
			this.TemplateAvatar.ApplyEyebrowSettings(this.ActiveSettings);
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x0011690E File Offset: 0x00114B0E
		public void EyebrowThicknessChanged(float newVal)
		{
			this.ActiveSettings.EyebrowThickness = newVal;
			this.TemplateAvatar.ApplyEyebrowSettings(this.ActiveSettings);
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x0011692D File Offset: 0x00114B2D
		public void EyebrowRestingHeightChanged(float newVal)
		{
			this.ActiveSettings.EyebrowRestingHeight = newVal;
			this.TemplateAvatar.ApplyEyebrowSettings(this.ActiveSettings);
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x0011694C File Offset: 0x00114B4C
		public void EyebrowRestingAngleChanged(float newVal)
		{
			this.ActiveSettings.EyebrowRestingAngle = newVal;
			this.TemplateAvatar.ApplyEyebrowSettings(this.ActiveSettings);
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x0011696B File Offset: 0x00114B6B
		public void PupilDilationChanged(float dilation)
		{
			this.ActiveSettings.PupilDilation = dilation;
			this.TemplateAvatar.ApplyEyeBallSettings(this.ActiveSettings);
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x0011698C File Offset: 0x00114B8C
		public void FaceLayerChanged(FaceLayer layer, int index)
		{
			string layerPath = (layer != null) ? layer.AssetPath : string.Empty;
			Color layerTint = this.ActiveSettings.FaceLayerSettings[index].layerTint;
			this.ActiveSettings.FaceLayerSettings[index] = new AvatarSettings.LayerSetting
			{
				layerPath = layerPath,
				layerTint = layerTint
			};
			this.TemplateAvatar.ApplyFaceLayerSettings(this.ActiveSettings);
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x00116A04 File Offset: 0x00114C04
		public void FaceLayerColorChanged(Color col, int index)
		{
			string layerPath = this.ActiveSettings.FaceLayerSettings[index].layerPath;
			this.ActiveSettings.FaceLayerSettings[index] = new AvatarSettings.LayerSetting
			{
				layerPath = layerPath,
				layerTint = col
			};
			this.TemplateAvatar.ApplyFaceLayerSettings(this.ActiveSettings);
		}

		// Token: 0x06004269 RID: 17001 RVA: 0x00116A64 File Offset: 0x00114C64
		public void BodyLayerChanged(AvatarLayer layer, int index)
		{
			string layerPath = (layer != null) ? layer.AssetPath : string.Empty;
			Color layerTint = this.ActiveSettings.BodyLayerSettings[index].layerTint;
			this.ActiveSettings.BodyLayerSettings[index] = new AvatarSettings.LayerSetting
			{
				layerPath = layerPath,
				layerTint = layerTint
			};
			this.TemplateAvatar.ApplyBodyLayerSettings(this.ActiveSettings, -1);
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x00116ADC File Offset: 0x00114CDC
		public void BodyLayerColorChanged(Color col, int index)
		{
			string layerPath = this.ActiveSettings.BodyLayerSettings[index].layerPath;
			this.ActiveSettings.BodyLayerSettings[index] = new AvatarSettings.LayerSetting
			{
				layerPath = layerPath,
				layerTint = col
			};
			this.TemplateAvatar.ApplyBodyLayerSettings(this.ActiveSettings, -1);
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x00116B3C File Offset: 0x00114D3C
		public void AccessoryChanged(Accessory acc, int index)
		{
			Debug.Log("Accessory changed: " + ((acc != null) ? acc.AssetPath : null));
			string path = (acc != null) ? acc.AssetPath : string.Empty;
			while (this.ActiveSettings.AccessorySettings.Count <= index)
			{
				this.ActiveSettings.AccessorySettings.Add(new AvatarSettings.AccessorySetting());
			}
			Color color = this.ActiveSettings.AccessorySettings[index].color;
			this.ActiveSettings.AccessorySettings[index] = new AvatarSettings.AccessorySetting
			{
				path = path,
				color = color
			};
			this.TemplateAvatar.ApplyAccessorySettings(this.ActiveSettings);
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x00116BF4 File Offset: 0x00114DF4
		public void AccessoryColorChanged(Color col, int index)
		{
			string path = this.ActiveSettings.AccessorySettings[index].path;
			this.ActiveSettings.AccessorySettings[index] = new AvatarSettings.AccessorySetting
			{
				path = path,
				color = col
			};
			this.TemplateAvatar.ApplyAccessorySettings(this.ActiveSettings);
		}

		// Token: 0x04003064 RID: 12388
		[SerializeField]
		private AvatarSettings ActiveSettings;

		// Token: 0x04003065 RID: 12389
		public Avatar TemplateAvatar;

		// Token: 0x04003066 RID: 12390
		public TMP_InputField SaveInputField;

		// Token: 0x04003067 RID: 12391
		public TMP_InputField LoadInputField;

		// Token: 0x04003068 RID: 12392
		public CustomizationManager.AvatarSettingsChanged OnAvatarSettingsChanged;

		// Token: 0x04003069 RID: 12393
		public AvatarSettings DefaultSettings;

		// Token: 0x0400306A RID: 12394
		private bool isEditingOriginal;

		// Token: 0x02000992 RID: 2450
		// (Invoke) Token: 0x0600426F RID: 17007
		public delegate void AvatarSettingsChanged(AvatarSettings settings);
	}
}
