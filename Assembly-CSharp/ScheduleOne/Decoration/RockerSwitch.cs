using System;
using UnityEngine;

namespace ScheduleOne.Decoration
{
	// Token: 0x02000BD7 RID: 3031
	public class RockerSwitch : MonoBehaviour
	{
		// Token: 0x0600550C RID: 21772 RVA: 0x00165E36 File Offset: 0x00164036
		private void Awake()
		{
			this.SetIsOn(this.isOn);
		}

		// Token: 0x0600550D RID: 21773 RVA: 0x00165E44 File Offset: 0x00164044
		public void SetIsOn(bool on)
		{
			this.isOn = on;
			this.Light.enabled = on;
			this.ButtonTransform.localEulerAngles = new Vector3(on ? 10f : -10f, 0f, 0f);
		}

		// Token: 0x04003F0C RID: 16140
		public MeshRenderer ButtonMesh;

		// Token: 0x04003F0D RID: 16141
		public Transform ButtonTransform;

		// Token: 0x04003F0E RID: 16142
		public Light Light;

		// Token: 0x04003F0F RID: 16143
		public bool isOn;
	}
}
