using System;
using System.Collections.Generic;
using ScheduleOne.Economy;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.NPCs.Relation
{
	// Token: 0x02000482 RID: 1154
	[Serializable]
	public class NPCRelationData
	{
		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060019B0 RID: 6576 RVA: 0x0006F895 File Offset: 0x0006DA95
		// (set) Token: 0x060019B1 RID: 6577 RVA: 0x0006F89D File Offset: 0x0006DA9D
		public float RelationDelta { get; protected set; } = 2f;

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060019B2 RID: 6578 RVA: 0x0006F8A6 File Offset: 0x0006DAA6
		public float NormalizedRelationDelta
		{
			get
			{
				return this.RelationDelta / 5f;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x0006F8B4 File Offset: 0x0006DAB4
		// (set) Token: 0x060019B4 RID: 6580 RVA: 0x0006F8BC File Offset: 0x0006DABC
		public bool Unlocked { get; protected set; }

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060019B5 RID: 6581 RVA: 0x0006F8C5 File Offset: 0x0006DAC5
		// (set) Token: 0x060019B6 RID: 6582 RVA: 0x0006F8CD File Offset: 0x0006DACD
		public NPCRelationData.EUnlockType UnlockType { get; protected set; }

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060019B7 RID: 6583 RVA: 0x0006F8D6 File Offset: 0x0006DAD6
		// (set) Token: 0x060019B8 RID: 6584 RVA: 0x0006F8DE File Offset: 0x0006DADE
		public NPC NPC { get; protected set; }

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060019B9 RID: 6585 RVA: 0x0006F8E7 File Offset: 0x0006DAE7
		public List<NPC> Connections
		{
			get
			{
				return this.FullGameConnections;
			}
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x0006F8EF File Offset: 0x0006DAEF
		public void SetNPC(NPC npc)
		{
			this.NPC = npc;
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x0006F8F8 File Offset: 0x0006DAF8
		public void Init(NPC npc)
		{
			this.SetNPC(npc);
			for (int i = 0; i < this.Connections.Count; i++)
			{
				if (this.Connections[i] == null)
				{
					this.Connections.RemoveAt(i);
					i--;
				}
				else if (!this.Connections[i].RelationData.Connections.Contains(this.NPC))
				{
					this.Connections[i].RelationData.Connections.Add(this.NPC);
				}
			}
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x0006F98C File Offset: 0x0006DB8C
		public virtual void ChangeRelationship(float deltaChange, bool network = true)
		{
			float relationDelta = this.RelationDelta;
			this.RelationDelta = Mathf.Clamp(this.RelationDelta + deltaChange, 0f, 5f);
			if (this.RelationDelta - relationDelta != 0f && this.onRelationshipChange != null)
			{
				this.onRelationshipChange(this.RelationDelta - relationDelta);
			}
			if (network)
			{
				this.NPC.SendRelationship(this.RelationDelta);
			}
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x0006F9FC File Offset: 0x0006DBFC
		public virtual void SetRelationship(float newDelta)
		{
			float relationDelta = this.RelationDelta;
			this.RelationDelta = Mathf.Clamp(newDelta, 0f, 5f);
			float relationDelta2 = this.RelationDelta;
			if (this.RelationDelta - relationDelta != 0f && this.onRelationshipChange != null)
			{
				this.onRelationshipChange(this.RelationDelta - relationDelta);
			}
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x0006FA5D File Offset: 0x0006DC5D
		public virtual void Unlock(NPCRelationData.EUnlockType type, bool notify = true)
		{
			if (this.Unlocked)
			{
				return;
			}
			this.Unlocked = true;
			this.UnlockType = type;
			if (this.onUnlocked != null)
			{
				this.onUnlocked(type, notify);
			}
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x0006FA8C File Offset: 0x0006DC8C
		public virtual void UnlockConnections()
		{
			for (int i = 0; i < this.Connections.Count; i++)
			{
				if (!this.Connections[i].RelationData.Unlocked)
				{
					this.Connections[i].RelationData.Unlock(NPCRelationData.EUnlockType.Recommendation, true);
				}
			}
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x0006FADF File Offset: 0x0006DCDF
		public RelationshipData GetSaveData()
		{
			return new RelationshipData(this.RelationDelta, this.Unlocked, this.UnlockType);
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x0006FAF8 File Offset: 0x0006DCF8
		public float GetAverageMutualRelationship()
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < this.Connections.Count; i++)
			{
				if (this.Connections[i].RelationData.Unlocked)
				{
					num2++;
					num += this.Connections[i].RelationData.RelationDelta;
				}
			}
			if (num2 == 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x0006FB65 File Offset: 0x0006DD65
		public bool IsKnown()
		{
			return this.Unlocked || this.IsMutuallyKnown();
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0006FB78 File Offset: 0x0006DD78
		public bool IsMutuallyKnown()
		{
			for (int i = 0; i < this.Connections.Count; i++)
			{
				if (!(this.Connections[i] == null) && this.Connections[i].RelationData.Unlocked)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0006FBCC File Offset: 0x0006DDCC
		public List<NPC> GetLockedConnections(bool excludeCustomers = false)
		{
			return this.Connections.FindAll((NPC x) => !x.RelationData.Unlocked && (!excludeCustomers || x.GetComponent<Customer>() == null));
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x0006FC00 File Offset: 0x0006DE00
		public List<NPC> GetLockedDealers(bool excludeRecommended)
		{
			return this.Connections.FindAll((NPC x) => !x.RelationData.Unlocked && x is Dealer && (!excludeRecommended || !(x as Dealer).HasBeenRecommended));
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x0006FC31 File Offset: 0x0006DE31
		public List<NPC> GetLockedSuppliers()
		{
			return this.Connections.FindAll((NPC x) => !x.RelationData.Unlocked && x is Supplier);
		}

		// Token: 0x0400162C RID: 5676
		public const float MinDelta = 0f;

		// Token: 0x0400162D RID: 5677
		public const float MaxDelta = 5f;

		// Token: 0x0400162E RID: 5678
		public const float DEFAULT_RELATION_DELTA = 2f;

		// Token: 0x04001633 RID: 5683
		[SerializeField]
		protected List<NPC> FullGameConnections = new List<NPC>();

		// Token: 0x04001634 RID: 5684
		[SerializeField]
		protected List<NPC> DemoConnections = new List<NPC>();

		// Token: 0x04001635 RID: 5685
		public Action<float> onRelationshipChange;

		// Token: 0x04001636 RID: 5686
		public Action<NPCRelationData.EUnlockType, bool> onUnlocked;

		// Token: 0x02000483 RID: 1155
		public enum EUnlockType
		{
			// Token: 0x04001638 RID: 5688
			Recommendation,
			// Token: 0x04001639 RID: 5689
			DirectApproach
		}
	}
}
