using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x0200098C RID: 2444
	[CreateAssetMenu(fileName = "BasicAvatarSettings", menuName = "ScriptableObjects/BasicAvatarSettings", order = 1)]
	[Serializable]
	public class BasicAvatarSettings : ScriptableObject
	{
		// Token: 0x06004233 RID: 16947 RVA: 0x001159FD File Offset: 0x00113BFD
		public T SetValue<T>(string fieldName, T value)
		{
			base.GetType().GetField(fieldName).SetValue(this, value);
			return value;
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x00115A18 File Offset: 0x00113C18
		public T GetValue<T>(string fieldName)
		{
			FieldInfo field = base.GetType().GetField(fieldName);
			if (field == null)
			{
				return default(T);
			}
			return (T)((object)field.GetValue(this));
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x00115A54 File Offset: 0x00113C54
		public AvatarSettings GetAvatarSettings()
		{
			AvatarSettings avatarSettings = ScriptableObject.CreateInstance<AvatarSettings>();
			avatarSettings.Gender = (float)this.Gender * 0.7f;
			avatarSettings.Weight = this.Weight;
			avatarSettings.Height = 1f;
			avatarSettings.SkinColor = this.SkinColor;
			avatarSettings.HairPath = this.HairStyle;
			avatarSettings.HairColor = this.HairColor;
			avatarSettings.FaceLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = this.Mouth,
				layerTint = Color.black
			});
			avatarSettings.FaceLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = this.FacialHair,
				layerTint = Color.white
			});
			avatarSettings.FaceLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = this.FacialDetails,
				layerTint = new Color(0f, 0f, 0f, this.FacialDetailsIntensity)
			});
			avatarSettings.FaceLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = "Avatar/Layers/Face/EyeShadow",
				layerTint = new Color(0f, 0f, 0f, 0.7f)
			});
			avatarSettings.EyeBallTint = this.EyeballColor;
			avatarSettings.LeftEyeLidColor = this.SkinColor;
			avatarSettings.RightEyeLidColor = this.SkinColor;
			avatarSettings.EyeballMaterialIdentifier = "Default";
			avatarSettings.PupilDilation = this.PupilDilation;
			Eye.EyeLidConfiguration eyeLidConfiguration = new Eye.EyeLidConfiguration
			{
				topLidOpen = this.UpperEyeLidRestingPosition,
				bottomLidOpen = this.LowerEyeLidRestingPosition
			};
			avatarSettings.LeftEyeRestingState = eyeLidConfiguration;
			avatarSettings.RightEyeRestingState = eyeLidConfiguration;
			avatarSettings.EyebrowScale = this.EyebrowScale;
			avatarSettings.EyebrowThickness = this.EyebrowThickness;
			avatarSettings.EyebrowRestingHeight = this.EyebrowRestingHeight;
			avatarSettings.EyebrowRestingAngle = this.EyebrowRestingAngle;
			avatarSettings.BodyLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = "Avatar/Layers/Top/Nipples",
				layerTint = new Color32(212, 181, 142, byte.MaxValue)
			});
			string layerPath = ((float)this.Gender <= 0.5f) ? "Avatar/Layers/Bottom/MaleUnderwear" : "Avatar/Layers/Bottom/FemaleUnderwear";
			avatarSettings.BodyLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = layerPath,
				layerTint = Color.white
			});
			if (!string.IsNullOrEmpty(this.Top))
			{
				avatarSettings.BodyLayerSettings.Add(new AvatarSettings.LayerSetting
				{
					layerPath = this.Top,
					layerTint = this.TopColor
				});
			}
			if (!string.IsNullOrEmpty(this.Bottom))
			{
				avatarSettings.BodyLayerSettings.Add(new AvatarSettings.LayerSetting
				{
					layerPath = this.Bottom,
					layerTint = this.BottomColor
				});
			}
			if (this.Tattoos != null)
			{
				for (int i = 0; i < this.Tattoos.Count; i++)
				{
					if (this.Tattoos[i].Contains("/Face/"))
					{
						avatarSettings.FaceLayerSettings.Add(new AvatarSettings.LayerSetting
						{
							layerPath = this.Tattoos[i],
							layerTint = Color.white
						});
					}
					else
					{
						avatarSettings.BodyLayerSettings.Add(new AvatarSettings.LayerSetting
						{
							layerPath = this.Tattoos[i],
							layerTint = Color.white
						});
					}
				}
			}
			if (!string.IsNullOrEmpty(this.Shoes))
			{
				avatarSettings.AccessorySettings.Add(new AvatarSettings.AccessorySetting
				{
					path = this.Shoes,
					color = this.ShoesColor
				});
			}
			if (!string.IsNullOrEmpty(this.Headwear))
			{
				avatarSettings.AccessorySettings.Add(new AvatarSettings.AccessorySetting
				{
					path = this.Headwear,
					color = this.HeadwearColor
				});
			}
			if (!string.IsNullOrEmpty(this.Eyewear))
			{
				avatarSettings.AccessorySettings.Add(new AvatarSettings.AccessorySetting
				{
					path = this.Eyewear,
					color = this.EyewearColor
				});
			}
			return avatarSettings;
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x0011038B File Offset: 0x0010E58B
		public virtual string GetJson(bool prettyPrint = true)
		{
			return JsonUtility.ToJson(this, prettyPrint);
		}

		// Token: 0x04003029 RID: 12329
		public const float GENDER_MULTIPLIER = 0.7f;

		// Token: 0x0400302A RID: 12330
		public const string MaleUnderwearPath = "Avatar/Layers/Bottom/MaleUnderwear";

		// Token: 0x0400302B RID: 12331
		public const string FemaleUnderwearPath = "Avatar/Layers/Bottom/FemaleUnderwear";

		// Token: 0x0400302C RID: 12332
		public int Gender;

		// Token: 0x0400302D RID: 12333
		public float Weight;

		// Token: 0x0400302E RID: 12334
		public Color SkinColor;

		// Token: 0x0400302F RID: 12335
		public string HairStyle;

		// Token: 0x04003030 RID: 12336
		public Color HairColor;

		// Token: 0x04003031 RID: 12337
		public string Mouth;

		// Token: 0x04003032 RID: 12338
		public string FacialHair;

		// Token: 0x04003033 RID: 12339
		public string FacialDetails;

		// Token: 0x04003034 RID: 12340
		public float FacialDetailsIntensity;

		// Token: 0x04003035 RID: 12341
		public Color EyeballColor;

		// Token: 0x04003036 RID: 12342
		public float UpperEyeLidRestingPosition;

		// Token: 0x04003037 RID: 12343
		public float LowerEyeLidRestingPosition;

		// Token: 0x04003038 RID: 12344
		public float PupilDilation = 1f;

		// Token: 0x04003039 RID: 12345
		public float EyebrowScale;

		// Token: 0x0400303A RID: 12346
		public float EyebrowThickness;

		// Token: 0x0400303B RID: 12347
		public float EyebrowRestingHeight;

		// Token: 0x0400303C RID: 12348
		public float EyebrowRestingAngle;

		// Token: 0x0400303D RID: 12349
		public string Top;

		// Token: 0x0400303E RID: 12350
		public Color TopColor;

		// Token: 0x0400303F RID: 12351
		public string Bottom;

		// Token: 0x04003040 RID: 12352
		public Color BottomColor;

		// Token: 0x04003041 RID: 12353
		public string Shoes;

		// Token: 0x04003042 RID: 12354
		public Color ShoesColor;

		// Token: 0x04003043 RID: 12355
		public string Headwear;

		// Token: 0x04003044 RID: 12356
		public Color HeadwearColor;

		// Token: 0x04003045 RID: 12357
		public string Eyewear;

		// Token: 0x04003046 RID: 12358
		public Color EyewearColor;

		// Token: 0x04003047 RID: 12359
		public List<string> Tattoos = new List<string>();
	}
}
