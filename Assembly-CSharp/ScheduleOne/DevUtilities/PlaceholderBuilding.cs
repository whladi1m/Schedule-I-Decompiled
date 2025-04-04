using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006E1 RID: 1761
	[ExecuteInEditMode]
	public class PlaceholderBuilding : MonoBehaviour
	{
		// Token: 0x06002FEB RID: 12267 RVA: 0x000C7D0A File Offset: 0x000C5F0A
		private void Awake()
		{
			if (Application.isPlaying)
			{
				this.Model.GetComponent<Collider>().enabled = true;
			}
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x000C7D24 File Offset: 0x000C5F24
		protected virtual void LateUpdate()
		{
			if (Application.isPlaying)
			{
				return;
			}
			base.gameObject.name = "Placeholder (" + this.Name + ")";
			this.Label.text = this.Name;
			this.Model.localScale = this.Dimensions;
			if (base.transform.position != this.lastFramePosition)
			{
				RaycastHit raycastHit;
				if (this.AutoGround && Physics.Raycast(base.transform.position + Vector3.up * 50f, Vector3.down, out raycastHit, 100f, 1 << LayerMask.NameToLayer("Default")))
				{
					this.Model.transform.position = new Vector3(this.Model.transform.position.x, raycastHit.point.y + this.Dimensions.y / 2f, this.Model.transform.position.z);
				}
				this.lastFramePosition = base.transform.position;
			}
			this.Label.transform.position = new Vector3(this.Label.transform.position.x, this.Model.transform.position.y + this.Dimensions.y / 2f + 0.1f, this.Label.transform.position.z);
		}

		// Token: 0x04002235 RID: 8757
		[Header("Settings")]
		public string Name;

		// Token: 0x04002236 RID: 8758
		public Vector3 Dimensions;

		// Token: 0x04002237 RID: 8759
		public bool AutoGround = true;

		// Token: 0x04002238 RID: 8760
		[Header("References")]
		public Transform Model;

		// Token: 0x04002239 RID: 8761
		public TextMeshPro Label;

		// Token: 0x0400223A RID: 8762
		private Vector3 lastFramePosition = Vector3.zero;
	}
}
