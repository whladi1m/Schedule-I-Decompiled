using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property.Utilities.Power;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000BDD RID: 3037
	public class UtilityPole : MonoBehaviour
	{
		// Token: 0x06005536 RID: 21814 RVA: 0x001666F4 File Offset: 0x001648F4
		private void Awake()
		{
			if (this.Cable1Container.gameObject.activeSelf)
			{
				this.cableStart = this.cable1Connection.position;
				this.cableEnd = this.cable1Segments[this.cable1Segments.Count - 1].position;
				this.cableMid = (this.cableStart + this.cableEnd) / 2f;
				return;
			}
			this.cableStart = this.cable2Connection.position;
			this.cableEnd = this.cable2Segments[this.cable2Segments.Count - 1].position;
			this.cableMid = (this.cableStart + this.cableEnd) / 2f;
		}

		// Token: 0x06005537 RID: 21815 RVA: 0x001667BE File Offset: 0x001649BE
		private void Start()
		{
			if (PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				this.<Start>g__Register|17_0();
				return;
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.<Start>g__Register|17_0));
		}

		// Token: 0x06005538 RID: 21816 RVA: 0x001667F0 File Offset: 0x001649F0
		private void UpdateCulling()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			float sqrMagnitude = (this.cableStart - PlayerSingleton<PlayerCamera>.Instance.transform.position).sqrMagnitude;
			float sqrMagnitude2 = (this.cableEnd - PlayerSingleton<PlayerCamera>.Instance.transform.position).sqrMagnitude;
			float sqrMagnitude3 = (this.cableMid - PlayerSingleton<PlayerCamera>.Instance.transform.position).sqrMagnitude;
			float num = Mathf.Min(new float[]
			{
				sqrMagnitude,
				sqrMagnitude2,
				sqrMagnitude3
			}) * QualitySettings.lodBias;
			this.Cable1Container.gameObject.SetActive(num < 10000f && this.Connection1Enabled);
			this.Cable2Container.gameObject.SetActive(num < 10000f && this.Connection2Enabled);
		}

		// Token: 0x06005539 RID: 21817 RVA: 0x001668D4 File Offset: 0x00164AD4
		[Button]
		public void Orient()
		{
			if (this.previousPole == null && this.nextPole == null)
			{
				Console.LogWarning("No neighbour poles!", null);
				return;
			}
			if (this.nextPole != null && this.previousPole != null)
			{
				Vector3 normalized = (base.transform.position - this.previousPole.transform.position).normalized;
				Vector3 normalized2 = (this.nextPole.transform.position - base.transform.position).normalized;
				Vector3 normalized3 = (normalized + normalized2).normalized;
				base.transform.rotation = Quaternion.LookRotation(normalized3, Vector3.up);
				return;
			}
			if (this.previousPole != null)
			{
				Vector3 normalized4 = (base.transform.position - this.previousPole.transform.position).normalized;
				base.transform.rotation = Quaternion.LookRotation(normalized4, Vector3.up);
				return;
			}
			if (this.nextPole != null)
			{
				Vector3 normalized5 = (this.nextPole.transform.position - base.transform.position).normalized;
				base.transform.rotation = Quaternion.LookRotation(normalized5, Vector3.up);
			}
		}

		// Token: 0x0600553A RID: 21818 RVA: 0x00166A3C File Offset: 0x00164C3C
		[Button]
		public void DrawLines()
		{
			if (this.previousPole == null)
			{
				if (this.Connection1Enabled)
				{
					foreach (Transform transform in this.cable1Segments)
					{
						transform.gameObject.SetActive(false);
					}
				}
				if (this.Connection2Enabled)
				{
					foreach (Transform transform2 in this.cable2Segments)
					{
						transform2.gameObject.SetActive(false);
					}
				}
				return;
			}
			if (this.Connection1Enabled)
			{
				PowerLine.DrawPowerLine(this.cable1Connection.position, this.previousPole.cable1Connection.position, this.cable1Segments, this.LengthFactor);
				foreach (Transform transform3 in this.cable1Segments)
				{
					transform3.gameObject.SetActive(true);
				}
			}
			if (this.Connection2Enabled)
			{
				PowerLine.DrawPowerLine(this.cable2Connection.position, this.previousPole.cable2Connection.position, this.cable2Segments, this.LengthFactor);
				foreach (Transform transform4 in this.cable2Segments)
				{
					transform4.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x0600553C RID: 21820 RVA: 0x00166C4F File Offset: 0x00164E4F
		[CompilerGenerated]
		private void <Start>g__Register|17_0()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.<Start>g__Register|17_0));
			PlayerSingleton<PlayerCamera>.Instance.RegisterMovementEvent(2, new Action(this.UpdateCulling));
		}

		// Token: 0x04003F2A RID: 16170
		public const float CABLE_CULL_DISTANCE = 100f;

		// Token: 0x04003F2B RID: 16171
		public const float CABLE_CULL_DISTANCE_SQR = 10000f;

		// Token: 0x04003F2C RID: 16172
		public UtilityPole previousPole;

		// Token: 0x04003F2D RID: 16173
		public UtilityPole nextPole;

		// Token: 0x04003F2E RID: 16174
		public bool Connection1Enabled = true;

		// Token: 0x04003F2F RID: 16175
		public bool Connection2Enabled = true;

		// Token: 0x04003F30 RID: 16176
		public float LengthFactor = 1.002f;

		// Token: 0x04003F31 RID: 16177
		[Header("References")]
		public Transform cable1Connection;

		// Token: 0x04003F32 RID: 16178
		public Transform cable2Connection;

		// Token: 0x04003F33 RID: 16179
		public List<Transform> cable1Segments = new List<Transform>();

		// Token: 0x04003F34 RID: 16180
		public List<Transform> cable2Segments = new List<Transform>();

		// Token: 0x04003F35 RID: 16181
		public Transform Cable1Container;

		// Token: 0x04003F36 RID: 16182
		public Transform Cable2Container;

		// Token: 0x04003F37 RID: 16183
		private Vector3 cableStart = Vector3.zero;

		// Token: 0x04003F38 RID: 16184
		private Vector3 cableEnd = Vector3.zero;

		// Token: 0x04003F39 RID: 16185
		private Vector3 cableMid = Vector3.zero;
	}
}
