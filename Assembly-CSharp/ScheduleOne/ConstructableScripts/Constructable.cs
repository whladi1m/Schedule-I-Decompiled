using System;
using System.Collections.Generic;
using EPOOutline;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Construction.Features;
using ScheduleOne.EntityFramework;
using UnityEngine;

namespace ScheduleOne.ConstructableScripts
{
	// Token: 0x02000913 RID: 2323
	public class Constructable : NetworkBehaviour
	{
		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06003EFB RID: 16123 RVA: 0x00109D98 File Offset: 0x00107F98
		public bool IsStatic
		{
			get
			{
				return this.isStatic;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06003EFC RID: 16124 RVA: 0x00109DA0 File Offset: 0x00107FA0
		public string ConstructableName
		{
			get
			{
				return this.constructableName;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06003EFD RID: 16125 RVA: 0x00109DA8 File Offset: 0x00107FA8
		public string ConstructableDescription
		{
			get
			{
				return this.constructableDescription;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06003EFE RID: 16126 RVA: 0x00109DB0 File Offset: 0x00107FB0
		public string ConstructableAssetPath
		{
			get
			{
				return this.constructableAssetPath;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06003EFF RID: 16127 RVA: 0x00109DB8 File Offset: 0x00107FB8
		public string PrefabID
		{
			get
			{
				return this.ID;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06003F00 RID: 16128 RVA: 0x00109DC0 File Offset: 0x00107FC0
		public Sprite ConstructableIcon
		{
			get
			{
				return this.constructableIcon;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06003F01 RID: 16129 RVA: 0x00109DC8 File Offset: 0x00107FC8
		public GameObject _constructionHandler_Asset
		{
			get
			{
				return this.constructionHandler_Asset;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06003F02 RID: 16130 RVA: 0x00109DD0 File Offset: 0x00107FD0
		// (set) Token: 0x06003F03 RID: 16131 RVA: 0x00109DD8 File Offset: 0x00107FD8
		public bool isVisible { get; protected set; } = true;

		// Token: 0x06003F04 RID: 16132 RVA: 0x00109DE4 File Offset: 0x00107FE4
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ConstructableScripts.Constructable_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x000609D2 File Offset: 0x0005EBD2
		public override void OnStartClient()
		{
			base.OnStartClient();
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x00109E03 File Offset: 0x00108003
		public virtual bool CanBeDestroyed(out string reason)
		{
			reason = string.Empty;
			return !this.isStatic;
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x00109E18 File Offset: 0x00108018
		public virtual bool CanBeDestroyed()
		{
			string text;
			return this.CanBeDestroyed(out text);
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x00109E2D File Offset: 0x0010802D
		public virtual void DestroyConstructable(bool callOnServer = true)
		{
			if (this.isDestroyed)
			{
				return;
			}
			this.isDestroyed = true;
			Console.Log("Destroying constructable", null);
			if (callOnServer)
			{
				this.Destroy_Networked();
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x00109E5F File Offset: 0x0010805F
		[ServerRpc(RequireOwnership = false)]
		private void Destroy_Networked()
		{
			this.RpcWriter___Server_Destroy_Networked_2166136261();
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x00109E67 File Offset: 0x00108067
		[ObserversRpc]
		private void DestroyConstructableWrapper()
		{
			this.RpcWriter___Observers_DestroyConstructableWrapper_2166136261();
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x000022C9 File Offset: 0x000004C9
		public virtual bool CanBeModified()
		{
			return true;
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x00014002 File Offset: 0x00012202
		public virtual bool CanBePickedUpByHand()
		{
			return false;
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x00109E6F File Offset: 0x0010806F
		public virtual bool CanBeSelected()
		{
			return !this.isStatic;
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x0003CD29 File Offset: 0x0003AF29
		public virtual string GetBuildableVersionAssetPath()
		{
			return string.Empty;
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x00109E7C File Offset: 0x0010807C
		public void ShowOutline(BuildableItem.EOutlineColor color)
		{
			if (this.outlineEffect == null)
			{
				this.outlineEffect = base.gameObject.AddComponent<Outlinable>();
				this.outlineEffect.OutlineParameters.BlurShift = 0f;
				this.outlineEffect.OutlineParameters.DilateShift = 0.5f;
				this.outlineEffect.OutlineParameters.FillPass.Shader = Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
				foreach (GameObject gameObject in this.outlineRenderers)
				{
					MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						OutlineTarget target = new OutlineTarget(componentsInChildren[i], 0);
						this.outlineEffect.TryAddTarget(target);
					}
				}
			}
			this.outlineEffect.OutlineParameters.Color = BuildableItem.GetColorFromOutlineColorEnum(color);
			Color32 colorFromOutlineColorEnum = BuildableItem.GetColorFromOutlineColorEnum(color);
			colorFromOutlineColorEnum.a = 9;
			this.outlineEffect.OutlineParameters.FillPass.SetColor("_PublicColor", colorFromOutlineColorEnum);
			this.outlineEffect.enabled = true;
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x00109FB8 File Offset: 0x001081B8
		public void HideOutline()
		{
			if (this.outlineEffect != null)
			{
				this.outlineEffect.enabled = false;
			}
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x00066DF6 File Offset: 0x00064FF6
		public virtual Vector3 GetCosmeticCenter()
		{
			return base.transform.position;
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x00109FD4 File Offset: 0x001081D4
		public float GetBoundingBoxLongestSide()
		{
			return Mathf.Max(Mathf.Max(this.boundingBox.size.x, this.boundingBox.size.y), this.boundingBox.size.z);
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x0010A010 File Offset: 0x00108210
		public virtual void SetInvisible()
		{
			this.isVisible = false;
			this.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Invisible"));
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x0010A030 File Offset: 0x00108230
		public virtual void RestoreVisibility()
		{
			this.isVisible = true;
			foreach (Transform transform in base.GetComponentsInChildren<Transform>(true))
			{
				if (transform.gameObject.layer != LayerMask.NameToLayer("Grid"))
				{
					if (this.originalLayers.ContainsKey(transform))
					{
						transform.gameObject.layer = this.originalLayers[transform];
					}
					else
					{
						transform.gameObject.layer = LayerMask.NameToLayer("Default");
					}
				}
			}
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x0010A0B8 File Offset: 0x001082B8
		public void SetLayerRecursively(GameObject go, int layerNumber)
		{
			foreach (Transform transform in go.GetComponentsInChildren<Transform>(true))
			{
				if (transform.gameObject.layer != LayerMask.NameToLayer("Grid"))
				{
					if (transform.gameObject.layer != LayerMask.NameToLayer("Default"))
					{
						if (this.originalLayers.ContainsKey(transform))
						{
							this.originalLayers[transform] = transform.gameObject.layer;
						}
						else
						{
							this.originalLayers.Add(transform, transform.gameObject.layer);
						}
					}
					transform.gameObject.layer = layerNumber;
				}
			}
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x0010A1D0 File Offset: 0x001083D0
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.ConstructableAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.ConstructableAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_Destroy_Networked_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_DestroyConstructableWrapper_2166136261));
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x0010A21C File Offset: 0x0010841C
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ConstructableScripts.ConstructableAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ConstructableScripts.ConstructableAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x0010A22F File Offset: 0x0010842F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x0010A240 File Offset: 0x00108440
		private void RpcWriter___Server_Destroy_Networked_2166136261()
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

		// Token: 0x06003F1B RID: 16155 RVA: 0x0010A2DA File Offset: 0x001084DA
		private void RpcLogic___Destroy_Networked_2166136261()
		{
			Console.Log("Networked", null);
			this.DestroyConstructableWrapper();
			base.Despawn(new DespawnType?(DespawnType.Destroy));
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x0010A2FC File Offset: 0x001084FC
		private void RpcReader___Server_Destroy_Networked_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___Destroy_Networked_2166136261();
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x0010A31C File Offset: 0x0010851C
		private void RpcWriter___Observers_DestroyConstructableWrapper_2166136261()
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

		// Token: 0x06003F1E RID: 16158 RVA: 0x0010A3C5 File Offset: 0x001085C5
		private void RpcLogic___DestroyConstructableWrapper_2166136261()
		{
			Console.Log("Wrapper", null);
			this.DestroyConstructable(false);
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x0010A3DC File Offset: 0x001085DC
		private void RpcReader___Observers_DestroyConstructableWrapper_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___DestroyConstructableWrapper_2166136261();
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x0010A3FC File Offset: 0x001085FC
		protected virtual void dll()
		{
			this.boundingBox.isTrigger = true;
			this.boundingBox.gameObject.layer = LayerMask.NameToLayer("Invisible");
			foreach (Feature feature in this.features)
			{
			}
		}

		// Token: 0x04002D78 RID: 11640
		[Header("Basic settings")]
		[SerializeField]
		protected bool isStatic;

		// Token: 0x04002D79 RID: 11641
		[SerializeField]
		protected string constructableName = "Constructable";

		// Token: 0x04002D7A RID: 11642
		[SerializeField]
		protected string constructableDescription = "Description";

		// Token: 0x04002D7B RID: 11643
		[SerializeField]
		protected string constructableAssetPath = string.Empty;

		// Token: 0x04002D7C RID: 11644
		[SerializeField]
		protected string ID = string.Empty;

		// Token: 0x04002D7D RID: 11645
		[SerializeField]
		protected Sprite constructableIcon;

		// Token: 0x04002D7E RID: 11646
		[Header("Bounds settings")]
		public BoxCollider boundingBox;

		// Token: 0x04002D7F RID: 11647
		[Header("Construction Handler")]
		[SerializeField]
		protected GameObject constructionHandler_Asset;

		// Token: 0x04002D80 RID: 11648
		[Header("Outline settings")]
		[SerializeField]
		protected List<GameObject> outlineRenderers = new List<GameObject>();

		// Token: 0x04002D81 RID: 11649
		protected Outlinable outlineEffect;

		// Token: 0x04002D82 RID: 11650
		[Header("Features")]
		public List<Feature> features = new List<Feature>();

		// Token: 0x04002D84 RID: 11652
		private bool isDestroyed;

		// Token: 0x04002D85 RID: 11653
		private Dictionary<Transform, LayerMask> originalLayers = new Dictionary<Transform, LayerMask>();

		// Token: 0x04002D86 RID: 11654
		private bool dll_Excuted;

		// Token: 0x04002D87 RID: 11655
		private bool dll_Excuted;
	}
}
