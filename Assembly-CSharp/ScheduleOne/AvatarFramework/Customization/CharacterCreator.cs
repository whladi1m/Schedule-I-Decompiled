using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Networking;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.UI.CharacterCreator;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x0200098D RID: 2445
	public class CharacterCreator : Singleton<CharacterCreator>
	{
		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06004238 RID: 16952 RVA: 0x00115E99 File Offset: 0x00114099
		// (set) Token: 0x06004239 RID: 16953 RVA: 0x00115EA1 File Offset: 0x001140A1
		public bool IsOpen { get; protected set; }

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x0600423A RID: 16954 RVA: 0x00115EAA File Offset: 0x001140AA
		// (set) Token: 0x0600423B RID: 16955 RVA: 0x00115EB2 File Offset: 0x001140B2
		public BasicAvatarSettings ActiveSettings { get; protected set; }

		// Token: 0x0600423C RID: 16956 RVA: 0x00115EBB File Offset: 0x001140BB
		protected override void Awake()
		{
			if (this.DemoCreator)
			{
				base.gameObject.SetActive(false);
				return;
			}
			base.Awake();
			this.Fields = this.Canvas.GetComponentsInChildren<BaseCharacterCreatorField>(true).ToList<BaseCharacterCreatorField>();
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x00115EEF File Offset: 0x001140EF
		protected override void Start()
		{
			base.Start();
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x00115F08 File Offset: 0x00114108
		private void Update()
		{
			this.RigContainer.localEulerAngles = Vector3.Lerp(this.RigContainer.localEulerAngles, new Vector3(0f, this.rigTargetY, 0f), Time.deltaTime * 5f);
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x00115F48 File Offset: 0x00114148
		public void Open(BasicAvatarSettings initialSettings, bool showUI = true)
		{
			this.IsOpen = true;
			if (showUI)
			{
				this.ShowUI();
			}
			if (!this.DemoCreator)
			{
				PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0f);
				PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0f, false);
			}
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<HUD>.Instance.canvas.enabled = false;
			this.Container.gameObject.SetActive(true);
			if (InstanceFinder.IsServer && !Singleton<Lobby>.Instance.IsInLobby)
			{
				NetworkSingleton<TimeManager>.Instance.TimeProgressionMultiplier = 0f;
			}
			if (initialSettings != null)
			{
				this.ActiveSettings = UnityEngine.Object.Instantiate<BasicAvatarSettings>(initialSettings);
			}
			else
			{
				this.ActiveSettings = ScriptableObject.CreateInstance<BasicAvatarSettings>();
			}
			this.Rig.LoadAvatarSettings(this.ActiveSettings.GetAvatarSettings());
			for (int i = 0; i < this.Fields.Count; i++)
			{
				this.Fields[i].ApplyValue();
				this.Fields[i].WriteValue(false);
			}
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x00116071 File Offset: 0x00114271
		public void ShowUI()
		{
			this.Canvas.enabled = true;
			this.CanvasAnimation.Play("Character creator fade in");
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x001160AA File Offset: 0x001142AA
		public void Close()
		{
			this.IsOpen = false;
			base.StartCoroutine(this.<Close>g__Close|28_0());
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x001160C0 File Offset: 0x001142C0
		public void DisableStuff()
		{
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x001160D4 File Offset: 0x001142D4
		public void Done()
		{
			if (!this.IsOpen)
			{
				return;
			}
			List<ClothingInstance> list = new List<ClothingInstance>();
			if (!string.IsNullOrEmpty(this.ActiveSettings.Shoes))
			{
				EClothingColor clothingColor = ClothingColorExtensions.GetClothingColor(this.ActiveSettings.ShoesColor);
				list.Add(new ClothingInstance(this.lastSelectedClothingDefinitions["Shoes"], 1, clothingColor));
			}
			if (!string.IsNullOrEmpty(this.ActiveSettings.Top))
			{
				EClothingColor clothingColor2 = ClothingColorExtensions.GetClothingColor(this.ActiveSettings.TopColor);
				list.Add(new ClothingInstance(this.lastSelectedClothingDefinitions["Top"], 1, clothingColor2));
			}
			if (!string.IsNullOrEmpty(this.ActiveSettings.Bottom))
			{
				EClothingColor clothingColor3 = ClothingColorExtensions.GetClothingColor(this.ActiveSettings.BottomColor);
				list.Add(new ClothingInstance(this.lastSelectedClothingDefinitions["Bottom"], 1, clothingColor3));
			}
			if (this.onComplete != null)
			{
				this.onComplete.Invoke(this.ActiveSettings);
			}
			if (this.onCompleteWithClothing != null)
			{
				this.onCompleteWithClothing.Invoke(this.ActiveSettings, list);
			}
			this.Close();
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x001161E9 File Offset: 0x001143E9
		public void SliderChanged(float newVal)
		{
			this.rigTargetY = newVal * 359f;
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x001161F8 File Offset: 0x001143F8
		public T SetValue<T>(string fieldName, T value, ClothingDefinition definition)
		{
			if (!this.lastSelectedClothingDefinitions.ContainsKey(fieldName))
			{
				this.lastSelectedClothingDefinitions.Add(fieldName, definition);
			}
			else
			{
				this.lastSelectedClothingDefinitions[fieldName] = definition;
			}
			if (fieldName == "Preset")
			{
				this.SelectPreset(value as string);
				return default(T);
			}
			this.ActiveSettings.SetValue<T>(fieldName, value);
			return value;
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x00116268 File Offset: 0x00114468
		public void SelectPreset(string presetName)
		{
			BasicAvatarSettings basicAvatarSettings = this.Presets.Find((BasicAvatarSettings p) => p.name == presetName);
			if (basicAvatarSettings == null)
			{
				Debug.LogError("Preset not found: " + presetName);
				return;
			}
			this.ActiveSettings = UnityEngine.Object.Instantiate<BasicAvatarSettings>(basicAvatarSettings);
			this.Rig.LoadAvatarSettings(this.ActiveSettings.GetAvatarSettings());
			for (int i = 0; i < this.Fields.Count; i++)
			{
				this.Fields[i].ApplyValue();
			}
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x00116304 File Offset: 0x00114504
		public void RefreshCategory(CharacterCreator.ECategory category)
		{
			AvatarSettings avatarSettings = this.ActiveSettings.GetAvatarSettings();
			switch (category)
			{
			case CharacterCreator.ECategory.Body:
				this.Rig.ApplyBodySettings(avatarSettings);
				this.Rig.ApplyEyeLidColorSettings(avatarSettings);
				this.Rig.ApplyBodyLayerSettings(avatarSettings, -1);
				return;
			case CharacterCreator.ECategory.Hair:
				this.Rig.ApplyHairSettings(avatarSettings);
				this.Rig.ApplyHairColorSettings(avatarSettings);
				this.Rig.ApplyFaceLayerSettings(avatarSettings);
				return;
			case CharacterCreator.ECategory.Face:
				this.Rig.ApplyFaceLayerSettings(avatarSettings);
				return;
			case CharacterCreator.ECategory.Eyes:
				this.Rig.ApplyEyeBallSettings(avatarSettings);
				this.Rig.ApplyEyeLidColorSettings(avatarSettings);
				this.Rig.ApplyEyeLidSettings(avatarSettings);
				return;
			case CharacterCreator.ECategory.Eyebrows:
				this.Rig.ApplyEyebrowSettings(avatarSettings);
				return;
			case CharacterCreator.ECategory.Clothing:
				this.Rig.ApplyBodyLayerSettings(avatarSettings, -1);
				return;
			case CharacterCreator.ECategory.Accessories:
				this.Rig.ApplyAccessorySettings(avatarSettings);
				return;
			default:
				return;
			}
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x00116402 File Offset: 0x00114602
		[CompilerGenerated]
		private IEnumerator <Close>g__Close|28_0()
		{
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.rigTargetY = 0f;
			this.Canvas.enabled = false;
			if (InstanceFinder.IsServer)
			{
				NetworkSingleton<TimeManager>.Instance.TimeProgressionMultiplier = 1f;
			}
			yield break;
		}

		// Token: 0x04003049 RID: 12361
		public List<BaseCharacterCreatorField> Fields = new List<BaseCharacterCreatorField>();

		// Token: 0x0400304B RID: 12363
		[Header("References")]
		public Transform Container;

		// Token: 0x0400304C RID: 12364
		public Transform CameraPosition;

		// Token: 0x0400304D RID: 12365
		public Transform RigContainer;

		// Token: 0x0400304E RID: 12366
		public Avatar Rig;

		// Token: 0x0400304F RID: 12367
		public Canvas Canvas;

		// Token: 0x04003050 RID: 12368
		public Animation CanvasAnimation;

		// Token: 0x04003051 RID: 12369
		[Header("Settings")]
		public bool DemoCreator;

		// Token: 0x04003052 RID: 12370
		public BasicAvatarSettings DefaultSettings;

		// Token: 0x04003053 RID: 12371
		public List<BasicAvatarSettings> Presets;

		// Token: 0x04003054 RID: 12372
		public UnityEvent<BasicAvatarSettings> onComplete;

		// Token: 0x04003055 RID: 12373
		public UnityEvent<BasicAvatarSettings, List<ClothingInstance>> onCompleteWithClothing;

		// Token: 0x04003056 RID: 12374
		private Dictionary<string, ClothingDefinition> lastSelectedClothingDefinitions = new Dictionary<string, ClothingDefinition>();

		// Token: 0x04003057 RID: 12375
		private float rigTargetY;

		// Token: 0x0200098E RID: 2446
		public enum ECategory
		{
			// Token: 0x04003059 RID: 12377
			Body,
			// Token: 0x0400305A RID: 12378
			Hair,
			// Token: 0x0400305B RID: 12379
			Face,
			// Token: 0x0400305C RID: 12380
			Eyes,
			// Token: 0x0400305D RID: 12381
			Eyebrows,
			// Token: 0x0400305E RID: 12382
			Clothing,
			// Token: 0x0400305F RID: 12383
			Accessories
		}
	}
}
