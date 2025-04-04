using System;
using UnityEngine;
using UnityEngine.UI;

namespace VolumetricFogAndMist2.Demos
{
	// Token: 0x02000162 RID: 354
	public class DemoSceneControls : MonoBehaviour
	{
		// Token: 0x060006CC RID: 1740 RVA: 0x0001EB25 File Offset: 0x0001CD25
		private void Start()
		{
			this.SetProfile(this.index);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001EB34 File Offset: 0x0001CD34
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				this.index++;
				if (this.index >= this.profiles.Length)
				{
					this.index = 0;
				}
				this.SetProfile(this.index);
			}
			if (Input.GetKeyDown(KeyCode.T))
			{
				this.fogVolume.gameObject.SetActive(!this.fogVolume.gameObject.activeSelf);
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001EBA8 File Offset: 0x0001CDA8
		private void SetProfile(int profileIndex)
		{
			if (profileIndex < 2)
			{
				this.fogVolume.transform.position = Vector3.up * 25f;
			}
			else
			{
				this.fogVolume.transform.position = Vector3.zero;
			}
			this.fogVolume.profile = this.profiles[profileIndex];
			this.presetNameDisplay.text = "Current fog preset: " + this.profiles[profileIndex].name;
			this.fogVolume.UpdateMaterialPropertiesNow(false, false);
		}

		// Token: 0x0400078E RID: 1934
		public VolumetricFogProfile[] profiles;

		// Token: 0x0400078F RID: 1935
		public VolumetricFog fogVolume;

		// Token: 0x04000790 RID: 1936
		public Text presetNameDisplay;

		// Token: 0x04000791 RID: 1937
		private int index;
	}
}
