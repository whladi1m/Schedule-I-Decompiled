using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x02000954 RID: 2388
	public class MugshotGenerator : Singleton<MugshotGenerator>
	{
		// Token: 0x0600411B RID: 16667 RVA: 0x001116BB File Offset: 0x0010F8BB
		protected override void Awake()
		{
			base.Awake();
			this.MugshotRig.gameObject.SetActive(false);
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x001116D4 File Offset: 0x0010F8D4
		private void LateUpdate()
		{
			if (this.generate)
			{
				this.generate = false;
				this.FinalizeMugshot();
			}
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x001116EB File Offset: 0x0010F8EB
		private void FinalizeMugshot()
		{
			this.finalTexture = this.Generator.GetTexture(this.MugshotRig.transform);
			Debug.Log("Mugshot capture");
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x00111713 File Offset: 0x0010F913
		[Button]
		public void GenerateMugshot()
		{
			this.GenerateMugshot(this.Settings, true, null);
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x00111724 File Offset: 0x0010F924
		public void GenerateMugshot(AvatarSettings settings, bool fileToFile, Action<Texture2D> callback)
		{
			MugshotGenerator.<>c__DisplayClass12_0 CS$<>8__locals1 = new MugshotGenerator.<>c__DisplayClass12_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.fileToFile = fileToFile;
			CS$<>8__locals1.settings = settings;
			CS$<>8__locals1.callback = callback;
			this.finalTexture = null;
			Debug.Log("Mugshot start");
			AvatarSettings avatarSettings = UnityEngine.Object.Instantiate<AvatarSettings>(CS$<>8__locals1.settings);
			avatarSettings.Height = 1f;
			this.MugshotRig.gameObject.SetActive(true);
			this.MugshotRig.LoadAvatarSettings(avatarSettings);
			LayerUtility.SetLayerRecursively(this.MugshotRig.gameObject, LayerMask.NameToLayer("IconGeneration"));
			SkinnedMeshRenderer[] componentsInChildren = this.MugshotRig.GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].updateWhenOffscreen = true;
			}
			this.generate = true;
			base.StartCoroutine(CS$<>8__locals1.<GenerateMugshot>g__Routine|0());
		}

		// Token: 0x04002EF6 RID: 12022
		public string OutputPath;

		// Token: 0x04002EF7 RID: 12023
		public AvatarSettings Settings;

		// Token: 0x04002EF8 RID: 12024
		[Header("References")]
		public Avatar MugshotRig;

		// Token: 0x04002EF9 RID: 12025
		public IconGenerator Generator;

		// Token: 0x04002EFA RID: 12026
		public AvatarSettings DefaultSettings;

		// Token: 0x04002EFB RID: 12027
		public Transform LookAtPosition;

		// Token: 0x04002EFC RID: 12028
		private Texture2D finalTexture;

		// Token: 0x04002EFD RID: 12029
		private bool generate;
	}
}
