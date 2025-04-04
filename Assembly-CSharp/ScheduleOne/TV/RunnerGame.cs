using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x0200029B RID: 667
	public class RunnerGame : TVApp
	{
		// Token: 0x06000DDC RID: 3548 RVA: 0x0003DC90 File Offset: 0x0003BE90
		protected override void Awake()
		{
			base.Awake();
			this.defaultCharacterY = this.Character.anchoredPosition.y;
			this.CloudSpawner.OnSpawn.AddListener(new UnityAction<GameObject>(this.CloudSpawned));
			this.ObstacleSpawner.OnSpawn.AddListener(new UnityAction<GameObject>(this.ObstacleSpawned));
			this.StartScreen.SetActive(true);
			this.isReady = true;
			this.GameSpeed = 0f;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0003DD0F File Offset: 0x0003BF0F
		public override void Open()
		{
			base.Open();
			this.RefreshHighScore();
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0003DD1D File Offset: 0x0003BF1D
		protected override void TryPause()
		{
			if (this.isReady)
			{
				this.Close();
				return;
			}
			base.TryPause();
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0003DD34 File Offset: 0x0003BF34
		public void Update()
		{
			if (!base.IsOpen)
			{
				return;
			}
			this.Ground.SpeedMultiplier = (base.IsPaused ? 0f : this.GameSpeed);
			this.CharacterFlipboard.SpeedMultiplier = (base.IsPaused ? 0f : this.GameSpeed);
			this.ScoreLabel.text = this.score.ToString("00000");
			for (int i = 0; i < this.clouds.Count; i++)
			{
				if (this.clouds[i] == null || this.clouds[i].gameObject == null)
				{
					this.clouds.RemoveAt(i);
					i--;
				}
				else
				{
					this.clouds[i].SpeedMultiplier = (base.IsPaused ? 0f : this.GameSpeed);
				}
			}
			for (int j = 0; j < this.obstacles.Count; j++)
			{
				if (this.obstacles[j] == null || this.obstacles[j].gameObject == null)
				{
					this.obstacles.RemoveAt(j);
					j--;
				}
				else
				{
					this.obstacles[j].SpeedMultiplier = (base.IsPaused ? 0f : this.GameSpeed);
				}
			}
			float spawnRateMultiplier = Mathf.Sqrt(this.GameSpeed);
			this.ObstacleSpawner.SpawnRateMultiplier = spawnRateMultiplier;
			this.CloudSpawner.SpawnRateMultiplier = spawnRateMultiplier;
			if (this.isReady && (GameInput.GetButtonDown(GameInput.ButtonCode.Jump) || GameInput.GetButtonDown(GameInput.ButtonCode.Forward)))
			{
				this.StartGame();
			}
			if (base.IsPaused || this.GameSpeed == 0f)
			{
				return;
			}
			this.score += (float)this.ScoreRate * Time.deltaTime;
			this.GameSpeed = Mathf.Clamp(this.GameSpeed + this.SpeedIncreaseRate * Time.deltaTime, this.MinGameSpeed, this.MaxGameSpeed);
			if (this.Character.anchoredPosition.y - this.defaultCharacterY > 10f)
			{
				this.CharacterFlipboard.Image.sprite = this.JumpSprite;
				this.CharacterFlipboard.enabled = false;
			}
			else
			{
				this.CharacterFlipboard.enabled = true;
			}
			this.yVelocity -= this.Gravity * this.GlobalForceMultiplier * Time.deltaTime;
			if (this.isJumping && (GameInput.GetButton(GameInput.ButtonCode.Crouch) || GameInput.GetButton(GameInput.ButtonCode.Backward)))
			{
				this.yVelocity -= this.DropForce * this.GlobalForceMultiplier * Time.deltaTime;
			}
			if (this.Character.anchoredPosition.y + this.yVelocity * Time.deltaTime <= this.defaultCharacterY)
			{
				if (this.isJumping)
				{
					this.CharacterFlipboard.SetIndex(0);
				}
				this.Character.anchoredPosition = new Vector2(this.Character.anchoredPosition.x, this.defaultCharacterY);
				this.yVelocity = 0f;
				this.isJumping = false;
				this.isGrounded = true;
			}
			else
			{
				this.Character.anchoredPosition = new Vector2(this.Character.anchoredPosition.x, this.Character.anchoredPosition.y + this.yVelocity * Time.deltaTime);
			}
			if ((GameInput.GetButtonDown(GameInput.ButtonCode.Jump) || GameInput.GetButtonDown(GameInput.ButtonCode.Forward)) && this.isGrounded)
			{
				this.Jump();
			}
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0003E0B2 File Offset: 0x0003C2B2
		private void Jump()
		{
			this.isGrounded = false;
			this.isJumping = true;
			this.yVelocity = this.JumpForce * this.GlobalForceMultiplier;
			if (this.onJump != null)
			{
				this.onJump.Invoke();
			}
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0003E0E8 File Offset: 0x0003C2E8
		private void CloudSpawned(GameObject cloud)
		{
			this.clouds.Add(cloud.GetComponent<UIMover>());
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0003E0FB File Offset: 0x0003C2FB
		private void ObstacleSpawned(GameObject obstacle)
		{
			this.obstacles.Add(obstacle.GetComponent<UIMover>());
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0003E110 File Offset: 0x0003C310
		private void RefreshHighScore()
		{
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("RunGameHighScore");
			this.HighScoreLabel.text = value.ToString("00000");
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0003E144 File Offset: 0x0003C344
		public void PlayerCollided()
		{
			if (this.isReady)
			{
				return;
			}
			this.EndGame();
			if (this.onHit != null)
			{
				this.onHit.Invoke();
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0003E168 File Offset: 0x0003C368
		private void EndGame()
		{
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("RunGameHighScore");
			if (this.score > value)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("RunGameHighScore", this.score.ToString(), true);
				this.NewHighScoreAnimation.Play();
				if (this.onNewHighScore != null)
				{
					this.onNewHighScore.Invoke();
				}
			}
			this.GameOverScreen.SetActive(true);
			this.RefreshHighScore();
			this.GameSpeed = 0f;
			this.isReady = true;
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0003E1EC File Offset: 0x0003C3EC
		private void StartGame()
		{
			this.ResetGame();
			this.GameSpeed = this.MinGameSpeed;
			this.GameOverScreen.SetActive(false);
			this.StartScreen.SetActive(false);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0003E218 File Offset: 0x0003C418
		private void ResetGame()
		{
			this.score = 0f;
			this.GameSpeed = this.MinGameSpeed;
			this.yVelocity = 0f;
			this.isJumping = false;
			this.isGrounded = true;
			this.isReady = false;
			this.Character.anchoredPosition = new Vector2(this.Character.anchoredPosition.x, this.defaultCharacterY);
			for (int i = 0; i < this.clouds.Count; i++)
			{
				if (this.clouds[i] == null || this.clouds[i].gameObject == null)
				{
					this.clouds.RemoveAt(i);
					i--;
				}
				else
				{
					UnityEngine.Object.Destroy(this.clouds[i].gameObject);
				}
			}
			this.clouds.Clear();
			for (int j = 0; j < this.obstacles.Count; j++)
			{
				if (this.obstacles[j] == null || this.obstacles[j].gameObject == null)
				{
					this.obstacles.RemoveAt(j);
					j--;
				}
				else
				{
					UnityEngine.Object.Destroy(this.obstacles[j].gameObject);
				}
			}
			this.obstacles.Clear();
		}

		// Token: 0x04000E75 RID: 3701
		public float GameSpeed = 1f;

		// Token: 0x04000E76 RID: 3702
		public float MinGameSpeed = 1.5f;

		// Token: 0x04000E77 RID: 3703
		public float MaxGameSpeed = 4f;

		// Token: 0x04000E78 RID: 3704
		public float SpeedIncreaseRate = 0.1f;

		// Token: 0x04000E79 RID: 3705
		public int ScoreRate = 50;

		// Token: 0x04000E7A RID: 3706
		public float Gravity = 9.8f;

		// Token: 0x04000E7B RID: 3707
		public float JumpForce = 10f;

		// Token: 0x04000E7C RID: 3708
		public float GlobalForceMultiplier = 20f;

		// Token: 0x04000E7D RID: 3709
		public float DropForce = 1f;

		// Token: 0x04000E7E RID: 3710
		public RectTransform Character;

		// Token: 0x04000E7F RID: 3711
		public Flipboard CharacterFlipboard;

		// Token: 0x04000E80 RID: 3712
		public SlidingRect Ground;

		// Token: 0x04000E81 RID: 3713
		public UISpawner CloudSpawner;

		// Token: 0x04000E82 RID: 3714
		public UISpawner ObstacleSpawner;

		// Token: 0x04000E83 RID: 3715
		public TextMeshProUGUI ScoreLabel;

		// Token: 0x04000E84 RID: 3716
		public TextMeshProUGUI HighScoreLabel;

		// Token: 0x04000E85 RID: 3717
		public GameObject StartScreen;

		// Token: 0x04000E86 RID: 3718
		public GameObject GameOverScreen;

		// Token: 0x04000E87 RID: 3719
		public Animation NewHighScoreAnimation;

		// Token: 0x04000E88 RID: 3720
		public Sprite JumpSprite;

		// Token: 0x04000E89 RID: 3721
		private bool isJumping;

		// Token: 0x04000E8A RID: 3722
		private bool isGrounded = true;

		// Token: 0x04000E8B RID: 3723
		private bool isReady;

		// Token: 0x04000E8C RID: 3724
		private float score;

		// Token: 0x04000E8D RID: 3725
		private float yVelocity;

		// Token: 0x04000E8E RID: 3726
		private float defaultCharacterY;

		// Token: 0x04000E8F RID: 3727
		private List<UIMover> clouds = new List<UIMover>();

		// Token: 0x04000E90 RID: 3728
		private List<UIMover> obstacles = new List<UIMover>();

		// Token: 0x04000E91 RID: 3729
		public UnityEvent onJump;

		// Token: 0x04000E92 RID: 3730
		public UnityEvent onHit;

		// Token: 0x04000E93 RID: 3731
		public UnityEvent onNewHighScore;
	}
}
