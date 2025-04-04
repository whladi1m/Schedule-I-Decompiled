using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x02000822 RID: 2082
	public class TrashSpawnVolume : MonoBehaviour
	{
		// Token: 0x06003967 RID: 14695 RVA: 0x000F2DCB File Offset: 0x000F0FCB
		public void Awake()
		{
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.SleepStart));
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x000F2DE8 File Offset: 0x000F0FE8
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.SleepStart));
			}
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x000F2E0C File Offset: 0x000F100C
		public void SleepStart()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (UnityEngine.Random.value > this.TrashSpawnChance)
			{
				return;
			}
			Collider[] array = Physics.OverlapBox(this.DetectionVolume.transform.TransformPoint(this.DetectionVolume.center), Vector3.Scale(this.DetectionVolume.size, this.DetectionVolume.transform.lossyScale) * 0.5f, this.DetectionVolume.transform.rotation, 1 << LayerMask.NameToLayer("Trash"), QueryTriggerInteraction.Collide);
			int num = 0;
			foreach (Collider collider in array)
			{
				if (num >= this.TrashLimit)
				{
					break;
				}
				if (collider.GetComponentInParent<TrashItem>() != null)
				{
					num++;
				}
			}
			num = Mathf.Max(UnityEngine.Random.Range(0, this.TrashLimit - num), 0);
			for (int j = num; j < this.TrashLimit; j++)
			{
				TrashItem randomGeneratableTrashPrefab = NetworkSingleton<TrashManager>.Instance.GetRandomGeneratableTrashPrefab();
				Vector3 posiiton = new Vector3(UnityEngine.Random.Range(this.CreatonVolume.bounds.min.x, this.CreatonVolume.bounds.max.x), UnityEngine.Random.Range(this.CreatonVolume.bounds.min.y, this.CreatonVolume.bounds.max.y), UnityEngine.Random.Range(this.CreatonVolume.bounds.min.z, this.CreatonVolume.bounds.max.z));
				NetworkSingleton<TrashManager>.Instance.CreateTrashItem(randomGeneratableTrashPrefab.ID, posiiton, UnityEngine.Random.rotation, default(Vector3), "", false).SetContinuousCollisionDetection();
			}
		}

		// Token: 0x0400296A RID: 10602
		public BoxCollider CreatonVolume;

		// Token: 0x0400296B RID: 10603
		public BoxCollider DetectionVolume;

		// Token: 0x0400296C RID: 10604
		public int TrashLimit = 10;

		// Token: 0x0400296D RID: 10605
		public float TrashSpawnChance = 1f;
	}
}
