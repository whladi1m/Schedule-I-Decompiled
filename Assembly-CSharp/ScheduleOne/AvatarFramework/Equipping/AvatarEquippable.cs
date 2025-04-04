using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Equipping
{
	// Token: 0x02000959 RID: 2393
	public class AvatarEquippable : MonoBehaviour
	{
		// Token: 0x06004134 RID: 16692 RVA: 0x00111A2C File Offset: 0x0010FC2C
		[Button]
		public void RecalculateAssetPath()
		{
			this.AssetPath = AssetPathUtility.GetResourcesPath(base.gameObject);
			string[] array = this.AssetPath.Split('/', StringSplitOptions.None);
			array[array.Length - 1] = base.gameObject.name;
			this.AssetPath = string.Join("/", array);
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x00111A7C File Offset: 0x0010FC7C
		protected virtual void Awake()
		{
			if (this.AssetPath == string.Empty)
			{
				Console.LogWarning(base.gameObject.name + " does not have an assetpath!", null);
			}
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x00111AAC File Offset: 0x0010FCAC
		public virtual void Equip(Avatar _avatar)
		{
			this.avatar = _avatar;
			if (this.Hand == AvatarEquippable.EHand.Right)
			{
				base.transform.SetParent(this.avatar.Anim.RightHandContainer);
			}
			else
			{
				base.transform.SetParent(this.avatar.Anim.LeftHandContainer);
			}
			this.PositionAnimationModel();
			this.InitializeAnimation();
			Player componentInParent = this.avatar.GetComponentInParent<Player>();
			if (componentInParent != null && componentInParent.IsOwner && !componentInParent.avatarVisibleToLocalPlayer)
			{
				LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Invisible"));
			}
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].isTrigger = true;
			}
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x00111B65 File Offset: 0x0010FD65
		public virtual void InitializeAnimation()
		{
			if (this.TriggerType == AvatarEquippable.ETriggerType.Trigger)
			{
				this.SetTrigger(this.AnimationTrigger);
				return;
			}
			this.SetBool(this.AnimationTrigger, true);
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x00111B89 File Offset: 0x0010FD89
		public virtual void Unequip()
		{
			if (this.TriggerType == AvatarEquippable.ETriggerType.Trigger)
			{
				this.SetTrigger("EndAction");
			}
			else
			{
				this.SetBool(this.AnimationTrigger, false);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x00111BB8 File Offset: 0x0010FDB8
		private void PositionAnimationModel()
		{
			Transform transform = (this.Hand == AvatarEquippable.EHand.Right) ? this.avatar.Anim.RightHandAlignmentPoint : this.avatar.Anim.LeftHandAlignmentPoint;
			base.transform.rotation = transform.rotation * (Quaternion.Inverse(this.AlignmentPoint.rotation) * base.transform.rotation);
			base.transform.position = transform.position + (base.transform.position - this.AlignmentPoint.position);
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x00111C58 File Offset: 0x0010FE58
		protected void SetTrigger(string anim)
		{
			if (this.avatar.GetComponentInParent<Player>() != null)
			{
				this.avatar.GetComponentInParent<Player>().SetAnimationTrigger(anim);
				return;
			}
			if (this.avatar.GetComponentInParent<NPC>() != null)
			{
				this.avatar.GetComponentInParent<NPC>().SetAnimationTrigger(anim);
			}
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x00111CB0 File Offset: 0x0010FEB0
		protected void SetBool(string anim, bool val)
		{
			if (this.avatar.GetComponentInParent<Player>() != null)
			{
				this.avatar.GetComponentInParent<Player>().SetAnimationBool(anim, val);
				return;
			}
			if (this.avatar.GetComponentInParent<NPC>() != null)
			{
				this.avatar.GetComponentInParent<NPC>().SetAnimationBool(anim, val);
			}
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x00111D08 File Offset: 0x0010FF08
		protected void ResetTrigger(string anim)
		{
			if (this.avatar.GetComponentInParent<Player>() != null)
			{
				this.avatar.GetComponentInParent<Player>().ResetAnimationTrigger(anim);
				return;
			}
			if (this.avatar.GetComponentInParent<NPC>() != null)
			{
				this.avatar.GetComponentInParent<NPC>().ResetAnimationTrigger(anim);
			}
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ReceiveMessage(string message, object parameter)
		{
		}

		// Token: 0x04002F0C RID: 12044
		[Header("Settings")]
		public Transform AlignmentPoint;

		// Token: 0x04002F0D RID: 12045
		[Range(0f, 1f)]
		public float Suspiciousness;

		// Token: 0x04002F0E RID: 12046
		public AvatarEquippable.EHand Hand = AvatarEquippable.EHand.Right;

		// Token: 0x04002F0F RID: 12047
		public AvatarEquippable.ETriggerType TriggerType;

		// Token: 0x04002F10 RID: 12048
		public string AnimationTrigger = "RightArm_Hold_ClosedHand";

		// Token: 0x04002F11 RID: 12049
		public string AssetPath = string.Empty;

		// Token: 0x04002F12 RID: 12050
		protected Avatar avatar;

		// Token: 0x0200095A RID: 2394
		public enum ETriggerType
		{
			// Token: 0x04002F14 RID: 12052
			Trigger,
			// Token: 0x04002F15 RID: 12053
			Bool
		}

		// Token: 0x0200095B RID: 2395
		public enum EHand
		{
			// Token: 0x04002F17 RID: 12055
			Left,
			// Token: 0x04002F18 RID: 12056
			Right
		}
	}
}
