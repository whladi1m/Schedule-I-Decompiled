using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x020001F2 RID: 498
	[DisallowMultipleComponent]
	[AddComponentMenu("Advanced People Pack/Character Customizable", -1)]
	public class CharacterCustomization : MonoBehaviour
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x00030478 File Offset: 0x0002E678
		public CharacterSettings Settings
		{
			get
			{
				return this._settings;
			}
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00030480 File Offset: 0x0002E680
		private void Awake()
		{
			this._transform = base.transform;
			this._lodGroup = base.GetComponent<LODGroup>();
			this.UpdateSkinnedMeshesOffscreenBounds();
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x000304A0 File Offset: 0x0002E6A0
		private void Update()
		{
			this.AnimationTick();
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x000304A8 File Offset: 0x0002E6A8
		private void LateUpdate()
		{
			if (this.feetOffset != 0f && this.applyFeetOffset)
			{
				this.SetFeetOffset(new Vector3(0f, this.feetOffset, 0f));
			}
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x000304DC File Offset: 0x0002E6DC
		public void AnimationTick()
		{
			if (this.currentBlendshapeAnimation != null)
			{
				this.currentBlendshapeAnimation.timer += Time.deltaTime * this.currentBlendshapeAnimation.preset.AnimationPlayDuration;
				for (int i = 0; i < this.currentBlendshapeAnimation.preset.blendshapes.Count; i++)
				{
					if (this.currentBlendshapeAnimation.preset.UseGlobalBlendCurve)
					{
						this.SetBlendshapeValue(this.currentBlendshapeAnimation.preset.blendshapes[i].BlendType, this.currentBlendshapeAnimation.preset.blendshapes[i].BlendValue * this.currentBlendshapeAnimation.preset.weightPower * this.currentBlendshapeAnimation.preset.GlobalBlendAnimationCurve.Evaluate(this.currentBlendshapeAnimation.timer), null, null);
					}
					else
					{
						this.SetBlendshapeValue(this.currentBlendshapeAnimation.preset.blendshapes[i].BlendType, this.currentBlendshapeAnimation.preset.blendshapes[i].BlendValue * this.currentBlendshapeAnimation.preset.weightPower * this.currentBlendshapeAnimation.preset.blendshapes[i].BlendAnimationCurve.Evaluate(this.currentBlendshapeAnimation.timer), null, null);
					}
				}
				if (this.currentBlendshapeAnimation.timer >= 1f)
				{
					this.currentBlendshapeAnimation = null;
				}
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003065C File Offset: 0x0002E85C
		public void SwitchCharacterSettings(int settingsIndex)
		{
			if (this.Settings.settingsSelectors.Count - 1 >= settingsIndex)
			{
				CharacterSettingsSelector characterSettingsSelector = this.Settings.settingsSelectors[settingsIndex];
				this.InitializeMeshes(characterSettingsSelector.settings, true);
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x000306A0 File Offset: 0x0002E8A0
		public void SwitchCharacterSettings(string selectorName)
		{
			for (int i = 0; i < this.Settings.settingsSelectors.Count; i++)
			{
				if (this.Settings.settingsSelectors[i].name == selectorName)
				{
					this.SwitchCharacterSettings(i);
					return;
				}
			}
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x000306F0 File Offset: 0x0002E8F0
		public void InitializeMeshes(CharacterSettings newSettings = null, bool resetAll = true)
		{
			this._transform = base.transform;
			if (this.selectedsettings == null && newSettings == null)
			{
				Debug.LogError("_settings = null, Unable to initialize character");
			}
			else
			{
				this._settings = ((newSettings != null) ? newSettings : this.selectedsettings);
				if (newSettings != null)
				{
					this.selectedsettings = newSettings;
				}
			}
			this.UnlockPrefab();
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this._transform.childCount; i++)
			{
				list.Add(this._transform.GetChild(i).gameObject);
			}
			UnityEngine.Object[] objects;
			if (list.Count > 0)
			{
				objects = list.ToArray();
				this.DestroyObjects(objects);
			}
			this.characterBlendshapeDatas.Clear();
			foreach (CharacterBlendshapeData characterBlendshapeData in this._settings.characterBlendshapeDatas)
			{
				this.characterBlendshapeDatas.Add(new CharacterBlendshapeData(characterBlendshapeData.blendshapeName, characterBlendshapeData.type, characterBlendshapeData.group, 0f));
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._settings.OriginalMesh, this._transform);
			gameObject.name = "Character";
			this.ProbesAnchorOverride = new GameObject("Probes Anchor").GetComponent<Transform>();
			this.ProbesAnchorOverride.parent = this._transform;
			this.ProbesAnchorOverride.localPosition = Vector3.up * 1.5f;
			this.ProbesAnchorOverride.localRotation = Quaternion.identity;
			List<Transform> list2 = new List<Transform>();
			new List<Transform>();
			for (int j = 0; j < gameObject.transform.childCount; j++)
			{
				Transform child = gameObject.transform.GetChild(j);
				list2.Add(child);
			}
			this.characterParts.Clear();
			this.clothesAnchors.Clear();
			foreach (Transform transform in list2)
			{
				transform.SetParent(this._transform);
				SkinnedMeshRenderer component = transform.GetComponent<SkinnedMeshRenderer>();
				string[] array = transform.name.Split('_', StringSplitOptions.None);
				string objType = array[0];
				string input = (array.Length == 3) ? array[2] : "-";
				int num = -1;
				Match match = Regex.Match(input, "(\\d+)");
				if (match.Success)
				{
					num = int.Parse(match.Groups[1].Value);
				}
				if ((num != -1 && num < this.MinLODLevels) || num > this.MaxLODLevels)
				{
					objects = new GameObject[]
					{
						transform.gameObject
					};
					this.DestroyObjects(objects);
				}
				else
				{
					if (component != null)
					{
						component.updateWhenOffscreen = true;
						component.probeAnchor = this.ProbesAnchorOverride;
					}
					if (objType != "ACCESSORY" && objType != "HAT" && objType != "PANTS" && objType != "SHIRT" && objType != "SHOES" && objType != "ITEM1" && objType.ToLowerInvariant() != "hips")
					{
						if (!(component == null))
						{
							component.sharedMaterials = new Material[0];
							if (objType == "COMBINED")
							{
								component.gameObject.SetActive(false);
							}
							else
							{
								component.sharedMaterials = new Material[]
								{
									this._settings.bodyMaterial
								};
							}
							CharacterPart characterPart = this.characterParts.Find((CharacterPart f) => f.name == objType);
							if (characterPart == null)
							{
								CharacterPart characterPart2 = new CharacterPart();
								characterPart2.name = objType;
								characterPart2.skinnedMesh.Add(component);
								this.characterParts.Add(characterPart2);
							}
							else
							{
								characterPart.skinnedMesh.Add(component);
							}
						}
					}
					else if (objType.ToLowerInvariant() == "hips")
					{
						transform.SetSiblingIndex(0);
						this.originHip = transform;
						Transform[] componentsInChildren = this.originHip.GetComponentsInChildren<Transform>();
						this.headHip = componentsInChildren.First((Transform f) => f.name.ToLowerInvariant() == "head");
					}
					else if ((objType == "HAT" || objType == "SHIRT" || objType == "PANTS" || objType == "SHOES" || objType == "ACCESSORY" || objType == "ITEM1") && !(component == null))
					{
						component.gameObject.SetActive(false);
						ClothesAnchor clothesAnchor = this.clothesAnchors.Find((ClothesAnchor f) => f.partType.ToString().ToLowerInvariant() == objType.ToLowerInvariant());
						if (clothesAnchor == null)
						{
							ClothesAnchor clothesAnchor2 = new ClothesAnchor();
							clothesAnchor2.partType = (CharacterElementType)Enum.Parse(typeof(CharacterElementType), objType.ToLowerInvariant(), true);
							clothesAnchor2.skinnedMesh.Add(component);
							this.clothesAnchors.Add(clothesAnchor2);
						}
						else
						{
							clothesAnchor.skinnedMesh.Add(component);
						}
					}
				}
			}
			objects = new GameObject[]
			{
				gameObject
			};
			this.DestroyObjects(objects);
			this._lodGroup = base.GetComponent<LODGroup>();
			this.animator = base.GetComponent<Animator>();
			if (this.animator == null)
			{
				this.animator = base.gameObject.AddComponent<Animator>();
			}
			if (this._lodGroup != null && this.MinLODLevels == this.MaxLODLevels)
			{
				UnityEngine.Object.DestroyImmediate(this._lodGroup);
			}
			else if (this._lodGroup == null)
			{
				this._lodGroup = base.gameObject.AddComponent<LODGroup>();
			}
			this.animator.avatar = this._settings.Avatar;
			this.animator.runtimeAnimatorController = this._settings.Animator;
			this.animator.Rebind();
			if (resetAll)
			{
				this.ResetAll(false);
			}
			this.RecalculateLOD();
			if (!this._settings.bodyMaterial.HasProperty("_SkinColor"))
			{
				this.notAPP2Shader = true;
			}
			else
			{
				this.Skin = this._settings.bodyMaterial.GetColor("_SkinColor");
				this.Eye = this._settings.bodyMaterial.GetColor("_EyeColor");
				this.Hair = this._settings.bodyMaterial.GetColor("_HairColor");
				this.Underpants = this._settings.bodyMaterial.GetColor("_UnderpantsColor");
				this.OralCavity = this._settings.bodyMaterial.GetColor("_OralCavityColor");
				this.Teeth = this._settings.bodyMaterial.GetColor("_TeethColor");
			}
			this.LockPrefab("");
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00030E84 File Offset: 0x0002F084
		public void UpdateSkinnedMeshesOffscreenBounds()
		{
			using (List<SkinnedMeshRenderer>.Enumerator enumerator = this.GetAllMeshes().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharacterCustomization.<>c__DisplayClass43_0 CS$<>8__locals1 = new CharacterCustomization.<>c__DisplayClass43_0();
					CS$<>8__locals1.<>4__this = this;
					CS$<>8__locals1.mesh = enumerator.Current;
					CS$<>8__locals1.mesh.updateWhenOffscreen = this.UpdateWhenOffscreenMeshes;
					if (!this.UpdateWhenOffscreenMeshes)
					{
						if (this.GetCharacterInstanceStatus() != CharacterInstanceStatus.PrefabEditingInProjectView && this.GetCharacterInstanceStatus() != CharacterInstanceStatus.PrefabStageSceneOpened)
						{
							base.StartCoroutine(CS$<>8__locals1.<UpdateSkinnedMeshesOffscreenBounds>g__UpdateBounds|0());
						}
						else
						{
							CS$<>8__locals1.<UpdateSkinnedMeshesOffscreenBounds>g__UpdateBounds|0();
						}
					}
				}
			}
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00030F24 File Offset: 0x0002F124
		public List<CharacterSettingsSelector> GetCharacterSettingsSelectors()
		{
			return this.Settings.settingsSelectors;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00030F34 File Offset: 0x0002F134
		public void ResetBodyMaterial()
		{
			foreach (CharacterPart characterPart in this.characterParts)
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in characterPart.skinnedMesh)
				{
					skinnedMeshRenderer.sharedMaterial = this._settings.bodyMaterial;
				}
			}
			CharacterPart characterPart2 = this.GetCharacterPart("HAIR");
			CharacterElementsPreset elementsPreset = this.GetElementsPreset(CharacterElementType.Hair, this.characterSelectedElements.GetSelectedIndex(CharacterElementType.Hair));
			if (elementsPreset != null)
			{
				List<Material> list = elementsPreset.mats.ToList<Material>();
				if (elementsPreset.mats != null && elementsPreset.mats.Length != 0)
				{
					for (int i = 0; i < characterPart2.skinnedMesh.Count; i++)
					{
						characterPart2.skinnedMesh[i].sharedMaterials = list.ToArray();
						for (int j = 0; j < list.Count; j++)
						{
							if (list[j].name == this._settings.bodyMaterial.name)
							{
								characterPart2.skinnedMesh[i].SetPropertyBlock(this.bodyPropertyBlock, j);
							}
						}
					}
				}
			}
			CharacterPart characterPart3 = this.GetCharacterPart("BEARD");
			CharacterElementsPreset elementsPreset2 = this.GetElementsPreset(CharacterElementType.Beard, this.characterSelectedElements.GetSelectedIndex(CharacterElementType.Beard));
			if (elementsPreset2 != null)
			{
				List<Material> list2 = elementsPreset2.mats.ToList<Material>();
				if (elementsPreset2.mats != null && elementsPreset2.mats.Length != 0)
				{
					for (int k = 0; k < characterPart3.skinnedMesh.Count; k++)
					{
						characterPart3.skinnedMesh[k].sharedMaterials = list2.ToArray();
						for (int l = 0; l < list2.Count; l++)
						{
							if (list2[l].name == this._settings.bodyMaterial.name)
							{
								characterPart3.skinnedMesh[k].SetPropertyBlock(this.bodyPropertyBlock, l);
							}
						}
					}
				}
			}
			ClothesAnchor clothesAnchor = this.GetClothesAnchor(CharacterElementType.Shoes);
			for (int m = 0; m < clothesAnchor.skinnedMesh.Count; m++)
			{
				List<Material> list3 = clothesAnchor.skinnedMesh[m].sharedMaterials.ToList<Material>();
				for (int n = 0; n < list3.Count; n++)
				{
					if (list3[n].name == this._settings.bodyMaterial.name)
					{
						list3[n] = this._settings.bodyMaterial;
						clothesAnchor.skinnedMesh[m].sharedMaterials = list3.ToArray();
					}
				}
			}
			if (this.CurrentCombinerState == CombinerState.Combined || this.CurrentCombinerState == CombinerState.UsedPreBuitMeshes)
			{
				List<SkinnedMeshRenderer> skinnedMesh = this.GetCharacterPart("COMBINED").skinnedMesh;
				for (int num = 0; num < skinnedMesh.Count; num++)
				{
					if (skinnedMesh[num] != null)
					{
						List<Material> list4 = skinnedMesh[num].sharedMaterials.ToList<Material>();
						for (int num2 = 0; num2 < list4.Count; num2++)
						{
							if (list4[num2].name == this._settings.bodyMaterial.name)
							{
								list4[num2] = this._settings.bodyMaterial;
								skinnedMesh[num].sharedMaterials = list4.ToArray();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x000312F0 File Offset: 0x0002F4F0
		public void InitColors()
		{
			if (this.Settings == null)
			{
				return;
			}
			if (this.bodyPropertyBlock == null)
			{
				this.bodyPropertyBlock = new MaterialPropertyBlock();
			}
			this.SetBodyColor(BodyColorPart.Skin, this.Skin);
			this.SetBodyColor(BodyColorPart.Eye, this.Eye);
			this.SetBodyColor(BodyColorPart.Hair, this.Hair);
			this.SetBodyColor(BodyColorPart.Underpants, this.Underpants);
			this.SetBodyColor(BodyColorPart.Teeth, this.Teeth);
			this.SetBodyColor(BodyColorPart.OralCavity, this.OralCavity);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00031370 File Offset: 0x0002F570
		public void ResetBodyColors()
		{
			if (this.notAPP2Shader)
			{
				return;
			}
			if (this._settings.bodyMaterial.HasProperty("_SkinColor"))
			{
				this.SetBodyColor(BodyColorPart.Skin, this._settings.bodyMaterial.GetColor("_SkinColor"));
			}
			if (this._settings.bodyMaterial.HasProperty("_EyeColor"))
			{
				this.SetBodyColor(BodyColorPart.Eye, this._settings.bodyMaterial.GetColor("_EyeColor"));
			}
			if (this._settings.bodyMaterial.HasProperty("_HairColor"))
			{
				this.SetBodyColor(BodyColorPart.Hair, this._settings.bodyMaterial.GetColor("_HairColor"));
			}
			if (this._settings.bodyMaterial.HasProperty("_UnderpantsColor"))
			{
				this.SetBodyColor(BodyColorPart.Underpants, this._settings.bodyMaterial.GetColor("_UnderpantsColor"));
			}
			if (this._settings.bodyMaterial.HasProperty("_OralCavityColor"))
			{
				this.SetBodyColor(BodyColorPart.OralCavity, this._settings.bodyMaterial.GetColor("_OralCavityColor"));
			}
			if (this._settings.bodyMaterial.HasProperty("_TeethColor"))
			{
				this.SetBodyColor(BodyColorPart.Teeth, this._settings.bodyMaterial.GetColor("_TeethColor"));
			}
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x000314B8 File Offset: 0x0002F6B8
		public void SetBlendshapeValue(CharacterBlendShapeType type, float weight, string[] forPart = null, CharacterElementType[] forClothPart = null)
		{
			try
			{
				string text = type.ToString();
				if (this.CurrentCombinerState != CombinerState.Combined && this.CurrentCombinerState != CombinerState.UsedPreBuitMeshes)
				{
					foreach (CharacterPart characterPart in this.characterParts)
					{
						if (forPart == null || forPart.Contains(characterPart.name))
						{
							foreach (SkinnedMeshRenderer skinnedMeshRenderer in characterPart.skinnedMesh)
							{
								if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
								{
									for (int i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
									{
										if (text == skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i))
										{
											int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(text);
											if (blendShapeIndex != -1 && !this.Settings.DisableBlendshapeModifier)
											{
												skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, weight);
											}
										}
									}
								}
							}
						}
					}
					using (List<ClothesAnchor>.Enumerator enumerator3 = this.clothesAnchors.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							ClothesAnchor clothesAnchor = enumerator3.Current;
							if (forClothPart == null || forClothPart.Contains(clothesAnchor.partType))
							{
								foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in clothesAnchor.skinnedMesh)
								{
									if (skinnedMeshRenderer2 != null && skinnedMeshRenderer2.sharedMesh != null)
									{
										for (int j = 0; j < skinnedMeshRenderer2.sharedMesh.blendShapeCount; j++)
										{
											if (text == skinnedMeshRenderer2.sharedMesh.GetBlendShapeName(j))
											{
												int blendShapeIndex2 = skinnedMeshRenderer2.sharedMesh.GetBlendShapeIndex(text);
												if (blendShapeIndex2 != -1 && !this.Settings.DisableBlendshapeModifier)
												{
													skinnedMeshRenderer2.SetBlendShapeWeight(blendShapeIndex2, weight);
												}
											}
										}
									}
								}
							}
						}
						goto IL_2AD;
					}
				}
				foreach (SkinnedMeshRenderer skinnedMeshRenderer3 in this.GetCharacterPart("COMBINED").skinnedMesh)
				{
					if (skinnedMeshRenderer3.sharedMesh != null)
					{
						for (int k = 0; k < skinnedMeshRenderer3.sharedMesh.blendShapeCount; k++)
						{
							if (!this.Settings.DisableBlendshapeModifier && text == skinnedMeshRenderer3.sharedMesh.GetBlendShapeName(k))
							{
								skinnedMeshRenderer3.SetBlendShapeWeight(skinnedMeshRenderer3.sharedMesh.GetBlendShapeIndex(text), weight);
							}
						}
					}
				}
				IL_2AD:
				this.GetBlendshapeData(type).value = weight;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0003181C File Offset: 0x0002FA1C
		public void ForceLOD(int lodLevel)
		{
			if (lodLevel > this.MaxLODLevels - this.MinLODLevels)
			{
				return;
			}
			if (lodLevel != 0)
			{
				this._lodGroup.ForceLOD(lodLevel);
				return;
			}
			this._lodGroup.ForceLOD(-1);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0003184C File Offset: 0x0002FA4C
		public void SetElementByIndex(CharacterElementType type, int index)
		{
			if (this.Settings == null)
			{
				Debug.LogError("settings = null");
				return;
			}
			if (type == CharacterElementType.Hair)
			{
				this.SetHairByIndex(index);
				this.RecalculateShapes();
				return;
			}
			if (type == CharacterElementType.Beard)
			{
				this.SetBeardByIndex(index);
				this.RecalculateShapes();
				return;
			}
			ClothesAnchor clothesAnchor = this.GetClothesAnchor(type);
			CharacterElementsPreset elementsPreset = this.GetElementsPreset(type, this.characterSelectedElements.GetSelectedIndex(type));
			CharacterElementsPreset elementsPreset2 = this.GetElementsPreset(type, index);
			float y = 0f;
			if (elementsPreset != null && (elementsPreset2 != null || index == -1))
			{
				this.UnHideParts(elementsPreset.hideParts, type);
			}
			if (elementsPreset2 != null)
			{
				if (elementsPreset2.mesh.Length == 0)
				{
					Debug.LogErrorFormat(string.Format("Not found meshes for <{0}> element", elementsPreset2.name), Array.Empty<object>());
					return;
				}
				if (type == CharacterElementType.Shirt)
				{
					this.GetBlendshapeData(CharacterBlendShapeType.BackpackOffset).value = 100f;
					if (this.characterSelectedElements.GetSelectedIndex(CharacterElementType.Item1) != -1)
					{
						this.SetBlendshapeValue(CharacterBlendShapeType.BackpackOffset, 100f, null, null);
					}
				}
				y = elementsPreset2.yOffset;
				for (int i = 0; i < this.MaxLODLevels - this.MinLODLevels + 1; i++)
				{
					int num = i + this.MinLODLevels;
					if (!clothesAnchor.skinnedMesh[i].gameObject.activeSelf && !this.IsBaked())
					{
						clothesAnchor.skinnedMesh[i].gameObject.SetActive(true);
					}
					clothesAnchor.skinnedMesh[i].sharedMesh = elementsPreset2.mesh[num];
					if (elementsPreset2.mats != null && elementsPreset2.mats.Length != 0)
					{
						List<Material> list = elementsPreset2.mats.ToList<Material>();
						clothesAnchor.skinnedMesh[i].sharedMaterials = list.ToArray();
						for (int j = 0; j < list.Count; j++)
						{
							if (list[j].name == this._settings.bodyMaterial.name)
							{
								clothesAnchor.skinnedMesh[i].SetPropertyBlock(this.bodyPropertyBlock, j);
							}
						}
					}
					for (int k = 0; k < clothesAnchor.skinnedMesh[i].sharedMesh.blendShapeCount; k++)
					{
						if (clothesAnchor.skinnedMesh[i] != null && clothesAnchor.skinnedMesh[i].sharedMesh != null)
						{
							string blendShapeName = clothesAnchor.skinnedMesh[i].sharedMesh.GetBlendShapeName(k);
							CharacterBlendshapeData blendshapeData = this.GetBlendshapeData(blendShapeName);
							if (blendshapeData != null && !this.Settings.DisableBlendshapeModifier)
							{
								clothesAnchor.skinnedMesh[i].SetBlendShapeWeight(k, blendshapeData.value);
							}
						}
					}
				}
				this.HideParts(elementsPreset2.hideParts);
			}
			else
			{
				if (type == CharacterElementType.Shirt)
				{
					this.GetBlendshapeData(CharacterBlendShapeType.BackpackOffset).value = 0f;
					if (this.characterSelectedElements.GetSelectedIndex(CharacterElementType.Item1) != -1)
					{
						this.SetBlendshapeValue(CharacterBlendShapeType.BackpackOffset, 0f, null, null);
					}
				}
				if (index != -1)
				{
					Debug.LogError(string.Format("Element <{0}> with index {1} not found. Please check Character Presets arrays.", type.ToString(), index));
					return;
				}
				if (clothesAnchor != null && clothesAnchor.skinnedMesh != null)
				{
					foreach (SkinnedMeshRenderer skinnedMeshRenderer in clothesAnchor.skinnedMesh)
					{
						if (skinnedMeshRenderer != null)
						{
							skinnedMeshRenderer.sharedMesh = null;
							skinnedMeshRenderer.gameObject.SetActive(false);
						}
					}
				}
			}
			if (type == CharacterElementType.Shoes)
			{
				this.SetFeetOffset(new Vector3(0f, y, 0f));
				this.feetOffset = y;
			}
			this.characterSelectedElements.SetSelectedIndex(type, index);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00031C00 File Offset: 0x0002FE00
		public void ClearElement(CharacterElementType type)
		{
			if (type == CharacterElementType.Hair)
			{
				this.SetHairByIndex(-1);
				return;
			}
			if (type == CharacterElementType.Beard)
			{
				this.SetBeardByIndex(-1);
				return;
			}
			this.SetElementByIndex(type, -1);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00031C24 File Offset: 0x0002FE24
		public void SetHeight(float height)
		{
			this.heightValue = height;
			if (this.originHip != null)
			{
				this.originHip.localScale = new Vector3(1f + height / 1.4f, 1f + height, 1f + height);
			}
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00031C71 File Offset: 0x0002FE71
		public void SetHeadSize(float size)
		{
			this.headSizeValue = size;
			if (this.headHip != null)
			{
				this.headHip.localScale = Vector3.one + Vector3.one * size;
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00031CA8 File Offset: 0x0002FEA8
		public void SetFeetOffset(Vector3 offset)
		{
			this.originHip.localPosition = offset;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00031CB8 File Offset: 0x0002FEB8
		private void SetHairByIndex(int index)
		{
			CharacterPart characterPart = this.GetCharacterPart("Hair");
			if (characterPart == null || characterPart.skinnedMesh.Count <= 0)
			{
				return;
			}
			if (index != -1)
			{
				CharacterElementsPreset characterElementsPreset = this._settings.hairPresets.ElementAtOrDefault(index);
				if (characterElementsPreset == null)
				{
					Debug.LogError(string.Format("Hair with index {0} not found", index));
					return;
				}
				for (int i = 0; i < this.MaxLODLevels - this.MinLODLevels + 1; i++)
				{
					int num = i + this.MinLODLevels;
					if (characterPart.skinnedMesh.Count > 0 && characterPart.skinnedMesh.Count - 1 >= i && characterPart.skinnedMesh[i] != null)
					{
						if (!characterPart.skinnedMesh[i].gameObject.activeSelf)
						{
							characterPart.skinnedMesh[i].gameObject.SetActive(true);
						}
						characterPart.skinnedMesh[i].sharedMesh = this._settings.hairPresets[index].mesh[num];
					}
					if (characterElementsPreset.mats != null && characterElementsPreset.mats.Length != 0)
					{
						List<Material> list = characterElementsPreset.mats.ToList<Material>();
						characterPart.skinnedMesh[i].sharedMaterials = list.ToArray();
						for (int j = 0; j < list.Count; j++)
						{
							if (list[j].name == this._settings.bodyMaterial.name)
							{
								characterPart.skinnedMesh[i].SetPropertyBlock(this.bodyPropertyBlock, j);
							}
						}
					}
				}
			}
			else
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in characterPart.skinnedMesh)
				{
					skinnedMeshRenderer.sharedMesh = null;
					skinnedMeshRenderer.gameObject.SetActive(false);
				}
			}
			this.characterSelectedElements.SetSelectedIndex(CharacterElementType.Hair, index);
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00031EB8 File Offset: 0x000300B8
		private void SetBeardByIndex(int index)
		{
			CharacterPart characterPart = this.GetCharacterPart("Beard");
			if (characterPart == null || characterPart.skinnedMesh.Count <= 0)
			{
				return;
			}
			if (index != -1)
			{
				CharacterElementsPreset characterElementsPreset = this._settings.beardPresets.ElementAtOrDefault(index);
				if (characterElementsPreset == null)
				{
					Debug.LogError(string.Format("Beard with index {0} not found", index));
					return;
				}
				for (int i = 0; i < this.MaxLODLevels - this.MinLODLevels + 1; i++)
				{
					int num = i + this.MinLODLevels;
					if (!characterPart.skinnedMesh[i].gameObject.activeSelf)
					{
						characterPart.skinnedMesh[i].gameObject.SetActive(true);
					}
					characterPart.skinnedMesh[i].sharedMesh = this._settings.beardPresets[index].mesh[num];
					if (characterElementsPreset.mats != null && characterElementsPreset.mats.Length != 0)
					{
						List<Material> list = characterElementsPreset.mats.ToList<Material>();
						characterPart.skinnedMesh[i].sharedMaterials = list.ToArray();
						for (int j = 0; j < list.Count; j++)
						{
							if (list[j].name == this._settings.bodyMaterial.name)
							{
								characterPart.skinnedMesh[i].SetPropertyBlock(this.bodyPropertyBlock, j);
							}
						}
					}
				}
			}
			else
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in characterPart.skinnedMesh)
				{
					skinnedMeshRenderer.sharedMesh = null;
					skinnedMeshRenderer.gameObject.SetActive(false);
				}
			}
			this.characterSelectedElements.SetSelectedIndex(CharacterElementType.Beard, index);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00032084 File Offset: 0x00030284
		public ClothesAnchor GetClothesAnchor(CharacterElementType type)
		{
			foreach (ClothesAnchor clothesAnchor in this.clothesAnchors)
			{
				if (clothesAnchor.partType == type)
				{
					return clothesAnchor;
				}
			}
			return null;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x000320E0 File Offset: 0x000302E0
		public CharacterPart GetCharacterPart(string name)
		{
			foreach (CharacterPart characterPart in this.characterParts)
			{
				if (characterPart.name.ToLowerInvariant() == name.ToLowerInvariant())
				{
					return characterPart;
				}
			}
			return null;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0003214C File Offset: 0x0003034C
		public List<SkinnedMeshRenderer> GetAllMeshesByLod(int lod)
		{
			List<SkinnedMeshRenderer> list = new List<SkinnedMeshRenderer>();
			foreach (CharacterPart characterPart in this.characterParts)
			{
				if (characterPart.skinnedMesh.Count >= lod)
				{
					list.Add(characterPart.skinnedMesh[lod]);
				}
			}
			foreach (ClothesAnchor clothesAnchor in this.clothesAnchors)
			{
				if (clothesAnchor.skinnedMesh.Count >= lod)
				{
					list.Add(clothesAnchor.skinnedMesh[lod]);
				}
			}
			return list;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00032220 File Offset: 0x00030420
		public List<SkinnedMeshRenderer> GetAllMeshes()
		{
			List<SkinnedMeshRenderer> list = new List<SkinnedMeshRenderer>();
			foreach (CharacterPart characterPart in this.characterParts)
			{
				list.AddRange(characterPart.skinnedMesh);
			}
			foreach (ClothesAnchor clothesAnchor in this.clothesAnchors)
			{
				list.AddRange(clothesAnchor.skinnedMesh);
			}
			return list;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x000322C8 File Offset: 0x000304C8
		public List<SkinnedMeshRenderer> GetAllMeshes(bool onlyBodyMeshes = false, string[] excludeNames = null)
		{
			List<SkinnedMeshRenderer> list = new List<SkinnedMeshRenderer>();
			foreach (CharacterPart characterPart in this.characterParts)
			{
				if (excludeNames == null || !excludeNames.Contains(characterPart.name))
				{
					list.AddRange(characterPart.skinnedMesh);
				}
			}
			if (onlyBodyMeshes)
			{
				foreach (ClothesAnchor clothesAnchor in this.clothesAnchors)
				{
					list.AddRange(clothesAnchor.skinnedMesh);
				}
			}
			return list;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00032384 File Offset: 0x00030584
		public void HideParts(string[] parts)
		{
			foreach (string text in parts)
			{
				foreach (CharacterPart characterPart in this.characterParts)
				{
					if (characterPart.name.ToLowerInvariant() == text.ToLowerInvariant())
					{
						foreach (SkinnedMeshRenderer skinnedMeshRenderer in characterPart.skinnedMesh)
						{
							skinnedMeshRenderer.enabled = false;
						}
					}
				}
			}
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00032448 File Offset: 0x00030648
		public void UnHideParts(string[] parts, CharacterElementType hidePartsForElement)
		{
			foreach (string text in parts)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				int selectedIndex = this.characterSelectedElements.GetSelectedIndex(CharacterElementType.Shirt);
				int selectedIndex2 = this.characterSelectedElements.GetSelectedIndex(CharacterElementType.Pants);
				int selectedIndex3 = this.characterSelectedElements.GetSelectedIndex(CharacterElementType.Shoes);
				if (selectedIndex != -1 && hidePartsForElement != CharacterElementType.Shirt)
				{
					string[] hideParts = this.GetElementsPreset(CharacterElementType.Shirt, selectedIndex).hideParts;
					for (int j = 0; j < hideParts.Length; j++)
					{
						if (hideParts[j] == text)
						{
							flag = true;
							break;
						}
					}
				}
				if (selectedIndex2 != -1 && hidePartsForElement != CharacterElementType.Pants)
				{
					string[] hideParts = this.GetElementsPreset(CharacterElementType.Pants, selectedIndex2).hideParts;
					for (int j = 0; j < hideParts.Length; j++)
					{
						if (hideParts[j] == text)
						{
							flag2 = true;
							break;
						}
					}
				}
				if (selectedIndex3 != -1 && hidePartsForElement != CharacterElementType.Shoes)
				{
					string[] hideParts = this.GetElementsPreset(CharacterElementType.Shoes, selectedIndex3).hideParts;
					for (int j = 0; j < hideParts.Length; j++)
					{
						if (hideParts[j] == text)
						{
							flag3 = true;
							break;
						}
					}
				}
				if (!flag && !flag2 && !flag3)
				{
					foreach (CharacterPart characterPart in this.characterParts)
					{
						if (characterPart.name.ToLowerInvariant() == text.ToLowerInvariant())
						{
							foreach (SkinnedMeshRenderer skinnedMeshRenderer in characterPart.skinnedMesh)
							{
								skinnedMeshRenderer.enabled = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00032604 File Offset: 0x00030804
		public void SetBodyColor(BodyColorPart bodyColorPart, Color color)
		{
			if (this.notAPP2Shader)
			{
				return;
			}
			if (this.bodyPropertyBlock == null)
			{
				this.bodyPropertyBlock = new MaterialPropertyBlock();
			}
			switch (bodyColorPart)
			{
			case BodyColorPart.Skin:
				this.bodyPropertyBlock.SetColor("_SkinColor", color);
				break;
			case BodyColorPart.Eye:
				this.bodyPropertyBlock.SetColor("_EyeColor", color);
				break;
			case BodyColorPart.Hair:
				this.bodyPropertyBlock.SetColor("_HairColor", color);
				break;
			case BodyColorPart.Underpants:
				this.bodyPropertyBlock.SetColor("_UnderpantsColor", color);
				break;
			case BodyColorPart.OralCavity:
				this.bodyPropertyBlock.SetColor("_OralCavityColor", color);
				break;
			case BodyColorPart.Teeth:
				this.bodyPropertyBlock.SetColor("_TeethColor", color);
				break;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in (this.IsBaked() ? this.GetCharacterPart("COMBINED").skinnedMesh : this.GetAllMeshes(true, new string[]
			{
				"COMBINED"
			})))
			{
				for (int i = 0; i < skinnedMeshRenderer.sharedMaterials.Length; i++)
				{
					if (skinnedMeshRenderer.sharedMaterials[i] == this._settings.bodyMaterial)
					{
						skinnedMeshRenderer.SetPropertyBlock(this.bodyPropertyBlock, i);
					}
				}
			}
			MeshRenderer component = base.transform.Find("hips/Root/Spine1/Spine2/Chest/Neck/Head/nose").GetComponent<MeshRenderer>();
			for (int j = 0; j < component.sharedMaterials.Length; j++)
			{
				if (component.sharedMaterials[j] == this._settings.bodyMaterial)
				{
					component.SetPropertyBlock(this.bodyPropertyBlock, j);
				}
			}
			switch (bodyColorPart)
			{
			case BodyColorPart.Skin:
				this.Skin = color;
				return;
			case BodyColorPart.Eye:
				this.Eye = color;
				return;
			case BodyColorPart.Hair:
				this.Hair = color;
				return;
			case BodyColorPart.Underpants:
				this.Underpants = color;
				return;
			case BodyColorPart.OralCavity:
				this.OralCavity = color;
				return;
			case BodyColorPart.Teeth:
				this.Teeth = color;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00032804 File Offset: 0x00030A04
		public Color GetBodyColor(BodyColorPart bodyColorPart)
		{
			switch (bodyColorPart)
			{
			case BodyColorPart.Skin:
				return this.Skin;
			case BodyColorPart.Eye:
				return this.Eye;
			case BodyColorPart.Hair:
				return this.Hair;
			case BodyColorPart.Underpants:
				return this.Underpants;
			case BodyColorPart.OralCavity:
				return this.OralCavity;
			case BodyColorPart.Teeth:
				return this.Teeth;
			default:
				return Color.clear;
			}
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00032860 File Offset: 0x00030A60
		public void SetCharacterSetup(CharacterCustomizationSetup characterCustomizationSetup)
		{
			characterCustomizationSetup.ApplyToCharacter(this);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0003286C File Offset: 0x00030A6C
		public CharacterCustomizationSetup GetSetup()
		{
			CharacterCustomizationSetup characterCustomizationSetup = new CharacterCustomizationSetup();
			foreach (CharacterBlendshapeData characterBlendshapeData in this.characterBlendshapeDatas)
			{
				characterCustomizationSetup.blendshapes.Add(new CharacterBlendshapeData(characterBlendshapeData.blendshapeName, characterBlendshapeData.type, characterBlendshapeData.group, characterBlendshapeData.value));
			}
			characterCustomizationSetup.MinLod = this.MinLODLevels;
			characterCustomizationSetup.MaxLod = this.MaxLODLevels;
			characterCustomizationSetup.selectedElements.Accessory = this.characterSelectedElements.Accessory;
			characterCustomizationSetup.selectedElements.Beard = this.characterSelectedElements.Beard;
			characterCustomizationSetup.selectedElements.Hair = this.characterSelectedElements.Hair;
			characterCustomizationSetup.selectedElements.Hat = this.characterSelectedElements.Hat;
			characterCustomizationSetup.selectedElements.Item1 = this.characterSelectedElements.Item1;
			characterCustomizationSetup.selectedElements.Pants = this.characterSelectedElements.Pants;
			characterCustomizationSetup.selectedElements.Shirt = this.characterSelectedElements.Shirt;
			characterCustomizationSetup.selectedElements.Shoes = this.characterSelectedElements.Shoes;
			characterCustomizationSetup.Height = this.heightValue;
			characterCustomizationSetup.HeadSize = this.headSizeValue;
			characterCustomizationSetup.SkinColor = new float[]
			{
				this.Skin.r,
				this.Skin.g,
				this.Skin.b,
				this.Skin.a
			};
			characterCustomizationSetup.HairColor = new float[]
			{
				this.Hair.r,
				this.Hair.g,
				this.Hair.b,
				this.Hair.a
			};
			characterCustomizationSetup.UnderpantsColor = new float[]
			{
				this.Underpants.r,
				this.Underpants.g,
				this.Underpants.b,
				this.Underpants.a
			};
			characterCustomizationSetup.TeethColor = new float[]
			{
				this.Teeth.r,
				this.Teeth.g,
				this.Teeth.b,
				this.Teeth.a
			};
			characterCustomizationSetup.OralCavityColor = new float[]
			{
				this.OralCavity.r,
				this.OralCavity.g,
				this.OralCavity.b,
				this.OralCavity.a
			};
			characterCustomizationSetup.EyeColor = new float[]
			{
				this.Eye.r,
				this.Eye.g,
				this.Eye.b,
				this.Eye.a
			};
			characterCustomizationSetup.settingsName = this.Settings.name;
			return characterCustomizationSetup;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00032B70 File Offset: 0x00030D70
		public void ApplySavedCharacterData(SavedCharacterData data)
		{
			this.LoadCharacterFromFile(data.path);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00032B80 File Offset: 0x00030D80
		public void LoadCharacterFromFile(string path)
		{
			if (File.Exists(path))
			{
				string extension = Path.GetExtension(path);
				string text = File.ReadAllText(path);
				if (text.Length > 0)
				{
					CharacterCustomizationSetup.CharacterFileSaveFormat format;
					if (extension == ".json")
					{
						format = CharacterCustomizationSetup.CharacterFileSaveFormat.Json;
					}
					else if (extension == ".xml")
					{
						format = CharacterCustomizationSetup.CharacterFileSaveFormat.Xml;
					}
					else
					{
						if (!(extension == ".bin"))
						{
							Debug.LogError("File format not supported - " + extension);
							return;
						}
						format = CharacterCustomizationSetup.CharacterFileSaveFormat.Binary;
					}
					CharacterCustomizationSetup characterCustomizationSetup = CharacterCustomizationSetup.Deserialize(text, format);
					if (characterCustomizationSetup != null)
					{
						this.SetCharacterSetup(characterCustomizationSetup);
						Debug.Log(string.Format("Loaded {0} save", path));
					}
				}
			}
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00032C14 File Offset: 0x00030E14
		public List<SavedCharacterData> GetSavedCharacterDatas(string path = "")
		{
			List<SavedCharacterData> list = new List<SavedCharacterData>();
			string persistentDataPath = Application.persistentDataPath;
			string text = string.Format("{0}/{1}", persistentDataPath, "apack_characters_data");
			text = string.Format("{0}/{1}", text, this.Settings.name);
			if (!Directory.Exists(text))
			{
				return list;
			}
			foreach (string path2 in Directory.GetFiles(text, "appack25*"))
			{
				SavedCharacterData savedCharacterData = new SavedCharacterData();
				string[] array = Path.GetFileName(path2).Split('_', StringSplitOptions.None);
				string text2 = "";
				for (int j = 1; j < array.Length - 1; j++)
				{
					text2 = text2 + array[j] + ((j != array.Length - 2) ? "_" : "");
				}
				savedCharacterData.name = text2;
				savedCharacterData.path = path2;
				list.Add(savedCharacterData);
			}
			return list;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00032CF9 File Offset: 0x00030EF9
		public void ClearSavedData(SavedCharacterData data)
		{
			if (data != null && File.Exists(data.path))
			{
				File.Delete(data.path);
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00032D18 File Offset: 0x00030F18
		public void ClearSavedData()
		{
			List<SavedCharacterData> savedCharacterDatas = this.GetSavedCharacterDatas("");
			foreach (SavedCharacterData data in savedCharacterDatas)
			{
				this.ClearSavedData(data);
			}
			Debug.Log(string.Format("Removed {0} saves", savedCharacterDatas.Count));
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00032D8C File Offset: 0x00030F8C
		public void SaveCharacterToFile(CharacterCustomizationSetup.CharacterFileSaveFormat format, string path = "", string name = "")
		{
			string text = "json";
			switch (format)
			{
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Json:
				text = "json";
				break;
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Xml:
				text = "xml";
				break;
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Binary:
				text = "bin";
				break;
			}
			string text3;
			if (path.Length == 0)
			{
				string[] array = new string[]
				{
					"json",
					"xml",
					"bin"
				};
				string persistentDataPath = Application.persistentDataPath;
				string text2 = string.Format("{0}/{1}", persistentDataPath, "apack_characters_data");
				text2 = string.Format("{0}/{1}", text2, this.Settings.name);
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				string name2 = base.gameObject.name;
				text3 = string.Format("{0}/appack25_{1}_{2}.{3}", new object[]
				{
					text2,
					name2,
					DateTimeOffset.Now.ToUnixTimeSeconds(),
					text
				});
			}
			else
			{
				text3 = string.Format("{0}/{1}_{2}.{3}", new object[]
				{
					path,
					name,
					DateTimeOffset.Now.ToUnixTimeSeconds(),
					text
				});
			}
			string text4 = this.GetSetup().Serialize(format);
			if (text4.Length > 0)
			{
				File.WriteAllText(text3, text4, Encoding.UTF8);
				Debug.Log(string.Format("Character data saved to ({0})", text3));
			}
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00032EE0 File Offset: 0x000310E0
		public void RecalculateShapes()
		{
			foreach (CharacterBlendshapeData characterBlendshapeData in this.characterBlendshapeDatas)
			{
				this.SetBlendshapeValue(characterBlendshapeData.type, characterBlendshapeData.value, null, null);
			}
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00032F40 File Offset: 0x00031140
		public void EditorSavePreBuiltPrefab()
		{
			Debug.LogError("Pre Built Character can be created only in editor.");
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00032F4C File Offset: 0x0003114C
		public void BakeCharacter(bool usePreBuiltMeshes = false)
		{
			if (usePreBuiltMeshes)
			{
				if (this.combinedCharacter == null)
				{
					Debug.LogError("CombinedCharacter variable == null");
					return;
				}
				if (this.combinedCharacter != null && this.combinedCharacter.settings != this.Settings)
				{
					Debug.LogError("PreBuilt settings not equal current character settings");
					return;
				}
				if (this.combinedCharacter.preBuiltDatas == null || this.combinedCharacter.preBuiltDatas.Count == 0)
				{
					Debug.LogErrorFormat("CombinedCharacter object({0}) is not valid!", new object[]
					{
						this.combinedCharacter.name
					});
					return;
				}
				if (this.combinedCharacter.preBuiltDatas[0].meshes.Count < this.MaxLODLevels - this.MinLODLevels + 1)
				{
					Debug.LogErrorFormat("CombinedCharacter number of meshes({0}) is less than the number of LODs({1}) in the character\nTry combine character again or change LODs count", new object[]
					{
						this.combinedCharacter.preBuiltDatas[0].meshes.Count,
						this.MaxLODLevels - this.MinLODLevels + 1
					});
					return;
				}
			}
			foreach (CharacterPart characterPart in this.characterParts)
			{
				if (characterPart.name.ToLowerInvariant() == "combined")
				{
					characterPart.skinnedMesh.ForEach(delegate(SkinnedMeshRenderer m)
					{
						m.sharedMesh = null;
						m.gameObject.SetActive(false);
					});
				}
			}
			try
			{
				if (!usePreBuiltMeshes)
				{
					CharacterCustomizationCombiner.MakeCombinedMeshes(this, null, 0f, delegate(List<SkinnedMeshRenderer> meshes)
					{
						this.<BakeCharacter>g__MeshesProcess|77_0(false);
					});
				}
				else
				{
					this.<BakeCharacter>g__MeshesProcess|77_0(true);
					this.CurrentCombinerState = CombinerState.UsedPreBuitMeshes;
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				this.ClearBake();
			}
			this.RecalculateShapes();
			this.RecalculateLOD();
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00033134 File Offset: 0x00031334
		public void ClearBake()
		{
			foreach (CharacterPart characterPart in this.characterParts)
			{
				if (characterPart.name.ToLowerInvariant() == "combined")
				{
					characterPart.skinnedMesh.ForEach(delegate(SkinnedMeshRenderer m)
					{
						if (m.sharedMesh != null && this.CurrentCombinerState != CombinerState.UsedPreBuitMeshes)
						{
							m.sharedMesh.Clear();
							m.sharedMesh.ClearBlendShapes();
						}
						if (this.CurrentCombinerState == CombinerState.UsedPreBuitMeshes)
						{
							m.sharedMesh = null;
						}
						m.sharedMaterials = new Material[0];
						m.gameObject.SetActive(false);
					});
				}
				else
				{
					characterPart.skinnedMesh.ForEach(delegate(SkinnedMeshRenderer m)
					{
						if (m.sharedMesh != null)
						{
							m.gameObject.SetActive(true);
						}
					});
				}
			}
			foreach (ClothesAnchor clothesAnchor in this.clothesAnchors)
			{
				clothesAnchor.skinnedMesh.ForEach(delegate(SkinnedMeshRenderer m)
				{
					if (m.sharedMesh != null)
					{
						m.gameObject.SetActive(true);
					}
				});
			}
			this.CurrentCombinerState = CombinerState.NotCombined;
			this.RecalculateShapes();
			this.RecalculateLOD();
			Resources.UnloadUnusedAssets();
			this.ApplyPrefab();
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0003325C File Offset: 0x0003145C
		public void RecalculateLOD()
		{
			if (!this._lodGroup && this.MinLODLevels != this.MaxLODLevels)
			{
				this._lodGroup = base.GetComponent<LODGroup>();
			}
			else if (this.MinLODLevels == this.MaxLODLevels)
			{
				return;
			}
			float[][] array = new float[4][];
			array[0] = new float[]
			{
				0.5f,
				0.2f,
				0.05f,
				0f
			};
			int num = 1;
			float[] array2 = new float[4];
			array2[0] = 0.4f;
			array2[1] = 0.1f;
			array[num] = array2;
			int num2 = 2;
			float[] array3 = new float[4];
			array3[0] = 0.3f;
			array[num2] = array3;
			array[3] = new float[4];
			float[][] array4 = array;
			LOD[] array5 = new LOD[this.MaxLODLevels - this.MinLODLevels + 1];
			for (int i = 0; i < this.MaxLODLevels - this.MinLODLevels + 1; i++)
			{
				if (this.clothesAnchors.ElementAtOrDefault(i) != null)
				{
					List<SkinnedMeshRenderer> list = new List<SkinnedMeshRenderer>();
					foreach (CharacterPart characterPart in this.characterParts)
					{
						list.Add(characterPart.skinnedMesh[i]);
					}
					foreach (ClothesAnchor clothesAnchor in this.clothesAnchors)
					{
						list.Add(clothesAnchor.skinnedMesh[i]);
					}
					LOD[] array6 = array5;
					int num3 = i;
					float screenRelativeTransitionHeight = array4[3 - (this.MaxLODLevels - this.MinLODLevels)][i];
					Renderer[] renderers = list.ToArray();
					array6[num3] = new LOD(screenRelativeTransitionHeight, renderers);
				}
			}
			this._lodGroup.SetLODs(array5);
			this._lodGroup.RecalculateBounds();
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00033420 File Offset: 0x00031620
		public void SetLODRange(int minLod, int maxLod)
		{
			if (this.IsBaked())
			{
				return;
			}
			this.MinLODLevels = minLod;
			this.MaxLODLevels = maxLod;
			this.InitializeMeshes(null, true);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00033441 File Offset: 0x00031641
		public bool IsBaked()
		{
			return this.CurrentCombinerState == CombinerState.Combined || this.CurrentCombinerState == CombinerState.UsedPreBuitMeshes;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00033458 File Offset: 0x00031658
		public CharacterElementsPreset GetElementsPreset(CharacterElementType type, int index)
		{
			List<CharacterElementsPreset> elementsPresets = this.GetElementsPresets(type);
			if (elementsPresets.Count <= 0 || elementsPresets.Count - 1 < index || index == -1)
			{
				return null;
			}
			return elementsPresets[index];
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00033490 File Offset: 0x00031690
		public CharacterElementsPreset GetElementsPreset(CharacterElementType type, string name)
		{
			List<CharacterElementsPreset> elementsPresets = this.GetElementsPresets(type);
			if (elementsPresets.Count <= 0)
			{
				return null;
			}
			return elementsPresets.Find((CharacterElementsPreset f) => f.name == name);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x000334D0 File Offset: 0x000316D0
		public List<CharacterElementsPreset> GetElementsPresets(CharacterElementType type)
		{
			switch (type)
			{
			case CharacterElementType.Hat:
				return this._settings.hatsPresets;
			case CharacterElementType.Shirt:
				return this._settings.shirtsPresets;
			case CharacterElementType.Pants:
				return this._settings.pantsPresets;
			case CharacterElementType.Shoes:
				return this._settings.shoesPresets;
			case CharacterElementType.Accessory:
				return this._settings.accessoryPresets;
			case CharacterElementType.Hair:
				return this._settings.hairPresets;
			case CharacterElementType.Beard:
				return this._settings.beardPresets;
			case CharacterElementType.Item1:
				return this._settings.item1Presets;
			default:
				return null;
			}
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00033568 File Offset: 0x00031768
		public void PlayBlendshapeAnimation(string animationName, float duration = 1f, float weightPower = 1f)
		{
			if (this.currentBlendshapeAnimation != null)
			{
				this.StopBlendshapeAnimations();
			}
			CurrentBlendshapeAnimation currentBlendshapeAnimation = new CurrentBlendshapeAnimation();
			foreach (CharacterAnimationPreset characterAnimationPreset in this._settings.characterAnimationPresets)
			{
				if (characterAnimationPreset.name == animationName)
				{
					currentBlendshapeAnimation.preset = characterAnimationPreset;
					break;
				}
			}
			foreach (BlendshapeEmotionValue blendshapeEmotionValue in currentBlendshapeAnimation.preset.blendshapes)
			{
				CharacterBlendshapeData blendshapeData = this.GetBlendshapeData(blendshapeEmotionValue.BlendType);
				if (blendshapeData != null)
				{
					currentBlendshapeAnimation.blendShapesTemp.Add(new BlendshapeEmotionValue
					{
						BlendType = blendshapeEmotionValue.BlendType,
						BlendValue = blendshapeData.value
					});
				}
			}
			currentBlendshapeAnimation.preset.AnimationPlayDuration = 1f / duration;
			currentBlendshapeAnimation.preset.weightPower = weightPower;
			this.currentBlendshapeAnimation = currentBlendshapeAnimation;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00033688 File Offset: 0x00031888
		public void StopBlendshapeAnimations()
		{
			if (this.currentBlendshapeAnimation != null)
			{
				for (int i = 0; i < this.currentBlendshapeAnimation.preset.blendshapes.Count; i++)
				{
					this.SetBlendshapeValue(this.currentBlendshapeAnimation.preset.blendshapes[i].BlendType, this.currentBlendshapeAnimation.blendShapesTemp[i].BlendValue, null, null);
				}
			}
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x000336F8 File Offset: 0x000318F8
		public void ResetAll(bool ignore_settingsDefaultElements = true)
		{
			this.ResetBodyColors();
			foreach (CharacterBlendshapeData characterBlendshapeData in this.characterBlendshapeDatas)
			{
				this.SetBlendshapeValue(characterBlendshapeData.type, 0f, null, null);
			}
			this.SetHeadSize(0f);
			this.SetHeight(0f);
			foreach (CharacterElementType type in Enum.GetValues(typeof(CharacterElementType)).Cast<CharacterElementType>().ToList<CharacterElementType>())
			{
				this.SetElementByIndex(type, -1);
			}
			this.characterSelectedElements = (ignore_settingsDefaultElements ? new CharacterSelectedElements() : ((CharacterSelectedElements)this._settings.DefaultSelectedElements.Clone()));
			if (!ignore_settingsDefaultElements)
			{
				foreach (CharacterElementType type2 in Enum.GetValues(typeof(CharacterElementType)).Cast<CharacterElementType>().ToList<CharacterElementType>())
				{
					int selectedIndex = this.characterSelectedElements.GetSelectedIndex(type2);
					if (selectedIndex != -1)
					{
						this.SetElementByIndex(type2, selectedIndex);
					}
				}
			}
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00033860 File Offset: 0x00031A60
		public void Randomize()
		{
			CharacterGenerator.Generate(this);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00033868 File Offset: 0x00031A68
		public Animator GetAnimator()
		{
			return this.animator;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00033870 File Offset: 0x00031A70
		public void UnlockPrefab()
		{
			bool isPlaying = Application.isPlaying;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00033870 File Offset: 0x00031A70
		public void LockPrefab(string custompath = "")
		{
			bool isPlaying = Application.isPlaying;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00033878 File Offset: 0x00031A78
		public void ApplyPrefab()
		{
			if (!this.applyFeetOffset)
			{
				this.SetFeetOffset(Vector3.zero);
			}
			else
			{
				this.SetFeetOffset(new Vector3(0f, this.feetOffset, 0f));
			}
			this.ResetBodyMaterial();
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x000338B0 File Offset: 0x00031AB0
		public void RevertBonesChanges()
		{
			SkinnedMeshRenderer skinnedMeshRenderer = this.Settings.OriginalMesh.GetComponentsInChildren<SkinnedMeshRenderer>()[0];
			Transform[] bones = skinnedMeshRenderer.bones;
			Transform rootBone = skinnedMeshRenderer.rootBone;
			Transform[] bones2 = this.GetCharacterPart("Head").skinnedMesh[0].bones;
			this.originHip.localPosition = new Vector3(0f, this.feetOffset, 0f);
			for (int i = 0; i < bones2.Length; i++)
			{
				bones2[i].localPosition = bones[i].localPosition;
				bones2[i].localRotation = bones[i].localRotation;
			}
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x000045B1 File Offset: 0x000027B1
		public void ApplyPrefabInPlaymode()
		{
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00033946 File Offset: 0x00031B46
		public void UpdateActualCharacterInstanceStatus(bool igroneUserNonPrefab = false)
		{
			if (Application.isPlaying || this.instanceStatus == CharacterInstanceStatus.NotAPrefabByUser)
			{
				return;
			}
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0003395B File Offset: 0x00031B5B
		public CharacterInstanceStatus GetCharacterInstanceStatus()
		{
			return this.instanceStatus;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00033963 File Offset: 0x00031B63
		public void SetNewCharacterInstanceStatus(CharacterInstanceStatus characterInstanceStatus)
		{
			this.instanceStatus = characterInstanceStatus;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0003396C File Offset: 0x00031B6C
		public CharacterBlendshapeData GetBlendshapeData(CharacterBlendShapeType type)
		{
			foreach (CharacterBlendshapeData characterBlendshapeData in this.characterBlendshapeDatas)
			{
				if (characterBlendshapeData.type == type)
				{
					return characterBlendshapeData;
				}
			}
			return null;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x000339C8 File Offset: 0x00031BC8
		public CharacterBlendshapeData GetBlendshapeData(string name)
		{
			foreach (CharacterBlendshapeData characterBlendshapeData in this.characterBlendshapeDatas)
			{
				if (characterBlendshapeData.blendshapeName == name)
				{
					return characterBlendshapeData;
				}
			}
			return null;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00033A2C File Offset: 0x00031C2C
		public List<CharacterBlendshapeData> GetBlendshapeDatasByGroup(CharacterBlendShapeGroup group)
		{
			List<CharacterBlendshapeData> list = new List<CharacterBlendshapeData>();
			foreach (CharacterBlendshapeData characterBlendshapeData in this.characterBlendshapeDatas)
			{
				if (characterBlendshapeData.group == group)
				{
					list.Add(characterBlendshapeData);
				}
			}
			return list;
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00033A90 File Offset: 0x00031C90
		private void DestroyObjects(UnityEngine.Object[] objects)
		{
			foreach (UnityEngine.Object @object in objects)
			{
				if (@object != null)
				{
					UnityEngine.Object.Destroy(@object);
				}
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00033B20 File Offset: 0x00031D20
		[CompilerGenerated]
		private void <BakeCharacter>g__MeshesProcess|77_0(bool usePreBuilt = false)
		{
			foreach (CharacterPart characterPart in this.characterParts)
			{
				if (characterPart.name.ToLowerInvariant() != "combined")
				{
					characterPart.skinnedMesh.ForEach(delegate(SkinnedMeshRenderer m)
					{
						m.gameObject.SetActive(false);
					});
				}
				else
				{
					for (int i = 0; i < characterPart.skinnedMesh.Count; i++)
					{
						if (usePreBuilt)
						{
							characterPart.skinnedMesh[i].sharedMesh = this.combinedCharacter.preBuiltDatas[0].meshes[i];
							characterPart.skinnedMesh[i].sharedMaterials = this.combinedCharacter.preBuiltDatas[0].materials.ToArray();
						}
						characterPart.skinnedMesh[i].gameObject.SetActive(true);
					}
				}
			}
			foreach (ClothesAnchor clothesAnchor in this.clothesAnchors)
			{
				clothesAnchor.skinnedMesh.ForEach(delegate(SkinnedMeshRenderer m)
				{
					m.gameObject.SetActive(false);
				});
			}
			if (Application.isPlaying)
			{
				this.InitColors();
			}
		}

		// Token: 0x04000BC2 RID: 3010
		[SerializeField]
		public bool isSettingsExpanded;

		// Token: 0x04000BC3 RID: 3011
		public CharacterSettings selectedsettings;

		// Token: 0x04000BC4 RID: 3012
		[SerializeField]
		private CharacterSettings _settings;

		// Token: 0x04000BC5 RID: 3013
		public List<CharacterPart> characterParts = new List<CharacterPart>();

		// Token: 0x04000BC6 RID: 3014
		public string prefabPath = string.Empty;

		// Token: 0x04000BC7 RID: 3015
		[SerializeField]
		public CharacterInstanceStatus instanceStatus;

		// Token: 0x04000BC8 RID: 3016
		public Transform originHip;

		// Token: 0x04000BC9 RID: 3017
		public Transform headHip;

		// Token: 0x04000BCA RID: 3018
		public List<ClothesAnchor> clothesAnchors = new List<ClothesAnchor>();

		// Token: 0x04000BCB RID: 3019
		public Animator animator;

		// Token: 0x04000BCC RID: 3020
		public CharacterSelectedElements characterSelectedElements = new CharacterSelectedElements();

		// Token: 0x04000BCD RID: 3021
		public float heightValue;

		// Token: 0x04000BCE RID: 3022
		public float headSizeValue;

		// Token: 0x04000BCF RID: 3023
		public float feetOffset;

		// Token: 0x04000BD0 RID: 3024
		public List<CharacterBlendshapeData> characterBlendshapeDatas = new List<CharacterBlendshapeData>();

		// Token: 0x04000BD1 RID: 3025
		public Color Skin;

		// Token: 0x04000BD2 RID: 3026
		public Color Eye;

		// Token: 0x04000BD3 RID: 3027
		public Color Hair;

		// Token: 0x04000BD4 RID: 3028
		public Color Underpants;

		// Token: 0x04000BD5 RID: 3029
		public Color OralCavity;

		// Token: 0x04000BD6 RID: 3030
		public Color Teeth;

		// Token: 0x04000BD7 RID: 3031
		public MaterialPropertyBlock bodyPropertyBlock;

		// Token: 0x04000BD8 RID: 3032
		public CurrentBlendshapeAnimation currentBlendshapeAnimation;

		// Token: 0x04000BD9 RID: 3033
		public CombinerState CurrentCombinerState;

		// Token: 0x04000BDA RID: 3034
		public CharacterPreBuilt combinedCharacter;

		// Token: 0x04000BDB RID: 3035
		public Transform ProbesAnchorOverride;

		// Token: 0x04000BDC RID: 3036
		public CharacterGeneratorSettings CharacterGenerator_settings;

		// Token: 0x04000BDD RID: 3037
		public bool UpdateWhenOffscreenMeshes = true;

		// Token: 0x04000BDE RID: 3038
		[SerializeField]
		public int MinLODLevels;

		// Token: 0x04000BDF RID: 3039
		[SerializeField]
		public int MaxLODLevels = 3;

		// Token: 0x04000BE0 RID: 3040
		private LODGroup _lodGroup;

		// Token: 0x04000BE1 RID: 3041
		public Transform _transform;

		// Token: 0x04000BE2 RID: 3042
		public bool applyFeetOffset = true;

		// Token: 0x04000BE3 RID: 3043
		public bool notAPP2Shader;

		// Token: 0x04000BE4 RID: 3044
		private GameObject prebuiltPrefab;
	}
}
