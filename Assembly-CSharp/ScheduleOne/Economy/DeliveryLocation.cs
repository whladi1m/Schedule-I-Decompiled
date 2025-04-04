using System;
using System.Collections.Generic;
using ScheduleOne.Map;
using ScheduleOne.Quests;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x02000664 RID: 1636
	public class DeliveryLocation : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002D23 RID: 11555 RVA: 0x000BCEF5 File Offset: 0x000BB0F5
		// (set) Token: 0x06002D24 RID: 11556 RVA: 0x000BCEFD File Offset: 0x000BB0FD
		public Guid GUID { get; protected set; }

		// Token: 0x06002D25 RID: 11557 RVA: 0x000BCF06 File Offset: 0x000BB106
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x000BCF18 File Offset: 0x000BB118
		private void Awake()
		{
			this.PoI.gameObject.SetActive(false);
			if (!GUIDManager.IsGUIDValid(this.StaticGUID) || GUIDManager.IsGUIDAlreadyRegistered(new Guid(this.StaticGUID)))
			{
				Console.LogError("Delivery location Static GUID is not valid.", null);
				return;
			}
			((IGUIDRegisterable)this).SetGUID(this.StaticGUID);
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x000BCF6D File Offset: 0x000BB16D
		private void OnValidate()
		{
			base.gameObject.name = this.LocationName;
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x000BCF80 File Offset: 0x000BB180
		public virtual string GetDescription()
		{
			return this.LocationDescription;
		}

		// Token: 0x0400202B RID: 8235
		public string LocationName = string.Empty;

		// Token: 0x0400202C RID: 8236
		public string LocationDescription = string.Empty;

		// Token: 0x0400202D RID: 8237
		public Transform CustomerStandPoint;

		// Token: 0x0400202E RID: 8238
		public Transform TeleportPoint;

		// Token: 0x0400202F RID: 8239
		public POI PoI;

		// Token: 0x04002030 RID: 8240
		public string StaticGUID = string.Empty;

		// Token: 0x04002031 RID: 8241
		public List<Contract> ScheduledContracts = new List<Contract>();
	}
}
