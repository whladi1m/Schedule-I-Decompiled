using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Equipping;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BC3 RID: 3011
	public class Dumpster : GridItem
	{
		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06005482 RID: 21634 RVA: 0x0016417E File Offset: 0x0016237E
		// (set) Token: 0x06005483 RID: 21635 RVA: 0x00164186 File Offset: 0x00162386
		public bool lidOpen { get; protected set; }

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06005484 RID: 21636 RVA: 0x0016418F File Offset: 0x0016238F
		// (set) Token: 0x06005485 RID: 21637 RVA: 0x00164197 File Offset: 0x00162397
		public float currentTrashLevel { get; protected set; }

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06005486 RID: 21638 RVA: 0x001641A0 File Offset: 0x001623A0
		public bool isFull
		{
			get
			{
				return this.currentTrashLevel >= Dumpster.capacity;
			}
		}

		// Token: 0x06005487 RID: 21639 RVA: 0x001641B4 File Offset: 0x001623B4
		protected virtual void Update()
		{
			if (this.lidOpen)
			{
				this.lid_CurrentAngle = Mathf.Clamp(this.lid_CurrentAngle + Time.deltaTime * 90f * 3f, 0f, 90f);
			}
			else
			{
				this.lid_CurrentAngle = Mathf.Clamp(this.lid_CurrentAngle - Time.deltaTime * 90f * 3f, 0f, 90f);
			}
			this.lid.localRotation = Quaternion.Euler(0f, 0f, -this.lid_CurrentAngle);
		}

		// Token: 0x06005488 RID: 21640 RVA: 0x00164246 File Offset: 0x00162446
		public virtual void Lid_Hovered()
		{
			if (this.lidOpen)
			{
				this.lid_IntObj.SetMessage("Close dumpster");
				return;
			}
			this.lid_IntObj.SetMessage("Open dumpster");
		}

		// Token: 0x06005489 RID: 21641 RVA: 0x00164271 File Offset: 0x00162471
		public virtual void Lid_Interacted()
		{
			this.lidOpen = !this.lidOpen;
		}

		// Token: 0x0600548A RID: 21642 RVA: 0x00164282 File Offset: 0x00162482
		protected bool DoesPlayerHaveBinEquipped()
		{
			return PlayerSingleton<PlayerInventory>.Instance.equippedSlot != null && PlayerSingleton<PlayerInventory>.Instance.equippedSlot.Equippable is Equippable_Bin;
		}

		// Token: 0x0600548B RID: 21643 RVA: 0x001642A9 File Offset: 0x001624A9
		public void ChangeTrashLevel(float change)
		{
			this.SetTrashLevel(this.currentTrashLevel + change);
		}

		// Token: 0x0600548C RID: 21644 RVA: 0x001642B9 File Offset: 0x001624B9
		public void SetTrashLevel(float trashLevel)
		{
			this.currentTrashLevel = Mathf.Clamp(trashLevel, 0f, Dumpster.capacity);
			this.UpdateTrashVisuals();
		}

		// Token: 0x0600548D RID: 21645 RVA: 0x001642D8 File Offset: 0x001624D8
		private void UpdateTrashVisuals()
		{
			this.trash.localPosition = new Vector3(this.trash.localPosition.x, this.trash_MinY + this.currentTrashLevel / Dumpster.capacity * (this.trash_MaxY - this.trash_MinY));
			this.trash.gameObject.SetActive(this.currentTrashLevel > 0f);
		}

		// Token: 0x0600548E RID: 21646 RVA: 0x00164343 File Offset: 0x00162543
		public override bool CanBeDestroyed(out string reason)
		{
			if (this.currentTrashLevel > 0f)
			{
				reason = "Dumpster is not empty";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x06005491 RID: 21649 RVA: 0x0016436E File Offset: 0x0016256E
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.DumpsterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.DumpsterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06005492 RID: 21650 RVA: 0x00164387 File Offset: 0x00162587
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.DumpsterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.DumpsterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06005493 RID: 21651 RVA: 0x001643A0 File Offset: 0x001625A0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005494 RID: 21652 RVA: 0x001643AE File Offset: 0x001625AE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003EAE RID: 16046
		public static float capacity = 100f;

		// Token: 0x04003EAF RID: 16047
		[Header("References")]
		[SerializeField]
		protected InteractableObject lid_IntObj;

		// Token: 0x04003EB0 RID: 16048
		[SerializeField]
		protected InteractableObject inner_IntObj;

		// Token: 0x04003EB1 RID: 16049
		[SerializeField]
		protected Transform lid;

		// Token: 0x04003EB2 RID: 16050
		[SerializeField]
		protected Transform trash;

		// Token: 0x04003EB3 RID: 16051
		public Transform standPoint;

		// Token: 0x04003EB4 RID: 16052
		[Header("Settings")]
		[SerializeField]
		protected float trash_MinY;

		// Token: 0x04003EB5 RID: 16053
		[SerializeField]
		protected float trash_MaxY;

		// Token: 0x04003EB8 RID: 16056
		private float lid_CurrentAngle;

		// Token: 0x04003EB9 RID: 16057
		private bool dll_Excuted;

		// Token: 0x04003EBA RID: 16058
		private bool dll_Excuted;
	}
}
