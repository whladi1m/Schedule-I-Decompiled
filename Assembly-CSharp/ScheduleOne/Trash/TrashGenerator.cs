using System;
using System.Collections.Generic;
using EasyButtons;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x0200081A RID: 2074
	[RequireComponent(typeof(BoxCollider))]
	public class TrashGenerator : MonoBehaviour, IGUIDRegisterable, ISaveable
	{
		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060038D3 RID: 14547 RVA: 0x000F042C File Offset: 0x000EE62C
		public string SaveFolderName
		{
			get
			{
				return "Generator_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060038D4 RID: 14548 RVA: 0x000F0460 File Offset: 0x000EE660
		public string SaveFileName
		{
			get
			{
				return "Generator_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060038D5 RID: 14549 RVA: 0x0004691A File Offset: 0x00044B1A
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060038D6 RID: 14550 RVA: 0x00014002 File Offset: 0x00012202
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060038D7 RID: 14551 RVA: 0x000F0492 File Offset: 0x000EE692
		// (set) Token: 0x060038D8 RID: 14552 RVA: 0x000F049A File Offset: 0x000EE69A
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060038D9 RID: 14553 RVA: 0x000F04A3 File Offset: 0x000EE6A3
		// (set) Token: 0x060038DA RID: 14554 RVA: 0x000F04AB File Offset: 0x000EE6AB
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060038DB RID: 14555 RVA: 0x000F04B4 File Offset: 0x000EE6B4
		// (set) Token: 0x060038DC RID: 14556 RVA: 0x000F04BC File Offset: 0x000EE6BC
		public bool HasChanged { get; set; }

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060038DD RID: 14557 RVA: 0x000F04C5 File Offset: 0x000EE6C5
		// (set) Token: 0x060038DE RID: 14558 RVA: 0x000F04CD File Offset: 0x000EE6CD
		public Guid GUID { get; protected set; }

		// Token: 0x060038DF RID: 14559 RVA: 0x000F04D6 File Offset: 0x000EE6D6
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x000F04E5 File Offset: 0x000EE6E5
		private void Awake()
		{
			TrashGenerator.AllGenerators.Add(this);
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x000F04F4 File Offset: 0x000EE6F4
		private void Start()
		{
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.SleepStart));
			this.boxCollider = base.GetComponent<BoxCollider>();
			this.boxCollider.isTrigger = true;
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Invisible"));
			this.GUID = new Guid(this.StaticGUID);
			GUIDManager.RegisterObject(this);
			this.InitializeSaveable();
		}

		// Token: 0x060038E2 RID: 14562 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x060038E3 RID: 14563 RVA: 0x000F0566 File Offset: 0x000EE766
		private void OnValidate()
		{
			if (string.IsNullOrEmpty(this.StaticGUID))
			{
				this.RegenerateGUID();
			}
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x000F057B File Offset: 0x000EE77B
		private void OnDestroy()
		{
			TrashGenerator.AllGenerators.Remove(this);
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x000F058C File Offset: 0x000EE78C
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			this.boxCollider = base.GetComponent<BoxCollider>();
			Gizmos.DrawWireCube(this.boxCollider.bounds.center, new Vector3(this.boxCollider.size.x * base.transform.localScale.x, this.boxCollider.size.y * base.transform.localScale.y, this.boxCollider.size.z * base.transform.localScale.z));
		}

		// Token: 0x060038E6 RID: 14566 RVA: 0x000F0630 File Offset: 0x000EE830
		public void AddGeneratedTrash(TrashItem item)
		{
			if (this.generatedTrash.Contains(item))
			{
				return;
			}
			item.onDestroyed = (Action<TrashItem>)Delegate.Combine(item.onDestroyed, new Action<TrashItem>(this.RemoveGeneratedTrash));
			this.generatedTrash.Add(item);
			this.HasChanged = true;
		}

		// Token: 0x060038E7 RID: 14567 RVA: 0x000F0681 File Offset: 0x000EE881
		public void RemoveGeneratedTrash(TrashItem item)
		{
			item.onDestroyed = (Action<TrashItem>)Delegate.Remove(item.onDestroyed, new Action<TrashItem>(this.RemoveGeneratedTrash));
			this.generatedTrash.Remove(item);
			this.HasChanged = true;
		}

		// Token: 0x060038E8 RID: 14568 RVA: 0x000F06BC File Offset: 0x000EE8BC
		[Button]
		private void RegenerateGUID()
		{
			this.StaticGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x000F06E4 File Offset: 0x000EE8E4
		[Button]
		private void AutoCalculateTrashCount()
		{
			this.boxCollider = base.GetComponent<BoxCollider>();
			float num = this.boxCollider.size.x * base.transform.localScale.x * (this.boxCollider.size.z * base.transform.localScale.z);
			this.MaxTrashCount = Mathf.FloorToInt(num * 0.015f);
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x000F0753 File Offset: 0x000EE953
		[Button]
		private void GenerateMaxTrash()
		{
			this.GenerateTrash(this.MaxTrashCount - this.generatedTrash.Count);
		}

		// Token: 0x060038EB RID: 14571 RVA: 0x000F0770 File Offset: 0x000EE970
		private void SleepStart()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			int num = Mathf.Min(this.MaxTrashCount - this.generatedTrash.Count, Mathf.FloorToInt((float)this.MaxTrashCount * 0.2f));
			if (num <= 0)
			{
				return;
			}
			this.GenerateTrash(num);
		}

		// Token: 0x060038EC RID: 14572 RVA: 0x000F07BC File Offset: 0x000EE9BC
		private void GenerateTrash(int count)
		{
			Console.Log("Generating " + count.ToString() + " trash items", null);
			for (int i = 0; i < count; i++)
			{
				Vector3 vector = new Vector3(UnityEngine.Random.Range(this.boxCollider.bounds.min.x, this.boxCollider.bounds.max.x), UnityEngine.Random.Range(this.boxCollider.bounds.min.y, this.boxCollider.bounds.max.y), UnityEngine.Random.Range(this.boxCollider.bounds.min.z, this.boxCollider.bounds.max.z));
				RaycastHit raycastHit;
				vector = (Physics.Raycast(vector, Vector3.down, out raycastHit, 20f, this.GroundCheckMask) ? raycastHit.point : vector);
				int num = 0;
				NavMeshHit navMeshHit;
				while (!NavMeshUtility.SamplePosition(vector, out navMeshHit, 1.5f, -1, true))
				{
					if (num > 10)
					{
						Console.Log("Failed to find a valid position for trash item", null);
						break;
					}
					vector = new Vector3(UnityEngine.Random.Range(this.boxCollider.bounds.min.x, this.boxCollider.bounds.max.x), UnityEngine.Random.Range(this.boxCollider.bounds.min.y, this.boxCollider.bounds.max.y), UnityEngine.Random.Range(this.boxCollider.bounds.min.z, this.boxCollider.bounds.max.z));
					vector = (Physics.Raycast(vector, Vector3.down, out raycastHit, 20f, this.GroundCheckMask) ? raycastHit.point : vector);
					num++;
				}
				vector += Vector3.up * 0.5f;
				TrashItem randomGeneratableTrashPrefab = NetworkSingleton<TrashManager>.Instance.GetRandomGeneratableTrashPrefab();
				TrashItem trashItem = NetworkSingleton<TrashManager>.Instance.CreateTrashItem(randomGeneratableTrashPrefab.ID, vector, UnityEngine.Random.rotation, default(Vector3), "", false);
				trashItem.SetContinuousCollisionDetection();
				this.AddGeneratedTrash(trashItem);
			}
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x000F0A31 File Offset: 0x000EEC31
		public bool ShouldSave()
		{
			return this.generatedTrash.Count > 0;
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x000F0A44 File Offset: 0x000EEC44
		public virtual string GetSaveString()
		{
			return new TrashGeneratorData(this.GUID.ToString(), this.generatedTrash.ConvertAll<string>((TrashItem x) => x.GUID.ToString()).ToArray()).GetJson(true);
		}

		// Token: 0x04002933 RID: 10547
		public const float TRASH_GENERATION_FRACTION = 0.2f;

		// Token: 0x04002934 RID: 10548
		public const float DEFAULT_TRASH_PER_M2 = 0.015f;

		// Token: 0x04002935 RID: 10549
		public static List<TrashGenerator> AllGenerators = new List<TrashGenerator>();

		// Token: 0x04002936 RID: 10550
		[Range(1f, 200f)]
		[SerializeField]
		private int MaxTrashCount = 10;

		// Token: 0x04002937 RID: 10551
		[SerializeField]
		private List<TrashItem> generatedTrash = new List<TrashItem>();

		// Token: 0x04002938 RID: 10552
		[Header("Settings")]
		public LayerMask GroundCheckMask;

		// Token: 0x04002939 RID: 10553
		private BoxCollider boxCollider;

		// Token: 0x0400293E RID: 10558
		public string StaticGUID = string.Empty;
	}
}
