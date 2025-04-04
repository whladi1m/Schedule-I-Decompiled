using System;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000514 RID: 1300
	public class HeavyFlinchBehaviour : Behaviour
	{
		// Token: 0x06001F4D RID: 8013 RVA: 0x000802D8 File Offset: 0x0007E4D8
		public override void BehaviourUpdate()
		{
			base.BehaviourUpdate();
			if (this.remainingFlinchTime > 0f)
			{
				this.remainingFlinchTime = Mathf.Clamp(this.remainingFlinchTime -= Time.deltaTime, 0f, 1.25f);
			}
			if (this.remainingFlinchTime <= 0f)
			{
				base.Disable_Networked(null);
			}
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x00076D70 File Offset: 0x00074F70
		public override void Disable()
		{
			base.Disable();
			this.End();
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x00080336 File Offset: 0x0007E536
		public void Flinch()
		{
			this.remainingFlinchTime += 1.25f;
			if (!base.Enabled)
			{
				base.Enable_Networked(null);
			}
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x00080359 File Offset: 0x0007E559
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.HeavyFlinchBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.HeavyFlinchBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x00080372 File Offset: 0x0007E572
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.HeavyFlinchBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.HeavyFlinchBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x0008038B File Offset: 0x0007E58B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x00080399 File Offset: 0x0007E599
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400186B RID: 6251
		public const float FLINCH_DURATION = 1.25f;

		// Token: 0x0400186C RID: 6252
		private float remainingFlinchTime;

		// Token: 0x0400186D RID: 6253
		private bool dll_Excuted;

		// Token: 0x0400186E RID: 6254
		private bool dll_Excuted;
	}
}
