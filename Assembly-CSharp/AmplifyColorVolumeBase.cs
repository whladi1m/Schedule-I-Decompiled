using System;
using AmplifyColor;
using UnityEngine;

// Token: 0x0200000A RID: 10
[ExecuteInEditMode]
[AddComponentMenu("")]
public class AmplifyColorVolumeBase : MonoBehaviour
{
	// Token: 0x06000042 RID: 66 RVA: 0x000040CC File Offset: 0x000022CC
	private void OnDrawGizmos()
	{
		if (this.ShowInSceneView)
		{
			BoxCollider component = base.GetComponent<BoxCollider>();
			BoxCollider2D component2 = base.GetComponent<BoxCollider2D>();
			if (component != null || component2 != null)
			{
				Vector3 center;
				Vector3 size;
				if (component != null)
				{
					center = component.center;
					size = component.size;
				}
				else
				{
					center = component2.offset;
					size = component2.size;
				}
				Gizmos.color = Color.green;
				Gizmos.matrix = base.transform.localToWorldMatrix;
				Gizmos.DrawWireCube(center, size);
			}
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00004154 File Offset: 0x00002354
	private void OnDrawGizmosSelected()
	{
		BoxCollider component = base.GetComponent<BoxCollider>();
		BoxCollider2D component2 = base.GetComponent<BoxCollider2D>();
		if (component != null || component2 != null)
		{
			Color green = Color.green;
			green.a = 0.2f;
			Gizmos.color = green;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Vector3 center;
			Vector3 size;
			if (component != null)
			{
				center = component.center;
				size = component.size;
			}
			else
			{
				center = component2.offset;
				size = component2.size;
			}
			Gizmos.DrawCube(center, size);
		}
	}

	// Token: 0x04000058 RID: 88
	public Texture2D LutTexture;

	// Token: 0x04000059 RID: 89
	public float Exposure = 1f;

	// Token: 0x0400005A RID: 90
	public float EnterBlendTime = 1f;

	// Token: 0x0400005B RID: 91
	public int Priority;

	// Token: 0x0400005C RID: 92
	public bool ShowInSceneView = true;

	// Token: 0x0400005D RID: 93
	[HideInInspector]
	public VolumeEffectContainer EffectContainer = new VolumeEffectContainer();
}
