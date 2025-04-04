using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Casino
{
	// Token: 0x0200074C RID: 1868
	public class PlayingCard : MonoBehaviour
	{
		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060032EB RID: 13035 RVA: 0x000D4B09 File Offset: 0x000D2D09
		// (set) Token: 0x060032EC RID: 13036 RVA: 0x000D4B11 File Offset: 0x000D2D11
		public bool IsFaceUp { get; private set; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060032ED RID: 13037 RVA: 0x000D4B1A File Offset: 0x000D2D1A
		// (set) Token: 0x060032EE RID: 13038 RVA: 0x000D4B22 File Offset: 0x000D2D22
		public PlayingCard.ECardSuit Suit { get; private set; }

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060032EF RID: 13039 RVA: 0x000D4B2B File Offset: 0x000D2D2B
		// (set) Token: 0x060032F0 RID: 13040 RVA: 0x000D4B33 File Offset: 0x000D2D33
		public PlayingCard.ECardValue Value { get; private set; }

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060032F1 RID: 13041 RVA: 0x000D4B3C File Offset: 0x000D2D3C
		// (set) Token: 0x060032F2 RID: 13042 RVA: 0x000D4B44 File Offset: 0x000D2D44
		public CardController CardController { get; private set; }

		// Token: 0x060032F3 RID: 13043 RVA: 0x000D4B4D File Offset: 0x000D2D4D
		private void OnValidate()
		{
			base.gameObject.name = "PlayingCard (" + this.CardID + ")";
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x000D4B6F File Offset: 0x000D2D6F
		public void SetCardController(CardController cardController)
		{
			this.CardController = cardController;
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x000D4B78 File Offset: 0x000D2D78
		public void SetCard(PlayingCard.ECardSuit suit, PlayingCard.ECardValue value, bool network = true)
		{
			if (network && this.CardController != null)
			{
				this.CardController.SendCardValue(this.CardID, suit, value);
				return;
			}
			this.Suit = suit;
			this.Value = value;
			PlayingCard.CardSprite cardSprite = this.GetCardSprite(suit, value);
			if (cardSprite != null)
			{
				this.CardSpriteRenderer.sprite = cardSprite.Sprite;
			}
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x000D4BD5 File Offset: 0x000D2DD5
		public void ClearCard()
		{
			this.SetCard(PlayingCard.ECardSuit.Spades, PlayingCard.ECardValue.Blank, true);
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x000D4BE0 File Offset: 0x000D2DE0
		public void SetFaceUp(bool faceUp, bool network = true)
		{
			if (network && this.CardController != null)
			{
				this.CardController.SendCardFaceUp(this.CardID, faceUp);
			}
			if (this.IsFaceUp == faceUp)
			{
				return;
			}
			this.IsFaceUp = faceUp;
			if (this.IsFaceUp)
			{
				this.FlipAnimation.Play(this.FlipFaceUpClip.name);
			}
			else
			{
				this.FlipAnimation.Play(this.FlipFaceDownClip.name);
			}
			this.FlipSound.Play();
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x000D4C64 File Offset: 0x000D2E64
		public void GlideTo(Vector3 position, Quaternion rotation, float duration = 0.5f, bool network = true)
		{
			PlayingCard.<>c__DisplayClass35_0 CS$<>8__locals1 = new PlayingCard.<>c__DisplayClass35_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.duration = duration;
			CS$<>8__locals1.position = position;
			CS$<>8__locals1.rotation = rotation;
			if (network && this.CardController != null)
			{
				this.CardController.SendCardGlide(this.CardID, CS$<>8__locals1.position, CS$<>8__locals1.rotation, CS$<>8__locals1.duration);
				return;
			}
			if (this.lastGlideTarget != null && this.lastGlideTarget.Item1.Equals(CS$<>8__locals1.position) && this.lastGlideTarget.Item2.Equals(CS$<>8__locals1.rotation))
			{
				return;
			}
			this.lastGlideTarget = new Tuple<Vector3, Quaternion>(CS$<>8__locals1.position, CS$<>8__locals1.rotation);
			CS$<>8__locals1.verticalOffset = 0.02f;
			if (this.moveRoutine != null)
			{
				base.StopCoroutine(this.moveRoutine);
			}
			this.moveRoutine = base.StartCoroutine(CS$<>8__locals1.<GlideTo>g__MoveRoutine|0());
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x000D4D50 File Offset: 0x000D2F50
		private PlayingCard.CardSprite GetCardSprite(PlayingCard.ECardSuit suit, PlayingCard.ECardValue val)
		{
			return this.CardSprites.FirstOrDefault((PlayingCard.CardSprite x) => x.Suit == suit && x.Value == val);
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x000D4D88 File Offset: 0x000D2F88
		[Button]
		public void VerifyCardSprites()
		{
			List<PlayingCard.CardSprite> list = new List<PlayingCard.CardSprite>(this.CardSprites);
			foreach (object obj in Enum.GetValues(typeof(PlayingCard.ECardSuit)))
			{
				PlayingCard.ECardSuit ecardSuit = (PlayingCard.ECardSuit)obj;
				foreach (object obj2 in Enum.GetValues(typeof(PlayingCard.ECardValue)))
				{
					PlayingCard.ECardValue ecardValue = (PlayingCard.ECardValue)obj2;
					PlayingCard.CardSprite cardSprite = this.GetCardSprite(ecardSuit, ecardValue);
					if (cardSprite == null)
					{
						Debug.LogError(string.Format("Card sprite for {0} {1} is missing.", ecardSuit, ecardValue));
					}
					else if (list.Contains(cardSprite))
					{
						Debug.LogError(string.Format("Card sprite for {0} {1} is duplicated.", ecardSuit, ecardValue));
					}
					else
					{
						list.Add(cardSprite);
					}
				}
			}
		}

		// Token: 0x04002460 RID: 9312
		public string CardID = "card_1";

		// Token: 0x04002461 RID: 9313
		[Header("References")]
		public SpriteRenderer CardSpriteRenderer;

		// Token: 0x04002462 RID: 9314
		public PlayingCard.CardSprite[] CardSprites;

		// Token: 0x04002463 RID: 9315
		public Animation FlipAnimation;

		// Token: 0x04002464 RID: 9316
		public AnimationClip FlipFaceUpClip;

		// Token: 0x04002465 RID: 9317
		public AnimationClip FlipFaceDownClip;

		// Token: 0x04002466 RID: 9318
		[Header("Sound")]
		public AudioSourceController FlipSound;

		// Token: 0x04002467 RID: 9319
		public AudioSourceController LandSound;

		// Token: 0x04002468 RID: 9320
		private Coroutine moveRoutine;

		// Token: 0x04002469 RID: 9321
		private Tuple<Vector3, Quaternion> lastGlideTarget;

		// Token: 0x0200074D RID: 1869
		[Serializable]
		public class CardSprite
		{
			// Token: 0x0400246A RID: 9322
			public PlayingCard.ECardSuit Suit;

			// Token: 0x0400246B RID: 9323
			public PlayingCard.ECardValue Value;

			// Token: 0x0400246C RID: 9324
			public Sprite Sprite;
		}

		// Token: 0x0200074E RID: 1870
		public struct CardData
		{
			// Token: 0x060032FD RID: 13053 RVA: 0x000D4EB7 File Offset: 0x000D30B7
			public CardData(PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
			{
				this.Suit = suit;
				this.Value = value;
			}

			// Token: 0x0400246D RID: 9325
			public PlayingCard.ECardSuit Suit;

			// Token: 0x0400246E RID: 9326
			public PlayingCard.ECardValue Value;
		}

		// Token: 0x0200074F RID: 1871
		public enum ECardSuit
		{
			// Token: 0x04002470 RID: 9328
			Spades,
			// Token: 0x04002471 RID: 9329
			Hearts,
			// Token: 0x04002472 RID: 9330
			Diamonds,
			// Token: 0x04002473 RID: 9331
			Clubs
		}

		// Token: 0x02000750 RID: 1872
		public enum ECardValue
		{
			// Token: 0x04002475 RID: 9333
			Blank,
			// Token: 0x04002476 RID: 9334
			Ace,
			// Token: 0x04002477 RID: 9335
			Two,
			// Token: 0x04002478 RID: 9336
			Three,
			// Token: 0x04002479 RID: 9337
			Four,
			// Token: 0x0400247A RID: 9338
			Five,
			// Token: 0x0400247B RID: 9339
			Six,
			// Token: 0x0400247C RID: 9340
			Seven,
			// Token: 0x0400247D RID: 9341
			Eight,
			// Token: 0x0400247E RID: 9342
			Nine,
			// Token: 0x0400247F RID: 9343
			Ten,
			// Token: 0x04002480 RID: 9344
			Jack,
			// Token: 0x04002481 RID: 9345
			Queen,
			// Token: 0x04002482 RID: 9346
			King
		}
	}
}
