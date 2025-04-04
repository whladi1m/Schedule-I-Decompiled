using System;
using System.Runtime.CompilerServices;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006D8 RID: 1752
	[RequireComponent(typeof(Light))]
	[ExecuteInEditMode]
	public class OptimizedLight : MonoBehaviour
	{
		// Token: 0x06002FC0 RID: 12224 RVA: 0x000C6F77 File Offset: 0x000C5177
		public virtual void Awake()
		{
			this._Light = base.GetComponent<Light>();
			this.maxDistanceSquared = this.MaxDistance * this.MaxDistance;
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x000C6F98 File Offset: 0x000C5198
		private void Start()
		{
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				this.<Start>g__Register|7_0();
				return;
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.<Start>g__Register|7_0));
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x000C6FC8 File Offset: 0x000C51C8
		private void OnDestroy()
		{
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.DeregisterMovementEvent(new Action(this.UpdateCull));
			}
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x000C6FE7 File Offset: 0x000C51E7
		public virtual void FixedUpdate()
		{
			if (this._Light != null)
			{
				this._Light.enabled = (this.Enabled && !this.DisabledForOptimization && !this.culled);
			}
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x000C7020 File Offset: 0x000C5220
		private void UpdateCull()
		{
			if (this == null || base.gameObject == null)
			{
				return;
			}
			this.culled = (Vector3.SqrMagnitude(PlayerSingleton<PlayerCamera>.Instance.transform.position - base.transform.position) > this.maxDistanceSquared * QualitySettings.lodBias);
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x000C707D File Offset: 0x000C527D
		public void SetEnabled(bool enabled)
		{
			this.Enabled = enabled;
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x000C70A0 File Offset: 0x000C52A0
		[CompilerGenerated]
		private void <Start>g__Register|7_0()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.<Start>g__Register|7_0));
			PlayerSingleton<PlayerCamera>.Instance.RegisterMovementEvent(Mathf.RoundToInt(Mathf.Clamp(this.MaxDistance / 10f, 0.5f, 20f)), new Action(this.UpdateCull));
		}

		// Token: 0x0400220F RID: 8719
		public bool Enabled = true;

		// Token: 0x04002210 RID: 8720
		[HideInInspector]
		public bool DisabledForOptimization;

		// Token: 0x04002211 RID: 8721
		[Range(10f, 500f)]
		public float MaxDistance = 100f;

		// Token: 0x04002212 RID: 8722
		public Light _Light;

		// Token: 0x04002213 RID: 8723
		private bool culled;

		// Token: 0x04002214 RID: 8724
		private float maxDistanceSquared;
	}
}
