using System;
using EasyButtons;
using ScheduleOne.Economy;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Relations
{
	// Token: 0x02000A7F RID: 2687
	public class RelationCircle : MonoBehaviour
	{
		// Token: 0x0600484B RID: 18507 RVA: 0x0012EB04 File Offset: 0x0012CD04
		private void Awake()
		{
			this.LoadNPCData();
			if (this.AssignedNPC != null)
			{
				this.AssignNPC(this.AssignedNPC);
			}
			else if (this.AssignedNPC_ID != string.Empty)
			{
				Console.LogWarning("Failed to find NPC with ID '" + this.AssignedNPC_ID + "'", null);
			}
			this.Button.onClick.AddListener(new UnityAction(this.ButtonClicked));
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener(delegate(BaseEventData <p0>)
			{
				this.HoverStart();
			});
			this.Trigger.triggers.Add(entry);
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.PointerExit;
			entry2.callback.AddListener(delegate(BaseEventData <p0>)
			{
				this.HoverEnd();
			});
			this.Trigger.triggers.Add(entry2);
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x0012EBE8 File Offset: 0x0012CDE8
		private void OnValidate()
		{
			if (this.AssignedNPC != null)
			{
				this.AssignedNPC_ID = this.AssignedNPC.ID;
				this.HeadshotImg.sprite = this.AssignedNPC.MugshotSprite;
			}
			if (this.AutoSetName && this.AssignedNPC != null)
			{
				base.gameObject.name = this.AssignedNPC_ID;
			}
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x0012EC54 File Offset: 0x0012CE54
		public void AssignNPC(NPC npc)
		{
			if (npc != null)
			{
				this.UnassignNPC();
			}
			this.AssignedNPC = npc;
			NPCRelationData relationData = this.AssignedNPC.RelationData;
			relationData.onRelationshipChange = (Action<float>)Delegate.Combine(relationData.onRelationshipChange, new Action<float>(this.RelationshipChange));
			NPCRelationData relationData2 = this.AssignedNPC.RelationData;
			relationData2.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData2.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.SetUnlocked));
			foreach (NPC npc2 in this.AssignedNPC.RelationData.Connections)
			{
				NPCRelationData relationData3 = npc2.RelationData;
				relationData3.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData3.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(delegate(NPCRelationData.EUnlockType <p0>, bool <p1>)
				{
					this.UpdateBlackout();
				}));
			}
			if (npc.RelationData.Unlocked)
			{
				this.SetUnlocked(npc.RelationData.UnlockType, false);
			}
			else
			{
				this.SetLocked();
			}
			if (npc is Dealer)
			{
				(npc as Dealer).onRecommended.AddListener(new UnityAction(this.UpdateBlackout));
			}
			this.HeadshotImg.sprite = this.AssignedNPC.MugshotSprite;
			this.RefreshNotchPosition();
			this.RefreshDependenceDisplay();
			this.UpdateBlackout();
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x0012EDB0 File Offset: 0x0012CFB0
		private void UnassignNPC()
		{
			if (this.AssignedNPC != null)
			{
				NPCRelationData relationData = this.AssignedNPC.RelationData;
				relationData.onRelationshipChange = (Action<float>)Delegate.Remove(relationData.onRelationshipChange, new Action<float>(this.RelationshipChange));
				NPCRelationData relationData2 = this.AssignedNPC.RelationData;
				relationData2.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Remove(relationData2.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.SetUnlocked));
			}
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x0012EE23 File Offset: 0x0012D023
		private void RelationshipChange(float change)
		{
			this.RefreshNotchPosition();
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x0012EE2B File Offset: 0x0012D02B
		public void SetNotchPosition(float relationshipDelta)
		{
			this.NotchPivot.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(90f, -90f, relationshipDelta / 5f));
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x0012EE5D File Offset: 0x0012D05D
		private void RefreshNotchPosition()
		{
			this.SetNotchPosition(this.AssignedNPC.RelationData.RelationDelta);
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x0012EE78 File Offset: 0x0012D078
		private void RefreshDependenceDisplay()
		{
			Customer component = this.AssignedNPC.GetComponent<Customer>();
			if (component == null)
			{
				this.PortraitBackground.color = RelationCircle.PortraitColor_ZeroDependence;
				return;
			}
			this.PortraitBackground.color = Color.Lerp(RelationCircle.PortraitColor_ZeroDependence, RelationCircle.PortraitColor_MaxDependence, component.CurrentAddiction);
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x0012EECB File Offset: 0x0012D0CB
		[Button]
		public void SetLocked()
		{
			this.Locked.gameObject.SetActive(true);
			this.NotchPivot.gameObject.SetActive(false);
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x0012EEEF File Offset: 0x0012D0EF
		[Button]
		public void SetUnlocked(NPCRelationData.EUnlockType unlockType, bool notify = true)
		{
			this.Locked.gameObject.SetActive(false);
			this.NotchPivot.gameObject.SetActive(true);
			this.SetBlackedOut(false);
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x0012EF1A File Offset: 0x0012D11A
		[Button]
		public void LoadNPCData()
		{
			this.AssignedNPC = NPCManager.GetNPC(this.AssignedNPC_ID);
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x0012EF30 File Offset: 0x0012D130
		private void UpdateBlackout()
		{
			bool blackedOut = false;
			if (!this.AssignedNPC.RelationData.Unlocked)
			{
				if (this.AssignedNPC is Dealer)
				{
					blackedOut = !(this.AssignedNPC as Dealer).HasBeenRecommended;
				}
				else if (this.AssignedNPC is Supplier)
				{
					blackedOut = true;
				}
				else if (this.AssignedNPC.GetComponent<Customer>() != null)
				{
					blackedOut = (!this.AssignedNPC.RelationData.Unlocked && !this.AssignedNPC.RelationData.IsMutuallyKnown());
				}
			}
			this.SetBlackedOut(blackedOut);
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x0012EFC8 File Offset: 0x0012D1C8
		public void SetBlackedOut(bool blackedOut)
		{
			this.HeadshotImg.color = (blackedOut ? Color.black : Color.white);
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x0012EFE4 File Offset: 0x0012D1E4
		private void ButtonClicked()
		{
			if (this.onClicked != null)
			{
				this.onClicked();
			}
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x0012EFF9 File Offset: 0x0012D1F9
		private void HoverStart()
		{
			if (this.onHoverStart != null)
			{
				this.onHoverStart();
			}
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x0012F00E File Offset: 0x0012D20E
		private void HoverEnd()
		{
			if (this.onHoverEnd != null)
			{
				this.onHoverEnd();
			}
		}

		// Token: 0x040035A7 RID: 13735
		public const float NotchMinRot = 90f;

		// Token: 0x040035A8 RID: 13736
		public const float NotchMaxRot = -90f;

		// Token: 0x040035A9 RID: 13737
		public static Color PortraitColor_ZeroDependence = new Color32(60, 60, 60, byte.MaxValue);

		// Token: 0x040035AA RID: 13738
		public static Color PortraitColor_MaxDependence = new Color32(120, 15, 15, byte.MaxValue);

		// Token: 0x040035AB RID: 13739
		public string AssignedNPC_ID = string.Empty;

		// Token: 0x040035AC RID: 13740
		public NPC AssignedNPC;

		// Token: 0x040035AD RID: 13741
		public Action onClicked;

		// Token: 0x040035AE RID: 13742
		public Action onHoverStart;

		// Token: 0x040035AF RID: 13743
		public Action onHoverEnd;

		// Token: 0x040035B0 RID: 13744
		public bool AutoSetName;

		// Token: 0x040035B1 RID: 13745
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x040035B2 RID: 13746
		public Image PortraitBackground;

		// Token: 0x040035B3 RID: 13747
		public Image HeadshotImg;

		// Token: 0x040035B4 RID: 13748
		public RectTransform NotchPivot;

		// Token: 0x040035B5 RID: 13749
		public RectTransform Locked;

		// Token: 0x040035B6 RID: 13750
		public Button Button;

		// Token: 0x040035B7 RID: 13751
		public EventTrigger Trigger;
	}
}
