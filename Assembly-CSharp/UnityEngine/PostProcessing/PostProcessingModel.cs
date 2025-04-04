using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D5 RID: 213
	[Serializable]
	public abstract class PostProcessingModel
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000372 RID: 882 RVA: 0x000140C7 File Offset: 0x000122C7
		// (set) Token: 0x06000373 RID: 883 RVA: 0x000140CF File Offset: 0x000122CF
		public bool enabled
		{
			get
			{
				return this.m_Enabled;
			}
			set
			{
				this.m_Enabled = value;
				if (value)
				{
					this.OnValidate();
				}
			}
		}

		// Token: 0x06000374 RID: 884
		public abstract void Reset();

		// Token: 0x06000375 RID: 885 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void OnValidate()
		{
		}

		// Token: 0x0400045A RID: 1114
		[SerializeField]
		[GetSet("enabled")]
		private bool m_Enabled;
	}
}
