using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Impostors
{
	// Token: 0x02000957 RID: 2391
	public class AvatarImpostor : MonoBehaviour
	{
		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600412A RID: 16682 RVA: 0x00111924 File Offset: 0x0010FB24
		// (set) Token: 0x0600412B RID: 16683 RVA: 0x0011192C File Offset: 0x0010FB2C
		public bool HasTexture { get; private set; }

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x0600412C RID: 16684 RVA: 0x00111935 File Offset: 0x0010FB35
		private Transform Camera
		{
			get
			{
				if (this.cachedCamera == null)
				{
					PlayerCamera instance = PlayerSingleton<PlayerCamera>.Instance;
					this.cachedCamera = ((instance != null) ? instance.transform : null);
				}
				return this.cachedCamera;
			}
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x00111964 File Offset: 0x0010FB64
		public void SetAvatarSettings(AvatarSettings settings)
		{
			Texture2D impostorTexture = settings.ImpostorTexture;
			if (impostorTexture != null)
			{
				this.meshRenderer.material.mainTexture = impostorTexture;
				this.HasTexture = true;
			}
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x00111999 File Offset: 0x0010FB99
		private void LateUpdate()
		{
			this.Realign();
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x001119A4 File Offset: 0x0010FBA4
		private void Realign()
		{
			if (this.Camera != null)
			{
				Vector3 position = this.Camera.position;
				position.y = base.transform.position.y;
				Vector3 forward = base.transform.position - position;
				base.transform.rotation = Quaternion.LookRotation(forward);
			}
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x00111A05 File Offset: 0x0010FC05
		public void EnableImpostor()
		{
			base.gameObject.SetActive(true);
			this.Realign();
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x000BEE78 File Offset: 0x000BD078
		public void DisableImpostor()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04002F06 RID: 12038
		public MeshRenderer meshRenderer;

		// Token: 0x04002F07 RID: 12039
		private Transform cachedCamera;
	}
}
