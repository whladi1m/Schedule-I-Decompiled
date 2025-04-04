using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.Networking;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006FF RID: 1791
	public class Settings : PersistentSingleton<Settings>
	{
		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x0600305F RID: 12383 RVA: 0x000C94B7 File Offset: 0x000C76B7
		// (set) Token: 0x06003060 RID: 12384 RVA: 0x000C94BF File Offset: 0x000C76BF
		public Settings.UnitType unitType { get; protected set; }

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06003061 RID: 12385 RVA: 0x000C94C8 File Offset: 0x000C76C8
		public bool PausingFreezesTime
		{
			get
			{
				return Player.PlayerList.Count <= 1 && !Singleton<Lobby>.Instance.IsInLobby;
			}
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x000C94E8 File Offset: 0x000C76E8
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<Settings>.Instance == null || Singleton<Settings>.Instance != this)
			{
				return;
			}
			this.playerControls = this.InputActions.FindActionMap("Generic", false);
			this.DisplaySettings = this.ReadDisplaySettings();
			this.UnappliedDisplaySettings = this.ReadDisplaySettings();
			this.GraphicsSettings = this.ReadGraphicsSettings();
			this.AudioSettings = this.ReadAudioSettings();
			this.InputSettings = this.ReadInputSettings();
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == "-beta")
				{
					GameManager.IS_BETA = true;
				}
			}
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x000C9592 File Offset: 0x000C7792
		protected override void Start()
		{
			base.Start();
			this.ApplyDisplaySettings(this.DisplaySettings);
			this.ApplyGraphicsSettings(this.GraphicsSettings);
			this.ApplyAudioSettings(this.AudioSettings);
			this.ApplyInputSettings(this.InputSettings);
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x000C95CC File Offset: 0x000C77CC
		public void ApplyDisplaySettings(DisplaySettings settings)
		{
			Resolution[] array = DisplaySettings.GetResolutions().ToArray();
			Resolution resolution = array[Mathf.Clamp(settings.ResolutionIndex, 0, array.Length - 1)];
			FullScreenMode fullScreenMode = FullScreenMode.Windowed;
			switch (settings.DisplayMode)
			{
			case DisplaySettings.EDisplayMode.Windowed:
				fullScreenMode = FullScreenMode.Windowed;
				break;
			case DisplaySettings.EDisplayMode.FullscreenWindow:
				fullScreenMode = FullScreenMode.FullScreenWindow;
				break;
			case DisplaySettings.EDisplayMode.ExclusiveFullscreen:
				fullScreenMode = FullScreenMode.ExclusiveFullScreen;
				break;
			}
			Screen.fullScreenMode = fullScreenMode;
			Screen.SetResolution(resolution.width, resolution.height, settings.DisplayMode == DisplaySettings.EDisplayMode.ExclusiveFullscreen || settings.DisplayMode == DisplaySettings.EDisplayMode.FullscreenWindow);
			QualitySettings.vSyncCount = (settings.VSync ? 1 : 0);
			Application.targetFrameRate = settings.TargetFPS;
			List<DisplayInfo> list = new List<DisplayInfo>();
			Screen.GetDisplayLayout(list);
			DisplayInfo displayInfo = list[Mathf.Clamp(settings.ActiveDisplayIndex, 0, list.Count - 1)];
			this.MoveMainWindowTo(displayInfo);
			CanvasScaler.SetScaleFactor(settings.UIScale);
			Singleton<Settings>.Instance.CameraBobIntensity = settings.CameraBobbing;
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x000C96BA File Offset: 0x000C78BA
		private void MoveMainWindowTo(DisplayInfo displayInfo)
		{
			Console.Log("Moving main window to display: " + displayInfo.name, null);
			Screen.MoveMainWindowTo(displayInfo, new Vector2Int(displayInfo.width / 2, displayInfo.height / 2));
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000C96EF File Offset: 0x000C78EF
		public void ReloadGraphicsSettings()
		{
			this.ApplyGraphicsSettings(this.GraphicsSettings);
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x000C9700 File Offset: 0x000C7900
		public void ApplyGraphicsSettings(GraphicsSettings settings)
		{
			QualitySettings.SetQualityLevel((int)settings.GraphicsQuality);
			PlayerCamera.SetAntialiasingMode(settings.AntiAliasingMode);
			this.CameraFOV = settings.FOV;
			this.SSAO.SetActive(settings.SSAO);
			this.GodRays.SetActive(settings.GodRays);
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000C9751 File Offset: 0x000C7951
		public void ReloadAudioSettings()
		{
			this.ApplyAudioSettings(this.AudioSettings);
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x000C9760 File Offset: 0x000C7960
		public void ApplyAudioSettings(AudioSettings settings)
		{
			Singleton<AudioManager>.Instance.SetMasterVolume(settings.MasterVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.Ambient, settings.AmbientVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.Music, settings.MusicVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.FX, settings.SFXVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.UI, settings.UIVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.Voice, settings.DialogueVolume);
			Singleton<AudioManager>.Instance.SetVolume(EAudioType.Footsteps, settings.FootstepsVolume);
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x000C97E3 File Offset: 0x000C79E3
		public void ReloadInputSettings()
		{
			this.ApplyInputSettings(this.InputSettings);
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x000C97F4 File Offset: 0x000C79F4
		public void ApplyInputSettings(InputSettings settings)
		{
			this.InputSettings = settings;
			this.LookSensitivity = settings.MouseSensitivity;
			this.InvertMouse = settings.InvertMouse;
			this.SprintMode = settings.SprintMode;
			this.InputActions.Disable();
			this.InputActions.LoadBindingOverridesFromJson(settings.BindingOverrides, true);
			this.InputActions.Enable();
			this.GameInput.PlayerInput.actions = this.InputActions;
			Action action = this.onInputsApplied;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x000C987C File Offset: 0x000C7A7C
		public void WriteDisplaySettings(DisplaySettings settings)
		{
			this.DisplaySettings = settings;
			this.UnappliedDisplaySettings = settings;
			PlayerPrefs.SetInt("ResolutionIndex", settings.ResolutionIndex);
			PlayerPrefs.SetInt("DisplayMode", (int)settings.DisplayMode);
			PlayerPrefs.SetInt("VSync", settings.VSync ? 1 : 0);
			PlayerPrefs.SetInt("TargetFPS", settings.TargetFPS);
			PlayerPrefs.SetFloat("UIScale", settings.UIScale);
			PlayerPrefs.SetFloat("CameraBobbing", settings.CameraBobbing);
			PlayerPrefs.SetInt("ActiveDisplayIndex", settings.ActiveDisplayIndex);
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x000C9910 File Offset: 0x000C7B10
		public DisplaySettings ReadDisplaySettings()
		{
			return new DisplaySettings
			{
				ResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", Screen.resolutions.Length - 1),
				DisplayMode = (DisplaySettings.EDisplayMode)PlayerPrefs.GetInt("DisplayMode", 2),
				VSync = (PlayerPrefs.GetInt("VSync", 1) == 1),
				TargetFPS = PlayerPrefs.GetInt("TargetFPS", 90),
				UIScale = PlayerPrefs.GetFloat("UIScale", 1f),
				CameraBobbing = PlayerPrefs.GetFloat("CameraBobbing", 0.7f),
				ActiveDisplayIndex = PlayerPrefs.GetInt("ActiveDisplayIndex", 0)
			};
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x000C99B8 File Offset: 0x000C7BB8
		public void WriteGraphicsSettings(GraphicsSettings settings)
		{
			this.GraphicsSettings = settings;
			PlayerPrefs.SetInt("QualityLevel", (int)settings.GraphicsQuality);
			PlayerPrefs.SetInt("AntiAliasing", (int)settings.AntiAliasingMode);
			PlayerPrefs.SetFloat("FOV", settings.FOV);
			PlayerPrefs.SetInt("SSAO", settings.SSAO ? 1 : 0);
			PlayerPrefs.SetInt("GodRays", settings.GodRays ? 1 : 0);
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x000C9A28 File Offset: 0x000C7C28
		public GraphicsSettings ReadGraphicsSettings()
		{
			return new GraphicsSettings
			{
				GraphicsQuality = (GraphicsSettings.EGraphicsQuality)PlayerPrefs.GetInt("QualityLevel", 2),
				AntiAliasingMode = (GraphicsSettings.EAntiAliasingMode)PlayerPrefs.GetInt("AntiAliasing", 2),
				FOV = PlayerPrefs.GetFloat("FOV", 80f),
				SSAO = (PlayerPrefs.GetInt("SSAO", 1) == 1),
				GodRays = (PlayerPrefs.GetInt("GodRays", 1) == 1)
			};
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x000C9A9C File Offset: 0x000C7C9C
		public void WriteAudioSettings(AudioSettings settings)
		{
			this.AudioSettings = settings;
			PlayerPrefs.SetFloat("MasterVolume", settings.MasterVolume);
			PlayerPrefs.SetFloat("AmbientVolume", settings.AmbientVolume);
			PlayerPrefs.SetFloat("MusicVolume", settings.MusicVolume);
			PlayerPrefs.SetFloat("SFXVolume", settings.SFXVolume);
			PlayerPrefs.SetFloat("UIVolume", settings.UIVolume);
			PlayerPrefs.SetFloat("DialogueVolume", settings.DialogueVolume);
			PlayerPrefs.SetFloat("FootstepsVolume", settings.FootstepsVolume);
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x000C9B20 File Offset: 0x000C7D20
		public AudioSettings ReadAudioSettings()
		{
			return new AudioSettings
			{
				MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f),
				AmbientVolume = PlayerPrefs.GetFloat("AmbientVolume", 1f),
				MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f),
				SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f),
				UIVolume = PlayerPrefs.GetFloat("UIVolume", 1f),
				DialogueVolume = PlayerPrefs.GetFloat("DialogueVolume", 1f),
				FootstepsVolume = PlayerPrefs.GetFloat("FootstepsVolume", 1f)
			};
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x000C9BC8 File Offset: 0x000C7DC8
		public void WriteInputSettings(InputSettings settings)
		{
			this.InputSettings = settings;
			PlayerPrefs.SetFloat("MouseSensitivity", settings.MouseSensitivity);
			PlayerPrefs.SetInt("InvertMouse", settings.InvertMouse ? 1 : 0);
			PlayerPrefs.SetInt("SprintMode", (int)settings.SprintMode);
			string value = this.GameInput.PlayerInput.actions.SaveBindingOverridesAsJson();
			PlayerPrefs.SetString("BindingOverrides", value);
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x000C9C34 File Offset: 0x000C7E34
		public InputSettings ReadInputSettings()
		{
			return new InputSettings
			{
				MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f),
				InvertMouse = (PlayerPrefs.GetInt("InvertMouse", 0) == 1),
				SprintMode = (InputSettings.EActionMode)PlayerPrefs.GetInt("SprintMode", 0),
				BindingOverrides = PlayerPrefs.GetString("BindingOverrides", this.GameInput.PlayerInput.actions.SaveBindingOverridesAsJson())
			};
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x000C9CA8 File Offset: 0x000C7EA8
		public string GetActionControlPath(string actionName)
		{
			InputAction inputAction = this.playerControls.FindAction(actionName, false);
			if (inputAction == null)
			{
				Console.LogError("Could not find action with name '" + actionName + "'", null);
				return string.Empty;
			}
			return inputAction.controls[0].path;
		}

		// Token: 0x040022A6 RID: 8870
		public const float MinYPos = -20f;

		// Token: 0x040022A8 RID: 8872
		public DisplaySettings DisplaySettings;

		// Token: 0x040022A9 RID: 8873
		public DisplaySettings UnappliedDisplaySettings;

		// Token: 0x040022AA RID: 8874
		public GraphicsSettings GraphicsSettings = new GraphicsSettings();

		// Token: 0x040022AB RID: 8875
		public AudioSettings AudioSettings = new AudioSettings();

		// Token: 0x040022AC RID: 8876
		public InputSettings InputSettings = new InputSettings();

		// Token: 0x040022AD RID: 8877
		public InputActionAsset InputActions;

		// Token: 0x040022AE RID: 8878
		public GameInput GameInput;

		// Token: 0x040022AF RID: 8879
		public ScriptableRendererFeature SSAO;

		// Token: 0x040022B0 RID: 8880
		public ScriptableRendererFeature GodRays;

		// Token: 0x040022B1 RID: 8881
		[Header("Camera")]
		public float LookSensitivity = 1f;

		// Token: 0x040022B2 RID: 8882
		public bool InvertMouse;

		// Token: 0x040022B3 RID: 8883
		public float CameraFOV = 75f;

		// Token: 0x040022B4 RID: 8884
		public InputSettings.EActionMode SprintMode = InputSettings.EActionMode.Hold;

		// Token: 0x040022B5 RID: 8885
		[Range(0f, 1f)]
		public float CameraBobIntensity = 1f;

		// Token: 0x040022B6 RID: 8886
		private InputActionMap playerControls;

		// Token: 0x040022B7 RID: 8887
		public Action onDisplayChanged;

		// Token: 0x040022B8 RID: 8888
		public Action onInputsApplied;

		// Token: 0x02000700 RID: 1792
		public enum UnitType
		{
			// Token: 0x040022BA RID: 8890
			Metric,
			// Token: 0x040022BB RID: 8891
			Imperial
		}
	}
}
