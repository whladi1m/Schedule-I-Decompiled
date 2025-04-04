using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Map;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000450 RID: 1104
	public class NPCManager : NetworkSingleton<NPCManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001737 RID: 5943 RVA: 0x00066ADE File Offset: 0x00064CDE
		public string SaveFolderName
		{
			get
			{
				return "NPCs";
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x00066ADE File Offset: 0x00064CDE
		public string SaveFileName
		{
			get
			{
				return "NPCs";
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x00066AE5 File Offset: 0x00064CE5
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600173A RID: 5946 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x00066AED File Offset: 0x00064CED
		// (set) Token: 0x0600173C RID: 5948 RVA: 0x00066AF5 File Offset: 0x00064CF5
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x0600173D RID: 5949 RVA: 0x00066AFE File Offset: 0x00064CFE
		// (set) Token: 0x0600173E RID: 5950 RVA: 0x00066B06 File Offset: 0x00064D06
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x00066B0F File Offset: 0x00064D0F
		// (set) Token: 0x06001740 RID: 5952 RVA: 0x00066B17 File Offset: 0x00064D17
		public bool HasChanged { get; set; }

		// Token: 0x06001741 RID: 5953 RVA: 0x00066B20 File Offset: 0x00064D20
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.NPCManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x00066B34 File Offset: 0x00064D34
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(delegate()
			{
				NPCManager.NPCRegistry.Clear();
			});
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x000045B1 File Offset: 0x000027B1
		public void Update()
		{
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00066B6C File Offset: 0x00064D6C
		public static NPC GetNPC(string id)
		{
			foreach (NPC npc in NPCManager.NPCRegistry)
			{
				if (npc.ID.ToLower() == id.ToLower())
				{
					return npc;
				}
			}
			return null;
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00066BD8 File Offset: 0x00064DD8
		public static List<NPC> GetNPCsInRegion(EMapRegion region)
		{
			List<NPC> list = new List<NPC>();
			foreach (NPC npc in NPCManager.NPCRegistry)
			{
				if (!(npc == null) && npc.Region == region)
				{
					list.Add(npc);
				}
			}
			return list;
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x0003CD29 File Offset: 0x0003AF29
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x00066C44 File Offset: 0x00064E44
		public List<Transform> GetOrderedDistanceWarpPoints(Vector3 origin)
		{
			return (from x in new List<Transform>(this.NPCWarpPoints)
			orderby Vector3.SqrMagnitude(x.position - origin)
			select x).ToList<Transform>();
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x00066C80 File Offset: 0x00064E80
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			for (int i = 0; i < NPCManager.NPCRegistry.Count; i++)
			{
				if (NPCManager.NPCRegistry[i].ShouldSave())
				{
					new SaveRequest(NPCManager.NPCRegistry[i], containerFolder);
					list.Add(NPCManager.NPCRegistry[i].SaveFolderName);
				}
			}
			return list;
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x00066D20 File Offset: 0x00064F20
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x00066D39 File Offset: 0x00064F39
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x00066D52 File Offset: 0x00064F52
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x00066D60 File Offset: 0x00064F60
		protected virtual void dll()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x04001500 RID: 5376
		public static List<NPC> NPCRegistry = new List<NPC>();

		// Token: 0x04001501 RID: 5377
		public Transform[] NPCWarpPoints;

		// Token: 0x04001502 RID: 5378
		public Transform NPCContainer;

		// Token: 0x04001503 RID: 5379
		[Header("Employee Prefabs")]
		public GameObject BotanistPrefab;

		// Token: 0x04001504 RID: 5380
		public GameObject PackagerPrefab;

		// Token: 0x04001505 RID: 5381
		[Header("Prefabs")]
		public NPCPoI NPCPoIPrefab;

		// Token: 0x04001506 RID: 5382
		public NPCPoI PotentialCustomerPoIPrefab;

		// Token: 0x04001507 RID: 5383
		public NPCPoI PotentialDealerPoIPrefab;

		// Token: 0x04001508 RID: 5384
		private NPCsLoader loader = new NPCsLoader();

		// Token: 0x0400150C RID: 5388
		private bool dll_Excuted;

		// Token: 0x0400150D RID: 5389
		private bool dll_Excuted;
	}
}
