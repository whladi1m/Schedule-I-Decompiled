using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x0200078C RID: 1932
	public class AudioZoneModifierVolume : MonoBehaviour
	{
		// Token: 0x0600349F RID: 13471 RVA: 0x000DD496 File Offset: 0x000DB696
		private void Start()
		{
			base.InvokeRepeating("Refresh", 0f, 0.25f);
			this.colliders = base.GetComponentsInChildren<BoxCollider>();
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Invisible"));
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000DD4D0 File Offset: 0x000DB6D0
		private void Refresh()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			BoxCollider[] array = this.colliders;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].bounds.Contains(PlayerSingleton<PlayerCamera>.Instance.transform.position))
				{
					foreach (AudioZone audioZone in this.Zones)
					{
						audioZone.AddModifier(this, this.VolumeMultiplier);
					}
					return;
				}
			}
			foreach (AudioZone audioZone2 in this.Zones)
			{
				audioZone2.RemoveModifier(this);
			}
		}

		// Token: 0x040025E5 RID: 9701
		public List<AudioZone> Zones = new List<AudioZone>();

		// Token: 0x040025E6 RID: 9702
		public float VolumeMultiplier = 0.5f;

		// Token: 0x040025E7 RID: 9703
		private BoxCollider[] colliders;
	}
}
