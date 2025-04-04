using System;
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005FC RID: 1532
	public class ViewmodelAvatar : Singleton<ViewmodelAvatar>
	{
		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x000A5216 File Offset: 0x000A3416
		// (set) Token: 0x0600282A RID: 10282 RVA: 0x000A521E File Offset: 0x000A341E
		public bool IsVisible { get; private set; }

		// Token: 0x0600282B RID: 10283 RVA: 0x000A5228 File Offset: 0x000A3428
		protected override void Awake()
		{
			base.Awake();
			this.baseOffset = base.transform.localPosition;
			this.SetVisibility(false);
			if (this.ParentAvatar.CurrentSettings != null)
			{
				this.SetAppearance(this.ParentAvatar.CurrentSettings);
			}
			this.ParentAvatar.onSettingsLoaded.AddListener(delegate()
			{
				this.SetAppearance(this.ParentAvatar.CurrentSettings);
			});
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000A5293 File Offset: 0x000A3493
		public void SetVisibility(bool isVisible)
		{
			this.SetOffset(Vector3.zero);
			this.IsVisible = isVisible;
			base.gameObject.SetActive(isVisible);
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x000A52B4 File Offset: 0x000A34B4
		public void SetAppearance(AvatarSettings settings)
		{
			AvatarSettings avatarSettings = UnityEngine.Object.Instantiate<AvatarSettings>(settings);
			avatarSettings.Height = 0.25f;
			this.Avatar.LoadAvatarSettings(avatarSettings);
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Viewmodel"));
			foreach (MeshRenderer meshRenderer in base.GetComponentsInChildren<MeshRenderer>())
			{
				if (meshRenderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					meshRenderer.enabled = false;
				}
				else
				{
					meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				}
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.GetComponentsInChildren<SkinnedMeshRenderer>())
			{
				if (skinnedMeshRenderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					skinnedMeshRenderer.enabled = false;
				}
				else
				{
					skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				}
			}
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000A535F File Offset: 0x000A355F
		public void SetAnimatorController(RuntimeAnimatorController controller)
		{
			this.Animator.runtimeAnimatorController = controller;
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x000A536D File Offset: 0x000A356D
		public void SetOffset(Vector3 offset)
		{
			base.transform.localPosition = this.baseOffset + offset;
		}

		// Token: 0x04001D49 RID: 7497
		public ScheduleOne.AvatarFramework.Avatar ParentAvatar;

		// Token: 0x04001D4A RID: 7498
		public Animator Animator;

		// Token: 0x04001D4B RID: 7499
		public ScheduleOne.AvatarFramework.Avatar Avatar;

		// Token: 0x04001D4C RID: 7500
		public Transform RightHandContainer;

		// Token: 0x04001D4D RID: 7501
		private Vector3 baseOffset;
	}
}
