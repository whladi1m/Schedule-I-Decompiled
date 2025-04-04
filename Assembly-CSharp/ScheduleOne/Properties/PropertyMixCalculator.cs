using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Product;
using ScheduleOne.Properties.MixMaps;
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000328 RID: 808
	public static class PropertyMixCalculator
	{
		// Token: 0x060011C8 RID: 4552 RVA: 0x0004D648 File Offset: 0x0004B848
		public static List<Property> MixProperties(List<Property> existingProperties, Property newProperty, EDrugType drugType)
		{
			StationRecipe recipe = NetworkSingleton<ProductManager>.Instance.GetRecipe(existingProperties, newProperty);
			if (recipe != null)
			{
				Console.Log("Existing recipe found! for " + recipe.Product.Item.Name, null);
				return (recipe.Product.Item as ProductDefinition).Properties;
			}
			Vector2 b = newProperty.MixDirection * newProperty.MixMagnitude;
			MixerMap mixerMap = NetworkSingleton<ProductManager>.Instance.GetMixerMap(drugType);
			List<PropertyMixCalculator.Reaction> list = new List<PropertyMixCalculator.Reaction>();
			for (int i = 0; i < existingProperties.Count; i++)
			{
				Vector2 point = mixerMap.GetEffect(existingProperties[i]).Position + b;
				MixerMapEffect effectAtPoint = mixerMap.GetEffectAtPoint(point);
				Property property = (effectAtPoint != null) ? effectAtPoint.Property : null;
				if (property != null)
				{
					PropertyMixCalculator.Reaction item = new PropertyMixCalculator.Reaction
					{
						Existing = existingProperties[i],
						Output = property
					};
					list.Add(item);
				}
			}
			List<Property> list2 = new List<Property>(existingProperties);
			foreach (PropertyMixCalculator.Reaction reaction in list)
			{
				if (!list2.Contains(reaction.Output))
				{
					list2[list2.IndexOf(reaction.Existing)] = reaction.Output;
				}
			}
			if (!list2.Contains(newProperty) && list2.Count < 8)
			{
				list2.Add(newProperty);
			}
			list2 = list2.Distinct<Property>().ToList<Property>();
			return list2;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0004D7D8 File Offset: 0x0004B9D8
		public static void Shuffle<T>(List<T> list, int seed)
		{
			System.Random random = new System.Random(seed);
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int index = random.Next(i + 1);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}

		// Token: 0x04001157 RID: 4439
		public const int MAX_PROPERTIES = 8;

		// Token: 0x04001158 RID: 4440
		public const float MAX_DELTA_DIFFERENCE = 0.5f;

		// Token: 0x02000329 RID: 809
		private class Reaction
		{
			// Token: 0x04001159 RID: 4441
			public Property Existing;

			// Token: 0x0400115A RID: 4442
			public Property Output;
		}
	}
}
