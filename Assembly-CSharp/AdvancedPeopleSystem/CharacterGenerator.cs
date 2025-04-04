using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000208 RID: 520
	public static class CharacterGenerator
	{
		// Token: 0x06000B6B RID: 2923 RVA: 0x00035098 File Offset: 0x00033298
		public static void Generate(CharacterCustomization cc)
		{
			CharacterGeneratorSettings generator = cc.Settings.generator;
			int num = generator.hair.GetRandom(cc.Settings.hairPresets.Count);
			int num2 = generator.beard.GetRandom(cc.Settings.beardPresets.Count);
			int num3 = generator.hat.GetRandom(cc.Settings.hatsPresets.Count);
			int num4 = generator.accessory.GetRandom(cc.Settings.accessoryPresets.Count);
			int num5 = generator.shirt.GetRandom(cc.Settings.shirtsPresets.Count);
			int num6 = generator.pants.GetRandom(cc.Settings.pantsPresets.Count);
			int num7 = generator.shoes.GetRandom(cc.Settings.shoesPresets.Count);
			float random = generator.headSize.GetRandom();
			float random2 = generator.headOffset.GetRandom();
			float random3 = generator.height.GetRandom();
			foreach (GeneratorExclude generatorExclude in generator.excludes)
			{
				if (CharacterGenerator.<Generate>g__CheckExclude|0_0(num, num2, num3, num4, num5, num6, num7, generatorExclude.exclude) == -1)
				{
					if (generatorExclude.ExcludeItem == ExcludeItem.Hair && num == generatorExclude.targetIndex)
					{
						num = -1;
					}
					if (generatorExclude.ExcludeItem == ExcludeItem.Beard && num2 == generatorExclude.targetIndex)
					{
						num2 = -1;
					}
					if (generatorExclude.ExcludeItem == ExcludeItem.Hat && num3 == generatorExclude.targetIndex)
					{
						num3 = -1;
					}
					if (generatorExclude.ExcludeItem == ExcludeItem.Accessory && num4 == generatorExclude.targetIndex)
					{
						num4 = -1;
					}
					if (generatorExclude.ExcludeItem == ExcludeItem.Shirt && num5 == generatorExclude.targetIndex)
					{
						num5 = -1;
					}
					if (generatorExclude.ExcludeItem == ExcludeItem.Pants && num6 == generatorExclude.targetIndex)
					{
						num6 = -1;
					}
					if (generatorExclude.ExcludeItem == ExcludeItem.Shoes && num7 == generatorExclude.targetIndex)
					{
						num7 = -1;
					}
				}
			}
			cc.SetHeadSize(random);
			cc.SetHeight(random3);
			cc.SetBlendshapeValue(CharacterBlendShapeType.Fat, generator.fat.GetRandom(), null, null);
			cc.SetElementByIndex(CharacterElementType.Hair, num);
			cc.SetElementByIndex(CharacterElementType.Beard, num2);
			cc.SetElementByIndex(CharacterElementType.Accessory, num4);
			cc.SetElementByIndex(CharacterElementType.Shirt, num5);
			cc.SetElementByIndex(CharacterElementType.Pants, num6);
			cc.SetElementByIndex(CharacterElementType.Shoes, num7);
			cc.SetElementByIndex(CharacterElementType.Hat, num3);
			cc.SetBodyColor(BodyColorPart.Skin, generator.skinColors.GetRandom());
			cc.SetBodyColor(BodyColorPart.Hair, generator.hairColors.GetRandom());
			cc.SetBodyColor(BodyColorPart.Eye, generator.eyeColors.GetRandom());
			cc.SetBlendshapeValue(CharacterBlendShapeType.Head_Offset, random2, null, null);
			foreach (MinMaxFacialBlendshapes minMaxFacialBlendshapes in generator.facialBlendshapes)
			{
				CharacterBlendShapeType type;
				if (Enum.TryParse<CharacterBlendShapeType>(minMaxFacialBlendshapes.name, out type))
				{
					cc.SetBlendshapeValue(type, minMaxFacialBlendshapes.GetRandom(), null, null);
				}
			}
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x000353A8 File Offset: 0x000335A8
		[CompilerGenerated]
		internal static int <Generate>g__CheckExclude|0_0(int hair, int beard, int hat, int accessory, int shirt, int pants, int shoes, List<ExcludeIndexes> excludeIndexes)
		{
			int result = 0;
			if (excludeIndexes.Count == 0)
			{
				result = -1;
			}
			else
			{
				foreach (ExcludeIndexes excludeIndexes2 in excludeIndexes)
				{
					if (excludeIndexes2.item == ExcludeItem.Hair && hair == excludeIndexes2.index)
					{
						result = -1;
						break;
					}
					if (excludeIndexes2.item == ExcludeItem.Beard && beard == excludeIndexes2.index)
					{
						result = -1;
						break;
					}
					if (excludeIndexes2.item == ExcludeItem.Hat && hat == excludeIndexes2.index)
					{
						result = -1;
						break;
					}
					if (excludeIndexes2.item == ExcludeItem.Accessory && accessory == excludeIndexes2.index)
					{
						result = -1;
						break;
					}
					if (excludeIndexes2.item == ExcludeItem.Shirt && shirt == excludeIndexes2.index)
					{
						result = -1;
						break;
					}
					if (excludeIndexes2.item == ExcludeItem.Pants && pants == excludeIndexes2.index)
					{
						result = -1;
						break;
					}
					if (excludeIndexes2.item == ExcludeItem.Shoes && shoes == excludeIndexes2.index)
					{
						result = -1;
						break;
					}
				}
			}
			return result;
		}
	}
}
