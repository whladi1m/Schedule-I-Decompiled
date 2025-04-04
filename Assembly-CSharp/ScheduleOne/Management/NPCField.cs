using System;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x02000568 RID: 1384
	public class NPCField : ConfigField
	{
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x0600228C RID: 8844 RVA: 0x0008E4DA File Offset: 0x0008C6DA
		// (set) Token: 0x0600228D RID: 8845 RVA: 0x0008E4E2 File Offset: 0x0008C6E2
		public NPC SelectedNPC { get; protected set; }

		// Token: 0x0600228E RID: 8846 RVA: 0x0008E4EB File Offset: 0x0008C6EB
		public NPCField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x0008E4FF File Offset: 0x0008C6FF
		public void SetNPC(NPC npc, bool network)
		{
			if (this.SelectedNPC == npc)
			{
				return;
			}
			this.SelectedNPC = npc;
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onNPCChanged != null)
			{
				this.onNPCChanged.Invoke(npc);
			}
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x0008E53B File Offset: 0x0008C73B
		public bool DoesNPCMatchRequirement(NPC npc)
		{
			return this.TypeRequirement == null || npc.GetType() == this.TypeRequirement;
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x0008E55E File Offset: 0x0008C75E
		public override bool IsValueDefault()
		{
			return this.SelectedNPC == null;
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x0008E56C File Offset: 0x0008C76C
		public NPCFieldData GetData()
		{
			return new NPCFieldData((this.SelectedNPC != null) ? this.SelectedNPC.GUID.ToString() : "");
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x0008E5AC File Offset: 0x0008C7AC
		public void Load(NPCFieldData data)
		{
			if (data != null && !string.IsNullOrEmpty(data.NPCGuid))
			{
				NPC @object = GUIDManager.GetObject<NPC>(new Guid(data.NPCGuid));
				if (@object != null)
				{
					this.SetNPC(@object, true);
				}
			}
		}

		// Token: 0x04001A17 RID: 6679
		public Type TypeRequirement;

		// Token: 0x04001A18 RID: 6680
		public UnityEvent<NPC> onNPCChanged = new UnityEvent<NPC>();
	}
}
