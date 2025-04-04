using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x02000821 RID: 2081
	[RequireComponent(typeof(BoxCollider))]
	public class TrashRemovalVolume : MonoBehaviour
	{
		// Token: 0x06003962 RID: 14690 RVA: 0x000F2C85 File Offset: 0x000F0E85
		public void Awake()
		{
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.SleepStart));
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x000F2CA2 File Offset: 0x000F0EA2
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.SleepStart));
			}
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x000F2CC8 File Offset: 0x000F0EC8
		private void SleepStart()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (UnityEngine.Random.value > this.RemovalChance)
			{
				return;
			}
			TrashItem[] trash = this.GetTrash();
			for (int i = 0; i < trash.Length; i++)
			{
				trash[i].DestroyTrash();
			}
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x000F2D08 File Offset: 0x000F0F08
		private TrashItem[] GetTrash()
		{
			List<TrashItem> list = new List<TrashItem>();
			Vector3 center = this.Collider.transform.TransformPoint(this.Collider.center);
			Vector3 halfExtents = Vector3.Scale(this.Collider.size, this.Collider.transform.lossyScale) * 0.5f;
			Collider[] array = Physics.OverlapBox(center, halfExtents, this.Collider.transform.rotation, 1 << LayerMask.NameToLayer("Trash"), QueryTriggerInteraction.Collide);
			for (int i = 0; i < array.Length; i++)
			{
				TrashItem componentInParent = array[i].GetComponentInParent<TrashItem>();
				if (componentInParent != null)
				{
					list.Add(componentInParent);
				}
			}
			return list.ToArray();
		}

		// Token: 0x04002968 RID: 10600
		public BoxCollider Collider;

		// Token: 0x04002969 RID: 10601
		public float RemovalChance = 1f;
	}
}
