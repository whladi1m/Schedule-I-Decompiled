using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.Map;
using ScheduleOne.Storage;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Economy
{
	// Token: 0x02000657 RID: 1623
	public class DeadDrop : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002C6B RID: 11371 RVA: 0x000B8EFF File Offset: 0x000B70FF
		// (set) Token: 0x06002C6C RID: 11372 RVA: 0x000B8F07 File Offset: 0x000B7107
		public Guid GUID { get; protected set; }

		// Token: 0x06002C6D RID: 11373 RVA: 0x000B8F10 File Offset: 0x000B7110
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x000B8F36 File Offset: 0x000B7136
		protected virtual void Awake()
		{
			DeadDrop.DeadDrops.Add(this);
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x000B8F5A File Offset: 0x000B715A
		private void OnValidate()
		{
			base.gameObject.name = this.DeadDropName;
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x000B8F70 File Offset: 0x000B7170
		protected virtual void Start()
		{
			base.GetComponent<StorageEntity>().StorageEntitySubtitle = this.DeadDropName;
			this.PoI.SetMainText("Dead Drop\n(" + this.DeadDropName + ")");
			this.UpdateDeadDrop();
			this.Storage.onContentsChanged.AddListener(new UnityAction(this.UpdateDeadDrop));
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x000B8FD0 File Offset: 0x000B71D0
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x000B8FDF File Offset: 0x000B71DF
		public void OnDestroy()
		{
			DeadDrop.DeadDrops.Remove(this);
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x000B8FF0 File Offset: 0x000B71F0
		public static DeadDrop GetRandomEmptyDrop(Vector3 origin)
		{
			List<DeadDrop> list = (from drop in DeadDrop.DeadDrops
			where drop.Storage.ItemCount == 0
			select drop).ToList<DeadDrop>();
			list = (from drop in list
			orderby Vector3.Distance(drop.transform.position, origin)
			select drop).ToList<DeadDrop>();
			list.RemoveAt(0);
			list.RemoveRange(list.Count / 2, list.Count / 2);
			if (list.Count == 0)
			{
				return null;
			}
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x000B908C File Offset: 0x000B728C
		private void UpdateDeadDrop()
		{
			this.PoI.enabled = false;
			this.Light.Enabled = (this.Storage.ItemCount > 0);
			if (this.ItemCountVariable != string.Empty)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.ItemCountVariable, this.Storage.ItemCount.ToString(), true);
			}
		}

		// Token: 0x04001FC8 RID: 8136
		public static List<DeadDrop> DeadDrops = new List<DeadDrop>();

		// Token: 0x04001FC9 RID: 8137
		public string DeadDropName;

		// Token: 0x04001FCA RID: 8138
		public string DeadDropDescription;

		// Token: 0x04001FCB RID: 8139
		public StorageEntity Storage;

		// Token: 0x04001FCC RID: 8140
		public POI PoI;

		// Token: 0x04001FCD RID: 8141
		public OptimizedLight Light;

		// Token: 0x04001FCE RID: 8142
		public string ItemCountVariable = string.Empty;

		// Token: 0x04001FD0 RID: 8144
		[SerializeField]
		protected string BakedGUID = string.Empty;
	}
}
