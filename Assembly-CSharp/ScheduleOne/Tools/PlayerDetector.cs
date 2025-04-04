using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x02000852 RID: 2130
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerDetector : MonoBehaviour
	{
		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06003A1F RID: 14879 RVA: 0x000F4AD1 File Offset: 0x000F2CD1
		// (set) Token: 0x06003A20 RID: 14880 RVA: 0x000F4AD9 File Offset: 0x000F2CD9
		public bool IgnoreNewDetections { get; protected set; }

		// Token: 0x06003A21 RID: 14881 RVA: 0x000F4AE4 File Offset: 0x000F2CE4
		private void Awake()
		{
			Rigidbody rigidbody = base.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = base.gameObject.AddComponent<Rigidbody>();
			}
			rigidbody.isKinematic = true;
			this.detectionColliders = base.GetComponentsInChildren<Collider>();
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x000F4B20 File Offset: 0x000F2D20
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onTick = (Action)Delegate.Combine(instance.onTick, new Action(this.MinPass));
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x000F4B48 File Offset: 0x000F2D48
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onTick = (Action)Delegate.Remove(instance.onTick, new Action(this.MinPass));
			}
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x000F4B78 File Offset: 0x000F2D78
		private void MinPass()
		{
			bool flag = false;
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if (Vector3.SqrMagnitude(Player.PlayerList[i].Avatar.CenterPoint - base.transform.position) < 400f)
				{
					flag = true;
					break;
				}
			}
			if (flag != this.collidersEnabled)
			{
				this.collidersEnabled = flag;
				for (int j = 0; j < this.detectionColliders.Length; j++)
				{
					this.detectionColliders[j].enabled = this.collidersEnabled;
				}
			}
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x000F4C08 File Offset: 0x000F2E08
		private void OnTriggerEnter(Collider other)
		{
			if (this.IgnoreNewDetections)
			{
				return;
			}
			Player componentInParent = other.GetComponentInParent<Player>();
			if (componentInParent != null && !this.DetectedPlayers.Contains(componentInParent) && other == componentInParent.CapCol)
			{
				this.DetectedPlayers.Add(componentInParent);
				if (this.onPlayerEnter != null)
				{
					this.onPlayerEnter.Invoke(componentInParent);
				}
				if (componentInParent.IsOwner && this.onLocalPlayerEnter != null)
				{
					this.onLocalPlayerEnter.Invoke();
				}
			}
			if (this.DetectPlayerInVehicle)
			{
				LandVehicle componentInParent2 = other.GetComponentInParent<LandVehicle>();
				if (componentInParent2 != null)
				{
					foreach (Player player in componentInParent2.OccupantPlayers)
					{
						if (player != null && !this.DetectedPlayers.Contains(player))
						{
							this.DetectedPlayers.Add(player);
							if (this.onPlayerEnter != null)
							{
								this.onPlayerEnter.Invoke(player);
							}
							if (player.IsOwner && this.onLocalPlayerEnter != null)
							{
								this.onLocalPlayerEnter.Invoke();
							}
						}
					}
				}
			}
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x000F4D34 File Offset: 0x000F2F34
		private void FixedUpdate()
		{
			for (int i = 0; i < this.DetectedPlayers.Count; i++)
			{
				if (this.DetectedPlayers[i].CurrentVehicle != null)
				{
					this.OnTriggerExit(this.DetectedPlayers[i].CapCol);
				}
			}
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x000F4D88 File Offset: 0x000F2F88
		private void OnTriggerExit(Collider other)
		{
			if (this.ignoreExit)
			{
				return;
			}
			Player componentInParent = other.GetComponentInParent<Player>();
			if (componentInParent != null && this.DetectedPlayers.Contains(componentInParent) && other == componentInParent.CapCol)
			{
				this.DetectedPlayers.Remove(componentInParent);
				if (this.onPlayerExit != null)
				{
					this.onPlayerExit.Invoke(componentInParent);
				}
				if (componentInParent.IsOwner && this.onLocalPlayerExit != null)
				{
					this.onLocalPlayerExit.Invoke();
				}
			}
			if (this.DetectPlayerInVehicle)
			{
				LandVehicle componentInParent2 = other.GetComponentInParent<LandVehicle>();
				if (componentInParent2 != null)
				{
					foreach (Player player in componentInParent2.OccupantPlayers)
					{
						if (player != null && this.DetectedPlayers.Contains(player))
						{
							this.DetectedPlayers.Remove(player);
							if (this.onPlayerExit != null)
							{
								this.onPlayerExit.Invoke(player);
							}
							if (player.IsOwner && this.onLocalPlayerExit != null)
							{
								this.onLocalPlayerExit.Invoke();
							}
						}
					}
				}
			}
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x000F4EB8 File Offset: 0x000F30B8
		public void SetIgnoreNewCollisions(bool ignore)
		{
			this.IgnoreNewDetections = ignore;
			if (!ignore)
			{
				this.ignoreExit = true;
				Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].isTrigger)
					{
						componentsInChildren[i].enabled = false;
						componentsInChildren[i].enabled = true;
					}
				}
				this.ignoreExit = false;
			}
		}

		// Token: 0x040029F0 RID: 10736
		public const float ACTIVATION_DISTANCE_SQ = 400f;

		// Token: 0x040029F1 RID: 10737
		public bool DetectPlayerInVehicle;

		// Token: 0x040029F2 RID: 10738
		public UnityEvent<Player> onPlayerEnter;

		// Token: 0x040029F3 RID: 10739
		public UnityEvent<Player> onPlayerExit;

		// Token: 0x040029F4 RID: 10740
		public UnityEvent onLocalPlayerEnter;

		// Token: 0x040029F5 RID: 10741
		public UnityEvent onLocalPlayerExit;

		// Token: 0x040029F6 RID: 10742
		public List<Player> DetectedPlayers = new List<Player>();

		// Token: 0x040029F8 RID: 10744
		private bool ignoreExit;

		// Token: 0x040029F9 RID: 10745
		private bool collidersEnabled = true;

		// Token: 0x040029FA RID: 10746
		private Collider[] detectionColliders;
	}
}
