using System;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Casino
{
	// Token: 0x02000748 RID: 1864
	public class CasinoGameInteraction : MonoBehaviour
	{
		// Token: 0x0600329D RID: 12957 RVA: 0x000D316C File Offset: 0x000D136C
		private void Awake()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x000D31A8 File Offset: 0x000D13A8
		private void Hovered()
		{
			if (this.Players.CurrentPlayerCount < this.Players.PlayerLimit)
			{
				this.IntObj.SetMessage("Play " + this.GameName);
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetMessage("Table is full");
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x000D3211 File Offset: 0x000D1411
		private void Interacted()
		{
			if (this.Players.CurrentPlayerCount < this.Players.PlayerLimit && this.onLocalPlayerRequestJoin != null)
			{
				this.onLocalPlayerRequestJoin(Player.Local);
			}
		}

		// Token: 0x0400244A RID: 9290
		public string GameName;

		// Token: 0x0400244B RID: 9291
		[Header("References")]
		public CasinoGamePlayers Players;

		// Token: 0x0400244C RID: 9292
		public InteractableObject IntObj;

		// Token: 0x0400244D RID: 9293
		public Action<Player> onLocalPlayerRequestJoin;
	}
}
