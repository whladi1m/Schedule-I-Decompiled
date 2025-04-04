using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000904 RID: 2308
	public class Equippable_AvatarViewmodel : Equippable_Viewmodel
	{
		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06003E82 RID: 16002 RVA: 0x00107D03 File Offset: 0x00105F03
		protected bool equipAnimDone
		{
			get
			{
				return this.timeEquipped >= this.EquipTime;
			}
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x00107D18 File Offset: 0x00105F18
		public override void Equip(ItemInstance item)
		{
			base.transform.SetParent(Singleton<ViewmodelAvatar>.Instance.RightHandContainer);
			if (this.AnimatorController != null)
			{
				Singleton<ViewmodelAvatar>.Instance.SetAnimatorController(this.AnimatorController);
				Singleton<ViewmodelAvatar>.Instance.SetVisibility(true);
				Singleton<ViewmodelAvatar>.Instance.SetOffset(this.ViewmodelAvatarOffset);
			}
			base.Equip(item);
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x00107D7A File Offset: 0x00105F7A
		public override void Unequip()
		{
			base.Unequip();
			Singleton<ViewmodelAvatar>.Instance.SetVisibility(false);
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x00107D8D File Offset: 0x00105F8D
		protected override void PlayEquipAnimation()
		{
			base.PlayEquipAnimation();
			if (this.EquipTrigger != string.Empty)
			{
				Singleton<ViewmodelAvatar>.Instance.Animator.SetTrigger(this.EquipTrigger);
			}
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x00107DBC File Offset: 0x00105FBC
		protected override void Update()
		{
			base.Update();
			this.timeEquipped += Time.deltaTime;
		}

		// Token: 0x04002CFF RID: 11519
		public RuntimeAnimatorController AnimatorController;

		// Token: 0x04002D00 RID: 11520
		public Vector3 ViewmodelAvatarOffset = Vector3.zero;

		// Token: 0x04002D01 RID: 11521
		[Header("Equipping")]
		public float EquipTime = 0.4f;

		// Token: 0x04002D02 RID: 11522
		public string EquipTrigger = "Equip";

		// Token: 0x04002D03 RID: 11523
		protected float timeEquipped;
	}
}
