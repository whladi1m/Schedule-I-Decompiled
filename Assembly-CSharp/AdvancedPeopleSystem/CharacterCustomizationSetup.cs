using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000205 RID: 517
	[Serializable]
	public class CharacterCustomizationSetup
	{
		// Token: 0x06000B66 RID: 2918 RVA: 0x00034CB8 File Offset: 0x00032EB8
		public void ApplyToCharacter(CharacterCustomization cc)
		{
			if (cc.Settings == null && this.settingsName != cc.Settings.name)
			{
				Debug.LogError("Character settings are not compatible with saved data");
				return;
			}
			cc.SetBodyColor(BodyColorPart.Skin, new Color(this.SkinColor[0], this.SkinColor[1], this.SkinColor[2], this.SkinColor[3]));
			cc.SetBodyColor(BodyColorPart.Eye, new Color(this.EyeColor[0], this.EyeColor[1], this.EyeColor[2], this.EyeColor[3]));
			cc.SetBodyColor(BodyColorPart.Hair, new Color(this.HairColor[0], this.HairColor[1], this.HairColor[2], this.HairColor[3]));
			cc.SetBodyColor(BodyColorPart.Underpants, new Color(this.UnderpantsColor[0], this.UnderpantsColor[1], this.UnderpantsColor[2], this.UnderpantsColor[3]));
			cc.SetBodyColor(BodyColorPart.Teeth, new Color(this.TeethColor[0], this.TeethColor[1], this.TeethColor[2], this.TeethColor[3]));
			cc.SetBodyColor(BodyColorPart.OralCavity, new Color(this.OralCavityColor[0], this.OralCavityColor[1], this.OralCavityColor[2], this.OralCavityColor[3]));
			cc.SetElementByIndex(CharacterElementType.Hair, this.selectedElements.Hair);
			cc.SetElementByIndex(CharacterElementType.Accessory, this.selectedElements.Accessory);
			cc.SetElementByIndex(CharacterElementType.Hat, this.selectedElements.Hat);
			cc.SetElementByIndex(CharacterElementType.Pants, this.selectedElements.Pants);
			cc.SetElementByIndex(CharacterElementType.Shoes, this.selectedElements.Shoes);
			cc.SetElementByIndex(CharacterElementType.Shirt, this.selectedElements.Shirt);
			cc.SetElementByIndex(CharacterElementType.Beard, this.selectedElements.Beard);
			cc.SetElementByIndex(CharacterElementType.Item1, this.selectedElements.Item1);
			cc.SetHeight(this.Height);
			cc.SetHeadSize(this.HeadSize);
			foreach (CharacterBlendshapeData characterBlendshapeData in this.blendshapes)
			{
				cc.SetBlendshapeValue(characterBlendshapeData.type, characterBlendshapeData.value, null, null);
			}
			cc.ApplyPrefab();
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00034F00 File Offset: 0x00033100
		public string Serialize(CharacterCustomizationSetup.CharacterFileSaveFormat format)
		{
			string result = string.Empty;
			switch (format)
			{
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Json:
				return JsonUtility.ToJson(this, true);
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Xml:
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(CharacterCustomizationSetup));
				using (StringWriter stringWriter = new StringWriter())
				{
					xmlSerializer.Serialize(stringWriter, this);
					return stringWriter.ToString();
				}
				break;
			}
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Binary:
				break;
			default:
				return result;
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				new BinaryFormatter().Serialize(memoryStream, this);
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
			return result;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00034FAC File Offset: 0x000331AC
		public static CharacterCustomizationSetup Deserialize(string data, CharacterCustomizationSetup.CharacterFileSaveFormat format)
		{
			CharacterCustomizationSetup result = null;
			switch (format)
			{
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Json:
				return JsonUtility.FromJson<CharacterCustomizationSetup>(data);
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Xml:
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(CharacterCustomizationSetup));
				using (StringReader stringReader = new StringReader(data))
				{
					return (CharacterCustomizationSetup)xmlSerializer.Deserialize(stringReader);
				}
				break;
			}
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Binary:
				break;
			default:
				return result;
			}
			using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data)))
			{
				result = (CharacterCustomizationSetup)new BinaryFormatter().Deserialize(memoryStream);
			}
			return result;
		}

		// Token: 0x04000C3C RID: 3132
		public string settingsName;

		// Token: 0x04000C3D RID: 3133
		public CharacterSelectedElements selectedElements = new CharacterSelectedElements();

		// Token: 0x04000C3E RID: 3134
		public List<CharacterBlendshapeData> blendshapes = new List<CharacterBlendshapeData>();

		// Token: 0x04000C3F RID: 3135
		public int MinLod;

		// Token: 0x04000C40 RID: 3136
		public int MaxLod;

		// Token: 0x04000C41 RID: 3137
		public float[] SkinColor;

		// Token: 0x04000C42 RID: 3138
		public float[] EyeColor;

		// Token: 0x04000C43 RID: 3139
		public float[] HairColor;

		// Token: 0x04000C44 RID: 3140
		public float[] UnderpantsColor;

		// Token: 0x04000C45 RID: 3141
		public float[] TeethColor;

		// Token: 0x04000C46 RID: 3142
		public float[] OralCavityColor;

		// Token: 0x04000C47 RID: 3143
		public float Height;

		// Token: 0x04000C48 RID: 3144
		public float HeadSize;

		// Token: 0x02000206 RID: 518
		public enum CharacterFileSaveFormat
		{
			// Token: 0x04000C4A RID: 3146
			Json,
			// Token: 0x04000C4B RID: 3147
			Xml,
			// Token: 0x04000C4C RID: 3148
			Binary
		}
	}
}
