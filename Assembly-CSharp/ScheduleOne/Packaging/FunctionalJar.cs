using System;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x02000873 RID: 2163
	public class FunctionalJar : FunctionalPackaging
	{
		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06003AA0 RID: 15008 RVA: 0x000F6819 File Offset: 0x000F4A19
		// (set) Token: 0x06003AA1 RID: 15009 RVA: 0x000F6821 File Offset: 0x000F4A21
		public override CursorManager.ECursorType HoveredCursor { get; protected set; } = CursorManager.ECursorType.Finger;

		// Token: 0x06003AA2 RID: 15010 RVA: 0x000F682C File Offset: 0x000F4A2C
		public override void Initialize(PackagingStation _station, Transform alignment, bool align = false)
		{
			base.Initialize(_station, alignment, align);
			this.lidPosition = base.transform.InverseTransformPoint(this.Lid.transform.position);
			this.LidObject = this.Lid.gameObject;
			this.Lid.transform.SetParent(_station.Container);
			this.Lid.transform.position = this.LidStartPoint.position;
			this.Lid.transform.rotation = this.LidStartPoint.rotation;
			this.LidSensor.enabled = false;
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x000F68CC File Offset: 0x000F4ACC
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.LidObject);
			base.Destroy();
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x000F68DF File Offset: 0x000F4ADF
		protected override void EnableSealing()
		{
			base.EnableSealing();
			this.Lid.enabled = true;
			this.Lid.ClickableEnabled = true;
			this.Lid.Rb.isKinematic = false;
			this.LidSensor.enabled = true;
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x000F691C File Offset: 0x000F4B1C
		protected override void LateUpdate()
		{
			base.LateUpdate();
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x000F6924 File Offset: 0x000F4B24
		protected override void OnTriggerStay(Collider other)
		{
			base.OnTriggerStay(other);
			if (this.Lid != null && this.Lid.enabled && other.gameObject.name == "LidTrigger")
			{
				this.Seal();
			}
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x000F6970 File Offset: 0x000F4B70
		public override void Seal()
		{
			base.Seal();
			this.Lid.enabled = false;
			this.Lid.ClickableEnabled = false;
			this.Lid.transform.SetParent(base.transform);
			UnityEngine.Object.Destroy(this.Lid.Rb);
			UnityEngine.Object.Destroy(this.Lid);
			UnityEngine.Object.Destroy(this.LidCollider);
			this.Lid.transform.position = base.transform.TransformPoint(this.lidPosition);
			this.LidSensor.enabled = false;
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x000F6A04 File Offset: 0x000F4C04
		protected override void FullyPacked()
		{
			base.FullyPacked();
			this.FullyPackedBlocker.SetActive(true);
		}

		// Token: 0x04002A75 RID: 10869
		[Header("References")]
		public Draggable Lid;

		// Token: 0x04002A76 RID: 10870
		public Transform LidStartPoint;

		// Token: 0x04002A77 RID: 10871
		public Collider LidSensor;

		// Token: 0x04002A78 RID: 10872
		public Collider LidCollider;

		// Token: 0x04002A79 RID: 10873
		public GameObject FullyPackedBlocker;

		// Token: 0x04002A7A RID: 10874
		private GameObject LidObject;

		// Token: 0x04002A7B RID: 10875
		private Vector3 lidPosition = Vector3.zero;
	}
}
