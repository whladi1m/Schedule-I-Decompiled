using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerScripts.Health
{
	// Token: 0x020005FE RID: 1534
	public class PlayerHealth : NetworkBehaviour
	{
		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06002840 RID: 10304 RVA: 0x000A5CCA File Offset: 0x000A3ECA
		// (set) Token: 0x06002841 RID: 10305 RVA: 0x000A5CD2 File Offset: 0x000A3ED2
		public bool IsAlive { get; protected set; } = true;

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06002842 RID: 10306 RVA: 0x000A5CDB File Offset: 0x000A3EDB
		// (set) Token: 0x06002843 RID: 10307 RVA: 0x000A5CE3 File Offset: 0x000A3EE3
		public float CurrentHealth { get; protected set; } = 100f;

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06002844 RID: 10308 RVA: 0x000A5CEC File Offset: 0x000A3EEC
		// (set) Token: 0x06002845 RID: 10309 RVA: 0x000A5CF4 File Offset: 0x000A3EF4
		public float TimeSinceLastDamage { get; protected set; }

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06002846 RID: 10310 RVA: 0x000A5CFD File Offset: 0x000A3EFD
		public bool CanTakeDamage
		{
			get
			{
				return this.IsAlive && !Player.Local.IsArrested && !Player.Local.IsUnconscious;
			}
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x000A5D22 File Offset: 0x000A3F22
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.PlayerScripts.Health.PlayerHealth_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x000A5D38 File Offset: 0x000A3F38
		private void Start()
		{
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x000A5D94 File Offset: 0x000A3F94
		[ObserversRpc]
		public void TakeDamage(float damage, bool flinch = true, bool playBloodMist = true)
		{
			this.RpcWriter___Observers_TakeDamage_3505310624(damage, flinch, playBloodMist);
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000A5DB3 File Offset: 0x000A3FB3
		private void Update()
		{
			this.TimeSinceLastDamage += Time.deltaTime;
			if (this.IsAlive && this.AfflictedWithLethalEffect)
			{
				this.TakeDamage(15f * Time.deltaTime, false, false);
			}
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000A5DEA File Offset: 0x000A3FEA
		private void MinPass()
		{
			if (this.IsAlive && this.CurrentHealth < 100f && this.TimeSinceLastDamage > 30f)
			{
				this.RecoverHealth(0.5f);
			}
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000A5E19 File Offset: 0x000A4019
		public void SetAfflictedWithLethalEffect(bool value)
		{
			this.AfflictedWithLethalEffect = value;
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000A5E24 File Offset: 0x000A4024
		public void RecoverHealth(float recovery)
		{
			if (this.CurrentHealth == 0f)
			{
				Console.LogWarning("RecoverHealth called on dead player. Use Revive() instead.", null);
				return;
			}
			this.CurrentHealth = Mathf.Clamp(this.CurrentHealth + recovery, 0f, 100f);
			if (this.onHealthChanged != null)
			{
				this.onHealthChanged.Invoke(this.CurrentHealth);
			}
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000A5E80 File Offset: 0x000A4080
		public void SetHealth(float health)
		{
			this.CurrentHealth = Mathf.Clamp(health, 0f, 100f);
			if (this.onHealthChanged != null)
			{
				this.onHealthChanged.Invoke(this.CurrentHealth);
			}
			if (this.CurrentHealth <= 0f)
			{
				this.SendDie();
			}
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000A5ECF File Offset: 0x000A40CF
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendDie()
		{
			this.RpcWriter___Server_SendDie_2166136261();
			this.RpcLogic___SendDie_2166136261();
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000A5EE0 File Offset: 0x000A40E0
		[ObserversRpc(RunLocally = true)]
		public void Die()
		{
			this.RpcWriter___Observers_Die_2166136261();
			this.RpcLogic___Die_2166136261();
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000A5EF9 File Offset: 0x000A40F9
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendRevive(Vector3 position, Quaternion rotation)
		{
			this.RpcWriter___Server_SendRevive_3848837105(position, rotation);
			this.RpcLogic___SendRevive_3848837105(position, rotation);
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x000A5F18 File Offset: 0x000A4118
		[ObserversRpc(RunLocally = true, ExcludeOwner = true)]
		public void Revive(Vector3 position, Quaternion rotation)
		{
			this.RpcWriter___Observers_Revive_3848837105(position, rotation);
			this.RpcLogic___Revive_3848837105(position, rotation);
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x000A5F41 File Offset: 0x000A4141
		[ObserversRpc]
		public void PlayBloodMist()
		{
			this.RpcWriter___Observers_PlayBloodMist_2166136261();
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x000A5F70 File Offset: 0x000A4170
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.Health.PlayerHealthAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.Health.PlayerHealthAssembly-CSharp.dll_Excuted = true;
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_TakeDamage_3505310624));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_SendDie_2166136261));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_Die_2166136261));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendRevive_3848837105));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_Revive_3848837105));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_PlayBloodMist_2166136261));
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x000A6018 File Offset: 0x000A4218
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.PlayerScripts.Health.PlayerHealthAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.PlayerScripts.Health.PlayerHealthAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x000A602B File Offset: 0x000A422B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x000A603C File Offset: 0x000A423C
		private void RpcWriter___Observers_TakeDamage_3505310624(float damage, bool flinch = true, bool playBloodMist = true)
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
			writer.WriteSingle(damage, AutoPackType.Unpacked);
			writer.WriteBoolean(flinch);
			writer.WriteBoolean(playBloodMist);
			base.SendObserversRpc(0U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x000A6114 File Offset: 0x000A4314
		public void RpcLogic___TakeDamage_3505310624(float damage, bool flinch = true, bool playBloodMist = true)
		{
			if (!this.IsAlive)
			{
				return;
			}
			if (!this.CanTakeDamage)
			{
				Console.LogWarning("Player cannot take damage right now.", null);
				return;
			}
			this.CurrentHealth = Mathf.Clamp(this.CurrentHealth - damage, 0f, 100f);
			Console.Log(damage.ToString() + " damange taken. New health: " + this.CurrentHealth.ToString(), null);
			this.TimeSinceLastDamage = 0f;
			if (this.onHealthChanged != null)
			{
				this.onHealthChanged.Invoke(this.CurrentHealth);
			}
			if (this.Player.IsOwner)
			{
				if (flinch && PlayerSingleton<PlayerCamera>.InstanceExists)
				{
					PlayerSingleton<PlayerCamera>.Instance.JoltCamera();
				}
				if (this.CurrentHealth <= 0f)
				{
					this.SendDie();
				}
			}
			if (playBloodMist)
			{
				this.PlayBloodMist();
			}
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x000A61E4 File Offset: 0x000A43E4
		private void RpcReader___Observers_TakeDamage_3505310624(PooledReader PooledReader0, Channel channel)
		{
			float damage = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			bool flinch = PooledReader0.ReadBoolean();
			bool playBloodMist = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___TakeDamage_3505310624(damage, flinch, playBloodMist);
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x000A623C File Offset: 0x000A443C
		private void RpcWriter___Server_SendDie_2166136261()
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
			base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x000A62D6 File Offset: 0x000A44D6
		public void RpcLogic___SendDie_2166136261()
		{
			this.Die();
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x000A62E0 File Offset: 0x000A44E0
		private void RpcReader___Server_SendDie_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendDie_2166136261();
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000A6310 File Offset: 0x000A4510
		private void RpcWriter___Observers_Die_2166136261()
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

		// Token: 0x06002860 RID: 10336 RVA: 0x000A63BC File Offset: 0x000A45BC
		public void RpcLogic___Die_2166136261()
		{
			if (!this.IsAlive)
			{
				Console.LogWarning("Already dead!", null);
				return;
			}
			this.IsAlive = false;
			Player player = this.Player;
			Debug.Log(((player != null) ? player.ToString() : null) + " died.");
			if (this.onDie != null)
			{
				this.onDie.Invoke();
			}
			Debug.Log("Dead!");
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000A6424 File Offset: 0x000A4624
		private void RpcReader___Observers_Die_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Die_2166136261();
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000A6450 File Offset: 0x000A4650
		private void RpcWriter___Server_SendRevive_3848837105(Vector3 position, Quaternion rotation)
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
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			base.SendServerRpc(3U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000A6509 File Offset: 0x000A4709
		public void RpcLogic___SendRevive_3848837105(Vector3 position, Quaternion rotation)
		{
			this.Revive(position, rotation);
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x000A6514 File Offset: 0x000A4714
		private void RpcReader___Server_SendRevive_3848837105(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendRevive_3848837105(position, rotation);
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000A6568 File Offset: 0x000A4768
		private void RpcWriter___Observers_Revive_3848837105(Vector3 position, Quaternion rotation)
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
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, true);
			writer.Store();
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x000A6630 File Offset: 0x000A4830
		public void RpcLogic___Revive_3848837105(Vector3 position, Quaternion rotation)
		{
			if (this.IsAlive)
			{
				Console.LogWarning("Revive called on living player. Use RecoverHealth() instead.", null);
				return;
			}
			this.CurrentHealth = 100f;
			this.IsAlive = true;
			if (this.onHealthChanged != null)
			{
				this.onHealthChanged.Invoke(this.CurrentHealth);
			}
			if (this.onRevive != null)
			{
				this.onRevive.Invoke();
			}
			if (base.IsOwner)
			{
				Singleton<HUD>.Instance.canvas.enabled = true;
				Player.Local.Energy.RestoreEnergy();
				PlayerSingleton<PlayerMovement>.Instance.Teleport(position);
				Player.Local.transform.rotation = rotation;
				PlayerSingleton<PlayerCamera>.Instance.ResetRotation();
			}
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x000A66DC File Offset: 0x000A48DC
		private void RpcReader___Observers_Revive_3848837105(PooledReader PooledReader0, Channel channel)
		{
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Revive_3848837105(position, rotation);
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x000A6730 File Offset: 0x000A4930
		private void RpcWriter___Observers_PlayBloodMist_2166136261()
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
			base.SendObserversRpc(5U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x000A67D9 File Offset: 0x000A49D9
		public void RpcLogic___PlayBloodMist_2166136261()
		{
			LayerUtility.SetLayerRecursively(this.BloodParticles.gameObject, LayerMask.NameToLayer("Default"));
			this.BloodParticles.Play();
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x000A6800 File Offset: 0x000A4A00
		private void RpcReader___Observers_PlayBloodMist_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___PlayBloodMist_2166136261();
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x000A6820 File Offset: 0x000A4A20
		private void dll()
		{
			Singleton<SleepCanvas>.Instance.onSleepFullyFaded.AddListener(delegate()
			{
				this.SetHealth(100f);
			});
		}

		// Token: 0x04001D7C RID: 7548
		public const float MAX_HEALTH = 100f;

		// Token: 0x04001D7D RID: 7549
		public const float HEALTH_RECOVERY_PER_MINUTE = 0.5f;

		// Token: 0x04001D81 RID: 7553
		[Header("References")]
		public Player Player;

		// Token: 0x04001D82 RID: 7554
		public ParticleSystem BloodParticles;

		// Token: 0x04001D83 RID: 7555
		public UnityEvent<float> onHealthChanged;

		// Token: 0x04001D84 RID: 7556
		public UnityEvent onDie;

		// Token: 0x04001D85 RID: 7557
		public UnityEvent onRevive;

		// Token: 0x04001D86 RID: 7558
		private bool AfflictedWithLethalEffect;

		// Token: 0x04001D87 RID: 7559
		private bool dll_Excuted;

		// Token: 0x04001D88 RID: 7560
		private bool dll_Excuted;
	}
}
