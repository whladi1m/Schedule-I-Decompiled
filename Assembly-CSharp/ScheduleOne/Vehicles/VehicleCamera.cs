using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007BA RID: 1978
	public class VehicleCamera : MonoBehaviour
	{
		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06003625 RID: 13861 RVA: 0x00044D66 File Offset: 0x00042F66
		private Transform cam
		{
			get
			{
				return PlayerSingleton<PlayerCamera>.Instance.transform;
			}
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000E3D34 File Offset: 0x000E1F34
		protected virtual void Start()
		{
			this.targetTransform = new GameObject("VehicleCameraTargetTransform").transform;
			this.targetTransform.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
			this.cameraDolly = new GameObject("VehicleCameraDolly").transform;
			this.cameraDolly.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
			if (Player.Local != null)
			{
				this.Subscribe();
				return;
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.Subscribe));
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x000E3DC9 File Offset: 0x000E1FC9
		private void Subscribe()
		{
			Player local = Player.Local;
			local.onEnterVehicle = (Player.VehicleEvent)Delegate.Combine(local.onEnterVehicle, new Player.VehicleEvent(this.PlayerEnteredVehicle));
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x000E3DF1 File Offset: 0x000E1FF1
		protected virtual void Update()
		{
			this.timeSinceCameraManuallyAdjusted += Time.deltaTime;
			this.CheckForClick();
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x000E3E0C File Offset: 0x000E200C
		private void PlayerEnteredVehicle(LandVehicle veh)
		{
			if (veh != this.vehicle)
			{
				return;
			}
			this.timeSinceCameraManuallyAdjusted = 100f;
			this.LateUpdate();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.targetTransform.position, this.targetTransform.rotation, 0f, false);
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x000E3E60 File Offset: 0x000E2060
		private void CheckForClick()
		{
			if (this.vehicle.localPlayerIsInVehicle && GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
			{
				if (GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick) && this.timeSinceCameraManuallyAdjusted > 0.01f)
				{
					Vector3 eulerAngles = this.cam.rotation.eulerAngles;
					this.x = eulerAngles.y;
					this.y = eulerAngles.x;
					this.orbitDistance = Mathf.Sqrt(Mathf.Pow(this.lateralOffset, 2f) + Mathf.Pow(this.verticalOffset, 2f));
				}
				this.timeSinceCameraManuallyAdjusted = 0f;
			}
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x000E3F00 File Offset: 0x000E2100
		protected virtual void LateUpdate()
		{
			if (this.vehicle.localPlayerIsInVehicle)
			{
				if (this.vehicle.speed_Kmh > 2f)
				{
					this.cameraReversed = false;
				}
				else if (this.vehicle.speed_Kmh < -15f)
				{
					this.cameraReversed = true;
				}
				this.targetTransform.position = this.LimitCameraPosition(this.GetTargetCameraPosition());
				this.targetTransform.LookAt(this.cameraOrigin);
				this.cameraDolly.position = Vector3.Lerp(this.cameraDolly.position, this.targetTransform.position, Time.deltaTime * 10f);
				this.cameraDolly.rotation = Quaternion.Lerp(this.cameraDolly.rotation, this.targetTransform.rotation, Time.deltaTime * 10f);
				this.orbitDistance = Mathf.Clamp(Vector3.Distance(this.cameraOrigin.position, this.cameraDolly.position), Mathf.Sqrt(Mathf.Pow(this.lateralOffset, 2f) + Mathf.Pow(this.verticalOffset, 2f)), 100f);
				if (this.timeSinceCameraManuallyAdjusted <= 0.01f)
				{
					if (GameInput.GetButton(GameInput.ButtonCode.SecondaryClick))
					{
						this.x += GameInput.MouseDelta.x * 60f * 0.02f * Singleton<Settings>.Instance.LookSensitivity;
						this.y -= GameInput.MouseDelta.y * 40f * 0.02f * Singleton<Settings>.Instance.LookSensitivity;
						this.y = VehicleCamera.ClampAngle(this.y, -20f, 89f);
						Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
						Vector3 targetPosition = rotation * new Vector3(0f, 0f, -this.orbitDistance) + this.cameraOrigin.position;
						this.cam.rotation = rotation;
						this.cam.position = this.LimitCameraPosition(targetPosition);
					}
					else
					{
						Vector3 normalized = (this.cameraOrigin.TransformPoint(this.lastFrameCameraOffset) - this.cameraOrigin.position).normalized;
						Vector3 targetPosition2 = this.cameraOrigin.position + normalized * this.orbitDistance;
						this.cam.position = this.LimitCameraPosition(targetPosition2);
						this.cam.LookAt(this.cameraOrigin);
						Vector3 eulerAngles = this.cam.rotation.eulerAngles;
						this.x = eulerAngles.y;
						this.y = eulerAngles.x;
					}
					this.lastManualOffset = this.cameraOrigin.InverseTransformPoint(this.cam.position);
				}
				else if (this.timeSinceCameraManuallyAdjusted < 0.61f)
				{
					this.targetTransform.position = Vector3.Lerp(this.cameraOrigin.TransformPoint(this.lastManualOffset), this.targetTransform.position, (this.timeSinceCameraManuallyAdjusted - 0.01f) / 0.6f);
					this.targetTransform.LookAt(this.cameraOrigin);
					this.cam.position = Vector3.Lerp(this.cam.position, this.targetTransform.position, Time.deltaTime * 10f);
					this.cam.rotation = Quaternion.Lerp(this.cam.rotation, this.targetTransform.rotation, Time.deltaTime * 10f);
				}
				else
				{
					this.cam.position = Vector3.Lerp(this.cam.position, this.targetTransform.position, Time.deltaTime * 10f);
					this.cam.rotation = Quaternion.Lerp(this.cam.rotation, this.targetTransform.rotation, Time.deltaTime * 10f);
				}
				this.lastFrameCameraOffset = this.cameraOrigin.InverseTransformPoint(this.cam.position);
			}
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x00007EE5 File Offset: 0x000060E5
		private static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000E4324 File Offset: 0x000E2524
		private Vector3 GetTargetCameraPosition()
		{
			Vector3 a = -base.transform.forward;
			a.y = 0f;
			a.Normalize();
			if (this.cameraReversed)
			{
				a *= -1f;
			}
			return base.transform.position + a * this.lateralOffset + Vector3.up * this.verticalOffset;
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000E439C File Offset: 0x000E259C
		private Vector3 LimitCameraPosition(Vector3 targetPosition)
		{
			Vector3 vector = targetPosition;
			default(LayerMask) | 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Terrain");
			float num = 0.45f;
			Vector3 vector2 = Vector3.Normalize(vector - this.cameraOrigin.position);
			RaycastHit raycastHit;
			if (Physics.Raycast(this.cameraOrigin.position, vector2, out raycastHit, Vector3.Distance(base.transform.position, vector) + num, 1 << LayerMask.NameToLayer("Default")))
			{
				vector = raycastHit.point - vector2 * num;
			}
			return vector;
		}

		// Token: 0x040026F1 RID: 9969
		private const float followDelta = 10f;

		// Token: 0x040026F2 RID: 9970
		private const float yMinLimit = -20f;

		// Token: 0x040026F3 RID: 9971
		private const float manualOverrideTime = 0.01f;

		// Token: 0x040026F4 RID: 9972
		private const float manualOverrideReturnTime = 0.6f;

		// Token: 0x040026F5 RID: 9973
		private const float xSpeed = 60f;

		// Token: 0x040026F6 RID: 9974
		private const float ySpeed = 40f;

		// Token: 0x040026F7 RID: 9975
		private const float yMaxLimit = 89f;

		// Token: 0x040026F8 RID: 9976
		[Header("References")]
		public LandVehicle vehicle;

		// Token: 0x040026F9 RID: 9977
		[Header("Camera Settings")]
		[SerializeField]
		protected Transform cameraOrigin;

		// Token: 0x040026FA RID: 9978
		[SerializeField]
		protected float lateralOffset = 4f;

		// Token: 0x040026FB RID: 9979
		[SerializeField]
		protected float verticalOffset = 1.5f;

		// Token: 0x040026FC RID: 9980
		protected bool cameraReversed;

		// Token: 0x040026FD RID: 9981
		protected float timeSinceCameraManuallyAdjusted = float.MaxValue;

		// Token: 0x040026FE RID: 9982
		protected float orbitDistance;

		// Token: 0x040026FF RID: 9983
		protected Vector3 lastFrameCameraOffset = Vector3.zero;

		// Token: 0x04002700 RID: 9984
		protected Vector3 lastManualOffset = Vector3.zero;

		// Token: 0x04002701 RID: 9985
		private Transform targetTransform;

		// Token: 0x04002702 RID: 9986
		private Transform cameraDolly;

		// Token: 0x04002703 RID: 9987
		private float x;

		// Token: 0x04002704 RID: 9988
		private float y;
	}
}
