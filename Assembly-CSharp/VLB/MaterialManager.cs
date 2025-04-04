using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace VLB
{
	// Token: 0x02000118 RID: 280
	public static class MaterialManager
	{
		// Token: 0x060004E1 RID: 1249 RVA: 0x000183C9 File Offset: 0x000165C9
		public static Material NewMaterialPersistent(Shader shader, bool gpuInstanced)
		{
			if (!shader)
			{
				Debug.LogError("Invalid VLB Shader. Please try to reset the VLB Config asset or reinstall the plugin.");
				return null;
			}
			Material material = new Material(shader);
			BatchingHelper.SetMaterialProperties(material, gpuInstanced);
			return material;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x000183EC File Offset: 0x000165EC
		public static Material GetInstancedMaterial(uint groupID, ref MaterialManager.StaticPropertiesSD staticProps)
		{
			MaterialManager.IStaticProperties staticProperties = staticProps;
			return MaterialManager.GetInstancedMaterial(MaterialManager.ms_MaterialsGroupSD, groupID, ref staticProperties);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00018414 File Offset: 0x00016614
		public static Material GetInstancedMaterial(uint groupID, ref MaterialManager.StaticPropertiesHD staticProps)
		{
			MaterialManager.IStaticProperties staticProperties = staticProps;
			return MaterialManager.GetInstancedMaterial(MaterialManager.ms_MaterialsGroupHD, groupID, ref staticProperties);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001843C File Offset: 0x0001663C
		private static Material GetInstancedMaterial(Hashtable groups, uint groupID, ref MaterialManager.IStaticProperties staticProps)
		{
			MaterialManager.MaterialsGroup materialsGroup = (MaterialManager.MaterialsGroup)groups[groupID];
			if (materialsGroup == null)
			{
				materialsGroup = new MaterialManager.MaterialsGroup(staticProps.GetPropertiesCount());
				groups[groupID] = materialsGroup;
			}
			int materialID = staticProps.GetMaterialID();
			Material material = materialsGroup.materials[materialID];
			if (material == null)
			{
				material = Config.Instance.NewMaterialTransient(staticProps.GetShaderMode(), true);
				if (material)
				{
					materialsGroup.materials[materialID] = material;
					staticProps.ApplyToMaterial(material);
				}
			}
			return material;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x000184BF File Offset: 0x000166BF
		private static void SetBlendingMode(this Material mat, int nameID, BlendMode value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000184BF File Offset: 0x000166BF
		private static void SetStencilRef(this Material mat, int nameID, int value)
		{
			mat.SetInt(nameID, value);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000184BF File Offset: 0x000166BF
		private static void SetStencilComp(this Material mat, int nameID, CompareFunction value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000184BF File Offset: 0x000166BF
		private static void SetStencilOp(this Material mat, int nameID, StencilOp value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000184BF File Offset: 0x000166BF
		private static void SetCull(this Material mat, int nameID, CullMode value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000184BF File Offset: 0x000166BF
		private static void SetZWrite(this Material mat, int nameID, MaterialManager.ZWrite value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000184BF File Offset: 0x000166BF
		private static void SetZTest(this Material mat, int nameID, CompareFunction value)
		{
			mat.SetInt(nameID, (int)value);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000184CC File Offset: 0x000166CC
		// Note: this type is marked as 'beforefieldinit'.
		static MaterialManager()
		{
			BlendMode[] array = new BlendMode[3];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.F186F2262AE48F2AA4F90C9A6B35913B0F6B0B895423B6267252259BFD357D3B).FieldHandle);
			MaterialManager.BlendingMode_SrcFactor = array;
			BlendMode[] array2 = new BlendMode[3];
			RuntimeHelpers.InitializeArray(array2, fieldof(<PrivateImplementationDetails>.0A0EC6D4742068B4D88C6145B8224EF1DC240C8A305CDFC50C3AAF9121E6875D).FieldHandle);
			MaterialManager.BlendingMode_DstFactor = array2;
			bool[] array3 = new bool[3];
			array3[0] = true;
			array3[1] = true;
			MaterialManager.BlendingMode_AlphaAsBlack = array3;
			MaterialManager.ms_MaterialsGroupSD = new Hashtable(1);
			MaterialManager.ms_MaterialsGroupHD = new Hashtable(1);
		}

		// Token: 0x04000627 RID: 1575
		public static MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04000628 RID: 1576
		private static readonly BlendMode[] BlendingMode_SrcFactor;

		// Token: 0x04000629 RID: 1577
		private static readonly BlendMode[] BlendingMode_DstFactor;

		// Token: 0x0400062A RID: 1578
		private static readonly bool[] BlendingMode_AlphaAsBlack;

		// Token: 0x0400062B RID: 1579
		private static Hashtable ms_MaterialsGroupSD;

		// Token: 0x0400062C RID: 1580
		private static Hashtable ms_MaterialsGroupHD;

		// Token: 0x02000119 RID: 281
		public enum BlendingMode
		{
			// Token: 0x0400062E RID: 1582
			Additive,
			// Token: 0x0400062F RID: 1583
			SoftAdditive,
			// Token: 0x04000630 RID: 1584
			TraditionalTransparency,
			// Token: 0x04000631 RID: 1585
			Count
		}

		// Token: 0x0200011A RID: 282
		public enum ColorGradient
		{
			// Token: 0x04000633 RID: 1587
			Off,
			// Token: 0x04000634 RID: 1588
			MatrixLow,
			// Token: 0x04000635 RID: 1589
			MatrixHigh,
			// Token: 0x04000636 RID: 1590
			Count
		}

		// Token: 0x0200011B RID: 283
		public enum Noise3D
		{
			// Token: 0x04000638 RID: 1592
			Off,
			// Token: 0x04000639 RID: 1593
			On,
			// Token: 0x0400063A RID: 1594
			Count
		}

		// Token: 0x0200011C RID: 284
		public static class SD
		{
			// Token: 0x0200011D RID: 285
			public enum DepthBlend
			{
				// Token: 0x0400063C RID: 1596
				Off,
				// Token: 0x0400063D RID: 1597
				On,
				// Token: 0x0400063E RID: 1598
				Count
			}

			// Token: 0x0200011E RID: 286
			public enum DynamicOcclusion
			{
				// Token: 0x04000640 RID: 1600
				Off,
				// Token: 0x04000641 RID: 1601
				ClippingPlane,
				// Token: 0x04000642 RID: 1602
				DepthTexture,
				// Token: 0x04000643 RID: 1603
				Count
			}

			// Token: 0x0200011F RID: 287
			public enum MeshSkewing
			{
				// Token: 0x04000645 RID: 1605
				Off,
				// Token: 0x04000646 RID: 1606
				On,
				// Token: 0x04000647 RID: 1607
				Count
			}

			// Token: 0x02000120 RID: 288
			public enum ShaderAccuracy
			{
				// Token: 0x04000649 RID: 1609
				Fast,
				// Token: 0x0400064A RID: 1610
				High,
				// Token: 0x0400064B RID: 1611
				Count
			}
		}

		// Token: 0x02000121 RID: 289
		public static class HD
		{
			// Token: 0x02000122 RID: 290
			public enum Attenuation
			{
				// Token: 0x0400064D RID: 1613
				Linear,
				// Token: 0x0400064E RID: 1614
				Quadratic,
				// Token: 0x0400064F RID: 1615
				Count
			}

			// Token: 0x02000123 RID: 291
			public enum Shadow
			{
				// Token: 0x04000651 RID: 1617
				Off,
				// Token: 0x04000652 RID: 1618
				On,
				// Token: 0x04000653 RID: 1619
				Count
			}

			// Token: 0x02000124 RID: 292
			public enum Cookie
			{
				// Token: 0x04000655 RID: 1621
				Off,
				// Token: 0x04000656 RID: 1622
				SingleChannel,
				// Token: 0x04000657 RID: 1623
				RGBA,
				// Token: 0x04000658 RID: 1624
				Count
			}
		}

		// Token: 0x02000125 RID: 293
		private interface IStaticProperties
		{
			// Token: 0x060004ED RID: 1261
			int GetPropertiesCount();

			// Token: 0x060004EE RID: 1262
			int GetMaterialID();

			// Token: 0x060004EF RID: 1263
			void ApplyToMaterial(Material mat);

			// Token: 0x060004F0 RID: 1264
			ShaderMode GetShaderMode();
		}

		// Token: 0x02000126 RID: 294
		public struct StaticPropertiesSD : MaterialManager.IStaticProperties
		{
			// Token: 0x060004F1 RID: 1265 RVA: 0x00014002 File Offset: 0x00012202
			public ShaderMode GetShaderMode()
			{
				return ShaderMode.SD;
			}

			// Token: 0x170000E2 RID: 226
			// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00018538 File Offset: 0x00016738
			public static int staticPropertiesCount
			{
				get
				{
					return 432;
				}
			}

			// Token: 0x060004F3 RID: 1267 RVA: 0x0001853F File Offset: 0x0001673F
			public int GetPropertiesCount()
			{
				return MaterialManager.StaticPropertiesSD.staticPropertiesCount;
			}

			// Token: 0x170000E3 RID: 227
			// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00018546 File Offset: 0x00016746
			private int blendingModeID
			{
				get
				{
					return (int)this.blendingMode;
				}
			}

			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0001854E File Offset: 0x0001674E
			private int noise3DID
			{
				get
				{
					if (!Config.Instance.featureEnabledNoise3D)
					{
						return 0;
					}
					return (int)this.noise3D;
				}
			}

			// Token: 0x170000E5 RID: 229
			// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00018564 File Offset: 0x00016764
			private int depthBlendID
			{
				get
				{
					if (!Config.Instance.featureEnabledDepthBlend)
					{
						return 0;
					}
					return (int)this.depthBlend;
				}
			}

			// Token: 0x170000E6 RID: 230
			// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0001857A File Offset: 0x0001677A
			private int colorGradientID
			{
				get
				{
					if (Config.Instance.featureEnabledColorGradient == FeatureEnabledColorGradient.Off)
					{
						return 0;
					}
					return (int)this.colorGradient;
				}
			}

			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00018590 File Offset: 0x00016790
			private int dynamicOcclusionID
			{
				get
				{
					if (!Config.Instance.featureEnabledDynamicOcclusion)
					{
						return 0;
					}
					return (int)this.dynamicOcclusion;
				}
			}

			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x060004F9 RID: 1273 RVA: 0x000185A6 File Offset: 0x000167A6
			private int meshSkewingID
			{
				get
				{
					if (!Config.Instance.featureEnabledMeshSkewing)
					{
						return 0;
					}
					return (int)this.meshSkewing;
				}
			}

			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x060004FA RID: 1274 RVA: 0x000185BC File Offset: 0x000167BC
			private int shaderAccuracyID
			{
				get
				{
					if (!Config.Instance.featureEnabledShaderAccuracyHigh)
					{
						return 0;
					}
					return (int)this.shaderAccuracy;
				}
			}

			// Token: 0x060004FB RID: 1275 RVA: 0x000185D2 File Offset: 0x000167D2
			public int GetMaterialID()
			{
				return (((((this.blendingModeID * 2 + this.noise3DID) * 2 + this.depthBlendID) * 3 + this.colorGradientID) * 3 + this.dynamicOcclusionID) * 2 + this.meshSkewingID) * 2 + this.shaderAccuracyID;
			}

			// Token: 0x060004FC RID: 1276 RVA: 0x00018610 File Offset: 0x00016810
			public void ApplyToMaterial(Material mat)
			{
				mat.SetKeywordEnabled("VLB_ALPHA_AS_BLACK", MaterialManager.BlendingMode_AlphaAsBlack[(int)this.blendingMode]);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_LOW", this.colorGradient == MaterialManager.ColorGradient.MatrixLow);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_HIGH", this.colorGradient == MaterialManager.ColorGradient.MatrixHigh);
				mat.SetKeywordEnabled("VLB_DEPTH_BLEND", this.depthBlend == MaterialManager.SD.DepthBlend.On);
				mat.SetKeywordEnabled("VLB_NOISE_3D", this.noise3D == MaterialManager.Noise3D.On);
				mat.SetKeywordEnabled("VLB_OCCLUSION_CLIPPING_PLANE", this.dynamicOcclusion == MaterialManager.SD.DynamicOcclusion.ClippingPlane);
				mat.SetKeywordEnabled("VLB_OCCLUSION_DEPTH_TEXTURE", this.dynamicOcclusion == MaterialManager.SD.DynamicOcclusion.DepthTexture);
				mat.SetKeywordEnabled("VLB_MESH_SKEWING", this.meshSkewing == MaterialManager.SD.MeshSkewing.On);
				mat.SetKeywordEnabled("VLB_SHADER_ACCURACY_HIGH", this.shaderAccuracy == MaterialManager.SD.ShaderAccuracy.High);
				mat.SetBlendingMode(ShaderProperties.BlendSrcFactor, MaterialManager.BlendingMode_SrcFactor[(int)this.blendingMode]);
				mat.SetBlendingMode(ShaderProperties.BlendDstFactor, MaterialManager.BlendingMode_DstFactor[(int)this.blendingMode]);
				mat.SetZTest(ShaderProperties.ZTest, CompareFunction.LessEqual);
			}

			// Token: 0x04000659 RID: 1625
			public MaterialManager.BlendingMode blendingMode;

			// Token: 0x0400065A RID: 1626
			public MaterialManager.Noise3D noise3D;

			// Token: 0x0400065B RID: 1627
			public MaterialManager.SD.DepthBlend depthBlend;

			// Token: 0x0400065C RID: 1628
			public MaterialManager.ColorGradient colorGradient;

			// Token: 0x0400065D RID: 1629
			public MaterialManager.SD.DynamicOcclusion dynamicOcclusion;

			// Token: 0x0400065E RID: 1630
			public MaterialManager.SD.MeshSkewing meshSkewing;

			// Token: 0x0400065F RID: 1631
			public MaterialManager.SD.ShaderAccuracy shaderAccuracy;
		}

		// Token: 0x02000127 RID: 295
		public struct StaticPropertiesHD : MaterialManager.IStaticProperties
		{
			// Token: 0x060004FD RID: 1277 RVA: 0x000022C9 File Offset: 0x000004C9
			public ShaderMode GetShaderMode()
			{
				return ShaderMode.HD;
			}

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x060004FE RID: 1278 RVA: 0x0001870E File Offset: 0x0001690E
			public static int staticPropertiesCount
			{
				get
				{
					return 216 * Config.Instance.raymarchingQualitiesCount;
				}
			}

			// Token: 0x060004FF RID: 1279 RVA: 0x00018720 File Offset: 0x00016920
			public int GetPropertiesCount()
			{
				return MaterialManager.StaticPropertiesHD.staticPropertiesCount;
			}

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x06000500 RID: 1280 RVA: 0x00018727 File Offset: 0x00016927
			private int blendingModeID
			{
				get
				{
					return (int)this.blendingMode;
				}
			}

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001872F File Offset: 0x0001692F
			private int attenuationID
			{
				get
				{
					return (int)this.attenuation;
				}
			}

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x06000502 RID: 1282 RVA: 0x00018737 File Offset: 0x00016937
			private int noise3DID
			{
				get
				{
					if (!Config.Instance.featureEnabledNoise3D)
					{
						return 0;
					}
					return (int)this.noise3D;
				}
			}

			// Token: 0x170000EE RID: 238
			// (get) Token: 0x06000503 RID: 1283 RVA: 0x0001874D File Offset: 0x0001694D
			private int colorGradientID
			{
				get
				{
					if (Config.Instance.featureEnabledColorGradient == FeatureEnabledColorGradient.Off)
					{
						return 0;
					}
					return (int)this.colorGradient;
				}
			}

			// Token: 0x170000EF RID: 239
			// (get) Token: 0x06000504 RID: 1284 RVA: 0x00018763 File Offset: 0x00016963
			private int dynamicOcclusionID
			{
				get
				{
					if (!Config.Instance.featureEnabledShadow)
					{
						return 0;
					}
					return (int)this.shadow;
				}
			}

			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x06000505 RID: 1285 RVA: 0x00018779 File Offset: 0x00016979
			private int cookieID
			{
				get
				{
					if (!Config.Instance.featureEnabledCookie)
					{
						return 0;
					}
					return (int)this.cookie;
				}
			}

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x06000506 RID: 1286 RVA: 0x0001878F File Offset: 0x0001698F
			private int raymarchingQualityID
			{
				get
				{
					return this.raymarchingQualityIndex;
				}
			}

			// Token: 0x06000507 RID: 1287 RVA: 0x00018798 File Offset: 0x00016998
			public int GetMaterialID()
			{
				return (((((this.blendingModeID * 2 + this.attenuationID) * 2 + this.noise3DID) * 3 + this.colorGradientID) * 2 + this.dynamicOcclusionID) * 3 + this.cookieID) * Config.Instance.raymarchingQualitiesCount + this.raymarchingQualityID;
			}

			// Token: 0x06000508 RID: 1288 RVA: 0x000187EC File Offset: 0x000169EC
			public void ApplyToMaterial(Material mat)
			{
				mat.SetKeywordEnabled("VLB_ALPHA_AS_BLACK", MaterialManager.BlendingMode_AlphaAsBlack[(int)this.blendingMode]);
				mat.SetKeywordEnabled("VLB_ATTENUATION_LINEAR", this.attenuation == MaterialManager.HD.Attenuation.Linear);
				mat.SetKeywordEnabled("VLB_ATTENUATION_QUAD", this.attenuation == MaterialManager.HD.Attenuation.Quadratic);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_LOW", this.colorGradient == MaterialManager.ColorGradient.MatrixLow);
				mat.SetKeywordEnabled("VLB_COLOR_GRADIENT_MATRIX_HIGH", this.colorGradient == MaterialManager.ColorGradient.MatrixHigh);
				mat.SetKeywordEnabled("VLB_NOISE_3D", this.noise3D == MaterialManager.Noise3D.On);
				mat.SetKeywordEnabled("VLB_SHADOW", this.shadow == MaterialManager.HD.Shadow.On);
				mat.SetKeywordEnabled("VLB_COOKIE_1CHANNEL", this.cookie == MaterialManager.HD.Cookie.SingleChannel);
				mat.SetKeywordEnabled("VLB_COOKIE_RGBA", this.cookie == MaterialManager.HD.Cookie.RGBA);
				for (int i = 0; i < Config.Instance.raymarchingQualitiesCount; i++)
				{
					mat.SetKeywordEnabled(ShaderKeywords.HD.GetRaymarchingQuality(i), this.raymarchingQualityIndex == i);
				}
				mat.SetBlendingMode(ShaderProperties.BlendSrcFactor, MaterialManager.BlendingMode_SrcFactor[(int)this.blendingMode]);
				mat.SetBlendingMode(ShaderProperties.BlendDstFactor, MaterialManager.BlendingMode_DstFactor[(int)this.blendingMode]);
				mat.SetZTest(ShaderProperties.ZTest, CompareFunction.Always);
			}

			// Token: 0x04000660 RID: 1632
			public MaterialManager.BlendingMode blendingMode;

			// Token: 0x04000661 RID: 1633
			public MaterialManager.HD.Attenuation attenuation;

			// Token: 0x04000662 RID: 1634
			public MaterialManager.Noise3D noise3D;

			// Token: 0x04000663 RID: 1635
			public MaterialManager.ColorGradient colorGradient;

			// Token: 0x04000664 RID: 1636
			public MaterialManager.HD.Shadow shadow;

			// Token: 0x04000665 RID: 1637
			public MaterialManager.HD.Cookie cookie;

			// Token: 0x04000666 RID: 1638
			public int raymarchingQualityIndex;
		}

		// Token: 0x02000128 RID: 296
		private class MaterialsGroup
		{
			// Token: 0x06000509 RID: 1289 RVA: 0x00018914 File Offset: 0x00016B14
			public MaterialsGroup(int count)
			{
				this.materials = new Material[count];
			}

			// Token: 0x04000667 RID: 1639
			public Material[] materials;
		}

		// Token: 0x02000129 RID: 297
		private enum ZWrite
		{
			// Token: 0x04000669 RID: 1641
			Off,
			// Token: 0x0400066A RID: 1642
			On
		}
	}
}
