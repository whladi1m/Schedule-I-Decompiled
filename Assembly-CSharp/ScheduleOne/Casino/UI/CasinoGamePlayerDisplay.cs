using System;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Casino.UI
{
	// Token: 0x02000762 RID: 1890
	public class CasinoGamePlayerDisplay : MonoBehaviour
	{
		// Token: 0x060033B4 RID: 13236 RVA: 0x000D82B0 File Offset: 0x000D64B0
		public void RefreshPlayers()
		{
			int currentPlayerCount = this.BindedPlayers.CurrentPlayerCount;
			this.TitleLabel.text = string.Concat(new string[]
			{
				"Players (",
				currentPlayerCount.ToString(),
				"/",
				this.BindedPlayers.PlayerLimit.ToString(),
				")"
			});
			for (int i = 0; i < this.PlayerEntries.Length; i++)
			{
				Player player = this.BindedPlayers.GetPlayer(i);
				if (player != null)
				{
					this.PlayerEntries[i].Find("Container/Name").GetComponent<TextMeshProUGUI>().text = player.PlayerName;
					this.PlayerEntries[i].Find("Container").gameObject.SetActive(true);
				}
				else
				{
					this.PlayerEntries[i].Find("Container").gameObject.SetActive(false);
				}
			}
			this.RefreshScores();
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x000D83A4 File Offset: 0x000D65A4
		public void RefreshScores()
		{
			int currentPlayerCount = this.BindedPlayers.CurrentPlayerCount;
			for (int i = 0; i < this.PlayerEntries.Length; i++)
			{
				if (currentPlayerCount > i)
				{
					this.PlayerEntries[i].Find("Container/Score").GetComponent<TextMeshProUGUI>().text = MoneyManager.FormatAmount((float)this.BindedPlayers.GetPlayerScore(this.BindedPlayers.GetPlayer(i)), false, false);
				}
			}
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x000D8410 File Offset: 0x000D6610
		public void Bind(CasinoGamePlayers players)
		{
			this.BindedPlayers = players;
			this.BindedPlayers.onPlayerListChanged.AddListener(new UnityAction(this.RefreshPlayers));
			this.BindedPlayers.onPlayerScoresChanged.AddListener(new UnityAction(this.RefreshScores));
			this.RefreshPlayers();
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x000D8464 File Offset: 0x000D6664
		public void Unbind()
		{
			if (this.BindedPlayers == null)
			{
				return;
			}
			this.BindedPlayers.onPlayerListChanged.RemoveListener(new UnityAction(this.RefreshPlayers));
			this.BindedPlayers.onPlayerScoresChanged.RemoveListener(new UnityAction(this.RefreshScores));
			this.BindedPlayers = null;
		}

		// Token: 0x04002511 RID: 9489
		public CasinoGamePlayers BindedPlayers;

		// Token: 0x04002512 RID: 9490
		[Header("References")]
		public TextMeshProUGUI TitleLabel;

		// Token: 0x04002513 RID: 9491
		public RectTransform[] PlayerEntries;
	}
}
