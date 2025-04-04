using System;
using System.Collections.Generic;
using EasyButtons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x0200029D RID: 669
	public class Snake : TVApp
	{
		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x0003E437 File Offset: 0x0003C637
		// (set) Token: 0x06000DEC RID: 3564 RVA: 0x0003E43F File Offset: 0x0003C63F
		public Vector2 HeadPosition { get; private set; } = new Vector2(10f, 6f);

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x0003E448 File Offset: 0x0003C648
		// (set) Token: 0x06000DEE RID: 3566 RVA: 0x0003E450 File Offset: 0x0003C650
		public List<Vector2> Tail { get; private set; } = new List<Vector2>();

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x0003E459 File Offset: 0x0003C659
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x0003E461 File Offset: 0x0003C661
		public Vector2 LastTailPosition { get; private set; } = Vector2.zero;

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x0003E46A File Offset: 0x0003C66A
		// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x0003E472 File Offset: 0x0003C672
		public Vector2 Direction { get; private set; } = Vector2.right;

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x0003E47B File Offset: 0x0003C67B
		// (set) Token: 0x06000DF4 RID: 3572 RVA: 0x0003E483 File Offset: 0x0003C683
		public Vector2 QueuedDirection { get; private set; } = Vector2.right;

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000DF5 RID: 3573 RVA: 0x0003E48C File Offset: 0x0003C68C
		// (set) Token: 0x06000DF6 RID: 3574 RVA: 0x0003E494 File Offset: 0x0003C694
		public Vector2 NextDirection { get; private set; } = Vector2.zero;

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x0003E49D File Offset: 0x0003C69D
		// (set) Token: 0x06000DF8 RID: 3576 RVA: 0x0003E4A5 File Offset: 0x0003C6A5
		public Snake.EGameState GameState { get; private set; }

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0003E4AE File Offset: 0x0003C6AE
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0003E4B8 File Offset: 0x0003C6B8
		private void Update()
		{
			if (base.IsPaused)
			{
				return;
			}
			if (!base.IsOpen)
			{
				return;
			}
			this.UpdateInput();
			this.UpdateMovement();
			this._timeOnGameOver += Time.deltaTime;
			this.ScoreText.text = this.Tail.Count.ToString();
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0003E514 File Offset: 0x0003C714
		private void UpdateInput()
		{
			if (this._timeOnGameOver < 0.3f)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Forward) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				if (this.Direction != Vector2.down)
				{
					this.QueuedDirection = Vector2.up;
				}
				this.NextDirection = Vector2.up;
				if (this.GameState == Snake.EGameState.Ready)
				{
					this.StartGame(Vector2.up);
					return;
				}
			}
			else if (GameInput.GetButtonDown(GameInput.ButtonCode.Backward) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				if (this.Direction != Vector2.up)
				{
					this.QueuedDirection = Vector2.down;
				}
				this.NextDirection = Vector2.down;
				if (this.GameState == Snake.EGameState.Ready)
				{
					this.StartGame(Vector2.down);
					return;
				}
			}
			else if (GameInput.GetButtonDown(GameInput.ButtonCode.Left) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (this.Direction != Vector2.right)
				{
					this.QueuedDirection = Vector2.left;
				}
				this.NextDirection = Vector2.left;
				if (this.GameState == Snake.EGameState.Ready)
				{
					this.StartGame(Vector2.left);
					return;
				}
			}
			else if (GameInput.GetButtonDown(GameInput.ButtonCode.Right) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (this.Direction != Vector2.left)
				{
					this.QueuedDirection = Vector2.right;
				}
				this.NextDirection = Vector2.right;
				if (this.GameState == Snake.EGameState.Ready)
				{
					this.StartGame(Vector2.right);
				}
			}
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0003E674 File Offset: 0x0003C874
		private void UpdateMovement()
		{
			if (this.GameState != Snake.EGameState.Playing)
			{
				return;
			}
			this._timeSinceLastMove += Time.deltaTime;
			if (this._timeSinceLastMove >= this.TimePerTile)
			{
				this._timeSinceLastMove -= this.TimePerTile;
				this.MoveSnake();
			}
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0003E6C4 File Offset: 0x0003C8C4
		private void MoveSnake()
		{
			this.Direction = this.QueuedDirection;
			Vector2 vector = this.HeadPosition + this.Direction;
			SnakeTile tile = this.GetTile(vector);
			if (tile == null)
			{
				this.GameOver();
				return;
			}
			if (tile.Type == SnakeTile.TileType.Snake && this.Tail.Count > 0 && tile.Position != this.Tail[this.Tail.Count - 1])
			{
				this.GameOver();
				return;
			}
			bool flag = false;
			if (tile.Type == SnakeTile.TileType.Food)
			{
				this.Eat();
				flag = true;
				if (this.GameState != Snake.EGameState.Playing)
				{
					return;
				}
			}
			this.GetTile(vector).SetType(SnakeTile.TileType.Snake, 0);
			Vector2 vector2 = this.HeadPosition;
			this.HeadPosition = vector;
			for (int i = 0; i < this.Tail.Count; i++)
			{
				if (i == this.Tail.Count - 1)
				{
					this.LastTailPosition = this.Tail[i];
				}
				Vector2 vector3 = this.Tail[i];
				this.Tail[i] = vector2;
				this.GetTile(this.Tail[i]).SetType(SnakeTile.TileType.Snake, 1 + i);
				vector2 = vector3;
			}
			this.GetTile(vector2).SetType(SnakeTile.TileType.Empty, 0);
			this.LastTailPosition = vector2;
			if (this.NextDirection != Vector2.zero && this.NextDirection != -this.Direction)
			{
				this.QueuedDirection = this.NextDirection;
			}
			if (flag)
			{
				this.SpawnFood();
			}
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0003E850 File Offset: 0x0003CA50
		private SnakeTile GetTile(Vector2 position)
		{
			if (position.x < 0f || position.x >= 20f || position.y < 0f || position.y >= 12f)
			{
				return null;
			}
			return this.Tiles[(int)position.y * 20 + (int)position.x];
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0003E8AC File Offset: 0x0003CAAC
		private void StartGame(Vector2 initialDir)
		{
			SnakeTile tile = this.GetTile(this.lastFoodPosition);
			if (tile != null)
			{
				tile.SetType(SnakeTile.TileType.Empty, 0);
			}
			this.SpawnFood();
			SnakeTile tile2 = this.GetTile(this.HeadPosition);
			if (tile2 != null)
			{
				tile2.SetType(SnakeTile.TileType.Empty, 0);
			}
			this.HeadPosition = new Vector2(10f, 6f);
			for (int i = 0; i < this.Tail.Count; i++)
			{
				this.GetTile(this.Tail[i]).SetType(SnakeTile.TileType.Empty, 0);
			}
			this.Tail.Clear();
			this.QueuedDirection = initialDir;
			this.NextDirection = Vector2.zero;
			this._timeSinceLastMove = 0f;
			this.MoveSnake();
			this.GameState = Snake.EGameState.Playing;
			if (this.onStart != null)
			{
				this.onStart.Invoke();
			}
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0003E982 File Offset: 0x0003CB82
		private void Eat()
		{
			this.Tail.Add(this.LastTailPosition);
			if (this.onEat != null)
			{
				this.onEat.Invoke();
			}
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0003E9A8 File Offset: 0x0003CBA8
		private void SpawnFood()
		{
			List<SnakeTile> list = new List<SnakeTile>();
			foreach (SnakeTile snakeTile in this.Tiles)
			{
				if (snakeTile.Type == SnakeTile.TileType.Empty)
				{
					list.Add(snakeTile);
				}
			}
			if (list.Count == 0)
			{
				this.Win();
				return;
			}
			SnakeTile snakeTile2 = list[UnityEngine.Random.Range(0, list.Count)];
			snakeTile2.SetType(SnakeTile.TileType.Food, 0);
			this.lastFoodPosition = snakeTile2.Position;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0003EA1C File Offset: 0x0003CC1C
		private void GameOver()
		{
			this.GameState = Snake.EGameState.Ready;
			this._timeOnGameOver = 0f;
			if (this.onGameOver != null)
			{
				this.onGameOver.Invoke();
			}
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0003EA43 File Offset: 0x0003CC43
		private void Win()
		{
			this.GameState = Snake.EGameState.Ready;
			this._timeOnGameOver = 0f;
			if (this.onWin != null)
			{
				this.onWin.Invoke();
			}
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0003EA6A File Offset: 0x0003CC6A
		protected override void TryPause()
		{
			if (this.GameState == Snake.EGameState.Ready)
			{
				this.Close();
				return;
			}
			base.TryPause();
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0003EA84 File Offset: 0x0003CC84
		[Button]
		public void CreateTiles()
		{
			SnakeTile[] tiles = this.Tiles;
			for (int i = 0; i < tiles.Length; i++)
			{
				UnityEngine.Object.DestroyImmediate(tiles[i].gameObject);
			}
			this.Tiles = new SnakeTile[240];
			float tileSize = this.PlaySpace.rect.width / 20f;
			for (int j = 0; j < 12; j++)
			{
				for (int k = 0; k < 20; k++)
				{
					SnakeTile snakeTile = UnityEngine.Object.Instantiate<SnakeTile>(this.TilePrefab, this.PlaySpace);
					snakeTile.SetType(SnakeTile.TileType.Empty, 0);
					snakeTile.SetPosition(new Vector2((float)k, (float)j), tileSize);
					this.Tiles[j * 20 + k] = snakeTile;
				}
			}
		}

		// Token: 0x04000E96 RID: 3734
		public const int SIZE_X = 20;

		// Token: 0x04000E97 RID: 3735
		public const int SIZE_Y = 12;

		// Token: 0x04000E98 RID: 3736
		[Header("Settings")]
		public SnakeTile TilePrefab;

		// Token: 0x04000E99 RID: 3737
		public float TimePerTile = 0.4f;

		// Token: 0x04000E9A RID: 3738
		[Header("References")]
		public RectTransform PlaySpace;

		// Token: 0x04000E9B RID: 3739
		public SnakeTile[] Tiles;

		// Token: 0x04000E9C RID: 3740
		public TextMeshProUGUI ScoreText;

		// Token: 0x04000EA3 RID: 3747
		private Vector2 lastFoodPosition = Vector2.zero;

		// Token: 0x04000EA5 RID: 3749
		private float _timeSinceLastMove;

		// Token: 0x04000EA6 RID: 3750
		private float _timeOnGameOver;

		// Token: 0x04000EA7 RID: 3751
		public UnityEvent onStart;

		// Token: 0x04000EA8 RID: 3752
		public UnityEvent onEat;

		// Token: 0x04000EA9 RID: 3753
		public UnityEvent onGameOver;

		// Token: 0x04000EAA RID: 3754
		public UnityEvent onWin;

		// Token: 0x0200029E RID: 670
		public enum EGameState
		{
			// Token: 0x04000EAC RID: 3756
			Ready,
			// Token: 0x04000EAD RID: 3757
			Playing
		}
	}
}
