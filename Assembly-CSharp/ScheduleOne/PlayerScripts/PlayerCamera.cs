using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.FX;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005D6 RID: 1494
	public class PlayerCamera : PlayerSingleton<PlayerCamera>
	{
		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06002682 RID: 9858 RVA: 0x0009CA5A File Offset: 0x0009AC5A
		// (set) Token: 0x06002683 RID: 9859 RVA: 0x0009CA61 File Offset: 0x0009AC61
		public static ScheduleOne.DevUtilities.GraphicsSettings.EAntiAliasingMode AntiAliasingMode { get; private set; }

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06002684 RID: 9860 RVA: 0x0009CA69 File Offset: 0x0009AC69
		// (set) Token: 0x06002685 RID: 9861 RVA: 0x0009CA71 File Offset: 0x0009AC71
		public bool canLook { get; protected set; } = true;

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06002686 RID: 9862 RVA: 0x0009CA7A File Offset: 0x0009AC7A
		public int activeUIElementCount
		{
			get
			{
				return this.activeUIElements.Count;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06002687 RID: 9863 RVA: 0x0009CA87 File Offset: 0x0009AC87
		// (set) Token: 0x06002688 RID: 9864 RVA: 0x0009CA8F File Offset: 0x0009AC8F
		public bool transformOverriden { get; protected set; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06002689 RID: 9865 RVA: 0x0009CA98 File Offset: 0x0009AC98
		// (set) Token: 0x0600268A RID: 9866 RVA: 0x0009CAA0 File Offset: 0x0009ACA0
		public bool fovOverriden { get; protected set; }

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x0600268B RID: 9867 RVA: 0x0009CAA9 File Offset: 0x0009ACA9
		// (set) Token: 0x0600268C RID: 9868 RVA: 0x0009CAB1 File Offset: 0x0009ACB1
		public bool FreeCamEnabled { get; private set; }

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x0600268D RID: 9869 RVA: 0x0009CABA File Offset: 0x0009ACBA
		// (set) Token: 0x0600268E RID: 9870 RVA: 0x0009CAC2 File Offset: 0x0009ACC2
		public bool ViewingAvatar { get; private set; }

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x0600268F RID: 9871 RVA: 0x0009CACB File Offset: 0x0009ACCB
		// (set) Token: 0x06002690 RID: 9872 RVA: 0x0009CAD3 File Offset: 0x0009ACD3
		public PlayerCamera.ECameraMode CameraMode { get; protected set; }

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06002691 RID: 9873 RVA: 0x0009CADC File Offset: 0x0009ACDC
		// (set) Token: 0x06002692 RID: 9874 RVA: 0x0009CAE4 File Offset: 0x0009ACE4
		public bool MethVisuals { get; set; }

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06002693 RID: 9875 RVA: 0x0009CAED File Offset: 0x0009ACED
		// (set) Token: 0x06002694 RID: 9876 RVA: 0x0009CAF5 File Offset: 0x0009ACF5
		public bool CocaineVisuals { get; set; }

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06002695 RID: 9877 RVA: 0x0009CAFE File Offset: 0x0009ACFE
		// (set) Token: 0x06002696 RID: 9878 RVA: 0x0009CB06 File Offset: 0x0009AD06
		public float FovJitter { get; private set; }

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06002697 RID: 9879 RVA: 0x0009CB0F File Offset: 0x0009AD0F
		// (set) Token: 0x06002698 RID: 9880 RVA: 0x0009CB17 File Offset: 0x0009AD17
		public List<string> activeUIElements { get; protected set; } = new List<string>();

		// Token: 0x06002699 RID: 9881 RVA: 0x0009CB20 File Offset: 0x0009AD20
		protected override void Awake()
		{
			base.Awake();
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 100);
			this.ApplyAASettings();
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x0009CB8C File Offset: 0x0009AD8C
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			if (!IsOwner)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			this.Camera.enabled = true;
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x0009CBB0 File Offset: 0x0009ADB0
		protected override void Start()
		{
			base.Start();
			if (Singleton<Settings>.InstanceExists)
			{
				this.Camera.fieldOfView = Singleton<Settings>.Instance.CameraFOV;
			}
			if (GameObject.Find("GlobalVolume") != null)
			{
				this.globalVolume = GameObject.Find("GlobalVolume").GetComponent<Volume>();
				this.globalVolume.sharedProfile.TryGet<DepthOfField>(out this.DoF);
				this.DoF.active = false;
			}
			this.cameralocalPos_PriorOverride = base.transform.localPosition;
			Singleton<EnvironmentFX>.Instance.HeightFog.mainCamera = this.Camera;
			this.FoVChangeSmoother.Initialize();
			this.FoVChangeSmoother.SetDefault(0f);
			this.SmoothLookSmoother.Initialize();
			this.SmoothLookSmoother.SetDefault(0f);
			this.SmoothLookSmoother.SetSmoothingSpeed(0.5f);
			this.LockMouse();
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x0009CC9B File Offset: 0x0009AE9B
		private void PlayerSpawned()
		{
			Player.Local.onTased.AddListener(delegate()
			{
				this.StartCameraShake(1f, 2f, true);
			});
			Player.Local.onTasedEnd.AddListener(new UnityAction(this.StopCameraShake));
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x0009CCD3 File Offset: 0x0009AED3
		public static void SetAntialiasingMode(ScheduleOne.DevUtilities.GraphicsSettings.EAntiAliasingMode mode)
		{
			PlayerCamera.AntiAliasingMode = mode;
			if (PlayerSingleton<PlayerCamera>.Instance != null)
			{
				PlayerSingleton<PlayerCamera>.Instance.ApplyAASettings();
			}
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x0009CCF4 File Offset: 0x0009AEF4
		public void ApplyAASettings()
		{
			AntialiasingMode antialiasing;
			switch (PlayerCamera.AntiAliasingMode)
			{
			case ScheduleOne.DevUtilities.GraphicsSettings.EAntiAliasingMode.Off:
				antialiasing = AntialiasingMode.None;
				break;
			case ScheduleOne.DevUtilities.GraphicsSettings.EAntiAliasingMode.FXAA:
				antialiasing = AntialiasingMode.FastApproximateAntialiasing;
				break;
			case ScheduleOne.DevUtilities.GraphicsSettings.EAntiAliasingMode.SMAA:
				antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
				break;
			default:
				antialiasing = AntialiasingMode.None;
				break;
			}
			this.Camera.GetComponent<UniversalAdditionalCameraData>().antialiasing = antialiasing;
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x0009CD3C File Offset: 0x0009AF3C
		protected virtual void Update()
		{
			this.UpdateCameraBob();
			if (this.canLook)
			{
				this.RotateCamera();
			}
			if (this.MethVisuals)
			{
				this.MethRumble.VolumeMultiplier = Mathf.MoveTowards(this.MethRumble.VolumeMultiplier, 1f, Time.deltaTime * 0.5f);
				if (!this.MethRumble.isPlaying)
				{
					this.MethRumble.Play();
				}
			}
			else
			{
				this.MethRumble.VolumeMultiplier = Mathf.MoveTowards(this.MethRumble.VolumeMultiplier, 0f, Time.deltaTime * 0.5f);
				if (this.MethRumble.VolumeMultiplier == 0f && this.MethRumble.isPlaying)
				{
					this.MethRumble.Stop();
				}
			}
			if (this.FreeCamEnabled)
			{
				this.RotateFreeCam();
				this.UpdateFreeCamInput();
				this.MoveFreeCam();
			}
			if (Player.Local.Schizophrenic)
			{
				this.timeUntilNextSchizoVoice -= Time.deltaTime;
				if (this.timeUntilNextSchizoVoice <= 0f)
				{
					this.timeUntilNextSchizoVoice = UnityEngine.Random.Range(5f, 20f);
					this.SchizoVoices.VolumeMultiplier = UnityEngine.Random.Range(0.5f, 1f);
					this.SchizoVoices.PitchMultiplier = UnityEngine.Random.Range(0.4f, 1f);
					this.SchizoVoices.transform.localPosition = UnityEngine.Random.insideUnitSphere * 1f;
					this.SchizoVoices.Play();
				}
			}
			if (GameInput.GetButton(GameInput.ButtonCode.ViewAvatar))
			{
				if (!this.ViewingAvatar && this.activeUIElementCount == 0 && this.canLook && !GameInput.IsTyping)
				{
					this.ViewAvatar();
				}
				if (this.ViewingAvatar)
				{
					Vector3 worldPos = this.ViewAvatarCameraPosition.position;
					Vector3 vector = PlayerSingleton<PlayerMovement>.Instance.transform.TransformPoint(new Vector3(0f, this.GetTargetLocalY(), 0f));
					RaycastHit raycastHit;
					if (Physics.Raycast(vector, (this.ViewAvatarCameraPosition.position - vector).normalized, out raycastHit, Vector3.Distance(vector, this.ViewAvatarCameraPosition.position), 1 << LayerMask.NameToLayer("Default"), QueryTriggerInteraction.Ignore))
					{
						worldPos = raycastHit.point;
					}
					this.OverrideTransform(worldPos, this.ViewAvatarCameraPosition.rotation, 0f, true);
					base.transform.LookAt(Player.Local.Avatar.LowestSpine.transform);
				}
			}
			else if (this.ViewingAvatar)
			{
				this.StopViewingAvatar();
			}
			if ((this.FreeCamEnabled || Application.isEditor) && Input.GetKeyDown(KeyCode.F12))
			{
				this.Screenshot();
			}
			this.UpdateMovementEvents();
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x0009CFE1 File Offset: 0x0009B1E1
		private void Screenshot()
		{
			base.StartCoroutine(PlayerCamera.<Screenshot>g__Routine|95_0());
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x0009CFF0 File Offset: 0x0009B1F0
		protected virtual void LateUpdate()
		{
			if (this.Camera == null || base.transform == null)
			{
				return;
			}
			if (!PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				return;
			}
			if (!this.transformOverriden && this.ILerpCamera_Coroutine == null)
			{
				base.transform.localPosition = new Vector3(0f, this.GetTargetLocalY(), 0f);
			}
			if (!this.fovOverriden && this.ILerpCameraFOV_Coroutine == null)
			{
				float num = Singleton<Settings>.Instance.CameraFOV * (PlayerSingleton<PlayerMovement>.Instance.isSprinting ? this.SprintFoVBoost : 1f);
				if (this.MethVisuals)
				{
					this.FovJitter = Mathf.Lerp(this.FovJitter, UnityEngine.Random.Range(0f, 1f), Time.deltaTime * 10f);
				}
				else if (this.CocaineVisuals)
				{
					this.FovJitter = Mathf.Lerp(this.FovJitter, 1f, Time.deltaTime * 0.5f);
				}
				else
				{
					this.FovJitter = Mathf.Lerp(this.FovJitter, 0f, Time.deltaTime * 3f);
				}
				if (Player.Local.Schizophrenic)
				{
					this.schizoFoV = -Mathf.Lerp(this.schizoFoV, Mathf.Sin(Time.time * 0.5f) * 20f, Time.deltaTime);
				}
				else
				{
					this.schizoFoV = Mathf.Lerp(this.schizoFoV, 0f, Time.deltaTime);
				}
				num += this.FovJitter * 6f;
				num += this.schizoFoV;
				num += this.FoVChangeSmoother.CurrentValue;
				this.Camera.fieldOfView = Mathf.MoveTowards(this.Camera.fieldOfView, num, Time.deltaTime * this.FoVChangeRate);
			}
			this.Camera.transform.localPosition = this.cameraLocalPos;
			this.cameraLocalPos = Vector3.zero;
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x0009D1D4 File Offset: 0x0009B3D4
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (this.FreeCamEnabled && action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.SetFreeCam(false, true);
			}
			if (this.ViewingAvatar && action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.StopViewingAvatar();
			}
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x0009D228 File Offset: 0x0009B428
		public float GetTargetLocalY()
		{
			if (!PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				return 0f;
			}
			return PlayerSingleton<PlayerMovement>.Instance.Controller.height / 2f + this.cameraOffsetFromTop;
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x0009D253 File Offset: 0x0009B453
		public void SetCameraMode(PlayerCamera.ECameraMode mode)
		{
			this.CameraMode = mode;
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x0009D25C File Offset: 0x0009B45C
		private void RotateCamera()
		{
			float num = GameInput.MouseDelta.x * (Singleton<Settings>.InstanceExists ? Singleton<Settings>.Instance.LookSensitivity : 1f);
			float num2 = GameInput.MouseDelta.y * (Singleton<Settings>.InstanceExists ? Singleton<Settings>.Instance.LookSensitivity : 1f);
			if (Player.Local.Disoriented)
			{
				num2 = -num2;
			}
			if (Player.Local.Seizure)
			{
				Vector2 b = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
				this.seizureJitter = Vector2.Lerp(this.seizureJitter, b, Time.deltaTime * 10f);
				num += this.seizureJitter.x;
				num2 += this.seizureJitter.y;
			}
			if (Player.Local.Schizophrenic)
			{
				num += Mathf.Sin(Time.time * 0.4f) * 0.01f;
				num2 += Mathf.Sin(Time.time * 0.3f) * 0.01f;
			}
			if (this.SmoothLook)
			{
				this.mouseX = Mathf.Lerp(this.mouseX, num, this.SmoothLookSpeed * Time.deltaTime);
				this.mouseY = Mathf.Lerp(this.mouseY, num2, this.SmoothLookSpeed * Time.deltaTime);
			}
			else if (this.SmoothLookSmoother.CurrentValue <= 0.01f)
			{
				this.mouseX = num;
				this.mouseY = num2;
			}
			else
			{
				float num3 = Mathf.Lerp(50f, 1f, this.SmoothLookSmoother.CurrentValue);
				this.mouseX = Mathf.Lerp(this.mouseX, num, num3 * Time.deltaTime);
				this.mouseY = Mathf.Lerp(this.mouseY, num2, num3 * Time.deltaTime);
			}
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			Vector3 eulerAngles2 = Player.Local.transform.rotation.eulerAngles;
			if (Singleton<Settings>.InstanceExists && Singleton<Settings>.Instance.InvertMouse)
			{
				this.mouseY = -this.mouseY;
			}
			this.mouseX += this.focusMouseX;
			this.mouseY += this.focusMouseY;
			eulerAngles.x -= Mathf.Clamp(this.mouseY, -89f, 89f);
			eulerAngles2.y += this.mouseX;
			eulerAngles.z = 0f;
			if (eulerAngles.x >= 180f)
			{
				if (eulerAngles.x < 271f)
				{
					eulerAngles.x = 271f;
				}
			}
			else if (eulerAngles.x > 89f)
			{
				eulerAngles.x = 89f;
			}
			base.transform.localRotation = Quaternion.Euler(eulerAngles);
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, 0f, 0f);
			Player.Local.transform.rotation = Quaternion.Euler(eulerAngles2);
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x0009D563 File Offset: 0x0009B763
		public void LockMouse()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			if (Singleton<HUD>.InstanceExists)
			{
				Singleton<HUD>.Instance.SetCrosshairVisible(true);
			}
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x0009D583 File Offset: 0x0009B783
		public void FreeMouse()
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			if (Singleton<HUD>.InstanceExists)
			{
				Singleton<HUD>.Instance.SetCrosshairVisible(false);
			}
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x0009D5A4 File Offset: 0x0009B7A4
		public bool LookRaycast(float range, out RaycastHit hit, LayerMask layerMask, bool includeTriggers = true, float radius = 0f)
		{
			if (radius == 0f)
			{
				return Physics.Raycast(base.transform.position, base.transform.forward, out hit, range, layerMask, includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
			}
			return Physics.SphereCast(base.transform.position, radius, base.transform.forward, out hit, range, layerMask, includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x0009D614 File Offset: 0x0009B814
		public bool LookRaycast_ExcludeBuildables(float range, out RaycastHit hit, LayerMask layerMask, bool includeTriggers = true)
		{
			RaycastHit[] array = Physics.RaycastAll(base.transform.position, base.transform.forward, range, layerMask, includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
			RaycastHit raycastHit = default(RaycastHit);
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].collider.GetComponentInParent<BuildableItem>() && (raycastHit.collider == null || Vector3.Distance(base.transform.position, array[i].point) < Vector3.Distance(base.transform.position, raycastHit.point)))
				{
					raycastHit = array[i];
				}
			}
			if (raycastHit.collider != null)
			{
				hit = raycastHit;
				return true;
			}
			hit = default(RaycastHit);
			return false;
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x0009D6E8 File Offset: 0x0009B8E8
		private void OnDrawGizmosSelected()
		{
			for (int i = 0; i < this.gizmos.Count; i++)
			{
				Gizmos.DrawSphere(this.gizmos[i], 0.05f);
			}
			this.gizmos.Clear();
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x0009D72C File Offset: 0x0009B92C
		public bool Raycast_ExcludeBuildables(Vector3 origin, Vector3 direction, float range, out RaycastHit hit, LayerMask layerMask, bool includeTriggers = false, float radius = 0f, float maxAngleDifference = 0f)
		{
			RaycastHit[] array;
			if (radius == 0f)
			{
				array = Physics.RaycastAll(origin, direction, range, layerMask, includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
			}
			else
			{
				array = Physics.SphereCastAll(origin, radius, direction, range, layerMask, includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
			}
			RaycastHit raycastHit = default(RaycastHit);
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i].point == Vector3.zero) && !array[i].collider.GetComponentInParent<BuildableItem>() && (maxAngleDifference == 0f || Vector3.Angle(direction, -array[i].normal) < maxAngleDifference) && (raycastHit.collider == null || Vector3.Distance(base.transform.position, array[i].point) < Vector3.Distance(base.transform.position, raycastHit.point)))
				{
					raycastHit = array[i];
				}
			}
			if (raycastHit.collider != null)
			{
				hit = raycastHit;
				return true;
			}
			hit = default(RaycastHit);
			return false;
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x0009D860 File Offset: 0x0009BA60
		public bool MouseRaycast(float range, out RaycastHit hit, LayerMask layerMask, bool includeTriggers = true, float radius = 0f)
		{
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			if (radius == 0f)
			{
				return Physics.Raycast(ray, out hit, range, layerMask, includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
			}
			return Physics.SphereCast(ray, radius, out hit, range, layerMask, includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x0009D8BA File Offset: 0x0009BABA
		public bool LookSpherecast(float range, float radius, out RaycastHit hit, LayerMask layerMask)
		{
			return Physics.SphereCast(base.transform.position, radius, base.transform.forward, out hit, range, layerMask);
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x0009D8E4 File Offset: 0x0009BAE4
		public void OverrideTransform(Vector3 worldPos, Quaternion rot, float lerpTime, bool keepParented = false)
		{
			this.canLook = false;
			if (this.ILerpCamera_Coroutine != null)
			{
				base.StopCoroutine(this.ILerpCamera_Coroutine);
				this.ILerpCamera_Coroutine = null;
			}
			else if (!this.transformOverriden)
			{
				this.cameralocalPos_PriorOverride = base.transform.localPosition;
				this.cameraLocalRot_PriorOverride = base.transform.localRotation;
			}
			this.transformOverriden = true;
			if (!keepParented)
			{
				base.transform.SetParent(null);
			}
			this.ILerpCamera_Coroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.ILerpCamera(worldPos, rot, lerpTime, true, false, false));
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x0009D972 File Offset: 0x0009BB72
		protected IEnumerator ILerpCamera(Vector3 endPos, Quaternion endRot, float lerpTime, bool worldSpace, bool returnToRestingPosition = false, bool reenableLook = false)
		{
			Vector3 startPos = base.transform.localPosition;
			Quaternion startRot = base.transform.rotation;
			if (worldSpace)
			{
				startPos = base.transform.position;
			}
			float elapsed = 0f;
			while (elapsed < lerpTime)
			{
				if (returnToRestingPosition)
				{
					base.transform.localPosition = Vector3.Lerp(startPos, new Vector3(0f, this.GetTargetLocalY(), 0f), elapsed / lerpTime);
				}
				else if (worldSpace)
				{
					base.transform.position = Vector3.Lerp(startPos, endPos, elapsed / lerpTime);
				}
				else
				{
					base.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / lerpTime);
				}
				base.transform.rotation = Quaternion.Lerp(startRot, endRot, elapsed / lerpTime);
				elapsed += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			if (returnToRestingPosition)
			{
				base.transform.localPosition = new Vector3(0f, this.GetTargetLocalY(), 0f);
			}
			else if (worldSpace)
			{
				base.transform.position = endPos;
			}
			else
			{
				base.transform.localPosition = endPos;
			}
			if (reenableLook)
			{
				this.SetCanLook(true);
			}
			base.transform.rotation = endRot;
			this.ILerpCamera_Coroutine = null;
			yield break;
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x0009D9B0 File Offset: 0x0009BBB0
		public void StopTransformOverride(float lerpTime, bool reenableCameraLook = true, bool returnToOriginalRotation = true)
		{
			if (this.blockNextStopTransformOverride)
			{
				this.blockNextStopTransformOverride = false;
				return;
			}
			if (this.ILerpCamera_Coroutine != null)
			{
				base.StopCoroutine(this.ILerpCamera_Coroutine);
				this.ILerpCamera_Coroutine = null;
			}
			this.transformOverriden = false;
			base.transform.SetParent(PlayerSingleton<PlayerMovement>.Instance.transform);
			if (this.ILerpCamera_Coroutine != null)
			{
				base.StopCoroutine(this.ILerpCamera_Coroutine);
			}
			Quaternion quaternion = PlayerSingleton<PlayerMovement>.Instance.transform.rotation * this.cameraLocalRot_PriorOverride;
			if (!returnToOriginalRotation)
			{
				quaternion = base.transform.rotation;
			}
			if (lerpTime == 0f)
			{
				base.transform.rotation = quaternion;
				base.transform.localPosition = new Vector3(0f, this.GetTargetLocalY(), 0f);
				if (reenableCameraLook)
				{
					this.SetCanLook_True();
					return;
				}
			}
			else
			{
				this.ILerpCamera_Coroutine = base.StartCoroutine(this.ILerpCamera(this.cameralocalPos_PriorOverride, quaternion, lerpTime, false, true, reenableCameraLook));
			}
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x0009DAA0 File Offset: 0x0009BCA0
		public void LookAt(Vector3 point, float duration = 0.25f)
		{
			PlayerCamera.<>c__DisplayClass117_0 CS$<>8__locals1 = new PlayerCamera.<>c__DisplayClass117_0();
			CS$<>8__locals1.point = point;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.duration = duration;
			if (this.lookRoutine != null)
			{
				base.StopCoroutine(this.lookRoutine);
			}
			base.StartCoroutine(CS$<>8__locals1.<LookAt>g__Look|0());
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x0009DAE9 File Offset: 0x0009BCE9
		private void SetCanLook_True()
		{
			this.SetCanLook(true);
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x0009DAF2 File Offset: 0x0009BCF2
		public void SetCanLook(bool c)
		{
			this.canLook = c;
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x0009DAFB File Offset: 0x0009BCFB
		public void SetDoFActive(bool active, float lerpTime)
		{
			if (this.DoFCoroutine != null)
			{
				base.StopCoroutine(this.DoFCoroutine);
			}
			this.DoFCoroutine = base.StartCoroutine(this.LerpDoF(active, lerpTime));
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x0009DB25 File Offset: 0x0009BD25
		private IEnumerator LerpDoF(bool active, float lerpTime)
		{
			if (active)
			{
				this.DoF.active = true;
			}
			float startFocusDist = this.DoF.focusDistance.value;
			float endFocusDist = 0f;
			if (active)
			{
				endFocusDist = 0.1f;
			}
			else
			{
				endFocusDist = 5f;
			}
			for (float i = 0f; i < lerpTime; i += Time.unscaledDeltaTime)
			{
				this.DoF.focusDistance.value = Mathf.Lerp(startFocusDist, endFocusDist, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.DoF.focusDistance.value = endFocusDist;
			if (!active)
			{
				this.DoF.active = false;
			}
			this.DoFCoroutine = null;
			yield break;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x0009DB44 File Offset: 0x0009BD44
		public void OverrideFOV(float fov, float lerpTime)
		{
			if (this.ILerpCameraFOV_Coroutine != null)
			{
				base.StopCoroutine(this.ILerpCameraFOV_Coroutine);
			}
			this.fovOverriden = true;
			if (fov == -1f)
			{
				fov = Singleton<Settings>.Instance.CameraFOV;
			}
			this.ILerpCameraFOV_Coroutine = base.StartCoroutine(this.ILerpFOV(fov, lerpTime));
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0009DB94 File Offset: 0x0009BD94
		protected IEnumerator ILerpFOV(float endFov, float lerpTime)
		{
			float startFov = this.Camera.fieldOfView;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.Camera.fieldOfView = Mathf.Lerp(startFov, endFov, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.Camera.fieldOfView = endFov;
			this.ILerpCameraFOV_Coroutine = null;
			yield break;
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x0009DBB1 File Offset: 0x0009BDB1
		public void StopFOVOverride(float lerpTime)
		{
			this.OverrideFOV(-1f, lerpTime);
			this.fovOverriden = false;
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x0009DBC6 File Offset: 0x0009BDC6
		public void AddActiveUIElement(string name)
		{
			if (!this.activeUIElements.Contains(name))
			{
				this.activeUIElements.Add(name);
			}
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x0009DBE2 File Offset: 0x0009BDE2
		public void RemoveActiveUIElement(string name)
		{
			if (this.activeUIElements.Contains(name))
			{
				this.activeUIElements.Remove(name);
			}
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x0009DC00 File Offset: 0x0009BE00
		public void RegisterMovementEvent(int threshold, Action action)
		{
			if (threshold < 1)
			{
				Console.LogWarning("Movement events min. threshold is 1m!", null);
				return;
			}
			if (!this.movementEvents.ContainsKey(threshold))
			{
				this.movementEvents.Add(threshold, new PlayerMovement.MovementEvent());
			}
			this.movementEvents[threshold].actions.Add(action);
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x0009DC54 File Offset: 0x0009BE54
		public void DeregisterMovementEvent(Action action)
		{
			foreach (int key in this.movementEvents.Keys)
			{
				PlayerMovement.MovementEvent movementEvent = this.movementEvents[key];
				if (movementEvent.actions.Contains(action))
				{
					movementEvent.actions.Remove(action);
					break;
				}
			}
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x0009DCD0 File Offset: 0x0009BED0
		private void UpdateMovementEvents()
		{
			foreach (int num in this.movementEvents.Keys.ToList<int>())
			{
				PlayerMovement.MovementEvent movementEvent = this.movementEvents[num];
				if (Vector3.Distance(base.transform.position, movementEvent.LastUpdatedDistance) > (float)num)
				{
					movementEvent.Update(base.transform.position);
				}
			}
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x0009DD60 File Offset: 0x0009BF60
		private void ViewAvatar()
		{
			this.ViewingAvatar = true;
			this.AddActiveUIElement("View avatar");
			Vector3 worldPos = this.ViewAvatarCameraPosition.position;
			Vector3 vector = PlayerSingleton<PlayerMovement>.Instance.transform.TransformPoint(new Vector3(0f, this.GetTargetLocalY(), 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(vector, (this.ViewAvatarCameraPosition.position - vector).normalized, out raycastHit, Vector3.Distance(vector, this.ViewAvatarCameraPosition.position), 1 << LayerMask.NameToLayer("Default"), QueryTriggerInteraction.Ignore))
			{
				worldPos = raycastHit.point;
			}
			this.OverrideTransform(worldPos, this.ViewAvatarCameraPosition.rotation, 0f, true);
			base.transform.LookAt(Player.Local.Avatar.LowestSpine.transform);
			Singleton<HUD>.Instance.canvas.enabled = false;
			PlayerSingleton<PlayerInventory>.Instance.SetViewmodelVisible(false);
			Player.Local.SetVisibleToLocalPlayer(true);
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x0009DE5C File Offset: 0x0009C05C
		private void StopViewingAvatar()
		{
			this.ViewingAvatar = false;
			this.RemoveActiveUIElement("View avatar");
			this.StopTransformOverride(0f, true, true);
			Singleton<HUD>.Instance.canvas.enabled = true;
			PlayerSingleton<PlayerInventory>.Instance.SetViewmodelVisible(true);
			Player.Local.SetVisibleToLocalPlayer(false);
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x0009DEB0 File Offset: 0x0009C0B0
		public void JoltCamera()
		{
			AnimationClip animationClip = this.JoltClips[UnityEngine.Random.Range(0, this.JoltClips.Length)];
			this.Animator.Play(animationClip.name, 0, 0f);
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x0009DEEC File Offset: 0x0009C0EC
		public bool PointInCameraView(Vector3 point)
		{
			Vector3 vector = this.Camera.WorldToViewportPoint(point);
			bool flag = this.Is01(vector.x) && this.Is01(vector.y);
			bool flag2 = vector.z > 0f;
			bool flag3 = false;
			Vector3 normalized = (point - this.Camera.transform.position).normalized;
			float num = Vector3.Distance(this.Camera.transform.position, point);
			RaycastHit raycastHit;
			if (Physics.Raycast(this.Camera.transform.position, normalized, out raycastHit, num + 0.05f, 1 << LayerMask.NameToLayer("Default")) && raycastHit.point != point)
			{
				flag3 = true;
			}
			return flag && flag2 && !flag3;
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x0009DFB9 File Offset: 0x0009C1B9
		public bool Is01(float a)
		{
			return a > 0f && a < 1f;
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x0009DFCD File Offset: 0x0009C1CD
		public void ResetRotation()
		{
			base.transform.localRotation = Quaternion.identity;
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x0009DFE0 File Offset: 0x0009C1E0
		public void FocusCameraOnTarget(Transform target)
		{
			PlayerCamera.<>c__DisplayClass138_0 CS$<>8__locals1 = new PlayerCamera.<>c__DisplayClass138_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.target = target;
			if (this.focusRoutine != null)
			{
				base.StopCoroutine(this.focusRoutine);
			}
			this.focusRoutine = base.StartCoroutine(CS$<>8__locals1.<FocusCameraOnTarget>g__FocusRoutine|0());
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x0009E027 File Offset: 0x0009C227
		public void StopFocus()
		{
			if (this.focusRoutine != null)
			{
				base.StopCoroutine(this.focusRoutine);
			}
			this.focusMouseX = 0f;
			this.focusMouseY = 0f;
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x0009E054 File Offset: 0x0009C254
		public void StartCameraShake(float intensity, float duration = -1f, bool decreaseOverTime = true)
		{
			PlayerCamera.<>c__DisplayClass140_0 CS$<>8__locals1 = new PlayerCamera.<>c__DisplayClass140_0();
			CS$<>8__locals1.duration = duration;
			CS$<>8__locals1.intensity = intensity;
			CS$<>8__locals1.decreaseOverTime = decreaseOverTime;
			CS$<>8__locals1.<>4__this = this;
			this.StopCameraShake();
			this.cameraShakeCoroutine = base.StartCoroutine(CS$<>8__locals1.<StartCameraShake>g__Shake|0());
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x0009E09B File Offset: 0x0009C29B
		public void StopCameraShake()
		{
			if (this.cameraShakeCoroutine != null)
			{
				base.StopCoroutine(this.cameraShakeCoroutine);
				this.Camera.transform.localPosition = Vector3.zero;
			}
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x0009E0C8 File Offset: 0x0009C2C8
		public void UpdateCameraBob()
		{
			float num = 1f;
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				num = PlayerSingleton<PlayerMovement>.Instance.CurrentSprintMultiplier - 1f;
			}
			num *= Singleton<Settings>.Instance.CameraBobIntensity;
			this.cameraLocalPos.x = this.cameraLocalPos.x + this.HorizontalBobCurve.Evaluate(Time.time * this.BobRate % 1f) * num * this.HorizontalCameraBob;
			this.cameraLocalPos.y = this.cameraLocalPos.y + this.VerticalBobCurve.Evaluate(Time.time * this.BobRate % 1f) * num * this.VerticalCameraBob;
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x0009E16C File Offset: 0x0009C36C
		public void SetFreeCam(bool enable, bool reenableLook = true)
		{
			this.FreeCamEnabled = enable;
			Singleton<HUD>.Instance.canvas.enabled = !enable;
			PlayerSingleton<PlayerMovement>.Instance.canMove = !enable;
			Player.Local.SetVisibleToLocalPlayer(enable);
			if (enable)
			{
				this.OverrideTransform(base.transform.position, base.transform.rotation, 0f, false);
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				return;
			}
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.StopTransformOverride(0f, reenableLook, true);
			this.freeCamMovement = Vector3.zero;
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x0009E20C File Offset: 0x0009C40C
		private void RotateFreeCam()
		{
			this.mouseX = Mathf.Lerp(this.mouseX, GameInput.MouseDelta.x * (Singleton<Settings>.InstanceExists ? Singleton<Settings>.Instance.LookSensitivity : 1f), this.SmoothLookSpeed * Time.deltaTime);
			this.mouseY = Mathf.Lerp(this.mouseY, GameInput.MouseDelta.y * (Singleton<Settings>.InstanceExists ? Singleton<Settings>.Instance.LookSensitivity : 1f), this.SmoothLookSpeed * Time.deltaTime);
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			Vector3 eulerAngles2 = base.transform.localRotation.eulerAngles;
			if (Singleton<Settings>.InstanceExists && Singleton<Settings>.Instance.InvertMouse)
			{
				this.mouseY = -this.mouseY;
			}
			eulerAngles.x -= Mathf.Clamp(this.mouseY, -89f, 89f);
			eulerAngles.y += this.mouseX;
			eulerAngles.z = 0f;
			if (eulerAngles.x >= 180f)
			{
				if (eulerAngles.x < 271f)
				{
					eulerAngles.x = 271f;
				}
			}
			else if (eulerAngles.x > 89f)
			{
				eulerAngles.x = 89f;
			}
			base.transform.localRotation = Quaternion.Euler(eulerAngles);
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, 0f);
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x0009E3A8 File Offset: 0x0009C5A8
		private void UpdateFreeCamInput()
		{
			int num = Mathf.RoundToInt(GameInput.MotionAxis.x);
			int num2 = Mathf.RoundToInt(GameInput.MotionAxis.y);
			int num3 = 0;
			if (GameInput.GetButton(GameInput.ButtonCode.Jump))
			{
				num3 = 1;
			}
			else if (GameInput.GetButton(GameInput.ButtonCode.Crouch))
			{
				num3 = -1;
			}
			if (GameInput.IsTyping)
			{
				num = 0;
				num2 = 0;
				num3 = 0;
			}
			this.freeCamSpeed += Input.mouseScrollDelta.y * Time.deltaTime;
			this.freeCamSpeed = Mathf.Clamp(this.freeCamSpeed, 0f, 10f);
			this.freeCamMovement = new Vector3(Mathf.MoveTowards(this.freeCamMovement.x, (float)num, Time.unscaledDeltaTime * this.FreeCamAcceleration), Mathf.MoveTowards(this.freeCamMovement.y, (float)num3, Time.unscaledDeltaTime * this.FreeCamAcceleration), Mathf.MoveTowards(this.freeCamMovement.z, (float)num2, Time.unscaledDeltaTime * this.FreeCamAcceleration));
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x0009E498 File Offset: 0x0009C698
		private void MoveFreeCam()
		{
			base.transform.position += base.transform.TransformVector(this.freeCamMovement) * this.FreeCamSpeed * this.freeCamSpeed * Time.unscaledDeltaTime * (GameInput.GetButton(GameInput.ButtonCode.Sprint) ? 3f : 1f);
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x0009E606 File Offset: 0x0009C806
		[CompilerGenerated]
		internal static IEnumerator <Screenshot>g__Routine|95_0()
		{
			yield return new WaitForEndOfFrame();
			string text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			text = Path.Combine(text, "Screenshot_" + DateTime.Now.ToString("HH-mm-ss") + ".png");
			Console.Log("Screenshot saved to: " + text, null);
			ScreenCapture.CaptureScreenshot(text, 2);
			yield return new WaitForEndOfFrame();
			yield break;
		}

		// Token: 0x04001BF5 RID: 7157
		public const float CAMERA_SHAKE_MULTIPLIER = 0.1f;

		// Token: 0x04001BF7 RID: 7159
		[Header("Settings")]
		public float cameraOffsetFromTop = -0.15f;

		// Token: 0x04001BF8 RID: 7160
		public float SprintFoVBoost = 1.15f;

		// Token: 0x04001BF9 RID: 7161
		public float FoVChangeRate = 4f;

		// Token: 0x04001BFA RID: 7162
		public float HorizontalCameraBob = 1f;

		// Token: 0x04001BFB RID: 7163
		public float VerticalCameraBob = 1f;

		// Token: 0x04001BFC RID: 7164
		public float BobRate = 10f;

		// Token: 0x04001BFD RID: 7165
		public AnimationCurve HorizontalBobCurve;

		// Token: 0x04001BFE RID: 7166
		public AnimationCurve VerticalBobCurve;

		// Token: 0x04001BFF RID: 7167
		public float FreeCamSpeed = 1f;

		// Token: 0x04001C00 RID: 7168
		public float FreeCamAcceleration = 2f;

		// Token: 0x04001C01 RID: 7169
		public bool SmoothLook;

		// Token: 0x04001C02 RID: 7170
		public float SmoothLookSpeed = 1f;

		// Token: 0x04001C03 RID: 7171
		public FloatSmoother FoVChangeSmoother;

		// Token: 0x04001C04 RID: 7172
		public FloatSmoother SmoothLookSmoother;

		// Token: 0x04001C05 RID: 7173
		[Header("References")]
		public Transform CameraContainer;

		// Token: 0x04001C06 RID: 7174
		public Camera Camera;

		// Token: 0x04001C07 RID: 7175
		public Animator Animator;

		// Token: 0x04001C08 RID: 7176
		public AnimationClip[] JoltClips;

		// Token: 0x04001C09 RID: 7177
		public UniversalRenderPipelineAsset[] URPAssets;

		// Token: 0x04001C0A RID: 7178
		public Transform ViewAvatarCameraPosition;

		// Token: 0x04001C0B RID: 7179
		public HeartbeatSoundController HeartbeatSoundController;

		// Token: 0x04001C0C RID: 7180
		public ParticleSystem Flies;

		// Token: 0x04001C0D RID: 7181
		public AudioSourceController MethRumble;

		// Token: 0x04001C0E RID: 7182
		public RandomizedAudioSourceController SchizoVoices;

		// Token: 0x04001C12 RID: 7186
		[HideInInspector]
		public bool blockNextStopTransformOverride;

		// Token: 0x04001C19 RID: 7193
		private Volume globalVolume;

		// Token: 0x04001C1A RID: 7194
		private DepthOfField DoF;

		// Token: 0x04001C1C RID: 7196
		private Coroutine cameraShakeCoroutine;

		// Token: 0x04001C1D RID: 7197
		private Vector3 cameraLocalPos = Vector3.zero;

		// Token: 0x04001C1E RID: 7198
		private Vector3 freeCamMovement = Vector3.zero;

		// Token: 0x04001C1F RID: 7199
		private Coroutine focusRoutine;

		// Token: 0x04001C20 RID: 7200
		private float focusMouseX;

		// Token: 0x04001C21 RID: 7201
		private float focusMouseY;

		// Token: 0x04001C22 RID: 7202
		private Dictionary<int, PlayerMovement.MovementEvent> movementEvents = new Dictionary<int, PlayerMovement.MovementEvent>();

		// Token: 0x04001C23 RID: 7203
		private float freeCamSpeed = 1f;

		// Token: 0x04001C24 RID: 7204
		private float mouseX;

		// Token: 0x04001C25 RID: 7205
		private float mouseY;

		// Token: 0x04001C26 RID: 7206
		private Vector2 seizureJitter = Vector2.zero;

		// Token: 0x04001C27 RID: 7207
		private float schizoFoV;

		// Token: 0x04001C28 RID: 7208
		private float timeUntilNextSchizoVoice = 15f;

		// Token: 0x04001C29 RID: 7209
		private List<Vector3> gizmos = new List<Vector3>();

		// Token: 0x04001C2A RID: 7210
		private Vector3 cameralocalPos_PriorOverride = Vector3.zero;

		// Token: 0x04001C2B RID: 7211
		private Quaternion cameraLocalRot_PriorOverride = Quaternion.identity;

		// Token: 0x04001C2C RID: 7212
		public Coroutine ILerpCamera_Coroutine;

		// Token: 0x04001C2D RID: 7213
		private Coroutine lookRoutine;

		// Token: 0x04001C2E RID: 7214
		private Coroutine DoFCoroutine;

		// Token: 0x04001C2F RID: 7215
		private Coroutine ILerpCameraFOV_Coroutine;

		// Token: 0x020005D7 RID: 1495
		public enum ECameraMode
		{
			// Token: 0x04001C31 RID: 7217
			Default,
			// Token: 0x04001C32 RID: 7218
			Vehicle,
			// Token: 0x04001C33 RID: 7219
			Skateboard
		}
	}
}
