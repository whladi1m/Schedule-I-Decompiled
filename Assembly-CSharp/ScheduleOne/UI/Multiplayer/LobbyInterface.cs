using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Networking;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Multiplayer
{
	// Token: 0x02000ACA RID: 2762
	public class LobbyInterface : PersistentSingleton<LobbyInterface>
	{
		// Token: 0x06004A20 RID: 18976 RVA: 0x00136BA4 File Offset: 0x00134DA4
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<LobbyInterface>.Instance == null || Singleton<LobbyInterface>.Instance != this)
			{
				return;
			}
			this.InviteButton.onClick.AddListener(new UnityAction(this.InviteClicked));
			this.LeaveButton.onClick.AddListener(new UnityAction(this.LeaveClicked));
			Lobby lobby = this.Lobby;
			lobby.onLobbyChange = (Action)Delegate.Combine(lobby.onLobbyChange, new Action(delegate()
			{
				this.UpdateButtons();
				this.UpdatePlayers();
				this.LobbyTitle.text = string.Concat(new string[]
				{
					"Lobby (",
					this.Lobby.PlayerCount.ToString(),
					"/",
					4.ToString(),
					")"
				});
			}));
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x00136C34 File Offset: 0x00134E34
		protected override void Start()
		{
			base.Start();
			if (Singleton<LobbyInterface>.Instance == null || Singleton<LobbyInterface>.Instance != this)
			{
				return;
			}
			this.UpdateButtons();
			this.UpdatePlayers();
			if (PlayerPrefs.GetInt("InviteHintShown", 0) == 0)
			{
				this.InviteHint.SetActive(true);
				return;
			}
			this.InviteHint.SetActive(false);
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x00136C94 File Offset: 0x00134E94
		private void LateUpdate()
		{
			if (Singleton<PauseMenu>.InstanceExists)
			{
				this.Canvas.enabled = (Singleton<PauseMenu>.Instance.IsPaused && this.Lobby.IsInLobby && !GameManager.IS_TUTORIAL);
				if (this.Canvas.enabled)
				{
					this.LeaveButton.gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				this.Canvas.enabled = true;
				this.LeaveButton.gameObject.SetActive(!this.Lobby.IsHost);
			}
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x00136D20 File Offset: 0x00134F20
		public void SetVisible(bool visible)
		{
			this.Canvas.enabled = visible;
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x00136D2E File Offset: 0x00134F2E
		public void LeaveClicked()
		{
			this.Lobby.LeaveLobby();
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x00136D3B File Offset: 0x00134F3B
		public void InviteClicked()
		{
			PlayerPrefs.SetInt("InviteHintShown", 1);
			this.InviteHint.SetActive(false);
			this.Lobby.TryOpenInviteInterface();
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x00136D60 File Offset: 0x00134F60
		private void UpdateButtons()
		{
			this.InviteButton.gameObject.SetActive(this.Lobby.IsHost && this.Lobby.PlayerCount < 4);
			this.LeaveButton.gameObject.SetActive(!this.Lobby.IsHost);
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x00136DBC File Offset: 0x00134FBC
		private void UpdatePlayers()
		{
			if (this.Lobby.IsInLobby)
			{
				for (int i = 0; i < this.PlayerSlots.Length; i++)
				{
					if (this.Lobby.Players[i] != CSteamID.Nil)
					{
						this.SetPlayer(i, this.Lobby.Players[i]);
					}
					else
					{
						this.ClearPlayer(i);
					}
				}
				return;
			}
			this.SetPlayer(0, this.Lobby.LocalPlayerID);
			for (int j = 1; j < this.PlayerSlots.Length; j++)
			{
				this.ClearPlayer(j);
			}
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x00136E54 File Offset: 0x00135054
		public void SetPlayer(int index, CSteamID player)
		{
			this.Lobby.Players[index] = player;
			this.PlayerSlots[index].Find("Frame/Avatar").GetComponent<RawImage>().texture = this.GetAvatar(player);
			this.PlayerSlots[index].gameObject.SetActive(true);
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x00136EA9 File Offset: 0x001350A9
		public void ClearPlayer(int index)
		{
			this.Lobby.Players[index] = CSteamID.Nil;
			this.PlayerSlots[index].gameObject.SetActive(false);
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x00136ED4 File Offset: 0x001350D4
		private Texture2D GetAvatar(CSteamID user)
		{
			if (!SteamManager.Initialized)
			{
				Debug.LogWarning("Steamworks not initialized");
				return new Texture2D(0, 0);
			}
			int mediumFriendAvatar = SteamFriends.GetMediumFriendAvatar(user);
			uint num;
			uint num2;
			if (SteamUtils.GetImageSize(mediumFriendAvatar, out num, out num2) && num > 0U && num2 > 0U)
			{
				byte[] array = new byte[num * num2 * 4U];
				Texture2D texture2D = new Texture2D((int)num, (int)num2, TextureFormat.RGBA32, false, false);
				if (SteamUtils.GetImageRGBA(mediumFriendAvatar, array, (int)(num * num2 * 4U)))
				{
					texture2D.LoadRawTextureData(array);
					texture2D.Apply();
				}
				return texture2D;
			}
			Debug.LogWarning("Couldn't get avatar.");
			return new Texture2D(0, 0);
		}

		// Token: 0x040037BE RID: 14270
		[Header("References")]
		public Lobby Lobby;

		// Token: 0x040037BF RID: 14271
		public Canvas Canvas;

		// Token: 0x040037C0 RID: 14272
		public TextMeshProUGUI LobbyTitle;

		// Token: 0x040037C1 RID: 14273
		public RectTransform[] PlayerSlots;

		// Token: 0x040037C2 RID: 14274
		public Button InviteButton;

		// Token: 0x040037C3 RID: 14275
		public Button LeaveButton;

		// Token: 0x040037C4 RID: 14276
		public GameObject InviteHint;
	}
}
