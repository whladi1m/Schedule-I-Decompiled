using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Doors;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BF5 RID: 3061
	public class NPCEnterableBuilding : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x060055AB RID: 21931 RVA: 0x0016834A File Offset: 0x0016654A
		// (set) Token: 0x060055AC RID: 21932 RVA: 0x00168352 File Offset: 0x00166552
		public Guid GUID { get; protected set; }

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x060055AD RID: 21933 RVA: 0x0016835B File Offset: 0x0016655B
		public int OccupantCount
		{
			get
			{
				return this.Occupants.Count;
			}
		}

		// Token: 0x060055AE RID: 21934 RVA: 0x00168368 File Offset: 0x00166568
		protected virtual void Awake()
		{
			if (!GUIDManager.IsGUIDValid(this.BakedGUID))
			{
				Console.LogError(base.gameObject.name + "'s baked GUID is not valid! Bad.", null);
			}
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
			if (this.Doors.Length == 0)
			{
				this.GetDoors();
				if (this.Doors.Length == 0)
				{
					Console.LogError(this.BuildingName + " has no doors! NPCs won't be able to enter the building.", null);
				}
			}
		}

		// Token: 0x060055AF RID: 21935 RVA: 0x001683E2 File Offset: 0x001665E2
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x060055B0 RID: 21936 RVA: 0x001683F4 File Offset: 0x001665F4
		public virtual void NPCEnteredBuilding(NPC npc)
		{
			if (!this.Occupants.Contains(npc))
			{
				this.Occupants.Add(npc);
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, npc.Movement.FootPosition) > 15f)
			{
				return;
			}
			AudioSourceController audioSourceController = UnityEngine.Object.Instantiate<AudioSourceController>(Singleton<AudioManager>.Instance.DoorOpen, NetworkSingleton<GameManager>.Instance.Temp.transform);
			audioSourceController.transform.position = npc.Avatar.transform.position;
			audioSourceController.Play();
			UnityEngine.Object.Destroy(audioSourceController.gameObject, audioSourceController.AudioSource.clip.length);
		}

		// Token: 0x060055B1 RID: 21937 RVA: 0x001684A8 File Offset: 0x001666A8
		public virtual void NPCExitedBuilding(NPC npc)
		{
			this.Occupants.Remove(npc);
			if (!PlayerSingleton<PlayerCamera>.InstanceExists || Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, npc.Avatar.transform.position) > 15f)
			{
				return;
			}
			if (!Singleton<AudioManager>.InstanceExists)
			{
				return;
			}
			if (!NetworkSingleton<GameManager>.InstanceExists)
			{
				return;
			}
			AudioSourceController audioSourceController = UnityEngine.Object.Instantiate<AudioSourceController>(Singleton<AudioManager>.Instance.DoorClose, NetworkSingleton<GameManager>.Instance.Temp.transform);
			audioSourceController.Play();
			UnityEngine.Object.Destroy(audioSourceController.gameObject, audioSourceController.AudioSource.clip.length);
		}

		// Token: 0x060055B2 RID: 21938 RVA: 0x00168545 File Offset: 0x00166745
		[Button]
		public void GetDoors()
		{
			this.Doors = base.GetComponentsInChildren<StaticDoor>();
		}

		// Token: 0x060055B3 RID: 21939 RVA: 0x00168553 File Offset: 0x00166753
		public List<NPC> GetSummonableNPCs()
		{
			return (from npc in this.Occupants
			where npc.CanBeSummoned
			select npc).ToList<NPC>();
		}

		// Token: 0x060055B4 RID: 21940 RVA: 0x00168584 File Offset: 0x00166784
		public StaticDoor GetClosestDoor(Vector3 pos, bool useableOnly)
		{
			return (from door in this.Doors
			where !useableOnly || door.Usable
			orderby Vector3.Distance(door.transform.position, pos)
			select door).FirstOrDefault<StaticDoor>();
		}

		// Token: 0x04003FA4 RID: 16292
		public const float DOOR_SOUND_DISTANCE_LIMIT = 15f;

		// Token: 0x04003FA6 RID: 16294
		[Header("Settings")]
		public string BuildingName;

		// Token: 0x04003FA7 RID: 16295
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x04003FA8 RID: 16296
		[Header("References")]
		public StaticDoor[] Doors;

		// Token: 0x04003FA9 RID: 16297
		[Header("Readonly")]
		[SerializeField]
		private List<NPC> Occupants = new List<NPC>();
	}
}
