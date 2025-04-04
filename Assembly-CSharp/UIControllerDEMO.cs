using System;
using System.Collections.Generic;
using AdvancedPeopleSystem;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000029 RID: 41
public class UIControllerDEMO : MonoBehaviour
{
	// Token: 0x060000BD RID: 189 RVA: 0x0000596C File Offset: 0x00003B6C
	public void SwitchCharacterSettings(string name)
	{
		this.CharacterCustomization.SwitchCharacterSettings(name);
		if (name == "Male")
		{
			this.maleUI.gameObject.SetActive(true);
			this.femaleUI.gameObject.SetActive(false);
		}
		if (name == "Female")
		{
			this.femaleUI.gameObject.SetActive(true);
			this.maleUI.gameObject.SetActive(false);
		}
	}

	// Token: 0x060000BE RID: 190 RVA: 0x000059E3 File Offset: 0x00003BE3
	public void ShowFaceEdit()
	{
		this.FaceEditPanel.gameObject.SetActive(true);
		this.BaseEditPanel.gameObject.SetActive(false);
		this.currentPanelIndex = 1;
		this.panelNameText.text = "FACE CUSTOMIZER";
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00005A1E File Offset: 0x00003C1E
	public void ShowBaseEdit()
	{
		this.FaceEditPanel.gameObject.SetActive(false);
		this.BaseEditPanel.gameObject.SetActive(true);
		this.currentPanelIndex = 0;
		this.panelNameText.text = "BASE CUSTOMIZER";
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00005A5C File Offset: 0x00003C5C
	public void SetFaceShape(int index)
	{
		List<CharacterBlendshapeData> blendshapeDatasByGroup = this.CharacterCustomization.GetBlendshapeDatasByGroup(CharacterBlendShapeGroup.Face);
		this.CharacterCustomization.SetBlendshapeValue(blendshapeDatasByGroup[index].type, this.faceShapeSliders[index].value, null, null);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00005A9C File Offset: 0x00003C9C
	public void SetHeadOffset()
	{
		this.CharacterCustomization.SetBlendshapeValue(CharacterBlendShapeType.Head_Offset, this.headOffsetSlider.value, null, null);
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00005AB8 File Offset: 0x00003CB8
	public void BodyFat()
	{
		this.CharacterCustomization.SetBlendshapeValue(CharacterBlendShapeType.Fat, this.fatSlider.value, null, null);
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00005AD3 File Offset: 0x00003CD3
	public void BodyMuscles()
	{
		this.CharacterCustomization.SetBlendshapeValue(CharacterBlendShapeType.Muscles, this.musclesSlider.value, null, null);
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00005AEE File Offset: 0x00003CEE
	public void BodyThin()
	{
		this.CharacterCustomization.SetBlendshapeValue(CharacterBlendShapeType.Thin, this.thinSlider.value, null, null);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00005B09 File Offset: 0x00003D09
	public void BodySlimness()
	{
		this.CharacterCustomization.SetBlendshapeValue(CharacterBlendShapeType.Slimness, this.slimnessSlider.value, null, null);
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00005B24 File Offset: 0x00003D24
	public void BodyBreast()
	{
		this.CharacterCustomization.SetBlendshapeValue(CharacterBlendShapeType.BreastSize, this.breastSlider.value, new string[]
		{
			"Chest",
			"Stomach",
			"Head"
		}, new CharacterElementType[]
		{
			CharacterElementType.Shirt
		});
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00005B70 File Offset: 0x00003D70
	public void SetHeight()
	{
		this.CharacterCustomization.SetHeight(this.heightSlider.value);
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00005B88 File Offset: 0x00003D88
	public void SetHeadSize()
	{
		this.CharacterCustomization.SetHeadSize(this.headSizeSlider.value);
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00005BA0 File Offset: 0x00003DA0
	public void Lod_Event(int next)
	{
		this.lodIndex += next;
		if (this.lodIndex < 0)
		{
			this.lodIndex = 3;
		}
		if (this.lodIndex > 3)
		{
			this.lodIndex = 0;
		}
		this.lod_text.text = this.lodIndex.ToString();
		this.CharacterCustomization.ForceLOD(this.lodIndex);
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00005C02 File Offset: 0x00003E02
	public void SetNewSkinColor(Color color)
	{
		this.SkinColorButtonColor.color = color;
		this.CharacterCustomization.SetBodyColor(BodyColorPart.Skin, color);
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00005C1D File Offset: 0x00003E1D
	public void SetNewEyeColor(Color color)
	{
		this.EyeColorButtonColor.color = color;
		this.CharacterCustomization.SetBodyColor(BodyColorPart.Eye, color);
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00005C38 File Offset: 0x00003E38
	public void SetNewHairColor(Color color)
	{
		this.HairColorButtonColor.color = color;
		this.CharacterCustomization.SetBodyColor(BodyColorPart.Hair, color);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005C53 File Offset: 0x00003E53
	public void SetNewUnderpantsColor(Color color)
	{
		this.UnderpantsColorButtonColor.color = color;
		this.CharacterCustomization.SetBodyColor(BodyColorPart.Underpants, color);
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00005C6E File Offset: 0x00003E6E
	public void VisibleSkinColorPanel(bool v)
	{
		this.HideAllPanels();
		this.SkinColorPanel.gameObject.SetActive(v);
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00005C87 File Offset: 0x00003E87
	public void VisibleEyeColorPanel(bool v)
	{
		this.HideAllPanels();
		this.EyeColorPanel.gameObject.SetActive(v);
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00005CA0 File Offset: 0x00003EA0
	public void VisibleHairColorPanel(bool v)
	{
		this.HideAllPanels();
		this.HairColorPanel.gameObject.SetActive(v);
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00005CB9 File Offset: 0x00003EB9
	public void VisibleUnderpantsColorPanel(bool v)
	{
		this.HideAllPanels();
		this.UnderpantsColorPanel.gameObject.SetActive(v);
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00005CD2 File Offset: 0x00003ED2
	public void ShirtPanel_Select(bool v)
	{
		this.HideAllPanels();
		if (!v)
		{
			this.ShirtPanel.gameObject.SetActive(false);
			return;
		}
		this.ShirtPanel.gameObject.SetActive(true);
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00005D00 File Offset: 0x00003F00
	public void PantsPanel_Select(bool v)
	{
		this.HideAllPanels();
		if (!v)
		{
			this.PantsPanel.gameObject.SetActive(false);
			return;
		}
		this.PantsPanel.gameObject.SetActive(true);
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00005D2E File Offset: 0x00003F2E
	public void ShoesPanel_Select(bool v)
	{
		this.HideAllPanels();
		if (!v)
		{
			this.ShoesPanel.gameObject.SetActive(false);
			return;
		}
		this.ShoesPanel.gameObject.SetActive(true);
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00005D5C File Offset: 0x00003F5C
	public void BackpackPanel_Select(bool v)
	{
		this.HideAllPanels();
		if (!v)
		{
			this.BackpackPanel.gameObject.SetActive(false);
			return;
		}
		this.BackpackPanel.gameObject.SetActive(true);
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00005D8A File Offset: 0x00003F8A
	public void HairPanel_Select(bool v)
	{
		this.HideAllPanels();
		if (!v)
		{
			this.HairPanel.gameObject.SetActive(false);
		}
		else
		{
			this.HairPanel.gameObject.SetActive(true);
		}
		this.currentPanelIndex = (v ? 1 : 0);
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00005DC6 File Offset: 0x00003FC6
	public void BeardPanel_Select(bool v)
	{
		this.HideAllPanels();
		if (!v)
		{
			this.BeardPanel.gameObject.SetActive(false);
		}
		else
		{
			this.BeardPanel.gameObject.SetActive(true);
		}
		this.currentPanelIndex = (v ? 1 : 0);
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00005E02 File Offset: 0x00004002
	public void HatPanel_Select(bool v)
	{
		this.HideAllPanels();
		if (!v)
		{
			this.HatPanel.gameObject.SetActive(false);
		}
		else
		{
			this.HatPanel.gameObject.SetActive(true);
		}
		this.currentPanelIndex = (v ? 1 : 0);
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00005E3E File Offset: 0x0000403E
	public void EmotionsPanel_Select(bool v)
	{
		this.HideAllPanels();
		if (!v)
		{
			this.EmotionsPanel.gameObject.SetActive(false);
		}
		else
		{
			this.EmotionsPanel.gameObject.SetActive(true);
		}
		this.currentPanelIndex = (v ? 1 : 0);
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00005E7A File Offset: 0x0000407A
	public void AccessoryPanel_Select(bool v)
	{
		this.HideAllPanels();
		if (!v)
		{
			this.AccessoryPanel.gameObject.SetActive(false);
		}
		else
		{
			this.AccessoryPanel.gameObject.SetActive(true);
		}
		this.currentPanelIndex = (v ? 1 : 0);
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00005EB8 File Offset: 0x000040B8
	public void SavesPanel_Select(bool v)
	{
		this.HideAllPanels();
		if (!v)
		{
			this.SavesPanel.gameObject.SetActive(false);
			foreach (RectTransform rectTransform in this.SavesList)
			{
				UnityEngine.Object.Destroy(rectTransform.gameObject);
			}
			this.SavesList.Clear();
			return;
		}
		List<SavedCharacterData> savedCharacterDatas = this.CharacterCustomization.GetSavedCharacterDatas("");
		for (int i = 0; i < savedCharacterDatas.Count; i++)
		{
			RectTransform rectTransform2 = UnityEngine.Object.Instantiate<RectTransform>(this.SavesPrefab, this.SavesPanelList);
			int index = i;
			rectTransform2.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.SaveSelect(index);
			});
			rectTransform2.GetComponentInChildren<Text>().text = string.Format("({0}) {1}", index, savedCharacterDatas[i].name);
			this.SavesList.Add(rectTransform2);
		}
		this.SavesPanel.gameObject.SetActive(true);
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00005FEC File Offset: 0x000041EC
	public void SaveSelect(int index)
	{
		List<SavedCharacterData> savedCharacterDatas = this.CharacterCustomization.GetSavedCharacterDatas("");
		this.CharacterCustomization.ApplySavedCharacterData(savedCharacterDatas[index]);
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0000601C File Offset: 0x0000421C
	public void EmotionsChange_Event(int index)
	{
		CharacterAnimationPreset characterAnimationPreset = this.CharacterCustomization.Settings.characterAnimationPresets[index];
		if (characterAnimationPreset != null)
		{
			this.CharacterCustomization.PlayBlendshapeAnimation(characterAnimationPreset.name, 2f, 1f);
		}
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0000605E File Offset: 0x0000425E
	public void HairChange_Event(int index)
	{
		this.CharacterCustomization.SetElementByIndex(CharacterElementType.Hair, index);
	}

	// Token: 0x060000DF RID: 223 RVA: 0x0000606D File Offset: 0x0000426D
	public void BeardChange_Event(int index)
	{
		this.CharacterCustomization.SetElementByIndex(CharacterElementType.Beard, index);
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x0000607C File Offset: 0x0000427C
	public void ShirtChange_Event(int index)
	{
		this.CharacterCustomization.SetElementByIndex(CharacterElementType.Shirt, index);
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x0000608B File Offset: 0x0000428B
	public void PantsChange_Event(int index)
	{
		this.CharacterCustomization.SetElementByIndex(CharacterElementType.Pants, index);
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0000609A File Offset: 0x0000429A
	public void ShoesChange_Event(int index)
	{
		this.CharacterCustomization.SetElementByIndex(CharacterElementType.Shoes, index);
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x000060A9 File Offset: 0x000042A9
	public void BackpackChange_Event(int index)
	{
		this.CharacterCustomization.SetElementByIndex(CharacterElementType.Item1, index);
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000060B8 File Offset: 0x000042B8
	public void HatChange_Event(int index)
	{
		this.CharacterCustomization.SetElementByIndex(CharacterElementType.Hat, index);
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x000060C7 File Offset: 0x000042C7
	public void AccessoryChange_Event(int index)
	{
		this.CharacterCustomization.SetElementByIndex(CharacterElementType.Accessory, index);
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x000060D8 File Offset: 0x000042D8
	public void HideAllPanels()
	{
		this.SkinColorPanel.gameObject.SetActive(false);
		this.EyeColorPanel.gameObject.SetActive(false);
		this.HairColorPanel.gameObject.SetActive(false);
		this.UnderpantsColorPanel.gameObject.SetActive(false);
		if (this.EmotionsPanel != null)
		{
			this.EmotionsPanel.gameObject.SetActive(false);
		}
		if (this.BeardPanel != null)
		{
			this.BeardPanel.gameObject.SetActive(false);
		}
		this.HairPanel.gameObject.SetActive(false);
		this.ShirtPanel.gameObject.SetActive(false);
		this.PantsPanel.gameObject.SetActive(false);
		this.ShoesPanel.gameObject.SetActive(false);
		this.BackpackPanel.gameObject.SetActive(false);
		this.HatPanel.gameObject.SetActive(false);
		this.AccessoryPanel.gameObject.SetActive(false);
		this.SavesPanel.gameObject.SetActive(false);
		this.currentPanelIndex = 0;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x000061F6 File Offset: 0x000043F6
	public void SaveToFile()
	{
		this.CharacterCustomization.SaveCharacterToFile(CharacterCustomizationSetup.CharacterFileSaveFormat.Json, "", "");
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000620E File Offset: 0x0000440E
	public void ClearFromFile()
	{
		this.SavesPanel.gameObject.SetActive(false);
		this.CharacterCustomization.ClearSavedData();
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0000622C File Offset: 0x0000442C
	public void Randimize()
	{
		this.CharacterCustomization.Randomize();
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0000623C File Offset: 0x0000443C
	public void PlayAnim()
	{
		this.walk_active = !this.walk_active;
		this.CharacterCustomization.GetAnimator().SetBool("walk", this.walk_active);
		this.playbutton_text.text = (this.walk_active ? "STOP" : "PLAY");
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00006294 File Offset: 0x00004494
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			this.canvasVisible = !this.canvasVisible;
			GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().enabled = this.canvasVisible;
		}
		this.Camera.transform.position = Vector3.Lerp(this.Camera.transform.position, this.CameraPositionForPanels[this.currentPanelIndex], Time.deltaTime * 5f);
		this.Camera.transform.eulerAngles = Vector3.Lerp(this.Camera.transform.eulerAngles, this.CameraEulerForPanels[this.currentPanelIndex], Time.deltaTime * 5f);
	}

	// Token: 0x040000AC RID: 172
	[Space(5f)]
	[Header("I do not recommend using it in your projects")]
	[Header("This script was created to demonstrate api")]
	public CharacterCustomization CharacterCustomization;

	// Token: 0x040000AD RID: 173
	[Space(15f)]
	public Text playbutton_text;

	// Token: 0x040000AE RID: 174
	public Text bake_text;

	// Token: 0x040000AF RID: 175
	public Text lod_text;

	// Token: 0x040000B0 RID: 176
	public Text panelNameText;

	// Token: 0x040000B1 RID: 177
	public Slider fatSlider;

	// Token: 0x040000B2 RID: 178
	public Slider musclesSlider;

	// Token: 0x040000B3 RID: 179
	public Slider thinSlider;

	// Token: 0x040000B4 RID: 180
	public Slider slimnessSlider;

	// Token: 0x040000B5 RID: 181
	public Slider breastSlider;

	// Token: 0x040000B6 RID: 182
	public Slider heightSlider;

	// Token: 0x040000B7 RID: 183
	public Slider legSlider;

	// Token: 0x040000B8 RID: 184
	public Slider headSizeSlider;

	// Token: 0x040000B9 RID: 185
	public Slider headOffsetSlider;

	// Token: 0x040000BA RID: 186
	public Slider[] faceShapeSliders;

	// Token: 0x040000BB RID: 187
	public RectTransform HairPanel;

	// Token: 0x040000BC RID: 188
	public RectTransform BeardPanel;

	// Token: 0x040000BD RID: 189
	public RectTransform ShirtPanel;

	// Token: 0x040000BE RID: 190
	public RectTransform PantsPanel;

	// Token: 0x040000BF RID: 191
	public RectTransform ShoesPanel;

	// Token: 0x040000C0 RID: 192
	public RectTransform HatPanel;

	// Token: 0x040000C1 RID: 193
	public RectTransform AccessoryPanel;

	// Token: 0x040000C2 RID: 194
	public RectTransform BackpackPanel;

	// Token: 0x040000C3 RID: 195
	public RectTransform FaceEditPanel;

	// Token: 0x040000C4 RID: 196
	public RectTransform BaseEditPanel;

	// Token: 0x040000C5 RID: 197
	public RectTransform SkinColorPanel;

	// Token: 0x040000C6 RID: 198
	public RectTransform EyeColorPanel;

	// Token: 0x040000C7 RID: 199
	public RectTransform HairColorPanel;

	// Token: 0x040000C8 RID: 200
	public RectTransform UnderpantsColorPanel;

	// Token: 0x040000C9 RID: 201
	public RectTransform EmotionsPanel;

	// Token: 0x040000CA RID: 202
	public RectTransform SavesPanel;

	// Token: 0x040000CB RID: 203
	public RectTransform SavesPanelList;

	// Token: 0x040000CC RID: 204
	public RectTransform SavesPrefab;

	// Token: 0x040000CD RID: 205
	public List<RectTransform> SavesList = new List<RectTransform>();

	// Token: 0x040000CE RID: 206
	public Image SkinColorButtonColor;

	// Token: 0x040000CF RID: 207
	public Image EyeColorButtonColor;

	// Token: 0x040000D0 RID: 208
	public Image HairColorButtonColor;

	// Token: 0x040000D1 RID: 209
	public Image UnderpantsColorButtonColor;

	// Token: 0x040000D2 RID: 210
	public Vector3[] CameraPositionForPanels;

	// Token: 0x040000D3 RID: 211
	public Vector3[] CameraEulerForPanels;

	// Token: 0x040000D4 RID: 212
	private int currentPanelIndex;

	// Token: 0x040000D5 RID: 213
	public Camera Camera;

	// Token: 0x040000D6 RID: 214
	public RectTransform femaleUI;

	// Token: 0x040000D7 RID: 215
	public RectTransform maleUI;

	// Token: 0x040000D8 RID: 216
	private int lodIndex;

	// Token: 0x040000D9 RID: 217
	private bool walk_active;

	// Token: 0x040000DA RID: 218
	private bool canvasVisible = true;
}
