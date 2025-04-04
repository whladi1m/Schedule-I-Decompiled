using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009B3 RID: 2483
	public class DailySummary : NetworkSingleton<DailySummary>
	{
		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06004316 RID: 17174 RVA: 0x00119292 File Offset: 0x00117492
		// (set) Token: 0x06004317 RID: 17175 RVA: 0x0011929A File Offset: 0x0011749A
		public bool IsOpen { get; private set; }

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06004318 RID: 17176 RVA: 0x001192A3 File Offset: 0x001174A3
		// (set) Token: 0x06004319 RID: 17177 RVA: 0x001192AB File Offset: 0x001174AB
		public int xpGained { get; private set; }

		// Token: 0x0600431A RID: 17178 RVA: 0x001192B4 File Offset: 0x001174B4
		protected override void Start()
		{
			base.Start();
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepEnd.AddListener(new UnityAction(this.SleepEnd));
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x00119308 File Offset: 0x00117508
		public void Open()
		{
			DailySummary.<>c__DisplayClass21_0 CS$<>8__locals1 = new DailySummary.<>c__DisplayClass21_0();
			CS$<>8__locals1.<>4__this = this;
			this.IsOpen = true;
			this.TitleLabel.text = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.CurrentDay.ToString() + ", Day " + (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.ElapsedDays + 1).ToString();
			CS$<>8__locals1.items = this.itemsSoldByPlayer.Keys.ToArray<string>();
			for (int i = 0; i < this.ProductEntries.Length; i++)
			{
				if (i < CS$<>8__locals1.items.Length)
				{
					ItemDefinition item = Registry.GetItem(CS$<>8__locals1.items[i]);
					this.ProductEntries[i].Find("Quantity").GetComponent<TextMeshProUGUI>().text = this.itemsSoldByPlayer[CS$<>8__locals1.items[i]].ToString() + "x";
					this.ProductEntries[i].Find("Image").GetComponent<Image>().sprite = item.Icon;
					this.ProductEntries[i].Find("Name").GetComponent<TextMeshProUGUI>().text = item.Name;
					this.ProductEntries[i].gameObject.SetActive(true);
				}
				else
				{
					this.ProductEntries[i].gameObject.SetActive(false);
				}
			}
			this.PlayerEarningsLabel.text = MoneyManager.FormatAmount(this.moneyEarnedByPlayer, false, false);
			this.DealerEarningsLabel.text = MoneyManager.FormatAmount(this.moneyEarnedByDealers, false, false);
			this.XPGainedLabel.text = this.xpGained.ToString() + " XP";
			this.Anim.Play("Daily summary 1");
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			base.StartCoroutine(CS$<>8__locals1.<Open>g__Wait|0());
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x00119503 File Offset: 0x00117703
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			this.Anim.Stop();
			this.Anim.Play("Daily summary close");
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x00119541 File Offset: 0x00117741
		private void SleepEnd()
		{
			this.ClearStats();
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x0011954C File Offset: 0x0011774C
		[ObserversRpc]
		public void AddSoldItem(string id, int amount)
		{
			this.RpcWriter___Observers_AddSoldItem_3643459082(id, amount);
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x00119567 File Offset: 0x00117767
		[ObserversRpc]
		public void AddPlayerMoney(float amount)
		{
			this.RpcWriter___Observers_AddPlayerMoney_431000436(amount);
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x00119573 File Offset: 0x00117773
		[ObserversRpc]
		public void AddDealerMoney(float amount)
		{
			this.RpcWriter___Observers_AddDealerMoney_431000436(amount);
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x0011957F File Offset: 0x0011777F
		[ObserversRpc]
		public void AddXP(int xp)
		{
			this.RpcWriter___Observers_AddXP_3316948804(xp);
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x0011958B File Offset: 0x0011778B
		private void ClearStats()
		{
			this.itemsSoldByPlayer.Clear();
			this.moneyEarnedByPlayer = 0f;
			this.moneyEarnedByDealers = 0f;
			this.xpGained = 0;
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x001195C8 File Offset: 0x001177C8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.UI.DailySummaryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.UI.DailySummaryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_AddSoldItem_3643459082));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_AddPlayerMoney_431000436));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_AddDealerMoney_431000436));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_AddXP_3316948804));
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x00119648 File Offset: 0x00117848
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.UI.DailySummaryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.UI.DailySummaryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x00119661 File Offset: 0x00117861
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x00119670 File Offset: 0x00117870
		private void RpcWriter___Observers_AddSoldItem_3643459082(string id, int amount)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(id);
			writer.WriteInt32(amount, AutoPackType.Packed);
			base.SendObserversRpc(0U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x00119738 File Offset: 0x00117938
		public void RpcLogic___AddSoldItem_3643459082(string id, int amount)
		{
			if (this.itemsSoldByPlayer.ContainsKey(id))
			{
				Dictionary<string, int> dictionary = this.itemsSoldByPlayer;
				dictionary[id] += amount;
				return;
			}
			this.itemsSoldByPlayer.Add(id, amount);
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x0011977C File Offset: 0x0011797C
		private void RpcReader___Observers_AddSoldItem_3643459082(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			int amount = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddSoldItem_3643459082(id, amount);
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x001197C4 File Offset: 0x001179C4
		private void RpcWriter___Observers_AddPlayerMoney_431000436(float amount)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(amount, AutoPackType.Unpacked);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x0011987F File Offset: 0x00117A7F
		public void RpcLogic___AddPlayerMoney_431000436(float amount)
		{
			this.moneyEarnedByPlayer += amount;
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x00119890 File Offset: 0x00117A90
		private void RpcReader___Observers_AddPlayerMoney_431000436(PooledReader PooledReader0, Channel channel)
		{
			float amount = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddPlayerMoney_431000436(amount);
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x001198C8 File Offset: 0x00117AC8
		private void RpcWriter___Observers_AddDealerMoney_431000436(float amount)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(amount, AutoPackType.Unpacked);
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x00119983 File Offset: 0x00117B83
		public void RpcLogic___AddDealerMoney_431000436(float amount)
		{
			this.moneyEarnedByDealers += amount;
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x00119994 File Offset: 0x00117B94
		private void RpcReader___Observers_AddDealerMoney_431000436(PooledReader PooledReader0, Channel channel)
		{
			float amount = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddDealerMoney_431000436(amount);
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x001199CC File Offset: 0x00117BCC
		private void RpcWriter___Observers_AddXP_3316948804(int xp)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(xp, AutoPackType.Packed);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x00119A87 File Offset: 0x00117C87
		public void RpcLogic___AddXP_3316948804(int xp)
		{
			this.xpGained += xp;
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x00119A98 File Offset: 0x00117C98
		private void RpcReader___Observers_AddXP_3316948804(PooledReader PooledReader0, Channel channel)
		{
			int xp = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddXP_3316948804(xp);
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x00119ACE File Offset: 0x00117CCE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040030F8 RID: 12536
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040030F9 RID: 12537
		public RectTransform Container;

		// Token: 0x040030FA RID: 12538
		public Animation Anim;

		// Token: 0x040030FB RID: 12539
		public TextMeshProUGUI TitleLabel;

		// Token: 0x040030FC RID: 12540
		public RectTransform[] ProductEntries;

		// Token: 0x040030FD RID: 12541
		public TextMeshProUGUI PlayerEarningsLabel;

		// Token: 0x040030FE RID: 12542
		public TextMeshProUGUI DealerEarningsLabel;

		// Token: 0x040030FF RID: 12543
		public TextMeshProUGUI XPGainedLabel;

		// Token: 0x04003100 RID: 12544
		public UnityEvent onClosed;

		// Token: 0x04003101 RID: 12545
		private Dictionary<string, int> itemsSoldByPlayer = new Dictionary<string, int>();

		// Token: 0x04003102 RID: 12546
		private float moneyEarnedByPlayer;

		// Token: 0x04003103 RID: 12547
		private float moneyEarnedByDealers;

		// Token: 0x04003105 RID: 12549
		private bool dll_Excuted;

		// Token: 0x04003106 RID: 12550
		private bool dll_Excuted;
	}
}
