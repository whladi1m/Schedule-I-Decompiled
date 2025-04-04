using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class NavMeshCleaner : MonoBehaviour
{
	// Token: 0x040000E7 RID: 231
	public List<Vector3> m_WalkablePoint = new List<Vector3>();

	// Token: 0x040000E8 RID: 232
	public float m_Height = 1f;

	// Token: 0x040000E9 RID: 233
	public float m_Offset;

	// Token: 0x040000EA RID: 234
	public int m_MidLayerCount = 3;
}
