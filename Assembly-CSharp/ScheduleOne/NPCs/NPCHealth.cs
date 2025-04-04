using System;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x0200044D RID: 1101
	[RequireComponent(typeof(NPCHealth))]
	[DisallowMultipleComponent]
	public class NPCHealth : NetworkBehaviour
	{
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x00064FB4 File Offset: 0x000631B4
		// (set) Token: 0x060016E0 RID: 5856 RVA: 0x00064FBC File Offset: 0x000631BC
		public float Health
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<Health>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.sync___set_value_<Health>k__BackingField(value, true);
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x00064FC6 File Offset: 0x000631C6
		// (set) Token: 0x060016E2 RID: 5858 RVA: 0x00064FCE File Offset: 0x000631CE
		public bool IsDead { get; private set; }

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060016E3 RID: 5859 RVA: 0x00064FD7 File Offset: 0x000631D7
		// (set) Token: 0x060016E4 RID: 5860 RVA: 0x00064FDF File Offset: 0x000631DF
		public bool IsKnockedOut { get; private set; }

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060016E5 RID: 5861 RVA: 0x00064FE8 File Offset: 0x000631E8
		// (set) Token: 0x060016E6 RID: 5862 RVA: 0x00064FF0 File Offset: 0x000631F0
		public int DaysPassedSinceDeath { get; private set; }

		// Token: 0x060016E7 RID: 5863 RVA: 0x00064FFC File Offset: 0x000631FC
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.NPCHealth_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x0006501B File Offset: 0x0006321B
		private void OnDestroy()
		{
			ScheduleOne.GameTime.TimeManager.onSleepStart = (Action)Delegate.Remove(ScheduleOne.GameTime.TimeManager.onSleepStart, new Action(this.SleepStart));
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x0006503D File Offset: 0x0006323D
		public override void OnStartServer()
		{
			base.OnStartServer();
			this.Health = this.MaxHealth;
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x00065051 File Offset: 0x00063251
		public void Load(NPCHealthData healthData)
		{
			this.Health = healthData.Health;
			this.DaysPassedSinceDeath = healthData.DaysPassedSinceDeath;
			if (this.IsDead)
			{
				this.Die();
				return;
			}
			if (this.Health == 0f)
			{
				this.KnockOut();
			}
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x0006508D File Offset: 0x0006328D
		private void Update()
		{
			if (!this.IsDead && this.AfflictedWithLethalEffect)
			{
				this.TakeDamage(15f * Time.deltaTime, true);
			}
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x000650B1 File Offset: 0x000632B1
		public void SetAfflictedWithLethalEffect(bool value)
		{
			this.AfflictedWithLethalEffect = value;
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x000650BC File Offset: 0x000632BC
		public void SleepStart()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.npc.IsConscious)
			{
				Console.Log(this.npc.fullName + " Dead: " + this.IsDead.ToString(), null);
				if (this.IsDead)
				{
					int daysPassedSinceDeath = this.DaysPassedSinceDeath;
					this.DaysPassedSinceDeath = daysPassedSinceDeath + 1;
					if (this.DaysPassedSinceDeath >= 3 || this.npc.IsImportant)
					{
						this.Revive();
					}
				}
				else
				{
					this.Revive();
				}
			}
			if (this.npc.IsConscious)
			{
				this.Health = this.MaxHealth;
			}
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x0006515C File Offset: 0x0006335C
		public void TakeDamage(float damage, bool isLethal = true)
		{
			if (this.IsDead)
			{
				return;
			}
			Console.Log(this.npc.fullName + " has taken " + damage.ToString() + " damage.", null);
			this.Health -= damage;
			if (this.Health <= 0f)
			{
				this.Health = 0f;
				if (!this.Invincible)
				{
					if (isLethal)
					{
						if (!this.IsDead)
						{
							this.Die();
							return;
						}
					}
					else if (!this.IsKnockedOut)
					{
						this.KnockOut();
					}
				}
			}
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x000651E8 File Offset: 0x000633E8
		public virtual void Die()
		{
			if (this.Invincible)
			{
				return;
			}
			Console.Log(this.npc.fullName + " has died.", null);
			this.IsDead = true;
			this.npc.behaviour.DeadBehaviour.Enable_Networked(null);
			if (this.onDie != null)
			{
				this.onDie.Invoke();
			}
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x0006524C File Offset: 0x0006344C
		public virtual void KnockOut()
		{
			if (this.Invincible)
			{
				return;
			}
			Console.Log(this.npc.fullName + " has been knocked out.", null);
			this.IsKnockedOut = true;
			this.npc.behaviour.UnconsciousBehaviour.Enable_Networked(null);
			if (this.onKnockedOut != null)
			{
				this.onKnockedOut.Invoke();
			}
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x000652B0 File Offset: 0x000634B0
		public virtual void Revive()
		{
			Console.Log(this.npc.fullName + " has been revived.", null);
			this.IsDead = false;
			this.IsKnockedOut = false;
			this.Health = this.MaxHealth;
			this.npc.behaviour.DeadBehaviour.SendDisable();
			this.npc.behaviour.UnconsciousBehaviour.SendDisable();
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00065330 File Offset: 0x00063530
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCHealthAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCHealthAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<Health>k__BackingField = new SyncVar<float>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<Health>k__BackingField);
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.NPCs.NPCHealth));
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x0006538B File Offset: 0x0006358B
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCHealthAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCHealthAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<Health>k__BackingField.SetRegistered();
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x000653A9 File Offset: 0x000635A9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x060016F6 RID: 5878 RVA: 0x000653B7 File Offset: 0x000635B7
		// (set) Token: 0x060016F7 RID: 5879 RVA: 0x000653BF File Offset: 0x000635BF
		public float SyncAccessor_<Health>k__BackingField
		{
			get
			{
				return this.<Health>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<Health>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<Health>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x000653FC File Offset: 0x000635FC
		public virtual bool NPCHealth(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_<Health>k__BackingField(this.syncVar___<Health>k__BackingField.GetValue(true), true);
				return true;
			}
			float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			this.sync___set_value_<Health>k__BackingField(value, Boolean2);
			return true;
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00065454 File Offset: 0x00063654
		protected virtual void dll()
		{
			this.npc = base.GetComponent<NPC>();
			ScheduleOne.GameTime.TimeManager.onSleepStart = (Action)Delegate.Remove(ScheduleOne.GameTime.TimeManager.onSleepStart, new Action(this.SleepStart));
			ScheduleOne.GameTime.TimeManager.onSleepStart = (Action)Delegate.Combine(ScheduleOne.GameTime.TimeManager.onSleepStart, new Action(this.SleepStart));
		}

		// Token: 0x040014DF RID: 5343
		public const int REVIVE_DAYS = 3;

		// Token: 0x040014E4 RID: 5348
		[Header("Settings")]
		public bool Invincible;

		// Token: 0x040014E5 RID: 5349
		public float MaxHealth = 100f;

		// Token: 0x040014E6 RID: 5350
		private NPC npc;

		// Token: 0x040014E7 RID: 5351
		public UnityEvent onDie;

		// Token: 0x040014E8 RID: 5352
		public UnityEvent onKnockedOut;

		// Token: 0x040014E9 RID: 5353
		private bool AfflictedWithLethalEffect;

		// Token: 0x040014EA RID: 5354
		public SyncVar<float> syncVar___<Health>k__BackingField;

		// Token: 0x040014EB RID: 5355
		private bool dll_Excuted;

		// Token: 0x040014EC RID: 5356
		private bool dll_Excuted;
	}
}
