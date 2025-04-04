using System;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Casino
{
	// Token: 0x0200075A RID: 1882
	public class SlotMachine : NetworkBehaviour
	{
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x0600335E RID: 13150 RVA: 0x000D688F File Offset: 0x000D4A8F
		// (set) Token: 0x0600335F RID: 13151 RVA: 0x000D6897 File Offset: 0x000D4A97
		public bool IsSpinning { get; private set; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06003360 RID: 13152 RVA: 0x000D68A0 File Offset: 0x000D4AA0
		private int currentBetAmount
		{
			get
			{
				return SlotMachine.BetAmounts[this.currentBetIndex];
			}
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x000D68B0 File Offset: 0x000D4AB0
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.SlotMachine_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x000D68CF File Offset: 0x000D4ACF
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.currentBetIndex != 1)
			{
				this.SetBetIndex(connection, this.currentBetIndex);
			}
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x000D68EE File Offset: 0x000D4AEE
		private void DownHovered()
		{
			if (this.IsSpinning)
			{
				this.DownButton.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.DownButton.SetInteractableState(InteractableObject.EInteractableState.Default);
			this.DownButton.SetMessage("Decrease bet");
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x000D6921 File Offset: 0x000D4B21
		private void DownInteracted()
		{
			if (this.onDownPressed != null)
			{
				this.onDownPressed.Invoke();
			}
			this.SendBetIndex(this.currentBetIndex - 1);
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x000D6944 File Offset: 0x000D4B44
		private void UpHovered()
		{
			if (this.IsSpinning)
			{
				this.UpButton.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.UpButton.SetInteractableState(InteractableObject.EInteractableState.Default);
			this.UpButton.SetMessage("Increase bet");
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x000D6977 File Offset: 0x000D4B77
		private void UpInteracted()
		{
			if (this.onUpPressed != null)
			{
				this.onUpPressed.Invoke();
			}
			this.SendBetIndex(this.currentBetIndex + 1);
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x000D699C File Offset: 0x000D4B9C
		private void HandleHovered()
		{
			if (this.IsSpinning)
			{
				this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			int currentBetAmount = this.currentBetAmount;
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance < (float)currentBetAmount)
			{
				this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				this.HandleIntObj.SetMessage("Insufficient cash");
				return;
			}
			this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
			this.HandleIntObj.SetMessage("Pull handle");
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x000D6A0C File Offset: 0x000D4C0C
		[Button]
		public void HandleInteracted()
		{
			if (this.IsSpinning)
			{
				return;
			}
			if (this.onHandlePulled != null)
			{
				this.onHandlePulled.Invoke();
			}
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance((float)(-(float)this.currentBetAmount), true, false);
			this.SendStartSpin(Player.Local.LocalConnection, this.currentBetAmount);
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x000D6A5F File Offset: 0x000D4C5F
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendBetIndex(int index)
		{
			this.RpcWriter___Server_SendBetIndex_3316948804(index);
			this.RpcLogic___SendBetIndex_3316948804(index);
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x000D6A75 File Offset: 0x000D4C75
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetBetIndex(NetworkConnection conn, int index)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetBetIndex_2681120339(conn, index);
				this.RpcLogic___SetBetIndex_2681120339(conn, index);
			}
			else
			{
				this.RpcWriter___Target_SetBetIndex_2681120339(conn, index);
			}
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x000D6AAC File Offset: 0x000D4CAC
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendStartSpin(NetworkConnection spinner, int betAmount)
		{
			this.RpcWriter___Server_SendStartSpin_2681120339(spinner, betAmount);
			this.RpcLogic___SendStartSpin_2681120339(spinner, betAmount);
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x000D6AD8 File Offset: 0x000D4CD8
		[ObserversRpc(RunLocally = true)]
		public void StartSpin(NetworkConnection spinner, SlotMachine.ESymbol[] symbols, int betAmount)
		{
			this.RpcWriter___Observers_StartSpin_2659526290(spinner, symbols, betAmount);
			this.RpcLogic___StartSpin_2659526290(spinner, symbols, betAmount);
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x000D6B09 File Offset: 0x000D4D09
		private SlotMachine.EOutcome EvaluateOutcome(SlotMachine.ESymbol[] outcome)
		{
			if (this.IsUniform(outcome))
			{
				if (outcome[0] == SlotMachine.ESymbol.Seven)
				{
					return SlotMachine.EOutcome.Jackpot;
				}
				if (outcome[0] == SlotMachine.ESymbol.Bell)
				{
					return SlotMachine.EOutcome.BigWin;
				}
				if (this.IsFruit(outcome[0]))
				{
					return SlotMachine.EOutcome.SmallWin;
				}
			}
			if (this.IsAllFruit(outcome))
			{
				return SlotMachine.EOutcome.MiniWin;
			}
			return SlotMachine.EOutcome.NoWin;
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x000D6B3D File Offset: 0x000D4D3D
		private int GetWinAmount(SlotMachine.EOutcome outcome, int betAmount)
		{
			switch (outcome)
			{
			case SlotMachine.EOutcome.Jackpot:
				return betAmount * 100;
			case SlotMachine.EOutcome.BigWin:
				return betAmount * 25;
			case SlotMachine.EOutcome.SmallWin:
				return betAmount * 10;
			case SlotMachine.EOutcome.MiniWin:
				return betAmount * 2;
			default:
				return 0;
			}
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x000D6B6C File Offset: 0x000D4D6C
		private void DisplayOutcome(SlotMachine.EOutcome outcome, int winAmount)
		{
			TextMeshProUGUI[] winAmountLabels = this.WinAmountLabels;
			for (int i = 0; i < winAmountLabels.Length; i++)
			{
				winAmountLabels[i].text = MoneyManager.FormatAmount((float)winAmount, false, false);
			}
			if (outcome == SlotMachine.EOutcome.Jackpot)
			{
				this.ScreenAnimation.Play(this.JackpotAnimation.name);
				ParticleSystem[] jackpotParticles = this.JackpotParticles;
				for (int i = 0; i < jackpotParticles.Length; i++)
				{
					jackpotParticles[i].Play();
				}
				return;
			}
			if (outcome == SlotMachine.EOutcome.BigWin)
			{
				this.ScreenAnimation.Play(this.BigWinAnimation.name);
				this.BigWinSound.Play();
				return;
			}
			if (outcome == SlotMachine.EOutcome.SmallWin)
			{
				this.ScreenAnimation.Play(this.SmallWinAnimation.name);
				this.SmallWinSound.Play();
				return;
			}
			if (outcome == SlotMachine.EOutcome.MiniWin)
			{
				this.ScreenAnimation.Play(this.MiniWinAnimation.name);
				this.MiniWinSound.Play();
			}
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x000D6C4B File Offset: 0x000D4E4B
		public static SlotMachine.ESymbol GetRandomSymbol()
		{
			if (Application.isEditor)
			{
				return SlotMachine.ESymbol.Seven;
			}
			return (SlotMachine.ESymbol)UnityEngine.Random.Range(0, Enum.GetValues(typeof(SlotMachine.ESymbol)).Length);
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x000D6C70 File Offset: 0x000D4E70
		private bool IsFruit(SlotMachine.ESymbol symbol)
		{
			return symbol == SlotMachine.ESymbol.Cherry || symbol == SlotMachine.ESymbol.Lemon || symbol == SlotMachine.ESymbol.Grape || symbol == SlotMachine.ESymbol.Watermelon;
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x000D6C84 File Offset: 0x000D4E84
		private bool IsAllFruit(SlotMachine.ESymbol[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				if (!this.IsFruit(symbols[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x000D6CB0 File Offset: 0x000D4EB0
		private bool IsUniform(SlotMachine.ESymbol[] symbols)
		{
			for (int i = 1; i < symbols.Length; i++)
			{
				if (symbols[i] != symbols[i - 1])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x000D6CD8 File Offset: 0x000D4ED8
		[Button]
		public void SimulateMany()
		{
			int num = 100;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int i = 0; i < num; i++)
			{
				num2--;
				SlotMachine.ESymbol[] array = new SlotMachine.ESymbol[this.Reels.Length];
				for (int j = 0; j < this.Reels.Length; j++)
				{
					array[j] = SlotMachine.GetRandomSymbol();
				}
				SlotMachine.EOutcome eoutcome = this.EvaluateOutcome(array);
				if (eoutcome == SlotMachine.EOutcome.MiniWin)
				{
					num4++;
				}
				if (eoutcome == SlotMachine.EOutcome.SmallWin)
				{
					num3++;
				}
				if (eoutcome == SlotMachine.EOutcome.BigWin)
				{
					num5++;
				}
				if (eoutcome == SlotMachine.EOutcome.Jackpot)
				{
					num6++;
				}
				int winAmount = this.GetWinAmount(eoutcome, 1);
				num2 += winAmount;
			}
			Console.Log("Simulated " + num.ToString() + " spins. Net win: " + num2.ToString(), null);
			Console.Log(string.Concat(new string[]
			{
				"Mini wins: ",
				num4.ToString(),
				" Small wins: ",
				num3.ToString(),
				" Big wins: ",
				num5.ToString(),
				" Jackpots: ",
				num6.ToString()
			}), null);
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x000D6E1C File Offset: 0x000D501C
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.SlotMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.SlotMachineAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendBetIndex_3316948804));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetBetIndex_2681120339));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetBetIndex_2681120339));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendStartSpin_2681120339));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_StartSpin_2659526290));
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x000D6EAD File Offset: 0x000D50AD
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.SlotMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.SlotMachineAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x000D6EC0 File Offset: 0x000D50C0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x000D6ED0 File Offset: 0x000D50D0
		private void RpcWriter___Server_SendBetIndex_3316948804(int index)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(index, AutoPackType.Packed);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x000D6F7C File Offset: 0x000D517C
		private void RpcLogic___SendBetIndex_3316948804(int index)
		{
			this.SetBetIndex(null, index);
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x000D6F88 File Offset: 0x000D5188
		private void RpcReader___Server_SendBetIndex_3316948804(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int index = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendBetIndex_3316948804(index);
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x000D6FCC File Offset: 0x000D51CC
		private void RpcWriter___Observers_SetBetIndex_2681120339(NetworkConnection conn, int index)
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
			writer.WriteInt32(index, AutoPackType.Packed);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x000D7087 File Offset: 0x000D5287
		public void RpcLogic___SetBetIndex_2681120339(NetworkConnection conn, int index)
		{
			this.currentBetIndex = Mathf.Clamp(index, 0, SlotMachine.BetAmounts.Length - 1);
			this.BetAmountLabel.text = MoneyManager.FormatAmount((float)this.currentBetAmount, false, false);
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x000D70B8 File Offset: 0x000D52B8
		private void RpcReader___Observers_SetBetIndex_2681120339(PooledReader PooledReader0, Channel channel)
		{
			int index = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetBetIndex_2681120339(null, index);
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x000D70FC File Offset: 0x000D52FC
		private void RpcWriter___Target_SetBetIndex_2681120339(NetworkConnection conn, int index)
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
			writer.WriteInt32(index, AutoPackType.Packed);
			base.SendTargetRpc(2U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x000D71B8 File Offset: 0x000D53B8
		private void RpcReader___Target_SetBetIndex_2681120339(PooledReader PooledReader0, Channel channel)
		{
			int index = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetBetIndex_2681120339(base.LocalConnection, index);
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x000D71F4 File Offset: 0x000D53F4
		private void RpcWriter___Server_SendStartSpin_2681120339(NetworkConnection spinner, int betAmount)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkConnection(spinner);
			writer.WriteInt32(betAmount, AutoPackType.Packed);
			base.SendServerRpc(3U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x000D72B0 File Offset: 0x000D54B0
		public void RpcLogic___SendStartSpin_2681120339(NetworkConnection spinner, int betAmount)
		{
			SlotMachine.ESymbol[] array = new SlotMachine.ESymbol[this.Reels.Length];
			for (int i = 0; i < this.Reels.Length; i++)
			{
				array[i] = SlotMachine.GetRandomSymbol();
			}
			this.StartSpin(spinner, array, betAmount);
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x000D72F0 File Offset: 0x000D54F0
		private void RpcReader___Server_SendStartSpin_2681120339(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection spinner = PooledReader0.ReadNetworkConnection();
			int betAmount = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendStartSpin_2681120339(spinner, betAmount);
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x000D7344 File Offset: 0x000D5544
		private void RpcWriter___Observers_StartSpin_2659526290(NetworkConnection spinner, SlotMachine.ESymbol[] symbols, int betAmount)
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
			writer.WriteNetworkConnection(spinner);
			writer.Write___ScheduleOne.Casino.SlotMachine/ESymbol[]FishNet.Serializing.Generated(symbols);
			writer.WriteInt32(betAmount, AutoPackType.Packed);
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x000D741C File Offset: 0x000D561C
		public void RpcLogic___StartSpin_2659526290(NetworkConnection spinner, SlotMachine.ESymbol[] symbols, int betAmount)
		{
			SlotMachine.<>c__DisplayClass41_0 CS$<>8__locals1 = new SlotMachine.<>c__DisplayClass41_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.symbols = symbols;
			CS$<>8__locals1.betAmount = betAmount;
			CS$<>8__locals1.spinner = spinner;
			if (this.IsSpinning)
			{
				return;
			}
			this.IsSpinning = true;
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<StartSpin>g__Spin|0());
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x000D746C File Offset: 0x000D566C
		private void RpcReader___Observers_StartSpin_2659526290(PooledReader PooledReader0, Channel channel)
		{
			NetworkConnection spinner = PooledReader0.ReadNetworkConnection();
			SlotMachine.ESymbol[] symbols = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Casino.SlotMachine/ESymbol[]FishNet.Serializing.Generateds(PooledReader0);
			int betAmount = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartSpin_2659526290(spinner, symbols, betAmount);
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x000D74D0 File Offset: 0x000D56D0
		private void dll()
		{
			this.DownButton.onHovered.AddListener(new UnityAction(this.DownHovered));
			this.DownButton.onInteractStart.AddListener(new UnityAction(this.DownInteracted));
			this.UpButton.onHovered.AddListener(new UnityAction(this.UpHovered));
			this.UpButton.onInteractStart.AddListener(new UnityAction(this.UpInteracted));
			this.HandleIntObj.onHovered.AddListener(new UnityAction(this.HandleHovered));
			this.HandleIntObj.onInteractStart.AddListener(new UnityAction(this.HandleInteracted));
			this.SetBetIndex(null, this.currentBetIndex);
		}

		// Token: 0x040024BB RID: 9403
		public static int[] BetAmounts = new int[]
		{
			5,
			10,
			25,
			50,
			100
		};

		// Token: 0x040024BD RID: 9405
		[Header("References")]
		public InteractableObject DownButton;

		// Token: 0x040024BE RID: 9406
		public InteractableObject UpButton;

		// Token: 0x040024BF RID: 9407
		public InteractableObject HandleIntObj;

		// Token: 0x040024C0 RID: 9408
		public TextMeshPro BetAmountLabel;

		// Token: 0x040024C1 RID: 9409
		public SlotReel[] Reels;

		// Token: 0x040024C2 RID: 9410
		public AudioSourceController SpinLoop;

		// Token: 0x040024C3 RID: 9411
		public Animation ScreenAnimation;

		// Token: 0x040024C4 RID: 9412
		public ParticleSystem[] JackpotParticles;

		// Token: 0x040024C5 RID: 9413
		[Header("Win Animations")]
		public TextMeshProUGUI[] WinAmountLabels;

		// Token: 0x040024C6 RID: 9414
		public AnimationClip MiniWinAnimation;

		// Token: 0x040024C7 RID: 9415
		public AnimationClip SmallWinAnimation;

		// Token: 0x040024C8 RID: 9416
		public AnimationClip BigWinAnimation;

		// Token: 0x040024C9 RID: 9417
		public AnimationClip JackpotAnimation;

		// Token: 0x040024CA RID: 9418
		public AudioSourceController MiniWinSound;

		// Token: 0x040024CB RID: 9419
		public AudioSourceController SmallWinSound;

		// Token: 0x040024CC RID: 9420
		public AudioSourceController BigWinSound;

		// Token: 0x040024CD RID: 9421
		public AudioSourceController JackpotSound;

		// Token: 0x040024CE RID: 9422
		public UnityEvent onDownPressed;

		// Token: 0x040024CF RID: 9423
		public UnityEvent onUpPressed;

		// Token: 0x040024D0 RID: 9424
		public UnityEvent onHandlePulled;

		// Token: 0x040024D1 RID: 9425
		private int currentBetIndex = 1;

		// Token: 0x040024D2 RID: 9426
		private bool dll_Excuted;

		// Token: 0x040024D3 RID: 9427
		private bool dll_Excuted;

		// Token: 0x0200075B RID: 1883
		public enum ESymbol
		{
			// Token: 0x040024D5 RID: 9429
			Cherry,
			// Token: 0x040024D6 RID: 9430
			Lemon,
			// Token: 0x040024D7 RID: 9431
			Grape,
			// Token: 0x040024D8 RID: 9432
			Watermelon,
			// Token: 0x040024D9 RID: 9433
			Bell,
			// Token: 0x040024DA RID: 9434
			Seven
		}

		// Token: 0x0200075C RID: 1884
		public enum EOutcome
		{
			// Token: 0x040024DC RID: 9436
			Jackpot,
			// Token: 0x040024DD RID: 9437
			BigWin,
			// Token: 0x040024DE RID: 9438
			SmallWin,
			// Token: 0x040024DF RID: 9439
			MiniWin,
			// Token: 0x040024E0 RID: 9440
			NoWin
		}
	}
}
