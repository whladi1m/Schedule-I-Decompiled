using System;
using FishNet.Connection;
using FishNet.Object;
using ScheduleOne.Property;
using ScheduleOne.UI.Management;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x02000570 RID: 1392
	public interface IConfigurable
	{
		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x060022C8 RID: 8904
		EntityConfiguration Configuration { get; }

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x060022C9 RID: 8905
		ConfigurationReplicator ConfigReplicator { get; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x060022CA RID: 8906
		EConfigurableType ConfigurableType { get; }

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x060022CB RID: 8907
		// (set) Token: 0x060022CC RID: 8908
		WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x060022CD RID: 8909
		// (set) Token: 0x060022CE RID: 8910
		NetworkObject CurrentPlayerConfigurer { get; set; }

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x060022CF RID: 8911 RVA: 0x0008EFF1 File Offset: 0x0008D1F1
		bool IsBeingConfiguredByOtherPlayer
		{
			get
			{
				return this.CurrentPlayerConfigurer != null && !this.CurrentPlayerConfigurer.IsOwner;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x060022D0 RID: 8912
		Sprite TypeIcon { get; }

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x060022D1 RID: 8913
		Transform Transform { get; }

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x060022D2 RID: 8914
		Transform UIPoint { get; }

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x060022D3 RID: 8915 RVA: 0x0008F011 File Offset: 0x0008D211
		bool IsDestroyed
		{
			get
			{
				return this == null || this.Transform == null;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x060022D4 RID: 8916
		bool CanBeSelected { get; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x060022D5 RID: 8917
		Property ParentProperty { get; }

		// Token: 0x060022D6 RID: 8918
		WorldspaceUIElement CreateWorldspaceUI();

		// Token: 0x060022D7 RID: 8919
		void DestroyWorldspaceUI();

		// Token: 0x060022D8 RID: 8920
		void ShowOutline(Color color);

		// Token: 0x060022D9 RID: 8921
		void HideOutline();

		// Token: 0x060022DA RID: 8922 RVA: 0x0008F024 File Offset: 0x0008D224
		void Selected()
		{
			this.Configuration.Selected();
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x0008F031 File Offset: 0x0008D231
		void Deselected()
		{
			this.Configuration.Deselected();
		}

		// Token: 0x060022DC RID: 8924
		void SetConfigurer(NetworkObject player);

		// Token: 0x060022DD RID: 8925
		void SendConfigurationToClient(NetworkConnection conn);
	}
}
