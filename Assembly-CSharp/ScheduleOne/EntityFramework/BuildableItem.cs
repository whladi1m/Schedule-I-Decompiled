using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using EPOOutline;
using FishNet;
using FishNet.Component.Ownership;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.EntityFramework
{
	// Token: 0x0200061A RID: 1562
	[RequireComponent(typeof(PredictedSpawn))]
	public class BuildableItem : NetworkBehaviour, IGUIDRegisterable, ISaveable
	{
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002919 RID: 10521 RVA: 0x000A970B File Offset: 0x000A790B
		// (set) Token: 0x0600291A RID: 10522 RVA: 0x000A9713 File Offset: 0x000A7913
		public ItemInstance ItemInstance { get; protected set; }

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x0600291B RID: 10523 RVA: 0x000A971C File Offset: 0x000A791C
		// (set) Token: 0x0600291C RID: 10524 RVA: 0x000A9724 File Offset: 0x000A7924
		public Property ParentProperty { get; protected set; }

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x0600291D RID: 10525 RVA: 0x000A972D File Offset: 0x000A792D
		// (set) Token: 0x0600291E RID: 10526 RVA: 0x000A9735 File Offset: 0x000A7935
		public bool IsDestroyed { get; protected set; }

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x0600291F RID: 10527 RVA: 0x000A973E File Offset: 0x000A793E
		// (set) Token: 0x06002920 RID: 10528 RVA: 0x000A9746 File Offset: 0x000A7946
		public bool Initialized { get; protected set; }

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06002921 RID: 10529 RVA: 0x000A974F File Offset: 0x000A794F
		// (set) Token: 0x06002922 RID: 10530 RVA: 0x000A9757 File Offset: 0x000A7957
		public Guid GUID { get; protected set; }

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002923 RID: 10531 RVA: 0x000A9760 File Offset: 0x000A7960
		// (set) Token: 0x06002924 RID: 10532 RVA: 0x000A9768 File Offset: 0x000A7968
		public bool IsCulled { get; protected set; }

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06002925 RID: 10533 RVA: 0x000A9771 File Offset: 0x000A7971
		public GameObject BuildHandler
		{
			get
			{
				return this.buildHandler;
			}
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x000A977C File Offset: 0x000A797C
		[Button]
		public void AddChildMeshes()
		{
			foreach (MeshRenderer meshRenderer in new List<MeshRenderer>(this.MeshesToCull))
			{
				foreach (MeshRenderer item in meshRenderer.GetComponentsInChildren<MeshRenderer>())
				{
					if (!this.MeshesToCull.Contains(item))
					{
						this.MeshesToCull.Add(item);
					}
				}
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002927 RID: 10535 RVA: 0x000A9800 File Offset: 0x000A7A00
		// (set) Token: 0x06002928 RID: 10536 RVA: 0x000A9808 File Offset: 0x000A7A08
		public bool LocallyBuilt { get; protected set; }

		// Token: 0x06002929 RID: 10537 RVA: 0x000A9811 File Offset: 0x000A7A11
		public void SetLocallyBuilt()
		{
			this.LocallyBuilt = true;
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600292A RID: 10538 RVA: 0x000A981C File Offset: 0x000A7A1C
		public string SaveFolderName
		{
			get
			{
				return this.ItemInstance.ID + "_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600292B RID: 10539 RVA: 0x000A9859 File Offset: 0x000A7A59
		public string SaveFileName
		{
			get
			{
				return "Data";
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x0600292C RID: 10540 RVA: 0x0004691A File Offset: 0x00044B1A
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600292D RID: 10541 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600292E RID: 10542 RVA: 0x000A9860 File Offset: 0x000A7A60
		// (set) Token: 0x0600292F RID: 10543 RVA: 0x000A9868 File Offset: 0x000A7A68
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002930 RID: 10544 RVA: 0x000A9871 File Offset: 0x000A7A71
		// (set) Token: 0x06002931 RID: 10545 RVA: 0x000A9879 File Offset: 0x000A7A79
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06002932 RID: 10546 RVA: 0x000A9882 File Offset: 0x000A7A82
		// (set) Token: 0x06002933 RID: 10547 RVA: 0x000A988A File Offset: 0x000A7A8A
		public bool HasChanged { get; set; }

		// Token: 0x06002934 RID: 10548 RVA: 0x000A9893 File Offset: 0x000A7A93
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.EntityFramework.BuildableItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x000A98A8 File Offset: 0x000A7AA8
		protected virtual void Start()
		{
			if (!this.isGhost)
			{
				this.InitializeSaveable();
				if (this.GUID == Guid.Empty)
				{
					this.GUID = GUIDManager.GenerateUniqueGUID();
					GUIDManager.RegisterObject(this);
				}
				ActivateDuringBuild[] componentsInChildren = base.transform.GetComponentsInChildren<ActivateDuringBuild>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x000A9910 File Offset: 0x000A7B10
		protected virtual Property GetProperty(Transform searchTransform = null)
		{
			if (searchTransform == null)
			{
				searchTransform = base.transform;
			}
			PropertyContentsContainer componentInParent = searchTransform.GetComponentInParent<PropertyContentsContainer>();
			if (componentInParent != null)
			{
				return componentInParent.Property;
			}
			return searchTransform.GetComponentInParent<Property>();
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000A994B File Offset: 0x000A7B4B
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (!connection.IsLocalClient && this.Initialized)
			{
				this.SendInitToClient(connection);
			}
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x000A996C File Offset: 0x000A7B6C
		protected virtual void SendInitToClient(NetworkConnection conn)
		{
			Console.Log("Sending BuildableItem init to client", null);
			this.ReceiveBuildableItemData(conn, this.ItemInstance, this.GUID.ToString(), this.ParentProperty.PropertyCode);
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x000A99B0 File Offset: 0x000A7BB0
		[ServerRpc(RequireOwnership = false)]
		public void SendBuildableItemData(ItemInstance instance, string GUID, string parentPropertyCode)
		{
			this.RpcWriter___Server_SendBuildableItemData_3537728543(instance, GUID, parentPropertyCode);
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x000A99C4 File Offset: 0x000A7BC4
		[ObserversRpc]
		[TargetRpc]
		public void ReceiveBuildableItemData(NetworkConnection conn, ItemInstance instance, string GUID, string parentPropertyCode)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveBuildableItemData_3859851844(conn, instance, GUID, parentPropertyCode);
			}
			else
			{
				this.RpcWriter___Target_ReceiveBuildableItemData_3859851844(conn, instance, GUID, parentPropertyCode);
			}
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x000A99FC File Offset: 0x000A7BFC
		public virtual void InitializeBuildableItem(ItemInstance instance, string GUID, string parentPropertyCode)
		{
			if (this.Initialized)
			{
				return;
			}
			if (instance == null)
			{
				Console.LogError("InitializeBuildItem: passed null instance", null);
			}
			if (instance.Quantity != 1)
			{
				Console.LogWarning("BuiltadlbeItem initialized with quantity '" + instance.Quantity.ToString() + "'! This should be 1.", null);
			}
			this.Initialized = true;
			this.ItemInstance = instance;
			this.SetGUID(new Guid(GUID));
			this.ParentProperty = Property.Properties.FirstOrDefault((Property p) => p.PropertyCode == parentPropertyCode);
			if (this.ParentProperty == null)
			{
				this.ParentProperty = Business.Businesses.FirstOrDefault((Business b) => b.PropertyCode == parentPropertyCode);
			}
			if (this.ParentProperty != null)
			{
				this.ParentProperty.BuildableItems.Add(this);
				if (this.ParentProperty.IsContentCulled)
				{
					this.SetCulled(true);
				}
			}
			else
			{
				Console.LogError("BuildableItem '" + base.gameObject.name + "' does not have a parent Property!", null);
			}
			ActivateDuringBuild[] componentsInChildren = base.transform.GetComponentsInChildren<ActivateDuringBuild>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(false);
			}
			if (this.onInitialized != null)
			{
				this.onInitialized.Invoke();
			}
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x000A9B46 File Offset: 0x000A7D46
		public bool CanBePickedUp(out string reason)
		{
			if (PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(this.ItemInstance, 1))
			{
				return this.CanBeDestroyed(out reason);
			}
			reason = "Item won't fit in inventory";
			return false;
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x000702CA File Offset: 0x0006E4CA
		public virtual bool CanBeDestroyed(out string reason)
		{
			reason = string.Empty;
			return true;
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x000A9B6C File Offset: 0x000A7D6C
		public virtual void PickupItem()
		{
			string empty = string.Empty;
			if (!this.CanBePickedUp(out empty))
			{
				Console.LogWarning("Item can not be picked up!", null);
				return;
			}
			PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.ItemInstance);
			this.DestroyItem(true);
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x000A9BAC File Offset: 0x000A7DAC
		public virtual void DestroyItem(bool callOnServer = true)
		{
			if (this.IsDestroyed)
			{
				return;
			}
			this.IsDestroyed = true;
			if (callOnServer)
			{
				this.Destroy_Networked();
			}
			if (this.ParentProperty != null)
			{
				this.ParentProperty.BuildableItems.Remove(this);
			}
			if (this.onDestroyed != null)
			{
				this.onDestroyed.Invoke();
			}
			if (this.onDestroyedWithParameter != null)
			{
				this.onDestroyedWithParameter(this);
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x000A9C25 File Offset: 0x000A7E25
		[ServerRpc(RequireOwnership = false)]
		private void Destroy_Networked()
		{
			this.RpcWriter___Server_Destroy_Networked_2166136261();
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x000A9C2D File Offset: 0x000A7E2D
		[ObserversRpc]
		private void DestroyItemWrapper()
		{
			this.RpcWriter___Observers_DestroyItemWrapper_2166136261();
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x000A9C35 File Offset: 0x000A7E35
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x000A9C44 File Offset: 0x000A7E44
		public static Color32 GetColorFromOutlineColorEnum(BuildableItem.EOutlineColor col)
		{
			switch (col)
			{
			case BuildableItem.EOutlineColor.White:
				return Color.white;
			case BuildableItem.EOutlineColor.Blue:
				return new Color32(0, 200, byte.MaxValue, byte.MaxValue);
			case BuildableItem.EOutlineColor.LightBlue:
				return new Color32(120, 225, byte.MaxValue, byte.MaxValue);
			default:
				return Color.white;
			}
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x000A9CA8 File Offset: 0x000A7EA8
		public virtual void ShowOutline(Color color)
		{
			if (this.IsDestroyed || base.gameObject == null)
			{
				return;
			}
			if (this.OutlineEffect == null)
			{
				this.OutlineEffect = base.gameObject.AddComponent<Outlinable>();
				this.OutlineEffect.OutlineParameters.BlurShift = 0f;
				this.OutlineEffect.OutlineParameters.DilateShift = 0.5f;
				this.OutlineEffect.OutlineParameters.FillPass.Shader = Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
				foreach (GameObject gameObject in this.OutlineRenderers)
				{
					MeshRenderer[] array = new MeshRenderer[0];
					if (this.IncludeOutlineRendererChildren)
					{
						array = gameObject.GetComponentsInChildren<MeshRenderer>();
					}
					else
					{
						array = new MeshRenderer[]
						{
							gameObject.GetComponent<MeshRenderer>()
						};
					}
					for (int i = 0; i < array.Length; i++)
					{
						OutlineTarget target = new OutlineTarget(array[i], 0);
						this.OutlineEffect.TryAddTarget(target);
					}
				}
			}
			this.OutlineEffect.OutlineParameters.Color = color;
			Color32 c = color;
			c.a = 9;
			this.OutlineEffect.OutlineParameters.FillPass.SetColor("_PublicColor", c);
			this.OutlineEffect.enabled = true;
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x000A9E18 File Offset: 0x000A8018
		public void ShowOutline(BuildableItem.EOutlineColor color)
		{
			this.ShowOutline(BuildableItem.GetColorFromOutlineColorEnum(color));
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x000A9E2B File Offset: 0x000A802B
		public virtual void HideOutline()
		{
			if (this.IsDestroyed || base.gameObject == null)
			{
				return;
			}
			if (this.OutlineEffect != null)
			{
				this.OutlineEffect.enabled = false;
			}
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x000A9E60 File Offset: 0x000A8060
		public Vector3 GetFurthestPointFromBoundingCollider(Vector3 pos)
		{
			Vector3[] array = new Vector3[8];
			BoxCollider boundingCollider = this.BoundingCollider;
			array[0] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(boundingCollider.size.x, -boundingCollider.size.y, boundingCollider.size.z) * 0.5f);
			array[1] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(-boundingCollider.size.x, -boundingCollider.size.y, boundingCollider.size.z) * 0.5f);
			array[2] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(-boundingCollider.size.x, -boundingCollider.size.y, -boundingCollider.size.z) * 0.5f);
			array[3] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(boundingCollider.size.x, -boundingCollider.size.y, -boundingCollider.size.z) * 0.5f);
			array[4] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(boundingCollider.size.x, boundingCollider.size.y, boundingCollider.size.z) * 0.5f);
			array[5] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(-boundingCollider.size.x, boundingCollider.size.y, boundingCollider.size.z) * 0.5f);
			array[6] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(-boundingCollider.size.x, boundingCollider.size.y, -boundingCollider.size.z) * 0.5f);
			array[7] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(boundingCollider.size.x, boundingCollider.size.y, -boundingCollider.size.z) * 0.5f);
			List<Vector3> list = new List<Vector3>();
			foreach (Vector3 vector in array)
			{
				if (list.Count == 0)
				{
					list.Add(vector);
				}
				else if (Vector3.Distance(pos, vector) > Vector3.Distance(pos, list[0]))
				{
					list.Clear();
					list.Add(vector);
				}
				else if (Mathf.Abs(Vector3.Distance(pos, vector) - Vector3.Distance(pos, list[0])) < 1E-06f)
				{
					list.Add(vector);
				}
			}
			Vector3 a = Vector3.zero;
			for (int j = 0; j < list.Count; j++)
			{
				a += list[j];
			}
			return a / (float)list.Count;
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x000AA1D8 File Offset: 0x000A83D8
		public bool GetPenetration(out float x, out float z, out float y)
		{
			Vector3 a = this.BoundingCollider.transform.TransformPoint(this.BoundingCollider.center);
			float num = this.BoundingCollider.size.x / 2f;
			float num2 = 0f;
			x = 0f;
			z = 0f;
			y = 0f;
			Vector3 vector = a - base.transform.right * num;
			RaycastHit raycastHit;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, base.transform.right, this.BoundingCollider.size.x / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(base.transform.right, -raycastHit.normal) < 5f)
			{
				x = this.BoundingCollider.size.x - Vector3.Distance(vector, raycastHit.point);
				Debug.DrawLine(a - base.transform.right * num, raycastHit.point, Color.green);
			}
			vector = a + base.transform.right * num;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, -base.transform.right, this.BoundingCollider.size.x / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(-base.transform.right, -raycastHit.normal) < 5f)
			{
				float num3 = -(this.BoundingCollider.size.x - Vector3.Distance(vector, raycastHit.point));
				x = num3;
				Debug.DrawLine(a + base.transform.right * num, raycastHit.point, Color.red);
			}
			num = this.BoundingCollider.size.z / 2f;
			vector = a - base.transform.forward * num;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, base.transform.forward, this.BoundingCollider.size.z / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(base.transform.forward, -raycastHit.normal) < 5f)
			{
				z = this.BoundingCollider.size.z - Vector3.Distance(vector, raycastHit.point);
				Debug.DrawLine(a - base.transform.forward * num, raycastHit.point, Color.cyan);
			}
			vector = a + base.transform.forward * num;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, -base.transform.forward, this.BoundingCollider.size.z / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(-base.transform.forward, -raycastHit.normal) < 5f)
			{
				float num4 = -(this.BoundingCollider.size.z - Vector3.Distance(vector, raycastHit.point));
				z = num4;
				Debug.DrawLine(a + base.transform.forward * num, raycastHit.point, Color.yellow);
			}
			num = this.BoundingCollider.size.y / 2f;
			vector = a - base.transform.up * num;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, base.transform.up, this.BoundingCollider.size.y / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(base.transform.forward, -raycastHit.normal) < 5f)
			{
				y = this.BoundingCollider.size.y - Vector3.Distance(vector, raycastHit.point);
				Debug.DrawLine(a - base.transform.up * num, raycastHit.point, Color.cyan);
			}
			vector = a + base.transform.up * num;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, -base.transform.up, this.BoundingCollider.size.y / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(-base.transform.up, -raycastHit.normal) < 5f)
			{
				float num5 = -(this.BoundingCollider.size.y - Vector3.Distance(vector, raycastHit.point));
				y = num5;
				Debug.DrawLine(a + base.transform.up * num, raycastHit.point, Color.yellow);
			}
			return x != 0f || z != 0f || y != 0f;
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x000AA7F8 File Offset: 0x000A89F8
		private bool HasLoS_IgnoreBuildables(Vector3 point)
		{
			RaycastHit raycastHit;
			return !PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(PlayerSingleton<PlayerCamera>.Instance.transform.position, point - PlayerSingleton<PlayerCamera>.Instance.transform.position, Vector3.Distance(point, PlayerSingleton<PlayerCamera>.Instance.transform.position) - 0.01f, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, 0f, 0f);
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x000AA874 File Offset: 0x000A8A74
		public virtual void SetCulled(bool culled)
		{
			this.IsCulled = culled;
			foreach (MeshRenderer meshRenderer in this.MeshesToCull)
			{
				if (!(meshRenderer == null))
				{
					meshRenderer.enabled = !culled;
				}
			}
			foreach (GameObject gameObject in this.GameObjectsToCull)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(!culled);
				}
			}
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x000AA90C File Offset: 0x000A8B0C
		public virtual string GetSaveString()
		{
			return new BuildableItemData(this.GUID, this.ItemInstance, 0).GetJson(true);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x000577B8 File Offset: 0x000559B8
		public virtual List<string> WriteData(string parentFolderPath)
		{
			return new List<string>();
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x000AA964 File Offset: 0x000A8B64
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.EntityFramework.BuildableItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.EntityFramework.BuildableItemAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendBuildableItemData_3537728543));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveBuildableItemData_3859851844));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveBuildableItemData_3859851844));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_Destroy_Networked_2166136261));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_DestroyItemWrapper_2166136261));
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x000AA9F5 File Offset: 0x000A8BF5
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.EntityFramework.BuildableItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.EntityFramework.BuildableItemAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x000AAA08 File Offset: 0x000A8C08
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x000AAA18 File Offset: 0x000A8C18
		private void RpcWriter___Server_SendBuildableItemData_3537728543(ItemInstance instance, string GUID, string parentPropertyCode)
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
			writer.WriteItemInstance(instance);
			writer.WriteString(GUID);
			writer.WriteString(parentPropertyCode);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x000AAAD9 File Offset: 0x000A8CD9
		public void RpcLogic___SendBuildableItemData_3537728543(ItemInstance instance, string GUID, string parentPropertyCode)
		{
			this.ReceiveBuildableItemData(null, instance, GUID, parentPropertyCode);
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x000AAAE8 File Offset: 0x000A8CE8
		private void RpcReader___Server_SendBuildableItemData_3537728543(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string guid = PooledReader0.ReadString();
			string parentPropertyCode = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendBuildableItemData_3537728543(instance, guid, parentPropertyCode);
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x000AAB3C File Offset: 0x000A8D3C
		private void RpcWriter___Observers_ReceiveBuildableItemData_3859851844(NetworkConnection conn, ItemInstance instance, string GUID, string parentPropertyCode)
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
			writer.WriteItemInstance(instance);
			writer.WriteString(GUID);
			writer.WriteString(parentPropertyCode);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000AAC0C File Offset: 0x000A8E0C
		public void RpcLogic___ReceiveBuildableItemData_3859851844(NetworkConnection conn, ItemInstance instance, string GUID, string parentPropertyCode)
		{
			this.InitializeBuildableItem(instance, GUID, parentPropertyCode);
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x000AAC18 File Offset: 0x000A8E18
		private void RpcReader___Observers_ReceiveBuildableItemData_3859851844(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string guid = PooledReader0.ReadString();
			string parentPropertyCode = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveBuildableItemData_3859851844(null, instance, guid, parentPropertyCode);
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x000AAC6C File Offset: 0x000A8E6C
		private void RpcWriter___Target_ReceiveBuildableItemData_3859851844(NetworkConnection conn, ItemInstance instance, string GUID, string parentPropertyCode)
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
			writer.WriteItemInstance(instance);
			writer.WriteString(GUID);
			writer.WriteString(parentPropertyCode);
			base.SendTargetRpc(2U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x000AAD3C File Offset: 0x000A8F3C
		private void RpcReader___Target_ReceiveBuildableItemData_3859851844(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string guid = PooledReader0.ReadString();
			string parentPropertyCode = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveBuildableItemData_3859851844(base.LocalConnection, instance, guid, parentPropertyCode);
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x000AAD98 File Offset: 0x000A8F98
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
			base.SendServerRpc(3U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x000AAE32 File Offset: 0x000A9032
		private void RpcLogic___Destroy_Networked_2166136261()
		{
			this.DestroyItemWrapper();
			base.Despawn(new DespawnType?(DespawnType.Destroy));
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000AAE48 File Offset: 0x000A9048
		private void RpcReader___Server_Destroy_Networked_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___Destroy_Networked_2166136261();
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x000AAE68 File Offset: 0x000A9068
		private void RpcWriter___Observers_DestroyItemWrapper_2166136261()
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
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x000AAF11 File Offset: 0x000A9111
		private void RpcLogic___DestroyItemWrapper_2166136261()
		{
			this.DestroyItem(false);
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x000AAF1C File Offset: 0x000A911C
		private void RpcReader___Observers_DestroyItemWrapper_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___DestroyItemWrapper_2166136261();
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void dll()
		{
		}

		// Token: 0x04001E62 RID: 7778
		[HideInInspector]
		public bool isGhost;

		// Token: 0x04001E63 RID: 7779
		[Header("Build Settings")]
		[SerializeField]
		protected GameObject buildHandler;

		// Token: 0x04001E64 RID: 7780
		public float HoldDistance = 2.5f;

		// Token: 0x04001E65 RID: 7781
		public Transform BuildPoint;

		// Token: 0x04001E66 RID: 7782
		public Transform MidAirCenterPoint;

		// Token: 0x04001E67 RID: 7783
		public BoxCollider BoundingCollider;

		// Token: 0x04001E68 RID: 7784
		[Header("Outline settings")]
		[SerializeField]
		protected List<GameObject> OutlineRenderers = new List<GameObject>();

		// Token: 0x04001E69 RID: 7785
		[SerializeField]
		protected bool IncludeOutlineRendererChildren = true;

		// Token: 0x04001E6A RID: 7786
		protected Outlinable OutlineEffect;

		// Token: 0x04001E6B RID: 7787
		[Header("Culling Settings")]
		public GameObject[] GameObjectsToCull;

		// Token: 0x04001E6C RID: 7788
		public List<MeshRenderer> MeshesToCull;

		// Token: 0x04001E6D RID: 7789
		[Header("Buildable Events")]
		public UnityEvent onInitialized;

		// Token: 0x04001E6E RID: 7790
		public UnityEvent onDestroyed;

		// Token: 0x04001E6F RID: 7791
		public Action<BuildableItem> onDestroyedWithParameter;

		// Token: 0x04001E74 RID: 7796
		private bool dll_Excuted;

		// Token: 0x04001E75 RID: 7797
		private bool dll_Excuted;

		// Token: 0x0200061B RID: 1563
		public enum EOutlineColor
		{
			// Token: 0x04001E77 RID: 7799
			White,
			// Token: 0x04001E78 RID: 7800
			Blue,
			// Token: 0x04001E79 RID: 7801
			LightBlue
		}
	}
}
