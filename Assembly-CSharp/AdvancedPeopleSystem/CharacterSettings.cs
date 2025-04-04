using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000214 RID: 532
	[CreateAssetMenu(fileName = "NewCharacterSettings", menuName = "Advanced People Pack/Settings", order = 1)]
	public class CharacterSettings : ScriptableObject
	{
		// Token: 0x04000C7D RID: 3197
		public GameObject OriginalMesh;

		// Token: 0x04000C7E RID: 3198
		public Material bodyMaterial;

		// Token: 0x04000C7F RID: 3199
		[Space(20f)]
		public List<CharacterAnimationPreset> characterAnimationPresets = new List<CharacterAnimationPreset>();

		// Token: 0x04000C80 RID: 3200
		[Space(20f)]
		public List<CharacterBlendshapeData> characterBlendshapeDatas = new List<CharacterBlendshapeData>();

		// Token: 0x04000C81 RID: 3201
		[Space(20f)]
		public List<CharacterElementsPreset> hairPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000C82 RID: 3202
		public List<CharacterElementsPreset> beardPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000C83 RID: 3203
		public List<CharacterElementsPreset> hatsPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000C84 RID: 3204
		public List<CharacterElementsPreset> accessoryPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000C85 RID: 3205
		public List<CharacterElementsPreset> shirtsPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000C86 RID: 3206
		public List<CharacterElementsPreset> pantsPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000C87 RID: 3207
		public List<CharacterElementsPreset> shoesPresets = new List<CharacterElementsPreset>();

		// Token: 0x04000C88 RID: 3208
		public List<CharacterElementsPreset> item1Presets = new List<CharacterElementsPreset>();

		// Token: 0x04000C89 RID: 3209
		[Space(20f)]
		public List<CharacterSettingsSelector> settingsSelectors = new List<CharacterSettingsSelector>();

		// Token: 0x04000C8A RID: 3210
		[Space(20f)]
		public RuntimeAnimatorController Animator;

		// Token: 0x04000C8B RID: 3211
		public Avatar Avatar;

		// Token: 0x04000C8C RID: 3212
		[Space(20f)]
		public CharacterGeneratorSettings generator;

		// Token: 0x04000C8D RID: 3213
		[Space(20f)]
		public CharacterSelectedElements DefaultSelectedElements = new CharacterSelectedElements();

		// Token: 0x04000C8E RID: 3214
		[Space(20f)]
		public bool DisableBlendshapeModifier;
	}
}
