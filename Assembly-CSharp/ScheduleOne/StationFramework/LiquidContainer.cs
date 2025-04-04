using System;
using LiquidVolumeFX;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.GameTime;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008AF RID: 2223
	public class LiquidContainer : MonoBehaviour
	{
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06003C7B RID: 15483 RVA: 0x000FE208 File Offset: 0x000FC408
		// (set) Token: 0x06003C7C RID: 15484 RVA: 0x000FE210 File Offset: 0x000FC410
		public float CurrentLiquidLevel { get; private set; }

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06003C7D RID: 15485 RVA: 0x000FE219 File Offset: 0x000FC419
		// (set) Token: 0x06003C7E RID: 15486 RVA: 0x000FE221 File Offset: 0x000FC421
		public Color LiquidColor { get; private set; } = Color.white;

		// Token: 0x06003C7F RID: 15487 RVA: 0x000FE22A File Offset: 0x000FC42A
		private void Awake()
		{
			this.liquidMesh = this.LiquidVolume.GetComponent<MeshRenderer>();
			this.SetLiquidColor(this.LiquidVolume.liquidColor1, true, true);
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x000FE250 File Offset: 0x000FC450
		private void Start()
		{
			this.LiquidVolume.directionalLight = Singleton<EnvironmentFX>.Instance.SunLight;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x000FE28D File Offset: 0x000FC48D
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x000FE2BC File Offset: 0x000FC4BC
		private void MinPass()
		{
			this.UpdateLighting();
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x000FE2C4 File Offset: 0x000FC4C4
		private void UpdateLighting()
		{
			if (this.AdjustMurkiness)
			{
				float t = Mathf.Abs((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f - 0.5f) / 0.5f;
				float b = Mathf.Lerp(1f, 0.75f, t);
				this.SetLiquidColor(this.LiquidColor * b, false, false);
			}
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x000FE324 File Offset: 0x000FC524
		public void SetLiquidLevel(float level, bool debug = false)
		{
			if (debug)
			{
				Console.Log("setting liquid level to: " + level.ToString(), null);
			}
			this.CurrentLiquidLevel = Mathf.Clamp01(level);
			this.LiquidVolume.level = Mathf.Lerp(0f, this.MaxLevel, this.CurrentLiquidLevel);
			if (this.liquidMesh != null)
			{
				this.liquidMesh.enabled = (this.CurrentLiquidLevel > 0.01f);
			}
			if (this.Collider != null && this.ColliderTransform_Min != null && this.ColliderTransform_Max != null)
			{
				this.Collider.transform.localPosition = Vector3.Lerp(this.ColliderTransform_Min.localPosition, this.ColliderTransform_Max.localPosition, this.CurrentLiquidLevel);
				this.Collider.transform.localScale = Vector3.Lerp(this.ColliderTransform_Min.localScale, this.ColliderTransform_Max.localScale, this.CurrentLiquidLevel);
			}
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x000FE42A File Offset: 0x000FC62A
		public void SetLiquidColor(Color color, bool setColorVariable = true, bool updateLigting = true)
		{
			if (setColorVariable)
			{
				this.LiquidColor = color;
			}
			this.LiquidVolume.liquidColor1 = color;
			this.LiquidVolume.liquidColor2 = color;
			if (updateLigting)
			{
				this.UpdateLighting();
			}
		}

		// Token: 0x04002B9D RID: 11165
		[Header("Settings")]
		[Range(0f, 1f)]
		public float Viscosity = 0.4f;

		// Token: 0x04002B9E RID: 11166
		public bool AdjustMurkiness = true;

		// Token: 0x04002B9F RID: 11167
		[Header("References")]
		public LiquidVolume LiquidVolume;

		// Token: 0x04002BA0 RID: 11168
		public LiquidVolumeCollider Collider;

		// Token: 0x04002BA1 RID: 11169
		public Transform ColliderTransform_Min;

		// Token: 0x04002BA2 RID: 11170
		public Transform ColliderTransform_Max;

		// Token: 0x04002BA3 RID: 11171
		[Header("Visuals Settings")]
		public float MaxLevel = 1f;

		// Token: 0x04002BA4 RID: 11172
		private MeshRenderer liquidMesh;
	}
}
