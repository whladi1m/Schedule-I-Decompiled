using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006D1 RID: 1745
	public class LightOptimizer : MonoBehaviour
	{
		// Token: 0x06002F82 RID: 12162 RVA: 0x000C6119 File Offset: 0x000C4319
		public void Awake()
		{
			this.lights = base.GetComponentsInChildren<OptimizedLight>();
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000C6128 File Offset: 0x000C4328
		public void FixedUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			OptimizedLight[] array;
			if (Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) > this.checkRange)
			{
				array = this.lights;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].DisabledForOptimization = true;
				}
				return;
			}
			if (this.activationZones.Length == 0 && this.viewPoints.Length == 0)
			{
				this.ApplyLights();
				return;
			}
			BoxCollider[] array2 = this.activationZones;
			for (int i = 0; i < array2.Length; i++)
			{
				if (array2[i].bounds.Contains(PlayerSingleton<PlayerCamera>.Instance.transform.position))
				{
					this.ApplyLights();
					return;
				}
			}
			GeometryUtility.CalculateFrustumPlanes(PlayerSingleton<PlayerCamera>.Instance.Camera);
			foreach (Transform transform in this.viewPoints)
			{
				if (this.PointInCameraView(transform.position))
				{
					this.ApplyLights();
					return;
				}
			}
			array = this.lights;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DisabledForOptimization = true;
			}
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000C623C File Offset: 0x000C443C
		public void ApplyLights()
		{
			OptimizedLight[] array = this.lights;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DisabledForOptimization = false;
			}
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000C6268 File Offset: 0x000C4468
		public bool PointInCameraView(Vector3 point)
		{
			Camera camera = PlayerSingleton<PlayerCamera>.Instance.Camera;
			bool flag = camera.WorldToViewportPoint(point).z > -1f;
			bool flag2 = false;
			Vector3 normalized = (point - camera.transform.position).normalized;
			float num = Vector3.Distance(camera.transform.position, point);
			RaycastHit raycastHit;
			if (Physics.Raycast(camera.transform.position, normalized, out raycastHit, num + 0.05f, 1 << LayerMask.NameToLayer("Default")) && raycastHit.point != point)
			{
				flag2 = true;
			}
			return flag && !flag2;
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x0009DFB9 File Offset: 0x0009C1B9
		public bool Is01(float a)
		{
			return a > 0f && a < 1f;
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x000C6307 File Offset: 0x000C4507
		public void LightsEnabled_True()
		{
			this.LightsEnabled = true;
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x000C6310 File Offset: 0x000C4510
		public void LightsEnabled_False()
		{
			this.LightsEnabled = false;
		}

		// Token: 0x040021EA RID: 8682
		public bool LightsEnabled = true;

		// Token: 0x040021EB RID: 8683
		[Header("References")]
		[SerializeField]
		protected BoxCollider[] activationZones;

		// Token: 0x040021EC RID: 8684
		[SerializeField]
		protected Transform[] viewPoints;

		// Token: 0x040021ED RID: 8685
		[Header("Settings")]
		public float checkRange = 50f;

		// Token: 0x040021EE RID: 8686
		protected OptimizedLight[] lights;
	}
}
