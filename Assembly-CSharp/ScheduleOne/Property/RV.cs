using System;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Growing;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Product;
using ScheduleOne.Storage;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Property
{
	// Token: 0x02000805 RID: 2053
	public class RV : Property
	{
		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060037FC RID: 14332 RVA: 0x000ECDCE File Offset: 0x000EAFCE
		// (set) Token: 0x060037FD RID: 14333 RVA: 0x000ECDD6 File Offset: 0x000EAFD6
		public bool _isExploded { get; private set; }

		// Token: 0x060037FE RID: 14334 RVA: 0x000ECDDF File Offset: 0x000EAFDF
		protected override void Start()
		{
			base.Start();
			base.InvokeRepeating("UpdateVariables", 0f, 0.5f);
			NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.OnSleep));
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x000ECE17 File Offset: 0x000EB017
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			bool isExploded = this._isExploded;
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x000ECE28 File Offset: 0x000EB028
		private void UpdateVariables()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this._isExploded)
			{
				return;
			}
			Pot[] array = (from x in this.BuildableItems
			where x is Pot
			select x as Pot).ToArray<Pot>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IsFilledWithSoil)
				{
					num++;
				}
				if (array[i].NormalizedWaterLevel > 0.9f)
				{
					num2++;
				}
				if (array[i].Plant != null)
				{
					num3++;
				}
				if (array[i].AppliedAdditives.Find((Additive x) => x.AdditiveName == "Speed Grow"))
				{
					num4++;
				}
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("RV_Soil_Pots", num.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("RV_Watered_Pots", num2.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("RV_Seed_Pots", num3.ToString(), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("RV_SpeedGrow_Pots", num4.ToString(), true);
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x000ECF84 File Offset: 0x000EB184
		public void Ransack()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			Debug.Log("Ransacking RV");
			foreach (BuildableItem buildableItem in this.BuildableItems)
			{
				IItemSlotOwner itemSlotOwner = null;
				if (buildableItem is IItemSlotOwner)
				{
					itemSlotOwner = (buildableItem as IItemSlotOwner);
				}
				else
				{
					StorageEntity component = buildableItem.GetComponent<StorageEntity>();
					if (component != null)
					{
						itemSlotOwner = component;
					}
				}
				if (itemSlotOwner != null)
				{
					for (int i = 0; i < itemSlotOwner.ItemSlots.Count; i++)
					{
						if (itemSlotOwner.ItemSlots[i].ItemInstance != null && itemSlotOwner.ItemSlots[i].ItemInstance is ProductItemInstance)
						{
							itemSlotOwner.ItemSlots[i].SetQuantity(0, false);
						}
					}
				}
			}
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x000ED06C File Offset: 0x000EB26C
		public override bool ShouldSave()
		{
			return !this._isExploded && base.ShouldSave();
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x000ED07E File Offset: 0x000EB27E
		[TargetRpc]
		public void SetExploded(NetworkConnection conn)
		{
			this.RpcWriter___Target_SetExploded_328543758(conn);
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x000ED08A File Offset: 0x000EB28A
		public void SetExploded()
		{
			this._isExploded = true;
			if (this.onSetExploded != null)
			{
				this.onSetExploded.Invoke();
			}
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x000ED0A6 File Offset: 0x000EB2A6
		private void OnSleep()
		{
			if (this.FXContainer != null)
			{
				this.FXContainer.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x000ED0C7 File Offset: 0x000EB2C7
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.RVAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.RVAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterTargetRpc(5U, new ClientRpcDelegate(this.RpcReader___Target_SetExploded_328543758));
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x000ED0F7 File Offset: 0x000EB2F7
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.RVAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.RVAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x000ED110 File Offset: 0x000EB310
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x000ED120 File Offset: 0x000EB320
		private void RpcWriter___Target_SetExploded_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(5U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x000ED1C8 File Offset: 0x000EB3C8
		public void RpcLogic___SetExploded_328543758(NetworkConnection conn)
		{
			this.SetExploded();
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x000ED1D0 File Offset: 0x000EB3D0
		private void RpcReader___Target_SetExploded_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetExploded_328543758(base.LocalConnection);
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x000ED1F6 File Offset: 0x000EB3F6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040028CC RID: 10444
		public Transform ModelContainer;

		// Token: 0x040028CD RID: 10445
		public Transform FXContainer;

		// Token: 0x040028CE RID: 10446
		public UnityEvent onSetExploded;

		// Token: 0x040028D0 RID: 10448
		private bool dll_Excuted;

		// Token: 0x040028D1 RID: 10449
		private bool dll_Excuted;
	}
}
