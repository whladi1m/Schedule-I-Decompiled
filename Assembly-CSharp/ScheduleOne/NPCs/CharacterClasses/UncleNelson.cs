using System;
using System.Collections.Generic;
using ScheduleOne.UI.Phone.Messages;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004D1 RID: 1233
	public class UncleNelson : NPC
	{
		// Token: 0x06001B89 RID: 7049 RVA: 0x000726FC File Offset: 0x000708FC
		public void SendInitialMessage()
		{
			if (base.MSGConversation.messageChainHistory.Count > 0 || base.MSGConversation.messageHistory.Count > 0)
			{
				return;
			}
			base.MSGConversation.SetIsKnown(false);
			base.MSGConversation.SendMessageChain(new MessageChain
			{
				Messages = new List<string>
				{
					this.InitialMessage
				}
			}, 0f, false, true);
			base.MSGConversation.SetRead(false);
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x00072794 File Offset: 0x00070994
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.UncleNelsonAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.UncleNelsonAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x000727AD File Offset: 0x000709AD
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.UncleNelsonAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.UncleNelsonAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x000727C6 File Offset: 0x000709C6
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000727D4 File Offset: 0x000709D4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001705 RID: 5893
		public string InitialMessage_Demo = "I’ve heard you’re in some trouble. Best not to talk over your mobile phone. Go find a payphone.\n- U.N.";

		// Token: 0x04001706 RID: 5894
		public string InitialMessage = "You get out alright? Best not to talk over your mobile phone. Go find a payphone.\n- U.N.";

		// Token: 0x04001707 RID: 5895
		private bool dll_Excuted;

		// Token: 0x04001708 RID: 5896
		private bool dll_Excuted;
	}
}
