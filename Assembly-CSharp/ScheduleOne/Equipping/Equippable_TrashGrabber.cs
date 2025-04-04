using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Trash;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Equipping
{
	// Token: 0x020008FF RID: 2303
	public class Equippable_TrashGrabber : Equippable_Viewmodel
	{
		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06003E61 RID: 15969 RVA: 0x00107513 File Offset: 0x00105713
		// (set) Token: 0x06003E62 RID: 15970 RVA: 0x0010751A File Offset: 0x0010571A
		public static Equippable_TrashGrabber Instance { get; private set; }

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06003E63 RID: 15971 RVA: 0x00107522 File Offset: 0x00105722
		public static bool IsEquipped
		{
			get
			{
				return Equippable_TrashGrabber.Instance != null;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06003E64 RID: 15972 RVA: 0x0010752F File Offset: 0x0010572F
		// (set) Token: 0x06003E65 RID: 15973 RVA: 0x00107537 File Offset: 0x00105737
		private float currentDropTime { get; set; }

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06003E66 RID: 15974 RVA: 0x00107540 File Offset: 0x00105740
		// (set) Token: 0x06003E67 RID: 15975 RVA: 0x00107548 File Offset: 0x00105748
		private float timeSinceLastDrop { get; set; } = 100f;

		// Token: 0x06003E68 RID: 15976 RVA: 0x00107554 File Offset: 0x00105754
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			this.trashGrabberInstance = (item as TrashGrabberInstance);
			TrashGrabberInstance trashGrabberInstance = this.trashGrabberInstance;
			trashGrabberInstance.onDataChanged = (Action)Delegate.Combine(trashGrabberInstance.onDataChanged, new Action(this.RefreshVisuals));
			this.defaultBinPosition = new Pose(this.Bin.localPosition, this.Bin.localRotation);
			this.defaultBinScale = this.Bin.localScale;
			Equippable_TrashGrabber.Instance = this;
			Singleton<InputPromptsCanvas>.Instance.LoadModule("trashgrabber");
			this.RefreshVisuals();
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x001075E8 File Offset: 0x001057E8
		public override void Unequip()
		{
			base.Unequip();
			TrashGrabberInstance trashGrabberInstance = this.trashGrabberInstance;
			trashGrabberInstance.onDataChanged = (Action)Delegate.Remove(trashGrabberInstance.onDataChanged, new Action(this.RefreshVisuals));
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			Equippable_TrashGrabber.Instance = null;
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x00107628 File Offset: 0x00105828
		protected override void Update()
		{
			base.Update();
			this.timeSinceLastDrop += Time.deltaTime;
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				this.currentDropTime = Mathf.Clamp(this.currentDropTime + Time.deltaTime, 0f, this.DropTime);
				if (this.trashGrabberInstance.GetTotalSize() > 0)
				{
					if (!this.TrashDropSound.isPlaying)
					{
						this.TrashDropSound.Play();
					}
					this.TrashDropSound.VolumeMultiplier = Mathf.Lerp(this.TrashDropSound.VolumeMultiplier, 1f, Time.deltaTime * 4f);
					if (this.currentDropTime >= this.DropTime - 0.05f && this.timeSinceLastDrop >= 0.15f)
					{
						this.timeSinceLastDrop = 0f;
						this.EjectTrash();
					}
				}
				else
				{
					this.TrashDropSound.VolumeMultiplier = Mathf.Lerp(this.TrashDropSound.VolumeMultiplier, 0f, Time.deltaTime * 4f);
				}
			}
			else
			{
				this.currentDropTime = Mathf.Clamp(this.currentDropTime - Time.deltaTime, 0f, this.DropTime);
				this.TrashDropSound.VolumeMultiplier = Mathf.Lerp(this.TrashDropSound.VolumeMultiplier, 0f, Time.deltaTime * 4f);
			}
			float t = Mathf.SmoothStep(0f, 1f, this.currentDropTime / this.DropTime);
			this.Bin.localPosition = Vector3.Lerp(this.defaultBinPosition.position, this.BinRaisedPosition.localPosition, t);
			this.Bin.localRotation = Quaternion.Lerp(this.defaultBinPosition.rotation, this.BinRaisedPosition.localRotation, t);
			this.Bin.localScale = Vector3.Lerp(this.defaultBinScale, this.BinRaisedPosition.localScale, t);
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x0010780C File Offset: 0x00105A0C
		private void EjectTrash()
		{
			if (this.trashGrabberInstance.GetTotalSize() <= 0)
			{
				return;
			}
			List<string> trashIDs = this.trashGrabberInstance.GetTrashIDs();
			string id = trashIDs[trashIDs.Count - 1];
			this.trashGrabberInstance.RemoveTrash(id, 1);
			NetworkSingleton<TrashManager>.Instance.CreateTrashItem(id, PlayerSingleton<PlayerCamera>.Instance.transform.TransformPoint(this.TrashDropOffset), UnityEngine.Random.rotation, PlayerSingleton<PlayerMovement>.Instance.Controller.velocity + PlayerSingleton<PlayerCamera>.Instance.transform.forward * this.DropForce, "", false);
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x001078A8 File Offset: 0x00105AA8
		private void OnDestroy()
		{
			if (Equippable_TrashGrabber.Instance == this)
			{
				Equippable_TrashGrabber.Instance = null;
			}
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x001078C0 File Offset: 0x00105AC0
		public void PickupTrash(TrashItem item)
		{
			this.GrabAnim.Stop();
			this.GrabAnim.Play();
			this.trashGrabberInstance.AddTrash(item.ID, 1);
			item.DestroyTrash();
			if (this.onPickup != null)
			{
				this.onPickup.Invoke();
			}
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x0010790F File Offset: 0x00105B0F
		public int GetCapacity()
		{
			return 20 - this.trashGrabberInstance.GetTotalSize();
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x00107920 File Offset: 0x00105B20
		private void RefreshVisuals()
		{
			float num = Mathf.Clamp01((float)this.trashGrabberInstance.GetTotalSize() / 20f);
			this.TrashContent.localPosition = Vector3.Lerp(this.TrashContent_Min.localPosition, this.TrashContent_Max.localPosition, num);
			this.TrashContent.localScale = Vector3.Lerp(this.TrashContent_Min.localScale, this.TrashContent_Max.localScale, num);
			this.TrashContent.gameObject.SetActive(num > 0f);
		}

		// Token: 0x04002CE5 RID: 11493
		public const float TrashDropSpacing = 0.15f;

		// Token: 0x04002CE6 RID: 11494
		[Header("References")]
		public Transform TrashContent;

		// Token: 0x04002CE7 RID: 11495
		public Transform TrashContent_Min;

		// Token: 0x04002CE8 RID: 11496
		public Transform TrashContent_Max;

		// Token: 0x04002CE9 RID: 11497
		public Animation GrabAnim;

		// Token: 0x04002CEA RID: 11498
		public Transform Bin;

		// Token: 0x04002CEB RID: 11499
		public Transform BinRaisedPosition;

		// Token: 0x04002CEC RID: 11500
		public AudioSourceController TrashDropSound;

		// Token: 0x04002CED RID: 11501
		[Header("Settings")]
		public float DropTime = 0.4f;

		// Token: 0x04002CEE RID: 11502
		public float DropForce = 1f;

		// Token: 0x04002CEF RID: 11503
		public Vector3 TrashDropOffset;

		// Token: 0x04002CF0 RID: 11504
		public UnityEvent onPickup;

		// Token: 0x04002CF3 RID: 11507
		private TrashGrabberInstance trashGrabberInstance;

		// Token: 0x04002CF4 RID: 11508
		private Pose defaultBinPosition;

		// Token: 0x04002CF5 RID: 11509
		private Vector3 defaultBinScale;
	}
}
