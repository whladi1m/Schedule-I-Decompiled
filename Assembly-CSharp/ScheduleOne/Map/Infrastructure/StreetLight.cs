using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Lighting;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Map.Infrastructure
{
	// Token: 0x02000C0C RID: 3084
	public class StreetLight : MonoBehaviour
	{
		// Token: 0x0600562B RID: 22059 RVA: 0x00169C90 File Offset: 0x00167E90
		protected virtual void Awake()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.UpdateState));
			if (this.BeamTracker != null)
			{
				this.BeamTracker.Override = true;
			}
			this.StartTimeOffset = (int)(Vector3.Distance(base.transform.position, StreetLight.POWER_ORIGIN) / 50f);
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x00169D00 File Offset: 0x00167F00
		private void Start()
		{
			this.UpdateState();
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x00169D08 File Offset: 0x00167F08
		protected virtual void UpdateState()
		{
			this.SetState(NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(TimeManager.AddMinutesTo24HourTime(this.StartTime, this.StartTimeOffset), TimeManager.AddMinutesTo24HourTime(this.EndTime, this.StartTimeOffset)));
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				this.UpdateShadows();
			}
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnDrawGizmos()
		{
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x00169D54 File Offset: 0x00167F54
		private void SetState(bool on)
		{
			if (this.BeamTracker != null)
			{
				this.BeamTracker.Enabled = this.isOn;
			}
			float num = 0f;
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				num = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			}
			if (num < this.LightMaxDistance * QualitySettings.lodBias)
			{
				this.Light.enabled = this.isOn;
			}
			else
			{
				this.Light.enabled = false;
			}
			if (on == this.isOn)
			{
				return;
			}
			this.isOn = on;
			if (this.LightRend != null)
			{
				this.LightRend.material = (this.isOn ? this.LightOnMat : this.LightOffMat);
			}
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x00169E1C File Offset: 0x0016801C
		private void UpdateShadows()
		{
			if (!this.ShadowsEnabled)
			{
				this.Light.shadows = LightShadows.None;
				return;
			}
			float num = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			if (num < this.SoftShadowsThreshold * QualitySettings.lodBias)
			{
				this.Light.shadows = LightShadows.Soft;
				return;
			}
			if (num < this.HardShadowsThreshold * QualitySettings.lodBias)
			{
				this.Light.shadows = LightShadows.Hard;
				return;
			}
			this.Light.shadows = LightShadows.None;
		}

		// Token: 0x04004011 RID: 16401
		public static Vector3 POWER_ORIGIN = new Vector3(150f, 0f, -150f);

		// Token: 0x04004012 RID: 16402
		[Header("References")]
		[SerializeField]
		protected MeshRenderer LightRend;

		// Token: 0x04004013 RID: 16403
		[SerializeField]
		protected Light Light;

		// Token: 0x04004014 RID: 16404
		[SerializeField]
		protected VolumetricLightTracker BeamTracker;

		// Token: 0x04004015 RID: 16405
		[Header("Materials")]
		public Material LightOnMat;

		// Token: 0x04004016 RID: 16406
		public Material LightOffMat;

		// Token: 0x04004017 RID: 16407
		[Header("Timing")]
		public int StartTime = 1800;

		// Token: 0x04004018 RID: 16408
		public int EndTime = 600;

		// Token: 0x04004019 RID: 16409
		public int StartTimeOffset;

		// Token: 0x0400401A RID: 16410
		[Header("Settings")]
		public bool ShadowsEnabled = true;

		// Token: 0x0400401B RID: 16411
		public float LightMaxDistance = 80f;

		// Token: 0x0400401C RID: 16412
		public float SoftShadowsThreshold = 12f;

		// Token: 0x0400401D RID: 16413
		public float HardShadowsThreshold = 36f;

		// Token: 0x0400401E RID: 16414
		private bool isOn;
	}
}
