using System;
using ScheduleOne.Audio;
using ScheduleOne.ObjectScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008A1 RID: 2209
	public class BoilingFlask : Fillable
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06003C3B RID: 15419 RVA: 0x000FD6D5 File Offset: 0x000FB8D5
		// (set) Token: 0x06003C3C RID: 15420 RVA: 0x000FD6DD File Offset: 0x000FB8DD
		public float CurrentTemperature { get; private set; }

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x000FD6E6 File Offset: 0x000FB8E6
		// (set) Token: 0x06003C3E RID: 15422 RVA: 0x000FD6EE File Offset: 0x000FB8EE
		public float CurrentTemperatureVelocity { get; private set; }

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06003C3F RID: 15423 RVA: 0x000FD6F7 File Offset: 0x000FB8F7
		public bool IsTemperatureInRange
		{
			get
			{
				return this.Recipe != null && this.CurrentTemperature >= this.Recipe.CookTemperatureLowerBound && this.CurrentTemperature <= this.Recipe.CookTemperatureUpperBound;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06003C40 RID: 15424 RVA: 0x000FD734 File Offset: 0x000FB934
		// (set) Token: 0x06003C41 RID: 15425 RVA: 0x000FD73C File Offset: 0x000FB93C
		public float OverheatScale { get; private set; }

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06003C42 RID: 15426 RVA: 0x000FD745 File Offset: 0x000FB945
		// (set) Token: 0x06003C43 RID: 15427 RVA: 0x000FD74D File Offset: 0x000FB94D
		public StationRecipe Recipe { get; private set; }

		// Token: 0x06003C44 RID: 15428 RVA: 0x000FD758 File Offset: 0x000FB958
		public void Update()
		{
			if (this.Burner == null)
			{
				return;
			}
			if (!this.LockTemperature)
			{
				float num = this.Burner.CurrentHeat - this.CurrentTemperature / 500f;
				this.CurrentTemperatureVelocity = Mathf.MoveTowards(this.CurrentTemperatureVelocity, num * this.TEMPERATURE_MAX_VELOCITY, this.TEMPERATURE_ACCELERATION * Time.deltaTime);
				this.CurrentTemperature = Mathf.Clamp(this.CurrentTemperature + this.CurrentTemperatureVelocity * Time.deltaTime, 0f, 500f);
			}
			if (this.CurrentTemperature > 0f)
			{
				this.BoilSound.VolumeMultiplier = Mathf.Clamp01(this.CurrentTemperature / 500f);
				this.BoilSound.AudioSource.pitch = this.BoilSoundPitchCurve.Evaluate(Mathf.Clamp01(this.CurrentTemperature / 500f));
				this.BoilSound.ApplyVolume();
				this.BoilSound.ApplyPitch();
				if (!this.BoilSound.AudioSource.isPlaying)
				{
					this.BoilSound.AudioSource.Play();
				}
			}
			else
			{
				this.BoilSound.AudioSource.Stop();
			}
			if (this.Recipe != null && this.CurrentTemperature >= this.Recipe.CookTemperatureUpperBound)
			{
				float num2 = Mathf.Clamp((this.CurrentTemperature - this.Recipe.CookTemperatureUpperBound) / (500f - this.Recipe.CookTemperatureUpperBound), 0.25f, 1f);
				this.OverheatScale += num2 * Time.deltaTime / 1.25f;
			}
			else
			{
				this.OverheatScale = Mathf.MoveTowards(this.OverheatScale, 0f, Time.deltaTime / 1.25f);
			}
			if (this.OverheatScale > 0f)
			{
				this.OverheatMesh.material.color = new Color(1f, 1f, 1f, Mathf.Pow(this.OverheatScale, 2f));
				this.OverheatMesh.enabled = true;
				return;
			}
			this.OverheatMesh.enabled = false;
		}

		// Token: 0x06003C45 RID: 15429 RVA: 0x000FD970 File Offset: 0x000FBB70
		private void FixedUpdate()
		{
			this.UpdateCanvas();
			this.UpdateSmoke();
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x000FD980 File Offset: 0x000FBB80
		private void UpdateCanvas()
		{
			if (this.TemperatureCanvas.gameObject.activeSelf)
			{
				this.TemperatureLabel.text = Mathf.RoundToInt(this.CurrentTemperature).ToString() + "°C";
				if (this.CurrentTemperature < this.Recipe.CookTemperatureLowerBound)
				{
					this.TemperatureLabel.color = Color.white;
				}
				else if (this.CurrentTemperature > this.Recipe.CookTemperatureUpperBound)
				{
					this.TemperatureLabel.color = new Color32(byte.MaxValue, 90, 90, byte.MaxValue);
				}
				else
				{
					this.TemperatureLabel.color = Color.green;
				}
				this.TemperatureSlider.value = this.CurrentTemperature / 500f;
				if (this.OverheatScale > 0f)
				{
					this.TemperatureLabel.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f) * Mathf.Clamp(this.OverheatScale, 0.3f, 1f) * this.LabelJitterScale;
					return;
				}
				this.TemperatureLabel.transform.localPosition = Vector3.zero;
			}
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x000FDAD0 File Offset: 0x000FBCD0
		private void UpdateSmoke()
		{
			if (this.CurrentTemperature < 1f)
			{
				if (this.SmokeParticles.isPlaying)
				{
					this.SmokeParticles.Stop();
				}
				return;
			}
			ParticleSystem.MainModule main = this.SmokeParticles.main;
			main.simulationSpeed = Mathf.Lerp(1f, 3f, this.CurrentTemperature / 500f);
			main.startColor = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, this.CurrentTemperature / 500f));
			if (!this.SmokeParticles.isPlaying)
			{
				this.SmokeParticles.Play();
			}
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x000FDB84 File Offset: 0x000FBD84
		public void SetCanvasVisible(bool visible)
		{
			this.TemperatureCanvas.gameObject.SetActive(visible);
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x000FDB97 File Offset: 0x000FBD97
		public void SetTemperature(float temp)
		{
			this.CurrentTemperature = temp;
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x000FDBA0 File Offset: 0x000FBDA0
		public void SetRecipe(StationRecipe recipe)
		{
			this.Recipe = recipe;
			if (recipe == null)
			{
				return;
			}
			float x = this.Recipe.CookTemperatureLowerBound / 500f;
			float x2 = this.Recipe.CookTemperatureUpperBound / 500f;
			this.TemperatureRangeIndicator.anchorMin = new Vector2(x, this.TemperatureRangeIndicator.anchorMin.y);
			this.TemperatureRangeIndicator.anchorMax = new Vector2(x2, this.TemperatureRangeIndicator.anchorMax.y);
		}

		// Token: 0x04002B5F RID: 11103
		public const float TEMPERATURE_MAX = 500f;

		// Token: 0x04002B60 RID: 11104
		public float TEMPERATURE_MAX_VELOCITY = 200f;

		// Token: 0x04002B61 RID: 11105
		public float TEMPERATURE_ACCELERATION = 50f;

		// Token: 0x04002B62 RID: 11106
		public const float OVERHEAT_TIME = 1.25f;

		// Token: 0x04002B67 RID: 11111
		public bool LockTemperature;

		// Token: 0x04002B68 RID: 11112
		public AnimationCurve BoilSoundPitchCurve;

		// Token: 0x04002B69 RID: 11113
		public float LabelJitterScale = 1f;

		// Token: 0x04002B6A RID: 11114
		[Header("References")]
		public BunsenBurner Burner;

		// Token: 0x04002B6B RID: 11115
		public Canvas TemperatureCanvas;

		// Token: 0x04002B6C RID: 11116
		public TextMeshProUGUI TemperatureLabel;

		// Token: 0x04002B6D RID: 11117
		public Slider TemperatureSlider;

		// Token: 0x04002B6E RID: 11118
		public RectTransform TemperatureRangeIndicator;

		// Token: 0x04002B6F RID: 11119
		public ParticleSystem SmokeParticles;

		// Token: 0x04002B70 RID: 11120
		public AudioSourceController BoilSound;

		// Token: 0x04002B71 RID: 11121
		public MeshRenderer OverheatMesh;
	}
}
