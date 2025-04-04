using System;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000BDE RID: 3038
	public class ToggleableLight : MonoBehaviour
	{
		// Token: 0x0600553D RID: 21821 RVA: 0x00166C88 File Offset: 0x00164E88
		protected virtual void Awake()
		{
			this.constructable = base.GetComponentInParent<Constructable_GridBased>();
			this.SetLights(this.isOn);
		}

		// Token: 0x0600553E RID: 21822 RVA: 0x00166CA2 File Offset: 0x00164EA2
		private void OnValidate()
		{
			if (this.isOn != this.lightsApplied)
			{
				this.SetLights(this.isOn);
			}
		}

		// Token: 0x0600553F RID: 21823 RVA: 0x00166CA2 File Offset: 0x00164EA2
		protected virtual void Update()
		{
			if (this.isOn != this.lightsApplied)
			{
				this.SetLights(this.isOn);
			}
		}

		// Token: 0x06005540 RID: 21824 RVA: 0x00166CBE File Offset: 0x00164EBE
		public void TurnOn()
		{
			this.isOn = true;
			this.Update();
		}

		// Token: 0x06005541 RID: 21825 RVA: 0x00166CCD File Offset: 0x00164ECD
		public void TurnOff()
		{
			this.isOn = false;
			this.Update();
		}

		// Token: 0x06005542 RID: 21826 RVA: 0x00166CDC File Offset: 0x00164EDC
		protected virtual void SetLights(bool active)
		{
			this.lightsApplied = this.isOn;
			foreach (OptimizedLight optimizedLight in this.lightSources)
			{
				if (!(optimizedLight == null))
				{
					optimizedLight.Enabled = active;
				}
			}
			Material material = active ? this.lightOnMat : this.lightOffMat;
			foreach (MeshRenderer meshRenderer in this.lightSurfacesMeshes)
			{
				if (!(meshRenderer == null))
				{
					Material[] sharedMaterials = meshRenderer.sharedMaterials;
					sharedMaterials[this.MaterialIndex] = material;
					meshRenderer.materials = sharedMaterials;
				}
			}
		}

		// Token: 0x04003F3A RID: 16186
		public bool isOn;

		// Token: 0x04003F3B RID: 16187
		[Header("References")]
		[SerializeField]
		protected OptimizedLight[] lightSources;

		// Token: 0x04003F3C RID: 16188
		[SerializeField]
		protected MeshRenderer[] lightSurfacesMeshes;

		// Token: 0x04003F3D RID: 16189
		public int MaterialIndex;

		// Token: 0x04003F3E RID: 16190
		[Header("Materials")]
		[SerializeField]
		protected Material lightOnMat;

		// Token: 0x04003F3F RID: 16191
		[SerializeField]
		protected Material lightOffMat;

		// Token: 0x04003F40 RID: 16192
		private Constructable_GridBased constructable;

		// Token: 0x04003F41 RID: 16193
		private bool lightsApplied;
	}
}
