using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Tiles;
using ScheduleOne.Trash;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B71 RID: 2929
	[RequireComponent(typeof(TrashContainer))]
	public class TrashContainerItem : GridItem, ITransitEntity
	{
		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06004E64 RID: 20068 RVA: 0x0014AAAB File Offset: 0x00148CAB
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06004E65 RID: 20069 RVA: 0x0014AAB8 File Offset: 0x00148CB8
		// (set) Token: 0x06004E66 RID: 20070 RVA: 0x0014AAC0 File Offset: 0x00148CC0
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06004E67 RID: 20071 RVA: 0x0014AAC9 File Offset: 0x00148CC9
		// (set) Token: 0x06004E68 RID: 20072 RVA: 0x0014AAD1 File Offset: 0x00148CD1
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06004E69 RID: 20073 RVA: 0x000AD06F File Offset: 0x000AB26F
		public Transform LinkOrigin
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06004E6A RID: 20074 RVA: 0x0014AADA File Offset: 0x00148CDA
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06004E6B RID: 20075 RVA: 0x0014AAE2 File Offset: 0x00148CE2
		public bool Selectable { get; }

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06004E6C RID: 20076 RVA: 0x0014AAEA File Offset: 0x00148CEA
		// (set) Token: 0x06004E6D RID: 20077 RVA: 0x0014AAF2 File Offset: 0x00148CF2
		public bool IsAcceptingItems { get; set; }

		// Token: 0x06004E6E RID: 20078 RVA: 0x0014AAFC File Offset: 0x00148CFC
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.TrashContainerItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004E6F RID: 20079 RVA: 0x0014AB1B File Offset: 0x00148D1B
		protected override void Start()
		{
			base.Start();
			this.Container.onTrashLevelChanged.AddListener(new UnityAction(this.TrashLevelChanged));
			this.Container.onTrashAdded.AddListener(new UnityAction<string>(this.TrashAdded));
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x0014AB5B File Offset: 0x00148D5B
		public override void InitializeGridItem(ItemInstance instance, Grid grid, Vector2 originCoordinate, int rotation, string GUID)
		{
			base.InitializeGridItem(instance, grid, originCoordinate, rotation, GUID);
			if (!this.isGhost)
			{
				base.InvokeRepeating("CheckTrashItems", UnityEngine.Random.Range(0f, 1f), 1f);
			}
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x0014AB94 File Offset: 0x00148D94
		private void TrashLevelChanged()
		{
			base.HasChanged = true;
			if (this.Container.NormalizedTrashLevel > 0.75f)
			{
				if (!this.Flies.isPlaying)
				{
					this.Flies.Play();
					return;
				}
			}
			else if (this.Flies.isPlaying)
			{
				this.Flies.Stop();
			}
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x0014ABEB File Offset: 0x00148DEB
		public override bool CanBeDestroyed(out string reason)
		{
			if (this.Container.TrashLevel > 0)
			{
				reason = "Contains trash";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x0014AC0B File Offset: 0x00148E0B
		public override string GetSaveString()
		{
			return new TrashContainerData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, this.Container.Content.GetData()).GetJson(true);
		}

		// Token: 0x06004E74 RID: 20084 RVA: 0x0014AC48 File Offset: 0x00148E48
		private void TrashAdded(string trashID)
		{
			if (this.TrashAddedSound == null)
			{
				return;
			}
			float volumeMultiplier = Mathf.Clamp01((float)NetworkSingleton<TrashManager>.Instance.GetTrashPrefab(trashID).Size / 4f);
			this.TrashAddedSound.VolumeMultiplier = volumeMultiplier;
			this.TrashAddedSound.Play();
		}

		// Token: 0x06004E75 RID: 20085 RVA: 0x0014AC98 File Offset: 0x00148E98
		public override void ShowOutline(Color color)
		{
			base.ShowOutline(color);
			this.PickupAreaProjector.enabled = true;
		}

		// Token: 0x06004E76 RID: 20086 RVA: 0x0014ACAD File Offset: 0x00148EAD
		public override void HideOutline()
		{
			base.HideOutline();
			this.PickupAreaProjector.enabled = false;
		}

		// Token: 0x06004E77 RID: 20087 RVA: 0x0014ACC4 File Offset: 0x00148EC4
		private void CheckTrashItems()
		{
			for (int i = 0; i < this.TrashItemsInRadius.Count; i++)
			{
				if (!this.IsTrashValid(this.TrashItemsInRadius[i]))
				{
					this.RemoveTrashItemFromRadius(this.TrashItemsInRadius[i]);
					i--;
				}
			}
			Collider[] array = Physics.OverlapSphere(base.transform.position, this.PickupRadius, LayerMask.GetMask(new string[]
			{
				"Trash"
			}), QueryTriggerInteraction.Ignore);
			for (int j = 0; j < array.Length; j++)
			{
				if (this.IsPointInRadius(array[j].transform.position))
				{
					TrashItem componentInParent = array[j].GetComponentInParent<TrashItem>();
					if (componentInParent != null && this.IsTrashValid(componentInParent))
					{
						this.AddTrashToRadius(componentInParent);
					}
				}
			}
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x0014AD84 File Offset: 0x00148F84
		private void AddTrashToRadius(TrashItem trashItem)
		{
			if (trashItem is TrashBag)
			{
				this.AddTrashBagToRadius(trashItem as TrashBag);
				return;
			}
			if (!this.TrashItemsInRadius.Contains(trashItem))
			{
				this.TrashItemsInRadius.Add(trashItem);
				trashItem.onDestroyed = (Action<TrashItem>)Delegate.Combine(trashItem.onDestroyed, new Action<TrashItem>(this.RemoveTrashItemFromRadius));
			}
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x0014ADE2 File Offset: 0x00148FE2
		private void AddTrashBagToRadius(TrashBag trashBag)
		{
			if (!this.TrashBagsInRadius.Contains(trashBag))
			{
				this.TrashBagsInRadius.Add(trashBag);
				trashBag.onDestroyed = (Action<TrashItem>)Delegate.Combine(trashBag.onDestroyed, new Action<TrashItem>(this.RemoveTrashItemFromRadius));
			}
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x0014AE20 File Offset: 0x00149020
		private void RemoveTrashItemFromRadius(TrashItem trashItem)
		{
			if (trashItem is TrashBag)
			{
				this.RemoveTrashBagFromRadius(trashItem as TrashBag);
				return;
			}
			if (this.TrashItemsInRadius.Contains(trashItem))
			{
				this.TrashItemsInRadius.Remove(trashItem);
				trashItem.onDestroyed = (Action<TrashItem>)Delegate.Remove(trashItem.onDestroyed, new Action<TrashItem>(this.RemoveTrashItemFromRadius));
			}
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x0014AE7F File Offset: 0x0014907F
		private void RemoveTrashBagFromRadius(TrashBag trashBag)
		{
			if (this.TrashBagsInRadius.Contains(trashBag))
			{
				this.TrashBagsInRadius.Remove(trashBag);
				trashBag.onDestroyed = (Action<TrashItem>)Delegate.Remove(trashBag.onDestroyed, new Action<TrashItem>(this.RemoveTrashItemFromRadius));
			}
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x0014AEC0 File Offset: 0x001490C0
		private bool IsTrashValid(TrashItem trashItem)
		{
			return !(trashItem == null) && this.IsPointInRadius(trashItem.transform.position) && !trashItem.Draggable.IsBeingDragged && base.ParentProperty.DoBoundsContainPoint(trashItem.transform.position);
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x0014AF18 File Offset: 0x00149118
		public bool IsPointInRadius(Vector3 point)
		{
			float num = Vector3.Distance(point, base.transform.position);
			float num2 = Mathf.Abs(point.y - base.transform.position.y);
			return num <= this.PickupRadius + 0.2f && num2 <= 2f;
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x0014AFC1 File Offset: 0x001491C1
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.TrashContainerItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.TrashContainerItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x0014AFDA File Offset: 0x001491DA
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.TrashContainerItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.TrashContainerItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x0014AFF3 File Offset: 0x001491F3
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x0014B004 File Offset: 0x00149204
		protected virtual void dll()
		{
			base.Awake();
			this.PickupAreaProjector.size = new Vector3(this.PickupRadius * 2f, this.PickupRadius * 2f, 0.2f);
			this.PickupAreaProjector.enabled = false;
		}

		// Token: 0x04003B4C RID: 15180
		public const float MAX_VERTICAL_OFFSET = 2f;

		// Token: 0x04003B4D RID: 15181
		public TrashContainer Container;

		// Token: 0x04003B4E RID: 15182
		public ParticleSystem Flies;

		// Token: 0x04003B4F RID: 15183
		public AudioSourceController TrashAddedSound;

		// Token: 0x04003B50 RID: 15184
		public DecalProjector PickupAreaProjector;

		// Token: 0x04003B51 RID: 15185
		public Transform[] accessPoints;

		// Token: 0x04003B52 RID: 15186
		[Header("Pickup settings")]
		public bool UsableByCleaners = true;

		// Token: 0x04003B53 RID: 15187
		public float PickupRadius = 5f;

		// Token: 0x04003B58 RID: 15192
		public List<TrashItem> TrashItemsInRadius = new List<TrashItem>();

		// Token: 0x04003B59 RID: 15193
		public List<TrashBag> TrashBagsInRadius = new List<TrashBag>();

		// Token: 0x04003B5A RID: 15194
		private bool dll_Excuted;

		// Token: 0x04003B5B RID: 15195
		private bool dll_Excuted;
	}
}
