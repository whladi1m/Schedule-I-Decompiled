using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB_Samples
{
	// Token: 0x02000160 RID: 352
	public class Rotater : MonoBehaviour
	{
		// Token: 0x060006C8 RID: 1736 RVA: 0x0001E968 File Offset: 0x0001CB68
		private void Update()
		{
			Vector3 vector = base.transform.rotation.eulerAngles;
			vector += this.EulerSpeed * Time.deltaTime;
			base.transform.rotation = Quaternion.Euler(vector);
		}

		// Token: 0x04000787 RID: 1927
		[FormerlySerializedAs("m_EulerSpeed")]
		public Vector3 EulerSpeed = Vector3.zero;
	}
}
