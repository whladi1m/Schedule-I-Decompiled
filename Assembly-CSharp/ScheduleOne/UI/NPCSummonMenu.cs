using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009F3 RID: 2547
	public class NPCSummonMenu : Singleton<NPCSummonMenu>
	{
		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x060044B3 RID: 17587 RVA: 0x0011FDCB File Offset: 0x0011DFCB
		// (set) Token: 0x060044B4 RID: 17588 RVA: 0x0011FDD3 File Offset: 0x0011DFD3
		public bool IsOpen { get; private set; }

		// Token: 0x060044B5 RID: 17589 RVA: 0x0011FDDC File Offset: 0x0011DFDC
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x0011FE13 File Offset: 0x0011E013
		private void Exit(ExitAction exit)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (exit.used)
			{
				return;
			}
			if (exit.exitType == ExitType.Escape)
			{
				exit.used = true;
				this.Close();
			}
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x0011FE40 File Offset: 0x0011E040
		public void Open(List<NPC> npcs, Action<NPC> _callback)
		{
			this.IsOpen = true;
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			this.callback = _callback;
			for (int i = 0; i < this.Entries.Length; i++)
			{
				if (npcs.Count > i)
				{
					this.Entries[i].Find("Icon").GetComponent<Image>().sprite = npcs[i].MugshotSprite;
					this.Entries[i].Find("Name").GetComponent<TextMeshProUGUI>().text = npcs[i].fullName;
					this.Entries[i].gameObject.SetActive(true);
					NPC npc = npcs[i];
					this.Entries[i].GetComponent<Button>().onClick.RemoveAllListeners();
					this.Entries[i].GetComponent<Button>().onClick.AddListener(delegate()
					{
						this.NPCSelected(npc);
					});
				}
				else
				{
					this.Entries[i].gameObject.SetActive(false);
				}
			}
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x0011FFA4 File Offset: 0x0011E1A4
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.callback = null;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x00120017 File Offset: 0x0011E217
		public void NPCSelected(NPC npc)
		{
			this.callback(npc);
			this.Close();
		}

		// Token: 0x04003297 RID: 12951
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003298 RID: 12952
		public RectTransform Container;

		// Token: 0x04003299 RID: 12953
		public RectTransform EntryContainer;

		// Token: 0x0400329A RID: 12954
		public RectTransform[] Entries;

		// Token: 0x0400329B RID: 12955
		private Action<NPC> callback;
	}
}
