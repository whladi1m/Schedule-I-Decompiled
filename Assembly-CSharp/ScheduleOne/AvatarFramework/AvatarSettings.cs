using System;
using System.Collections.Generic;
using System.Reflection;
using FishNet.Serializing.Helping;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x02000944 RID: 2372
	[CreateAssetMenu(fileName = "Avatar Settings", menuName = "ScriptableObjects/Avatar Settings", order = 1)]
	[Serializable]
	public class AvatarSettings : ScriptableObject
	{
		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06004096 RID: 16534 RVA: 0x0010FD13 File Offset: 0x0010DF13
		public float UpperEyelidRestingPosition
		{
			get
			{
				return this.LeftEyeRestingState.topLidOpen;
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06004097 RID: 16535 RVA: 0x0010FD20 File Offset: 0x0010DF20
		public float LowerEyelidRestingPosition
		{
			get
			{
				return this.LeftEyeRestingState.bottomLidOpen;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06004098 RID: 16536 RVA: 0x0010FD2D File Offset: 0x0010DF2D
		public string FaceLayer1Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 0)
				{
					return null;
				}
				return this.FaceLayerSettings[0].layerPath;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06004099 RID: 16537 RVA: 0x0010FD50 File Offset: 0x0010DF50
		public Color FaceLayer1Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 0)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[0].layerTint;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x0600409A RID: 16538 RVA: 0x0010FD77 File Offset: 0x0010DF77
		public string FaceLayer2Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 1)
				{
					return null;
				}
				return this.FaceLayerSettings[1].layerPath;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x0600409B RID: 16539 RVA: 0x0010FD9A File Offset: 0x0010DF9A
		public Color FaceLayer2Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 1)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[1].layerTint;
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x0600409C RID: 16540 RVA: 0x0010FDC1 File Offset: 0x0010DFC1
		public string FaceLayer3Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 2)
				{
					return null;
				}
				return this.FaceLayerSettings[2].layerPath;
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x0600409D RID: 16541 RVA: 0x0010FDE4 File Offset: 0x0010DFE4
		public Color FaceLayer3Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 2)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[2].layerTint;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x0600409E RID: 16542 RVA: 0x0010FE0B File Offset: 0x0010E00B
		public string FaceLayer4Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 3)
				{
					return null;
				}
				return this.FaceLayerSettings[3].layerPath;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x0600409F RID: 16543 RVA: 0x0010FE2E File Offset: 0x0010E02E
		public Color FaceLayer4Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 3)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[3].layerTint;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x060040A0 RID: 16544 RVA: 0x0010FE55 File Offset: 0x0010E055
		public string FaceLayer5Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 4)
				{
					return null;
				}
				return this.FaceLayerSettings[4].layerPath;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x060040A1 RID: 16545 RVA: 0x0010FE78 File Offset: 0x0010E078
		public Color FaceLayer5Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 4)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[4].layerTint;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x060040A2 RID: 16546 RVA: 0x0010FE9F File Offset: 0x0010E09F
		public string FaceLayer6Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 5)
				{
					return null;
				}
				return this.FaceLayerSettings[5].layerPath;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x060040A3 RID: 16547 RVA: 0x0010FEC2 File Offset: 0x0010E0C2
		public Color FaceLayer6Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 5)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[5].layerTint;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x060040A4 RID: 16548 RVA: 0x0010FEE9 File Offset: 0x0010E0E9
		public string BodyLayer1Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 0)
				{
					return null;
				}
				return this.BodyLayerSettings[0].layerPath;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x060040A5 RID: 16549 RVA: 0x0010FF0C File Offset: 0x0010E10C
		public Color BodyLayer1Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 0)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[0].layerTint;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x060040A6 RID: 16550 RVA: 0x0010FF33 File Offset: 0x0010E133
		public string BodyLayer2Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 1)
				{
					return null;
				}
				return this.BodyLayerSettings[1].layerPath;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x060040A7 RID: 16551 RVA: 0x0010FF56 File Offset: 0x0010E156
		public Color BodyLayer2Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 1)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[1].layerTint;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x0010FF7D File Offset: 0x0010E17D
		public string BodyLayer3Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 2)
				{
					return null;
				}
				return this.BodyLayerSettings[2].layerPath;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x060040A9 RID: 16553 RVA: 0x0010FFA0 File Offset: 0x0010E1A0
		public Color BodyLayer3Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 2)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[2].layerTint;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x060040AA RID: 16554 RVA: 0x0010FFC7 File Offset: 0x0010E1C7
		public string BodyLayer4Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 3)
				{
					return null;
				}
				return this.BodyLayerSettings[3].layerPath;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x060040AB RID: 16555 RVA: 0x0010FFEA File Offset: 0x0010E1EA
		public Color BodyLayer4Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 3)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[3].layerTint;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x060040AC RID: 16556 RVA: 0x00110011 File Offset: 0x0010E211
		public string BodyLayer5Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 4)
				{
					return null;
				}
				return this.BodyLayerSettings[4].layerPath;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x060040AD RID: 16557 RVA: 0x00110034 File Offset: 0x0010E234
		public Color BodyLayer5Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 4)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[4].layerTint;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x060040AE RID: 16558 RVA: 0x0011005B File Offset: 0x0010E25B
		public string BodyLayer6Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 5)
				{
					return null;
				}
				return this.BodyLayerSettings[5].layerPath;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x060040AF RID: 16559 RVA: 0x0011007E File Offset: 0x0010E27E
		public Color BodyLayer6Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 5)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[5].layerTint;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x060040B0 RID: 16560 RVA: 0x001100A5 File Offset: 0x0010E2A5
		public string Accessory1Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 0)
				{
					return null;
				}
				return this.AccessorySettings[0].path;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x060040B1 RID: 16561 RVA: 0x001100C8 File Offset: 0x0010E2C8
		public Color Accessory1Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 0)
				{
					return Color.white;
				}
				return this.AccessorySettings[0].color;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x060040B2 RID: 16562 RVA: 0x001100EF File Offset: 0x0010E2EF
		public string Accessory2Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 1)
				{
					return null;
				}
				return this.AccessorySettings[1].path;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x060040B3 RID: 16563 RVA: 0x00110112 File Offset: 0x0010E312
		public Color Accessory2Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 1)
				{
					return Color.white;
				}
				return this.AccessorySettings[1].color;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x060040B4 RID: 16564 RVA: 0x00110139 File Offset: 0x0010E339
		public string Accessory3Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 2)
				{
					return null;
				}
				return this.AccessorySettings[2].path;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x060040B5 RID: 16565 RVA: 0x0011015C File Offset: 0x0010E35C
		public Color Accessory3Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 2)
				{
					return Color.white;
				}
				return this.AccessorySettings[2].color;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x060040B6 RID: 16566 RVA: 0x00110183 File Offset: 0x0010E383
		public string Accessory4Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 3)
				{
					return null;
				}
				return this.AccessorySettings[3].path;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x060040B7 RID: 16567 RVA: 0x001101A6 File Offset: 0x0010E3A6
		public Color Accessory4Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 3)
				{
					return Color.white;
				}
				return this.AccessorySettings[3].color;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x060040B8 RID: 16568 RVA: 0x001101CD File Offset: 0x0010E3CD
		public string Accessory5Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 4)
				{
					return null;
				}
				return this.AccessorySettings[4].path;
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x060040B9 RID: 16569 RVA: 0x001101F0 File Offset: 0x0010E3F0
		public Color Accessory5Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 4)
				{
					return Color.white;
				}
				return this.AccessorySettings[4].color;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x060040BA RID: 16570 RVA: 0x00110217 File Offset: 0x0010E417
		public string Accessory6Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 5)
				{
					return null;
				}
				return this.AccessorySettings[5].path;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x060040BB RID: 16571 RVA: 0x0011023A File Offset: 0x0010E43A
		public Color Accessory6Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 5)
				{
					return Color.white;
				}
				return this.AccessorySettings[5].color;
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x060040BC RID: 16572 RVA: 0x00110261 File Offset: 0x0010E461
		public string Accessory7Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 6)
				{
					return null;
				}
				return this.AccessorySettings[6].path;
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x060040BD RID: 16573 RVA: 0x00110284 File Offset: 0x0010E484
		public Color Accessory7Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 6)
				{
					return Color.white;
				}
				return this.AccessorySettings[6].color;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x060040BE RID: 16574 RVA: 0x001102AB File Offset: 0x0010E4AB
		public string Accessory8Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 7)
				{
					return null;
				}
				return this.AccessorySettings[7].path;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x060040BF RID: 16575 RVA: 0x001102CE File Offset: 0x0010E4CE
		public Color Accessory8Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 7)
				{
					return Color.white;
				}
				return this.AccessorySettings[7].color;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x060040C0 RID: 16576 RVA: 0x001102F5 File Offset: 0x0010E4F5
		public string Accessory9Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 8)
				{
					return null;
				}
				return this.AccessorySettings[8].path;
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x060040C1 RID: 16577 RVA: 0x00110318 File Offset: 0x0010E518
		public Color Accessory9Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 8)
				{
					return Color.white;
				}
				return this.AccessorySettings[8].color;
			}
		}

		// Token: 0x17000931 RID: 2353
		public object this[string propertyName]
		{
			get
			{
				FieldInfo field = base.GetType().GetField(propertyName);
				PropertyInfo property = base.GetType().GetProperty(propertyName);
				if (field != null)
				{
					return field.GetValue(this);
				}
				if (property != null)
				{
					return property.GetValue(this, null);
				}
				return null;
			}
		}

		// Token: 0x060040C3 RID: 16579 RVA: 0x0011038B File Offset: 0x0010E58B
		public virtual string GetJson(bool prettyPrint = true)
		{
			return JsonUtility.ToJson(this, prettyPrint);
		}

		// Token: 0x04002E82 RID: 11906
		public Color SkinColor;

		// Token: 0x04002E83 RID: 11907
		public float Height;

		// Token: 0x04002E84 RID: 11908
		public float Gender;

		// Token: 0x04002E85 RID: 11909
		public float Weight;

		// Token: 0x04002E86 RID: 11910
		public string HairPath;

		// Token: 0x04002E87 RID: 11911
		public Color HairColor;

		// Token: 0x04002E88 RID: 11912
		public float EyebrowScale;

		// Token: 0x04002E89 RID: 11913
		public float EyebrowThickness;

		// Token: 0x04002E8A RID: 11914
		public float EyebrowRestingHeight;

		// Token: 0x04002E8B RID: 11915
		public float EyebrowRestingAngle;

		// Token: 0x04002E8C RID: 11916
		public Color LeftEyeLidColor;

		// Token: 0x04002E8D RID: 11917
		public Color RightEyeLidColor;

		// Token: 0x04002E8E RID: 11918
		public Eye.EyeLidConfiguration LeftEyeRestingState;

		// Token: 0x04002E8F RID: 11919
		public Eye.EyeLidConfiguration RightEyeRestingState;

		// Token: 0x04002E90 RID: 11920
		public string EyeballMaterialIdentifier;

		// Token: 0x04002E91 RID: 11921
		public Color EyeBallTint;

		// Token: 0x04002E92 RID: 11922
		public float PupilDilation;

		// Token: 0x04002E93 RID: 11923
		public List<AvatarSettings.LayerSetting> FaceLayerSettings = new List<AvatarSettings.LayerSetting>();

		// Token: 0x04002E94 RID: 11924
		public List<AvatarSettings.LayerSetting> BodyLayerSettings = new List<AvatarSettings.LayerSetting>();

		// Token: 0x04002E95 RID: 11925
		public List<AvatarSettings.AccessorySetting> AccessorySettings = new List<AvatarSettings.AccessorySetting>();

		// Token: 0x04002E96 RID: 11926
		public bool UseCombinedLayer;

		// Token: 0x04002E97 RID: 11927
		public string CombinedLayerPath;

		// Token: 0x04002E98 RID: 11928
		[CodegenExclude]
		public Texture2D ImpostorTexture;

		// Token: 0x02000945 RID: 2373
		[Serializable]
		public struct LayerSetting
		{
			// Token: 0x04002E99 RID: 11929
			public string layerPath;

			// Token: 0x04002E9A RID: 11930
			public Color layerTint;
		}

		// Token: 0x02000946 RID: 2374
		[Serializable]
		public class AccessorySetting
		{
			// Token: 0x04002E9B RID: 11931
			public string path;

			// Token: 0x04002E9C RID: 11932
			public Color color;
		}
	}
}
