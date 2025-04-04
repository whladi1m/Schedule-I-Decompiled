using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000207 RID: 519
	public static class CharacterSystemUpdater
	{
		// Token: 0x06000B69 RID: 2921 RVA: 0x00035050 File Offset: 0x00033250
		[RuntimeInitializeOnLoadMethod]
		private static void updateCharacters()
		{
			CharacterSystemUpdater.UpdateCharactersOnScene(false, null);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0003505C File Offset: 0x0003325C
		public static void UpdateCharactersOnScene(bool revertPrefabs = false, CharacterCustomization reverbObject = null)
		{
			CharacterCustomization[] array = UnityEngine.Object.FindObjectsOfType<CharacterCustomization>();
			if (array == null)
			{
				return;
			}
			foreach (CharacterCustomization characterCustomization in array)
			{
				if (!(characterCustomization == null))
				{
					characterCustomization.InitColors();
				}
			}
		}
	}
}
