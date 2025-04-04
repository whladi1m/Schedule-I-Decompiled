using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001DB RID: 475
	public class LightningSpawnArea : MonoBehaviour
	{
		// Token: 0x06000A92 RID: 2706 RVA: 0x0002EC6C File Offset: 0x0002CE6C
		public void OnDrawGizmosSelected()
		{
			Vector3 localScale = base.transform.localScale;
			Gizmos.color = Color.yellow;
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, this.lightningArea);
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0002ECCA File Offset: 0x0002CECA
		private void OnEnable()
		{
			LightningRenderer.AddSpawnArea(this);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0002ECD2 File Offset: 0x0002CED2
		private void OnDisable()
		{
			LightningRenderer.RemoveSpawnArea(this);
		}

		// Token: 0x04000B78 RID: 2936
		[Tooltip("Dimensions of the lightning area where lightning bolts will be spawned inside randomly.")]
		public Vector3 lightningArea = new Vector3(40f, 20f, 20f);
	}
}
