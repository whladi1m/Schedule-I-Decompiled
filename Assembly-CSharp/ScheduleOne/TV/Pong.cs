using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.TV
{
	// Token: 0x02000295 RID: 661
	public class Pong : TVApp
	{
		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x0003D3D9 File Offset: 0x0003B5D9
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x0003D3E1 File Offset: 0x0003B5E1
		public Pong.EGameMode GameMode { get; set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0003D3EA File Offset: 0x0003B5EA
		// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x0003D3F2 File Offset: 0x0003B5F2
		public Pong.EState State { get; set; }

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0003D3FB File Offset: 0x0003B5FB
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x0003D403 File Offset: 0x0003B603
		public int LeftScore { get; set; }

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x0003D40C File Offset: 0x0003B60C
		// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x0003D414 File Offset: 0x0003B614
		public int RightScore { get; set; }

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0003D41D File Offset: 0x0003B61D
		private void Update()
		{
			if (!base.IsOpen || base.IsPaused)
			{
				return;
			}
			this.UpdateInputs();
			if (this.GameMode == Pong.EGameMode.SinglePlayer)
			{
				this.UpdateAI();
			}
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0003D444 File Offset: 0x0003B644
		private void FixedUpdate()
		{
			if (!base.IsOpen || base.IsPaused)
			{
				this.Ball.RB.isKinematic = true;
				return;
			}
			this.ballVelocity = this.Ball.RB.velocity;
			this.Ball.RB.velocity += this.Ball.RB.velocity.normalized * this.VelocityGainPerSecond * Time.deltaTime;
			if (this.Ball.RB.velocity.magnitude > this.MaxVelocity)
			{
				this.Ball.RB.velocity = this.Ball.RB.velocity.normalized * this.MaxVelocity;
			}
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0003D524 File Offset: 0x0003B724
		protected override void TryPause()
		{
			this.Ball.RB.isKinematic = true;
			if (this.State == Pong.EState.Ready || this.State == Pong.EState.GameOver)
			{
				this.Close();
				return;
			}
			base.TryPause();
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0003D558 File Offset: 0x0003B758
		public void UpdateInputs()
		{
			if (this.State == Pong.EState.Playing)
			{
				Vector2 vector;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.Rect, Input.mousePosition, PlayerSingleton<PlayerCamera>.Instance.Camera, out vector);
				if (this.GameMode == Pong.EGameMode.SinglePlayer)
				{
					this.SetPaddleTargetY(Pong.ESide.Left, vector.y);
					return;
				}
			}
			else if (this.State == Pong.EState.Ready)
			{
				if (GameInput.GetButtonDown(GameInput.ButtonCode.Jump))
				{
					this.ServeBall();
					return;
				}
			}
			else if (this.State == Pong.EState.GameOver && GameInput.GetButtonDown(GameInput.ButtonCode.Jump))
			{
				this.ResetGame();
			}
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0003D5D4 File Offset: 0x0003B7D4
		private void UpdateAI()
		{
			if (this.State == Pong.EState.Playing)
			{
				this.reactionTimer += Time.deltaTime;
				if (this.reactionTimer >= this.ReactionTime)
				{
					float t = (Mathf.Clamp01(this.Ball.Rect.anchoredPosition.x / 300f) + 1f) / 2f;
					this.reactionTimer = 0f;
					float num = this.TargetRandomization * Mathf.Lerp(3f, 1f, t);
					float targetY = this.Ball.Rect.anchoredPosition.y + UnityEngine.Random.Range(-num, num);
					this.RightPaddle.SetTargetY(targetY);
					this.RightPaddle.SpeedMultiplier = Mathf.Lerp(0.1f, 1f, t) * this.SpeedMultiplier;
				}
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0003D6B0 File Offset: 0x0003B8B0
		public void GoalHit(Pong.ESide side)
		{
			if (this.State != Pong.EState.Playing)
			{
				return;
			}
			if (side == Pong.ESide.Left)
			{
				int num = this.RightScore;
				this.RightScore = num + 1;
				if (this.onRightScore != null)
				{
					this.onRightScore.Invoke();
				}
			}
			else
			{
				int num = this.LeftScore;
				this.LeftScore = num + 1;
				if (this.onLeftScore != null)
				{
					this.onLeftScore.Invoke();
				}
			}
			this.LeftScoreLabel.text = this.LeftScore.ToString();
			this.RightScoreLabel.text = this.RightScore.ToString();
			this.Ball.RB.velocity = Vector3.zero;
			this.Ball.RB.isKinematic = true;
			this.State = Pong.EState.Ready;
			if (this.LeftScore >= this.GoalsToWin)
			{
				this.Win(Pong.ESide.Left);
				return;
			}
			if (this.RightScore >= this.GoalsToWin)
			{
				this.Win(Pong.ESide.Right);
			}
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0003D79C File Offset: 0x0003B99C
		private void Win(Pong.ESide winner)
		{
			if (winner == Pong.ESide.Left)
			{
				this.WinnerLabel.text = "Player 1 Wins!";
				this.WinnerLabel.color = this.LeftPaddle.GetComponent<Image>().color;
				if (this.onLocalPlayerWin != null)
				{
					this.onLocalPlayerWin.Invoke();
				}
			}
			else
			{
				this.WinnerLabel.text = "Player 2 Wins!";
				this.WinnerLabel.color = this.RightPaddle.GetComponent<Image>().color;
			}
			this.State = Pong.EState.GameOver;
			if (this.onGameOver != null)
			{
				this.onGameOver.Invoke();
			}
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0003D834 File Offset: 0x0003BA34
		private void ResetBall()
		{
			this.Ball.RB.isKinematic = true;
			this.Ball.Rect.anchoredPosition = Vector2.zero;
			this.Ball.transform.localPosition = Vector3.zero;
			this.Ball.transform.localRotation = Quaternion.identity;
			this.Ball.RB.velocity = Vector3.zero;
			this.Ball.RB.isKinematic = false;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0003D8B8 File Offset: 0x0003BAB8
		private void ServeBall()
		{
			this.ResetBall();
			this.Ball.RB.isKinematic = false;
			if (this.nextBallSide == Pong.ESide.Left)
			{
				Vector2 normalized = new Vector2(-1f, UnityEngine.Random.Range(-0.5f, 0.5f)).normalized;
				this.Ball.RB.AddRelativeForce(normalized * this.InitialVelocity, ForceMode.VelocityChange);
			}
			else
			{
				Vector2 normalized2 = new Vector2(1f, UnityEngine.Random.Range(-0.5f, 0.5f)).normalized;
				this.Ball.RB.AddRelativeForce(normalized2 * this.InitialVelocity, ForceMode.VelocityChange);
			}
			this.State = Pong.EState.Playing;
			this.nextBallSide = ((this.nextBallSide == Pong.ESide.Left) ? Pong.ESide.Right : Pong.ESide.Left);
			if (this.onServe != null)
			{
				this.onServe.Invoke();
			}
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0003D99C File Offset: 0x0003BB9C
		private void ResetGame()
		{
			this.State = Pong.EState.Ready;
			this.LeftScore = 0;
			this.RightScore = 0;
			this.LeftScoreLabel.text = this.LeftScore.ToString();
			this.RightScoreLabel.text = this.RightScore.ToString();
			this.ResetBall();
			this.nextBallSide = Pong.ESide.Left;
			this.ballVelocity = Vector3.zero;
			if (this.onReset != null)
			{
				this.onReset.Invoke();
			}
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0003DA1B File Offset: 0x0003BC1B
		public void SetPaddleTargetY(Pong.ESide player, float y)
		{
			if (player == Pong.ESide.Left)
			{
				this.LeftPaddle.SetTargetY(y);
				return;
			}
			this.RightPaddle.SetTargetY(y);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0003DA39 File Offset: 0x0003BC39
		public override void Resume()
		{
			base.Resume();
			this.Ball.RB.isKinematic = false;
			this.Ball.RB.velocity = this.ballVelocity;
		}

		// Token: 0x04000E4A RID: 3658
		public RectTransform Rect;

		// Token: 0x04000E4B RID: 3659
		public PongPaddle LeftPaddle;

		// Token: 0x04000E4C RID: 3660
		public PongPaddle RightPaddle;

		// Token: 0x04000E4D RID: 3661
		public PongBall Ball;

		// Token: 0x04000E4E RID: 3662
		public TextMeshProUGUI LeftScoreLabel;

		// Token: 0x04000E4F RID: 3663
		public TextMeshProUGUI RightScoreLabel;

		// Token: 0x04000E50 RID: 3664
		public TextMeshProUGUI WinnerLabel;

		// Token: 0x04000E51 RID: 3665
		[Header("Settings")]
		public float InitialVelocity = 0.8f;

		// Token: 0x04000E52 RID: 3666
		public float VelocityGainPerSecond = 0.05f;

		// Token: 0x04000E53 RID: 3667
		public float MaxVelocity = 2f;

		// Token: 0x04000E54 RID: 3668
		public int GoalsToWin = 10;

		// Token: 0x04000E55 RID: 3669
		[Header("AI")]
		public float ReactionTime = 0.1f;

		// Token: 0x04000E56 RID: 3670
		public float TargetRandomization = 10f;

		// Token: 0x04000E57 RID: 3671
		public float SpeedMultiplier = 0.5f;

		// Token: 0x04000E58 RID: 3672
		public UnityEvent onServe;

		// Token: 0x04000E59 RID: 3673
		public UnityEvent onLeftScore;

		// Token: 0x04000E5A RID: 3674
		public UnityEvent onRightScore;

		// Token: 0x04000E5B RID: 3675
		public UnityEvent onGameOver;

		// Token: 0x04000E5C RID: 3676
		public UnityEvent onLocalPlayerWin;

		// Token: 0x04000E5D RID: 3677
		public UnityEvent onReset;

		// Token: 0x04000E5E RID: 3678
		private Pong.ESide nextBallSide;

		// Token: 0x04000E5F RID: 3679
		private Vector3 ballVelocity = Vector3.zero;

		// Token: 0x04000E60 RID: 3680
		private float reactionTimer;

		// Token: 0x02000296 RID: 662
		public enum EGameMode
		{
			// Token: 0x04000E62 RID: 3682
			SinglePlayer,
			// Token: 0x04000E63 RID: 3683
			MultiPlayer
		}

		// Token: 0x02000297 RID: 663
		public enum ESide
		{
			// Token: 0x04000E65 RID: 3685
			Left,
			// Token: 0x04000E66 RID: 3686
			Right
		}

		// Token: 0x02000298 RID: 664
		public enum EState
		{
			// Token: 0x04000E68 RID: 3688
			Ready,
			// Token: 0x04000E69 RID: 3689
			Playing,
			// Token: 0x04000E6A RID: 3690
			GameOver
		}
	}
}
