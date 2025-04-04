using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x0200093D RID: 2365
	public class Accessory : MonoBehaviour
	{
		// Token: 0x0600402C RID: 16428 RVA: 0x0010D614 File Offset: 0x0010B814
		private void Awake()
		{
			for (int i = 0; i < this.skinnedMeshesToBind.Length; i++)
			{
				this.skinnedMeshesToBind[i].updateWhenOffscreen = true;
			}
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x0010D644 File Offset: 0x0010B844
		public void ApplyColor(Color col)
		{
			foreach (MeshRenderer meshRenderer in this.meshesToColor)
			{
				for (int j = 0; j < meshRenderer.materials.Length; j++)
				{
					meshRenderer.materials[j].color = col;
					if (!this.ColorAllMeshes)
					{
						break;
					}
				}
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.skinnedMeshesToColor)
			{
				for (int k = 0; k < skinnedMeshRenderer.materials.Length; k++)
				{
					skinnedMeshRenderer.materials[k].color = col;
					if (!this.ColorAllMeshes)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x0010D6E0 File Offset: 0x0010B8E0
		public void ApplyShapeKeys(float gender, float weight)
		{
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.shapeKeyMeshRends)
			{
				if (skinnedMeshRenderer.sharedMesh.blendShapeCount >= 2)
				{
					skinnedMeshRenderer.SetBlendShapeWeight(0, gender);
					skinnedMeshRenderer.SetBlendShapeWeight(1, weight);
				}
			}
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x0010D724 File Offset: 0x0010B924
		public void BindBones(Transform[] bones)
		{
			SkinnedMeshRenderer[] array = this.skinnedMeshesToBind;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].bones = bones;
			}
		}

		// Token: 0x04002E19 RID: 11801
		[Header("Settings")]
		public string Name;

		// Token: 0x04002E1A RID: 11802
		public string AssetPath;

		// Token: 0x04002E1B RID: 11803
		public bool ReduceFootSize;

		// Token: 0x04002E1C RID: 11804
		[Range(0f, 1f)]
		public float FootSizeReduction = 1f;

		// Token: 0x04002E1D RID: 11805
		public bool ShouldBlockHair;

		// Token: 0x04002E1E RID: 11806
		public bool ColorAllMeshes = true;

		// Token: 0x04002E1F RID: 11807
		[Header("References")]
		public MeshRenderer[] meshesToColor;

		// Token: 0x04002E20 RID: 11808
		public SkinnedMeshRenderer[] skinnedMeshesToColor;

		// Token: 0x04002E21 RID: 11809
		public SkinnedMeshRenderer[] skinnedMeshesToBind;

		// Token: 0x04002E22 RID: 11810
		public SkinnedMeshRenderer[] shapeKeyMeshRends;
	}
}
