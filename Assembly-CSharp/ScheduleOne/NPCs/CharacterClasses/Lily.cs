using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004B6 RID: 1206
	public class Lily : NPC
	{
		// Token: 0x06001AD4 RID: 6868 RVA: 0x000713A3 File Offset: 0x0006F5A3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.CharacterClasses.Lily_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x000713B7 File Offset: 0x0006F5B7
		private void Unlocked(NPCRelationData.EUnlockType type, bool b)
		{
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Lily_Unlocked", "true", true);
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x000713CE File Offset: 0x0006F5CE
		protected override void MinPass()
		{
			base.MinPass();
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x000713D6 File Offset: 0x0006F5D6
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LilyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LilyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x000713EF File Offset: 0x0006F5EF
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LilyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LilyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x00071408 File Offset: 0x0006F608
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x00071416 File Offset: 0x0006F616
		protected virtual void dll()
		{
			base.Awake();
			NPCRelationData relationData = this.RelationData;
			relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.Unlocked));
		}

		// Token: 0x040016B0 RID: 5808
		[Header("References")]
		public Transform TutorialScheduleGroup;

		// Token: 0x040016B1 RID: 5809
		public Transform RegularScheduleGroup;

		// Token: 0x040016B2 RID: 5810
		public Conditions TutorialConditions;

		// Token: 0x040016B3 RID: 5811
		private bool dll_Excuted;

		// Token: 0x040016B4 RID: 5812
		private bool dll_Excuted;
	}
}
