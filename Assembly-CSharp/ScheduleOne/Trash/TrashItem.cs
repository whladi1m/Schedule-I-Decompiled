using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dragging;
using ScheduleOne.Equipping;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x0200081C RID: 2076
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Draggable))]
	[RequireComponent(typeof(PhysicsDamageable))]
	public class TrashItem : MonoBehaviour, IGUIDRegisterable, ISaveable
	{
		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060038F4 RID: 14580 RVA: 0x000F0B15 File Offset: 0x000EED15
		// (set) Token: 0x060038F5 RID: 14581 RVA: 0x000F0B1D File Offset: 0x000EED1D
		public Guid GUID { get; protected set; }

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060038F6 RID: 14582 RVA: 0x000F0B26 File Offset: 0x000EED26
		// (set) Token: 0x060038F7 RID: 14583 RVA: 0x000F0B2E File Offset: 0x000EED2E
		public Property CurrentProperty { get; protected set; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060038F8 RID: 14584 RVA: 0x000F0B38 File Offset: 0x000EED38
		public string SaveFolderName
		{
			get
			{
				return "Trash_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x000F0B6C File Offset: 0x000EED6C
		public string SaveFileName
		{
			get
			{
				return "Trash_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060038FA RID: 14586 RVA: 0x0004691A File Offset: 0x00044B1A
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x060038FB RID: 14587 RVA: 0x00014002 File Offset: 0x00012202
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x060038FC RID: 14588 RVA: 0x000F0B9E File Offset: 0x000EED9E
		// (set) Token: 0x060038FD RID: 14589 RVA: 0x000F0BA6 File Offset: 0x000EEDA6
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060038FE RID: 14590 RVA: 0x000F0BAF File Offset: 0x000EEDAF
		// (set) Token: 0x060038FF RID: 14591 RVA: 0x000F0BB7 File Offset: 0x000EEDB7
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06003900 RID: 14592 RVA: 0x000F0BC0 File Offset: 0x000EEDC0
		// (set) Token: 0x06003901 RID: 14593 RVA: 0x000F0BC8 File Offset: 0x000EEDC8
		public bool HasChanged { get; set; }

		// Token: 0x06003902 RID: 14594 RVA: 0x000F0BD4 File Offset: 0x000EEDD4
		protected void Awake()
		{
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Trash"));
			this.RecheckPosition();
			base.InvokeRepeating("RecheckPosition", UnityEngine.Random.Range(0f, 1f), 1f);
			this.SetPhysicsActive(false);
			this.Rigidbody.drag = 0.1f;
			this.Rigidbody.angularDrag = 0.1f;
			this.Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
			this.Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
			this.Rigidbody.sleepThreshold = 0.01f;
			this.Draggable.onDragStart.AddListener(delegate()
			{
				this.SetContinuousCollisionDetection();
			});
			PhysicsDamageable physicsDamageable = base.GetComponent<PhysicsDamageable>();
			if (physicsDamageable == null)
			{
				physicsDamageable = base.gameObject.AddComponent<PhysicsDamageable>();
			}
			PhysicsDamageable physicsDamageable2 = physicsDamageable;
			physicsDamageable2.onImpacted = (Action<Impact>)Delegate.Combine(physicsDamageable2.onImpacted, new Action<Impact>(delegate(Impact impact)
			{
				if (impact.ImpactForce > 0f)
				{
					this.SetContinuousCollisionDetection();
				}
			}));
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x000F0CC4 File Offset: 0x000EEEC4
		protected void Start()
		{
			this.InitializeSaveable();
			TimeManager.onSleepEnd = (Action<int>)Delegate.Combine(TimeManager.onSleepEnd, new Action<int>(this.SleepEnd));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			this.Draggable.onHovered.AddListener(new UnityAction(this.Hovered));
			this.Draggable.onInteracted.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x000F0D58 File Offset: 0x000EEF58
		protected void OnValidate()
		{
			if (this.Rigidbody == null)
			{
				this.Rigidbody = base.GetComponent<Rigidbody>();
			}
			if (this.Draggable == null)
			{
				this.Draggable = base.GetComponent<Draggable>();
			}
			if (this.colliders == null || this.colliders.Length == 0)
			{
				this.colliders = base.GetComponentsInChildren<Collider>();
			}
			if (base.GetComponent<ImpactSoundEntity>() == null)
			{
				base.gameObject.AddComponent<ImpactSoundEntity>();
			}
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x000F0DD0 File Offset: 0x000EEFD0
		protected void MinPass()
		{
			if (this == null || base.transform == null)
			{
				return;
			}
			if (Time.time - this.timeOnPhysicsEnabled > 30f)
			{
				float num = Vector3.SqrMagnitude(PlayerSingleton<PlayerMovement>.Instance.transform.position - base.transform.position);
				this.SetCollidersEnabled(num < 900f);
			}
			if (base.transform.position.y < -100f && InstanceFinder.IsServer)
			{
				Console.LogWarning("Trash item fell below the world. Destroying.", null);
				this.DestroyTrash();
			}
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x000045B1 File Offset: 0x000027B1
		protected void SleepEnd(int mins)
		{
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x000F0E6C File Offset: 0x000EF06C
		protected void Hovered()
		{
			if (Equippable_TrashGrabber.IsEquipped && this.CanGoInContainer)
			{
				if (Equippable_TrashGrabber.Instance.GetCapacity() > 0)
				{
					this.Draggable.IntObj.SetMessage("Pick up");
					this.Draggable.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
					return;
				}
				this.Draggable.IntObj.SetMessage("Bin is full");
				this.Draggable.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
			}
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x000F0EE2 File Offset: 0x000EF0E2
		protected void Interacted()
		{
			if (Equippable_TrashGrabber.IsEquipped && this.CanGoInContainer && Equippable_TrashGrabber.Instance.GetCapacity() > 0)
			{
				Equippable_TrashGrabber.Instance.PickupTrash(this);
			}
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x000F0F0C File Offset: 0x000EF10C
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
			string text = this.GUID.ToString();
			if (text[text.Length - 1] != '1')
			{
				text = text.Substring(0, text.Length - 1) + "1";
			}
			else
			{
				text = text.Substring(0, text.Length - 1) + "2";
			}
			this.Draggable.SetGUID(new Guid(text));
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x000F0F94 File Offset: 0x000EF194
		public void SetVelocity(Vector3 velocity)
		{
			this.Rigidbody.velocity = velocity;
			this.HasChanged = true;
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x000F0FA9 File Offset: 0x000EF1A9
		public void DestroyTrash()
		{
			NetworkSingleton<TrashManager>.Instance.DestroyTrash(this);
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x000F0FB8 File Offset: 0x000EF1B8
		public virtual void Deinitialize()
		{
			TimeManager.onSleepEnd = (Action<int>)Delegate.Remove(TimeManager.onSleepEnd, new Action<int>(this.SleepEnd));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x000F100C File Offset: 0x000EF20C
		private void OnDestroy()
		{
			TimeManager.onSleepEnd = (Action<int>)Delegate.Remove(TimeManager.onSleepEnd, new Action<int>(this.SleepEnd));
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x000F1066 File Offset: 0x000EF266
		private void RecheckPosition()
		{
			if (Vector3.Distance(this.lastPosition, base.transform.position) > 1f)
			{
				this.lastPosition = base.transform.position;
				this.HasChanged = true;
				this.RecheckProperty();
			}
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x000F10A4 File Offset: 0x000EF2A4
		public virtual TrashItemData GetData()
		{
			return new TrashItemData(this.ID, this.GUID.ToString(), base.transform.position, base.transform.rotation);
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x000F10E6 File Offset: 0x000EF2E6
		public virtual string GetSaveString()
		{
			return this.GetData().GetJson(true);
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x000022C9 File Offset: 0x000004C9
		public virtual bool ShouldSave()
		{
			return true;
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x000F10F4 File Offset: 0x000EF2F4
		private void RecheckProperty()
		{
			if (this.CurrentProperty != null && this.CurrentProperty.DoBoundsContainPoint(base.transform.position))
			{
				return;
			}
			this.CurrentProperty = null;
			for (int i = 0; i < Property.OwnedProperties.Count; i++)
			{
				if (Vector3.Distance(base.transform.position, Property.OwnedProperties[i].BoundingBox.transform.position) <= 25f && Property.OwnedProperties[i].DoBoundsContainPoint(base.transform.position))
				{
					this.CurrentProperty = Property.OwnedProperties[i];
					return;
				}
			}
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x000F11A4 File Offset: 0x000EF3A4
		public void SetContinuousCollisionDetection()
		{
			this.Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
			this.SetPhysicsActive(true);
			base.CancelInvoke("SetDiscreteCollisionDetection");
			base.Invoke("SetDiscreteCollisionDetection", 60f);
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x000F11D4 File Offset: 0x000EF3D4
		public void SetDiscreteCollisionDetection()
		{
			if (this.Rigidbody == null)
			{
				return;
			}
			this.SetPhysicsActive(false);
			this.Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x000F11F8 File Offset: 0x000EF3F8
		public void SetPhysicsActive(bool active)
		{
			this.Rigidbody.isKinematic = !active;
			this.SetCollidersEnabled(active);
			if (active)
			{
				this.timeOnPhysicsEnabled = Time.time;
			}
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x000F1220 File Offset: 0x000EF420
		public void SetCollidersEnabled(bool enabled)
		{
			if (this.collidersEnabled == enabled)
			{
				return;
			}
			this.collidersEnabled = enabled;
			for (int i = 0; i < this.colliders.Length; i++)
			{
				this.colliders[i].enabled = true;
			}
			if (!this.collidersEnabled)
			{
				this.Rigidbody.isKinematic = true;
			}
		}

		// Token: 0x04002941 RID: 10561
		public const float POSITION_CHANGE_THRESHOLD = 1f;

		// Token: 0x04002942 RID: 10562
		public const float LINEAR_DRAG = 0.1f;

		// Token: 0x04002943 RID: 10563
		public const float ANGULAR_DRAG = 0.1f;

		// Token: 0x04002944 RID: 10564
		public const float MIN_Y = -100f;

		// Token: 0x04002945 RID: 10565
		public const int INTERACTION_PRIORITY = 5;

		// Token: 0x04002946 RID: 10566
		public Rigidbody Rigidbody;

		// Token: 0x04002947 RID: 10567
		public Draggable Draggable;

		// Token: 0x04002948 RID: 10568
		[Header("Settings")]
		public string ID = "trashid";

		// Token: 0x04002949 RID: 10569
		[Range(0f, 5f)]
		public int Size = 2;

		// Token: 0x0400294A RID: 10570
		[Range(0f, 10f)]
		public int SellValue = 1;

		// Token: 0x0400294B RID: 10571
		public bool CanGoInContainer = true;

		// Token: 0x0400294C RID: 10572
		public Collider[] colliders;

		// Token: 0x0400294F RID: 10575
		private Vector3 lastPosition = Vector3.zero;

		// Token: 0x04002950 RID: 10576
		public Action<TrashItem> onDestroyed;

		// Token: 0x04002951 RID: 10577
		private bool collidersEnabled = true;

		// Token: 0x04002952 RID: 10578
		private float timeOnPhysicsEnabled;
	}
}
