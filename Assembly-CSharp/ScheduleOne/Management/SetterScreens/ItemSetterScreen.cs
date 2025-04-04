using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management.Presets.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.Management.SetterScreens
{
	// Token: 0x02000588 RID: 1416
	public class ItemSetterScreen : Singleton<ItemSetterScreen>
	{
		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x0600235D RID: 9053 RVA: 0x000903EE File Offset: 0x0008E5EE
		// (set) Token: 0x0600235E RID: 9054 RVA: 0x000903F6 File Offset: 0x0008E5F6
		public ItemList Option { get; private set; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x0600235F RID: 9055 RVA: 0x000903FF File Offset: 0x0008E5FF
		public bool IsOpen
		{
			get
			{
				return this.Option != null;
			}
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0009040C File Offset: 0x0008E60C
		protected override void Awake()
		{
			base.Awake();
			this.allEntry = this.CreateEntry(null, "All", new Action(this.AllClicked), "", false);
			this.noneEntry = this.CreateEntry(null, "None", new Action(this.NoneClicked), "", false);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
			this.Close();
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x00090480 File Offset: 0x0008E680
		public virtual void Open(ItemList option)
		{
			this.Option = option;
			this.TitleLabel.text = this.Option.Name;
			base.gameObject.SetActive(true);
			this.allEntry.gameObject.SetActive(option.CanBeAll);
			this.noneEntry.gameObject.SetActive(option.CanBeNone);
			this.CreateEntries();
			this.RefreshTicks();
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x000904EE File Offset: 0x0008E6EE
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.Close();
			}
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x00090518 File Offset: 0x0008E718
		public virtual void Close()
		{
			this.Option = null;
			this.DestroyEntries();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x00090534 File Offset: 0x0008E734
		private RectTransform CreateEntry(Sprite icon, string label, Action onClick, string prefabID = "", bool createPair = false)
		{
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.ListEntryPrefab, this.EntryContainer).GetComponent<RectTransform>();
			if (icon == null)
			{
				component.Find("Icon").gameObject.SetActive(false);
				component.Find("Title").GetComponent<RectTransform>().offsetMin = new Vector2(0.5f, 0f);
			}
			else
			{
				component.Find("Icon").GetComponent<Image>().sprite = icon;
			}
			component.Find("Title").GetComponent<TextMeshProUGUI>().text = label;
			component.GetComponent<Button>().onClick.AddListener(delegate()
			{
				onClick();
			});
			if (createPair)
			{
				this.pairs.Add(new ItemSetterScreen.Pair
				{
					prefabID = prefabID,
					entry = component
				});
			}
			return component;
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x00090616 File Offset: 0x0008E816
		private void AllClicked()
		{
			this.Option.All = true;
			this.Option.None = false;
			this.RefreshTicks();
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x00090636 File Offset: 0x0008E836
		private void NoneClicked()
		{
			this.Option.All = false;
			this.Option.None = true;
			this.Option.Selection.Clear();
			this.RefreshTicks();
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x00090668 File Offset: 0x0008E868
		private void EntryClicked(string prefabID)
		{
			if (this.Option.All)
			{
				this.Option.Selection.Clear();
				this.Option.Selection.AddRange(this.Option.OptionList);
				this.Option.Selection.Remove(prefabID);
			}
			else if (this.Option.Selection.Contains(prefabID))
			{
				this.Option.Selection.Remove(prefabID);
			}
			else
			{
				this.Option.Selection.Add(prefabID);
			}
			this.Option.All = false;
			this.Option.None = false;
			this.RefreshTicks();
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x00090718 File Offset: 0x0008E918
		private void RefreshTicks()
		{
			this.SetEntryTicked(this.allEntry, false);
			this.SetEntryTicked(this.noneEntry, false);
			for (int k = 0; k < this.pairs.Count; k++)
			{
				this.SetEntryTicked(this.pairs[k].entry, false);
			}
			if (this.Option.All)
			{
				this.SetEntryTicked(this.allEntry, true);
				for (int j = 0; j < this.pairs.Count; j++)
				{
					this.SetEntryTicked(this.pairs[j].entry, true);
				}
				return;
			}
			if (this.Option.None || this.Option.Selection.Count == 0)
			{
				this.SetEntryTicked(this.noneEntry, true);
				return;
			}
			int i;
			Predicate<ItemSetterScreen.Pair> <>9__0;
			int i2;
			for (i = 0; i < this.Option.Selection.Count; i = i2 + 1)
			{
				List<ItemSetterScreen.Pair> list = this.pairs;
				Predicate<ItemSetterScreen.Pair> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((ItemSetterScreen.Pair x) => x.prefabID == this.Option.Selection[i]));
				}
				this.SetEntryTicked(list.Find(match).entry, true);
				i2 = i;
			}
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x00090859 File Offset: 0x0008EA59
		private void SetEntryTicked(RectTransform entry, bool ticked)
		{
			entry.Find("Tick").gameObject.SetActive(ticked);
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x00090874 File Offset: 0x0008EA74
		private void CreateEntries()
		{
			for (int i = 0; i < this.Option.OptionList.Count; i++)
			{
				Console.Log(this.Option.OptionList[i], null);
			}
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x000908B4 File Offset: 0x0008EAB4
		private void DestroyEntries()
		{
			foreach (ItemSetterScreen.Pair pair in this.pairs)
			{
				UnityEngine.Object.Destroy(pair.entry.gameObject);
			}
			this.pairs.Clear();
		}

		// Token: 0x04001A72 RID: 6770
		[Header("Prefabs")]
		public GameObject ListEntryPrefab;

		// Token: 0x04001A73 RID: 6771
		[Header("References")]
		public RectTransform EntryContainer;

		// Token: 0x04001A74 RID: 6772
		public TextMeshProUGUI TitleLabel;

		// Token: 0x04001A75 RID: 6773
		private RectTransform allEntry;

		// Token: 0x04001A76 RID: 6774
		private RectTransform noneEntry;

		// Token: 0x04001A77 RID: 6775
		private List<ItemSetterScreen.Pair> pairs = new List<ItemSetterScreen.Pair>();

		// Token: 0x02000589 RID: 1417
		private class Pair
		{
			// Token: 0x04001A78 RID: 6776
			public string prefabID;

			// Token: 0x04001A79 RID: 6777
			public RectTransform entry;
		}
	}
}
