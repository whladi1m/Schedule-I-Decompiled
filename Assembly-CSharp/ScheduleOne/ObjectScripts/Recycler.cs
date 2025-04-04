using System;
using System.Collections;
using System.Collections.Generic;
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
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.Misc;
using ScheduleOne.Money;
using ScheduleOne.Trash;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B69 RID: 2921
	public class Recycler : NetworkBehaviour
	{
		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06004DD2 RID: 19922 RVA: 0x0014847A File Offset: 0x0014667A
		// (set) Token: 0x06004DD3 RID: 19923 RVA: 0x00148482 File Offset: 0x00146682
		public Recycler.EState State { get; protected set; }

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06004DD4 RID: 19924 RVA: 0x0014848B File Offset: 0x0014668B
		// (set) Token: 0x06004DD5 RID: 19925 RVA: 0x00148493 File Offset: 0x00146693
		public bool IsHatchOpen { get; private set; }

		// Token: 0x06004DD6 RID: 19926 RVA: 0x0014849C File Offset: 0x0014669C
		public void Start()
		{
			this.HandleIntObj.onInteractStart.AddListener(new UnityAction(this.HandleInteracted));
			this.ButtonIntObj.onInteractStart.AddListener(new UnityAction(this.ButtonInteracted));
			this.CashIntObj.onInteractStart.AddListener(new UnityAction(this.CashInteracted));
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06004DD7 RID: 19927 RVA: 0x00148523 File Offset: 0x00146723
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SetState(connection, this.State, true);
		}

		// Token: 0x06004DD8 RID: 19928 RVA: 0x0014853A File Offset: 0x0014673A
		private void OnDestroy()
		{
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x0014856C File Offset: 0x0014676C
		private void MinPass()
		{
			if (this.State == Recycler.EState.HatchOpen)
			{
				this.OpenHatchInstruction.gameObject.SetActive(false);
				this.InsertTrashInstruction.gameObject.SetActive(false);
				this.PressBeginInstruction.gameObject.SetActive(false);
				this.ProcessingScreen.gameObject.SetActive(false);
				if (this.GetTrash().Length != 0)
				{
					this.ButtonIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
					this.ButtonLight.isOn = true;
					this.PressBeginInstruction.gameObject.SetActive(true);
					return;
				}
				this.ButtonIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				this.ButtonLight.isOn = false;
				this.InsertTrashInstruction.gameObject.SetActive(true);
			}
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x00148625 File Offset: 0x00146825
		public void HandleInteracted()
		{
			this.SendState(Recycler.EState.HatchOpen);
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x00148630 File Offset: 0x00146830
		public void ButtonInteracted()
		{
			this.ProcessingLabel.text = "Processing...";
			this.ValueLabel.text = MoneyManager.FormatAmount(0f, false, false);
			this.PressSound.Play();
			this.SendState(Recycler.EState.Processing);
			base.StartCoroutine(this.Process(true));
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x00148684 File Offset: 0x00146884
		public void CashInteracted()
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(this.cashValue, true, false);
			NetworkSingleton<MoneyManager>.Instance.ChangeLifetimeEarnings(this.cashValue);
			this.SendState(Recycler.EState.HatchClosed);
			this.BankNote.gameObject.SetActive(false);
			this.cashValue = 0f;
			this.SendCashCollected();
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x001486DC File Offset: 0x001468DC
		[ServerRpc(RequireOwnership = false)]
		private void SendCashCollected()
		{
			this.RpcWriter___Server_SendCashCollected_2166136261();
		}

		// Token: 0x06004DDE RID: 19934 RVA: 0x001486E4 File Offset: 0x001468E4
		[ObserversRpc(RunLocally = true)]
		private void CashCollected()
		{
			this.RpcWriter___Observers_CashCollected_2166136261();
			this.RpcLogic___CashCollected_2166136261();
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x001486F2 File Offset: 0x001468F2
		[ObserversRpc(RunLocally = true)]
		private void EnableCash()
		{
			this.RpcWriter___Observers_EnableCash_2166136261();
			this.RpcLogic___EnableCash_2166136261();
		}

		// Token: 0x06004DE0 RID: 19936 RVA: 0x00148700 File Offset: 0x00146900
		[ObserversRpc(RunLocally = true)]
		private void SetCashValue(float amount)
		{
			this.RpcWriter___Observers_SetCashValue_431000436(amount);
			this.RpcLogic___SetCashValue_431000436(amount);
		}

		// Token: 0x06004DE1 RID: 19937 RVA: 0x00148716 File Offset: 0x00146916
		private IEnumerator Process(bool startedByLocalPlayer)
		{
			yield return new WaitForSeconds(0.5f);
			if (this.onStart != null)
			{
				this.onStart.Invoke();
			}
			TrashItem[] trash = this.GetTrash();
			if (startedByLocalPlayer)
			{
				int num = trash.Length;
				float num2 = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("TrashRecycled") + (float)num;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("TrashRecycled", num2.ToString(), true);
				if (num2 >= 500f)
				{
					Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.UPSTANDING_CITIZEN);
				}
			}
			float value = 0f;
			TrashItem[] array = trash;
			int j = 0;
			while (j < array.Length)
			{
				TrashItem trashItem = array[j];
				if (trashItem is TrashBag)
				{
					using (List<TrashContent.Entry>.Enumerator enumerator = ((TrashBag)trashItem).Content.Entries.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							TrashContent.Entry entry = enumerator.Current;
							value += (float)(entry.UnitValue * entry.Quantity);
						}
						goto IL_14A;
					}
					goto IL_135;
				}
				goto IL_135;
				IL_14A:
				if (InstanceFinder.IsServer)
				{
					trashItem.DestroyTrash();
				}
				j++;
				continue;
				IL_135:
				value += (float)trashItem.SellValue;
				goto IL_14A;
			}
			if (this.cashValue <= 0f)
			{
				this.SetCashValue(value);
			}
			float lerpTime = 1.5f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				float t = i / lerpTime;
				float amount = Mathf.Lerp(0f, this.cashValue, t);
				this.ValueLabel.text = MoneyManager.FormatAmount(amount, true, false);
				yield return new WaitForEndOfFrame();
			}
			if (this.onStop != null)
			{
				this.onStop.Invoke();
			}
			this.ProcessingLabel.text = "Thank you";
			this.ValueLabel.text = MoneyManager.FormatAmount(value, false, false);
			this.DoneSound.Play();
			yield return new WaitForSeconds(0.3f);
			this.CashEjectSound.Play();
			this.CashAnim.Play();
			yield return new WaitForSeconds(0.25f);
			if (InstanceFinder.IsServer)
			{
				this.EnableCash();
			}
			yield break;
		}

		// Token: 0x06004DE2 RID: 19938 RVA: 0x0014872C File Offset: 0x0014692C
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendState(Recycler.EState state)
		{
			this.RpcWriter___Server_SendState_3569965459(state);
			this.RpcLogic___SendState_3569965459(state);
		}

		// Token: 0x06004DE3 RID: 19939 RVA: 0x00148744 File Offset: 0x00146944
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetState(NetworkConnection conn, Recycler.EState state, bool force = false)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetState_3790170803(conn, state, force);
				this.RpcLogic___SetState_3790170803(conn, state, force);
			}
			else
			{
				this.RpcWriter___Target_SetState_3790170803(conn, state, force);
			}
		}

		// Token: 0x06004DE4 RID: 19940 RVA: 0x00148794 File Offset: 0x00146994
		private void SetHatchOpen(bool open)
		{
			if (open == this.IsHatchOpen)
			{
				return;
			}
			this.IsHatchOpen = open;
			if (this.IsHatchOpen)
			{
				this.OpenSound.Play();
				this.HatchAnim.Play("Recycler open");
				return;
			}
			this.CloseSound.Play();
			this.HatchAnim.Play("Recycler close");
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x001487F4 File Offset: 0x001469F4
		private TrashItem[] GetTrash()
		{
			List<TrashItem> list = new List<TrashItem>();
			Vector3 center = this.CheckCollider.transform.TransformPoint(this.CheckCollider.center);
			Vector3 halfExtents = Vector3.Scale(this.CheckCollider.size, this.CheckCollider.transform.lossyScale) * 0.5f;
			Collider[] array = Physics.OverlapBox(center, halfExtents, this.CheckCollider.transform.rotation, this.DetectionMask, QueryTriggerInteraction.Collide);
			for (int i = 0; i < array.Length; i++)
			{
				TrashItem componentInParent = array[i].GetComponentInParent<TrashItem>();
				if (componentInParent != null && !list.Contains(componentInParent))
				{
					list.Add(componentInParent);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x001488AC File Offset: 0x00146AAC
		private void OnDrawGizmos()
		{
			Vector3 center = this.CheckCollider.transform.TransformPoint(this.CheckCollider.center);
			Vector3 a = Vector3.Scale(this.CheckCollider.size, this.CheckCollider.transform.lossyScale) * 0.5f;
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(center, a * 2f);
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x0014891C File Offset: 0x00146B1C
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.RecyclerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.RecyclerAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendCashCollected_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_CashCollected_2166136261));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_EnableCash_2166136261));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetCashValue_431000436));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_SendState_3569965459));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_SetState_3790170803));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_SetState_3790170803));
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x001489DB File Offset: 0x00146BDB
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.RecyclerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.RecyclerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x001489EE File Offset: 0x00146BEE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x001489FC File Offset: 0x00146BFC
		private void RpcWriter___Server_SendCashCollected_2166136261()
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
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004DEC RID: 19948 RVA: 0x00148A96 File Offset: 0x00146C96
		private void RpcLogic___SendCashCollected_2166136261()
		{
			this.CashCollected();
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x00148AA0 File Offset: 0x00146CA0
		private void RpcReader___Server_SendCashCollected_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendCashCollected_2166136261();
		}

		// Token: 0x06004DEE RID: 19950 RVA: 0x00148AC0 File Offset: 0x00146CC0
		private void RpcWriter___Observers_CashCollected_2166136261()
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
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x00148B69 File Offset: 0x00146D69
		private void RpcLogic___CashCollected_2166136261()
		{
			this.SendState(Recycler.EState.HatchClosed);
			this.BankNote.gameObject.SetActive(false);
			this.cashValue = 0f;
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x00148B90 File Offset: 0x00146D90
		private void RpcReader___Observers_CashCollected_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CashCollected_2166136261();
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x00148BBC File Offset: 0x00146DBC
		private void RpcWriter___Observers_EnableCash_2166136261()
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
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004DF2 RID: 19954 RVA: 0x00148C65 File Offset: 0x00146E65
		private void RpcLogic___EnableCash_2166136261()
		{
			this.CashIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x06004DF3 RID: 19955 RVA: 0x00148C74 File Offset: 0x00146E74
		private void RpcReader___Observers_EnableCash_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EnableCash_2166136261();
		}

		// Token: 0x06004DF4 RID: 19956 RVA: 0x00148CA0 File Offset: 0x00146EA0
		private void RpcWriter___Observers_SetCashValue_431000436(float amount)
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
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x00148D5B File Offset: 0x00146F5B
		private void RpcLogic___SetCashValue_431000436(float amount)
		{
			this.cashValue = amount;
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x00148D64 File Offset: 0x00146F64
		private void RpcReader___Observers_SetCashValue_431000436(PooledReader PooledReader0, Channel channel)
		{
			float amount = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCashValue_431000436(amount);
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x00148DA4 File Offset: 0x00146FA4
		private void RpcWriter___Server_SendState_3569965459(Recycler.EState state)
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
			writer.Write___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generated(state);
			base.SendServerRpc(4U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06004DF8 RID: 19960 RVA: 0x00148E4B File Offset: 0x0014704B
		public void RpcLogic___SendState_3569965459(Recycler.EState state)
		{
			this.SetState(null, state, false);
		}

		// Token: 0x06004DF9 RID: 19961 RVA: 0x00148E58 File Offset: 0x00147058
		private void RpcReader___Server_SendState_3569965459(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Recycler.EState state = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendState_3569965459(state);
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x00148E98 File Offset: 0x00147098
		private void RpcWriter___Observers_SetState_3790170803(NetworkConnection conn, Recycler.EState state, bool force = false)
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
			writer.Write___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generated(state);
			writer.WriteBoolean(force);
			base.SendObserversRpc(5U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x00148F5C File Offset: 0x0014715C
		private void RpcLogic___SetState_3790170803(NetworkConnection conn, Recycler.EState state, bool force = false)
		{
			if (this.State == state && !force)
			{
				return;
			}
			this.State = state;
			this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			this.ButtonIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			this.CashIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			this.OpenHatchInstruction.gameObject.SetActive(false);
			this.InsertTrashInstruction.gameObject.SetActive(false);
			this.PressBeginInstruction.gameObject.SetActive(false);
			this.ProcessingScreen.gameObject.SetActive(false);
			this.ButtonLight.isOn = false;
			this.Cash.gameObject.SetActive(false);
			switch (this.State)
			{
			case Recycler.EState.HatchClosed:
				this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.OpenHatchInstruction.gameObject.SetActive(true);
				return;
			case Recycler.EState.HatchOpen:
				if (this.GetTrash().Length != 0)
				{
					this.ButtonIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
					this.ButtonLight.isOn = true;
					this.PressBeginInstruction.gameObject.SetActive(true);
				}
				else
				{
					this.InsertTrashInstruction.gameObject.SetActive(true);
				}
				this.SetHatchOpen(true);
				return;
			case Recycler.EState.Processing:
				base.StartCoroutine(this.Process(false));
				this.ProcessingScreen.gameObject.SetActive(true);
				this.ButtonAnim.Play();
				this.SetHatchOpen(false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x001490BC File Offset: 0x001472BC
		private void RpcReader___Observers_SetState_3790170803(PooledReader PooledReader0, Channel channel)
		{
			Recycler.EState state = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generateds(PooledReader0);
			bool force = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetState_3790170803(null, state, force);
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x0014910C File Offset: 0x0014730C
		private void RpcWriter___Target_SetState_3790170803(NetworkConnection conn, Recycler.EState state, bool force = false)
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
			writer.Write___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generated(state);
			writer.WriteBoolean(force);
			base.SendTargetRpc(6U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x001491D0 File Offset: 0x001473D0
		private void RpcReader___Target_SetState_3790170803(PooledReader PooledReader0, Channel channel)
		{
			Recycler.EState state = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generateds(PooledReader0);
			bool force = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetState_3790170803(base.LocalConnection, state, force);
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x001489EE File Offset: 0x00146BEE
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003AE9 RID: 15081
		public LayerMask DetectionMask;

		// Token: 0x04003AEA RID: 15082
		[Header("References")]
		public InteractableObject HandleIntObj;

		// Token: 0x04003AEB RID: 15083
		public InteractableObject ButtonIntObj;

		// Token: 0x04003AEC RID: 15084
		public InteractableObject CashIntObj;

		// Token: 0x04003AED RID: 15085
		public ToggleableLight ButtonLight;

		// Token: 0x04003AEE RID: 15086
		public Animation ButtonAnim;

		// Token: 0x04003AEF RID: 15087
		public Animation HatchAnim;

		// Token: 0x04003AF0 RID: 15088
		public Animation CashAnim;

		// Token: 0x04003AF1 RID: 15089
		public RectTransform OpenHatchInstruction;

		// Token: 0x04003AF2 RID: 15090
		public RectTransform InsertTrashInstruction;

		// Token: 0x04003AF3 RID: 15091
		public RectTransform PressBeginInstruction;

		// Token: 0x04003AF4 RID: 15092
		public RectTransform ProcessingScreen;

		// Token: 0x04003AF5 RID: 15093
		public TextMeshProUGUI ProcessingLabel;

		// Token: 0x04003AF6 RID: 15094
		public TextMeshProUGUI ValueLabel;

		// Token: 0x04003AF7 RID: 15095
		public BoxCollider CheckCollider;

		// Token: 0x04003AF8 RID: 15096
		public Transform Cash;

		// Token: 0x04003AF9 RID: 15097
		public GameObject BankNote;

		// Token: 0x04003AFA RID: 15098
		[Header("Sound")]
		public AudioSourceController OpenSound;

		// Token: 0x04003AFB RID: 15099
		public AudioSourceController CloseSound;

		// Token: 0x04003AFC RID: 15100
		public AudioSourceController PressSound;

		// Token: 0x04003AFD RID: 15101
		public AudioSourceController DoneSound;

		// Token: 0x04003AFE RID: 15102
		public AudioSourceController CashEjectSound;

		// Token: 0x04003AFF RID: 15103
		private float cashValue;

		// Token: 0x04003B00 RID: 15104
		public UnityEvent onStart;

		// Token: 0x04003B01 RID: 15105
		public UnityEvent onStop;

		// Token: 0x04003B02 RID: 15106
		private bool dll_Excuted;

		// Token: 0x04003B03 RID: 15107
		private bool dll_Excuted;

		// Token: 0x02000B6A RID: 2922
		public enum EState
		{
			// Token: 0x04003B05 RID: 15109
			HatchClosed,
			// Token: 0x04003B06 RID: 15110
			HatchOpen,
			// Token: 0x04003B07 RID: 15111
			Processing
		}
	}
}
