using System;
using ScheduleOne.NPCs;
using ScheduleOne.Police;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x02000833 RID: 2099
	[RequireComponent(typeof(Rigidbody))]
	public class CombatNPCDetector : MonoBehaviour
	{
		// Token: 0x060039AC RID: 14764 RVA: 0x000F3D20 File Offset: 0x000F1F20
		private void Awake()
		{
			Rigidbody rigidbody = base.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = base.gameObject.AddComponent<Rigidbody>();
			}
			rigidbody.isKinematic = true;
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x000F3D50 File Offset: 0x000F1F50
		private void FixedUpdate()
		{
			if (this.timeSinceLastContact < 0.1f)
			{
				this.contactTime += Time.fixedDeltaTime;
				if (this.contactTime >= this.ContactTimeForDetection)
				{
					this.contactTime = 0f;
					if (this.onDetected != null)
					{
						this.onDetected.Invoke();
					}
				}
			}
			else
			{
				this.contactTime = 0f;
			}
			this.timeSinceLastContact += Time.fixedDeltaTime;
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x000F3DC8 File Offset: 0x000F1FC8
		private void OnTriggerStay(Collider other)
		{
			NPC componentInParent = other.GetComponentInParent<NPC>();
			if (componentInParent != null && (!this.DetectOnlyInCombat || componentInParent.behaviour.CombatBehaviour.Active))
			{
				this.timeSinceLastContact = 0f;
				return;
			}
			PoliceOfficer policeOfficer = componentInParent as PoliceOfficer;
			if (policeOfficer != null && (!this.DetectOnlyInCombat || policeOfficer.PursuitBehaviour.Active))
			{
				this.timeSinceLastContact = 0f;
				return;
			}
		}

		// Token: 0x040029A6 RID: 10662
		public bool DetectOnlyInCombat;

		// Token: 0x040029A7 RID: 10663
		public UnityEvent onDetected;

		// Token: 0x040029A8 RID: 10664
		public float ContactTimeForDetection = 0.5f;

		// Token: 0x040029A9 RID: 10665
		private float contactTime;

		// Token: 0x040029AA RID: 10666
		private float timeSinceLastContact = 100f;
	}
}
