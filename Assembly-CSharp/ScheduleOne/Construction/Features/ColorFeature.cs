using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.UI.Construction.Features;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000711 RID: 1809
	public class ColorFeature : Feature
	{
		// Token: 0x06003100 RID: 12544 RVA: 0x000CB6BC File Offset: 0x000C98BC
		public override FI_Base CreateInterface(Transform parent)
		{
			FI_ColorPicker fi_ColorPicker = base.CreateInterface(parent) as FI_ColorPicker;
			fi_ColorPicker.onSelectionChanged.AddListener(new UnityAction<ColorFeature.NamedColor>(this.ApplyColor));
			fi_ColorPicker.onSelectionPurchased.AddListener(new UnityAction<ColorFeature.NamedColor>(this.BuyColor));
			return fi_ColorPicker;
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x000CB6F8 File Offset: 0x000C98F8
		public override void Default()
		{
			this.BuyColor(this.colors[this.defaultColorIndex]);
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x000CB714 File Offset: 0x000C9914
		private void ApplyColor(ColorFeature.NamedColor color)
		{
			for (int i = 0; i < this.colorTargets.Count; i++)
			{
				this.colorTargets[i].material.color = color.color;
			}
			foreach (ColorFeature.SecondaryPaintTarget secondaryPaintTarget in this.secondaryTargets)
			{
				for (int j = 0; j < secondaryPaintTarget.colorTargets.Count; j++)
				{
					secondaryPaintTarget.colorTargets[j].material.color = ColorFeature.ModifyColor(color.color, secondaryPaintTarget.sChange, secondaryPaintTarget.vChange);
				}
			}
		}

		// Token: 0x06003103 RID: 12547 RVA: 0x000CB7D8 File Offset: 0x000C99D8
		public static Color ModifyColor(Color original, float sChange, float vChange)
		{
			float h;
			float num;
			float num2;
			Color.RGBToHSV(original, out h, out num, out num2);
			num = Mathf.Clamp(num + sChange / 100f, 0f, 1f);
			num2 = Mathf.Clamp(num2 + vChange / 100f, 0f, 1f);
			return Color.HSVToRGB(h, num, num2);
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x000CB82B File Offset: 0x000C9A2B
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		protected virtual void SetData(int colorIndex)
		{
			this.RpcWriter___Server_SetData_3316948804(colorIndex);
			this.RpcLogic___SetData_3316948804(colorIndex);
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x000CB841 File Offset: 0x000C9A41
		private void ReceiveData()
		{
			this.ApplyColor(this.colors[this.SyncAccessor_ownedColorIndex]);
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x000CB85A File Offset: 0x000C9A5A
		private void BuyColor(ColorFeature.NamedColor color)
		{
			this.SetData(this.colors.IndexOf(color));
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x000CB898 File Offset: 0x000C9A98
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Construction.Features.ColorFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Construction.Features.ColorFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___ownedColorIndex = new SyncVar<int>(this, 0U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.ownedColorIndex);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetData_3316948804));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Construction.Features.ColorFeature));
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x000CB910 File Offset: 0x000C9B10
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Construction.Features.ColorFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Construction.Features.ColorFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___ownedColorIndex.SetRegistered();
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x000CB934 File Offset: 0x000C9B34
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x000CB944 File Offset: 0x000C9B44
		private void RpcWriter___Server_SetData_3316948804(int colorIndex)
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
			writer.WriteInt32(colorIndex, AutoPackType.Packed);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x000CB9F0 File Offset: 0x000C9BF0
		protected virtual void RpcLogic___SetData_3316948804(int colorIndex)
		{
			if (!base.IsSpawned)
			{
				this.ApplyColor(this.colors[colorIndex]);
				return;
			}
			this.sync___set_value_ownedColorIndex(colorIndex, true);
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x000CBA18 File Offset: 0x000C9C18
		private void RpcReader___Server_SetData_3316948804(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int colorIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetData_3316948804(colorIndex);
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x0600310E RID: 12558 RVA: 0x000CBA5B File Offset: 0x000C9C5B
		// (set) Token: 0x0600310F RID: 12559 RVA: 0x000CBA63 File Offset: 0x000C9C63
		public int SyncAccessor_ownedColorIndex
		{
			get
			{
				return this.ownedColorIndex;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.ownedColorIndex = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___ownedColorIndex.SetValue(value, value);
				}
			}
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x000CBAA0 File Offset: 0x000C9CA0
		public virtual bool ColorFeature(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_ownedColorIndex(this.syncVar___ownedColorIndex.GetValue(true), true);
				return true;
			}
			int value = PooledReader0.ReadInt32(AutoPackType.Packed);
			this.sync___set_value_ownedColorIndex(value, Boolean2);
			return true;
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x000CBAF7 File Offset: 0x000C9CF7
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400230A RID: 8970
		[Header("References")]
		[SerializeField]
		protected List<MeshRenderer> colorTargets = new List<MeshRenderer>();

		// Token: 0x0400230B RID: 8971
		[SerializeField]
		protected List<ColorFeature.SecondaryPaintTarget> secondaryTargets = new List<ColorFeature.SecondaryPaintTarget>();

		// Token: 0x0400230C RID: 8972
		[Header("Color settings")]
		public List<ColorFeature.NamedColor> colors = new List<ColorFeature.NamedColor>();

		// Token: 0x0400230D RID: 8973
		public int defaultColorIndex;

		// Token: 0x0400230E RID: 8974
		[SyncVar]
		public int ownedColorIndex;

		// Token: 0x0400230F RID: 8975
		public SyncVar<int> syncVar___ownedColorIndex;

		// Token: 0x04002310 RID: 8976
		private bool dll_Excuted;

		// Token: 0x04002311 RID: 8977
		private bool dll_Excuted;

		// Token: 0x02000712 RID: 1810
		[Serializable]
		public class NamedColor
		{
			// Token: 0x04002312 RID: 8978
			public string colorName;

			// Token: 0x04002313 RID: 8979
			public Color color;

			// Token: 0x04002314 RID: 8980
			public float price = 100f;
		}

		// Token: 0x02000713 RID: 1811
		[Serializable]
		public class SecondaryPaintTarget
		{
			// Token: 0x04002315 RID: 8981
			public List<MeshRenderer> colorTargets = new List<MeshRenderer>();

			// Token: 0x04002316 RID: 8982
			public float sChange;

			// Token: 0x04002317 RID: 8983
			public float vChange;
		}
	}
}
