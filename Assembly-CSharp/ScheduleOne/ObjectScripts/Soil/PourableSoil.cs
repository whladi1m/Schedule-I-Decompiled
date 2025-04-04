using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts.Soil
{
	// Token: 0x02000BD0 RID: 3024
	public class PourableSoil : Pourable
	{
		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x060054E9 RID: 21737 RVA: 0x00165532 File Offset: 0x00163732
		// (set) Token: 0x060054EA RID: 21738 RVA: 0x0016553A File Offset: 0x0016373A
		public int currentCut { get; protected set; }

		// Token: 0x060054EB RID: 21739 RVA: 0x00165543 File Offset: 0x00163743
		protected override void Awake()
		{
			base.Awake();
			this.highlightScale = this.Highlights[0].transform.localScale;
			this.UpdateHighlights();
			this.ClickableEnabled = false;
		}

		// Token: 0x060054EC RID: 21740 RVA: 0x00165570 File Offset: 0x00163770
		protected override void Update()
		{
			base.Update();
			this.timeSinceStart += Time.deltaTime;
			this.UpdateHighlights();
		}

		// Token: 0x060054ED RID: 21741 RVA: 0x00165590 File Offset: 0x00163790
		private void UpdateHighlights()
		{
			if (this.Highlights[0] == null)
			{
				return;
			}
			for (int i = 0; i < this.Highlights.Length; i++)
			{
				if (this.IsOpen || i < this.currentCut)
				{
					this.Highlights[i].gameObject.SetActive(false);
				}
				else
				{
					float num = (float)i / (float)this.Highlights.Length;
					float num2 = Mathf.Sin(Mathf.Clamp(this.timeSinceStart * 5f - num, 0f, float.MaxValue)) + 1f;
					this.Highlights[i].transform.localScale = new Vector3(this.highlightScale.x * num2, this.highlightScale.y, this.highlightScale.z * num2);
				}
			}
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x00165660 File Offset: 0x00163860
		protected override void PourAmount(float amount)
		{
			base.PourAmount(amount);
			this.SoilBag.localScale = new Vector3(1f, Mathf.Lerp(0.45f, 1f, this.currentQuantity / this.StartQuantity), 1f);
			if (base.IsPourPointOverPot())
			{
				if (this.TargetPot.SoilID != this.SoilDefinition.ID)
				{
					this.TargetPot.SetSoilID(this.SoilDefinition.ID);
					this.TargetPot.SetSoilUses(this.SoilDefinition.Uses);
				}
				this.TargetPot.SetSoilState(Pot.ESoilState.Flat);
				this.TargetPot.AddSoil(amount);
			}
			if (this.TargetPot.SoilLevel >= this.TargetPot.SoilCapacity)
			{
				Singleton<TaskManager>.Instance.currentTask.Success();
			}
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x0016573A File Offset: 0x0016393A
		protected override bool CanPour()
		{
			return this.IsOpen && base.CanPour();
		}

		// Token: 0x060054F0 RID: 21744 RVA: 0x0016574C File Offset: 0x0016394C
		public void Cut()
		{
			this.TopColliders[this.currentCut].enabled = false;
			this.LerpCut(this.currentCut);
			if (this.currentCut == this.Bones.Length - 1)
			{
				this.FinishCut();
			}
			this.SnipSound.AudioSource.pitch = 0.9f + (float)this.currentCut * 0.05f;
			this.SnipSound.PlayOneShot(false);
			int currentCut = this.currentCut;
			this.currentCut = currentCut + 1;
		}

		// Token: 0x060054F1 RID: 21745 RVA: 0x001657D4 File Offset: 0x001639D4
		private void FinishCut()
		{
			this.IsOpen = true;
			Rigidbody rigidbody = this.TopParent.gameObject.AddComponent<Rigidbody>();
			this.TopParent.transform.SetParent(null);
			rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
			rigidbody.AddRelativeForce(Vector3.forward * 1.5f, ForceMode.VelocityChange);
			rigidbody.AddRelativeForce(Vector3.up * 0.3f, ForceMode.VelocityChange);
			rigidbody.AddTorque(Vector3.up * 1.5f, ForceMode.VelocityChange);
			this.ClickableEnabled = true;
			if (this.onOpened != null)
			{
				this.onOpened.Invoke();
			}
			UnityEngine.Object.Destroy(this.TopParent.gameObject, 3f);
			UnityEngine.Object.Destroy(this.TopMesh.gameObject, 3f);
		}

		// Token: 0x060054F2 RID: 21746 RVA: 0x00165898 File Offset: 0x00163A98
		private void LerpCut(int cutIndex)
		{
			PourableSoil.<>c__DisplayClass25_0 CS$<>8__locals1 = new PourableSoil.<>c__DisplayClass25_0();
			CS$<>8__locals1.bone = this.Bones[cutIndex];
			CS$<>8__locals1.startRot = CS$<>8__locals1.bone.localRotation;
			CS$<>8__locals1.endRot = CS$<>8__locals1.bone.localRotation * Quaternion.Euler(0f, 0f, 10f);
			base.StartCoroutine(CS$<>8__locals1.<LerpCut>g__Routine|0());
		}

		// Token: 0x04003EE5 RID: 16101
		public const float TEAR_ANGLE = 10f;

		// Token: 0x04003EE6 RID: 16102
		public const float HIGHLIGHT_CYCLE_TIME = 5f;

		// Token: 0x04003EE7 RID: 16103
		public bool IsOpen;

		// Token: 0x04003EE8 RID: 16104
		public SoilDefinition SoilDefinition;

		// Token: 0x04003EE9 RID: 16105
		[Header("References")]
		public Transform SoilBag;

		// Token: 0x04003EEA RID: 16106
		public Transform[] Bones;

		// Token: 0x04003EEB RID: 16107
		public List<Collider> TopColliders;

		// Token: 0x04003EEC RID: 16108
		public MeshRenderer[] Highlights;

		// Token: 0x04003EED RID: 16109
		public Transform TopParent;

		// Token: 0x04003EEE RID: 16110
		public AudioSourceController SnipSound;

		// Token: 0x04003EEF RID: 16111
		public SkinnedMeshRenderer TopMesh;

		// Token: 0x04003EF1 RID: 16113
		public UnityEvent onOpened;

		// Token: 0x04003EF2 RID: 16114
		private Vector3 highlightScale = Vector3.zero;

		// Token: 0x04003EF3 RID: 16115
		private float timeSinceStart;
	}
}
