using System;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000520 RID: 1312
	public class RagdollBehaviour : Behaviour
	{
		// Token: 0x06001FD8 RID: 8152 RVA: 0x000831D8 File Offset: 0x000813D8
		private void Start()
		{
			base.InvokeRepeating("InfrequentUpdate", 0f, 0.1f);
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x000831F0 File Offset: 0x000813F0
		private void InfrequentUpdate()
		{
			if (this.Seizure)
			{
				Rigidbody[] ragdollRBs = base.Npc.Avatar.RagdollRBs;
				for (int i = 0; i < ragdollRBs.Length; i++)
				{
					ragdollRBs[i].AddForce(UnityEngine.Random.insideUnitSphere * this.SeizureForce, ForceMode.Acceleration);
				}
			}
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x00083250 File Offset: 0x00081450
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.RagdollBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.RagdollBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x00083269 File Offset: 0x00081469
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.RagdollBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.RagdollBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x00083282 File Offset: 0x00081482
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x00083290 File Offset: 0x00081490
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040018CB RID: 6347
		public bool Seizure;

		// Token: 0x040018CC RID: 6348
		public float SeizureForce = 1f;

		// Token: 0x040018CD RID: 6349
		private bool dll_Excuted;

		// Token: 0x040018CE RID: 6350
		private bool dll_Excuted;
	}
}
