using System;
using ScheduleOne.AvatarFramework;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Items;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScheduleOne.UI
{
	// Token: 0x020009AB RID: 2475
	public class CharacterDisplay : Singleton<CharacterDisplay>
	{
		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x060042D8 RID: 17112 RVA: 0x001181C7 File Offset: 0x001163C7
		// (set) Token: 0x060042D9 RID: 17113 RVA: 0x001181CF File Offset: 0x001163CF
		public bool IsOpen { get; private set; }

		// Token: 0x060042DA RID: 17114 RVA: 0x001181D8 File Offset: 0x001163D8
		protected override void Awake()
		{
			base.Awake();
			this.SetOpen(false);
			if (this.ParentAvatar.CurrentSettings != null)
			{
				this.SetAppearance(this.ParentAvatar.CurrentSettings);
			}
			this.ParentAvatar.onSettingsLoaded.AddListener(delegate()
			{
				this.SetAppearance(this.ParentAvatar.CurrentSettings);
			});
			AudioSource[] componentsInChildren = this.Avatar.GetComponentsInChildren<AudioSource>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x00118258 File Offset: 0x00116458
		public void SetOpen(bool open)
		{
			this.IsOpen = open;
			this.Container.gameObject.SetActive(open);
			if (this.IsOpen)
			{
				LayerUtility.SetLayerRecursively(this.Container.gameObject, LayerMask.NameToLayer("Overlay"));
				this.SetAppearance(this.ParentAvatar.CurrentSettings);
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), Player.Local.Clothing.ItemSlots);
			}
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x001182D4 File Offset: 0x001164D4
		private void Update()
		{
			if (this.IsOpen)
			{
				this.targetRotation = Mathf.Lerp(this.targetRotation, Mathf.Lerp(0f, 359f, Singleton<GameplayMenuInterface>.Instance.CharacterInterface.RotationSlider.value), Time.deltaTime * 5f);
				this.AvatarContainer.localEulerAngles = new Vector3(0f, this.targetRotation, 0f);
			}
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x00118348 File Offset: 0x00116548
		public void SetAppearance(AvatarSettings settings)
		{
			AvatarSettings settings2 = UnityEngine.Object.Instantiate<AvatarSettings>(settings);
			this.Avatar.LoadAvatarSettings(settings2);
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Overlay"));
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

		// Token: 0x040030CD RID: 12493
		public CharacterDisplay.SlotAlignmentPoint[] AlignmentPoints;

		// Token: 0x040030CE RID: 12494
		[Header("References")]
		public Transform Container;

		// Token: 0x040030CF RID: 12495
		public ScheduleOne.AvatarFramework.Avatar ParentAvatar;

		// Token: 0x040030D0 RID: 12496
		public ScheduleOne.AvatarFramework.Avatar Avatar;

		// Token: 0x040030D1 RID: 12497
		public Transform AvatarContainer;

		// Token: 0x040030D2 RID: 12498
		private float targetRotation;

		// Token: 0x020009AC RID: 2476
		[Serializable]
		public class SlotAlignmentPoint
		{
			// Token: 0x040030D3 RID: 12499
			public EClothingSlot SlotType;

			// Token: 0x040030D4 RID: 12500
			public Transform Point;
		}
	}
}
