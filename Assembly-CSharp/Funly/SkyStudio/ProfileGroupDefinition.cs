using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001B4 RID: 436
	public class ProfileGroupDefinition
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x00027594 File Offset: 0x00025794
		public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, string tooltip)
		{
			return ProfileGroupDefinition.NumberGroupDefinition(groupName, propKey, minimumValue, maximumValue, value, ProfileGroupDefinition.RebuildType.None, null, false, tooltip);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x000275B4 File Offset: 0x000257B4
		public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return ProfileGroupDefinition.NumberGroupDefinition(groupName, propKey, minimumValue, maximumValue, value, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Float, dependsOnKeyword, dependsOnValue, tooltip);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x000275D4 File Offset: 0x000257D4
		public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, ProfileGroupDefinition.RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return ProfileGroupDefinition.NumberGroupDefinition(groupName, propKey, minimumValue, maximumValue, value, rebuildType, ProfileGroupDefinition.FormatStyle.Float, dependsOnKeyword, dependsOnValue, tooltip);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x000275F8 File Offset: 0x000257F8
		public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, ProfileGroupDefinition.RebuildType rebuildType, ProfileGroupDefinition.FormatStyle formatStyle, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return new ProfileGroupDefinition
			{
				type = ProfileGroupDefinition.GroupType.Number,
				formatStyle = formatStyle,
				groupName = groupName,
				propertyKey = propKey,
				value = value,
				minimumValue = minimumValue,
				maximumValue = maximumValue,
				tooltip = tooltip,
				rebuildType = rebuildType,
				dependsOnFeature = dependsOnKeyword,
				dependsOnValue = dependsOnValue
			};
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0002765D File Offset: 0x0002585D
		public static ProfileGroupDefinition ColorGroupDefinition(string groupName, string propKey, Color color, string tooltip)
		{
			return ProfileGroupDefinition.ColorGroupDefinition(groupName, propKey, color, ProfileGroupDefinition.RebuildType.None, null, false, tooltip);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0002766B File Offset: 0x0002586B
		public static ProfileGroupDefinition ColorGroupDefinition(string groupName, string propKey, Color color, string dependsOnFeature, bool dependsOnValue, string tooltip)
		{
			return ProfileGroupDefinition.ColorGroupDefinition(groupName, propKey, color, ProfileGroupDefinition.RebuildType.None, dependsOnFeature, dependsOnValue, tooltip);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0002767C File Offset: 0x0002587C
		public static ProfileGroupDefinition ColorGroupDefinition(string groupName, string propKey, Color color, ProfileGroupDefinition.RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return new ProfileGroupDefinition
			{
				type = ProfileGroupDefinition.GroupType.Color,
				propertyKey = propKey,
				groupName = groupName,
				color = color,
				tooltip = tooltip,
				rebuildType = rebuildType,
				dependsOnFeature = dependsOnKeyword,
				dependsOnValue = dependsOnValue
			};
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x000276C9 File Offset: 0x000258C9
		public static ProfileGroupDefinition SpherePointGroupDefinition(string groupName, string propKey, float horizontalRotation, float verticalRotation, string tooltip)
		{
			return ProfileGroupDefinition.SpherePointGroupDefinition(groupName, propKey, horizontalRotation, verticalRotation, ProfileGroupDefinition.RebuildType.None, null, false, tooltip);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x000276DC File Offset: 0x000258DC
		public static ProfileGroupDefinition SpherePointGroupDefinition(string groupName, string propKey, float horizontalRotation, float verticalRotation, ProfileGroupDefinition.RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return new ProfileGroupDefinition
			{
				type = ProfileGroupDefinition.GroupType.SpherePoint,
				propertyKey = propKey,
				groupName = groupName,
				tooltip = tooltip,
				rebuildType = rebuildType,
				dependsOnFeature = dependsOnKeyword,
				dependsOnValue = dependsOnValue,
				spherePoint = new SpherePoint(horizontalRotation, verticalRotation)
			};
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00027730 File Offset: 0x00025930
		public static ProfileGroupDefinition TextureGroupDefinition(string groupName, string propKey, Texture2D texture, string tooltip)
		{
			return ProfileGroupDefinition.TextureGroupDefinition(groupName, propKey, texture, ProfileGroupDefinition.RebuildType.None, null, false, tooltip);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0002773E File Offset: 0x0002593E
		public static ProfileGroupDefinition TextureGroupDefinition(string groupName, string propKey, Texture2D texture, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return ProfileGroupDefinition.TextureGroupDefinition(groupName, propKey, texture, ProfileGroupDefinition.RebuildType.None, dependsOnKeyword, dependsOnValue, tooltip);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00027750 File Offset: 0x00025950
		public static ProfileGroupDefinition TextureGroupDefinition(string groupName, string propKey, Texture2D texture, ProfileGroupDefinition.RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return new ProfileGroupDefinition
			{
				type = ProfileGroupDefinition.GroupType.Texture,
				groupName = groupName,
				propertyKey = propKey,
				texture = texture,
				tooltip = tooltip,
				rebuildType = rebuildType,
				dependsOnFeature = dependsOnKeyword,
				dependsOnValue = dependsOnValue
			};
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0002779D File Offset: 0x0002599D
		public static ProfileGroupDefinition BoolGroupDefinition(string groupName, string propKey, bool value, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return new ProfileGroupDefinition
			{
				type = ProfileGroupDefinition.GroupType.Boolean,
				groupName = groupName,
				propertyKey = propKey,
				dependsOnFeature = dependsOnKeyword,
				dependsOnValue = dependsOnValue,
				tooltip = tooltip,
				boolValue = value
			};
		}

		// Token: 0x040009A8 RID: 2472
		public ProfileGroupDefinition.GroupType type;

		// Token: 0x040009A9 RID: 2473
		public ProfileGroupDefinition.FormatStyle formatStyle;

		// Token: 0x040009AA RID: 2474
		public ProfileGroupDefinition.RebuildType rebuildType;

		// Token: 0x040009AB RID: 2475
		public string propertyKey;

		// Token: 0x040009AC RID: 2476
		public string groupName;

		// Token: 0x040009AD RID: 2477
		public Color color;

		// Token: 0x040009AE RID: 2478
		public SpherePoint spherePoint;

		// Token: 0x040009AF RID: 2479
		public float minimumValue = -1f;

		// Token: 0x040009B0 RID: 2480
		public float maximumValue = -1f;

		// Token: 0x040009B1 RID: 2481
		public float value = -1f;

		// Token: 0x040009B2 RID: 2482
		public bool boolValue;

		// Token: 0x040009B3 RID: 2483
		public Texture2D texture;

		// Token: 0x040009B4 RID: 2484
		public string tooltip;

		// Token: 0x040009B5 RID: 2485
		public string dependsOnFeature;

		// Token: 0x040009B6 RID: 2486
		public bool dependsOnValue;

		// Token: 0x020001B5 RID: 437
		public enum GroupType
		{
			// Token: 0x040009B8 RID: 2488
			None,
			// Token: 0x040009B9 RID: 2489
			Color,
			// Token: 0x040009BA RID: 2490
			Number,
			// Token: 0x040009BB RID: 2491
			Texture,
			// Token: 0x040009BC RID: 2492
			SpherePoint,
			// Token: 0x040009BD RID: 2493
			Boolean
		}

		// Token: 0x020001B6 RID: 438
		public enum FormatStyle
		{
			// Token: 0x040009BF RID: 2495
			None,
			// Token: 0x040009C0 RID: 2496
			Integer,
			// Token: 0x040009C1 RID: 2497
			Float
		}

		// Token: 0x020001B7 RID: 439
		public enum RebuildType
		{
			// Token: 0x040009C3 RID: 2499
			None,
			// Token: 0x040009C4 RID: 2500
			Stars
		}
	}
}
