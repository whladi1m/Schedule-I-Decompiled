using System;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000910 RID: 2320
	public class Equippable_Viewmodel : Equippable_StorableItem
	{
		// Token: 0x06003EE3 RID: 16099 RVA: 0x00109844 File Offset: 0x00107A44
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			base.transform.localPosition = this.localPosition;
			base.transform.localEulerAngles = this.localEulerAngles;
			base.transform.localScale = this.localScale;
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Viewmodel"));
			foreach (MeshRenderer meshRenderer in base.gameObject.GetComponentsInChildren<MeshRenderer>())
			{
				if (meshRenderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					meshRenderer.enabled = false;
				}
				else
				{
					meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				}
			}
			this.PlayEquipAnimation();
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x001098DD File Offset: 0x00107ADD
		public override void Unequip()
		{
			base.Unequip();
			this.PlayUnequipAnimation();
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x001098EB File Offset: 0x00107AEB
		protected virtual void PlayEquipAnimation()
		{
			if (this.AvatarEquippable != null)
			{
				Player.Local.SendEquippable_Networked(this.AvatarEquippable.AssetPath);
			}
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x00109910 File Offset: 0x00107B10
		protected virtual void PlayUnequipAnimation()
		{
			if (this.AvatarEquippable != null)
			{
				Player.Local.SendEquippable_Networked(string.Empty);
			}
		}

		// Token: 0x04002D62 RID: 11618
		[Header("Viewmodel settings")]
		public Vector3 localPosition;

		// Token: 0x04002D63 RID: 11619
		public Vector3 localEulerAngles;

		// Token: 0x04002D64 RID: 11620
		public Vector3 localScale = Vector3.one;

		// Token: 0x04002D65 RID: 11621
		[Header("Third person animation settings")]
		public AvatarEquippable AvatarEquippable;
	}
}
