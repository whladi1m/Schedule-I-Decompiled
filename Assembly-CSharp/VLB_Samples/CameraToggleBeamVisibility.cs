using System;
using UnityEngine;
using VLB;

namespace VLB_Samples
{
	// Token: 0x0200015B RID: 347
	[RequireComponent(typeof(Camera))]
	public class CameraToggleBeamVisibility : MonoBehaviour
	{
		// Token: 0x060006B8 RID: 1720 RVA: 0x0001E288 File Offset: 0x0001C488
		private void Update()
		{
			if (Input.GetKeyDown(this.m_KeyCode))
			{
				Camera component = base.GetComponent<Camera>();
				int geometryLayerID = Config.Instance.geometryLayerID;
				int num = 1 << geometryLayerID;
				if ((component.cullingMask & num) == num)
				{
					component.cullingMask &= ~num;
					return;
				}
				component.cullingMask |= num;
			}
		}

		// Token: 0x04000775 RID: 1909
		[SerializeField]
		private KeyCode m_KeyCode = KeyCode.Space;
	}
}
